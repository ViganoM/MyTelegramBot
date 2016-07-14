using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using System.Timers;
using JsonTypes;
using System.IO;
using myTelegramBot.Properties;


namespace myTelegramBot {
    public enum activity { WaitingSend, WaitingResponse, Inactive, PrivacyConsent }
    static class localUsersData {
        public static string now {
            get { return DateTime.Now.ToString(Settings.Default.datetimeFormat); }
        }
        static string LoadFilepath {
            get {
                if ( !Directory.Exists(Settings.Default.DataFolderPath) )
                    Directory.CreateDirectory(Settings.Default.DataFolderPath);
                return Settings.Default.DataFolderPath + Settings.Default.settingFilename + Settings.Default.lastSettingFilename;
            }
        }
        public static string SaveFilepath {
            get {
                if ( !Directory.Exists(Settings.Default.DataFolderPath) )
                    Directory.CreateDirectory(Settings.Default.DataFolderPath);
                return Settings.Default.DataFolderPath + Settings.Default.settingFilename;
            }
        }
        public static string LogFilepath { get { return Settings.Default.DataFolderPath + "_LOG"; } }

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
            string end = now;
            try {
                new BinaryFormatter().Serialize(new FileStream(SaveFilepath + end, FileMode.Create, FileAccess.Write), usersData);
                Settings.Default.lastSettingFilename = end;
            } catch ( SerializationException exception ) {
                System.IO.File.AppendAllLines(LogFilepath, exception.ToString().Split(new string[] { "\n" }, StringSplitOptions.None).ToArray());
                //MessageBox.Show("Settings could not be saved.\nA new attempt will be made 1 second after this box will be closed.\nSettings will be saved in folder:\n" + Settings.Default.DataFolderPath + "\nin a file whose name starts with:\n" + Settings.Default.settingFilename + "\nplus date and time of saving, in format:\n" + Settings.Default.datetimeFormat + "\nplus:\n_2nd\n\n" + exception.ToString(), "Settings not saved", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Thread.Sleep(1000);
                end = now;
                try {
                    new BinaryFormatter().Serialize(new FileStream(SaveFilepath + end + "_2nd", FileMode.Create, FileAccess.Write), usersData);
                    Settings.Default.lastSettingFilename = end+"_2nd";
                    //MessageBox.Show("Settings were eventually saved at:\n" + Settings.Default.DataFolderPath + end + "_2nd\nPlease manually manage the settings files", "Settings eventually saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
                } catch ( SerializationException exception2 ) {
                    System.IO.File.AppendAllLines(LogFilepath, exception2.ToString().Split(new string[] { "\n" }, StringSplitOptions.None).ToArray());
                    System.IO.File.AppendAllLines(LogFilepath, new string[] { "-------", now, "Data not saved" });
                    //MessageBox.Show("Settings were not be saved.\nData will be lost\n" + exception2.ToString(), "Settings definitively not saved", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            Settings.Default.lastUpdateId = ServerMethods.lastUpdate;
            Settings.Default.Save();
        }
        public static bool LoadData(bool reset = false) {
            if ( reset ) {
                Settings.Default.Reset();
                Settings.Default.Save();
                Environment.Exit(0);
            }

            ServerMethods.lastUpdate = Settings.Default.lastUpdateId;

            try {
                usersData = new BinaryFormatter().Deserialize(new FileStream(LoadFilepath, FileMode.Open, FileAccess.Read)) as Dictionary<int, Userdata>;
                #region update non serialized stuff
                foreach ( Userdata userdata in usersData.Values )
                    userdata.Restore();

                #endregion
                return true;
            } catch ( FileNotFoundException fileNotFound ) {
                System.IO.File.AppendAllLines(LogFilepath, fileNotFound.ToString().Split(new string[] { "\n" }, StringSplitOptions.None).ToArray());
                //MessageBox.Show("Settings not restored because the file could not be found:\n" + LoadFilepath + "\nA new attempt will be made looking for older files after this box will be closed\n\n" + fileNotFound.ToString(), "Settings not restored", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return SecondLoad();
            } catch ( SerializationException serializationEx ) {
                System.IO.File.AppendAllLines(LogFilepath, serializationEx.ToString().Split(new string[] { "\n" }, StringSplitOptions.None).ToArray());
                return SecondLoad();
            }
            }
        
        static bool SecondLoad() {
            FileInfo[] list = new DirectoryInfo(Settings.Default.DataFolderPath).GetFiles().OrderByDescending(x => x.CreationTime).ToArray();
            foreach ( FileInfo file in list )
                try {
                    usersData = new BinaryFormatter().Deserialize(new FileStream(file.FullName, FileMode.Open, FileAccess.Read)) as Dictionary<int, Userdata>;
                    //MessageBox.Show("Settings restored from file:\n" + file.FullName + "\nInstead of file:\n" + LoadFilepath, "Settings restores from an unexpected file", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return true;
                } catch ( SerializationException serializationEx ) {
                    Program.form.WriteToConsole("Settings not restored from " + file.FullName, Color.Red);
                    System.IO.File.AppendAllLines(LogFilepath , serializationEx.ToString().Split(new string[] { "\n" }, StringSplitOptions.None).ToArray());
                }
            System.IO.File.AppendAllLines(LogFilepath, (new Exception("Settings could not be restored from any file in path:\n" + Settings.Default.DataFolderPath)).ToString().Split(new string[] { "\n" },StringSplitOptions.None).ToArray());
            // MessageBox.Show("Settings not restored from any file in path:\n" + Settings.Default.DataFolderPath + "\nConsider to change the data saving folder", "Settings definitely not restored", MessageBoxButtons.OK, MessageBoxIcon.Error);

            return false;
        }

        public static void DeleteOld() {
            FileInfo[] files = new DirectoryInfo(Settings.Default.DataFolderPath).GetFiles("*", SearchOption.AllDirectories).OrderBy(x => x.CreationTime).ToArray();
            if ( files.Sum(x => x.Length) > Settings.Default.maxDataFolderSize_MB * 1024 * 1024 )
                for ( int n = 0 ; n < files.Length / 2 ; n++ )
                    if ( !files[n].Name.Contains("_LOG") )
                        files[n].Delete();
        }
    }

[Serializable]
public class Userdata {
    public Chat chat { get; private set; }
    public User user { get; private set; }
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
        lastUserUpdate = DateTime.Now;
    }

    public void MessageInput(Update update) {
        lastUpdate = update.message.message_id;
        switch ( activity ) {
            case activity.WaitingResponse:
                activity = activity.WaitingSend;
                TimeSpan delta = DateTime.Now - lastMessageSent;
                ServerMethods.sendMessage(chat.id, "Got it! You took " + delta.TotalSeconds.ToString("#.000") + " seconds.");
                if ( delta.TotalSeconds < Settings.Default.minResponseTime_s ) {
                    ServerMethods.sendMessage(chat.id, "That's a new record!\nWould you like to share it with other users? (They will know your username and your record; if you did not set an username, your first name will be used instead)\nWrite <i>yes</i> for yes (case insensitive).\nWrite anything else for no.\nbtw: You will not receive other messagges until you answer me.");
                    activity = activity.PrivacyConsent;
                    Settings.Default.minResponseTime_s = delta.Seconds;
                } else if ( delta.TotalSeconds < response.Min() )
                    ServerMethods.sendMessage(chat.id, "That's a new personal record!\nYour previous record was " + response.Min().ToString("#.000") + " seconds.");
                response.Add(delta.TotalSeconds);
                break;
            case activity.WaitingSend:
                ServerMethods.sendMessage(chat.id, "What do you mean?\nUse commands or wait my next message", reply_to_message_id: update.message.message_id);
                break;
            case activity.Inactive:
                ServerMethods.sendMessage(chat.id, "Currently I am not sending you messagges. Use \\write to receive messagges");
                break;
            case activity.PrivacyConsent:
                if ( update.message.text.ToLower() == "yes" )
                    if ( response.Min() < Settings.Default.minResponseTime_s )
                        ShareRecord();
                    else
                        ServerMethods.sendMessage(chat.id, "You took too long to answer. Strange right?\nThere is a new record now...");
                else
                    ServerMethods.sendMessage(chat.id, "Ok. I'll keep your secret.");
                activity = activity.Inactive;
                ServerMethods.sendMessage(chat.id, "Use command /write to receive messagges.\nBy now I'm idle.");
                break;
        }
        //user update
        if ( DateTime.Now - lastUserUpdate > new TimeSpan(Settings.Default.userUpdatePeriod_day, 0, 0, 0) )
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
        ServerMethods.sendMessage(chat.id, "Answer this!\nI sent this: " + DateTime.Now.ToString("HH:mm.ss.fff"), !notificate);
        lastMessageSent = DateTime.Now;
        activity = activity.WaitingResponse;
        Program.form.WriteToConsole("Scheduled message sent", Color.Blue);
    }

    public void SendStats() {
        ServerMethods.sendMessage(chat.id, string.Format("Here are your stats:\nNumber of responses: {0}\nAverage response time: <b>{1}</b>\nJoin Date: {2}\nSpeed: {3}\nActivity: {4}\nNotification active?: {5}\nLast message I sent: {6}", response.Count, response.Average(), joinDate.ToLongDateString(), speed, activity.ToString(), notificate, lastMessageSent.ToString("HH:mm.ss.fff - ddd dd MMMM yyyy")));
    }
    void ShareRecord() {
        string name = user.first_name;
        if ( user.username != null )
            name = user.username;
        ServerMethods.sendBroadMessage("The user " + name + " just answered a message in " + response.Min().ToString("#.000") + " seconds!");
    }
}
}
