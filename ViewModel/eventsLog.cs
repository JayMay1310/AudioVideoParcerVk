using ContactsParserLinkedin.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactsParserLinkedin.ViewModel
{
    public class eventsLog
    {
        IMsgService MsgService;

        public eventsLog(IMsgService MsgService)
        {
            this.MsgService = MsgService;
        }

        public async void UniversalLog(string message)
        {
            await Task.Run(async () =>
            {
                await Task.Delay(new Random().Next(1000, 1300));
                MsgService.Publish(message);
            });
        }
    }
}
