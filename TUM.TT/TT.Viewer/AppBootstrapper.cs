using System;
using System.Collections.Generic;
using Caliburn.Micro;
using TT.Viewer.ViewModels;
using System.Reflection;
using TT.Models.Serialization;
using TT.Lib.Managers;
using MahApps.Metro.Controls.Dialogs;
using TT.Lib;
using TT.Report.Generators;
using TT.Report.Renderers;

namespace TT.Viewer {

    public class AppBootstrapper : BootstrapperBase {
        SimpleContainer container;
        internal static string UserTto;

        public AppBootstrapper() {
            Initialize();
        }

        protected override void Configure() {
            container = new SimpleContainer();

            container.Singleton<IWindowManager, WindowManager>();
            container.Singleton<IEventAggregator, EventAggregator>();
            container.Singleton<IMatchSerializer, XmlMatchSerializer>();
            container.Singleton<IMatchManager, MatchManager>();
            container.Singleton<IReportGenerationQueueManager, ReportGenerationQueueManager>();
            container.Singleton<IShell, ShellViewModel>();
            container.Singleton<IDialogCoordinator, DialogCoordinator>();
            container.AllTypesOf<IResultViewTabItem>(Assembly.GetExecutingAssembly());
            // Report generation
            container.Singleton<IReportGenerator, DefaultReportGenerator>("default");
            container.Singleton<IReportGenerator, CustomizedReportGenerator>("customized");
            // Report rendering
            container.RegisterPerRequest(typeof(IReportRenderer),"PDF", typeof(PdfRenderer));
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

        protected override void OnStartup(object sender, System.Windows.StartupEventArgs e)
        {
            if (e.Args.Length > 0)
                UserTto = e.Args[0];
            DisplayRootViewFor<IShell>();
        }

        protected override IEnumerable<Assembly> SelectAssemblies()
        {
            return new[] {
                    Assembly.GetEntryAssembly(),
                    System.Reflection.Assembly.GetAssembly(typeof(TT.Lib.ViewModels.NavigationControlViewModel))
                };
        }
    }
}