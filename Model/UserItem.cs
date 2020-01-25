using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace AudioVideoParcerVk.Model
{
    public class UserItem
    {
        private long _userId;
        private string _userName;
        private string _age;
        private string _familyStatus;
        private string _city;
        private string _artist;
        private int _countsongs;
        private string _phone;
        private string _country;
        private string _query;

        public UserItem(long userId, string userName, string age, string familyStatus, string city, string country, string artist, int countsongs, string phone, string query)
        {
            _userId = userId;
            _userName = userName;
            _age = age;
            _familyStatus = familyStatus;
            _city = city;
            _artist = artist;
            _countsongs = countsongs;
            _phone = phone;
            _country = country;
            _query = query;
        }

        public long UserId
        {
            get { return _userId; }
            set
            {
                if (value.Equals(_userId)) return;
                _userId = value;
                RaisePropertyChanged("UserId");
            }
        }

        public string City
        {
            get { return _city; }
            set
            {
                if (value.Equals(_city)) return;
                _city = value;
                RaisePropertyChanged("City");
            }
        }

        public string Country
        {
            get { return _country; }
            set
            {
                if (value.Equals(_country)) return;
                _country = value;
                RaisePropertyChanged("Country");
            }
        }

        public string Artist
        {
            get { return _artist; }
            set
            {
                if (value.Equals(_artist)) return;
                _artist = value;
                RaisePropertyChanged("Artist");
            }
        }

        public string UserName
        {
            get { return _userName; }
            set
            {
                if (value.Equals(_userName)) return;
                _userName = value;
                RaisePropertyChanged("UserName");
            }
        }

        public string FamilyStatus
        {
            get { return _familyStatus; }
            set
            {
                if (value.Equals(_familyStatus)) return;
                _familyStatus = value;
                RaisePropertyChanged("FamilyStatus");
            }
        }

        public string Age
        {
            get { return _age; }
            set
            {
                if (value.Equals(_age)) return;
                _age = value;
                RaisePropertyChanged("Age");
            }
        }


        public int CountSongs
        {
            get { return _countsongs; }
            set
            {
                if (value.Equals(_countsongs)) return;
                _countsongs = value;
                RaisePropertyChanged("CountSongs");
            }
        }

        public string Phone
        {
            get { return _phone; }
            set
            {
                if (value.Equals(_phone)) return;
                _phone = value;
                RaisePropertyChanged("Phone");
            }
        }

        public string Query
        {
            get { return _query; }
            set
            {
                if (value.Equals(_query)) return;
                _query = value;
                RaisePropertyChanged("Query");
            }
        }



        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        protected void RaisePropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

    }
}
