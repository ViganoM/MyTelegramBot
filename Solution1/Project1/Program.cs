using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Web.UI;
using System.Web.Script.Serialization;
using System.IO;
using System.Diagnostics;
using System.Threading;
using JsonTypes;
namespace Project1 {
    class Program {

        static void Main(string[] args) {
            if ( ServerMethods.GetMe().ok )
                new Program().Updater();
            Console.ReadKey();
        }
        void Updater() {
            while ( true ) {
                foreach ( Update update in ServerMethods.GetUpdates().result )
                    ServerMethods.sendMessage(update.message.chat.id, update.message.text + "\n<b>MV</b>",ServerMethods.parse_mode.HTML);
                Thread.Sleep(100);
            }
        }
    }
}
