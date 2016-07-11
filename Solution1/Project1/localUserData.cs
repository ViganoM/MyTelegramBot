using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JsonTypes;
using myTelegramBot.Properties;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.Serialization;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;
using System.Drawing;
using System.Threading;
using System.Diagnostics;
using System.Security.Permissions;


namespace myTelegramBot {
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
        }
    }

    [Serializable]
    public class Userdata {
        public Chat chat { get; private set; }  //to keep updated
        public User user { get; private set; }
        public Bitmap photo { get; private set; }
        public readonly DateTime joinDate;
        public DateTime lastSave = new DateTime(0);

        public int lastUpdate = 0;
        int _speed = 3;
        public int speed {
            get { return _speed; }
            set {
                if ( value >= 1 && value <= 5 )
                    _speed = value;
                Program.form.SetUser(this);
            }
        }
        bool _active =true;
        public bool active {
            get { return _active; }
            set {
                _active = value;
                Program.form.SetUser(this);
            }
        }

        public Userdata(Chat chatItem, User userItem) {
            chat = chatItem;
            user = userItem;
            photo = ServerMethods.getUserPhoto(user.id);
            joinDate = DateTime.Now;
        }
    }
}
