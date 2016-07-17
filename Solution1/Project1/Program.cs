using System;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using myTelegramBot.Properties;
using System.Diagnostics;
using System.IO;

namespace myTelegramBot {
    public class Program {
        /*//use this to STOP Updater loop*/
        public static bool run { private get; set; } = true;

        public static Form form;
        static AutoResetEvent formCreated = new AutoResetEvent(false);

        public static void Main(string[] args) {
            /*
            Thread formThread = new Thread(RunForm);
            formThread.Name = "Form Thread";
            formThread.Start();
            formCreated.WaitOne();
            formCreated = null; //this is not supposed to be used anymore
            */

            localUsersData.LoadData();
            localUsersData.DeleteOld();

            if ( ServerMethods.GetMe().ok ) {
                new Thread(Updater).Start();
                ServerMethods.sendBroadMessage("I am back on\n<b>MV</b>", true);
            }

        }
        /*
        static void RunForm() {
            form = new Form(formCreated);
            try {
                       Application.Run(form);
            } catch ( Exception exception ) {
                File.AppendAllLines(localUsersData.LogFilepath, exception.ToString().Split(new string[] { "\n" }, StringSplitOptions.None).ToArray());
                throw;
            }
            //code after Application.Run is not executed!
        }
        */
        public static void Close(object sender, FormClosingEventArgs e) {
            ServerMethods.sendBroadMessage("I will be off for a while\n<b>MV</b>", true);
            localUsersData.SaveData();
            Environment.Exit(0);
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
    }
}