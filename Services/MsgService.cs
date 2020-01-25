using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactsParserLinkedin.Services
{
    public delegate void mydelegate(object sender, MyArgs Args);

    public interface IMsgService
    {

        event mydelegate PublishEvent;

        void Publish(string msg);
    }

    public class MyArgs
    {
        public MyArgs(string Msg)
        {
            this.Msg = Msg;
        }
        public string Msg { get; set; }
    }

    public class MsgService : IMsgService
    {

        public event mydelegate PublishEvent;

        public void Publish(string msg)
        {
            string MSG = DateTime.Now.ToLongTimeString() + "\t" + msg;
            PublishEvent(this, new MyArgs(MSG));
        }
    }
}
