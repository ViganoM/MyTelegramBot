using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using JsonTypes;
using Task;
using myTelegramBot.Properties;

namespace myTelegramBot {
    public class Program {
        /*//use this to STOP Updater loop*/
        public static bool run { private get; set; }


        static void Main(string[] args) {
            if ( ServerMethods.GetMe().ok )
                Updater();
            else
                Console.WriteLine("Telegram doesn't like you!\n(GetMe returned not ok)");

        }

        /*//use this to START the Updarer loop*/
        public static void Updater() {
            ServerMethods.GetUpdates(0, 1000);
            run = true;
            while ( true ) {
                if ( ServerMethods.lastUpdate.Count == 0 )
                    ServerMethods.GetUpdates(0, 1000);
                List<int> keys = ServerMethods.lastUpdate.Keys.ToList();

                foreach ( int chat_id in keys )
                    if ( ServerMethods.GetLastUpdateByChat(chat_id, true) != null ) {
                        string messagetext = ServerMethods.GetLastUpdateByChat(chat_id).message.text;
                        ServerMethods.sendMessage(chat_id, messagetext + "\n<b>MV</b>", ServerMethods.parse_mode.HTML);
                        new Thread(new ParameterizedThreadStart(new Program().Anagram)).Start(new Tuple<string, int>(messagetext, chat_id));
                        Chat chat = ServerMethods.getChat(chat_id).result;
                        Console.WriteLine(string.Format("{0} ({1} {2}) wrote: {3} (in a {4} chat)", chat.username, chat.first_name, chat.last_name, messagetext, chat.type));
                    }
                Thread.Sleep(Settings.Default.waitTime);
            }
        }

        void Anagram(object data) {
            string word = ((Tuple<string, int>) data).Item1;
            int chat_id = ((Tuple<string, int>) data).Item2;

            string message = "hahahahah.\nNO";
            switch ( word.Length ) {
                case 1:
                    message = "WTF?";
                    break;
                case 2:
                    message = "Your dubmness is considerable!";
                    break;
                case 3:
                    message = "It's 6 combinations\nThey were calculated before this message was sent";
                    break;
                case 4:
                    message = "It's 24 combinations\nThey were calculated before you received this message";
                    break;
                case 5:
                    message = "It's 120 combinations\\nIt might take a bit";
                    break;
                case 6:
                    message = "It's 720 combinations\nIt may take a while";
                    break;
                case 7:
                    message = "It's 5040 combinations ...\nYou have to wait";
                    break;
                case 8:
                    message = "It's more than 40,000 combinations!\n There are faster ways to do this...";
                    break;
                case 9:
                    message = "Sit down and get ready to be bombarded";
                    break;
            }

            ServerMethods.sendMessage(chat_id, message);

            if ( word.Length >= 10 || word.Length == 0 )
                return;

            Anagram anagram = new Anagram();
            AutoResetEvent wait = new AutoResetEvent(false);

            anagram.StartAnagram((string) word, Settings.Default.dictionaryPath, wait);

            wait.WaitOne();

            List<string> result = anagram.GetResult();
            ServerMethods.sendMessage(chat_id, "I've got " + result.Count + "results!");
            foreach ( string s in result )
                ServerMethods.sendMessage(chat_id, s, true);

            ServerMethods.sendMessage(chat_id, "Anagrams of " + word.ToUpper() + " finished!");
        }
    }
}

