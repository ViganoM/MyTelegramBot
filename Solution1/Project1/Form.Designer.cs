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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.ListViewGroup listViewGroup2 = new System.Windows.Forms.ListViewGroup("ListViewGroup", System.Windows.Forms.HorizontalAlignment.Left);
            this.label_light_working = new System.Windows.Forms.Label();
            this.label_light_waiting = new System.Windows.Forms.Label();
            this.listView_users = new System.Windows.Forms.ListView();
            this.chat_id = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.username = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.FirstName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.LastName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.join_date = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.active = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.speed = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.imageList_users = new System.Windows.Forms.ImageList(this.components);
            this.SuspendLayout();
            // 
            // label_light_working
            // 
            this.label_light_working.AutoSize = true;
            this.label_light_working.Location = new System.Drawing.Point(9, 9);
            this.label_light_working.Name = "label_light_working";
            this.label_light_working.Size = new System.Drawing.Size(60, 13);
            this.label_light_working.TabIndex = 0;
            this.label_light_working.Text = "WORKING";
            this.label_light_working.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label_light_waiting
            // 
            this.label_light_waiting.AutoSize = true;
            this.label_light_waiting.BackColor = System.Drawing.Color.LightGreen;
            this.label_light_waiting.Location = new System.Drawing.Point(9, 22);
            this.label_light_waiting.Name = "label_light_waiting";
            this.label_light_waiting.Size = new System.Drawing.Size(54, 13);
            this.label_light_waiting.TabIndex = 1;
            this.label_light_waiting.Text = "WAITING";
            this.label_light_waiting.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // listView_users
            // 
            this.listView_users.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.listView_users.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.chat_id,
            this.username,
            this.FirstName,
            this.LastName,
            this.join_date,
            this.active,
            this.speed});
            listViewGroup2.Header = "ListViewGroup";
            listViewGroup2.Name = "listViewGroup1";
            this.listView_users.Groups.AddRange(new System.Windows.Forms.ListViewGroup[] {
            listViewGroup2});
            this.listView_users.Location = new System.Drawing.Point(12, 38);
            this.listView_users.Name = "listView_users";
            this.listView_users.Size = new System.Drawing.Size(121, 211);
            this.listView_users.TabIndex = 3;
            this.listView_users.UseCompatibleStateImageBehavior = false;
            // 
            // imageList_users
            // 
            this.imageList_users.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.imageList_users.ImageSize = new System.Drawing.Size(16, 16);
            this.imageList_users.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(543, 261);
            this.Controls.Add(this.listView_users);
            this.Controls.Add(this.label_light_waiting);
            this.Controls.Add(this.label_light_working);
            this.Name = "Form";
            this.Text = "myTelegramBot";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(Program.Close);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label_light_working;
        private System.Windows.Forms.Label label_light_waiting;
        private System.Windows.Forms.ListView listView_users;
        private System.Windows.Forms.ColumnHeader chat_id;
        private System.Windows.Forms.ColumnHeader username;
        private System.Windows.Forms.ColumnHeader FirstName;
        private System.Windows.Forms.ColumnHeader LastName;
        private System.Windows.Forms.ColumnHeader join_date;
        private System.Windows.Forms.ColumnHeader active;
        private System.Windows.Forms.ColumnHeader speed;
        private System.Windows.Forms.ImageList imageList_users;
    }
}