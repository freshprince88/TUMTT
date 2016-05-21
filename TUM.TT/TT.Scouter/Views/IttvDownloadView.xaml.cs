using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TT.Lib.Events;

namespace TT.Scouter.Views
{
    /// <summary>
    /// Interaktionslogik für IttvDownloadView.xaml
    /// </summary>
    public partial class IttvDownloadView : MahApps.Metro.Controls.MetroWindow
    {
        public IEventAggregator Events { get; set; }
        string url { get; set; }
        string link { get; set; }
        public IttvDownloadView()
        {
            InitializeComponent();
            Events = IoC.Get<IEventAggregator>();
            Name = "ITTV";
            MyWebBrowser.DocumentCompleted += UrlChange;
            MyWebBrowser.Navigated += grabLink;
            Events.Subscribe(this);
        }

        public void DisposeBrowser()
        {
            MyWebBrowser.Dispose();
        }
        private void UrlChange(Object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            if (MyWebBrowser.ReadyState != WebBrowserReadyState.Complete)
            {
                url = e.Url.AbsoluteUri.ToString();


            }
        }
        private void grabLink(object sender, WebBrowserNavigatedEventArgs e)
        {

            String scheme = null;

            try
            {
                scheme = e.Url.Scheme;
                link = e.Url.AbsoluteUri;
                Events.PublishOnUIThread(new WebBrowserEvent(link));

            }
            catch
            {
            }
        }
    }


}
    

