using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TT.Models;
using TT.Models.Serialization;

namespace TT.Lib.Managers
{
    public class ViewManager : IViewManager
    {
        public CombinationList Combinations { get; private set; }
        
        public IMatchManager MatchManager { get; private set; }

        public Playlist ActivePlaylist
        {
            get
            {
                return MatchManager.ActivePlaylist;
            }

            set
            {
                MatchManager.ActivePlaylist = value;
            }
        }

        public IEnumerable<Rally> SelectedRallies
        {
            get
            {
                return MatchManager.SelectedRallies;
            }

            set
            {
                MatchManager.SelectedRallies = value;
            }
        }

        public ViewManager(IMatchManager matchManager)
        {
            this.MatchManager = matchManager;

            Combinations = new CombinationList(LoadCombinations());
            Combinations.CollectionChanged += Combinations_CollectionChanged;
        }

        private void Combinations_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case System.Collections.Specialized.NotifyCollectionChangedAction.Add:
                    foreach (Combination item in e.NewItems)
                    {
                        SaveCombination(item);
                    }
                    break;
                case System.Collections.Specialized.NotifyCollectionChangedAction.Remove:
                    foreach (Combination item in e.OldItems)
                    {
                        DeleteCombination(item);
                    }
                    break;
                case System.Collections.Specialized.NotifyCollectionChangedAction.Replace:
                    foreach (Combination item in e.OldItems)
                    {
                        DeleteCombination(item);
                    }
                    foreach (Combination item in e.NewItems)
                    {
                        SaveCombination(item);
                    }
                    break;
                case System.Collections.Specialized.NotifyCollectionChangedAction.Reset:
                    foreach (Combination item in e.OldItems)
                    {
                        DeleteCombination(item);
                    }
                    break;
                case System.Collections.Specialized.NotifyCollectionChangedAction.Move:
                    break;
            }
        }

        private void Filters_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case System.Collections.Specialized.NotifyCollectionChangedAction.Add:
                    foreach (Filter item in e.NewItems)
                    {
                        SaveFilter(item);
                    }
                    break;
                case System.Collections.Specialized.NotifyCollectionChangedAction.Remove:
                    foreach (Filter item in e.OldItems)
                    {
                        DeleteFilter(item);
                    }
                    break;
                case System.Collections.Specialized.NotifyCollectionChangedAction.Replace:
                    foreach (Filter item in e.OldItems)
                    {
                        DeleteFilter(item);
                    }
                    foreach (Filter item in e.NewItems)
                    {
                        SaveFilter(item);
                    }
                    break;
                case System.Collections.Specialized.NotifyCollectionChangedAction.Reset:
                    foreach (Filter item in e.OldItems)
                    {
                        DeleteFilter(item);
                    }
                    break;
                case System.Collections.Specialized.NotifyCollectionChangedAction.Move:
                    break;
            }
        }

        private IEnumerable<Filter> LoadFilter()
        {
            var tempFilterList = new List<Filter>();
            var serializer = new XmlFilterSerializer();

            DirectoryInfo d = new DirectoryInfo(Filter.FILTER_PATH);
            if (d.Exists)
            {
                FileInfo[] Files = d.GetFiles("*.flt");
                foreach (FileInfo file in Files)
                {
                    var stream = file.OpenRead();
                    tempFilterList.Add(serializer.Deserialize(stream));
                    stream.Close();
                }
            }
            return tempFilterList;
        }

        private void SaveFilter(Filter filter)
        {
            var serializer = new XmlFilterSerializer();

            DirectoryInfo d = new DirectoryInfo(Filter.FILTER_PATH);
            if (!d.Exists)
                d.Create();

            try
            {
                var filterStream = File.Create(d.FullName + "\\" + filter.ID + ".flt");
                serializer.Serialize(filterStream, filter);
                filterStream.Close();
            }
            catch (Exception e)
            {
                var message = String.Concat("Couldn't save Filter: ", e.Message);
                System.Windows.MessageBox.Show(message, "Error while trying to save Filter", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            }
        }

        private void DeleteFilter(Filter filter)
        {
            DirectoryInfo d = new DirectoryInfo(Filter.FILTER_PATH);
            if (d.Exists)
            {
                try
                {
                    File.Delete(d.FullName + "\\" + filter.ID + ".flt");
                }
                catch { }
            }
        }

        private IEnumerable<Combination> LoadCombinations()
        {
            var tempCombinationList = new List<Combination>();
            var serializer = new XmlCombinationSerializer();

            DirectoryInfo d = new DirectoryInfo(Combination.COMBINATION_PATH);
            if (d.Exists)
            {
                FileInfo[] Files = d.GetFiles("*.flt");
                foreach (FileInfo file in Files)
                {
                    var stream = file.OpenRead();
                    tempCombinationList.Add(serializer.Deserialize(stream));
                    stream.Close();
                }
            }
            return tempCombinationList;
        }


        private void DeleteCombination(Combination item)
        {
            DirectoryInfo d = new DirectoryInfo(Combination.COMBINATION_PATH);
            if (d.Exists)
            {
                try
                {
                    File.Delete(d.FullName + "\\" + item.ID + ".flt");
                }
                catch { }
            }
        }

        private void SaveCombination(Combination item)
        {
            var serializer = new XmlCombinationSerializer();

            DirectoryInfo d = new DirectoryInfo(Combination.COMBINATION_PATH);
            if (!d.Exists)
                d.Create();

            try
            {
                var filterStream = File.Create(d.FullName + "\\" + item.ID + ".flt");
                serializer.Serialize(filterStream, item);
                filterStream.Close();
            }
            catch (Exception e)
            {
                var message = String.Concat("Couldn't save Filter: ", e.Message);
                System.Windows.MessageBox.Show(message, "Error while trying to save Combination", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            }

        }
    }
}
