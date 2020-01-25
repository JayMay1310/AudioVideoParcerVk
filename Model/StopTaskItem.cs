using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AudioVideoParcerVk.Model
{
    public class StopTaskItem
    {
        public StopTaskItem(bool value)
        {
            Value = value;
        }

        public bool Value
        {
            get;
            private set;
        }
    }
}
