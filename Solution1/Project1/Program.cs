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
namespace Project1 {
    class Program {

        static void Main(string[] args) {
            if ( ServerMethods.GetMe().ok )
                new Program().DiscoverChar();
            // new Program().Updater();
            //Console.ReadKey();
        }
        void Updater() {
            ServerMethods.GetUpdates(0, 1000);
            while ( true ) {
                List<int> keys = ServerMethods.lastUpdate.Keys.ToList();

                foreach ( int chat_id in keys )
                    if ( ServerMethods.GetLastUpdateByChat(chat_id, true) != null )
                        ServerMethods.sendMessage(chat_id, ServerMethods.GetLastUpdateByChat(chat_id).message.text + "<i>hi</i>\n<b>MV</b>", ServerMethods.parse_mode.HTML);
                Thread.Sleep(100);
            }
        }
        void DiscoverChar() {
            for ( int n = char.MinValue ; n <= char.MaxValue ; n++ )
                if ( !char.IsControl((char) n) ) {
                    try {
                        ServerMethods.sendMessage(157874244, (char) n + new Random().Next().ToString());
                    } catch {
                        Console.WriteLine("--------------------" + (char) n);
                        Console.WriteLine((char) n);
                  }
                }
        }
    }
}

