namespace myTelegramBot {
    partial class Form {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if ( disposing && (components != null) ) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            System.Windows.Forms.ListViewGroup listViewGroup1 = new System.Windows.Forms.ListViewGroup("ListViewGroup", System.Windows.Forms.HorizontalAlignment.Left);
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form));
            this.label_light_loading = new System.Windows.Forms.Label();
            this.label_light_working = new System.Windows.Forms.Label();
            this.label_light_waiting = new System.Windows.Forms.Label();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabPage_users = new System.Windows.Forms.TabPage();
            this.listView_users = new System.Windows.Forms.ListView();
            this.column_chat_id = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.column_username = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.column_first_name = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.column_last_name = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.column_join_date = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.column_active = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.column_speed = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tabPage_settings = new System.Windows.Forms.TabPage();
            this.button1 = new System.Windows.Forms.Button();
            this.textbox_console = new System.Windows.Forms.RichTextBox();
            this.label_time_working = new System.Windows.Forms.Label();
            this.label_time_workingAverage = new System.Windows.Forms.Label();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.tabControl.SuspendLayout();
            this.tabPage_users.SuspendLayout();
            this.tabPage_settings.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label_light_loading
            // 
            this.label_light_loading.AutoSize = true;
            this.label_light_loading.BackColor = System.Drawing.Color.LightGreen;
            this.label_light_loading.Location = new System.Drawing.Point(17, 35);
            this.label_light_loading.Name = "label_light_loading";
            this.label_light_loading.Size = new System.Drawing.Size(55, 13);
            this.label_light_loading.TabIndex = 8;
            this.label_light_loading.Text = "LOADING";
            this.label_light_loading.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label_light_working
            // 
            this.label_light_working.AutoSize = true;
            this.label_light_working.Location = new System.Drawing.Point(12, 9);
            this.label_light_working.Name = "label_light_working";
            this.label_light_working.Size = new System.Drawing.Size(60, 13);
            this.label_light_working.TabIndex = 6;
            this.label_light_working.Text = "WORKING";
            this.label_light_working.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label_light_waiting
            // 
            this.label_light_waiting.AutoSize = true;
            this.label_light_waiting.BackColor = System.Drawing.SystemColors.Control;
            this.label_light_waiting.Location = new System.Drawing.Point(18, 22);
            this.label_light_waiting.Name = "label_light_waiting";
            this.label_light_waiting.Size = new System.Drawing.Size(54, 13);
            this.label_light_waiting.TabIndex = 7;
            this.label_light_waiting.Text = "WAITING";
            this.label_light_waiting.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tabPage_users);
            this.tabControl.Controls.Add(this.tabPage_settings);
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.Location = new System.Drawing.Point(0, 0);
            this.tabControl.Margin = new System.Windows.Forms.Padding(5);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(391, 339);
            this.tabControl.TabIndex = 11;
            // 
            // tabPage_users
            // 
            this.tabPage_users.Controls.Add(this.listView_users);
            this.tabPage_users.Location = new System.Drawing.Point(4, 22);
            this.tabPage_users.Name = "tabPage_users";
            this.tabPage_users.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_users.Size = new System.Drawing.Size(383, 313);
            this.tabPage_users.TabIndex = 0;
            this.tabPage_users.Text = "Users";
            this.tabPage_users.UseVisualStyleBackColor = true;
            // 
            // listView_users
            // 
            this.listView_users.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.column_chat_id,
            this.column_username,
            this.column_first_name,
            this.column_last_name,
            this.column_join_date,
            this.column_active,
            this.column_speed});
            this.listView_users.Dock = System.Windows.Forms.DockStyle.Fill;
            listViewGroup1.Header = "ListViewGroup";
            listViewGroup1.Name = "listViewGroup1";
            this.listView_users.Groups.AddRange(new System.Windows.Forms.ListViewGroup[] {
            listViewGroup1});
            this.listView_users.Location = new System.Drawing.Point(3, 3);
            this.listView_users.Name = "listView_users";
            this.listView_users.Size = new System.Drawing.Size(377, 307);
            this.listView_users.TabIndex = 3;
            this.listView_users.UseCompatibleStateImageBehavior = false;
            this.listView_users.View = System.Windows.Forms.View.Details;
            // 
            // column_chat_id
            // 
            this.column_chat_id.Text = "chat_id";
            // 
            // column_username
            // 
            this.column_username.Text = "Username";
            // 
            // column_first_name
            // 
            this.column_first_name.Text = "First Name";
            // 
            // column_last_name
            // 
            this.column_last_name.Text = "Last Name";
            // 
            // column_join_date
            // 
            this.column_join_date.Text = "join date";
            // 
            // column_active
            // 
            this.column_active.Text = "active";
            // 
            // column_speed
            // 
            this.column_speed.Text = "speed";
            // 
            // tabPage_settings
            // 
            this.tabPage_settings.Controls.Add(this.button1);
            this.tabPage_settings.Location = new System.Drawing.Point(4, 22);
            this.tabPage_settings.Name = "tabPage_settings";
            this.tabPage_settings.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_settings.Size = new System.Drawing.Size(383, 313);
            this.tabPage_settings.TabIndex = 2;
            this.tabPage_settings.Text = "Settings";
            this.tabPage_settings.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(6, 8);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(131, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "SetPath";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // textbox_console
            // 
            this.textbox_console.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textbox_console.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textbox_console.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.textbox_console.HideSelection = false;
            this.textbox_console.Location = new System.Drawing.Point(0, 53);
            this.textbox_console.Margin = new System.Windows.Forms.Padding(5);
            this.textbox_console.Name = "textbox_console";
            this.textbox_console.ReadOnly = true;
            this.textbox_console.Size = new System.Drawing.Size(256, 286);
            this.textbox_console.TabIndex = 9;
            this.textbox_console.Text = "Telegram Bot\nMichele Viganò";
            // 
            // label_time_working
            // 
            this.label_time_working.Location = new System.Drawing.Point(74, 9);
            this.label_time_working.Name = "label_time_working";
            this.label_time_working.Size = new System.Drawing.Size(52, 13);
            this.label_time_working.TabIndex = 13;
            this.label_time_working.Text = "0 ms";
            // 
            // label_time_workingAverage
            // 
            this.label_time_workingAverage.Location = new System.Drawing.Point(132, 9);
            this.label_time_workingAverage.Name = "label_time_workingAverage";
            this.label_time_workingAverage.Size = new System.Drawing.Size(54, 13);
            this.label_time_workingAverage.TabIndex = 14;
            this.label_time_workingAverage.Text = "0 ms";
            // 
            // splitContainer1
            // 
            this.splitContainer1.BackColor = System.Drawing.SystemColors.Control;
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.label_light_waiting);
            this.splitContainer1.Panel1.Controls.Add(this.label_time_workingAverage);
            this.splitContainer1.Panel1.Controls.Add(this.label_light_working);
            this.splitContainer1.Panel1.Controls.Add(this.label_time_working);
            this.splitContainer1.Panel1.Controls.Add(this.label_light_loading);
            this.splitContainer1.Panel1.Controls.Add(this.textbox_console);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.BackColor = System.Drawing.SystemColors.Control;
            this.splitContainer1.Panel2.Controls.Add(this.tabControl);
            this.splitContainer1.Size = new System.Drawing.Size(651, 339);
            this.splitContainer1.SplitterDistance = 256;
            this.splitContainer1.TabIndex = 16;
            this.splitContainer1.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.splitContainer1_SplitterMoved);
            // 
            // Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(651, 339);
            this.Controls.Add(this.splitContainer1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(370, 200);
            this.Name = "Form";
            this.Text = "myTelegramBot";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form_FormClosing);
            this.tabControl.ResumeLayout(false);
            this.tabPage_users.ResumeLayout(false);
            this.tabPage_settings.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Label label_light_loading;
        private System.Windows.Forms.Label label_light_working;
        private System.Windows.Forms.Label label_light_waiting;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabPage_users;
        private System.Windows.Forms.ListView listView_users;
        private System.Windows.Forms.ColumnHeader column_chat_id;
        private System.Windows.Forms.ColumnHeader column_username;
        private System.Windows.Forms.ColumnHeader column_first_name;
        private System.Windows.Forms.ColumnHeader column_last_name;
        private System.Windows.Forms.ColumnHeader column_join_date;
        private System.Windows.Forms.ColumnHeader column_active;
        private System.Windows.Forms.ColumnHeader column_speed;
        private System.Windows.Forms.TabPage tabPage_settings;
        private System.Windows.Forms.RichTextBox textbox_console;
        private System.Windows.Forms.Label label_time_working;
        private System.Windows.Forms.Label label_time_workingAverage;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Button button1;
    }
}