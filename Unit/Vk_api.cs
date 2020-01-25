using AudioVideoParcerVk.Model;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using VkNet;
using VkNet.Enums.Filters;
using VkNet.Exception;
using VkNet.Model;
using VkNet.Model.Attachments;
using VkNet.Model.RequestParams;

namespace AudioVideoParcerVk.Unit
{
    public class Vk_api
    {
        public VkApi vk = new VkApi();

        private bool _task;
        public bool Task
        {
            get { return this._task; }
            set
            {
                // Implement with property changed handling for INotifyPropertyChanged
                if (!string.Equals(this._task, value))
                {
                    this._task = value;                    
                }
            }
        }


        /// <summary>
        /// Авторизация vk
        /// </summary>
        /// <param name="login">Логин</param>
        /// <param name="pass">Пароль</param>
        /// <returns></returns>
        /// 
        public VkApi Auth(string login, string pass)
        {
            ulong appID = 5533330;

            var authorize = new ApiAuthParams();
            Settings scope = Settings.All;
            authorize.Login = login;
            authorize.Password = pass;
            authorize.ApplicationId = appID;
            authorize.Settings = scope;

            try
            {
                vk.Authorize(authorize);
            }
            catch (VkNet.Exception.VkApiAuthorizationException e)
            {
                //TO DO: Вынести строковую константу из кода, для локализируемости
                MessageBox.Show(e.Email +
                                "\nВозможно данный логин не существует, либо пароль набран не верно.\nПовторите ввод.");
                return null;
            }

            return vk;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="_vk"></param>
        /// <param name="groups"></param>
        /// <param name="getDetailUser">Если true, то получает детальную информацию</param>
        /// <returns></returns>
        public IEnumerable<UserItem> GetUserDataGroupsExecute(VkApi _vk, string groups, bool getDetailUser)
        {
            IEnumerable<UserItem> finishedResult;
            this.vk = _vk;
            List<User> listUserId = GetIdUser(groups);
            if (getDetailUser)
            {
                finishedResult = listUserId.Select(b => new UserItem(b.Id, "-", "-", "-", "-", "-", "-", 13, "-", groups));              
            }
            else
            {
                finishedResult = GetUserGroupsInformation(listUserId, groups);
            }

            return finishedResult;
        }

        public List<UserItem> GetUserDataExecute(VkApi _vk, string query)
        {
            this.vk = _vk;
            //Получаем максимально доступное количество композиций по запросу
            int offset = 0;
            List<Audio> listAudio = GetAudio(query);
            //Получаем информацию о пользователях у которых есть данная композиция
            List<UserItem> finishedResult= GetUserInformation(listAudio, query);

            return finishedResult;
        }

        public IEnumerable<UserItem> GetUserDataVideoExecute(VkApi _vk, string query, bool getDetailUser)
        {
            IEnumerable<UserItem> finishedResult;

            this.vk = _vk;
            //Получаем максимально доступное количество композиций по запросу
            int offset = 0;
            List<Video> listVideo = GetVideo(query);
            //Получаем информацию о пользователях у которых есть данная композиция
            if (getDetailUser)
            {
                finishedResult = listVideo.Select(b => new UserItem((long)b.OwnerId, "-", "-", "-", "-", "-", "-", 13, "-", query));
            }
            else
            {
                finishedResult = GetUserVideoInformation(listVideo, query);
            }
            
            return finishedResult;
        }

        public IEnumerable<UserItem> GetUserDataNewsExecute(VkApi _vk, string query, bool getDetailUser)
        {
            this.vk = _vk;
            IEnumerable<UserItem> finishedResult;
            //Получаем максимально доступное количество новостей по запросу
            int offset = 0;
            List<NewsSearchResult> listNews = GetNews(query);
            //Получаем информацию о пользователях которые создали данную новость
            if (getDetailUser)
            {
                finishedResult = listNews.Select(b => new UserItem((long)b.OwnerId, "-", "-", "-", "-", "-", "-", 13, "-", query));
            }
            else
            {
                finishedResult = GetUserNewsInformation(listNews, query);
            }

            return finishedResult;
        }

        private List<User> GetIdUser(string groups)
        {
            List<User> listUser = new List<User>();
            try
            {
                var GroupParams = new GroupsGetMembersParams();
                GroupParams.GroupId = groups;
                GroupParams.Count = 1000;
                GroupParams.Offset = 0;
                ReadOnlyCollection<User> listUserRaw;

                int totalCount;
                vk.Groups.GetMembers(out totalCount, GroupParams);
                int offset = 0;
                Thread.Sleep(2000);
                for (int i = 0; i < totalCount; )
                {
                    GroupParams.Offset = i;

                    listUserRaw = vk.Groups.GetMembers(out totalCount, GroupParams);
                    foreach (var item in listUserRaw)
                    {
                        listUser.Add(item);
                    }

                    Thread.Sleep(2000);
                    i += 1000;
                }

                return listUser;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return listUser;
            }
        }

        private List<NewsSearchResult> GetNews(string query)
        {
            var NewsSearchParams = new NewsFeedSearchParams();
            NewsSearchParams.Query = query;
            NewsSearchParams.Count = 200;

            List<NewsSearchResult> listNews = new List<NewsSearchResult>();
            IReadOnlyCollection<NewsSearchResult> listNewsReadOnly = vk.NewsFeed.Search(NewsSearchParams);
            Thread.Sleep(1000);
            foreach (NewsSearchResult item in listNewsReadOnly)
            {
                listNews.Add(item);
            }

            return listNews;
        }

        private List<Video> GetVideo(string query)
        {
            var videoSearchParams = new VideoSearchParams();
            videoSearchParams.Query = query;
            videoSearchParams.Count = 200;

            
            List<Video> listVideo = new List<Video>();
            long totalCount;
            int offset = 0;
            for (int i = 0; i < 5; i++)
            {
                videoSearchParams.Offset = offset;
                IReadOnlyCollection<Video> listVideoReadOnly = vk.Video.Search(videoSearchParams);
                Thread.Sleep(1000);
                foreach (Video item in listVideoReadOnly)
                {
                    listVideo.Add(item);
                }
                offset += 200;
            }

            return listVideo;
        }

        /// <summary>
        /// Метод  возвращает список объектов audio с максисмальным смещением offset 1000.
        /// </summary>
        /// <param name="query"></param>
        private List<Audio> GetAudio(string query)
        {
            var audioSearchParams = new AudioSearchParams();
            audioSearchParams.Query = query;
            
            audioSearchParams.Count = 300;

            long totalCount;
            List<Audio> listAudio = new List<Audio>();
            int offset = 0;
            for (int i = 0; i < 5; i++)
            {
                audioSearchParams.Offset = offset;
                IReadOnlyCollection<Audio> listAudioReadOnly = vk.Audio.Search(audioSearchParams, out totalCount);
                Thread.Sleep(1000);
                foreach (Audio item in listAudioReadOnly)
                {
                    listAudio.Add(item);
                }
                offset += 300;
            }
            return listAudio;
        }
        /// <summary>
        /// Метод получает информацию о пользователях у которых есть найденные композций
        /// </summary>
        private List<UserItem> GetUserInformation(List<Audio> listAudio, string query)
        {
            Task = true;
            List<UserItem> userData = new List<UserItem>();
            Messenger.Default.Register<StopTaskItem>(this, HandleRegistrationStopTaskItem);
            
            int countAddGlobal = 0;
            int allAudioCount = listAudio.Count;
            foreach (Audio item in listAudio)
            {
                if (!Task)
                {
                    Messenger.Default.Send(new DataItem { Title = "Завершаем выполнение текущей операций", vk = null });
                    break;
                }
                
                if (item.OwnerId.ToString().Contains("-"))
                {
                    Messenger.Default.Send(new DataItem { Title = String.Format("Audio {0} принадлежит сообществу {1}", item.Artist + " - " + item.Title, item.OwnerId) });
                    allAudioCount--;
                    continue;
                }

                try
                {                    
                    int countArtist = 0;

                    var audioParams = new AudioGetParams();
                    audioParams.OwnerId = item.OwnerId;
                    audioParams.NeedUser = false;
                    audioParams.Offset = 0;
                    audioParams.Count = 6000;

                    User outUser;
                    IReadOnlyCollection<Audio> thisUserAllSongs = vk.Audio.Get(out outUser, audioParams);
                    foreach (Audio songUser in thisUserAllSongs)
                    {
                        if (songUser.Artist.Contains(item.Artist))
                        {
                            countArtist += 1;
                        }
                    }

                    User userAccess = vk.Users.Get(item.OwnerId.Value, ProfileFields.All);
                    string city;
                    string country;
                    string age;
                    string familyStatus;
                    if (userAccess.City == null)
                    {
                        city = "-";
                    }
                    else
                    {
                        city = userAccess.City.Title;
                    }

                    if (userAccess.Country == null)
                    {
                        country = "-";
                    }
                    else
                    {
                        country = userAccess.Country.Title;
                    }

                    if (userAccess.BirthDate == null)
                    {
                        age = "-";
                    }
                    else
                    {
                        age = userAccess.BirthDate;
                    }

                    if (userAccess.Relation == null)
                    {
                        familyStatus = "-";
                    }

                    else
                    {
                        familyStatus = userAccess.Relation.ToString();
                    }


                    userData.Add(new UserItem(userAccess.Id, String.Format("{0} {1}", userAccess.FirstName, userAccess.LastName), age, familyStatus, city, country, item.Artist, countArtist, userAccess.MobilePhone, query));

                    countAddGlobal += 1;
                    Messenger.Default.Send(new DataItem { Title = String.Format("User {0} получен. {1} из {2} ({1}/{2}) Поисковой запрос: {3} , Название композиций: {4}", userAccess.Id, countAddGlobal, allAudioCount, query, item.Artist + " - " + item.Title) });

                    /*
                    User user;
                    
                    int countArtist = 0;

                    IReadOnlyCollection<Audio> thisUserAllSongs = vk.Audio.Get(out user, audioParams);
                    foreach (Audio songUser in thisUserAllSongs)
                    {
                        if (songUser.Artist.Contains(item.Artist))
                        {
                            countArtist += 1;
                        }
                    }
                    string city;
                    string country;
                    if (user.City == null)
                    {
                        city = "-";
                    }
                    else
                    {
                        city = user.City.Title;
                    }

                    if (user.Country == null)
                    {
                        country = "-";
                    }
                    else
                    {
                        country = user.Country.Title;
                    }

                    userData.Add(new UserItem(user.Id, String.Format("{0} {1}", user.FirstName, user.LastName), city, country, item.Artist, countArtist, user.MobilePhone));

                    countAddGlobal += 1;
                    Messenger.Default.Send(new DataItem(String.Format("User {0} получен. {1} из {2} ({1}/{2}) Поисковой запрос: {3}", user.Id, countAddGlobal, listAudio.Count, query)));
                    */
                }
                catch (AccessDeniedException ex)
                {
                    Messenger.Default.Send(ex.Message);
                }
                catch (Exception ex)
                {
                    Messenger.Default.Send(ex.Message);
                }
                                           
                //Thread.Sleep(2000);
            }

            return userData;            
        }
        
        private List<UserItem> GetUserGroupsInformation(List<User> listUser, string groups)
        {
            Task = true;
            List<UserItem> userData = new List<UserItem>();
            List<long> listBufer = new List<long>();//Буфер
            List<User> listUserRaw = new List<User>();//Итоговый результат

            Messenger.Default.Register<StopTaskItem>(this, HandleRegistrationStopTaskItem);

            int countAddGlobal = 0;
            int count = 0;
            int step = 0;
          
            foreach (User item in listUser)
            {
                if (!Task)
                {
                    Messenger.Default.Send(new DataItem { Title = "Завершаем выполнение текущей операций" });
                    break;
                }
                if (count >= 500 || step == listUser.Count - 1)
                {
                    Messenger.Default.Send(new DataItem { Title = "Получаем пакет пользователей" });
                    var userRawList = vk.Users.Get(listBufer, ProfileFields.All);
                    foreach (var userItem in userRawList)
                    {
                        listUserRaw.Add(userItem);
                    }
                    listBufer.Clear();
                    count = 0;
                    countAddGlobal++;
                    Messenger.Default.Send(new DataItem { Title = String.Format("Шаг {0} из {1}", countAddGlobal, listUser.Count / 500) });
                    //userData.Add(new UserItem(userAccess.Id, String.Format("{0} {1}", userAccess.FirstName, userAccess.LastName), age, familyStatus, city, country, "", 0, userAccess.MobilePhone, groups));

                    //countAddGlobal += 1;
                    //Messenger.Default.Send(new DataItem { Title = String.Format("User {0} получен. {1} из {2} ({1}/{2}) Поисковой запрос: {3}", userAccess.Id, countAddGlobal, listUser.Count, groups) });
                }
                
                listBufer.Add(item.Id);
                count++;
                step++;            
            }
            foreach (var itemUser in listUserRaw)
            {
                string city;
                string country;

                if (itemUser.City == null)
                {
                    city = "-";
                }
                else
                {
                    city = itemUser.City.Title;
                }

                if (itemUser.Country == null)
                {
                    country = "-";
                }
                else
                {
                    country = itemUser.Country.Title;
                }

                userData.Add(new UserItem(itemUser.Id, itemUser.FirstName + " " + itemUser.LastName ?? "-", itemUser.BirthDate ?? "-", itemUser.Relation.ToString(), city, country, "", 0, itemUser.MobilePhone ?? "-", groups));
            }
           
            return userData;
        }

        private List<UserItem> GetUserVideoInformation(List<Video> listAudio, string query)
        {
            Task = true;
            List<UserItem> userData = new List<UserItem>();
            Messenger.Default.Register<StopTaskItem>(this, HandleRegistrationStopTaskItem);

            int countAddGlobal = 0;
            int allVideoCount = listAudio.Count;
            foreach (Video item in listAudio)
            {
                if (!Task)
                {
                    Messenger.Default.Send(new DataItem { Title = "Завершаем выполнение текущей операций" });
                    break;
                }

                if (item.OwnerId.ToString().Contains("-"))
                {
                    Messenger.Default.Send(new DataItem { Title = String.Format("Video {0} принадлежит сообществу {1}", item.Title, item.OwnerId) });
                    allVideoCount--;
                    continue;
                }

                try
                {
                    User userAccess = vk.Users.Get(item.OwnerId.Value, ProfileFields.All);
                    string city;
                    string country;
                    string age;
                    string familyStatus;
                    if (userAccess.City == null)
                    {
                        city = "-";
                    }
                    else
                    {
                        city = userAccess.City.Title;
                    }

                    if (userAccess.Country == null)
                    {
                        country = "-";
                    }
                    else
                    {
                        country = userAccess.Country.Title;
                    }

                    if (userAccess.BirthDate == null)
                    {
                        age = "-";
                    }
                    else
                    {
                        age = userAccess.BirthDate;
                    }

                    if (userAccess.Relation == null)
                    {
                        familyStatus = "-";
                    }

                    else
                    {
                        familyStatus = userAccess.Relation.ToString();
                    }


                    userData.Add(new UserItem(userAccess.Id, String.Format("{0} {1}", userAccess.FirstName, userAccess.LastName), age, familyStatus, city, country, "", 0, userAccess.MobilePhone, query));

                    countAddGlobal += 1;
                    Messenger.Default.Send(new DataItem { Title = String.Format("User {0} получен. {1} из {2} ({1}/{2}) Поисковой запрос: {3}, Название: {4}", userAccess.Id, countAddGlobal, allVideoCount, query, item.Title) });

                    /*
                    User user;
                    
                    int countArtist = 0;

                    IReadOnlyCollection<Audio> thisUserAllSongs = vk.Audio.Get(out user, audioParams);
                    foreach (Audio songUser in thisUserAllSongs)
                    {
                        if (songUser.Artist.Contains(item.Artist))
                        {
                            countArtist += 1;
                        }
                    }
                    string city;
                    string country;
                    if (user.City == null)
                    {
                        city = "-";
                    }
                    else
                    {
                        city = user.City.Title;
                    }

                    if (user.Country == null)
                    {
                        country = "-";
                    }
                    else
                    {
                        country = user.Country.Title;
                    }

                    userData.Add(new UserItem(user.Id, String.Format("{0} {1}", user.FirstName, user.LastName), city, country, item.Artist, countArtist, user.MobilePhone));

                    countAddGlobal += 1;
                    Messenger.Default.Send(new DataItem(String.Format("User {0} получен. {1} из {2} ({1}/{2}) Поисковой запрос: {3}", user.Id, countAddGlobal, listAudio.Count, query)));
                    */
                }
                catch (AccessDeniedException ex)
                {
                    Messenger.Default.Send(ex.Message);
                }
                catch (Exception ex)
                {
                    Messenger.Default.Send(ex.Message);
                }

                //Thread.Sleep(2000);
            }

            return userData;
        }

        private List<UserItem> GetUserNewsInformation(List<NewsSearchResult> listNews, string query)
        {
            Task = true;
            List<UserItem> userData = new List<UserItem>();
            Messenger.Default.Register<StopTaskItem>(this, HandleRegistrationStopTaskItem);

            int countAddGlobal = 0;
            foreach (NewsSearchResult item in listNews)
            {
                if (!Task)
                {
                    Messenger.Default.Send(new DataItem { Title = "Завершаем выполнение текущей операций" });
                    break;
                }

                try
                {
                    User userAccess = vk.Users.Get(item.OwnerId, ProfileFields.All);
                    string city;
                    string country;
                    string age;
                    string familyStatus;
                    if (userAccess.City == null)
                    {
                        city = "-";
                    }
                    else
                    {
                        city = userAccess.City.Title;
                    }

                    if (userAccess.Country == null)
                    {
                        country = "-";
                    }
                    else
                    {
                        country = userAccess.Country.Title;
                    }

                    if (userAccess.BirthDate == null)
                    {
                        age = "-";
                    }
                    else
                    {
                        age = userAccess.BirthDate;
                    }

                    if (userAccess.Relation == null)
                    {
                        familyStatus = "-";
                    }

                    else
                    {
                        familyStatus = userAccess.Relation.ToString();
                    }


                    userData.Add(new UserItem(userAccess.Id, String.Format("{0} {1}", userAccess.FirstName, userAccess.LastName), age, familyStatus, city, country, "", 0, userAccess.MobilePhone, query));

                    countAddGlobal += 1;
                    Messenger.Default.Send(new DataItem { Title = String.Format("User {0} получен. {1} из {2} ({1}/{2}) Поисковой запрос: {3}", userAccess.Id, countAddGlobal, listNews.Count, query) });

                    /*
                    User user;
                    
                    int countArtist = 0;

                    IReadOnlyCollection<Audio> thisUserAllSongs = vk.Audio.Get(out user, audioParams);
                    foreach (Audio songUser in thisUserAllSongs)
                    {
                        if (songUser.Artist.Contains(item.Artist))
                        {
                            countArtist += 1;
                        }
                    }
                    string city;
                    string country;
                    if (user.City == null)
                    {
                        city = "-";
                    }
                    else
                    {
                        city = user.City.Title;
                    }

                    if (user.Country == null)
                    {
                        country = "-";
                    }
                    else
                    {
                        country = user.Country.Title;
                    }

                    userData.Add(new UserItem(user.Id, String.Format("{0} {1}", user.FirstName, user.LastName), city, country, item.Artist, countArtist, user.MobilePhone));

                    countAddGlobal += 1;
                    Messenger.Default.Send(new DataItem(String.Format("User {0} получен. {1} из {2} ({1}/{2}) Поисковой запрос: {3}", user.Id, countAddGlobal, listAudio.Count, query)));
                    */
                }
                catch (AccessDeniedException ex)
                {
                    Messenger.Default.Send(ex.Message);
                }
                catch (Exception ex)
                {
                    Messenger.Default.Send(ex.Message);
                }

                //Thread.Sleep(2000);
            }

            return userData;
        }



        /// <summary>
        /// 
        /// </summary>
        private void GetUserExtendedInformation()
        {

        }

        /// <summary>
        /// Replace Empty '-'
        /// </summary>
        /// <param name="user"></param>
        private void ReplaceEmptyParam(User user)
        {

        }

        private void HandleRegistrationStopTaskItem(StopTaskItem info)
        {
            Task = false;
        }

    }
   
}
