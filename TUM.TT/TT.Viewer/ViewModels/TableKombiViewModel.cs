using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TT.Viewer.ViewModels
{
    public class TableKombiViewModel : Screen
    {
        private IEventAggregator events;

        #region Properties
        public bool ButtonsVisible { get; set; }
        private List<int> m_botBtnPos;
        private List<int> m_topBtnPos;

        public Match Game { get; set; }

        public List<int> TopButtonPositions
        {
            get
            {
                return m_topBtnPos;
            }
            set
            {
                m_topBtnPos = value;
                ChangeVisibleButtons();
            }
        }

        public List<int> BottomButtonPositions
        {
            get
            {
                return m_botBtnPos;
            }
            set
            {
                m_botBtnPos = value;
                ChangeVisibleButtons();
            }
        }
          

        
        #endregion

        public TableKombiViewModel(IEventAggregator eventAggregator)
        {
            events = eventAggregator;
            TopButtonPositions = new List<int>();
            BottomButtonPositions = new List<int>();
        }

        #region Helper Methods

        private void ChangeVisibleButtons()
        {
            if (TopButtonPositions != null)
            {
                foreach (var pos in TopButtonPositions)
                {
                    //TODO: Set Button on this position visisble
                }
            }
            if (BottomButtonPositions != null)
            {
                foreach (var pos in BottomButtonPositions)
                {
                    //TODO: Set Button on this position visisble
                }
            }
        }
        #endregion
    }
}
