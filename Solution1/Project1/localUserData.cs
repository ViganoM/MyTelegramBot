using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JsonTypes;
using myTelegramBot.Properties;

namespace myTelegramBot {
    static class localUsersData {
        public static List<Userdata> usersData { get; private set; }

        public static bool Load() {
            if ( Settings.Default.localData == null ) {
                Settings.Default.localData =
                return false;
            }
            usersData = Settings.Default.localData;
            return true;
        }
        public static bool AddUser(Chat chat) {
            foreach ( Userdata userdata in usersData )
                if ( userdata.chat.id == chat.id )
                    return false;
            usersData.Add(new Userdata(chat));
            return true;
        }
    }

    class Userdata {
        public Chat chat { get; }
        public List<Message> messagges { get; set; }

        public Userdata(Chat chatItem) {
            chat = chatItem;
        }
    }
}
