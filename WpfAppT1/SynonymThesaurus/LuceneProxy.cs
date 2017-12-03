using System;
using System.Collections.Generic;
using System.Linq;
using Version = Lucene.Net.Util.Version;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.QueryParsers;
using Lucene.Net.Search;
using Lucene.Net.Store;
using System.IO;

namespace WpfAppT1.SynonymThesaurus
{
    public class LuceneProxy : IDisposable
    {
        public LuceneProxy(string luceneDir)
        {
            _luceneDir = luceneDir;
        }

        public IEnumerable<LuceneSynonymData> SearchDefault(string input, string fieldName = "")
        {
            return string.IsNullOrEmpty(input) ? 
              Enumerable.Empty<LuceneSynonymData>() : Search(input, fieldName);
        }

        public IEnumerable<LuceneSynonymData> GetAllIndexRecords()
        {
            // validate search index
            if (!System.IO.Directory.EnumerateFiles(_luceneDir).Any())
                yield break;
            // set up lucene searcher
            using (var searcher = new IndexSearcher(_directory, false))
            {
                using (var reader = IndexReader.Open(_directory, false))
                {
                    var docs = new List<Document>();
                    var term = reader.TermDocs();
                    // v 2.9.4: use 'hit.Doc()'
                    // v 3.0.3: use 'hit.Doc'
                    while (term.Next())
                        yield return MapToSynonymData(searcher.Doc(term.Doc));
                }
            }
        }

        public void AddUpdateLuceneIndex(IEnumerable<LuceneSynonymData> synonymDatas)
        {
            // init lucene
            using (var analyzer = new StandardAnalyzer(Version.LUCENE_30))
            {
                using (var writer = new IndexWriter(_directory, analyzer, IndexWriter.MaxFieldLength.UNLIMITED))
                {
                    // add data to lucene search index (replaces older entries if any)
                    foreach (var synonymData in synonymDatas)
                        AddToLuceneIndex(synonymData, writer);

                    // close handles
                    analyzer.Close();
                }
            }
        }

        private IEnumerable<LuceneSynonymData> Search(string searchQuery, string searchField = "")
        {
            // validation
            if (string.IsNullOrEmpty(searchQuery.Replace("*", "").Replace("?", "")))
                yield break;

            // set up lucene searcher
            using (var searcher = new IndexSearcher(_directory, false))
            {
                using (var analyzer = new StandardAnalyzer(Version.LUCENE_30))
                {

                    var queryParser = string.IsNullOrEmpty(searchField) ?
                        new MultiFieldQueryParser(Version.LUCENE_30, new[] { nameof(LuceneSynonymData.Id), nameof(LuceneSynonymData.Word) }, analyzer) :
                        new QueryParser(Version.LUCENE_30, searchField, analyzer);
                    var query = ParseQuery(searchQuery, queryParser);
                    var hits = searcher.Search(query, null, _hitsLimit, Sort.INDEXORDER).ScoreDocs;
                    foreach (var synonymData in MapToSynonymDataList(hits, searcher))
                        yield return synonymData;
                    analyzer.Close();
                }
            }
        }

        private static Query ParseQuery(string searchQuery, QueryParser parser)
        {
            Query query;
            try
            {
                query = parser.Parse(searchQuery.Trim());
            }
            catch (ParseException)
            {
                query = parser.Parse(QueryParser.Escape(searchQuery.Trim()));
            }
            return query;
        }

        private static void AddToLuceneIndex(LuceneSynonymData synonymData, IndexWriter writer)
        {
            // add new index entry
            var doc = new Document();

            // add lucene fields mapped to db fields
            doc.Add(new Field(nameof(LuceneSynonymData.Id), synonymData.Id.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED));
            doc.Add(new Field(nameof(LuceneSynonymData.Word), synonymData.Word, Field.Store.YES, Field.Index.ANALYZED));

            // add entry to index
            writer.AddDocument(doc);
        }

        private static LuceneSynonymData MapToSynonymData(Document doc)
        {
            return new LuceneSynonymData(Guid.Parse(doc.Get(nameof(LuceneSynonymData.Id))), 
                                                    doc.Get(nameof(LuceneSynonymData.Word)));
        }

        private static IEnumerable<LuceneSynonymData> MapToSynonymDataList(IEnumerable<ScoreDoc> hits, IndexSearcher searcher)
        {
            // v 2.9.4: use 'hit.doc'
            // v 3.0.3: use 'hit.Doc'
            return hits.Select(hit => MapToSynonymData(searcher.Doc(hit.Doc)));
        }

        private FSDirectory _directory
        {
            get
            {
                 _directoryTemp = _directoryTemp ?? 
                    FSDirectory.Open(new DirectoryInfo(_luceneDir));
                if (IndexWriter.IsLocked(_directoryTemp))
                    IndexWriter.Unlock(_directoryTemp);
                var lockFileFullPath = Path.Combine(_luceneDir, _lockFilePath);
                if (File.Exists(lockFileFullPath))
                    File.Delete(lockFileFullPath);
                return _directoryTemp;
            }
        }

        #region IDisposable
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        // Protected implementation of Dispose pattern.
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
                _directoryTemp?.Dispose();

            _disposed = true;
        }
        #endregion IDisposable

        private static readonly int _hitsLimit = 1000;
        private bool _disposed = false;
        private static readonly string _lockFilePath = "write.lock";
        private string _luceneDir;
        private FSDirectory _directoryTemp;
    }
}
