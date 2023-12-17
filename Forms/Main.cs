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
using System.Net;
using NetFwTypeLib;

namespace Website_Blocker_from_scratch
{
    public partial class Main : Form
    {
        //Primjeri
        string[] examples = { "www.facebook.com", "www.youtube.com", "www.twitter.com", "www.instagram.com","boki01.github.io", "web.whatsapp.com","www.example.com","www.tsrb.hr" };
        //Loopback adresa
        string textToAdd = "\n127.0.0.1\t";
        //Vatrozid
        INetFwPolicy2 firewallPolicy = (INetFwPolicy2)Activator.CreateInstance(Type.GetTypeFromProgID("HNetCfg.FwPolicy2"));
        public Main()
        {
            InitializeComponent();
            this.MaximizeBox = false; //Zabranjeno maksimiziranje prozora
            this.FormBorderStyle = FormBorderStyle.FixedSingle; //Zabranjeno mijenjanje veličine prozora
            data.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill; //Popunjavanje glavnog zaglavlja 
            data.AllowUserToResizeRows = false; //Zabranjeno mijenjanje veličine redova
            data.AllowUserToResizeColumns = false; //Zabranjeno mijenjanje veličine stupaca
            this.AcceptButton = add; // Pritisak tipke ENTER je isto kao da je pritisnut gumb "Dodaj"
        }

        private void Main_Load(object sender, EventArgs e)
        {
            //Biranje nasumičnog primjera
            Random rnd = new Random();
            int r = rnd.Next(examples.Length);
            text.Cue = examples[r];

            try 
            {
                //Čitanje datoteke 'hosts'
                string windowsPath = Environment.GetEnvironmentVariable("windir");
                string hostsFilePath = Path.Combine(windowsPath, "System32", "drivers", "etc", "hosts");
                string[] lines = System.IO.File.ReadAllLines(hostsFilePath);

                DataTable dt = new DataTable();
                dt.Columns.Add("Crna lista"); //Dodavanje stupca "Crna lista"

                foreach (string line in lines) //Za svaku liniju u datoteci "hosts"
                {
                    if (!string.IsNullOrWhiteSpace(line) && !line.StartsWith("#")) //Ignoriraj sve linije koje počinju s "#" [komentare] i prazne linije
                    {

                        string[] parts = line.Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries); //Razdvajanje linije na dijelove; razmak i tabulator

                        if (parts.Length >= 2) //Ako ima više od 2 dijela
                        {
                            dt.Rows.Add(parts[1]); //Dodaj drugi dio u stupac "Crna lista"; domenu
                        }

                    }
                }

                data.DataSource = dt; //Dodaj sve u DataGridView
            }
            catch (Exception ex) //Ako se dogodi pogreška
            {
                MessageBox.Show(
                    "0: Datoteka 'hosts' nije pronađena!",
                    "Pogreška",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                ); //Obavijesti korisnika
                Application.Exit(); //Izađi iz programa
            }
            
        }
        private void update()
        {
            //Izvrši se sve isto kao u Main_Load
            string windowsPath = Environment.GetEnvironmentVariable("windir");
            string hostsFilePath = Path.Combine(windowsPath, "System32", "drivers", "etc", "hosts");
            string[] lines = System.IO.File.ReadAllLines(hostsFilePath);

            DataTable dt = new DataTable();
            dt.Columns.Add("Crna lista");

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

        private bool IsParameterInDataGrid(string parameter) //Provjeri postoji li element u DataGridView-u
        {
            
            foreach (DataGridViewRow row in data.Rows) //Za svaki red u DataGridView-u
            {
                
                if (row.Cells["Crna lista"].Value.ToString() == parameter) //Provjeri je li vrijednost u stupcu "Crna lista" jednaka parametru
                {
                    return true;
                }
            }
            return false;
        }


        private void add_Click(object sender, EventArgs e) //Prilikom pritiska gumba "Dodaj" ; dodavanje u datoteku "hosts"
        {
               
                if (text.Text == "" || IsParameterInDataGrid(text.Text)) //Ako je TextBox prazan ili je unesena domena već u DataGridView-u => ignoriraj
                {
                   text.Text = ""; //Vrati tekst na prvobitno stanje
                   return;
                }
            
                string windowsPath = Environment.GetEnvironmentVariable("windir");
                string hostsFilePath = Path.Combine(windowsPath, "System32", "drivers", "etc", "hosts");

                
                string[] urls = text.Text.Split(' '); //U slučaju da je korisnik unio više od jedne domene, razdvoji ih na dijelove
                
                string firstUrl = urls[0]; //Prva domena

                if (IsParameterInDataGrid(firstUrl)) //Ako je prva domena već u DataGridView-u => ignoriraj
                {
                    text.Text = ""; //Vrati tekst u TextBox-u na prvobitno stanje
                    return;
                }
            try
            {
                IPAddress[] addresses = Dns.GetHostAddresses(firstUrl); //Dohvati IP adrese domene
                string[] addressStrings = addresses.Select(ip => ip.ToString()).ToArray(); //Pretvori IP adrese u string
                string addressesString = string.Join(",", addressStrings); //Spoji sve IP adrese u jedan string

                INetFwRule firewallRuleOUT = (INetFwRule)Activator.CreateInstance(Type.GetTypeFromProgID("HNetCfg.FWRule")); //Generiraj pravilo za vatrozid
                firewallRuleOUT.Action = NET_FW_ACTION_.NET_FW_ACTION_BLOCK; //Zabrani pristup
                firewallRuleOUT.Description = "Pravilo generirano od strane aplikacije Website Blocker;\t" + firstUrl; //Opis
                firewallRuleOUT.Direction = NET_FW_RULE_DIRECTION_.NET_FW_RULE_DIR_OUT; //Pravilo se odnosi na odlazni promet
                firewallRuleOUT.Enabled = true;
                firewallRuleOUT.InterfaceTypes = "All"; //Pravilo se odnosi na sve mrežne adaptere
                firewallRuleOUT.RemoteAddresses = addressesString; //Unesi sve IP adrese domene
                firewallRuleOUT.Name = "Block " + firstUrl; //Ime pravila

                firewallPolicy.Rules.Add(firewallRuleOUT); //Dodaj pravilo

                INetFwRule firewallRuleIN = (INetFwRule)Activator.CreateInstance(Type.GetTypeFromProgID("HNetCfg.FWRule"));
                firewallRuleIN.Action = NET_FW_ACTION_.NET_FW_ACTION_BLOCK;
                firewallRuleIN.Description = "Pravilo generirano od strane aplikacije Website Blocker;\t" + firstUrl;
                firewallRuleIN.Direction = NET_FW_RULE_DIRECTION_.NET_FW_RULE_DIR_IN; //Pravilo se odnosi na dolazni promet
                firewallRuleIN.Enabled = true;
                firewallRuleIN.InterfaceTypes = "All";
                firewallRuleIN.RemoteAddresses = addressesString;
                firewallRuleIN.Name = "Block " + firstUrl;

                firewallPolicy.Rules.Add(firewallRuleIN);

                File.AppendAllText(hostsFilePath, textToAdd + firstUrl); //Dodavanje prve domene u datoteku 'hosts'

            }
            catch (Exception ex)
            {
                MessageBox.Show(

                    "?: \n.Nije moguće pronaći IP adrese\n.Greška s vatrozidom.\n.Program je potrebno pokrenuti s najvećim privilegijama.\n\n" + ex.Message,
                    "Pogreška",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error

                );

                if (ex.Message == "No such host is known") //Ako nije moguće pronaći IP adrese
                {
                    text.Text = ""; //Vrati tekst u TextBox-u na prvobitno stanje
                    return;
                }
                Application.Exit();
            }

                update(); //Ažuriranje prikaza u DataGridView-u

                MessageBox.Show(
                    "Pristup web stranici je uspješno zabranjen.\nSpremanje promjena može potrajati.",
                    "Uspjeh",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );
           

            text.Text = ""; //Vrati tekst u TextBox-u na prvobitno stanje
        }

        private void data_MouseClick(object sender, MouseEventArgs e) //Prilikom pritiska klika miša na DataGridView
        {
            if (e.Button == MouseButtons.Right) //Ako je pritisnut desni klik miša
            {
                var hittestInfo = data.HitTest(e.X, e.Y); //Dohvati informacije o kliku miša (poziciji)

                if (hittestInfo.Type == DataGridViewHitTestType.Cell) //Ako je kliknut ćelija (podatak)
                {
                    //Dodaj ContextMenuStrip = izbornik opcija
                    ContextMenuStrip contextMenu = new ContextMenuStrip();
                    ToolStripMenuItem menuItem = new ToolStripMenuItem("Ukloni"); //Dodaj opciju "Ukloni"
                    
                    menuItem.Image = Properties.Resources.Remove; //Dodaj ikonu

                    menuItem.Click += Remove; //Pridruži odgovarajuću funkciju koja barata s uklanjanjem zabrane
                    contextMenu.Items.Add(menuItem); //Dodaj opciju u izbornik

                    
                    contextMenu.Show(data, new Point(e.X, e.Y)); //Prikaži izbornik na poziciji klika miša

                }
            }
        }

        private void Remove(object sender, EventArgs e) //Funkcija koja barata s uklanjanjem zabrane
        {
            
            var menuItem = (ToolStripMenuItem)sender; //Dohvati opciju koja je pritisnuta
            var contextMenu = (ContextMenuStrip)menuItem.Owner; //Dohvati izbornik
            var dataGridView = (DataGridView)contextMenu.SourceControl; //Dohvati DataGridView
            var cell = dataGridView.CurrentCell; //Dohvati ćeliju koja je pritisnuta
            string siteToUnblock = cell.Value.ToString(); //Dohvati vrijednost ćelije

            string windowsPath = Environment.GetEnvironmentVariable("windir");
            string path = Path.Combine(windowsPath, "System32", "drivers", "etc", "hosts");
            string hostsContent = File.ReadAllText(path); //Pročitaj sve podatke datoteke 'hosts'

            string lineToRemove = "127.0.0.1\t" + siteToUnblock; //Izrada linije koju treba ukloniti
            hostsContent = hostsContent.Replace(lineToRemove, ""); //Zamijeni liniju koju treba ukloniti s praznim retkom

            try
            {
                File.WriteAllText(path, hostsContent); //Spremi promjene u datoteku 'hosts'
                firewallPolicy.Rules.Remove("Block " + siteToUnblock);
                firewallPolicy.Rules.Remove("Block " + siteToUnblock);

            }
            catch (Exception ex) //Ako se dogodi pogreška
            {
                MessageBox.Show(
                "1: Program je potrebno pokrenuti s najvećim privilegijama.",
                "Pogreška",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error
                );

                Application.Exit();
            }

            
            dataGridView.Rows.RemoveAt(cell.RowIndex); //Ukloni red iz DataGridView-a
        }

        private void text_Enter(object sender, EventArgs e) //Prilikom ulaska u TextBox
        {
            //Prikaži dodatne informacije (hint)
            info.Visible = true;
            infoIcon.Visible = true;
        }

        private void text_Leave(object sender, EventArgs e) //Prilikom izlaska iz TextBox
        {
            //Sakrij dodatne informacije (hint) + generiraj novi primjer
            Random rnd = new Random();
            int r = rnd.Next(examples.Length);
            text.Cue = examples[r];
            info.Visible = false;
            infoIcon.Visible = false;
        }
    }
}