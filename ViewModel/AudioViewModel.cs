using AudioVideoParcerVk.Model;
using AudioVideoParcerVk.Unit;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Threading;
using KBCsv;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Telerik.Windows.Data;
using VkNet;

namespace AudioVideoParcerVk.ViewModel
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class AudioViewModel : ViewModelBase
    {
        private Vk_api vk_api = new Vk_api();
        private VkApi vk;

        public ICommand GetSearchValue { get; private set; }
        public ICommand StopGetValue { get; private set; }
        public ICommand UnloadCSV { get; private set; }
        public ICommand OpenFileManySearch { get; private set; }
        public ICommand GetListSearchValue { get; private set; }
        public ICommand ClearRadGrid { get; private set; }

        public ObservableCollection<UserItem> UserItemList { get; set; }
        public static QueryableCollectionView PagedAudioSource { get; set; }

        List<UserItem> resultList;

        private bool StopCheck { get; set; }

        /// <summary>
        /// SearchAudio
        /// </summary>
        private string _searchAudio;
        public string SearchAudio
        {
            get { return this._searchAudio; }
            set
            {
                // Implement with property changed handling for INotifyPropertyChanged
                if (!string.Equals(this._searchAudio, value))
                {
                    this._searchAudio = value;
                    RaisePropertyChanged("SearchAudio"); // Method to raise the PropertyChanged event in your BaseViewModel class...
                }
            }
        }

        private bool _active;
        public bool Active
        {
            get { return this._active; }
            set
            {
                // Implement with property changed handling for INotifyPropertyChanged
                if (!string.Equals(this._active, value))
                {
                    this._active = value;
                    RaisePropertyChanged("Active");
                }
            }
        }

        /// <summary>
        /// FilePath
        /// </summary>
        private string _filePathManySearchLabel;
        public string FilePathManySearchLabel
        {
            get { return this._filePathManySearchLabel; }
            set
            {
                // Implement with property changed handling for INotifyPropertyChanged
                if (!string.Equals(this._filePathManySearchLabel, value))
                {
                    this._filePathManySearchLabel = value;
                    RaisePropertyChanged("FilePathManySearchLabel"); // Method to raise the PropertyChanged event in your BaseViewModel class...
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

        private bool _comboCheckOneSearch;
        public bool ComboCheckOneSearch
        {
            get { return _comboCheckOneSearch; }
            set
            {
                if (_comboCheckOneSearch != value)
                {
                    if (value != false)
                    {
                        ComboCheckManySearch = false;
                    }
                    _comboCheckOneSearch = value;
                    GroupBoxManyIsEnabled = false;
                    GroupBoxOneIsEnabled = true;
                    RaisePropertyChanged("ComboCheckOneSearch");
                }
            }
        }

        private bool _comboCheckManySearch;
        public bool ComboCheckManySearch
        {
            get { return _comboCheckManySearch; }
            set
            {
                if (_comboCheckManySearch != value)
                {
                    if (value != false)
                    {
                        ComboCheckOneSearch = false;
                    }
                    _comboCheckManySearch = value;
                    GroupBoxManyIsEnabled = true;
                    GroupBoxOneIsEnabled = false;

                    RaisePropertyChanged("ComboCheckManySearch");
                }
            }
        }

        private bool _groupBoxManyIsEnabled;
        public bool GroupBoxManyIsEnabled
        {
            get { return this._groupBoxManyIsEnabled; }
            set
            {
                // Implement with property changed handling for INotifyPropertyChanged
                if (!string.Equals(this._groupBoxManyIsEnabled, value))
                {
                    this._groupBoxManyIsEnabled = value;
                    RaisePropertyChanged("GroupBoxManyIsEnabled");
                }
            }
        }

        private bool _groupBoxOneIsEnabled;
        public bool GroupBoxOneIsEnabled
        {
            get { return this._groupBoxOneIsEnabled; }
            set
            {
                // Implement with property changed handling for INotifyPropertyChanged
                if (!string.Equals(this._groupBoxOneIsEnabled, value))
                {
                    this._groupBoxOneIsEnabled = value;
                    RaisePropertyChanged("GroupBoxOneIsEnabled");
                }
            }
        }

        /// <summary>
        /// Initializes a new instance of the AudioViewModel class.
        /// </summary>
        public AudioViewModel()
        {
            Messenger.Default.Register<DataItem>(this, HandleRegistrationInfo);

            resultList = new List<UserItem>();

            UserItemList = new ObservableCollection<UserItem>(resultList.Select(b => new UserItem(b.UserId, b.UserName, b.Age, b.FamilyStatus, b.City, b.Country, b.Artist, b.CountSongs, b.Phone, b.Query)));
            PagedAudioSource = new QueryableCollectionView(UserItemList);

            GetSearchValue = new RelayCommand(() => GetSearchValueExecute(SearchAudio), () => true);
            StopGetValue = new RelayCommand(() => StopTaskExecute(), () => true);
            UnloadCSV = new RelayCommand(() => UnloadCSVExecute(), () => true);
            OpenFileManySearch = new RelayCommand(() => OpenFileManySearchExecute(), () => true);
            GetListSearchValue = new RelayCommand(() => GetListSearchValueExecute(), () => true);
            ClearRadGrid = new RelayCommand(() => ClearRadGridExecute(), () => true);
        }

        private void ClearRadGridExecute()
        {
            Task.Factory.StartNew(() =>
            {
                DispatcherHelper.CheckBeginInvokeOnUI(() =>
                {
                    UserItemList.Clear();
                    for (int i = 0; i < PagedAudioSource.Count; i++)
                    {
                        PagedAudioSource.RemoveAt(i);
                    }

                    //PagedVideoSource = new QueryableCollectionView(UserItemList);
                });
            });
        }

        private void GetSearchValueExecute(string searchValue)
        {
            Task.Factory.StartNew(() =>
            {
                if (SearchAudio == null || SearchAudio == "")
                {
                    DispatcherHelper.CheckBeginInvokeOnUI(() => Active = true);
                    MessageBox.Show("Строка поиска не может быть пустой! ");
                    return;
                }
                if (vk == null)
                {
                    DispatcherHelper.CheckBeginInvokeOnUI(() => Active = true);
                    MessageBox.Show("Object vk == null. Return");
                    return;
                }

                DispatcherHelper.CheckBeginInvokeOnUI(() =>
                {
                    StatusWorker = "Работает";
                    Active = false;
                    Messenger.Default.Send(new StopTaskItem(true));
                });
                
                resultList = vk_api.GetUserDataExecute(vk, SearchAudio);

                DispatcherHelper.CheckBeginInvokeOnUI(() =>
                {
                                     
                    UserItemList.Clear();
                   
                    PagedAudioSource = new QueryableCollectionView(UserItemList);
                    //PagedPaginationAudioSource = new QueryableCollectionView(UserItemList);

                    UserItemList = new ObservableCollection<UserItem>(resultList.Select(b => new UserItem(b.UserId, b.UserName, b.Age, b.FamilyStatus, b.City, b.Country, b.Artist, b.CountSongs, b.Phone, b.Query)));

                    foreach (var pagedItem in UserItemList)
                    {
                        PagedAudioSource.AddNew(pagedItem);
                    }

                    Messenger.Default.Send(new StopTaskItem(false));
                    Active = true;
                    StatusWorker = "Не работает";

                });

            });
        }

        private void GetListSearchValueExecute()
        {
            if (FilePathManySearchLabel == null)
            {
                MessageBox.Show("Выберите файл");
                return;
            }
            string readQuery = System.IO.File.ReadAllText(FilePathManySearchLabel, Encoding.Default);
            string[] separators = { ";" };
            string[] words = readQuery.Split(separators, StringSplitOptions.RemoveEmptyEntries);
            Task.Factory.StartNew(() =>
            {
                DispatcherHelper.CheckBeginInvokeOnUI(() => StatusWorker = "Работает");
                DispatcherHelper.CheckBeginInvokeOnUI(() => UserItemList.Clear());
                DispatcherHelper.CheckBeginInvokeOnUI(() => Active = false);
                Messenger.Default.Send(new StopTaskItem(true));
                foreach (string queryItem in words)
                {
                   
                    if (vk == null)
                    {
                        MessageBox.Show("Object vk == null. Return");
                        return;
                    }

                    resultList = vk_api.GetUserDataExecute(vk, queryItem);
                    DispatcherHelper.CheckBeginInvokeOnUI(() =>
                    {
                        UserItemList.Clear();

                        UserItemList = new ObservableCollection<UserItem>(resultList.Select(b => new UserItem(b.UserId, b.UserName, b.Age, b.FamilyStatus, b.City, b.Country, b.Artist, b.CountSongs, b.Phone, b.Query)));

                        foreach (var pagedItem in UserItemList)
                        {
                            PagedAudioSource.AddNew(pagedItem);
                        }

                    });

                    if (StopCheck)
                    {
                        StopCheck = false;
                        break;
                    }
                }
                Messenger.Default.Send(new StopTaskItem(false));
                DispatcherHelper.CheckBeginInvokeOnUI(() => StatusWorker = "Не работает");
                DispatcherHelper.CheckBeginInvokeOnUI(() => Active = true);
                MessageBox.Show("Получение данных завершено");
            });
        }

        private void StopTaskExecute()
        {
            Messenger.Default.Send(new StopTaskItem(false));
            StopCheck = true;
        }

        private void UnloadCSVExecute()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.RestoreDirectory = true;
            saveFileDialog.DefaultExt = ".csv";
            saveFileDialog.FileName = "output";

            // Show save file dialog box
            Nullable<bool> result = saveFileDialog.ShowDialog();
            // Process save file dialog box results
            if (result == true)
            {
                // Save document
                string fileOutputPath = saveFileDialog.FileName;
                using (var streamWriter = new StreamWriter(fileOutputPath))
                using (var writer = new CsvWriter(streamWriter))
                {
                    writer.ForceDelimit = true;
                    writer.ValueSeparator = ';';

                    writer.WriteRecord("_userId_", "_userName_", "_city_", "_age_", "_familyStatus",  "_country_", "_artist_", "_countsongs_", "_phone_");
                    foreach (UserItem item in PagedAudioSource)
                    {
                        writer.WriteRecord(item.UserId.ToString(), item.UserName, item.City, item.Age, item.FamilyStatus, item.Country, item.Artist, item.CountSongs.ToString(), item.Phone);
                    }
                }
            }
        }

        private void OpenFileManySearchExecute()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            openFileDialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            openFileDialog.FilterIndex = 2;
            openFileDialog.RestoreDirectory = true;

            // Show save file dialog box
            Nullable<bool> result = openFileDialog.ShowDialog();
            // Process save file dialog box results
            if (result == true)
            {
                FilePathManySearchLabel = openFileDialog.FileName;
            }
        }



        private void HandleRegistrationInfo(DataItem info)
        {
            try
            {
                Active = info.vk.IsAuthorized;
                vk = info.vk;
            }
            catch
            {

            }
        }
    }
}