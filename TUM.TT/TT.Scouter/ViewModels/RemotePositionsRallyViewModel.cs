using Caliburn.Micro;
using System.Collections.ObjectModel;
using TT.Lib.Events;
using TT.Models;
using System;

namespace TT.Scouter.ViewModels
{
    public class RemotePositionsRallyViewModel : Conductor<IScreen>.Collection.AllActive
    {
        private ObservableCollection<Schlag> _strokes;
        public ObservableCollection<Schlag> Strokes
        {
            get
            {
                return _strokes;
            }
            set
            {
                _strokes = value;
            }
        }

        private Schlag _stroke;
        private RemoteViewModel remoteViewModel;

        public string ToogleCalibrationButtonText { get; private set; }

        public Schlag CurrentStroke
        {
            get { return _stroke; }
            set
            {
                if (_stroke != value && value != null)
                {
                    _stroke = value;
                }
            }
        }

        public RemotePositionsRallyViewModel(ObservableCollection<Schlag> strokes, RemoteViewModel remoteViewModel)
        {
            this._strokes = strokes;
            this.remoteViewModel = remoteViewModel;
            ToogleCalibrationButtonText = "Hide Calibration";
        }

        protected override void OnActivate()
        {
            base.OnActivate();
        }

        public void CalibrateTable()
        {
            remoteViewModel.CalibrateTable();

            if (ToogleCalibrationButtonText.Equals("Show Calibration"))
                ToogleCalibrationButtonText = "Hide Calibration";
            NotifyOfPropertyChange("ToogleCalibrationButtonText");
        }

        public void ToogleCalibration()
        {
            remoteViewModel.ToogleCalibration();

            if (ToogleCalibrationButtonText.Equals("Hide Calibration"))
                ToogleCalibrationButtonText = "Show Calibration";
            else
                ToogleCalibrationButtonText = "Hide Calibration";
            NotifyOfPropertyChange("ToogleCalibrationButtonText");
        }
    }
}