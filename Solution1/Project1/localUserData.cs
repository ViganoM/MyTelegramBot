﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using System.Timers;
using System.Windows.Forms;
using JsonTypes;
using myTelegramBot.Properties;


namespace myTelegramBot {
    public enum activity { WaitingSend, WaitingResponse, Inactive }
    static class localUsersData {
        static string now {
            get { return DateTime.Now.ToString(Settings.Default.datetimeFormat); }
        }
        static string LoadPath {
            get {
                if ( !Directory.Exists(Settings.Default.DataPath) )
                    Directory.CreateDirectory(Settings.Default.DataPath);
                return Settings.Default.DataPath + Settings.Default.DataFileName + Settings.Default.lastDataFileName;
            }
        }
        static string SavePath {
            get {
                if ( !Directory.Exists(Settings.Default.DataPath) )
                    Directory.CreateDirectory(Settings.Default.DataPath);
                return Settings.Default.DataPath + Settings.Default.DataFileName;
            }
        }
        static object lockSaving = new object();

        public static Dictionary<int, Userdata> usersData { get; private set; } = new Dictionary<int, Userdata>();

        public static bool AddUser(Chat chat, User user) {
            if ( usersData.ContainsKey(chat.id) )
                return false;

            Userdata userdata = new Userdata(chat, user);
            usersData.Add(chat.id, userdata);
            Program.form.SetUser(userdata);
            Program.form.WriteToConsole("User " + user.username + " added", Color.Violet);
            return true;
        }

        public static void UpdateForm() {
            if ( usersData.Count > 0 )
                Program.form.SetUsers(usersData.Values.ToList());
            else
                Program.form.SetUsers(new List<Userdata>());
        }

        public static void SaveData() {
            try {
                string end = now;
                lock ( lockSaving )
                    new BinaryFormatter().Serialize(new FileStream(SavePath + end, FileMode.Create, FileAccess.Write), usersData);
                Settings.Default.lastDataFileName = end;
            } catch ( Exception exception ) {
                MessageBox.Show("Settings could not be saved.\nA new attempt will be made 1 second after this box will be closed.\nSettings will be saved in folder:\n" + Settings.Default.DataPath + "\nin a file whose name starts with:\n" + Settings.Default.DataFileName + "\nplus date and time of saving, in format:\n" + Settings.Default.datetimeFormat + "\nplus:\n_2nd\n\n" + exception.ToString(), "Settings not saved", MessageBoxButtons.OK, MessageBoxIcon.Error);
                try {
                    Thread.Sleep(1000);
                    string end = now;
                    lock ( lockSaving )
                        new BinaryFormatter().Serialize(new FileStream(SavePath + end + "_2nd", FileMode.Create, FileAccess.Write), usersData);
                    Settings.Default.lastDataFileName = end;

                    MessageBox.Show("Settings were eventually saved at:\n" + Settings.Default.DataPath + end + "_2nd\nPlease manually manage the settings files", "Settings eventually saved", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    Process.Start(Settings.Default.DataPath);
                } catch ( Exception exception2 ) {
                    MessageBox.Show("Settings were not be saved.\nData will be lost\n" + exception2.ToString(), "Settings definitively not saved", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            Settings.Default.lastUpdate = ServerMethods.lastUpdate;
            Settings.Default.Save();

        }
        public static bool LoadData(bool reset = false) {
            if ( reset ) {
                Settings.Default.Reset();
                Settings.Default.Save();
                Environment.Exit(0);
            }

            ServerMethods.lastUpdate = Settings.Default.lastUpdate;
            Program.form.WriteToConsole("Last Update setting restored;\nLast update was " + ServerMethods.lastUpdate, Color.Green);

            try {
                lock ( lockSaving )
                    usersData = new BinaryFormatter().Deserialize(new FileStream(LoadPath, FileMode.Open, FileAccess.Read)) as Dictionary<int, Userdata>;
                Program.form.WriteToConsole("Settings succesfully restored from " + LoadPath, Color.Green);
                #region update non serialized stuff
                foreach ( Userdata userdata in usersData.Values )
                    userdata.Restore();

                #endregion
                return true;
            } catch ( FileNotFoundException fileNotFound ) {
                Program.form.WriteToConsole("Settings file not found", Color.Red);
                MessageBox.Show("Settings not restored because the file could not be found:\n" + LoadPath + "\n\n" + fileNotFound.ToString(), "Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            } catch ( Exception exception ) {
                //TODO allow the choise of a file and load it
                Program.form.WriteToConsole("Settings not restored from " + LoadPath, Color.Red);
                MessageBox.Show("Settings not restored\n" + exception.ToString(), "Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }   //TODO load last saving if failsS
    }

    [Serializable]
    public class Userdata {
        public Chat chat { get; private set; }
        public User user { get; private set; }
        public Bitmap photo { get; private set; }
        public readonly DateTime joinDate;
        DateTime lastUserUpdate = new DateTime(0);
        List<double > _response = new List<double>();
        public List<double> response {
            get {
                if ( _response.Count == 0 )
                    return new List<double>() { 0 };
                return _response;
            }
            private set { _response = value; }
        }
        public int supportRequests = 0;

        public int lastUpdate { get; private set; } = 0;
        int _speed = 3;
        public int speed {
            get { return _speed; }
            set {
                if ( value >= 1 && value <= 5 )
                    _speed = value;
                Program.form.SetUser(this);
            }
        }
        activity _activity = activity.WaitingSend;
        public activity activity {
            get { return _activity; }
            set {
                _activity = value;
                if ( activity == activity.Inactive )
                    timer.Stop();
                else if ( activity == activity.WaitingSend )
                    Next();

                Program.form.SetUser(this);
            }
        }
        public bool notificate=true;

        [NonSerialized]
        System.Timers.Timer timer = new System.Timers.Timer();
        DateTime nextMessageSent;
        DateTime lastMessageSent;


        public Userdata(Chat chatItem, User userItem) {
            chat = chatItem;
            user = userItem;
            photo = ServerMethods.getUserPhoto(user.id);
            joinDate = DateTime.Now;
            timer.Elapsed += new ElapsedEventHandler(Send);
            timer.AutoReset = false;
            Next();
        }
        public void Restore() {
            response = new List<double>();
            timer = new System.Timers.Timer();
            timer.Elapsed += new ElapsedEventHandler(Send);
            timer.AutoReset = false;
            if ( activity == activity.WaitingSend ) {
                if ( nextMessageSent - DateTime.Now <= new TimeSpan(0) )
                    Next(true);
                else {
                    timer.Interval = (nextMessageSent - DateTime.Now).TotalMilliseconds;
                    timer.Start();
                    Program.form.WriteToConsole("Message scheduled for " + nextMessageSent.ToString("HH\\:mm\\.ss"), Color.Blue);
                }
            }

        }
        void Update(JsonTypes.Message message) {
            chat = message.chat;
            user = message.from;
            photo = ServerMethods.getUserPhoto(user.id);
            lastUserUpdate = DateTime.Now;
        }

        public void MessageInput(Update update) {

            lastUpdate = update.message.message_id;
            if ( activity == activity.WaitingResponse ) {
                activity = activity.WaitingSend;
                TimeSpan delta = DateTime.Now - lastMessageSent;
                response.Add(delta.TotalSeconds);
                ServerMethods.sendMessage(chat.id, "Got it! You took " + delta.TotalSeconds.ToString("#.0") + " seconds.");
            } else if ( activity == activity.WaitingSend )
                ServerMethods.sendMessage(chat.id, "What do you mean?\nUse commands or wait my next message", false, update.message.message_id);
            else if ( activity == activity.Inactive )
                ServerMethods.sendMessage(chat.id, "Currently I am not sending you messagges. Use \\write to receive messagges", false);

            //user update
            if ( DateTime.Now - lastUserUpdate > new TimeSpan(Settings.Default.updateUserTime, 0, 0, 0) )
                Update(update.message);
        }
        void Next(bool soon = false) {
            if ( activity == activity.WaitingSend ) {
                double addSeconds = 0;
                Random random = new Random();
                if ( soon )
                    addSeconds = random.NextDouble() * 3600;    //between 0 and 1 hour
                else
                    switch ( _speed ) {
                        case 1:
                            addSeconds = (random.Next(25, 51) + random.NextDouble()) * 3600;
                            break;
                        case 2:
                            addSeconds = (random.Next(20, 41) + random.NextDouble()) * 3600;
                            break;
                        case 3:
                            addSeconds = (random.Next(15, 31) + random.NextDouble()) * 3600;
                            break;
                        case 4:
                            addSeconds = (random.Next(10, 21) + random.NextDouble()) * 3600;
                            break;
                        case 5:
                            addSeconds = (random.Next(5, 11) + random.NextDouble()) * 3600;
                            break;
                    }
                timer.Interval = addSeconds * 1000;
                nextMessageSent = DateTime.Now.AddSeconds(addSeconds);
                timer.Start();
                Program.form.WriteToConsole("Message scheduled for " + nextMessageSent.ToString("HH\\:mm\\.ss"), Color.Blue);
            }
        }
        void Send(object sender, ElapsedEventArgs e) {
            ServerMethods.sendMessage(chat.id, "Answer this!\nSent: " + DateTime.Now.ToString("HH:mm.ss"), !notificate);
            lastMessageSent = DateTime.Now;
            activity = activity.WaitingResponse;
            Program.form.WriteToConsole("Scheduled message sent", Color.Blue);
        }

    }
}
