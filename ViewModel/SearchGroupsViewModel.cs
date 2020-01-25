using AudioVideoParcerVk.Unit;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System.Windows.Input;
using VkNet;

namespace AudioVideoParcerVk.ViewModel
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class SearchGroupsViewModel : ViewModelBase
    {
        private Vk_api vk_api = new Vk_api();
        private VkApi vk;

        public ICommand GetSearchValue { get; private set; }
        public ICommand StopGetValue { get; private set; }


        /// <summary>
        /// SearchAudio
        /// </summary>
        private string _searchGroups;
        public string SearchGroups
        {
            get { return this._searchGroups; }
            set
            {
                // Implement with property changed handling for INotifyPropertyChanged
                if (!string.Equals(this._searchGroups, value))
                {
                    this._searchGroups = value;
                    RaisePropertyChanged("SearchGroups"); // Method to raise the PropertyChanged event in your BaseViewModel class...
                }
            }
        }

        private string _statusWorker;
        public string StatusWorker
        {
            get { return this._statusWorker; }
            set
            {
                // Implement with property changed handling for INotifyPropertyChanged
                if (!string.Equals(this._statusWorker, value))
                {
                    this._statusWorker = value;
                    RaisePropertyChanged("StatusWorker"); // Method to raise the PropertyChanged event in your BaseViewModel class...
                }
            }
        }


        /// <summary>
        /// Initializes a new instance of the SearchGroupsViewModel class.
        /// </summary>
        public SearchGroupsViewModel()
        {
            //GetSearchValue = new RelayCommand(() => GetSearchValueExecute(SearchGroups), () => true);


        }
    }
}