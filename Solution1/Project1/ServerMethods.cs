using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using JsonTypes;
using System.Web;
using System.Net;
using System.Web.Script.Serialization;


namespace Project1 {
    class ServerMethods {

        const string token = "225365907:AAE0D5dgDjzLlwHp5jLVVZMBmRZyPBFpepI";
        const string website = "https://api.telegram.org/bot"+token+"/";

        public static Dictionary<int, int> lastUpdate { get; private set; } = new Dictionary<int, int>();

        public enum parse_mode { Markdown, HTML }

        static public getUpdates GetUpdates(int limit = 100) {
            string argument = string.Format("?limit={0}", limit);
            return GetUpdates(argument);
        }
        static public getUpdates GetUpdates(int offset, int limit = 100) {
            string argument = string.Format("?offset={0}&limit={1}", offset, limit);
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
        static public Update GetLastUpdateByChat(int chat_id, bool veryLast = false) {
            string argument;
            if ( veryLast )
                argument = string.Format("?offset={0}", lastUpdate[chat_id] + 1);
            else
                argument = string.Format("?offset={0}", lastUpdate[chat_id]);
            List<Update> UpdateList = GetUpdates(argument).result;
            for ( int n = UpdateList.Count - 1 ; n >= 0 ; n-- )
                if ( UpdateList[n].message.chat.id != chat_id )
                    UpdateList.RemoveAt(n);
            if ( UpdateList.Count > 1 )
                throw new Exception("too many Update objects returning in GetLastUpdateByChat method");
            if ( UpdateList.Count == 0 )
                return null;
            return UpdateList[UpdateList.Count - 1];
        }
        static getUpdates GetUpdates(string argument) {
            string response = new WebClient().DownloadString(website + "getUpdates" + argument);
            getUpdates getUpdates = new JavaScriptSerializer().Deserialize<getUpdates>(response);
            for ( int n = getUpdates.result.Count - 1 ; n >= 0 ; n-- ) {
                Update update = getUpdates.result[n];
                if ( !lastUpdate.ContainsKey(update.message.chat.id) )
                    lastUpdate.Add(update.message.chat.id, update.update_id);
                if ( update.update_id > lastUpdate[update.message.chat.id] )
                    lastUpdate[update.message.chat.id] = update.update_id;
            }
            return getUpdates;
        }

        static public getMe GetMe() {
            string response = new WebClient().DownloadString(website + "getMe");
            return new JavaScriptSerializer().Deserialize<getMe>(response);
        }

        static public Message sendMessage(int chat_id, string text, int reply_to_message_id = -1) {
            string argument = string.Format("?chat_id={0}&text= {1}", chat_id, text);
            if ( reply_to_message_id != -1 )
                argument += "&reply_to_message_id=" + reply_to_message_id;
            return sendMessage(argument);
        }
        static public Message sendMessage(int chat_id, string text, parse_mode parse_mode, int reply_to_message_id = -1) {
            string argument = string.Format("?chat_id={0}&text={1}&parse_mode={2}", chat_id, text, parse_mode);
            if ( reply_to_message_id != -1 )
                argument += "&reply_to_message_id=" + reply_to_message_id;
            return sendMessage(argument);
        }
        static public Message sendMessage(int chat_id, string text, bool disable_notification, int reply_to_message_id = -1) {
            string argument = string.Format("?chat_id={0}&text={1}&disable_notification={2}", chat_id, text, disable_notification);
            if ( reply_to_message_id != -1 )
                argument += "&reply_to_message_id=" + reply_to_message_id;
            return sendMessage(argument);
        }
        static public Message sendMessage(int chat_id, string text, parse_mode parse_mode, bool disable_notification, int reply_to_message_id = -1) {
            string argument = string.Format("?chat_id={0}&text={1}&parse_mode={2}&disable_notification={3}", chat_id, text, parse_mode, disable_notification);
            if ( reply_to_message_id != -1 )
                argument += "&reply_to_message_id=" + reply_to_message_id;
            return sendMessage(argument);
        }
        static Message sendMessage(string argument) {
            string response = new WebClient().DownloadString(website + "sendMessage" + argument);
            return new JavaScriptSerializer().Deserialize<Message>(response);
        }
    }
}