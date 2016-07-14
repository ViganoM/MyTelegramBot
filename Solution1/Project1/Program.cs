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
            #region Run Form
            Thread formThread = new Thread(RunForm);
            formThread.Name = "Form Thread";
            formThread.Start();
            formCreated.WaitOne();
            formCreated = null; //this is not supposed to be used anymore
            form.WaitingTime = Settings.Default.updateLoopSleepDuration_ms;   //HACK this creates problems
            #endregion

            if ( localUsersData.LoadData() )
                form.WriteToConsole("Local data restored", System.Drawing.Color.Green);
            else
                form.WriteToConsole("Local data NOT RESTORED, you idiot!", System.Drawing.Color.Red);
            localUsersData.DeleteOld();

            if ( ServerMethods.GetMe().ok ) {
                form.WriteToConsole("FULLY OPERATIONAL", System.Drawing.Color.Green);
                new Thread(Updater).Start();
                if ( localUsersData.usersData.Count > 0 )
                    form.SetUsers(localUsersData.usersData.Values.ToList());
                form.Loading = false;
                ServerMethods.sendBroadMessage("I am back on\n<b>MV</b>", true);
                form.WriteToConsole("Welcome message sent", System.Drawing.Color.Blue);
            } else
                form.WriteToConsole("Telegram doesn't like you!\n(GetMe returned not ok)", System.Drawing.Color.Red);

        }

        static void RunForm() {
            form = new Form(formCreated);
            try {
                Application.Run(form);
            } catch ( Exception exception ) {
                File.AppendAllLines(localUsersData.LogFilepath, exception.ToString().Split(new string[] { "\n" }, StringSplitOptions.None).ToArray());
                //MessageBox.Show("Form thread could not start\n" + exception.ToString(), "Form cannot start", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw;
            }
            //code after Application.Run is not executed!
        }
        public static void Close(object sender, FormClosingEventArgs e) {
            ServerMethods.sendBroadMessage("I will be off for a while\n<b>MV</b>", true);
            localUsersData.SaveData();
            Environment.Exit(0);
        }

        /*//use this to START the Updarer loop*/
        static void Updater() {
            Stopwatch chrono = new Stopwatch();
            while ( run ) {
                form.Working = true;
                chrono.Restart();
                ServerMethods.GetUnreadUpdates();
                form.WorkingTime = chrono.ElapsedMilliseconds;
                form.Working = false;
                Thread.Sleep(Settings.Default.updateLoopSleepDuration_ms);
            }
        }
    }
}