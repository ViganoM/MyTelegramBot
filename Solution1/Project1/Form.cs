using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace myTelegramBot {
    public partial class Form : System.Windows.Forms.Form {
        public Form() {
            InitializeComponent();
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
        public List<Userdata> users {
            set {
                foreach ( Userdata userdata in value ) {
                    imageList_users.Images.Add(userdata.chat.id.ToString(), userdata.photo);
                    listView_users.Items.Add(new ListViewItem(new string[7] {
                        userdata.chat.id.ToString(),
                        userdata.chat.username,
                        userdata.chat.first_name,
                        userdata.chat.last_name,
                        userdata.joinDate.ToString("dd MMM yy"),
                        userdata.active.ToString(),
                        userdata.speed.ToString()
                    }, imageKey: userdata.chat.id.ToString()));
                }
            }
        }

        void WriteToConsole(string text, Color color) {

        }
        void WriteToConsole(string text) {
            WriteToConsole(text, SystemColors.ControlText);
        }
    }
}
