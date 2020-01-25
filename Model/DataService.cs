using System;

namespace AudioVideoParcerVk.Model
{
    public class DataService : IDataService
    {
        public void GetData(Action<DataItem, Exception> callback)
        {
            // Use this to connect to the actual data service

            var item = new DataItem { Title = "Welcome to MVVM Light" };
            callback(item, null);
        }
    }
}