using System;
using AudioVideoParcerVk.Model;

namespace AudioVideoParcerVk.Design
{
    public class DesignDataService : IDataService
    {
        public void GetData(Action<DataItem, Exception> callback)
        {
            // Use this to create design time data

            var item = new DataItem { Title = "Welcome to MVVM Light [design]" };
            callback(item, null);
        }
    }
}