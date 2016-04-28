using System;
using System.Collections.Generic;
using Caliburn.Micro;
using TT.Viewer.ViewModels;
using System.Reflection;
using TT.Models.Serialization;
using TT.Models.Managers;
using MahApps.Metro.Controls.Dialogs;
using TT.Models;

namespace TT.Viewer {

    public class AppBootstrapper : BootstrapperBase {
        SimpleContainer container;

        public AppBootstrapper() {
            Initialize();
        }

        protected override void Configure() {
            container = new SimpleContainer();

            container.Singleton<IWindowManager, WindowManager>();
            container.Singleton<IEventAggregator, EventAggregator>();
            container.Singleton<IMatchSerializer, XmlMatchSerializer>();
            container.Singleton<IMatchManager, MatchManager>();
            container.Singleton<IShell, ShellViewModel>();
            container.Singleton<IDialogCoordinator, DialogCoordinator>();
            container.AllTypesOf<IResultViewTabItem>(Assembly.GetExecutingAssembly());
        }

        protected override object GetInstance(Type service, string key) {
            var instance = container.GetInstance(service, key);
            if (instance != null)
                return instance;

            throw new InvalidOperationException("Could not locate any instances.");
        }

        protected override IEnumerable<object> GetAllInstances(Type service) {
            return container.GetAllInstances(service);
        }

        protected override void BuildUp(object instance) {
            container.BuildUp(instance);
        }

        protected override void OnStartup(object sender, System.Windows.StartupEventArgs e) {
            DisplayRootViewFor<IShell>();
        }
    }
}