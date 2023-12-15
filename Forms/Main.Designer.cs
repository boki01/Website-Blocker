namespace Website_Blocker_from_scratch
{
    partial class Main
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
            this.data = new System.Windows.Forms.DataGridView();
            this.panel1 = new System.Windows.Forms.Panel();
            this.add = new System.Windows.Forms.Button();
            this.info = new System.Windows.Forms.Label();
            this.infoIcon = new System.Windows.Forms.PictureBox();
            this.text = new NullTextBox();
            ((System.ComponentModel.ISupportInitialize)(this.data)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.infoIcon)).BeginInit();
            this.SuspendLayout();
            // 
            // data
            // 
            this.data.AllowUserToAddRows = false;
            this.data.AllowUserToDeleteRows = false;
            this.data.AllowUserToResizeColumns = false;
            this.data.AllowUserToResizeRows = false;
            this.data.BackgroundColor = System.Drawing.Color.WhiteSmoke;
            this.data.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.data.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.data.Dock = System.Windows.Forms.DockStyle.Fill;
            this.data.Location = new System.Drawing.Point(0, 0);
            this.data.Name = "data";
            this.data.ReadOnly = true;
            this.data.RowHeadersWidth = 30;
            this.data.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.data.Size = new System.Drawing.Size(504, 277);
            this.data.TabIndex = 0;
            this.data.MouseClick += new System.Windows.Forms.MouseEventHandler(this.data_MouseClick);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.data);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(504, 277);
            this.panel1.TabIndex = 1;
            // 
            // add
            // 
            this.add.BackColor = System.Drawing.Color.Transparent;
            this.add.Location = new System.Drawing.Point(418, 286);
            this.add.Name = "add";
            this.add.Size = new System.Drawing.Size(75, 21);
            this.add.TabIndex = 3;
            this.add.Text = "Dodaj";
            this.add.UseVisualStyleBackColor = false;
            this.add.Click += new System.EventHandler(this.add_Click);
            // 
            // info
            // 
            this.info.AutoSize = true;
            this.info.Location = new System.Drawing.Point(25, 308);
            this.info.Name = "info";
            this.info.Size = new System.Drawing.Size(156, 13);
            this.info.TabIndex = 5;
            this.info.Text = "Provjerite sadrži li domena www";
            this.info.Visible = false;
            // 
            // infoIcon
            // 
            this.infoIcon.BackgroundImage = global::Website_Blocker_from_scratch.Properties.Resources.info;
            this.infoIcon.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.infoIcon.Location = new System.Drawing.Point(12, 307);
            this.infoIcon.Name = "infoIcon";
            this.infoIcon.Size = new System.Drawing.Size(15, 15);
            this.infoIcon.TabIndex = 4;
            this.infoIcon.TabStop = false;
            this.infoIcon.Visible = false;
            // 
            // text
            // 
            this.text.Cue = "www.example.com";
            this.text.Location = new System.Drawing.Point(12, 286);
            this.text.Name = "text";
            this.text.Size = new System.Drawing.Size(400, 20);
            this.text.TabIndex = 2;
            this.text.Enter += new System.EventHandler(this.text_Enter);
            this.text.Leave += new System.EventHandler(this.text_Leave);
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ClientSize = new System.Drawing.Size(504, 331);
            this.Controls.Add(this.info);
            this.Controls.Add(this.infoIcon);
            this.Controls.Add(this.add);
            this.Controls.Add(this.text);
            this.Controls.Add(this.panel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Main";
            this.Text = "Website Blocker";
            this.Load += new System.EventHandler(this.Main_Load);
            ((System.ComponentModel.ISupportInitialize)(this.data)).EndInit();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.infoIcon)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView data;
        private System.Windows.Forms.Panel panel1;
        private NullTextBox text;
        private System.Windows.Forms.Button add;
        private System.Windows.Forms.PictureBox infoIcon;
        private System.Windows.Forms.Label info;
    }
}

