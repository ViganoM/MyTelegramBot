using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using JsonTypes;
using myTelegramBot.Properties;

namespace myTelegramBot {
    public class Program {
        /*//use this to STOP Updater loop*/
        public static bool run { private get; set; } = true;


        static void Main(string[] args) {
            if ( localUsersData.LoadData() )
                Console.WriteLine("local data restored!");
            else
                Console.WriteLine("local data NOT RESTORED, you idiot!");

            if ( ServerMethods.GetMe().ok ) {
                Console.WriteLine("fully operational");
                new Thread(Updater).Start();
                ServerMethods.sendBroadMessage("I am back on\n<b>MV</b>", ServerMethods.parse_mode.HTML);
            } else
                Console.WriteLine("Telegram doesn't like you!\n(GetMe returned not ok)");

            while ( Console.ReadLine() != "exit" )
                ;
            ServerMethods.sendBroadMessage("I will be off for a while\n<b>MV</b>", ServerMethods.parse_mode.HTML);

            localUsersData.SaveData();
            Environment.Exit(0);
        }

        /*//use this to START the Updarer loop*/
        public static void Updater() {
            while ( run ) {
                ServerMethods.GetUnreadUpdates();
                Thread.Sleep(Settings.Default.waitTime);
            }
        }
    }
}
