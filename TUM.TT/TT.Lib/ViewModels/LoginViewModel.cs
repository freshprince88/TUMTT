using Caliburn.Micro;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using TT.Lib.Managers;
using TT.Lib.Properties;
using TT.Lib.Events;
using TT.Models;
using TT.Models.Api;
using TT.Models.Util;
using System.Windows.Controls;

namespace TT.Lib.ViewModels
{
    public class LoginViewModel : Conductor<IScreen>.Collection.AllActive, IShell, INotifyPropertyChangedEx
    {
        public IEventAggregator events { get; private set; }
        public IMatchManager MatchManager { get; set; }
        public ICloudSyncManager CloudSyncManager;
        public IMatchLibraryManager MatchLibrary { get; private set; }
        private readonly IWindowManager _windowManager;
        private IDialogCoordinator DialogCoordinator;

        #region Calculated Properties
        private bool isLoginActive = false;
        public bool IsLoginActive
        {
            get
            {
                return isLoginActive;
            }
            set
            {
                isLoginActive = value;
                NotifyOfPropertyChange("IsLoginActive");
            }
        }
        
        private string message;
        public string Message
        {
            get
            {
                return message;
            }
            set
            {
                message = value;
                NotifyOfPropertyChange("Message");
            }
        }

        private string email;
        public string Email
        {
            get
            {
                return email;
            }
            set
            {
                email = value;
            }
        }
        #endregion


        public LoginViewModel(IWindowManager windowmanager, IEventAggregator eventAggregator, IMatchManager man, IDialogCoordinator coordinator, ICloudSyncManager cloudSyncManager, IMatchLibraryManager libraryManager)
        {
            this.DisplayName = "Login";
            this.events = eventAggregator;
            MatchManager = man;
            MatchLibrary = libraryManager;
            CloudSyncManager = cloudSyncManager;
            _windowManager = windowmanager;
            DialogCoordinator = coordinator;

            email = CloudSyncManager.GetAccountEmail();
        }

        #region Caliburn Hooks

        /// <summary>
        /// Initializes this view model.
        /// </summary>
        protected override void OnInitialize()
        {
            base.OnInitialize();
        }

        protected override void OnActivate()
        {

            base.OnActivate();
            // Subscribe ourself to the event bus
            this.events.Subscribe(this);

        }

        protected override void OnViewReady(object view)
        {
            base.OnViewReady(view);
        }

        protected override void OnDeactivate(bool close)
        {

            events.Unsubscribe(this);
        }

        #endregion
        public async void Login(string email, PasswordBox passwordBox)
        {
            IsLoginActive = true;
            Message = "";
            var password = passwordBox.Password;
            if (String.IsNullOrEmpty(email) || String.IsNullOrEmpty(password))
            {
                Message = "Please fill username and passowrd.";
                IsLoginActive = false;
                return;
            }

            CloudSyncManager.SetCredentials(email, password);
            await CloudSyncManager.Login();

            var status = CloudSyncManager.GetConnectionStatus();
            if(status == ConnectionStatus.Online)
            {
                this.TryClose();
                return;
            }
            IsLoginActive = false;
            Message = CloudSyncManager.GetConnectionMessage();
        }

        public void ResetPassword()
        {
            var frontendUrl = Settings.Default.CloudApiFrontend;
            var path = "/reset-password";
            System.Diagnostics.Process.Start(frontendUrl + path);
        }

        #region View Methods

        #endregion

    }
}
