using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using JsonTypes;
using System.Web;
using System.Net;
using System.Web.Script.Serialization;

namespace myTelegramBot {
    public class ServerMethods {

        const string token = "225365907:AAE0D5dgDjzLlwHp5jLVVZMBmRZyPBFpepI";
        const string website = "https://api.telegram.org/bot"+token+"/";
        
        public enum parse_mode { Markdown, HTML }
        static List<string> restart_phrases = new List<string>() {"I was just waiting this", "I couldn't wait anymore!", "Got it...\nGet ready.." };
        static List<string> HTMLstyle = new List<string>()  { "b", "strong", "i", "em", "a", "code", "pre" };
        static int lastUpdate=0;
        static Dictionary<int, Userdata> usersData = localUsersData.usersData;

        static public getUpdates GetUpdates(int limit = 100) {
            string argument = string.Format("?limit={0}", limit);
            return GetUpdates(argument);
        }
        static public getUpdates GetUpdates(int offset, int limit = 100) {
            string argument = string.Format("?offset={0}&limit={1}", offset, limit);
            return GetUpdates(argument);
        }
        static public getUpdates GetUnreadUpdates(int limit = 100) {
            string argument = string.Format("?offset={0}&limit={1}", lastUpdate+1, limit);
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
            string argument = string.Format("?offset={0}&limit={1}", usersData[chat_id].lastUpdate+1, limit);
            getUpdates getUpdates = GetUpdates(argument);
            for ( int n = getUpdates.result.Count - 1 ; n >= 0 ; n-- )
                if ( getUpdates.result[n].message.chat.id != chat_id )
                    getUpdates.result.RemoveAt(n);
            return getUpdates;
        }
        static getUpdates GetUpdates(string argument) {
            string response = new WebClient().DownloadString(website + "getUpdates" + argument);
            getUpdates getUpdates = new JavaScriptSerializer().Deserialize<getUpdates>(response);
            
            for ( int n = 0 ; n < getUpdates.result.Count ; n++ ) {
                Update update = getUpdates.result[n];
                //eventually add to localUsers FIRST
                localUsersData.AddUser(update.message.chat);
                //parse the message to check if contains commands
                ParseMessage(update.message);
                //update lastUpdate
                if ( update.update_id > usersData[update.message.chat.id].lastUpdate )
                    usersData[update.message.chat.id].lastUpdate = update.update_id;
                if ( update.update_id > lastUpdate )
                    lastUpdate = update.update_id;
            }
            return getUpdates;
        }

        static public getMe GetMe() {
            string response = new WebClient().DownloadString(website + "getMe");
            return new JavaScriptSerializer().Deserialize<getMe>(response);
        }

        static public Message sendMessage(int chat_id, string text, int reply_to_message_id = -1) {
            TextCleaner(ref text);
            string argument = string.Format("?chat_id={0}&text= {1}", chat_id, text);
            if ( reply_to_message_id != -1 )
                argument += "&reply_to_message_id=" + reply_to_message_id;
            return sendMessage(argument);
        }
        static public Message sendMessage(int chat_id, string text, parse_mode parse_mode, int reply_to_message_id = -1) {
            TextCleaner(ref text);
            string argument = string.Format("?chat_id={0}&text={1}&parse_mode={2}", chat_id, text, parse_mode);
            if ( reply_to_message_id != -1 )
                argument += "&reply_to_message_id=" + reply_to_message_id;
            return sendMessage(argument);
        }
        static public Message sendMessage(int chat_id, string text, bool disable_notification, int reply_to_message_id = -1) {
            TextCleaner(ref text);
            string argument = string.Format("?chat_id={0}&text={1}&disable_notification={2}", chat_id, text, disable_notification);
            if ( reply_to_message_id != -1 )
                argument += "&reply_to_message_id=" + reply_to_message_id;
            return sendMessage(argument);
        }
        static public Message sendMessage(int chat_id, string text, parse_mode parse_mode, bool disable_notification, int reply_to_message_id = -1) {
            TextCleaner(ref text);
            string argument = string.Format("?chat_id={0}&text={1}&parse_mode={2}&disable_notification={3}", chat_id, text, parse_mode, disable_notification);
            if ( reply_to_message_id != -1 )
                argument += "&reply_to_message_id=" + reply_to_message_id;
            return sendMessage(argument);
        }
        static public List<Message> sendBroadMessage(string text) {
            List<Message> messagges = new List<Message>();
            TextCleaner(ref text);
            foreach ( int chat_id in usersData.Keys ) {
                string argument = string.Format("?chat_id={0}&text={1}", chat_id, text);
                messagges.Add(sendMessage(argument));
            }
            return messagges;
        }
        static public List<Message> sendBroadMessage(string text, parse_mode parse_mode) {
            List<Message> messagges = new List<Message>();
            TextCleaner(ref text);
            foreach ( int chat_id in usersData.Keys ) {
                string argument = string.Format("?chat_id={0}&text={1}&parse_mode={2}", chat_id, text, parse_mode);
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
            string response = new WebClient().DownloadString(website + "sendMessage" + argument);
            return new JavaScriptSerializer().Deserialize<Message>(response);
        }

        static public getChat getChat(int chat_id) {
            string argument = "?chat_id=" + chat_id;
            string response = new WebClient().DownloadString(website + "getChat" + argument);
            return new JavaScriptSerializer().Deserialize<getChat>(response);
        }

        //STUFF

        static public void ParseMessage(Message message) {
            if ( message.entities == null || message.entities.Count == 0 )
                return;

            bool foundOne = false;
            foreach ( MessageEntity entity in message.entities )
                if ( entity.type == MessageEntityTypes.bot_command.ToString() )
                    if ( !foundOne )
                        foundOne = true;
                    else {
                        sendMessage(message.chat.id, "I'm not so clever to handle multiple commands\nUse one by time, please", message.message_id);
                        return;
                    }

            string command = message.text.Substring(message.entities[0].offset, message.entities[0].length);

            switch ( command ) {
                //the following cases are the commands allowed in the bot
                case "/start":
                    sendMessage(message.chat.id, "REMEMBER THIS BOT IS STILL IN DEVELOPMENT AND IS NOT WORKING BY NOW\nIf you keep this chat YOU WILL ME NOTIFIED AS SOON AS READY\n\nYou will receive a message at random times. Write something back as soon as you can!");
                    break;
                case "/faster":
                    usersData[message.chat.id].speed += 1;
                    sendMessage(message.chat.id, "Speed is now set to " + usersData[message.chat.id].speed + " of 5");
                    break;
                case "/slower":
                    usersData[message.chat.id].speed -= 1;
                    sendMessage(message.chat.id, "Speed is now set to " + usersData[message.chat.id].speed + " of 5");
                    break;
                case "/pause":
                    usersData[message.chat.id].active = false;
                    sendMessage(message.chat.id, "I will take a break...", true);
                    break;
                case "/write":
                    usersData[message.chat.id].active = true;
                    sendMessage(message.chat.id, restart_phrases[new Random().Next(0, restart_phrases.Count)]);
                    break;
            }
        }
    }
}