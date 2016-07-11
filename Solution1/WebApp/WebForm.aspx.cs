using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using myTelegramBot;

namespace WebApp {
    public partial class WebForm1 : System.Web.UI.Page {
        protected void Page_Load(object sender, EventArgs e) {
            runButton.Click += RunButton_Click;
        }

        private void RunButton_Click(object sender, EventArgs e){
            if ( runButton.Text == "run" ) {
                //Program.Updater();
                runButton.Text = "stop";
            } else
                Program.run = false;
        }
    }
}