using Caliburn.Micro;
using System.Collections.ObjectModel;
using TT.Models;

namespace TT.Report.ViewModels
{
    public class StatisticsGridViewModel : Screen
    {
        public MatchPlayer Player { get; set; }
        public int StrokeNumber { get; set; }

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
        
        public StatisticsGridViewModel()
        {
            selectedRallies = new ObservableCollection<Rally>();
        }
    }
}
