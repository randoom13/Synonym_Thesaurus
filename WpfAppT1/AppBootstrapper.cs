using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfAppT1.ViewModels;

namespace WpfAppT1
{
    public class AppBootstrapper : BootstrapperBase
    {
        public AppBootstrapper()
        {
            Initialize();
        }

        protected override void OnStartup(object sender, System.Windows.StartupEventArgs e)
        {
            DisplayRootViewFor<MainViewModel>();
        }

        protected override void Configure()
        {
            base.Configure();
            _container.Singleton<IApplicationData, ApplicationData>();
            _container.Singleton<IWindowManager, WindowManager>();
            _container.PerRequest<MainViewModel>();
        }

        protected override object GetInstance(Type serviceType, string key)
        {
            var instance = _container.GetInstance(serviceType, key);
            if (instance == null)
                Debug.WriteLine("Could not resolve instance of {0}", serviceType);

            return instance;
        }

        protected override IEnumerable<object> GetAllInstances(Type serviceType)
        {
            return _container.GetAllInstances(serviceType);
        }

        protected override void BuildUp(object instance)
        {
            _container.BuildUp(instance);
        }

        private readonly SimpleContainer _container = new SimpleContainer();
    }
}
