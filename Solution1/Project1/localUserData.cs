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

namespace myTelegramBot {
    static class localUsersData {
        public static Dictionary<int, Userdata> usersData { get; private set; } = new Dictionary<int, Userdata>();

        public static bool AddUser(Chat chat, User user) {
            if ( usersData.ContainsKey(chat.id) )
                return false;
            usersData.Add(chat.id, new Userdata(chat, user));
            return true;
        }

        public static void SaveData() {
            new BinaryFormatter().Serialize(new FileStream(Settings.Default.DataPath, FileMode.OpenOrCreate, FileAccess.Write), usersData);
        }
        public static bool LoadData(bool reset=false) {
            if ( reset ) {
                Settings.Default.Reset();
                Settings.Default.Save();
                Environment.Exit(0);
            }
            if ( !System.IO.File.Exists(Settings.Default.DataPath) ) {
                MessageBox.Show("Settings file not found at:\n" + Settings.Default.DataPath, "Settings not restored", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            try {
                usersData = new BinaryFormatter().Deserialize(new FileStream(Settings.Default.DataPath, FileMode.Open, FileAccess.Read)) as Dictionary<int, Userdata>;
            } catch (Exception exception ) {
                MessageBox.Show("Settings not restored\n" + exception.ToString(), "Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }
        public static void ShowData() {
            string show = "";
            foreach ( int chat_id in usersData.Keys )
                show += chat_id.ToString() + "\n";
            MessageBox.Show(show);
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
                if ( _speed > 1 && _speed < 5 )
                    _speed = value;
            }
        }
        public bool active = true;

        public Userdata(Chat chatItem, User userItem) {
            chat = chatItem;
            user = userItem;
            photo = ServerMethods.getUserPhoto(user.id);
            joinDate = DateTime.Now;
        }
    }
}
