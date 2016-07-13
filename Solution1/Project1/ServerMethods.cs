using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using JsonTypes;
using System.Web;
using System.Net;
using System.Web.Script.Serialization;
using myTelegramBot.Properties;
using System.Drawing;
using System.Linq;
// System.Windows.Forms; cannot be used because it has many Types in common with JsonTypes 

namespace myTelegramBot {
    public class ServerMethods {

        const string token = "225365907:AAE0D5dgDjzLlwHp5jLVVZMBmRZyPBFpepI";
        const string website = "https://api.telegram.org/bot"+token+"/";
        const string fileWebsite = "https://api.telegram.org/file/bot" + token+"/";
        const int Developer_chat_id = 157874244;

        public enum parse_mode { Markdown, HTML }
        static List<string> restart_phrases = new List<string>() {"I was just waiting this", "I couldn't wait anymore!", "Got it...\nGet ready.." };
        static List<string> HTMLstyle = new List<string>()  { "b", "strong", "i", "em", "a", "code", "pre" };
        public static int lastUpdate = 0;

        /*GetUpdates
        static public getUpdates GetUpdates(int limit = 100) {
            string argument = string.Format("?limit={0}", limit);
            return GetUpdates(argument);
        }
        static public getUpdates GetUpdates(int offset, int limit = 100) {
            string argument = string.Format("?offset={0}&limit={1}", offset, limit);
            return GetUpdates(argument);
        }
        */
        static public getUpdates GetUnreadUpdates(int limit = 100) {
            string argument = string.Format("?offset={0}&limit={1}", lastUpdate + 1, limit);
            return GetUpdates(argument);
        }
        static public getUpdates GetUpdatesByChat(int chat_id, int limit = 100) {
            string argument = string.Format("?limit={0}", limit);
            getUpdates getUpdate = GetUpdates(argument);
            for ( int n = getUpdate.result.Count - 1 ; n >= 0 ; n-- )
                if ( getUpdate.result[n].message.chat.id != chat_id )
                    getUpdate.result.RemoveAt(n);
            return getUpdate;
        }
        static public getUpdates GetUnreadUpdatesByChat(int chat_id, int limit = 100) {
            string argument = string.Format("?offset={0}&limit={1}", localUsersData.usersData[chat_id].lastUpdate + 1, limit);
            getUpdates getUpdates = GetUpdates(argument);
            for ( int n = getUpdates.result.Count - 1 ; n >= 0 ; n-- )
                if ( getUpdates.result[n].message.chat.id != chat_id )
                    getUpdates.result.RemoveAt(n);
            return getUpdates;
        }
        static getUpdates GetUpdates(string argument) {
            string response = "";
            try {
                response = new WebClient().DownloadString(website + "getUpdates" + argument);
            } catch ( WebException exception ) {
                string path = localUsersData.SavePath + "_log" + DateTime.Now.ToString(Settings.Default.datetimeFormat);
                System.IO.File.WriteAllText(path, exception.ToString());
                Program.form.WriteToConsole("A WebException occoured in GetUpdate. Informations were written in file " + path, Color.Red);
                return new getUpdates();
            }
            getUpdates getUpdates = new JavaScriptSerializer().Deserialize<getUpdates>(response);
            for ( int n = 0 ; n < getUpdates.result.Count ; n++ ) {

                Update update = getUpdates.result[n];

                //eventually add to localUsers FIRST
                localUsersData.AddUser(update.message.chat, update.message.from);
                //parse the message to check if contains commands
                if ( !ParseMessage(update.message) )
                    //give info to userdata of sender
                    localUsersData.usersData[update.message.chat.id].MessageInput(update);
                //update ServerMethods.lastUpdate
                if ( update.update_id > lastUpdate )
                    lastUpdate = update.update_id;
            }
            return getUpdates;
        }

        static public getMe GetMe() {
            string response = new WebClient().DownloadString(website + "getMe");
            return new JavaScriptSerializer().Deserialize<getMe>(response);
        }

        static public Message sendMessage(int chat_id, string text, bool disable_notification = false, int reply_to_message_id = -1, parse_mode parse_mode = parse_mode.HTML) {
            TextCleaner(ref text);
            string argument = string.Format("?chat_id={0}&text={1}&disable_notification={2}&parse_mode={3}", chat_id, text, disable_notification, parse_mode);
            if ( reply_to_message_id != -1 )
                argument += "&reply_to_message_id=" + reply_to_message_id;
            return sendMessage(argument);
        }
        static public List<Message> sendBroadMessage(string text, bool disable_notification = false, parse_mode parse_mode = parse_mode.HTML) {
            List<Message> messagges = new List<Message>();
            TextCleaner(ref text);
            foreach ( int chat_id in localUsersData.usersData.Keys ) {
                string argument = string.Format("?chat_id={0}&text={1}&disable_notification={2}&parse_mode={3}", chat_id, text, disable_notification, parse_mode);
                messagges.Add(sendMessage(argument));
            }
            return messagges;
        }
        static public string TextCleaner(ref string text) {
            text.Replace("#", "");
            text = text.Replace("&", "");
            return text;
        }
        static Message sendMessage(string argument) {
            string response = "";
            try {
                response = new WebClient().DownloadString(website + "sendMessage" + argument);
            } catch ( WebException exception ) {
                System.Windows.Forms.MessageBox.Show("Cannot send a message because of a Web Exception\n\n" + exception.ToString(), "Message not sent", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Hand);
                while ( argument.Split('&').Length > 2 ) {
                    argument = argument.Replace(argument.Split('&').Last(x => x.Length > 0), "");
                    argument = argument.Remove(argument.Length - 1);
                    try {
                        response = new WebClient().DownloadString(website + "sendMessage" + argument);
                        System.Windows.Forms.MessageBox.Show("New attempt was succesfull\nArgument string used was:\n" + argument, "Message eventually sent", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
                        break;
                    } catch ( WebException exception2 ) {
                        System.Windows.Forms.MessageBox.Show("New attempt failed because of a Web Exception\nArgument string used was:\n" + argument + "\n\n" + exception2, "Message not sent again", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Hand);
                    }
                }
            }
            if ( response != "" )
                return new JavaScriptSerializer().Deserialize<Message>(response);
            else
                return new Message();
        }

        static public getChat getChat(int chat_id) {
            string argument = "?chat_id=" + chat_id;
            string response = new WebClient().DownloadString(website + "getChat" + argument);
            return new JavaScriptSerializer().Deserialize<getChat>(response);
        }

        /*Photos
        public static Bitmap getUserPhoto(int user_id) {
            PhotoSize photo = new PhotoSize();
            UserProfilePhotos userprofilephotos = getUserProfilePhotos(user_id);    //TODO photos are not deserialized!
            PhotoSize[][] photoListList = userprofilephotos.photos;
            if ( photoListList == null || photoListList.Length == 0 )
                return new Bitmap(Resources.MyIcon.ToBitmap());

            foreach ( PhotoSize p in photoListList.Last() )
                if ( photo.height < p.height )
                    photo = p;

            return new Bitmap(DownloadFile(getFile(photo.file_id).file_path));
        }

        static UserProfilePhotos getUserProfilePhotos(int user_id, int offset = 0, int limit = 100) {
            string argument = string.Format("?user_id={0}&offset={1}&limit={2}", user_id, offset, limit);
            string response = new WebClient().DownloadString(website + "getUserProfilePhotos" + argument);
            return new JavaScriptSerializer().Deserialize<UserProfilePhotos>(response);
        }
        static File getFile(string file_id) {
            string response = new WebClient().DownloadString(website + "getFile" + "?file_id=" + file_id);
            return new JavaScriptSerializer().Deserialize<File>(response);
        }
        static string DownloadFile(string file_path) {
            string path = Settings.Default.PhotoPath + file_path;
            int n = 0;
            while ( System.IO.File.Exists(path) )
                path += string.Format(" {0}", n++);

            new WebClient().DownloadFile(fileWebsite + file_path, path);
            return path;
        }
        */
        static public bool ParseMessage(Message message) {
            if ( message.entities == null || message.entities.Count == 0 )
                return false;

            bool foundOne = false;
            foreach ( MessageEntity entity in message.entities )
                if ( entity.type == MessageEntityTypes.bot_command.ToString() )
                    if ( !foundOne )
                        foundOne = true;
                    else {
                        sendMessage(message.chat.id, "I'm not so clever to handle multiple commands\nUse one by time, please", reply_to_message_id: message.message_id);
                        return false;
                    }

            string command = message.text.Substring(message.entities[0].offset, message.entities[0].length);
            Userdata user = localUsersData.usersData[message.chat.id];

            switch ( command ) {
                //the following cases are all the commands allowed in the bot
                case "/start":
                    sendMessage(message.chat.id, "REMEMBER THIS BOT IS STILL IN DEVELOPMENT AND IS NOT WORKING BY NOW\nIf you keep this chat YOU WILL ME NOTIFIED AS SOON AS READY\n\nYou will receive a message at random times. Write something back as soon as you can!");
                    break;
                case "/faster":
                    user.speed += 1;
                    sendMessage(message.chat.id, "Speed is now set to " + localUsersData.usersData[message.chat.id].speed + " of 5");
                    break;
                case "/slower":
                    user.speed -= 1;
                    sendMessage(message.chat.id, "Speed is now set to " + localUsersData.usersData[message.chat.id].speed + " of 5");
                    break;
                case "/pause":
                    user.activity = activity.Inactive;
                    sendMessage(message.chat.id, "I will take a break...\nYou will no longer receive messages until you enter the command /write", true);
                    break;
                case "/write":
                    if ( user.activity == activity.WaitingSend )
                        sendMessage(message.chat.id, "I am currently active. Sit down and wait my message.");
                    else {
                        user.activity = activity.WaitingSend;
                        sendMessage(message.chat.id, restart_phrases[new Random().Next(0, restart_phrases.Count)] + "\nSpeed is " + localUsersData.usersData[message.chat.id].speed);
                    }
                    break;
                case "/notificate":
                    user.notificate = !user.notificate;
                    if ( user.notificate )
                        sendMessage(message.chat.id, "Notifications are now active");
                    else
                        sendMessage(message.chat.id, "Notifications are now disabled. Be carefull!");
                    break;
                case "/support":
                    user.supportRequests++;
                    sendMessage(Developer_chat_id, "An User request your attention!\n" + message.text, true);
                    sendMessage(Developer_chat_id, string.Format("Details:\nchat_id: {0}\nuser_id: {1}\nUsername: {2}\nFirst name: {3}\nLast name: {4}\nJoin_date: {5}\nResponse_n: {6}\nResponse_avg: {7}\nSpeed: {8}\nNotificate: {9}\nSupport_req: {10}", message.chat.id, message.from.id, message.from.username, message.from.first_name, message.from.last_name, user.joinDate, user.response.Count, user.response.Average(), user.speed, user.notificate, user.supportRequests), true);
                    sendMessage(message.chat.id, "A message was just sent to the developer.\nHe is magnanimous and will personally contact you.");
                    break;
                case "/stats":
                    user.SendStats();
                    break;
                default:
                    sendMessage(message.chat.id, "Unrecognized command!", reply_to_message_id: message.message_id);
                    break;
            }
            return true;
        }
    }
}