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

        static Dictionary<int,int> lastUpdate = new Dictionary<int, int>();

        public enum parse_mode { Markdown, HTML }

        static public getUpdates GetUpdates(int limit = 100) {
            string argument = string.Format("?limit={0}", limit);
            return GetUpdates(argument);
        }
        static public getUpdates GetUpdates(int offset, int limit = 100) {
            string argument = string.Format("?offset={0}&limit={1}", offset, limit);
            return GetUpdates(argument);
        }
        static public getUpdates GetUpdatesbyChat(int chat_id, int limit=100) {
            string argument = string.Format("?limit={0}", limit);
            getUpdates getUpdates = GetUpdates(argument).result;
            for (int n=getUpdates.result.cou

        } 
        static public getUpdates GetLastUpdatebyChat(int chat_id) {

        }
        static getUpdates GetUpdates(string argument) {
            string response = new WebClient().DownloadString(website + "getUpdates" + argument);
            getUpdates getUpdates = new JavaScriptSerializer().Deserialize<getUpdates>(response);
            foreach ( Update update in getUpdates.result ) {
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
            string argument = string.Format("?chat_id={0}&text={1}", chat_id, text);
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