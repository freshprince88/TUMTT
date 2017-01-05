using Caliburn.Micro;
using System.Collections.ObjectModel;
using TT.Models;

namespace TT.Report.ViewModels
{
    public class ServiceStatisticsGridViewModel : Screen
    {
        public MatchPlayer Player { get; set; }

        private ObservableCollection<Rally> selectedRallies;
        public ObservableCollection<Rally> SelectedRallies
        {
            get
            {
                return selectedRallies;
            }
            set
            {
                selectedRallies = value;
                NotifyOfPropertyChange();
            }
        }
        
        public ServiceStatisticsGridViewModel()
        {
            selectedRallies = new ObservableCollection<Rally>();
        }
    }
}
