using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using JsonTypes;
using myTelegramBot.Properties;

namespace myTelegramBot {
    public class Program {
        /*//use this to STOP Updater loop*/
        public static bool run { private get; set; }


        static void Main(string[] args) {
            Console.ReadLine();
            localUsersData.Load();

            if ( ServerMethods.GetMe().ok ) {
                Console.WriteLine("fully operational");
                new Thread(Updater).Start();
            } else
                Console.WriteLine("Telegram doesn't like you!\n(GetMe returned not ok)");
            ServerMethods.sendBroadMessage("I am back on\n<b>MV</b>", ServerMethods.parse_mode.HTML);
            while ( Console.ReadLine() != "exit" )
                ;
            ServerMethods.sendBroadMessage("I will be off for a while\n<b>MV</b>", ServerMethods.parse_mode.HTML);
            Settings.Default.Save();
            
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
                    if ( ServerMethods.GetLastUpdateByChat(chat_id, true) != null ) {
                        string messagetext = ServerMethods.GetLastUpdateByChat(chat_id).message.text;
                        ServerMethods.sendMessage(chat_id, messagetext + "\n<b>MV</b>", ServerMethods.parse_mode.HTML);
                        Chat chat = ServerMethods.getChat(chat_id).result;
                        Console.WriteLine(string.Format("{0} ({1} {2}) wrote: {3} (in a {4} chat)", chat.username, chat.first_name, chat.last_name, messagetext, chat.type));
                    }
                Thread.Sleep(Settings.Default.waitTime);
            }
        }
    }
}

