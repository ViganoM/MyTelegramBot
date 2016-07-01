using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Web.UI;
using System.Web.Script.Serialization;
using System.IO;
using System.Diagnostics;
using System.Threading;
using System.Linq;
using JsonTypes;

namespace myTelegramBot {
    class Program {

        static void Main(string[] args) {
            if ( ServerMethods.GetMe().ok )
             new Program().Updater();
            //Console.ReadKey();
        }
        void Updater() {
            ServerMethods.GetUpdates(0, 1000);
            while ( true ) {
                if ( ServerMethods.lastUpdate.Count == 0 )
                    ServerMethods.GetUpdates(0, 1000);
                List<int> keys = ServerMethods.lastUpdate.Keys.ToList();
                
                foreach ( int chat_id in keys )
                    if ( ServerMethods.GetLastUpdateByChat(chat_id, true) != null )
                        ServerMethods.sendMessage(chat_id, ServerMethods.GetLastUpdateByChat(chat_id).message.text + "\n<i>hi</i>\n<b><a href=amicidibembo.eu>MV</a></b>", ServerMethods.parse_mode.HTML);
                Thread.Sleep(100);
            }
        }
    //    void 
    }
}

