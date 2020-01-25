using GalaSoft.MvvmLight;
using AudioVideoParcerVk.Model;
using System.Collections.ObjectModel;
using Telerik.Windows.Data;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using AudioVideoParcerVk.Unit;
using VkNet;
using GalaSoft.MvvmLight.Messaging;
using System;
using GalaSoft.MvvmLight.Ioc;
using ContactsParserLinkedin.Services;
using ContactsParserLinkedin.ViewModel;
using GalaSoft.MvvmLight.Threading;
using System.Threading.Tasks;

namespace AudioVideoParcerVk.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// See http://www.mvvmlight.net
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        //Пустой экземпляр для доступа к либе 
        private Vk_api vk_api;
        //Экземпляр для передачи в табы
        private VkApi vk = new VkApi();

        public ICommand Auth { get; private set; }
        
        private readonly IDataService _dataService;

        /// <summary>
        /// Login
        /// </summary>
        private string _login;
        public string Login
        {
            get { return this._login; }
            set
            {
                // Implement with property changed handling for INotifyPropertyChanged
                if (!string.Equals(this._login, value))
                {
                    this._login = value;
                    RaisePropertyChanged("Login"); // Method to raise the PropertyChanged event in your BaseViewModel class...
                }
            }
        }

        /// <summary>
        /// Password
        /// </summary>
        private string _password;
        public string Password
        {
            get { return this._password; }
            set
            {
                // Implement with property changed handling for INotifyPropertyChanged
                if (!string.Equals(this._password, value))
                {
                    this._password = value;
                    RaisePropertyChanged("Password"); // Method to raise the PropertyChanged event in your BaseViewModel class...
                }
            }
        }
        
        private string _progressBarVisible;
        public string ProgressBarVisible
        {
            get { return this._progressBarVisible; }
            set
            {
                // Implement with property changed handling for INotifyPropertyChanged
                if (!string.Equals(this._progressBarVisible, value))
                {
                    this._progressBarVisible = value;
                    RaisePropertyChanged("ProgressBarVisible"); // Method to raise the PropertyChanged event in your BaseViewModel class...
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
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
        

        private bool _progressBarBool;
        public bool ProgressBarBool
        {
            get { return this._progressBarBool; }
            set
            {
                // Implement with property changed handling for INotifyPropertyChanged
                if (!string.Equals(this._progressBarBool, value))
                {
                    this._progressBarBool = value;
                    RaisePropertyChanged("ProgressBarBool");
                }
            }
        }

        private string _OutPutText;
        public string OutPutText
        {
            get { return _OutPutText; }
            set { if (_OutPutText == value) return; _OutPutText = value; RaisePropertyChanged(nameof(OutPutText)); }
        }
        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel(IDataService dataService)
        {
            _dataService = dataService;
            _dataService.GetData(
                (item, error) =>
                {
                    if (error != null)
                    {
                        // Report error here
                        return;
                    }
                });
            ProgressBarBool = false;
            ProgressBarVisible = "Hidden";
            Login = Properties.Settings.Default.Login;
            Password = Properties.Settings.Default.Password;


            SimpleIoc.Default.Register<IMsgService, MsgService>();
            SimpleIoc.Default.Register<eventsLog>();

            var s = SimpleIoc.Default.GetInstance<IMsgService>();
            s.PublishEvent += (sender, args) =>
            {
                DispatcherHelper.CheckBeginInvokeOnUI(() =>
                {
                    OutPutText = $"{args.Msg}\n" + OutPutText;
                });
            };
            vk_api = new Vk_api();

            Auth = new RelayCommand(() => AuthExecute(Login, Password), () => true);

            UniversalLog("Программа работает и готова к использованию");
            Messenger.Default.Register<DataItem>(this, HandleRegistrationInfo);
            Messenger.Default.Register<StopTaskItem>(this, HandleRegistrationInfoProgressBar);
        }

        private void AuthExecute(string login, string password)
        {
            Properties.Settings.Default.Login = login;
            Properties.Settings.Default.Password = password;
            Properties.Settings.Default.Save();
            Task.Factory.StartNew(() =>
            {
                ProgressBarVisible = "Visibility";
                ProgressBarBool = true;
                this.vk = vk_api.Auth(login, password);
                if (this.vk != null)
                {
                    if (this.vk.IsAuthorized)
                    {
                        //Если авторизация выполнена, то делаем активными элементы
                        Messenger.Default.Send(new DataItem { Title = String.Format("Авторизация выполнена успешно"), vk = this.vk });
                    }
                }

                ProgressBarVisible = "Hidden";
                ProgressBarBool = false;
            });
        }

        private void HandleRegistrationInfo(DataItem info)
        {
            DispatcherHelper.CheckBeginInvokeOnUI(() => UniversalLog(info.Title));
        }
        private void HandleRegistrationInfoProgressBar(StopTaskItem info)
        {
            if (info.Value)
            {
                ProgressBarVisible = "Visibility";
                ProgressBarBool = true;
            }
            else
            {
                ProgressBarVisible = "Hidden";
                ProgressBarBool = false;
            }
        }

        internal void UniversalLog(string message)
        {
            SimpleIoc.Default.GetInstance<eventsLog>().UniversalLog(message);
        }

    }
}