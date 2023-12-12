using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using static System.Windows.Forms.LinkLabel;
using System.Resources;

namespace Website_Blocker_from_scratch
{
    public partial class Main : Form
    {
        string[] examples = { "www.facebook.com", "www.youtube.com", "www.twitter.com", "www.instagram.com","boki01.github.io", "web.whatsapp.com","www.example.com","www.tsrb.hr" };

        string textToAdd = "\n127.0.0.1\t";
        public Main()
        {
            InitializeComponent();
            this.MaximizeBox = false;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            data.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            data.AllowUserToResizeRows = false;
            data.AllowUserToResizeColumns = false;
            data.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.AcceptButton = add;
        }

        private void Main_Load(object sender, EventArgs e)
        {
            Random rnd = new Random();
            int r = rnd.Next(examples.Length);
            text.Cue = examples[r];

            try
            {
                string[] lines = System.IO.File.ReadAllLines(@"C:\Windows\System32\drivers\etc\hosts");
                DataTable dt = new DataTable();
                dt.Columns.Add("Blocked site");

                foreach (string line in lines)
                {
                    if (!string.IsNullOrWhiteSpace(line) && !line.StartsWith("#"))
                    {

                        string[] parts = line.Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);

                        if (parts.Length >= 2)
                        {
                            dt.Rows.Add(parts[1]);
                        }

                    }
                }

                data.DataSource = dt;
            }
            catch (Exception exc)
            {
                MessageBox.Show(
                    "ERROR # 0: Hosts file not found! The file is included with Windows.",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
                Application.Exit();
            }
            
        }
        private void update()
        {
            string[] lines = System.IO.File.ReadAllLines(@"C:\Windows\System32\drivers\etc\hosts");
            lines = System.IO.File.ReadAllLines(@"C:\Windows\System32\drivers\etc\hosts");

            DataTable dt = new DataTable();
            dt.Columns.Add("Blocked site");

            foreach (string line in lines)
            {
                if (!string.IsNullOrWhiteSpace(line) && !line.StartsWith("#")) 
                {
                    string[] parts = line.Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries); 
                    if (parts.Length >= 2)
                    {
                        dt.Rows.Add(parts[1]);
                    }
                }
            }

            data.DataSource = dt;
        }
        private bool IsParameterInDataGrid(string parameter)
        {
            
            foreach (DataGridViewRow row in data.Rows)
            {
                
                if (row.Cells["Blocked site"].Value.ToString() == parameter)
                {
                    return true;
                }
            }
            return false;
        }


        private void add_Click(object sender, EventArgs e)
        {
            if (text.Text != "" && !IsParameterInDataGrid(text.Text))
            {
                
                string[] urls = text.Text.Split(' ');

                
                string firstUrl = urls[0];

                if (IsParameterInDataGrid(firstUrl))
                {
                    text.Text = "";
                    return;
                }

                try
                {
                    File.AppendAllText(@"C:\Windows\System32\drivers\etc\hosts", textToAdd + firstUrl);
                }
                catch (Exception exc)
                {
                    MessageBox.Show(
                        "ERROR # 1: The program should be run under admin privileges!",
                        "Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error
                    );
                    Application.Exit();
                }

                update();

                MessageBox.Show(
                    "The site has been blocked successfully!\n\nIt may take some time to apply changes.",
                    "Success",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );
            }
            text.Text = "";
        }

        private void data_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                var hittestInfo = data.HitTest(e.X, e.Y);

                if (hittestInfo.Type == DataGridViewHitTestType.Cell)
                {
                    
                    ContextMenuStrip contextMenu = new ContextMenuStrip();
                    ToolStripMenuItem menuItem = new ToolStripMenuItem("Remove");
                    
                    menuItem.Image = Properties.Resources.Remove;

                    menuItem.Click += Remove;
                    contextMenu.Items.Add(menuItem);


                    
                    contextMenu.Show(data, new Point(e.X, e.Y));

                }
            }
        }

        private void Remove(object sender, EventArgs e)
        {
            
            var menuItem = (ToolStripMenuItem)sender;
            var contextMenu = (ContextMenuStrip)menuItem.Owner;
            var dataGridView = (DataGridView)contextMenu.SourceControl;
            var cell = dataGridView.CurrentCell;
            string siteToUnblock = cell.Value.ToString();

            string path = @"C:\Windows\System32\drivers\etc\hosts";
            string hostsContent = File.ReadAllText(path);

            string lineToRemove = "127.0.0.1\t" + siteToUnblock;
            hostsContent = hostsContent.Replace(lineToRemove, "");

            try
            {
                File.WriteAllText(path, hostsContent);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                "ERROR # 1: The program should be run under admin privileges!!",
                "Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error
                );

                Application.Exit();
            }

            
            dataGridView.Rows.RemoveAt(cell.RowIndex);
        }

        private void text_Enter(object sender, EventArgs e)
        {
            info.Visible = true;
            infoIcon.Visible = true;
        }

        private void text_Leave(object sender, EventArgs e)
        {
            info.Visible = false;
            infoIcon.Visible = false;
        }
    }
}
