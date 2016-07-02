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
   public class Program {
        /*//use this to STOP Updater loop*/
        public static bool run { private get; set; }


        static void Main(string[] args) {
            if ( ServerMethods.GetMe().ok )
             Updater();
        }

        /*//use this to START the Updarer loop*/
        public static void Updater() {
            ServerMethods.GetUpdates(0, 1000);
            run = true;
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
    
    }
}

