using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JsonTypes;
using myTelegramBot.Properties;
using System.ComponentModel;
using System.Globalization;

namespace myTelegramBot {
    static class localUsersData {
        public static Dictionary<int, Userdata> usersData { get; private set; } = new Dictionary<int, Userdata>();

        public static bool AddUser(Chat chat) {
            if ( usersData.ContainsKey(chat.id) )
                return false;
            usersData.Add(chat.id, new Userdata(chat));
            return true;
        }

        public static void SaveData() {
            foreach ( int key in usersData.Keys ) {
                //list
                Settings.Default.setting.Add(key);
                usersData[key].lastSave = DateTime.Now;
                Settings.Default.localData.userdata = usersData;
            }
            ShowData();
            Settings.Default.Save();
        }
        public static bool LoadData() {
            //list
            if ( Settings.Default.setting == null ) {
                Settings.Default.setting = new List<int>();
                Settings.Default.Save();
            }
            if ( Settings.Default.localData == null ) {
                Settings.Default.localData = new LocalUsers();
                Settings.Default.Save();
                System.Windows.Forms.MessageBox.Show(Settings.Default.localData.ToString());
                ShowData();
                return false;
            }
            usersData = Settings.Default.localData.userdata;
            return true;
        }
        public static void ShowData() {
            string show = "";
            foreach ( Userdata userdata in Settings.Default.localData.userdata.Values )
                show += userdata.chat.id.ToString() + "\n";

            System.Windows.Forms.MessageBox.Show(show);
        }
    }

    [Serializable]
    public class Userdata {
       public Chat chat { get; set; }
        public DateTime lastSave = new DateTime(0);

        public int lastUpdate = 0;
        public int _speed = 3;
        public int speed {
            get { return _speed; }
            set {
                if ( _speed > 1 && _speed < 5 )
                    _speed = value;
            }
        }
        public bool active = true;

        public Userdata(Chat chatItem) {
            chat = chatItem;
        }
        Userdata() {
            chat = new Chat();
            lastSave = new DateTime(0);
            lastUpdate = 0;
            _speed = 3;
            speed = 3;
            active = true;
        }

    }
}
