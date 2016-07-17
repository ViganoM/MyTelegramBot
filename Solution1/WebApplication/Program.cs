using System;
using System.Linq;
using System.Threading;
using System.Diagnostics;
using System.IO;
using WebApplication;
using System.Collections.Generic;

namespace myTelegramBot {
    public class Program {
        static object lockLog = new object();
        /*//use this to STOP Updater loop*/
        public static bool run { private get; set; } = true;

        public static void Main(string[] args) {
            localUsersData.LoadData();
            localUsersData.DeleteOld();

            if ( ServerMethods.GetMe().ok ) {
                new Thread(Updater).Start();
                ServerMethods.sendBroadMessage("I am back on\n<b>MV</b>", true);
            }
        }

        /*//use this to START the Updarer loop*/
        static void Updater() {
            long cycle = 0;
            int block = Convert.ToInt16(Settings.Default.SavePeriod_s / (Settings.Default.updateLoopSleepDuration_ms / 1000f));
            while ( run ) {
                ServerMethods.GetUnreadUpdates();
                Thread.Sleep(Settings.Default.updateLoopSleepDuration_ms);
                cycle++;
                if ( cycle % block == 0 )
                    localUsersData.SaveData();
            }
        }

        FileStream stream = new FileStream(localUsersData.LogFilepath, FileMode.Append, FileAccess.Write, FileShare.Write);
        public static void WriteException(IEnumerable<string> text) {
            lock ( lockLog )
                ;//File.AppendAllLines(localUsersData.LogFilepath, text);
        }
    }
}