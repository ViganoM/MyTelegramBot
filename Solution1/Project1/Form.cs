using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Threading;
using System.Linq;

namespace myTelegramBot {
    public partial class Form : System.Windows.Forms.Form {
        public Form(AutoResetEvent formCreated) {
            CheckForIllegalCrossThreadCalls = false;
            InitializeComponent();
            Thread.Sleep(500);
            formCreated.Set();
        }

        List<long> WorkingTimes = new List<long>();
        public long WorkingTime {
            set {
                label_time_working.Text = value.ToString("# ms");
                WorkingTimes.Add(value);
                if ( WorkingTimes.Count % 5 == 0 )
                    label_time_workingAverage.Text = WorkingTimes.Average().ToString("#.0 ms");
            }
        }
        public int WaitingTime {
            set { label_time_waiting.Text = value.ToString("# ms"); }
        }
        public bool Working {
            set {
                if ( value ) {
                    label_light_waiting.BackColor = SystemColors.Control;
                    label_light_working.BackColor = Color.LightGreen;
                } else {
                    label_light_working.BackColor = SystemColors.Control;
                    label_light_waiting.BackColor = Color.LightGreen;
                }
            }
        }
        public bool Loading {
            set {
                if ( value ) {
                    label_light_working.BackColor = SystemColors.Control;
                    label_light_waiting.BackColor = SystemColors.Control;
                    label_light_loading.BackColor = Color.LightGreen;
                } else {
                    label_light_loading.BackColor = SystemColors.Control;
                    label_light_working.BackColor = SystemColors.Control;
                    label_light_waiting.BackColor = Color.LightGreen;
                }
            }
        }

        public void SetUser(Userdata userData) {
            if ( listView_users.Items.ContainsKey(userData.chat.id.ToString()) )
                RemoveUser(userData);
            List<Userdata> list = new List<Userdata>() { userData };
            SetUsers(list);
        }
        public void SetUsers(List<Userdata> userList, bool clear = false) {
            if ( clear ) {
                imageList_users.Images.Clear();
                listView_users.Clear();
            }
            foreach ( Userdata userdata in userList ) {
                imageList_users.Images.Add(userdata.chat.id.ToString(), userdata.photo);
                ListViewItem listViewItem = new ListViewItem(new string[7] {
                        userdata.chat.id.ToString(),
                        userdata.chat.username,
                        userdata.chat.first_name,
                        userdata.chat.last_name,
                        userdata.joinDate.ToString("dd MMM yy"),
                        userdata.activity.ToString(),
                        userdata.speed.ToString()
                    }, imageKey: userdata.chat.id.ToString());
                listViewItem.Name = userdata.chat.id.ToString();
                listView_users.Items.Add(listViewItem);
            }
        }
        public void RemoveUser(Userdata userData) {
            imageList_users.Images.RemoveByKey(userData.chat.id.ToString());
            listView_users.Items.RemoveByKey(userData.chat.id.ToString());
        }

        public void WriteToConsole(string text, Color color) {
            textbox_console.SelectionStart = textbox_console.TextLength;
            textbox_console.SelectionLength = 0;
            textbox_console.SelectionColor = color;
            WriteToConsole(text);
            textbox_console.SelectionColor = SystemColors.WindowText;
        }
        public void WriteToConsole(string text) {
            textbox_console.AppendText("\n • " + text);

        }

        private void Form_FormClosing(object sender, FormClosingEventArgs e) {
            WriteToConsole("process is closing", Color.OrangeRed);
            Program.Close(sender, e);
        }
    }
}
