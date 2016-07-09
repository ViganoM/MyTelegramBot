using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using JsonTypes;
using System.Windows.Forms;
using myTelegramBot.Properties;

namespace myTelegramBot {
    public class Program {
        /*//use this to STOP Updater loop*/
        public static bool run { private get; set; } = true;

        static Form form = new Form();
        static Thread formThread = new Thread(RunForm);

        static void Main(string[] args) {
            formThread.Name = "Form Thread";
            formThread.Start(form as object);

            if ( localUsersData.LoadData() )
                Console.WriteLine("local data restored!");
            else
                Console.WriteLine("local data NOT RESTORED, you idiot!");

            if ( ServerMethods.GetMe().ok ) {
                Console.WriteLine("fully operational");
                new Thread(Updater).Start();
                form.users = localUsersData.usersData.Values.ToList();
                ServerMethods.sendBroadMessage("I am back on\n<b>MV</b>", ServerMethods.parse_mode.HTML);
            } else
                Console.WriteLine("Telegram doesn't like you!\n(GetMe returned not ok)");
        }

        static void RunForm(object form) {
            try {
                Application.Run(form as Form);
            } catch ( Exception exception ) {
                MessageBox.Show("Form thread could not start\n" + exception.ToString(), "Form cannot start", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }
        public static void Close(object sender, FormClosingEventArgs e) {
            ServerMethods.sendBroadMessage("I will be off for a while\n<b>MV</b>", ServerMethods.parse_mode.HTML);
            localUsersData.SaveData();
            Environment.Exit(0);
        }

        /*//use this to START the Updarer loop*/
        static void Updater() {
            while ( run ) {
                form.Working = true;
                ServerMethods.GetUnreadUpdates();
                form.Working = false;
                Thread.Sleep(Settings.Default.waitTime);
            }
        }
    }
}
