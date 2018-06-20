using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using System.IO;
using System.Xml;
using System.Threading;
using System.ServiceModel;

namespace Kraken
{
    /// <summary>
    /// Interaction logic for Form1.
    /// </summary>
    public partial class Form1 : Form
    {
        #region Fields

        internal Felhasznalo belepo;

        private string[] felevek = new string[12]
        {
            "2008/09/1",
            "2008/09/2",
            "2009/10/1",
            "2009/10/2",
            "2010/11/1",
            "2010/11/2",
            "2011/12/1",
            "2011/12/2",
            "2012/13/1",
            "2012/13/2",
            "2013/14/1",
            "2013/14/2"
        };

        private string box = "";
        private string aktualis_felev = "2013/14/2";

        // Kulcsok.
        private int targy_kulcs = 0;
        private int hallgato_kulcs = 0;
        private int oktato_kulcs = 0;
        private int felhasznalo_kulcs = 0;
        private int uzenet_kulcs = 0;

        // Tárgyfelvétel.
        private string h_targyfelv_id; // kód.
        private string h_targyfelv_elokov; // előkövetelmény.

        // Üzenet.
        private string uzenet_id1; // idő.
        private string uzenet_id2; // küldő/címzett.
        private string uzenet_id3; // tárgy.
        private string uzenet_id4; // szöveg.
        private bool cim_valtozas = false; // cím.
        private bool targy_valtozas = false; // tárgy.
        private bool szoveg_valtozas = false; // szöveg.

        // Értékelés.
        private bool alairas_change = false; // aláírás.
        private bool jegy_change = false; // jegy.

        // szerverhez csatlakozáshoz szükséges Objektumok.
        private ChannelFactory<IServerM> chanel = new ChannelFactory<IServerM>(new NetTcpBinding(), new EndpointAddress(new Uri("net.tcp://localhost/KrakenService")));
        private IServerM server = null;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Form1"/> class.
        /// </summary>
        public Form1()
        {
            InitializeComponent();
        }
		
		/// <summary>
        /// Destroys the instance of the <see cref="Form1"/> class.
        /// </summary>
        ~Form1()
        { }

        #endregion Constructors

        #region Methods

        #region Presets

        /// <summary>
        /// Handles the Click event of the label1 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void label1_Click(object sender, EventArgs e)
        {
            BejelentkezesEsKijelentkezes();
        }

        /// <summary>
        /// Handles the Load event of the Form1 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void Form1_Load(object sender, EventArgs e)
        {
            // Preset values.
            Form_Elokeszites();
            Form_Tabok();

            try
            {
                server = chanel.CreateChannel();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        /// <summary>
        /// Login and Logout.
        /// </summary>
        private void BejelentkezesEsKijelentkezes()
        {
			// Bejelentkezés.
            if (label1.Text == "Bejelentkezés")
            {
                // Bejelentkező felület meghívása.
                Form2 bejelentkezes = new Form2();
                bejelentkezes.ShowDialog();

                // A felhasználó adatainak lekérése.
                try
                {
                    this.belepo = bejelentkezes.belepo;
                }
                catch
                { }

                // Frissítés.
                Form_Frissites();
            }
			// Kijelentkezés.
            else
            {
                label1.Text = "Bejelentkezés";
                label48.Text = "";

                try
                {
                    belepo.Neve = "senki";
                    tabControl1.TabPages.Add(tabPage1);
                    tabControl1.TabPages.Remove(tabPage2);
                    tabControl1.TabPages.Remove(tabPage3);
                    tabControl1.TabPages.Remove(tabPage4);
                }
                catch
                { }
            }
        }

        /// <summary>
        /// Preset Form.
        /// </summary>
        private void Form_Elokeszites()
        {
            // Kivétel érték beállítása.
            try
            {
                belepo.Neve = "senki";
            }
            catch
            { }
        }

        /// <summary>
        /// Preset tabs.
        /// </summary>
        private void Form_Tabok()
        {
            // Felesleges fülek kiszedése.
            try
            {
				// Hallgatói felület.
                tabControl1.TabPages.Remove(tabPage2);
				// Oktatói felület.
                tabControl1.TabPages.Remove(tabPage3);
				// Admin felület.
                tabControl1.TabPages.Remove(tabPage4);
            }
            catch
            { }
        }

        /// <summary>
        /// Refresh after login.
        /// </summary>
        private void Form_Frissites()
        {
            // Értékek beállítása.
            comboBox1.Items.Clear();
            comboBox2.Items.Clear();
            comboBox3.Items.Clear();
            comboBox4.Items.Clear();
            comboBox5.Items.Clear();
            comboBox6.Items.Clear();
            comboBox7.Items.Clear();
            comboBox8.Items.Clear();
            comboBox9.Items.Clear();
            comboBox10.Items.Clear();
            comboBox11.Items.Clear();
            comboBox12.Items.Clear();
            comboBox13.Items.Clear();
            comboBox14.Items.Clear();
            comboBox15.Items.Clear();
            comboBox16.Items.Clear();
            comboBox17.Items.Clear();
            comboBox20.Items.Clear();
            comboBox21.Items.Clear();

            for (int i = 0; i < felevek.Length; i++)
            {
                comboBox1.Items.Add(felevek[i].ToString());
                comboBox3.Items.Add(felevek[i].ToString());
                comboBox4.Items.Add(felevek[i].ToString());
                comboBox5.Items.Add(felevek[i].ToString());
                comboBox6.Items.Add(felevek[i].ToString());
                comboBox7.Items.Add(felevek[i].ToString());
                comboBox9.Items.Add(felevek[i].ToString());
                comboBox18.Items.Add(felevek[i].ToString());
            }

            for (int i = 0; i < 7; i++)
            {
                comboBox20.Items.Add(2008 + i);
                comboBox21.Items.Add(2008 + i);
            }

            comboBox2.Items.Add("Tárgy neve");
            comboBox2.Items.Add("Tárgy kódja");
            comboBox2.Items.Add("Kredit");
            comboBox2.Items.Add("Oktató neve");
            comboBox2.Items.Add("Kurzusidő");
            comboBox2.Items.Add("Előkövetelmény");
            comboBox2.Items.Add("Követelmény");

            try
            {
                string megy = "oktato_nevek";
                string kod = "";
                Felhasznalo[] eredmeny = server.Felhasznalo_Lista(megy, kod);

                for (int i = 0; i < eredmeny.Length; i++)
                {
                    comboBox8.Items.Add(eredmeny[i].Neve.ToString());
                }
            }
            catch
            { }

            comboBox10.Items.Add("nő");
            comboBox10.Items.Add("férfi");
            comboBox11.Items.Add("aktív");
            comboBox11.Items.Add("passzív");
            comboBox11.Items.Add("megszűnt");
            comboBox12.Items.Add("aktív");
            comboBox12.Items.Add("szabadság");
            comboBox12.Items.Add("megszűnt");
            comboBox13.Items.Add("nő");
            comboBox13.Items.Add("férfi");
            comboBox14.Items.Add("hallgató");
            //comboBox14.Items.Add("oktató");
            //comboBox14.Items.Add("adminisztrátor");
            //comboBox15.Items.Add("hallgató");
            comboBox15.Items.Add("oktató");
            comboBox15.Items.Add("adminisztrátor");
            comboBox16.Items.Add("Aláírva");
            //comboBox16.Items.Add("Megtagadva");
            comboBox16.Items.Add("Letiltva");
            comboBox17.Items.Add("1");
            comboBox17.Items.Add("2");
            comboBox17.Items.Add("3");
            comboBox17.Items.Add("4");
            comboBox17.Items.Add("5");
            comboBox19.Items.Add("Aláírva");
            //comboBox19.Items.Add("Megtagadva");
            comboBox19.Items.Add("Letiltva");
            comboBox22.Items.Add("1");
            comboBox22.Items.Add("2");
            comboBox22.Items.Add("3");
            comboBox22.Items.Add("4");
            comboBox22.Items.Add("5");

            // Ellenőrzöm, hogy van-e felhasználó bejelentkezve.
            try
            {
                if (belepo.Neve != "senki")
                {
                    // label-ek beállítása.
                    label1.Text = "Kilépés";
                    label48.Text = belepo.Kodja.ToString();

                    // Megfelelő fül betöltése.
                    tabControl1.TabPages.Remove(tabPage1);

                    switch (belepo.Beosztasa.ToString())
                    {
                        case "hallgato":
                            tabControl1.TabPages.Add(tabPage2);
                            break;
                        case "oktato":
                            tabControl1.TabPages.Add(tabPage3);
                            break;
                        case "admin":
                            tabControl1.TabPages.Add(tabPage4);
                            break;
                    }
                }
            }
            catch
            { }

            // XML kulcsok betöltése.
            if (label1.Text == "Kilépés")
            {
                try
                {
                    string megy = "";

                    // A tárgyak xml kulcsának beállítása.
                    megy = "tárgyak";
                    targy_kulcs = server.Kulcs_Betoltes(megy);

                    // Az üzenetek xml kulcsának beállítása.
                    megy = "üzenetek";
                    uzenet_kulcs = server.Kulcs_Betoltes(megy);

                    // A bejelentkezett felhasználó xml kulcsának beállítása.
					// admin.
                    if (belepo.Beosztasa.ToString() == "admin")
                    {
                        megy = "hallgatók";
                        hallgato_kulcs = server.Kulcs_Betoltes(megy);
                        megy = "oktatók";
                        oktato_kulcs = server.Kulcs_Betoltes(megy);
                        megy = "felhasználók";
                        felhasznalo_kulcs = server.Kulcs_Betoltes(megy);
                    }
                    else
                    {
						// hallgató.
                        if (belepo.Beosztasa.ToString() == "hallgato")
                        {
                            megy = "hallgatók";
                            hallgato_kulcs = server.Kulcs_Betoltes(megy);
                        }
						// oktató.
                        else
                        {
                            megy = "oktatók";
                            oktato_kulcs = server.Kulcs_Betoltes(megy);
                        }
                    }
                }
                catch
                { }
            }
        }

        #endregion Presets

        #region Common

        /// <summary>
        /// Sets the password button.
        /// </summary>
        private void Adatok_Jelszo_Gomb()
        {
            switch (belepo.Beosztasa.ToString())
            {
                case "hallgato":
                    if ((textBox43.Text.ToString() != "") && (textBox43.Text.ToString() != belepo.Jelszava.ToString()))
                    {
                        button41.Enabled = true;
                    }
                    else
                    {
                        button41.Enabled = false;
                    }
                    break;
                case "oktato":
                    if ((textBox44.Text.ToString() != "") && (textBox44.Text.ToString() != belepo.Jelszava.ToString()))
                    {
                        button42.Enabled = true;
                    }
                    else
                    {
                        button42.Enabled = false;
                    }
                    break;
                case "admin":
                    if ((textBox45.Text.ToString() != "") && (textBox45.Text.ToString() != belepo.Jelszava.ToString()))
                    {
                        button43.Enabled = true;
                    }
                    else
                    {
                        button43.Enabled = false;
                    }
                    break;
            }
        }

        /// <summary>
        /// Edits the password.
        /// </summary>
        private void Adatok_Jelszo_Modosítas()
        {
            try
            {
                Felhasznalo mit = new Felhasznalo(belepo.Idje.ToString(), belepo.Torolve.ToString(), belepo.Kodja.ToString(), belepo.Neve.ToString(), belepo.Jelszava.ToString(), belepo.Beosztasa.ToString(), belepo.Emailje.ToString(), belepo.Szuletesi_helye.ToString(), belepo.Szuletesi_ideje.ToString(), belepo.Fizetese.ToString(), belepo.Anyja_neve.ToString(), belepo.Telefonja.ToString(), belepo.Bankszamla.ToString(), belepo.Statusza.ToString(), belepo.Beiratkozva.ToString(), belepo.Neme.ToString(), belepo.Jogviszonya.ToString(), belepo.Lakcime.ToString());
                string ujjelszo = "";

                switch (belepo.Beosztasa.ToString())
                {
                    case "hallgato":
                        ujjelszo = textBox43.Text.ToString();
                        break;
                    case "oktato":
                        ujjelszo = textBox44.Text.ToString();
                        break;
                    case "admin":
                        ujjelszo = textBox45.Text.ToString();
                        break;
                }

                Felhasznalo mire = new Felhasznalo(belepo.Idje.ToString(), belepo.Torolve.ToString(), belepo.Kodja.ToString(), belepo.Neve.ToString(), ujjelszo, belepo.Beosztasa.ToString(), belepo.Emailje.ToString(), belepo.Szuletesi_helye.ToString(), belepo.Szuletesi_ideje.ToString(), belepo.Fizetese.ToString(), belepo.Anyja_neve.ToString(), belepo.Telefonja.ToString(), belepo.Bankszamla.ToString(), belepo.Statusza.ToString(), belepo.Beiratkozva.ToString(), belepo.Neme.ToString(), belepo.Jogviszonya.ToString(), belepo.Lakcime.ToString());
                server.Felhasznalo_Modosit(mit, mire);

                belepo.Jelszava = ujjelszo.ToString();

                // Frissítés.
                switch (belepo.Beosztasa.ToString())
                {
                    case "hallgato":
                        Hallgato_Adatok_Betoltes();
                        break;
                    case "oktato":
                        Oktato_Adatok_Betoltes();
                        break;
                    case "admin":
                        Admin_Adatok_Betoltes();
                        break;
                }
            }
            catch
            { }
        }

        #region Uzenetek

        /// <summary>
        /// Counts the incoming messages.
        /// </summary>
        private void Uzenetek_Bejovo_Olvasatlan_Betoltes()
        {
            try
            {
                // Az olvasatlan üzenetek számának megszerzése.
                string megy = "uzenetek_bejovo_olvasatlan";
                string kod = belepo.Kodja.ToString();
                Uzenet[] eredmeny = server.Uzenet_Lista(megy, "", kod, "", "", "");

                // Az olvasatlan üzenetek számának kiírása.
                switch (belepo.Beosztasa.ToString())
                {
                    case "hallgato":
                        if (eredmeny.Length > 0)
                        {
                            tabPage25.Text = "Beérkezett üzenetek (" + eredmeny.Length.ToString() + ")";
                            // Félkövér...
                        }
                        else
                        {
                            tabPage25.Text = "Beérkezett üzenetek";
                            // Nem félkövér...
                        }
                        break;
                    case "oktato":
                        if (eredmeny.Length > 0)
                        {
                            tabPage9.Text = "Beérkezett üzenetek (" + eredmeny.Length.ToString() + ")";
                            // Félkövér...
                        }
                        else
                        {
                            tabPage9.Text = "Beérkezett üzenetek";
                            // Nem félkövér...
                        }
                        break;
                    case "admin":
                        if (eredmeny.Length > 0)
                        {
                            tabPage31.Text = "Beérkezett üzenetek (" + eredmeny.Length.ToString() + ")";
                            // Félkövér...
                        }
                        else
                        {
                            tabPage31.Text = "Beérkezett üzenetek";
                            // Nem félkövér...
                        }
                        break;
                }
            }
            catch
            { }
        }

        /// <summary>
        /// Select an incoming message.
        /// </summary>
        private void Uzenetek_Beerkezett_Kijeloles()
        {
            switch (belepo.Beosztasa.ToString())
            {
                case "hallgato":
                    richTextBox1.Text = "";

                    // Kijelölés esetén engedélyezzük a gombot.
                    if (dataGridView3.CurrentCellAddress.X >= 0)
                    {
                        try
                        {
                            button9.Enabled = true;

                            // Üzenetazonosítók megszerzése.
                            int kijeloltsorindex = dataGridView3.SelectedCells[0].RowIndex;
                            DataGridViewRow kijeloltsor = dataGridView3.Rows[kijeloltsorindex];
                            uzenet_id1 = kijeloltsor.Cells["kuldesido"].Value.ToString();
                            kijeloltsorindex = dataGridView3.SelectedCells[0].RowIndex;
                            kijeloltsor = dataGridView3.Rows[kijeloltsorindex];
                            uzenet_id2 = kijeloltsor.Cells["felado"].Value.ToString();
                            kijeloltsorindex = dataGridView3.SelectedCells[0].RowIndex;
                            kijeloltsor = dataGridView3.Rows[kijeloltsorindex];
                            uzenet_id3 = kijeloltsor.Cells["targy"].Value.ToString();
                            kijeloltsorindex = dataGridView3.SelectedCells[0].RowIndex;
                            kijeloltsor = dataGridView3.Rows[kijeloltsorindex];
                            uzenet_id4 = kijeloltsor.Cells["be_szoveg"].Value.ToString();

                            // Üzenet Olvasása.
                            string kod = belepo.Kodja.ToString();
                            string megy = "uzenetek_beerkezett_kijeloles";
                            Uzenet[] eredmeny = server.Uzenet_Lista(megy, uzenet_id2, kod, uzenet_id3, uzenet_id4, uzenet_id1);
                            richTextBox1.Text = eredmeny[0].Szovege.ToString();

                            // Megnyitás esetén olvasott státusz változtatása.
                            if (eredmeny[0].Olvasva_e.ToString() == "false")
                            {
                                Uzenet mit = new Uzenet(eredmeny[0].Ideje.ToString(), eredmeny[0].Torolve.ToString(), eredmeny[0].Kuldoje.ToString(), eredmeny[0].Cimzettje.ToString(), eredmeny[0].Targya.ToString(), eredmeny[0].Szovege.ToString(), eredmeny[0].Megvan_kuldonek.ToString(), eredmeny[0].Megvan_cimzettnek.ToString(), eredmeny[0].Mentve_e.ToString(), eredmeny[0].Olvasva_e.ToString(), eredmeny[0].Idje.ToString());
                                Uzenet mire = new Uzenet(eredmeny[0].Ideje.ToString(), eredmeny[0].Torolve.ToString(), eredmeny[0].Kuldoje.ToString(), eredmeny[0].Cimzettje.ToString(), eredmeny[0].Targya.ToString(), eredmeny[0].Szovege.ToString(), eredmeny[0].Megvan_kuldonek.ToString(), eredmeny[0].Megvan_cimzettnek.ToString(), eredmeny[0].Mentve_e.ToString(), "true", eredmeny[0].Idje.ToString());

                                server.Uzenet_Modosit(mit, mire);
                            }
                        }
                        catch
                        { }
                    }
                    else
                    {
                        button9.Enabled = false;
                    }
                    break;
                case "oktato":
                    richTextBox5.Text = "";

                    // Kijelölés esetén engedélyezzük a gombot.
                    if (dataGridView6.CurrentCellAddress.X >= 0)
                    {
                        try
                        {
                            button11.Enabled = true;

                            // Üzenetazonosítók megszerzése.
                            int kijeloltsorindex = dataGridView6.SelectedCells[0].RowIndex;
                            DataGridViewRow kijeloltsor = dataGridView6.Rows[kijeloltsorindex];
                            uzenet_id1 = kijeloltsor.Cells["dataGridViewTextBoxColumn6"].Value.ToString();
                            kijeloltsorindex = dataGridView6.SelectedCells[0].RowIndex;
                            kijeloltsor = dataGridView6.Rows[kijeloltsorindex];
                            uzenet_id2 = kijeloltsor.Cells["dataGridViewTextBoxColumn4"].Value.ToString();
                            kijeloltsorindex = dataGridView6.SelectedCells[0].RowIndex;
                            kijeloltsor = dataGridView6.Rows[kijeloltsorindex];
                            uzenet_id3 = kijeloltsor.Cells["dataGridViewTextBoxColumn5"].Value.ToString();
                            kijeloltsorindex = dataGridView6.SelectedCells[0].RowIndex;
                            kijeloltsor = dataGridView6.Rows[kijeloltsorindex];
                            uzenet_id4 = kijeloltsor.Cells["Column46"].Value.ToString();

                            // Üzenet Olvasása.
                            string kod = belepo.Kodja.ToString();
                            string megy = "uzenetek_beerkezett_kijeloles";
                            Uzenet[] eredmeny = server.Uzenet_Lista(megy, uzenet_id2, kod, uzenet_id3, uzenet_id4, uzenet_id1);
                            richTextBox5.Text = eredmeny[0].Szovege.ToString();

                            // Megnyitás esetén olvasott státusz változtatása.
                            if (eredmeny[0].Olvasva_e.ToString() == "false")
                            {
                                Uzenet mit = new Uzenet(eredmeny[0].Ideje.ToString(), eredmeny[0].Torolve.ToString(), eredmeny[0].Kuldoje.ToString(), eredmeny[0].Cimzettje.ToString(), eredmeny[0].Targya.ToString(), eredmeny[0].Szovege.ToString(), eredmeny[0].Megvan_kuldonek.ToString(), eredmeny[0].Megvan_cimzettnek.ToString(), eredmeny[0].Mentve_e.ToString(), eredmeny[0].Olvasva_e.ToString(), eredmeny[0].Idje.ToString());
                                Uzenet mire = new Uzenet(eredmeny[0].Ideje.ToString(), eredmeny[0].Torolve.ToString(), eredmeny[0].Kuldoje.ToString(), eredmeny[0].Cimzettje.ToString(), eredmeny[0].Targya.ToString(), eredmeny[0].Szovege.ToString(), eredmeny[0].Megvan_kuldonek.ToString(), eredmeny[0].Megvan_cimzettnek.ToString(), eredmeny[0].Mentve_e.ToString(), "true", eredmeny[0].Idje.ToString());

                                server.Uzenet_Modosit(mit, mire);
                            }
                        }
                        catch
                        { }
                    }
                    else
                    {
                        button11.Enabled = false;
                    }
                    break;
                case "admin":
                    richTextBox9.Text = "";

                    // Kijelölés esetén engedélyezzük a gombot.
                    if (dataGridView12.CurrentCellAddress.X >= 0)
                    {
                        try
                        {
                            button22.Enabled = true;

                            // Üzenetazonosítók megszerzése.
                            int kijeloltsorindex = dataGridView6.SelectedCells[0].RowIndex;
                            DataGridViewRow kijeloltsor = dataGridView6.Rows[kijeloltsorindex];
                            uzenet_id1 = kijeloltsor.Cells["dataGridViewTextBoxColumn14"].Value.ToString();
                            kijeloltsorindex = dataGridView6.SelectedCells[0].RowIndex;
                            kijeloltsor = dataGridView6.Rows[kijeloltsorindex];
                            uzenet_id2 = kijeloltsor.Cells["dataGridViewTextBoxColumn12"].Value.ToString();
                            kijeloltsorindex = dataGridView6.SelectedCells[0].RowIndex;
                            kijeloltsor = dataGridView6.Rows[kijeloltsorindex];
                            uzenet_id3 = kijeloltsor.Cells["dataGridViewTextBoxColumn13"].Value.ToString();
                            kijeloltsorindex = dataGridView6.SelectedCells[0].RowIndex;
                            kijeloltsor = dataGridView6.Rows[kijeloltsorindex];
                            uzenet_id4 = kijeloltsor.Cells["Column47"].Value.ToString();

                            // Üzenet Olvasása.
                            string kod = belepo.Kodja.ToString();
                            string megy = "uzenetek_beerkezett_kijeloles";
                            Uzenet[] eredmeny = server.Uzenet_Lista(megy, uzenet_id2, kod, uzenet_id3, uzenet_id4, uzenet_id1);
                            richTextBox9.Text = eredmeny[0].Szovege.ToString();

                            // Megnyitás esetén olvasott státusz változtatása.
                            if (eredmeny[0].Olvasva_e.ToString() == "false")
                            {
                                Uzenet mit = new Uzenet(eredmeny[0].Ideje.ToString(), eredmeny[0].Torolve.ToString(), eredmeny[0].Kuldoje.ToString(), eredmeny[0].Cimzettje.ToString(), eredmeny[0].Targya.ToString(), eredmeny[0].Szovege.ToString(), eredmeny[0].Megvan_kuldonek.ToString(), eredmeny[0].Megvan_cimzettnek.ToString(), eredmeny[0].Mentve_e.ToString(), eredmeny[0].Olvasva_e.ToString(), eredmeny[0].Idje.ToString());
                                Uzenet mire = new Uzenet(eredmeny[0].Ideje.ToString(), eredmeny[0].Torolve.ToString(), eredmeny[0].Kuldoje.ToString(), eredmeny[0].Cimzettje.ToString(), eredmeny[0].Targya.ToString(), eredmeny[0].Szovege.ToString(), eredmeny[0].Megvan_kuldonek.ToString(), eredmeny[0].Megvan_cimzettnek.ToString(), eredmeny[0].Mentve_e.ToString(), "true", eredmeny[0].Idje.ToString());

                                server.Uzenet_Modosit(mit, mire);
                            }
                        }
                        catch
                        { }
                    }
                    else
                    {
                        button22.Enabled = false;
                    }
                    break;
            }
        }

        /// <summary>
        /// Deletes an incoming message.
        /// </summary>
        private void Uzenetek_Beerkezett_Torles()
        {
            try
            {
                string megy = "uzenetek_beerkezett_torles";
                string kod = belepo.Kodja.ToString();
                Uzenet[] eredmeny = server.Uzenet_Lista(megy, uzenet_id2, kod, uzenet_id3, uzenet_id4, uzenet_id1);

                if (eredmeny.Length == 1)
                {
                    Uzenet mit = new Uzenet(eredmeny[0].Ideje.ToString(), eredmeny[0].Torolve.ToString(), eredmeny[0].Kuldoje.ToString(), eredmeny[0].Cimzettje.ToString(), eredmeny[0].Targya.ToString(), eredmeny[0].Szovege.ToString(), eredmeny[0].Megvan_kuldonek.ToString(), eredmeny[0].Megvan_cimzettnek.ToString(), eredmeny[0].Mentve_e.ToString(), eredmeny[0].Olvasva_e.ToString(), eredmeny[0].Idje.ToString());
                    Uzenet mire = new Uzenet(eredmeny[0].Ideje.ToString(), eredmeny[0].Torolve.ToString(), eredmeny[0].Kuldoje.ToString(), eredmeny[0].Cimzettje.ToString(), eredmeny[0].Targya.ToString(), eredmeny[0].Szovege.ToString(), eredmeny[0].Megvan_kuldonek.ToString(), "false", eredmeny[0].Mentve_e.ToString(), eredmeny[0].Olvasva_e.ToString(), eredmeny[0].Idje.ToString());
                    server.Uzenet_Modosit(mit, mire);
                }
            }
            catch
            { }

            // Frissítés.
            switch (belepo.Beosztasa.ToString())
            {
                case "hallgato":
                    Hallgato_Uzenetek_Bejovo_Betoltes();
                    break;
                case "oktato":
                    Oktato_Uzenetek_Bejovo_Betoltes();
                    break;
                case "admin":
                    Admin_Uzenetek_Bejovo_Betoltes();
                    break;
            }
        }

        /// <summary>
        /// Select a sent message.
        /// </summary>
        private void Uzenetek_Elkuldott_Kijeloles()
        {
            switch (belepo.Beosztasa.ToString())
            {
                case "hallgato":
                    richTextBox2.Text = "";

                    // Kijelölés esetén engedélyezzük a gombot.
                    if (dataGridView4.CurrentCellAddress.X >= 0)
                    {
                        try
                        {
                            button10.Enabled = true;

                            // Üzenetazonosítók megszerzése.
                            int kijeloltsorindex = dataGridView4.SelectedCells[0].RowIndex;
                            DataGridViewRow kijeloltsor = dataGridView4.Rows[kijeloltsorindex];
                            uzenet_id1 = kijeloltsor.Cells["kuldesideje"].Value.ToString();
                            kijeloltsorindex = dataGridView4.SelectedCells[0].RowIndex;
                            kijeloltsor = dataGridView4.Rows[kijeloltsorindex];
                            uzenet_id2 = kijeloltsor.Cells["cimzett"].Value.ToString();
                            kijeloltsorindex = dataGridView4.SelectedCells[0].RowIndex;
                            kijeloltsor = dataGridView4.Rows[kijeloltsorindex];
                            uzenet_id3 = kijeloltsor.Cells["targya"].Value.ToString();
                            kijeloltsorindex = dataGridView4.SelectedCells[0].RowIndex;
                            kijeloltsor = dataGridView4.Rows[kijeloltsorindex];
                            uzenet_id4 = kijeloltsor.Cells["ki_szoveg"].Value.ToString();

                            // Üzenet Olvasása.
                            string megy = "uzenetek_elkuldott_kijeloles";
                            string kod = belepo.Kodja.ToString();
                            Uzenet[] eredmeny = server.Uzenet_Lista(megy, kod, uzenet_id2, uzenet_id3, uzenet_id4, uzenet_id1);
                            richTextBox2.Text = eredmeny[0].Szovege.ToString();
                        }
                        catch
                        { }
                    }
                    else
                    {
                        button10.Enabled = false;
                    }
                    break;
                case "oktato":
                    richTextBox6.Text = "";

                    // Kijelölés esetén engedélyezzük a gombot.
                    if (dataGridView10.CurrentCellAddress.X >= 0)
                    {
                        try
                        {
                            button15.Enabled = true;

                            // Üzenetazonosítók megszerzése.
                            int kijeloltsorindex = dataGridView10.SelectedCells[0].RowIndex;
                            DataGridViewRow kijeloltsor = dataGridView10.Rows[kijeloltsorindex];
                            uzenet_id1 = kijeloltsor.Cells["kuldesideje"].Value.ToString();
                            kijeloltsorindex = dataGridView10.SelectedCells[0].RowIndex;
                            kijeloltsor = dataGridView10.Rows[kijeloltsorindex];
                            uzenet_id2 = kijeloltsor.Cells["cimzett"].Value.ToString();
                            kijeloltsorindex = dataGridView10.SelectedCells[0].RowIndex;
                            kijeloltsor = dataGridView10.Rows[kijeloltsorindex];
                            uzenet_id3 = kijeloltsor.Cells["targya"].Value.ToString();
                            kijeloltsorindex = dataGridView10.SelectedCells[0].RowIndex;
                            kijeloltsor = dataGridView10.Rows[kijeloltsorindex];
                            uzenet_id4 = kijeloltsor.Cells["ki_szoveg"].Value.ToString();

                            // Üzenet Olvasása.
                            string megy = "uzenetek_elkuldott_kijeloles";
                            string kod = belepo.Kodja.ToString();
                            Uzenet[] eredmeny = server.Uzenet_Lista(megy, kod, uzenet_id2, uzenet_id3, uzenet_id4, uzenet_id1);
                            richTextBox6.Text = eredmeny[0].Szovege.ToString();
                        }
                        catch
                        { }
                    }
                    else
                    {
                        button15.Enabled = false;
                    }
                    break;
                case "admin":
                    richTextBox10.Text = "";

                    // Kijelölés esetén engedélyezzük a gombot.
                    if (dataGridView13.CurrentCellAddress.X >= 0)
                    {
                        try
                        {
                            button23.Enabled = true;

                            // Üzenetazonosítók megszerzése.
                            int kijeloltsorindex = dataGridView13.SelectedCells[0].RowIndex;
                            DataGridViewRow kijeloltsor = dataGridView13.Rows[kijeloltsorindex];
                            uzenet_id1 = kijeloltsor.Cells["kuldesideje"].Value.ToString();
                            kijeloltsorindex = dataGridView13.SelectedCells[0].RowIndex;
                            kijeloltsor = dataGridView13.Rows[kijeloltsorindex];
                            uzenet_id2 = kijeloltsor.Cells["cimzett"].Value.ToString();
                            kijeloltsorindex = dataGridView13.SelectedCells[0].RowIndex;
                            kijeloltsor = dataGridView13.Rows[kijeloltsorindex];
                            uzenet_id3 = kijeloltsor.Cells["targya"].Value.ToString();
                            kijeloltsorindex = dataGridView13.SelectedCells[0].RowIndex;
                            kijeloltsor = dataGridView13.Rows[kijeloltsorindex];
                            uzenet_id4 = kijeloltsor.Cells["ki_szoveg"].Value.ToString();

                            // Üzenet Olvasása.
                            string megy = "uzenetek_elkuldott_kijeloles";
                            string kod = belepo.Kodja.ToString();
                            Uzenet[] eredmeny = server.Uzenet_Lista(megy, kod, uzenet_id2, uzenet_id3, uzenet_id4, uzenet_id1);
                            richTextBox10.Text = eredmeny[0].Szovege.ToString();
                        }
                        catch
                        { }
                    }
                    else
                    {
                        button23.Enabled = false;
                    }
                    break;
            }
        }

        /// <summary>
        /// Deletes a sent message.
        /// </summary>
        private void Uzenetek_Elkuldott_Torles()
        {
            try
            {
                string megy = "uzenetek_elkuldott_torles";
                string kod = belepo.Kodja.ToString();
                Uzenet[] eredmeny = server.Uzenet_Lista(megy, kod, uzenet_id2, uzenet_id3, uzenet_id4, uzenet_id1);

                if (eredmeny.Length == 1)
                {
                    Uzenet mit = new Uzenet(eredmeny[0].Ideje.ToString(), eredmeny[0].Torolve.ToString(), eredmeny[0].Kuldoje.ToString(), eredmeny[0].Cimzettje.ToString(), eredmeny[0].Targya.ToString(), eredmeny[0].Szovege.ToString(), eredmeny[0].Megvan_kuldonek.ToString(), eredmeny[0].Megvan_cimzettnek.ToString(), eredmeny[0].Mentve_e.ToString(), eredmeny[0].Olvasva_e.ToString(), eredmeny[0].Idje.ToString());
                    Uzenet mire = new Uzenet(eredmeny[0].Ideje.ToString(), eredmeny[0].Torolve.ToString(), eredmeny[0].Kuldoje.ToString(), eredmeny[0].Cimzettje.ToString(), eredmeny[0].Targya.ToString(), eredmeny[0].Szovege.ToString(), eredmeny[0].Megvan_kuldonek.ToString(), "false", eredmeny[0].Mentve_e.ToString(), eredmeny[0].Olvasva_e.ToString(), eredmeny[0].Idje.ToString());

                    server.Uzenet_Modosit(mit, mire);
                }
            }
            catch
            { }

            // Frissítés.
            switch (belepo.Beosztasa.ToString())
            {
                case "hallgato":
                    Hallgato_Uzenetek_Elkuldott_Betoltes();
                    break;
                case "oktato":
                    Oktato_Uzenetek_Elkuldott_Betoltes();
                    break;
                case "admin":
                    Admin_Uzenetek_Elkuldott_Betoltes();
                    break;
            }
        }

        /// <summary>
        /// Selet a draft message.
        /// </summary>
        private void Uzenetek_Piszkozat_Kijeloles()
        {
            switch (belepo.Beosztasa.ToString())
            {
                case "hallgato":
                    richTextBox4.Text = "";

                    // Kijelölés esetén engedélyezzük a gombot.
                    if (dataGridView5.CurrentCellAddress.X >= 0)
                    {
                        try
                        {
                            button8.Enabled = true;

                            // Üzenetazonosítók megszerzése.
                            int kijeloltsorindex = dataGridView5.SelectedCells[0].RowIndex;
                            DataGridViewRow kijeloltsor = dataGridView5.Rows[kijeloltsorindex];
                            uzenet_id1 = kijeloltsor.Cells["ido"].Value.ToString();
                            kijeloltsorindex = dataGridView5.SelectedCells[0].RowIndex;
                            kijeloltsor = dataGridView5.Rows[kijeloltsorindex];
                            uzenet_id2 = kijeloltsor.Cells["cimzet"].Value.ToString();
                            kijeloltsorindex = dataGridView5.SelectedCells[0].RowIndex;
                            kijeloltsor = dataGridView5.Rows[kijeloltsorindex];
                            uzenet_id3 = kijeloltsor.Cells["targ"].Value.ToString();
                            kijeloltsorindex = dataGridView5.SelectedCells[0].RowIndex;
                            kijeloltsor = dataGridView5.Rows[kijeloltsorindex];
                            uzenet_id4 = kijeloltsor.Cells["szoveg"].Value.ToString();

                            // Üzenet Olvasása
                            string megy = "uzenetek_piszkozat_kijeloles";
                            string kod = belepo.Kodja.ToString();
                            Uzenet[] eredmeny = server.Uzenet_Lista(megy, kod, uzenet_id2, uzenet_id3, uzenet_id4, uzenet_id1);
                            textBox9.Text = eredmeny[0].Cimzettje.ToString();
                            textBox8.Text = eredmeny[0].Targya.ToString();
                            richTextBox4.Text = eredmeny[0].Szovege.ToString();

                            // Gombok láthatóságának beállítása.
                            if (textBox9.Text != "")
                            {
                                button6.Enabled = true;
                            }
                            else
                            {
                                button6.Enabled = false;
                            }
                            if (cim_valtozas || targy_valtozas || szoveg_valtozas)
                            {
                                button7.Enabled = true;
                            }
                            else
                            {
                                button7.Enabled = false;
                            }
                        }
                        catch
                        { }
                    }
                    else
                    {
                        button6.Enabled = false;
                        button7.Enabled = false;
                        button8.Enabled = false;
                    }
                    break;
                case "oktato":
                    richTextBox7.Text = "";

                    // Kijelölés esetén engedélyezzük a gombot.
                    if (dataGridView11.CurrentCellAddress.X >= 0)
                    {
                        try
                        {
                            button16.Enabled = true;

                            // Üzenetazonosítók megszerzése.
                            int kijeloltsorindex = dataGridView11.SelectedCells[0].RowIndex;
                            DataGridViewRow kijeloltsor = dataGridView11.Rows[kijeloltsorindex];
                            uzenet_id1 = kijeloltsor.Cells["dataGridViewTextBoxColumn11"].Value.ToString();
                            kijeloltsorindex = dataGridView11.SelectedCells[0].RowIndex;
                            kijeloltsor = dataGridView11.Rows[kijeloltsorindex];
                            uzenet_id2 = kijeloltsor.Cells["dataGridViewTextBoxColumn10"].Value.ToString();
                            kijeloltsorindex = dataGridView11.SelectedCells[0].RowIndex;
                            kijeloltsor = dataGridView11.Rows[kijeloltsorindex];
                            uzenet_id3 = kijeloltsor.Cells["Column49"].Value.ToString();
                            kijeloltsorindex = dataGridView11.SelectedCells[0].RowIndex;
                            kijeloltsor = dataGridView11.Rows[kijeloltsorindex];
                            uzenet_id4 = kijeloltsor.Cells["Column50"].Value.ToString();

                            // Üzenet Olvasása.
                            string megy = "uzenetek_piszkozat_kijeloles";
                            string kod = belepo.Kodja.ToString();
                            Uzenet[] eredmeny = server.Uzenet_Lista(megy, kod, uzenet_id2, uzenet_id3, uzenet_id4, uzenet_id1);
                            textBox11.Text = eredmeny[0].Cimzettje.ToString();
                            textBox10.Text = eredmeny[0].Targya.ToString();
                            richTextBox7.Text = eredmeny[0].Szovege.ToString();

                            // Gombok láthatóságának beállítása.
                            if (textBox11.Text != "")
                            {
                                button18.Enabled = true;
                            }
                            else
                            {
                                button18.Enabled = false;
                            }
                            if (cim_valtozas || targy_valtozas || szoveg_valtozas)
                            {
                                button17.Enabled = true;
                            }
                            else
                            {
                                button17.Enabled = false;
                            }
                        }
                        catch
                        { }
                    }
                    else
                    {
                        button16.Enabled = false;
                        button17.Enabled = false;
                        button18.Enabled = false;
                    }
                    break;
                case "admin":
                    richTextBox11.Text = "";

                    // Kijelölés esetén engedélyezzük a gombot.
                    if (dataGridView14.CurrentCellAddress.X >= 0)
                    {
                        try
                        {
                            button26.Enabled = true;

                            // Üzenetazonosítók megszerzése.
                            int kijeloltsorindex = dataGridView14.SelectedCells[0].RowIndex;
                            DataGridViewRow kijeloltsor = dataGridView14.Rows[kijeloltsorindex];
                            uzenet_id1 = kijeloltsor.Cells["dataGridViewTextBoxColumn11"].Value.ToString();
                            kijeloltsorindex = dataGridView14.SelectedCells[0].RowIndex;
                            kijeloltsor = dataGridView14.Rows[kijeloltsorindex];
                            uzenet_id2 = kijeloltsor.Cells["dataGridViewTextBoxColumn10"].Value.ToString();
                            kijeloltsorindex = dataGridView14.SelectedCells[0].RowIndex;
                            kijeloltsor = dataGridView14.Rows[kijeloltsorindex];
                            uzenet_id3 = kijeloltsor.Cells["Column49"].Value.ToString();
                            kijeloltsorindex = dataGridView14.SelectedCells[0].RowIndex;
                            kijeloltsor = dataGridView14.Rows[kijeloltsorindex];
                            uzenet_id4 = kijeloltsor.Cells["Column50"].Value.ToString();

                            // Üzenet Olvasása.
                            string megy = "uzenetek_piszkozat_kijeloles";
                            string kod = belepo.Kodja.ToString();
                            Uzenet[] eredmeny = server.Uzenet_Lista(megy, kod, uzenet_id2, uzenet_id3, uzenet_id4, uzenet_id1);
                            textBox87.Text = eredmeny[0].Cimzettje.ToString();
                            textBox86.Text = eredmeny[0].Targya.ToString();
                            richTextBox11.Text = eredmeny[0].Szovege.ToString();

                            // Gombok láthatóságának beállítása.
                            if (textBox87.Text != "")
                            {
                                button24.Enabled = true;
                            }
                            else
                            {
                                button24.Enabled = false;
                            }
                            if (cim_valtozas || targy_valtozas || szoveg_valtozas)
                            {
                                button25.Enabled = true;
                            }
                            else
                            {
                                button25.Enabled = false;
                            }
                        }
                        catch
                        { }
                    }
                    else
                    {
                        button24.Enabled = false;
                        button25.Enabled = false;
                        button26.Enabled = false;
                    }
                    break;
            }
        }

        /// <summary>
        /// Draft message assignnee text change.
        /// </summary>
        private void Uzenetek_Piszkozat_Cimzett()
        {
            switch (belepo.Beosztasa.ToString())
            {
                case "hallgato":
                    if (textBox9.Text.ToString() != "")
                    {
                        cim_valtozas = true;
                    }
                    else
                    {
                        cim_valtozas = false;
                    }
                    break;
                case "oktato":
                    if (textBox11.Text.ToString() != "")
                    {
                        cim_valtozas = true;
                    }
                    else
                    {
                        cim_valtozas = false;
                    }
                    break;
                case "admin":
                    if (textBox87.Text.ToString() != "")
                    {
                        cim_valtozas = true;
                    }
                    else
                    {
                        cim_valtozas = false;
                    }
                    break;
            }
        }

        /// <summary>
        /// Draft message subject text change.
        /// </summary>
        private void Uzenetek_Piszkozat_Targy()
        {
            switch (belepo.Beosztasa.ToString())
            {
                case "hallgato":
                    if (textBox8.Text.ToString() != "")
                    {
                        targy_valtozas = true;
                    }
                    else
                    {
                        targy_valtozas = false;
                    }
                    break;
                case "oktato":
                    if (textBox10.Text.ToString() != "")
                    {
                        targy_valtozas = true;
                    }
                    else
                    {
                        targy_valtozas = false;
                    }
                    break;
                case "admin":
                    if (textBox86.Text.ToString() != "")
                    {
                        targy_valtozas = true;
                    }
                    else
                    {
                        targy_valtozas = false;
                    }
                    break;
            }
        }

        /// <summary>
        /// Drafe message text change.
        /// </summary>
        private void Uzenetek_Piszkozat_Szoveg()
        {
            switch (belepo.Beosztasa.ToString())
            {
                case "hallgato":
                    if (richTextBox4.Text.ToString() != "")
                    {
                        szoveg_valtozas = true;
                    }
                    else
                    {
                        szoveg_valtozas = false;
                    }
                    break;
                case "oktato":
                    if (richTextBox7.Text.ToString() != "")
                    {
                        szoveg_valtozas = true;
                    }
                    else
                    {
                        szoveg_valtozas = false;
                    }
                    break;
                case "admin":
                    if (richTextBox11.Text.ToString() != "")
                    {
                        szoveg_valtozas = true;
                    }
                    else
                    {
                        szoveg_valtozas = false;
                    }
                    break;
            }
        }

        /// <summary>
        /// Sends a draft message.
        /// </summary>
        private void Uzenetek_Piszkozat_Kuldes()
        {
            try
            {
                string megy = "uzenetek_piszkozat_kuldes";
                string kod = belepo.Kodja.ToString();
                Uzenet[] eredmeny = server.Uzenet_Lista(megy, kod, uzenet_id2, uzenet_id3, uzenet_id4, uzenet_id1);

                if (eredmeny.Length == 1)
                {
                    Uzenet mit = new Uzenet(eredmeny[0].Ideje.ToString(), eredmeny[0].Torolve.ToString(), eredmeny[0].Kuldoje.ToString(), eredmeny[0].Cimzettje.ToString(), eredmeny[0].Targya.ToString(), eredmeny[0].Szovege.ToString(), eredmeny[0].Megvan_kuldonek.ToString(), eredmeny[0].Megvan_cimzettnek.ToString(), eredmeny[0].Mentve_e.ToString(), eredmeny[0].Olvasva_e.ToString(), eredmeny[0].Idje.ToString());
                    DateTime idoo = DateTime.Now;
                    string formatum = "yyyy.MM.dd,HH:mm:ss";
                    string cimzett = "";
                    string targy = "";
                    string szoveg = "";

                    switch (belepo.Beosztasa.ToString())
                    {
                        case "hallgato":
                            cimzett = textBox9.Text.ToString();
                            targy = textBox8.Text.ToString();
                            szoveg = richTextBox4.Text.ToString();
                            break;
                        case "oktato":
                            cimzett = textBox11.Text.ToString();
                            targy = textBox10.Text.ToString();
                            szoveg = richTextBox7.Text.ToString();
                            break;
                        case "admin":
                            cimzett = textBox87.Text.ToString();
                            targy = textBox86.Text.ToString();
                            szoveg = richTextBox11.Text.ToString();
                            break;
                    }

                    Uzenet mire = new Uzenet(idoo.ToString(formatum), eredmeny[0].Torolve.ToString(), eredmeny[0].Kuldoje.ToString(), cimzett.ToString(), targy.ToString(), szoveg.ToString(), eredmeny[0].Megvan_kuldonek.ToString(), "true", "false", "false", eredmeny[0].Idje.ToString());
                    server.Uzenet_Modosit(mit, mire);
                }

                cim_valtozas = false;
                targy_valtozas = false;
                szoveg_valtozas = false;
            }
            catch
            { }

            // Frissítés.
            switch (belepo.Beosztasa.ToString())
            {
                case "hallgato":
                    Hallgato_Uzenetek_Piszkozat_Betoltes();
                    break;
                case "oktato":
                    Oktato_Uzenetek_Piszkozat_Betoltes();
                    break;
                case "admin":
                    Admin_Uzenetek_Piszkozat_Betoltes();
                    break;
            }
        }

        /// <summary>
        /// Saves a draft message.
        /// </summary>
        private void Uzenetek_Piszkozat_Mentes()
        {
            try
            {
                string megy = "uzenetek_piszkozat_mentes";
                string kod = belepo.Kodja.ToString();
                Uzenet[] eredmeny = server.Uzenet_Lista(megy, kod, uzenet_id2, uzenet_id3, uzenet_id4, uzenet_id1);

                if (eredmeny.Length == 1)
                {
                    Uzenet mit = new Uzenet(eredmeny[0].Ideje.ToString(), eredmeny[0].Torolve.ToString(), eredmeny[0].Kuldoje.ToString(), eredmeny[0].Cimzettje.ToString(), eredmeny[0].Targya.ToString(), eredmeny[0].Szovege.ToString(), eredmeny[0].Megvan_kuldonek.ToString(), eredmeny[0].Megvan_cimzettnek.ToString(), eredmeny[0].Mentve_e.ToString(), eredmeny[0].Olvasva_e.ToString(), eredmeny[0].Idje.ToString());
                    DateTime idoo = DateTime.Now;
                    string formatum = "yyyy.MM.dd,HH:mm:ss";
                    string cimzett = "";
                    string targy = "";
                    string szoveg = "";

                    switch (belepo.Beosztasa.ToString())
                    {
                        case "hallgato":
                            cimzett = textBox9.Text.ToString();
                            targy = textBox8.Text.ToString();
                            szoveg = richTextBox4.Text.ToString();
                            break;
                        case "oktato":
                            cimzett = textBox11.Text.ToString();
                            targy = textBox10.Text.ToString();
                            szoveg = richTextBox7.Text.ToString();
                            break;
                        case "admin":
                            cimzett = textBox87.Text.ToString();
                            targy = textBox86.Text.ToString();
                            szoveg = richTextBox11.Text.ToString();
                            break;
                    }

                    Uzenet mire = new Uzenet(idoo.ToString(formatum), eredmeny[0].Torolve.ToString(), eredmeny[0].Kuldoje.ToString(), cimzett.ToString(), targy.ToString(), szoveg.ToString(), eredmeny[0].Megvan_kuldonek.ToString(), eredmeny[0].Megvan_cimzettnek.ToString(), eredmeny[0].Mentve_e.ToString(), eredmeny[0].Olvasva_e.ToString(), eredmeny[0].Idje.ToString());
                    server.Uzenet_Modosit(mit, mire);
                }

                cim_valtozas = false;
                targy_valtozas = false;
                szoveg_valtozas = false;
            }
            catch
            { }

            // Frissítés.
            switch (belepo.Beosztasa.ToString())
            {
                case "hallgato":
                    Hallgato_Uzenetek_Piszkozat_Betoltes();
                    break;
                case "oktato":
                    Oktato_Uzenetek_Piszkozat_Betoltes();
                    break;
                case "admin":
                    Admin_Uzenetek_Piszkozat_Betoltes();
                    break;
            }
        }

        /// <summary>
        /// Deletes a draft message.
        /// </summary>
        private void Uzenetek_Piszkozat_Torles()
        {
            try
            {
                string megy = "uzenetek_piszkozat_torles";
                string kod = belepo.Kodja.ToString();
                Uzenet[] eredmeny = server.Uzenet_Lista(megy, kod, uzenet_id2, uzenet_id3, uzenet_id4, uzenet_id1);

                if (eredmeny.Length == 1)
                {
                    Uzenet mit = new Uzenet(eredmeny[0].Ideje.ToString(), eredmeny[0].Torolve.ToString(), eredmeny[0].Kuldoje.ToString(), eredmeny[0].Cimzettje.ToString(), eredmeny[0].Targya.ToString(), eredmeny[0].Szovege.ToString(), eredmeny[0].Megvan_kuldonek.ToString(), eredmeny[0].Megvan_cimzettnek.ToString(), eredmeny[0].Mentve_e.ToString(), eredmeny[0].Olvasva_e.ToString(), eredmeny[0].Idje.ToString());
                    server.Uzenet_Torles(mit);
                }

                cim_valtozas = false;
                targy_valtozas = false;
                szoveg_valtozas = false;
            }
            catch
            { }

            // Frissítés.
            switch (belepo.Beosztasa.ToString())
            {
                case "hallgato":
                    Hallgato_Uzenetek_Piszkozat_Betoltes();
                    break;
                case "oktato":
                    Oktato_Uzenetek_Piszkozat_Betoltes();
                    break;
                case "admin":
                    Admin_Uzenetek_Piszkozat_Betoltes();
                    break;
            }
        }

        /// <summary>
        /// New message aggignnee text change.
        /// </summary>
        private void Uzenetek_Uj_Cimzett()
        {
            switch (belepo.Beosztasa.ToString())
            {
                case "hallgato":
                    if (textBox2.Text.ToString() != "")
                    {
                        cim_valtozas = true;
                    }
                    else
                    {
                        cim_valtozas = false;
                    }
                    break;
                case "oktato":
                    if (textBox5.Text.ToString() != "")
                    {
                        cim_valtozas = true;
                    }
                    else
                    {
                        cim_valtozas = false;
                    }
                    break;
                case "admin":
                    if (textBox7.Text.ToString() != "")
                    {
                        cim_valtozas = true;
                    }
                    else
                    {
                        cim_valtozas = false;
                    }
                    break;
            }

            // Gombok láthatóságának beállítása.
            Uzenetek_Uj_Gomb_Ellenorzes();
        }

        /// <summary>
        /// New message subject text change.
        /// </summary>
        private void Uzenetek_Uj_Targy()
        {
            switch (belepo.Beosztasa.ToString())
            {
                case "hallgato":
                    if (textBox3.Text.ToString() != "")
                    {
                        targy_valtozas = true;
                    }
                    else
                    {
                        targy_valtozas = false;
                    }
                    break;
                case "oktato":
                    if (textBox4.Text.ToString() != "")
                    {
                        cim_valtozas = true;
                    }
                    else
                    {
                        cim_valtozas = false;
                    }
                    break;
                case "admin":
                    if (textBox6.Text.ToString() != "")
                    {
                        cim_valtozas = true;
                    }
                    else
                    {
                        cim_valtozas = false;
                    }
                    break;
            }

            // Gombok láthatóságának beállítása.
            Uzenetek_Uj_Gomb_Ellenorzes();
        }

        /// <summary>
        /// New message text change.
        /// </summary>
        private void Uzenetek_Uj_Szoveg()
        {
            switch (belepo.Beosztasa.ToString())
            {
                case "hallgato":
                    if (richTextBox3.Text.ToString() != "")
                    {
                        szoveg_valtozas = true;
                    }
                    else
                    {
                        szoveg_valtozas = false;
                    }
                    break;
                case "oktato":
                    if (richTextBox8.Text.ToString() != "")
                    {
                        szoveg_valtozas = true;
                    }
                    else
                    {
                        szoveg_valtozas = false;
                    }
                    break;
                case "admin":
                    if (richTextBox12.Text.ToString() != "")
                    {
                        szoveg_valtozas = true;
                    }
                    else
                    {
                        szoveg_valtozas = false;
                    }
                    break;
            }

            // Gombok láthatóságának beállítása.
            Uzenetek_Uj_Gomb_Ellenorzes();
        }

        /// <summary>
        /// New message buttons.
        /// </summary>
        private void Uzenetek_Uj_Gomb_Ellenorzes()
        {
            switch (belepo.Beosztasa.ToString())
            {
                case "hallgato":
                    // Küldés gomb.
                    if (textBox2.Text.ToString() != "")
                    {
                        if (button3.Enabled == false)
                        {
                            button3.Enabled = true;
                        }
                    }
                    else
                    {
                        if (button3.Enabled == true)
                        {
                            button3.Enabled = false;
                        }
                    }

                    // Mentés és Elvetés gomb.
                    if ((textBox2.Text.ToString() != "") || (textBox3.Text.ToString() != "") || (richTextBox3.Text.ToString() != ""))
                    {
                        if (button4.Enabled == false)
                        {
                            button4.Enabled = true;
                        }
                        if (button5.Enabled == false)
                        {
                            button5.Enabled = true;
                        }
                    }

                    if ((textBox2.Text.ToString() == "") && (textBox3.Text.ToString() == "") && (richTextBox3.Text.ToString() == ""))
                    {
                        if (button4.Enabled == true)
                        {
                            button4.Enabled = false;
                        }
                        if (button5.Enabled == true)
                        {
                            button5.Enabled = false;
                        }
                    }
                    break;
                case "oktato":
                    // Küldés gomb.
                    if (textBox5.Text.ToString() != "")
                    {
                        if (button21.Enabled == false)
                        {
                            button21.Enabled = true;
                        }
                    }
                    else
                    {
                        if (button21.Enabled == true)
                        {
                            button21.Enabled = false;
                        }
                    }

                    // Mentés és Elvetés gomb.
                    if ((textBox5.Text.ToString() != "") || (textBox4.Text.ToString() != "") || (richTextBox4.Text.ToString() != ""))
                    {
                        if (button20.Enabled == false)
                        {
                            button20.Enabled = true;
                        }
                        if (button19.Enabled == false)
                        {
                            button19.Enabled = true;
                        }
                    }

                    if ((textBox5.Text.ToString() == "") && (textBox4.Text.ToString() == "") && (richTextBox4.Text.ToString() == ""))
                    {
                        if (button20.Enabled == true)
                        {
                            button20.Enabled = false;
                        }
                        if (button19.Enabled == true)
                        {
                            button19.Enabled = false;
                        }
                    }
                    break;
                case "admin":
                    // Küldés gomb.
                    if (textBox7.Text.ToString() != "")
                    {
                        if (button29.Enabled == false)
                        {
                            button29.Enabled = true;
                        }
                    }
                    else
                    {
                        if (button29.Enabled == true)
                        {
                            button29.Enabled = false;
                        }
                    }

                    // Mentés és Elvetés gomb.
                    if ((textBox7.Text.ToString() != "") || (textBox6.Text.ToString() != "") || (richTextBox6.Text.ToString() != ""))
                    {
                        if (button28.Enabled == false)
                        {
                            button28.Enabled = true;
                        }
                        if (button27.Enabled == false)
                        {
                            button27.Enabled = true;
                        }
                    }

                    if ((textBox7.Text.ToString() == "") && (textBox6.Text.ToString() == "") && (richTextBox6.Text.ToString() == ""))
                    {
                        if (button28.Enabled == true)
                        {
                            button28.Enabled = false;
                        }
                        if (button27.Enabled == true)
                        {
                            button27.Enabled = false;
                        }
                    }
                    break;
            }
        }

        /// <summary>
        /// Sends new message.
        /// </summary>
        private void Uzenetek_Uj_Kuldes()
        {
            try
            {
                DateTime idoo = DateTime.Now;
                string formatum = "yyyy.MM.dd,HH:mm:ss";
                string cimzett = "";
                string targy = "";
                string szoveg = "";

                switch (belepo.Beosztasa.ToString())
                {
                    case "hallgato":
                        cimzett = textBox2.Text.ToString();
                        targy = textBox3.Text.ToString();
                        szoveg = richTextBox3.Text.ToString();
                        break;
                    case "oktato":
                        cimzett = textBox5.Text.ToString();
                        targy = textBox4.Text.ToString();
                        szoveg = richTextBox8.Text.ToString();
                        break;
                    case "admin":
                        cimzett = textBox7.Text.ToString();
                        targy = textBox6.Text.ToString();
                        szoveg = richTextBox12.Text.ToString();
                        break;
                }

                Uzenet mit = new Uzenet(idoo.ToString(formatum), "false", belepo.Kodja.ToString(), cimzett.ToString(), targy.ToString(), szoveg.ToString(), "true", "true", "false", "false", uzenet_kulcs.ToString());
                uzenet_kulcs++;

                server.Uzenet_Hozzaad(mit);
            }
            catch
            { }

            // Frissítés.
            switch (belepo.Beosztasa.ToString())
            {
                case "hallgato":
                    Hallgato_Uzenetek_Uj_Betoltes();
                    break;
                case "oktato":
                    Oktato_Uzenetek_Uj_Betoltes();
                    break;
                case "admin":
                    Admin_Uzenetek_Uj_Betoltes();
                    break;
            }
        }

        /// <summary>
        /// Saves new message.
        /// </summary>
        private void Uzenetek_Uj_Piszkozat()
        {
            try
            {
                DateTime idoo = DateTime.Now;
                string formatum = "yyyy.MM.dd,HH:mm:ss";
                string cimzett = "";
                string targy = "";
                string szoveg = "";

                switch (belepo.Beosztasa.ToString())
                {
                    case "hallgato":
                        cimzett = textBox2.Text.ToString();
                        targy = textBox3.Text.ToString();
                        szoveg = richTextBox3.Text.ToString();
                        break;
                    case "oktato":
                        cimzett = textBox5.Text.ToString();
                        targy = textBox4.Text.ToString();
                        szoveg = richTextBox8.Text.ToString();
                        break;
                    case "admin":
                        cimzett = textBox7.Text.ToString();
                        targy = textBox6.Text.ToString();
                        szoveg = richTextBox12.Text.ToString();
                        break;
                }

                Uzenet mit = new Uzenet(idoo.ToString(formatum), "false", belepo.Kodja.ToString(), cimzett.ToString(), targy.ToString(), szoveg.ToString(), "true", "false", "true", "true", uzenet_kulcs.ToString());
                uzenet_kulcs++;

                server.Uzenet_Hozzaad(mit);
            }
            catch
            { }

            // Frissítés.
            switch (belepo.Beosztasa.ToString())
            {
                case "hallgato":
                    Hallgato_Uzenetek_Uj_Betoltes();
                    break;
                case "oktato":
                    Oktato_Uzenetek_Uj_Betoltes();
                    break;
                case "admin":
                    Admin_Uzenetek_Uj_Betoltes();
                    break;
            }
        }

        #endregion Uzenetek

        #region Ertekeles

        /// <summary>
        /// Loads subjects.
        /// </summary>
        private void Ertekeles_Targyak()
        {
            switch (belepo.Beosztasa.ToString())
            {
                case "oktato":
                    listBox1.Items.Clear();
                    listBox2.Items.Clear();
                    comboBox16.Enabled = true;
                    comboBox17.Enabled = true;
                    comboBox16.Text = "";
                    comboBox17.Text = "";

                    if (comboBox6.Text.ToString() != "")
                    {
                        try
                        {
                            string megy = "oktato_ertekeles_targy_lista";
                            string felev = comboBox6.Text.ToString();
                            string kod = belepo.Kodja.ToString();
                            Oktato_index[] eredmeny = server.Oktato_Index_Lista(megy, felev, kod);

                            for (int i = 0; i < eredmeny.Length; i++)
                            {
                                listBox1.Items.Add(eredmeny[i].Neve.ToString());
                            }
                        }
                        catch
                        { }
                    }
                    break;
                case "admin":
                    listBox3.Items.Clear();
                    listBox4.Items.Clear();
                    comboBox19.Enabled = true;
                    comboBox22.Enabled = true;
                    comboBox19.Text = "";
                    comboBox22.Text = "";

                    if (comboBox7.Text.ToString() != "")
                    {
                        try
                        {
                            string megy = "admin_ertekeles_targy_lista";
                            // félév átalakítása.
                            string felev = comboBox7.Text.ToString();
                            int j = felev.Length;

                            if (felev[j - 1].ToString() == "1")
                            {
                                felev = "2013/14/1";
                            }
                            else
                            {
                                felev = "2013/14/2";
                            }

                            Targy[] eredmeny = server.Targy_Lista(megy, felev);

                            for (int i = 0; i < eredmeny.Length; i++)
                            {
                                if (eredmeny[i].Feleve.ToString() == felev.ToString())
                                {
                                    listBox4.Items.Add(eredmeny[i].Neve.ToString());
                                }
                            }
                        }
                        catch
                        { }
                    }
                    break;
            }

        }

        /// <summary>
        /// Edit button.
        /// </summary>
        private void Ertekeles_Modositas_Gomb()
        {
            try
            {
                if (alairas_change == true)
                {
                    Ertekeles_Alairas();
                }
                if (jegy_change == true)
                {
                    Ertekeles_Jegy();
                }
            }
            catch
            { }
        }

        /// <summary>
        /// Sets button.
        /// </summary>
        private void Ertekeles_Gomb_Ellenorzes()
        {
            switch (belepo.Beosztasa.ToString())
            {
                case "oktato":
                    if ((jegy_change == true) || (alairas_change == true))
                    {
                        if ((comboBox6.Text.ToString() != aktualis_felev.ToString()) && (belepo.Jogviszonya.ToString() != "aktív"))
                        {
                            button32.Enabled = false;
                        }
                        else
                        {
                            button32.Enabled = true;
                        }
                    }

                    if ((jegy_change == false) && (alairas_change == false))
                    {
                        button32.Enabled = false;
                    }
                    break;
                case "admin":
                    if ((jegy_change == true) || (alairas_change == true))
                    {
                        if ((comboBox7.Text.ToString() != aktualis_felev.ToString()) && (belepo.Jogviszonya.ToString() != "aktív"))
                        {
                            button33.Enabled = false;
                        }
                        else
                        {
                            button33.Enabled = true;
                        }
                    }

                    if ((jegy_change == false) && (alairas_change == false))
                    {
                        button33.Enabled = false;
                    }
                    break;
            }
        }

        /// <summary>
        /// Loads the students.
        /// </summary>
        private void Ertekeles_Hallgatok()
        {
            switch (belepo.Beosztasa.ToString())
            {
                case "oktato":
                    listBox2.Items.Clear();
                    comboBox16.Enabled = true;
                    comboBox17.Enabled = true;
                    comboBox16.Text = "";
                    comboBox17.Text = "";

                    if (listBox1.SelectedItem.ToString() != "")
                    {
                        try
                        {
                            string megy = "oktato_ertekeles_hallgato_lista";
                            string felev = comboBox6.Text.ToString();
                            string kod = listBox1.SelectedItem.ToString();
                            Hallgato_index[] eredmeny = server.Hallgato_Index_Lista(megy, felev, kod);

                            for (int i = 0; i < eredmeny.Length; i++)
                            {
                                listBox2.Items.Add(eredmeny[i].Krakenje.ToString());
                            }
                        }
                        catch
                        { }
                    }
                    break;
                case "admin":
                    listBox3.Items.Clear();
                    comboBox19.Enabled = true;
                    comboBox22.Enabled = true;
                    comboBox19.Text = "";
                    comboBox22.Text = "";

                    if (listBox4.SelectedItem.ToString() != "")
                    {
                        try
                        {
                            string megy = "admin_ertekeles_hallgato_lista";
                            string felev = comboBox7.Text.ToString();
                            string kod = listBox4.SelectedItem.ToString();
                            Hallgato_index[] eredmeny = server.Hallgato_Index_Lista(megy, felev, kod);

                            for (int i = 0; i < eredmeny.Length; i++)
                            {
                                listBox3.Items.Add(eredmeny[i].Krakenje.ToString());
                            }
                        }
                        catch
                        { }
                    }
                    break;
            }
        }

        /// <summary>
        /// Loads the students grades.
        /// </summary>
        private void Ertekeles_Hallgato_Targyai()
        {
            switch (belepo.Beosztasa.ToString())
            {
                case "oktato":
                    comboBox16.Text = "";
                    comboBox17.Text = "";
                    comboBox16.Enabled = true;
                    comboBox17.Enabled = true;

                    if (listBox2.SelectedItem.ToString() != "")
                    {
                        try
                        {
                            string megy = "oktatko_ertekeles_hallgato_targy_lista";
                            string felev = comboBox6.Text.ToString();
                            string kod = listBox2.SelectedItem.ToString() + "*" + listBox1.SelectedItem.ToString();
                            Hallgato_index[] eredmeny = server.Hallgato_Index_Lista(megy, felev, kod);

                            for (int i = 0; i < eredmeny.Length; i++)
                            {
                                if (eredmeny[i].Kovetelmenye.ToString() == "évközi jegy")
                                {
                                    comboBox16.Enabled = false;

                                    if (eredmeny[i].Jegye.ToString() != "")
                                    {
                                        comboBox17.Text = eredmeny[i].Jegye.ToString();
                                    }
                                    else
                                    {
                                        comboBox17.Text = "";
                                    }
                                }
                                else // vizsgás.
                                {
                                    if (eredmeny[i].Alairasa.ToString() != "")
                                    {
                                        comboBox16.Text = eredmeny[i].Alairasa.ToString();

                                        if (comboBox16.Text.ToString() != "Aláírva")
                                        {
                                            comboBox17.Enabled = false;
                                        }
                                        else
                                        {
                                            comboBox17.Enabled = true;
                                        }
                                    }
                                    else
                                    {
                                        comboBox16.Text = "";
                                        comboBox17.Enabled = false;
                                    }

                                    if (eredmeny[i].Jegye.ToString() != "")
                                    {
                                        comboBox17.Text = eredmeny[i].Jegye.ToString();
                                    }
                                    else
                                    {
                                        comboBox17.Text = "";
                                    }
                                }
                            }
                        }
                        catch
                        { }
                    }
                    break;
                case "admin":
                    comboBox19.Text = "";
                    comboBox22.Text = "";
                    comboBox19.Enabled = true;
                    comboBox22.Enabled = true;

                    if (listBox3.SelectedItem.ToString() != "")
                    {
                        try
                        {
                            string megy = "admin_ertekeles_hallgato_targy_lista";
                            string felev = comboBox7.Text.ToString();
                            string kod = listBox3.SelectedItem.ToString() + "*" + listBox4.SelectedItem.ToString();
                            Hallgato_index[] eredmeny = server.Hallgato_Index_Lista(megy, felev, kod);

                            for (int i = 0; i < eredmeny.Length; i++)
                            {
                                if (eredmeny[i].Kovetelmenye.ToString() == "évközi jegy")
                                {
                                    comboBox19.Enabled = false;

                                    if (eredmeny[i].Jegye.ToString() != "")
                                    {
                                        comboBox22.Text = eredmeny[i].Jegye.ToString();
                                    }
                                    else
                                    {
                                        comboBox22.Text = "";
                                    }
                                }
                                else // vizsgás.
                                {
                                    if (eredmeny[i].Alairasa.ToString() != "")
                                    {
                                        comboBox19.Text = eredmeny[i].Alairasa.ToString();

                                        if (comboBox19.Text.ToString() != "Aláírva")
                                        {
                                            comboBox22.Enabled = false;
                                        }
                                        else
                                        {
                                            comboBox22.Enabled = true;
                                        }
                                    }
                                    else
                                    {
                                        comboBox19.Text = "";
                                        comboBox22.Enabled = false;
                                    }

                                    if (eredmeny[i].Jegye.ToString() != "")
                                    {
                                        comboBox22.Text = eredmeny[i].Jegye.ToString();
                                    }
                                    else
                                    {
                                        comboBox22.Text = "";
                                    }
                                }
                            }
                        }
                        catch
                        { }
                    }
                    break;
            }
        }

        /// <summary>
        /// Edit signature.
        /// </summary>
        private void Ertekeles_Alairas()
        {
            // Módosítás.
            try
            {
                string megy = "ertekeles_alairas";
                string felev = "";
                string kod = "";

                switch (belepo.Beosztasa.ToString())
                {
                    case "oktato":
                        felev = comboBox6.Text.ToString();
                        kod = listBox2.SelectedItem.ToString() + "*" + listBox1.SelectedItem.ToString();
                        break;
                    case "admin":
                        felev = comboBox7.Text.ToString();
                        kod = listBox3.SelectedItem.ToString() + "*" + listBox4.SelectedItem.ToString();
                        break;
                }

                Hallgato_index[] eredmeny = server.Hallgato_Index_Lista(megy, felev, kod);

                if (eredmeny.Length == 1)
                {
                    string alairas = "";
                    Hallgato_index mit = new Hallgato_index(eredmeny[0].Neve.ToString(), eredmeny[0].Torolve.ToString(), eredmeny[0].Kodja.ToString(), eredmeny[0].Kreditje.ToString(), eredmeny[0].Ideje.ToString(), eredmeny[0].Oktatoja.ToString(), eredmeny[0].Feleve.ToString(), eredmeny[0].Kovetelmenye.ToString(), eredmeny[0].Elokovetelmenye.ToString(), eredmeny[0].Idje.ToString(), eredmeny[0].Krakenje.ToString(), eredmeny[0].Jegye.ToString(), eredmeny[0].Alairasa.ToString());

                    switch (belepo.Beosztasa.ToString())
                    {
                        case "oktato":
                            alairas = comboBox16.Text.ToString();
                            break;
                        case "admin":
                            alairas = comboBox19.Text.ToString();
                            break;
                    }

                    Hallgato_index mire = new Hallgato_index(eredmeny[0].Neve.ToString(), eredmeny[0].Torolve.ToString(), eredmeny[0].Kodja.ToString(), eredmeny[0].Kreditje.ToString(), eredmeny[0].Ideje.ToString(), eredmeny[0].Oktatoja.ToString(), eredmeny[0].Feleve.ToString(), eredmeny[0].Kovetelmenye.ToString(), eredmeny[0].Elokovetelmenye.ToString(), eredmeny[0].Idje.ToString(), eredmeny[0].Krakenje.ToString(), eredmeny[0].Jegye.ToString(), alairas.ToString());
                    server.Hallgato_Index_Modosit(mit, mire);
                }
            }
            catch
            { }

            // Üzenet.
            try
            {
                DateTime idoo = DateTime.Now;
                string formatum = "yyyy.MM.dd,HH:mm:ss";
                string text = "A(z) " + listBox1.SelectedItem.ToString() + " tárgyra aláírás érkezett, eredménye: " + comboBox16.Text.ToString() + " módosító: " + belepo.Neve.ToString();
                string subject = "Aláírás";
                Uzenet mit2 = new Uzenet(idoo.ToString(formatum), belepo.Torolve.ToString(), belepo.Kodja.ToString(), listBox2.SelectedItem.ToString(), subject.ToString(), text.ToString(), "true", "true", "false", "false", uzenet_kulcs.ToString());
                uzenet_kulcs++;

                server.Uzenet_Hozzaad(mit2);
                alairas_change = false;
            }
            catch
            { }

            if (belepo.Beosztasa.ToString() == "oktato")
            {
                Oktato_Ertekeles_Betoltes();
            }
            else
            {
                Admin_Ertekeles_Betoltes();
            }
        }

        /// <summary>
        /// Edit grade.
        /// </summary>
        private void Ertekeles_Jegy()
        {
            // Módosítás.
            try
            {
                string megy = "ertekeles_jegy";
                string felev = "";
                string kod = "";

                switch (belepo.Beosztasa.ToString())
                {
                    case "oktato":
                        felev = comboBox6.Text.ToString();
                        kod = listBox2.SelectedItem.ToString() + "*" + listBox1.SelectedItem.ToString();
                        break;
                    case "admin":
                        felev = comboBox7.Text.ToString();
                        kod = listBox3.SelectedItem.ToString() + "*" + listBox4.SelectedItem.ToString();
                        break;
                }

                Hallgato_index[] eredmeny = server.Hallgato_Index_Lista(megy, felev, kod);

                if (eredmeny.Length == 1)
                {
                    string jegy = "";
                    Hallgato_index mit = new Hallgato_index(eredmeny[0].Neve.ToString(), eredmeny[0].Torolve.ToString(), eredmeny[0].Kodja.ToString(), eredmeny[0].Kreditje.ToString(), eredmeny[0].Ideje.ToString(), eredmeny[0].Oktatoja.ToString(), eredmeny[0].Feleve.ToString(), eredmeny[0].Kovetelmenye.ToString(), eredmeny[0].Elokovetelmenye.ToString(), eredmeny[0].Idje.ToString(), eredmeny[0].Krakenje.ToString(), eredmeny[0].Jegye.ToString(), eredmeny[0].Alairasa.ToString());

                    switch (belepo.Beosztasa.ToString())
                    {
                        case "oktato":
                            jegy = comboBox17.Text.ToString();
                            break;
                        case "admin":
                            jegy = comboBox22.Text.ToString();
                            break;
                    }

                    Hallgato_index mire = new Hallgato_index(eredmeny[0].Neve.ToString(), eredmeny[0].Torolve.ToString(), eredmeny[0].Kodja.ToString(), eredmeny[0].Kreditje.ToString(), eredmeny[0].Ideje.ToString(), eredmeny[0].Oktatoja.ToString(), eredmeny[0].Feleve.ToString(), eredmeny[0].Kovetelmenye.ToString(), eredmeny[0].Elokovetelmenye.ToString(), eredmeny[0].Idje.ToString(), eredmeny[0].Krakenje.ToString(), jegy.ToString(), eredmeny[0].Alairasa.ToString());
                    server.Hallgato_Index_Modosit(mit, mire);
                }
            }
            catch
            { }

            // Üzenet.
            try
            {
                DateTime idoo = DateTime.Now;
                string formatum = "yyyy.MM.dd,HH:mm:ss";
                string text = "A(z) " + listBox1.SelectedItem.ToString() + " tárgyra jegybeírás történt, eredménye: " + comboBox17.Text.ToString() + " módosító: " + belepo.Neve.ToString();
                string subject = "Érdemjegy";
                Uzenet megy = new Uzenet(idoo.ToString(formatum), belepo.Torolve.ToString(), belepo.Kodja.ToString(), listBox2.SelectedItem.ToString(), subject.ToString(), text.ToString(), "true", "true", "false", "false", uzenet_kulcs.ToString());
                uzenet_kulcs++;

                server.Uzenet_Hozzaad(megy);
                jegy_change = false;
            }
            catch
            { }

            if (belepo.Beosztasa.ToString() == "oktato")
            {
                Oktato_Ertekeles_Betoltes();
            }
            else
            {
                Admin_Ertekeles_Betoltes();
            }
        }

        /// <summary>
        /// Check signature button.
        /// </summary>
        private void Ertekeles_Alairas_Gomb()
        {
            switch (belepo.Beosztasa.ToString())
            {
                case "oktato":
                    if (comboBox16.Text != "")
                    {
                        if (alairas_change == false)
                        {
                            alairas_change = true;
                            Ertekeles_Gomb_Ellenorzes();
                        }
                    }
                    break;
                case "admin":
                    if (comboBox19.Text != "")
                    {
                        if (alairas_change == false)
                        {
                            alairas_change = true;
                            Ertekeles_Gomb_Ellenorzes();
                        }
                    }
                    break;
            }
        }

        /// <summary>
        /// Check grade button.
        /// </summary>
        private void Ertekeles_Jegy_Gomb()
        {
            switch (belepo.Beosztasa.ToString())
            {
                case "oktato":
                    if (comboBox17.Text != "")
                    {
                        if (jegy_change == false)
                        {
                            jegy_change = true;
                            Ertekeles_Gomb_Ellenorzes();
                        }
                    }
                    break;
                case "admin":
                    if (comboBox22.Text != "")
                    {
                        if (jegy_change == false)
                        {
                            jegy_change = true;
                            Ertekeles_Gomb_Ellenorzes();
                        }
                    }
                    break;
            }
        }

        #endregion Ertekeles

        #endregion Common

        #region StudentGUI

        #region Adatok

        /// <summary>
        /// Handles the Layout event of the tabPage5 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="LayoutEventArgs"/> instance containing the event data.</param>
        private void tabPage5_Layout(object sender, LayoutEventArgs e)
        {
            Hallgato_Adatok_Betoltes();
        }

        /// <summary>
        /// Handles the TextChanged event of the textBox43 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void textBox43_TextChanged(object sender, EventArgs e)
        {
            Adatok_Jelszo_Gomb();
        }

        /// <summary>
        /// Handles the TextChanged event of the textBox41 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void button41_Click(object sender, EventArgs e)
        {
            Adatok_Jelszo_Modosítas();
        }

        /// <summary>
        /// Loads the values.
        /// </summary>
        private void Hallgato_Adatok_Betoltes()
        {
            button41.Enabled = false;
            textBox43.Text = "";

            try
            {
                label3.Text = "Név: " + belepo.Neve.ToString();
                label8.Text = "Kraken kód: " + belepo.Kodja.ToString();
                label9.Text = "Jelszó: " + belepo.Jelszava.ToString();
                label12.Text = "E-mail cím: " + belepo.Emailje.ToString();
                label18.Text = "Fizetés: " + belepo.Fizetese.ToString();
                label19.Text = "Születési hely: " + belepo.Szuletesi_helye.ToString();
                label20.Text = "Születési idő: " + belepo.Szuletesi_ideje.ToString();
                label21.Text = "Anyja neve: " + belepo.Anyja_neve.ToString();
                label22.Text = "Bankszámla szám: " + belepo.Bankszamla.ToString();
                label23.Text = "Státusz: " + belepo.Statusza.ToString();
                label24.Text = "Beiratkozás ideje: " + belepo.Beiratkozva.ToString();
                label25.Text = "Nem: " + belepo.Neme.ToString();
                label13.Text = "Jogviszony: " + belepo.Jogviszonya.ToString();
                label58.Text = "Lakcím: " + belepo.Lakcime.ToString();
                label108.Text = "Telefonszám: " + belepo.Telefonja.ToString();
            }
            catch
            { }
        }

        #endregion Adatok

        #region Targyfelvetel

        /// <summary>
        /// Handles the CellClick event of the dataGridView1 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="DataGridViewCellEventArgs"/> instance containing the event data.</param>
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            Hallgato_Targyfelvetel_Kijeloles();
        }

        /// <summary>
        /// Handles the Click event of the button1 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void button1_Click(object sender, EventArgs e)
        {
            Hallgato_Targyfelvetel_Listazas();
        }

        /// <summary>
        /// Handles the Click event of the button12 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void button12_Click(object sender, EventArgs e)
        {
            Hallgato_Targyfelvetel_Felvetel();
        }

        /// <summary>
        /// Handles the Click event of the button13 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void button13_Click(object sender, EventArgs e)
        {
            Hallgato_Targyfelvetel_Leadas();
        }

        /// <summary>
        /// Handles the Layout event of the tabPage6 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="LayoutEventArgs"/> instance containing the event data.</param>
        private void tabPage6_Layout(object sender, LayoutEventArgs e)
        {
            Hallgato_Targyfelvetel_Betoltes();
        }

        /// <summary>
        /// Preset the values.
        /// </summary>
        private void Hallgato_Targyfelvetel_Betoltes()
        {
            dataGridView1.Rows.Clear();
            button12.Enabled = false;
            button13.Enabled = false;
            textBox1.Text = "";
            comboBox1.Text = "";
            comboBox2.Text = "";
            checkBox1.Checked = false;
            checkBox2.Checked = false;
        }

        /// <summary>
        /// Select a subject.
        /// </summary>
        private void Hallgato_Targyfelvetel_Kijeloles()
        {
            // Kijelölés esetén engedélyezzük a gombokat.
            if (dataGridView1.CurrentCellAddress.X >= 0)
            {
                if (belepo.Statusza.ToString() == "aktív")
                {
                    button12.Enabled = true;
                    button13.Enabled = true;

                    // Tárgyazonosító megszerzése.
                    try
                    {
                        int kijeloltsorindex = dataGridView1.SelectedCells[0].RowIndex;
                        DataGridViewRow kijeloltsor = dataGridView1.Rows[kijeloltsorindex];
                        h_targyfelv_id = kijeloltsor.Cells["targykod"].Value.ToString();
                        kijeloltsorindex = dataGridView1.SelectedCells[0].RowIndex;
                        kijeloltsor = dataGridView1.Rows[kijeloltsorindex];
                        h_targyfelv_elokov = kijeloltsor.Cells["elokovetelmeny"].Value.ToString();
                    }
                    catch
                    { }
                }
            }
            else
            {
                button12.Enabled = false;
                button13.Enabled = false;
            }
        }

        /// <summary>
        /// Lists the subjects.
        /// </summary>
        private void Hallgato_Targyfelvetel_Listazas()
        {
            dataGridView1.Rows.Clear();

			// A félév kiválasztva.
            if (comboBox1.Text.ToString() != "")
            {
                try
                {
                    // Listázási segédlet
                    string megy1 = "hallgato_targyfelvetel_lista";
                    string megy2 = "hallgato_targyfelvetel_lista";
                    string felev = comboBox1.Text.ToString();
                    string kod = belepo.Kodja.ToString();
                    Targy[] eredmeny1 = server.Targy_Lista(megy1, felev);
                    Hallgato_index[] eredmeny2 = server.Hallgato_Index_Lista(megy2, felev, kod);

					// A felvett- vagy teljesített tárgyak kihagyása.
                    if ((checkBox1.Checked == true) || (checkBox2.Checked == true))
                    {
						// Részletesebb keresés.
                        if ((textBox1.Text.ToString() != "") && (comboBox2.Text.ToString() != ""))
                        {
                            // A keresés és a kihagyások.
                            Osszetett_Kereses(eredmeny1, eredmeny2, felev);
                        }
                        else
                        {
                            // Csak a kihagyások.
                            Csak_Kihagyasos_Kereses(eredmeny1, eredmeny2, felev);
                        }
                    }
                    else
                    {
						// Részletesebb keresés.
                        if ((textBox1.Text.ToString() != "") && (comboBox2.Text.ToString() != ""))
                        {
                            // Csak a keresés.
                            Csak_Kereses(eredmeny1, eredmeny2, felev);
                        }
                        else
                        {
                            // Csak a tárgylista.
                            Csak_Listas_Kereses(eredmeny1, eredmeny2, felev);
                        }
                    }
                }
                catch
                { }
            }
        }

        /// <summary>
        /// Search engine.
        /// </summary>
        /// <param name="targylista">The targylista.</param>
        /// <param name="hallgatolista">The hallgatolista.</param>
        /// <param name="felev">The felev.</param>
        private void Osszetett_Kereses(Targy[] targylista, Hallgato_index[] hallgatolista, string felev)
        {
            try
            {
                for (int i = 0; i < targylista.Length; i++)
                {
                    string[] oszlop = new string[9] { "", "", "", "", "", "", "", "", "" };
                    string eredmeny = "";

                    for (int j = 0; j < hallgatolista.Length; j++)
                    {
                        if ((hallgatolista[j].Kodja.ToString() == targylista[i].Kodja.ToString()) && (int.Parse(hallgatolista[j].Jegye) > 1))
                        {
                            eredmeny = "Teljesitve";
                        }
                        else
                        {
                            if ((hallgatolista[j].Kodja.ToString() == targylista[i].Kodja.ToString()) && (hallgatolista[j].Feleve.ToString() == aktualis_felev.ToString()))
                            {
                                eredmeny = "Felvéve";
                            }
                        }
                    }

                    if (checkBox1.Checked == true)
                    {
                        if (checkBox2.Checked == true)
                        {
                            if (eredmeny == "")
                            {
                                string keresendo = "";

                                switch (comboBox2.Text.ToString())
                                {
                                    case "Tárgy neve":
                                        keresendo = targylista[i].Neve.ToString();
                                        break;
                                    case "Tárgy kódja":
                                        keresendo = targylista[i].Kodja.ToString();
                                        break;
                                    case "Kurzusidő":
                                        keresendo = targylista[i].Ideje.ToString();
                                        break;
                                    case "Kredit":
                                        keresendo = targylista[i].Kreditje.ToString();
                                        break;
                                    case "Követelmény":
                                        keresendo = targylista[i].Kovetelmenye.ToString();
                                        break;
                                    case "Előkövetelmény":
                                        keresendo = targylista[i].Elokovetelmenye.ToString();
                                        break;
                                    case "Oktató neve":
                                        keresendo = targylista[i].Oktatoja.ToString();
                                        break;
                                }

                                for (int k = 0; k < textBox1.Text.Length; k++)
                                {
                                    if (keresendo.Substring(k, textBox1.Text.Length).ToUpper() == textBox1.Text.ToUpper())
                                    {
                                        oszlop[0] = targylista[i].Neve.ToString();
                                        oszlop[1] = eredmeny;
                                        oszlop[2] = targylista[i].Kodja.ToString();
                                        oszlop[3] = targylista[i].Oktatoja.ToString();
                                        oszlop[4] = targylista[i].Kreditje.ToString();
                                        oszlop[5] = targylista[i].Ideje.ToString();
                                        oszlop[6] = targylista[i].Feleve.ToString();
                                        oszlop[7] = targylista[i].Elokovetelmenye.ToString();
                                        oszlop[8] = targylista[i].Kovetelmenye.ToString();
                                        dataGridView1.Rows.Add(oszlop);
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (eredmeny == "" || eredmeny == "Felvéve")
                            {
                                string keresendo = "";

                                switch (comboBox2.Text.ToString())
                                {
                                    case "Tárgy neve":
                                        keresendo = targylista[i].Neve.ToString();
                                        break;
                                    case "Tárgy kódja":
                                        keresendo = targylista[i].Kodja.ToString();
                                        break;
                                    case "Kurzusidő":
                                        keresendo = targylista[i].Ideje.ToString();
                                        break;
                                    case "Kredit":
                                        keresendo = targylista[i].Kreditje.ToString();
                                        break;
                                    case "Követelmény":
                                        keresendo = targylista[i].Kovetelmenye.ToString();
                                        break;
                                    case "Előkövetelmény":
                                        keresendo = targylista[i].Elokovetelmenye.ToString();
                                        break;
                                    case "Oktató neve":
                                        keresendo = targylista[i].Oktatoja.ToString();
                                        break;
                                }

                                for (int k = 0; k < textBox1.Text.Length; k++)
                                {
                                    if (keresendo.Substring(k, textBox1.Text.Length).ToUpper() == textBox1.Text.ToUpper())
                                    {
                                        oszlop[0] = targylista[i].Neve.ToString();
                                        oszlop[1] = eredmeny;
                                        oszlop[2] = targylista[i].Kodja.ToString();
                                        oszlop[3] = targylista[i].Oktatoja.ToString();
                                        oszlop[4] = targylista[i].Kreditje.ToString();
                                        oszlop[5] = targylista[i].Ideje.ToString();
                                        oszlop[6] = targylista[i].Feleve.ToString();
                                        oszlop[7] = targylista[i].Elokovetelmenye.ToString();
                                        oszlop[8] = targylista[i].Kovetelmenye.ToString();
                                        dataGridView1.Rows.Add(oszlop);
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        if (checkBox2.Checked == true)
                        {
                            if (eredmeny == "" || eredmeny == "Teljesítve")
                            {
                                string keresendo = "";

                                switch (comboBox2.Text.ToString())
                                {
                                    case "Tárgy neve":
                                        keresendo = targylista[i].Neve.ToString();
                                        break;
                                    case "Tárgy kódja":
                                        keresendo = targylista[i].Kodja.ToString();
                                        break;
                                    case "Kurzusidő":
                                        keresendo = targylista[i].Ideje.ToString();
                                        break;
                                    case "Kredit":
                                        keresendo = targylista[i].Kreditje.ToString();
                                        break;
                                    case "Követelmény":
                                        keresendo = targylista[i].Kovetelmenye.ToString();
                                        break;
                                    case "Előkövetelmény":
                                        keresendo = targylista[i].Elokovetelmenye.ToString();
                                        break;
                                    case "Oktató neve":
                                        keresendo = targylista[i].Oktatoja.ToString();
                                        break;
                                }

                                for (int k = 0; k < textBox1.Text.Length; k++)
                                {
                                    if (keresendo.Substring(k, textBox1.Text.Length).ToUpper() == textBox1.Text.ToUpper())
                                    {
                                        oszlop[0] = targylista[i].Neve.ToString();
                                        oszlop[1] = eredmeny;
                                        oszlop[2] = targylista[i].Kodja.ToString();
                                        oszlop[3] = targylista[i].Oktatoja.ToString();
                                        oszlop[4] = targylista[i].Kreditje.ToString();
                                        oszlop[5] = targylista[i].Ideje.ToString();
                                        oszlop[6] = targylista[i].Feleve.ToString();
                                        oszlop[7] = targylista[i].Elokovetelmenye.ToString();
                                        oszlop[8] = targylista[i].Kovetelmenye.ToString();
                                        dataGridView1.Rows.Add(oszlop);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch
            { }
        }

        /// <summary>
        /// Only list search.
        /// </summary>
        /// <param name="targylista">The targylista.</param>
        /// <param name="hallgatolista">The hallgatolista.</param>
        /// <param name="felev">The felev.</param>
        private void Csak_Listas_Kereses(Targy[] targylista, Hallgato_index[] hallgatolista, string felev)
        {
            try
            {
                for (int i = 0; i < targylista.Length; i++)
                {
                    string[] oszlop = new string[9] { "", "", "", "", "", "", "", "", "" };
                    string eredmeny = "";

                    if ((targylista[i].Torolve.ToString() == "false") && (targylista[i].Feleve.ToString() == felev.ToString()))
                    {
                        for (int j = 0; j < hallgatolista.Length; j++)
                        {
                            if ((hallgatolista[j].Kodja.ToString() == targylista[i].Kodja.ToString()) && (int.Parse(hallgatolista[j].Jegye) > 1))
                            {
                                eredmeny = "Teljesitve";
                            }
                            else
                            {
                                if ((hallgatolista[j].Kodja.ToString() == targylista[i].Kodja.ToString()) && (hallgatolista[j].Feleve.ToString() == aktualis_felev.ToString()))
                                {
                                    eredmeny = "Felvéve";
                                }
                            }
                        }

                        oszlop[0] = targylista[i].Neve.ToString();
                        oszlop[1] = eredmeny;
                        oszlop[2] = targylista[i].Kodja.ToString();
                        oszlop[3] = targylista[i].Oktatoja.ToString();
                        oszlop[4] = targylista[i].Kreditje.ToString();
                        oszlop[5] = targylista[i].Ideje.ToString();
                        oszlop[6] = targylista[i].Feleve.ToString();
                        oszlop[7] = targylista[i].Elokovetelmenye.ToString();
                        oszlop[8] = targylista[i].Kovetelmenye.ToString();
                        dataGridView1.Rows.Add(oszlop);
                    }
                }
            }
            catch
            { }
        }

        /// <summary>
        /// Query.
        /// </summary>
        /// <param name="targylista">The targylista.</param>
        /// <param name="hallgatolista">The hallgatolista.</param>
        /// <param name="felev">The felev.</param>
        private void Csak_Kereses(Targy[] targylista, Hallgato_index[] hallgatolista, string felev)
        {
            try
            {
                for (int i = 0; i < targylista.Length; i++)
                {
                    string[] oszlop = new string[9] { "", "", "", "", "", "", "", "", "" };
                    string eredmeny = "";

                    if ((targylista[i].Torolve.ToString() == "false") && (targylista[i].Feleve.ToString() == felev.ToString()))
                    {
                        for (int j = 0; j < hallgatolista.Length; j++)
                        {
                            if ((hallgatolista[j].Kodja.ToString() == targylista[i].Kodja.ToString()) && (int.Parse(hallgatolista[j].Jegye) > 1))
                            {
                                eredmeny = "Teljesitve";
                            }
                            else
                            {
                                if ((hallgatolista[j].Kodja.ToString() == targylista[i].Kodja.ToString()) && (hallgatolista[j].Feleve.ToString() == aktualis_felev.ToString()))
                                {
                                    eredmeny = "Felvéve";
                                }
                            }
                        }
                    }

                    string keresendo = "";

                    switch (comboBox2.Text.ToString())
                    {
                        case "Tárgy neve":
                            keresendo = targylista[i].Neve.ToString();
                            break;
                        case "Tárgy kódja":
                            keresendo = targylista[i].Kodja.ToString();
                            break;
                        case "Kurzusidő":
                            keresendo = targylista[i].Ideje.ToString();
                            break;
                        case "Kredit":
                            keresendo = targylista[i].Kreditje.ToString();
                            break;
                        case "Követelmény":
                            keresendo = targylista[i].Kovetelmenye.ToString();
                            break;
                        case "Előkövetelmény":
                            keresendo = targylista[i].Elokovetelmenye.ToString();
                            break;
                        case "Oktató neve":
                            keresendo = targylista[i].Oktatoja.ToString();
                            break;
                    }

                    for (int k = 0; k < textBox1.Text.Length; k++)
                    {
                        if (keresendo.Substring(k, textBox1.Text.Length).ToUpper() == textBox1.Text.ToUpper())
                        {
                            oszlop[0] = targylista[i].Neve.ToString();
                            oszlop[1] = eredmeny;
                            oszlop[2] = targylista[i].Kodja.ToString();
                            oszlop[3] = targylista[i].Oktatoja.ToString();
                            oszlop[4] = targylista[i].Kreditje.ToString();
                            oszlop[5] = targylista[i].Ideje.ToString();
                            oszlop[6] = targylista[i].Feleve.ToString();
                            oszlop[7] = targylista[i].Elokovetelmenye.ToString();
                            oszlop[8] = targylista[i].Kovetelmenye.ToString();
                            dataGridView1.Rows.Add(oszlop);
                        }
                    }
                }
            }
            catch
            { }
        }

        /// <summary>
        /// Leave out subjects.
        /// </summary>
        /// <param name="targylista">The targylista.</param>
        /// <param name="hallgatolista">The hallgatolista.</param>
        /// <param name="felev">The felev.</param>
        private void Csak_Kihagyasos_Kereses(Targy[] targylista, Hallgato_index[] hallgatolista, string felev)
        {
            try
            {
                for (int i = 0; i < targylista.Length; i++)
                {
                    string[] oszlop = new string[9] { "", "", "", "", "", "", "", "", "" };
                    string eredmeny = "";

                    if ((targylista[i].Torolve.ToString() == "false") && (targylista[i].Feleve.ToString() == felev.ToString()))
                    {
                        for (int j = 0; j < hallgatolista.Length; j++)
                        {
                            if ((hallgatolista[j].Kodja.ToString() == targylista[i].Kodja.ToString()) && (int.Parse(hallgatolista[j].Jegye) > 1))
                            {
                                eredmeny = "Teljesitve";
                            }
                            else
                            {
                                if ((hallgatolista[j].Kodja.ToString() == targylista[i].Kodja.ToString()) && (hallgatolista[j].Feleve.ToString() == aktualis_felev.ToString()))
                                {
                                    eredmeny = "Felvéve";
                                }
                            }
                        }

                        if (checkBox1.Checked == true)
                        {
                            if (checkBox2.Checked == true)
                            {
                                if (eredmeny == "")
                                {
                                    oszlop[0] = targylista[i].Neve.ToString();
                                    oszlop[1] = eredmeny;
                                    oszlop[2] = targylista[i].Kodja.ToString();
                                    oszlop[3] = targylista[i].Oktatoja.ToString();
                                    oszlop[4] = targylista[i].Kreditje.ToString();
                                    oszlop[5] = targylista[i].Ideje.ToString();
                                    oszlop[6] = targylista[i].Feleve.ToString();
                                    oszlop[7] = targylista[i].Elokovetelmenye.ToString();
                                    oszlop[8] = targylista[i].Kovetelmenye.ToString();
                                    dataGridView1.Rows.Add(oszlop);
                                }
                            }
                            else
                            {
                                if (eredmeny == "" || eredmeny == "Felvéve")
                                {
                                    oszlop[0] = targylista[i].Neve.ToString();
                                    oszlop[1] = eredmeny;
                                    oszlop[2] = targylista[i].Kodja.ToString();
                                    oszlop[3] = targylista[i].Oktatoja.ToString();
                                    oszlop[4] = targylista[i].Kreditje.ToString();
                                    oszlop[5] = targylista[i].Ideje.ToString();
                                    oszlop[6] = targylista[i].Feleve.ToString();
                                    oszlop[7] = targylista[i].Elokovetelmenye.ToString();
                                    oszlop[8] = targylista[i].Kovetelmenye.ToString();
                                    dataGridView1.Rows.Add(oszlop);
                                }
                            }
                        }
                        else
                        {
                            if (checkBox2.Checked == true)
                            {
                                if (eredmeny == "" || eredmeny == "Teljesítve")
                                {
                                    oszlop[0] = targylista[i].Neve.ToString();
                                    oszlop[1] = eredmeny;
                                    oszlop[2] = targylista[i].Kodja.ToString();
                                    oszlop[3] = targylista[i].Oktatoja.ToString();
                                    oszlop[4] = targylista[i].Kreditje.ToString();
                                    oszlop[5] = targylista[i].Ideje.ToString();
                                    oszlop[6] = targylista[i].Feleve.ToString();
                                    oszlop[7] = targylista[i].Elokovetelmenye.ToString();
                                    oszlop[8] = targylista[i].Kovetelmenye.ToString();
                                    dataGridView1.Rows.Add(oszlop);
                                }
                            }
                        }
                    }
                }
            }
            catch
            { }
        }

        /// <summary>
        /// Add a subject to a student.
        /// </summary>
        private void Hallgato_Targyfelvetel_Felvetel()
        {
            try
            {
                string megy1 = "hallgato_targyfelvetel_felvetel";
                string megy2 = "hallgato_targyfelvetel_felvetel";
                string felev = comboBox1.Text.ToString() + "*" + h_targyfelv_id.ToString();
                string kod = belepo.Kodja.ToString();
                int van1 = 0;
                int van2 = 0;
                int van3 = 0;
                int van4 = 0;
                int id = -1;
                Targy[] eredmeny1 = server.Targy_Lista(megy1, felev);
                Hallgato_index[] eredmeny2 = server.Hallgato_Index_Lista(megy2, felev, kod);

                // Teljesítette-e már korábban.
                for (int i = 0; i < eredmeny2.Length; i++)
                {
                    if ((eredmeny2[i].Torolve.ToString() == "false") && (eredmeny2[i].Kodja.ToString() == h_targyfelv_id.ToString()) && (int.Parse(eredmeny2[i].Jegye) > 1))
                    {
                        van1++;
                    }
                }

                // Fel van-e véve az aktuális félévben.
                for (int i = 0; i < eredmeny2.Length; i++)
                {
                    if ((eredmeny2[i].Torolve.ToString() == "false") && (eredmeny2[i].Kodja.ToString() == h_targyfelv_id.ToString()) && (eredmeny2[i].Feleve.ToString() == aktualis_felev.ToString()))
                    {
                        van2++;
                    }
                }

                // Teljesítve van-e az előkövetelménye.
                for (int i = 0; i < eredmeny2.Length; i++)
                {
                    if ((eredmeny2[i].Torolve.ToString() == "false") && (eredmeny2[i].Kodja.ToString() == h_targyfelv_elokov.ToString()) && (int.Parse(eredmeny2[i].Jegye) > 1))
                    {
                        van3++;
                    }
                }

                // Leadta-e már korábban a tárgyat.
                for (int i = 0; i < eredmeny2.Length; i++)
                {
                    if ((eredmeny2[i].Torolve.ToString() == "false") && (eredmeny2[i].Kodja.ToString() == h_targyfelv_id.ToString()))
                    {
                        van4++;
                        id = i;
                    }
                }

                if (van4 == 0)
                {
                    if ((van1 == 0) && (van2 == 0) && (van3 == 1))
                    {
                        // új.
                        Hallgato_index felvesz = new Hallgato_index(eredmeny1[0].Neve.ToString(), "false", eredmeny1[0].Kodja.ToString(), eredmeny1[0].Kreditje.ToString(), eredmeny1[0].Ideje.ToString(), eredmeny1[0].Oktatoja.ToString(), aktualis_felev.ToString(), eredmeny1[0].Kovetelmenye.ToString(), eredmeny1[0].Elokovetelmenye.ToString(), targy_kulcs.ToString(), belepo.Kodja.ToString(), "", "");
                        targy_kulcs++;

                        server.Hallgato_Index_Hozzaad(felvesz);
                    }
                    else
                    {
                        if (van2 > 0)
                        {
                            MessageBox.Show("A tárgy már fel van véve.");
                        }
                        if (van1 > 0)
                        {
                            MessageBox.Show("A tárgy már teljesítve van.");
                        }
                        if (van3 == 0)
                        {
                            MessageBox.Show("A tárgy előkövetelménye nincs teljesítve.");
                        }
                    }
                }
                else
                {
                    if (van4 == 1)
                    {
                        // Visszaállítás.
                        Hallgato_index mit = new Hallgato_index(eredmeny2[id].Neve.ToString(), eredmeny2[id].Torolve.ToString(), eredmeny2[id].Kodja.ToString(), eredmeny2[id].Kreditje.ToString(), eredmeny2[id].Ideje.ToString(), eredmeny2[id].Oktatoja.ToString(), eredmeny2[id].Feleve.ToString(), eredmeny2[id].Kovetelmenye.ToString(), eredmeny2[id].Elokovetelmenye.ToString(), eredmeny2[id].Idje.ToString(), eredmeny2[id].Krakenje.ToString(), eredmeny2[id].Jegye.ToString(), eredmeny2[id].Alairasa.ToString());
                        Hallgato_index mire = new Hallgato_index(eredmeny2[id].Neve.ToString(), "false", eredmeny2[id].Kodja.ToString(), eredmeny2[id].Kreditje.ToString(), eredmeny2[id].Ideje.ToString(), eredmeny2[id].Oktatoja.ToString(), eredmeny2[id].Feleve.ToString(), eredmeny2[id].Kovetelmenye.ToString(), eredmeny2[id].Elokovetelmenye.ToString(), eredmeny2[id].Idje.ToString(), eredmeny2[id].Krakenje.ToString(), eredmeny2[id].Jegye.ToString(), eredmeny2[id].Alairasa.ToString());

                        server.Hallgato_Index_Modosit(mit, mire);
                    }
                    else
                    {
                        MessageBox.Show("Több leadás is volt.");
                    }
                }

                Hallgato_Targyfelvetel_Betoltes();
            }
            catch
            { }
        }

        /// <summary>
        /// Delete a subject form a student.
        /// </summary>
        private void Hallgato_Targyfelvetel_Leadas()
        {
            try
            {
                string megy = "hallgato_targyfelvetel_leadas";
                string felev = comboBox1.Text.ToString();
                string kod = belepo.Kodja.ToString() + "*" + h_targyfelv_id.ToString();
                Hallgato_index[] eredmeny = server.Hallgato_Index_Lista(megy, felev, kod);

                if (eredmeny[0].Kodja.ToString() == "0")
                {
                    MessageBox.Show("Nincs ilyen tárgy felvéve.");
                }
                else
                {
                    Hallgato_index mit = new Hallgato_index(eredmeny[0].Neve.ToString(), eredmeny[0].Torolve.ToString(), eredmeny[0].Kodja.ToString(), eredmeny[0].Kreditje.ToString(), eredmeny[0].Ideje.ToString(), eredmeny[0].Oktatoja.ToString(), eredmeny[0].Feleve.ToString(), eredmeny[0].Kovetelmenye.ToString(), eredmeny[0].Elokovetelmenye.ToString(), eredmeny[0].Idje.ToString(), eredmeny[0].Krakenje.ToString(), eredmeny[0].Jegye.ToString(), eredmeny[0].Alairasa.ToString());
                    Hallgato_index mire = new Hallgato_index(eredmeny[0].Neve.ToString(), "false", eredmeny[0].Kodja.ToString(), eredmeny[0].Kreditje.ToString(), eredmeny[0].Ideje.ToString(), eredmeny[0].Oktatoja.ToString(), eredmeny[0].Feleve.ToString(), eredmeny[0].Kovetelmenye.ToString(), eredmeny[0].Elokovetelmenye.ToString(), eredmeny[0].Idje.ToString(), eredmeny[0].Krakenje.ToString(), eredmeny[0].Jegye.ToString(), eredmeny[0].Alairasa.ToString());
                    server.Hallgato_Index_Modosit(mit, mire);
                }

                Hallgato_Targyfelvetel_Betoltes();
            }
            catch
            { }
        }

        #endregion Targyfelvetel

        #region Leckekonyv

        /// <summary>
        /// Handles the Click event of the button2 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void button2_Click(object sender, EventArgs e)
        {
            Hallgato_Leckekonyv_Listazas();
        }

        /// <summary>
        /// Handles the Layout event of the tabPage7 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="LayoutEventArgs"/> instance containing the event data.</param>
        private void tabPage7_Layout(object sender, LayoutEventArgs e)
        {
            Hallgato_Leckekonyv_Betoltes();
        }

        /// <summary>
        /// Lists the subjects.
        /// </summary>
        private void Hallgato_Leckekonyv_Listazas()
        {
            dataGridView2.Rows.Clear();

            if (comboBox3.Text.ToString() != "")
            {
                try
                {
                    string megy = "hallgato_leckekonyv";
                    string felev = comboBox3.Text.ToString();
                    string kod = belepo.Kodja.ToString();
                    Hallgato_index[] eredmeny = server.Hallgato_Index_Lista(megy, felev, kod);

                    for (int i = 0; i < eredmeny.Length; i++)
                    {
                        string[] oszlop = new string[8] { "", "", "", "", "", "", "", "" };
                        oszlop[0] = eredmeny[i].Neve.ToString();
                        oszlop[1] = eredmeny[i].Kodja.ToString();
                        oszlop[2] = eredmeny[i].Kovetelmenye.ToString();
                        oszlop[3] = eredmeny[i].Kreditje.ToString();
                        oszlop[4] = eredmeny[i].Oktatoja.ToString();
                        oszlop[5] = eredmeny[i].Alairasa.ToString();
                        oszlop[6] = eredmeny[i].Jegye.ToString();
                        oszlop[7] = eredmeny[i].Ideje.ToString();
                        dataGridView2.Rows.Add(oszlop);
                    }
                }
                catch
                { }
            }
        }

        /// <summary>
        /// Loads the subjects.
        /// </summary>
        private void Hallgato_Leckekonyv_Betoltes()
        {
            dataGridView2.Rows.Clear();
            comboBox3.Text = "";
        }

        #endregion Leckekonyv

        #region Uzenetek

        /// <summary>
        /// Handles the Click event of the button9 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void button9_Click(object sender, EventArgs e)
        {
            Uzenetek_Beerkezett_Torles();
        }

        /// <summary>
        /// Handles the CellClick event of the dataGridView3 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="DataGridViewCellEventArgs"/> instance containing the event data.</param>
        private void dataGridView3_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            Uzenetek_Beerkezett_Kijeloles();
        }

        /// <summary>
        /// Handles the CellClick event of the dataGridView4 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="DataGridViewCellEventArgs"/> instance containing the event data.</param>
        private void dataGridView4_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            Uzenetek_Elkuldott_Kijeloles();
        }

        /// <summary>
        /// Handles the Click event of the button10 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void button10_Click(object sender, EventArgs e)
        {
            Uzenetek_Elkuldott_Torles();
        }

        /// <summary>
        /// Handles the CellClick event of the dataGridView5 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="DataGridViewCellEventArgs"/> instance containing the event data.</param>
        private void dataGridView5_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            Uzenetek_Piszkozat_Kijeloles();
        }

        /// <summary>
        /// Handles the Click event of the button6 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void button6_Click(object sender, EventArgs e)
        {
            Uzenetek_Piszkozat_Kuldes();
        }

        /// <summary>
        /// Handles the Click event of the button7 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void button7_Click(object sender, EventArgs e)
        {
            Uzenetek_Piszkozat_Mentes();
        }

        /// <summary>
        /// Handles the Click event of the button8 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void button8_Click(object sender, EventArgs e)
        {
            Uzenetek_Piszkozat_Torles();
        }

        /// <summary>
        /// Handles the TextChanged event of the textBox9 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void textBox9_TextChanged(object sender, EventArgs e)
        {
            Uzenetek_Piszkozat_Cimzett();
        }

        /// <summary>
        /// Handles the TextChanged event of the textBox8 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void textBox8_TextChanged(object sender, EventArgs e)
        {
            Uzenetek_Piszkozat_Targy();
        }

        /// <summary>
        /// Handles the TextChanged event of the richTextBox4 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void richTextBox4_TextChanged(object sender, EventArgs e)
        {
            Uzenetek_Piszkozat_Szoveg();
        }

        /// <summary>
        /// Handles the Click event of the button3 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void button3_Click(object sender, EventArgs e)
        {
            Uzenetek_Uj_Kuldes();
        }

        /// <summary>
        /// Handles the Click event of the button4 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void button4_Click(object sender, EventArgs e)
        {
            Uzenetek_Uj_Piszkozat();
        }

        /// <summary>
        /// Handles the Click event of the button5 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void button5_Click(object sender, EventArgs e)
        {
            Hallgato_Uzenetek_Uj_Betoltes();
        }

        /// <summary>
        /// Handles the TextChanged event of the textBox2 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            Uzenetek_Uj_Cimzett();
        }

        /// <summary>
        /// Handles the TextChanged event of the textBox3 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            Uzenetek_Uj_Targy();
        }

        /// <summary>
        /// Handles the TextChanged event of the richTextBox3 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void richTextBox3_TextChanged(object sender, EventArgs e)
        {
            Uzenetek_Uj_Szoveg();
        }

        /// <summary>
        /// Handles the Layout event of the tabPage8 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="LayoutEventArgs"/> instance containing the event data.</param>
        private void tabPage8_Layout(object sender, LayoutEventArgs e)
        {
            Hallgato_Uzenetek_Betoltes();
        }

        /// <summary>
        /// Handles the Layout event of the tabPage25 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="LayoutEventArgs"/> instance containing the event data.</param>
        private void tabPage25_Layout(object sender, LayoutEventArgs e)
        {
            Hallgato_Uzenetek_Bejovo_Betoltes();
        }

        /// <summary>
        /// Handles the Layout event of the tabPage26 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="LayoutEventArgs"/> instance containing the event data.</param>
        private void tabPage26_Layout(object sender, LayoutEventArgs e)
        {
            Hallgato_Uzenetek_Elkuldott_Betoltes();
        }

        /// <summary>
        /// Handles the Layout event of the tabPage27 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="LayoutEventArgs"/> instance containing the event data.</param>
        private void tabPage27_Layout(object sender, LayoutEventArgs e)
        {
            Hallgato_Uzenetek_Piszkozat_Betoltes();
        }

        /// <summary>
        /// Handles the Layout event of the tabPage28 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="LayoutEventArgs"/> instance containing the event data.</param>
        private void tabPage28_Layout(object sender, LayoutEventArgs e)
        {
            Hallgato_Uzenetek_Uj_Betoltes();
        }

        /// <summary>
        /// Loads the incoming messages.
        /// </summary>
        private void Hallgato_Uzenetek_Bejovo_Betoltes()
        {
            dataGridView3.Rows.Clear();
            button9.Enabled = false;

            // Új Üzenetek számának megjelenítése.
            Uzenetek_Bejovo_Olvasatlan_Betoltes();

            // Bejövő üzenetek megjelenítése.
            try
            {
                string megy = "uzenetek_bejovo_lista";
                string kod = belepo.Kodja.ToString();
                Uzenet[] eredmeny = server.Uzenet_Lista(megy, uzenet_id2, kod, uzenet_id3, uzenet_id4, uzenet_id1);

                for (int i = 0; i < eredmeny.Length; i++)
                {
                    string[] oszlop = new string[4] { "", "", "", "" };
                    oszlop[0] = eredmeny[i].Kuldoje.ToString();
                    oszlop[1] = eredmeny[i].Targya.ToString();
                    oszlop[2] = eredmeny[i].Ideje.ToString();
                    oszlop[3] = eredmeny[i].Szovege.ToString();
                    dataGridView3.Rows.Add(oszlop);
                }
            }
            catch
            { }
        }

        /// <summary>
        /// Loads the sent messages.
        /// </summary>
        private void Hallgato_Uzenetek_Elkuldott_Betoltes()
        {
            dataGridView4.Rows.Clear();
            button10.Enabled = false;

            try
            {
                string megy = "uzenetek_elkuldott_lista";
                string kod = belepo.Kodja.ToString();
                Uzenet[] eredmeny = server.Uzenet_Lista(megy, kod, uzenet_id2, uzenet_id3, uzenet_id4, uzenet_id1);

                for (int i = 0; i < eredmeny.Length; i++)
                {
                    string[] oszlop = new string[4] { "", "", "", "" };
                    oszlop[0] = eredmeny[i].Cimzettje.ToString();
                    oszlop[1] = eredmeny[i].Targya.ToString();
                    oszlop[2] = eredmeny[i].Ideje.ToString();
                    oszlop[3] = eredmeny[i].Szovege.ToString();
                    dataGridView4.Rows.Add(oszlop);
                }
            }
            catch
            { }
        }

        /// <summary>
        /// Loads the draft messages.
        /// </summary>
        private void Hallgato_Uzenetek_Piszkozat_Betoltes()
        {
            dataGridView5.Rows.Clear();
            button6.Enabled = false;
            button7.Enabled = false;
            button8.Enabled = false;
            textBox8.Text = "";
            textBox9.Text = "";
            richTextBox4.Text = "";
            cim_valtozas = false;
            targy_valtozas = false;
            szoveg_valtozas = false;

            try
            {
                string megy = "uzenetek_piszkozat_lista";
                string kod = belepo.Kodja.ToString();
                Uzenet[] eredmeny = server.Uzenet_Lista(megy, kod, uzenet_id2, uzenet_id3, uzenet_id4, uzenet_id1);

                for (int i = 0; i < eredmeny.Length; i++)
                {
                    string[] oszlop = new string[4] { "", "", "", "" };
                    oszlop[0] = eredmeny[i].Cimzettje.ToString();
                    oszlop[1] = eredmeny[i].Targya.ToString();
                    oszlop[2] = eredmeny[i].Ideje.ToString();
                    oszlop[3] = eredmeny[i].Szovege.ToString();
                    dataGridView5.Rows.Add(oszlop);
                }
            }
            catch
            { }
        }

        /// <summary>
        /// Loads messages.
        /// </summary>
        private void Hallgato_Uzenetek_Betoltes()
        { }

        /// <summary>
        /// Loads the values for new messages.
        /// </summary>
        private void Hallgato_Uzenetek_Uj_Betoltes()
        {
            textBox2.Text = "";
            textBox3.Text = "";
            richTextBox3.Text = "";
            cim_valtozas = false;
            targy_valtozas = false;
            szoveg_valtozas = false;
            button3.Enabled = false;
            button4.Enabled = false;
            button5.Enabled = false;
        }

        #endregion Uzenetek

        #region Targyak

        /// <summary>
        /// Handles the Click event of the button14 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void button14_Click(object sender, EventArgs e)
        {
            Hallgato_Felvett_Targyak();
        }

        /// <summary>
        /// Handles the Layout event of the tabPage11 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="LayoutEventArgs"/> instance containing the event data.</param>
        private void tabPage11_Layout(object sender, LayoutEventArgs e)
        {
            Hallgato_Teljesitett_Targyak_Betoltes();
        }

        /// <summary>
        /// Loads the students subjects.
        /// </summary>
        private void Hallgato_Felvett_Targyak()
        {
            dataGridView9.Rows.Clear();

            if (comboBox4.Text.ToString() != "")
            {
                try
                {
                    string megy = "hallgato_felvett_targyak";
                    string felev = comboBox4.Text.ToString();
                    string kod = belepo.Kodja.ToString();
                    Hallgato_index[] eredmeny = server.Hallgato_Index_Lista(megy, felev, kod);

                    for (int i = 0; i < eredmeny.Length; i++)
                    {
                        string[] oszlop = new string[8] { "", "", "", "", "", "", "", "" };
                        oszlop[0] = eredmeny[i].Neve.ToString();
                        oszlop[1] = eredmeny[i].Kodja.ToString();
                        oszlop[2] = eredmeny[i].Kovetelmenye.ToString();
                        oszlop[3] = eredmeny[i].Kreditje.ToString();
                        oszlop[4] = eredmeny[i].Oktatoja.ToString();
                        oszlop[5] = eredmeny[i].Alairasa.ToString();
                        oszlop[6] = eredmeny[i].Jegye.ToString();
                        oszlop[7] = eredmeny[i].Ideje.ToString();
                        dataGridView9.Rows.Add(eredmeny[i]);
                    }
                }
                catch
                { }
            }
        }

        /// <summary>
        /// Loads the students done subjects.
        /// </summary>
        private void Hallgato_Teljesitett_Targyak_Betoltes()
        {
            dataGridView7.Rows.Clear();
            dataGridView9.Rows.Clear();

            try
            {
                comboBox4.Text = "";
                string megy = "hallgato_teljesitett_targyak";
                string felev = "mind";
                string kod = belepo.Kodja.ToString();
                Hallgato_index[] eredmeny = server.Hallgato_Index_Lista(megy, felev, kod);

                for (int i = 0; i < eredmeny.Length; i++)
                {
                    string[] oszlop = new string[9] { "", "", "", "", "", "", "", "", "" };
                    oszlop[0] = eredmeny[i].Neve.ToString();
                    oszlop[1] = eredmeny[i].Kodja.ToString();
                    oszlop[2] = eredmeny[i].Kovetelmenye.ToString();
                    oszlop[3] = eredmeny[i].Kreditje.ToString();
                    oszlop[4] = eredmeny[i].Oktatoja.ToString();
                    oszlop[5] = eredmeny[i].Alairasa.ToString();
                    oszlop[6] = eredmeny[i].Jegye.ToString();
                    oszlop[7] = eredmeny[i].Ideje.ToString();
                    oszlop[8] = eredmeny[i].Feleve.ToString();
                    dataGridView7.Rows.Add(eredmeny[i]);
                }
            }
            catch
            { }
        }

        #endregion Targyak

        #endregion StudentGUI

        #region TeacherGUI

        #region Adatok

        /// <summary>
        /// Handles the Click event of the button42 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void button42_Click(object sender, EventArgs e)
        {
            Adatok_Jelszo_Modosítas();
        }

        /// <summary>
        /// Handles the TextChanged event of the textBox44 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void textBox44_TextChanged(object sender, EventArgs e)
        {
            Adatok_Jelszo_Gomb();
        }

        /// <summary>
        /// Handles the Layout event of the tabPage15 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="LayoutEventArgs"/> instance containing the event data.</param>
        private void tabPage15_Layout(object sender, LayoutEventArgs e)
        {
            Oktato_Adatok_Betoltes();
        }

        /// <summary>
        /// Fills the values.
        /// </summary>
        private void Oktato_Adatok_Betoltes()
        {
            button42.Enabled = false;
            textBox44.Text = "";

            try
            {
                label2.Text = "Név: " + belepo.Neve.ToString();
                label36.Text = "Kraken kód: " + belepo.Kodja.ToString();
                label35.Text = "Jelszó: " + belepo.Jelszava.ToString();
                label34.Text = "E-mail cím: " + belepo.Emailje.ToString();
                label33.Text = "Fizetés: " + belepo.Fizetese.ToString();
                label32.Text = "Születési hely: " + belepo.Szuletesi_helye.ToString();
                label31.Text = "Születési idő: " + belepo.Szuletesi_ideje.ToString();
                label30.Text = "Anyja neve: " + belepo.Anyja_neve.ToString();
                label29.Text = "Bankszámla szám: " + belepo.Bankszamla.ToString();
                label28.Text = "Státusz: " + belepo.Statusza.ToString();
                label27.Text = "Beiratkozás ideje: " + belepo.Beiratkozva.ToString();
                label26.Text = "Nem: " + belepo.Neme.ToString();
                label49.Text = "Jogviszony: " + belepo.Jogviszonya.ToString();
                label59.Text = "Lakcím: " + belepo.Lakcime.ToString();
                label109.Text = "Telefonszám: " + belepo.Telefonja.ToString();
            }
            catch
            { }
        }

        #endregion Adatok

        #region Oktatott_targyak

        /// <summary>
        /// Handles the Click event of the button30 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void button30_Click(object sender, EventArgs e)
        {
            Oktato_Oktatott_Targy();
        }

        /// <summary>
        /// Handles the Layout event of the tabPage19 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="LayoutEventArgs"/> instance containing the event data.</param>
        private void tabPage19_Layout(object sender, LayoutEventArgs e)
        {
            Oktato_Oktatott_Targyak_Betoltes();
        }

        /// <summary>
        /// Lists the subjects.
        /// </summary>
        private void Oktato_Oktatott_Targy()
        {
            dataGridView15.Rows.Clear();

            if (comboBox5.Text.ToString() != "")
            {
                try
                {
                    string megy = "oktato_oktatott_targyak";
                    string felev = comboBox5.Text.ToString();
                    string kod = belepo.Kodja.ToString();
                    Oktato_index[] eredmeny = server.Oktato_Index_Lista(megy, felev, kod);

                    for (int i = 0; i < eredmeny.Length; i++)
                    {
                        string[] oszlop = new string[6] { "", "", "", "", "", "" };
                        oszlop[0] = eredmeny[i].Neve.ToString();
                        oszlop[1] = eredmeny[i].Kodja.ToString();
                        oszlop[2] = eredmeny[i].Kreditje.ToString();
                        oszlop[3] = eredmeny[i].Kovetelmenye.ToString();
                        oszlop[4] = eredmeny[i].Elokovetelmenye.ToString();
                        oszlop[5] = eredmeny[i].Ideje.ToString();
                        dataGridView15.Rows.Add(oszlop);
                    }
                }
                catch
                { }
            }
        }

        /// <summary>
        /// Lists the subjects.
        /// </summary>
        private void Oktato_Oktatott_Targyak_Betoltes()
        {
            dataGridView15.Rows.Clear();
        }

        #endregion Oktatott_targyak

        #region Uzenetek

        /// <summary>
        /// Handles the Layout event of the tabPage18 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="LayoutEventArgs"/> instance containing the event data.</param>
        private void tabPage18_Layout(object sender, LayoutEventArgs e)
        {
            Oktato_Uzenetek_Betoltes();
        }

        /// <summary>
        /// Handles the Layout event of the tabPage9 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="LayoutEventArgs"/> instance containing the event data.</param>
        private void tabPage9_Layout(object sender, LayoutEventArgs e)
        {
            Oktato_Uzenetek_Bejovo_Betoltes();
        }

        /// <summary>
        /// Handles the Layout event of the tabPage10 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="LayoutEventArgs"/> instance containing the event data.</param>
        private void tabPage10_Layout(object sender, LayoutEventArgs e)
        {
            Oktato_Uzenetek_Elkuldott_Betoltes();
        }

        /// <summary>
        /// Handles the Layout event of the tabPage29 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="LayoutEventArgs"/> instance containing the event data.</param>
        private void tabPage29_Layout(object sender, LayoutEventArgs e)
        {
            Oktato_Uzenetek_Piszkozat_Betoltes();
        }

        /// <summary>
        /// Handles the TextChanged event of the textBox10 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void textBox10_TextChanged(object sender, EventArgs e)
        {
            Uzenetek_Piszkozat_Targy();
        }

        /// <summary>
        /// Handles the TextChanged event of the textBox11 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void textBox11_TextChanged(object sender, EventArgs e)
        {
            Uzenetek_Piszkozat_Cimzett();
        }

        /// <summary>
        /// Handles the TextChanged event of the richTextBox7 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void richTextBox7_TextChanged(object sender, EventArgs e)
        {
            Uzenetek_Piszkozat_Szoveg();
        }

        /// <summary>
        /// Handles the CellClick event of the dataGridView11 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="DataGridViewCellEventArgs"/> instance containing the event data.</param>
        private void dataGridView11_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            Uzenetek_Piszkozat_Kijeloles();
        }

        /// <summary>
        /// Handles the CellClick event of the dataGridView10 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="DataGridViewCellEventArgs"/> instance containing the event data.</param>
        private void dataGridView10_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            Uzenetek_Elkuldott_Kijeloles();
        }

        /// <summary>
        /// Handles the CellClick event of the dataGridView6 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="DataGridViewCellEventArgs"/> instance containing the event data.</param>
        private void dataGridView6_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            Uzenetek_Beerkezett_Kijeloles();
        }

        /// <summary>
        /// Handles the TextChanged event of the textBox5 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            Uzenetek_Uj_Cimzett();
        }

        /// <summary>
        /// Handles the TextChanged event of the textBox4 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            Uzenetek_Uj_Targy();
        }

        /// <summary>
        /// Handles the TextChanged event of the richTextBox8 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void richTextBox8_TextChanged(object sender, EventArgs e)
        {
            Uzenetek_Uj_Szoveg();
        }

        /// <summary>
        /// Handles the Layout event of the tabPage30 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="LayoutEventArgs"/> instance containing the event data.</param>
        private void tabPage30_Layout(object sender, LayoutEventArgs e)
        {
            Oktato_Uzenetek_Uj_Betoltes();
        }

        /// <summary>
        /// Handles the Click event of the button19 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void button19_Click(object sender, EventArgs e)
        {
            Oktato_Uzenetek_Uj_Betoltes();
        }

        /// <summary>
        /// Handles the Click event of the button20 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void button20_Click(object sender, EventArgs e)
        {
            Uzenetek_Uj_Piszkozat();
        }

        /// <summary>
        /// Handles the Click event of the button21 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void button21_Click(object sender, EventArgs e)
        {
            Uzenetek_Uj_Kuldes();
        }

        /// <summary>
        /// Handles the Click event of the button16 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void button16_Click(object sender, EventArgs e)
        {
            Uzenetek_Piszkozat_Torles();
        }

        /// <summary>
        /// Handles the Click event of the button17 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void button17_Click(object sender, EventArgs e)
        {
            Uzenetek_Piszkozat_Mentes();
        }

        /// <summary>
        /// Handles the Click event of the button18 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void button18_Click(object sender, EventArgs e)
        {
            Uzenetek_Piszkozat_Kuldes();
        }

        /// <summary>
        /// Handles the Click event of the button15 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void button15_Click(object sender, EventArgs e)
        {
            Uzenetek_Elkuldott_Torles();
        }

        /// <summary>
        /// Handles the Click event of the button11 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void button11_Click(object sender, EventArgs e)
        {
            Uzenetek_Beerkezett_Torles();
        }

        /// <summary>
        /// Loads messages.
        /// </summary>
        private void Oktato_Uzenetek_Betoltes()
        { }

        /// <summary>
        /// Loads the incoming messages.
        /// </summary>
        private void Oktato_Uzenetek_Bejovo_Betoltes()
        {
            dataGridView6.Rows.Clear();
            button11.Enabled = false;
            richTextBox5.Text = "";

            // Új Üzenetek számának megjelenítése.
            Uzenetek_Bejovo_Olvasatlan_Betoltes();

            // Bejövő üzenetek megjelenítése.
            try
            {
                string megy = "uzenetek_bejovo_lista";
                string kod = belepo.Kodja.ToString();
                Uzenet[] eredmeny = server.Uzenet_Lista(megy, uzenet_id2, kod, uzenet_id3, uzenet_id4, uzenet_id1);

                for (int i = 0; i < eredmeny.Length; i++)
                {
                    string[] oszlop = new string[4] { "", "", "", "" };
                    oszlop[0] = eredmeny[i].Kuldoje.ToString();
                    oszlop[1] = eredmeny[i].Targya.ToString();
                    oszlop[2] = eredmeny[i].Ideje.ToString();
                    oszlop[3] = eredmeny[i].Szovege.ToString();
                    dataGridView6.Rows.Add(oszlop);
                }
            }
            catch
            { }
        }

        /// <summary>
        /// Loads the sent messages.
        /// </summary>
        private void Oktato_Uzenetek_Elkuldott_Betoltes()
        {
            dataGridView10.Rows.Clear();
            button15.Enabled = false;
            richTextBox6.Text = "";

            try
            {
                string megy = "uzenetek_elkuldott_lista";
                string kod = belepo.Kodja.ToString();
                Uzenet[] eredmeny = server.Uzenet_Lista(megy, kod, uzenet_id2, uzenet_id3, uzenet_id4, uzenet_id1);

                for (int i = 0; i < eredmeny.Length; i++)
                {
                    string[] oszlop = new string[4] { "", "", "", "" };
                    oszlop[0] = eredmeny[i].Cimzettje.ToString();
                    oszlop[1] = eredmeny[i].Targya.ToString();
                    oszlop[2] = eredmeny[i].Ideje.ToString();
                    oszlop[3] = eredmeny[i].Szovege.ToString();
                    dataGridView10.Rows.Add(oszlop);
                }
            }
            catch
            { }
        }

        /// <summary>
        /// Loads the draft messages.
        /// </summary>
        private void Oktato_Uzenetek_Piszkozat_Betoltes()
        {
            dataGridView11.Rows.Clear();
            button16.Enabled = false;
            button17.Enabled = false;
            button18.Enabled = false;
            textBox10.Text = "";
            textBox11.Text = "";
            richTextBox7.Text = "";
            cim_valtozas = false;
            targy_valtozas = false;
            szoveg_valtozas = false;

            try
            {
                string megy = "uzenetek_piszkozat_lista";
                string kod = belepo.Kodja.ToString();
                Uzenet[] eredmeny = server.Uzenet_Lista(megy, kod, uzenet_id2, uzenet_id3, uzenet_id4, uzenet_id1);

                for (int i = 0; i < eredmeny.Length; i++)
                {
                    string[] oszlop = new string[4] { "", "", "", "" };
                    oszlop[0] = eredmeny[i].Cimzettje.ToString();
                    oszlop[1] = eredmeny[i].Targya.ToString();
                    oszlop[2] = eredmeny[i].Ideje.ToString();
                    oszlop[3] = eredmeny[i].Szovege.ToString();
                    dataGridView11.Rows.Add(oszlop);
                }
            }
            catch
            { }
        }

        /// <summary>
        /// Loads the values for the new messages.
        /// </summary>
        private void Oktato_Uzenetek_Uj_Betoltes()
        {
            textBox4.Text = "";
            textBox5.Text = "";
            richTextBox8.Text = "";
            cim_valtozas = false;
            targy_valtozas = false;
            szoveg_valtozas = false;
            button19.Enabled = false;
            button20.Enabled = false;
            button21.Enabled = false;
        }

        #endregion Uzenetek

        #region Ertekeles

        /// <summary>
        /// Handles the Layout event of the tabPage20 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="LayoutEventArgs"/> instance containing the event data.</param>
        private void tabPage20_Layout(object sender, LayoutEventArgs e)
        {
            Oktato_Ertekeles_Betoltes();
        }

        /// <summary>
        /// Handles the Click event of the button31 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void button31_Click(object sender, EventArgs e)
        {
            Ertekeles_Targyak();
        }

        /// <summary>
        /// Handles the SelectedIndexChanged event of the listBox1 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Ertekeles_Hallgatok();
        }

        /// <summary>
        /// Handles the SelectedIndexChanged event of the comboBox17 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void comboBox17_SelectedIndexChanged(object sender, EventArgs e)
        {
            Ertekeles_Jegy_Gomb();
        }

        /// <summary>
        /// Handles the Click event of the button32 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void button32_Click(object sender, EventArgs e)
        {
            Ertekeles_Modositas_Gomb();
        }

        /// <summary>
        /// Handles the SelectedIndexChanged event of the comboBox16 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void comboBox16_SelectedIndexChanged(object sender, EventArgs e)
        {
            Ertekeles_Alairas_Gomb();
        }

        /// <summary>
        /// Handles the SelectedIndexChanged event of the listBox2 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            Ertekeles_Hallgato_Targyai();
        }

        /// <summary>
        /// Sets the values.
        /// </summary>
        private void Oktato_Ertekeles_Betoltes()
        {
            listBox1.Items.Clear();
            listBox2.Items.Clear();
            comboBox16.Text = "";
            comboBox17.Text = "";
            button32.Enabled = false;
            jegy_change = false;
            alairas_change = false;
        }

        #endregion Ertekeles

        #endregion TeacherGUI

        #region AdminGUI

        #region Adatok

        /// <summary>
        /// Handles the TextChanged event of the textBox45 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void textBox45_TextChanged(object sender, EventArgs e)
        {
            Adatok_Jelszo_Gomb();
        }

        /// <summary>
        /// Handles the Click event of the button43 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void button43_Click(object sender, EventArgs e)
        {
            Adatok_Jelszo_Modosítas();
        }

        /// <summary>
        /// Handles the Layout event of the tabPage13 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="LayoutEventArgs"/> instance containing the event data.</param>
        private void tabPage13_Layout(object sender, LayoutEventArgs e)
        {
            Admin_Adatok_Betoltes();
        }

        /// <summary>
        /// Loads the admin user's data.
        /// </summary>
        private void Admin_Adatok_Betoltes()
        {
            button43.Enabled = false;
            textBox45.Text = "";

            try
            {
                label4.Text = "Név: " + belepo.Neve.ToString();
                label47.Text = "Kraken kód: " + belepo.Kodja.ToString();
                label46.Text = "Jelszó: " + belepo.Jelszava.ToString();
                label45.Text = "E-mail cím: " + belepo.Emailje.ToString();
                label44.Text = "Fizetés: " + belepo.Fizetese.ToString();
                label43.Text = "Születési hely: " + belepo.Szuletesi_helye.ToString();
                label42.Text = "Születési idő: " + belepo.Szuletesi_ideje.ToString();
                label41.Text = "Anyja neve: " + belepo.Anyja_neve.ToString();
                label40.Text = "Bankszámla szám: " + belepo.Bankszamla.ToString();
                label39.Text = "Státusz: " + belepo.Statusza.ToString();
                label38.Text = "Beiratkozás ideje: " + belepo.Beiratkozva.ToString();
                label37.Text = "Nem: " + belepo.Neme.ToString();
                label50.Text = "Jogviszony: " + belepo.Jogviszonya.ToString();
                label60.Text = "Lakcím: " + belepo.Lakcime.ToString();
                label110.Text = "Telefonszám: " + belepo.Telefonja.ToString();
            }
            catch
            { }
        }

        #endregion Adatok

        #region Uzenetek

        /// <summary>
        /// Handles the Layout event of the tabPage14 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="LayoutEventArgs"/> instance containing the event data.</param>
        private void tabPage14_Layout(object sender, LayoutEventArgs e)
        {
            Admin_Uzenetek_Betoltes();
        }

        /// <summary>
        /// Handles the Layout event of the tabPage31 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="LayoutEventArgs"/> instance containing the event data.</param>
        private void tabPage31_Layout(object sender, LayoutEventArgs e)
        {
            Admin_Uzenetek_Bejovo_Betoltes();
        }

        /// <summary>
        /// Handles the Layout event of the tabPage32 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="LayoutEventArgs"/> instance containing the event data.</param>
        private void tabPage32_Layout(object sender, LayoutEventArgs e)
        {
            Admin_Uzenetek_Elkuldott_Betoltes();
        }

        /// <summary>
        /// Handles the Layout event of the tabPage33 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="LayoutEventArgs"/> instance containing the event data.</param>
        private void tabPage33_Layout(object sender, LayoutEventArgs e)
        {
            Admin_Uzenetek_Piszkozat_Betoltes();
        }

        /// <summary>
        /// Handles the Layout event of the tabPage34 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="LayoutEventArgs"/> instance containing the event data.</param>
        private void tabPage34_Layout(object sender, LayoutEventArgs e)
        {
            Admin_Uzenetek_Uj_Betoltes();
        }

        /// <summary>
        /// Handles the CellClick event of the dataGridView12 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="DataGridViewCellEventArgs"/> instance containing the event data.</param>
        private void dataGridView12_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            Uzenetek_Beerkezett_Kijeloles();
        }

        /// <summary>
        /// Handles the Click event of the button22 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void button22_Click(object sender, EventArgs e)
        {
            Uzenetek_Beerkezett_Torles();
        }

        /// <summary>
        /// Handles the CellClick event of the dataGridView13 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="DataGridViewCellEventArgs"/> instance containing the event data.</param>
        private void dataGridView13_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            Uzenetek_Elkuldott_Kijeloles();
        }

        /// <summary>
        /// Handles the Click event of the button23 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void button23_Click(object sender, EventArgs e)
        {
            Uzenetek_Elkuldott_Torles();
        }

        /// <summary>
        /// Handles the CellClick event of the dataGridView14 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="DataGridViewCellEventArgs"/> instance containing the event data.</param>
        private void dataGridView14_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            Uzenetek_Piszkozat_Kijeloles();
        }

        /// <summary>
        /// Handles the TextChanged event of the textBox87 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void textBox87_TextChanged(object sender, EventArgs e)
        {
            Uzenetek_Piszkozat_Cimzett();
        }

        /// <summary>
        /// Handles the TextChanged event of the textBox86 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void textBox86_TextChanged(object sender, EventArgs e)
        {
            Uzenetek_Piszkozat_Targy();
        }

        /// <summary>
        /// Handles the TextChanged event of the richTextBox11 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void richTextBox11_TextChanged(object sender, EventArgs e)
        {
            Uzenetek_Piszkozat_Szoveg();
        }

        /// <summary>
        /// Handles the Click event of the button26 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void button26_Click(object sender, EventArgs e)
        {
            Uzenetek_Piszkozat_Kuldes();
        }

        /// <summary>
        /// Handles the Click event of the button25 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void button25_Click(object sender, EventArgs e)
        {
            Uzenetek_Piszkozat_Mentes();
        }

        /// <summary>
        /// Handles the Click event of the button24 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void button24_Click(object sender, EventArgs e)
        {
            Uzenetek_Piszkozat_Torles();
        }

        /// <summary>
        /// Handles the TextChanged event of the textBox7 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void textBox7_TextChanged(object sender, EventArgs e)
        {
            Uzenetek_Uj_Cimzett();
        }

        /// <summary>
        /// Handles the TextChanged event of the textBox6 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void textBox6_TextChanged(object sender, EventArgs e)
        {
            Uzenetek_Uj_Targy();
        }

        /// <summary>
        /// Handles the TextChanged event of the richTextBox12 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void richTextBox12_TextChanged(object sender, EventArgs e)
        {
            Uzenetek_Uj_Szoveg();
        }

        /// <summary>
        /// Handles the Click event of the button29 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void button29_Click(object sender, EventArgs e)
        {
            Uzenetek_Uj_Kuldes();
        }

        /// <summary>
        /// Handles the Click event of the button28 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void button28_Click(object sender, EventArgs e)
        {
            Uzenetek_Uj_Piszkozat();
        }

        /// <summary>
        /// Handles the Click event of the button27 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void button27_Click(object sender, EventArgs e)
        {
            Admin_Uzenetek_Uj_Betoltes();
        }

        /// <summary>
        /// Loads the admin user's messages.
        /// </summary>
        private void Admin_Uzenetek_Betoltes()
        { }

        /// <summary>
        /// Loads the admin user's incoming messages.
        /// </summary>
        private void Admin_Uzenetek_Bejovo_Betoltes()
        {
            dataGridView12.Rows.Clear();
            button22.Enabled = false;

            // Új Üzenetek számának megjelenítése.
            Uzenetek_Bejovo_Olvasatlan_Betoltes();

            // Bejövő üzenetek megjelenítése.
            try
            {
                string megy = "uzenetek_bejovo_lista";
                string kod = belepo.Kodja.ToString();
                Uzenet[] eredmeny = server.Uzenet_Lista(megy, uzenet_id2, kod, uzenet_id3, uzenet_id4, uzenet_id1);

                for (int i = 0; i < eredmeny.Length; i++)
                {
                    string[] oszlop = new string[4] { "", "", "", "" };
                    oszlop[0] = eredmeny[i].Kuldoje.ToString();
                    oszlop[1] = eredmeny[i].Targya.ToString();
                    oszlop[2] = eredmeny[i].Ideje.ToString();
                    oszlop[3] = eredmeny[i].Szovege.ToString();
                    dataGridView12.Rows.Add(oszlop);
                }
            }
            catch
            { }
        }

        /// <summary>
        /// Loads the admin user's sent messages.
        /// </summary>
        private void Admin_Uzenetek_Elkuldott_Betoltes()
        {
            dataGridView13.Rows.Clear();
            button23.Enabled = false;

            try
            {
                string megy = "uzenetek_elkuldott_lista";
                string kod = belepo.Kodja.ToString();
                Uzenet[] eredmeny = server.Uzenet_Lista(megy, kod, uzenet_id2, uzenet_id3, uzenet_id4, uzenet_id1);

                for (int i = 0; i < eredmeny.Length; i++)
                {
                    string[] oszlop = new string[4] { "", "", "", "" };
                    oszlop[0] = eredmeny[i].Cimzettje.ToString();
                    oszlop[1] = eredmeny[i].Targya.ToString();
                    oszlop[2] = eredmeny[i].Ideje.ToString();
                    oszlop[3] = eredmeny[i].Szovege.ToString();
                    dataGridView13.Rows.Add(oszlop);
                }
            }
            catch
            { }
        }

        /// <summary>
        /// Loads the admin user's draft messages.
        /// </summary>
        private void Admin_Uzenetek_Piszkozat_Betoltes()
        {
            dataGridView14.Rows.Clear();
            button26.Enabled = false;
            button25.Enabled = false;
            button24.Enabled = false;
            textBox86.Text = "";
            textBox87.Text = "";
            richTextBox11.Text = "";
            cim_valtozas = false;
            targy_valtozas = false;
            szoveg_valtozas = false;

            try
            {
                string megy = "uzenetek_piszkozat_lista";
                string kod = belepo.Kodja.ToString();
                Uzenet[] eredmeny = server.Uzenet_Lista(megy, kod, uzenet_id2, uzenet_id3, uzenet_id4, uzenet_id1);

                for (int i = 0; i < eredmeny.Length; i++)
                {
                    string[] oszlop = new string[4] { "", "", "", "" };
                    oszlop[0] = eredmeny[i].Cimzettje.ToString();
                    oszlop[1] = eredmeny[i].Targya.ToString();
                    oszlop[2] = eredmeny[i].Ideje.ToString();
                    oszlop[3] = eredmeny[i].Szovege.ToString();
                    dataGridView14.Rows.Add(oszlop);
                }
            }
            catch
            { }
        }

        /// <summary>
        /// Presets the admin user's new message writer.
        /// </summary>
        private void Admin_Uzenetek_Uj_Betoltes()
        {
            textBox6.Text = "";
            textBox7.Text = "";
            richTextBox12.Text = "";
            cim_valtozas = false;
            targy_valtozas = false;
            szoveg_valtozas = false;
            button27.Enabled = false;
            button28.Enabled = false;
            button29.Enabled = false;
        }

        #endregion Uzenetek

        #region Ertekeles

        /// <summary>
        /// Handles the Click event of the button34 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void button34_Click(object sender, EventArgs e)
        {
            Ertekeles_Targyak();
        }

        /// <summary>
        /// Handles the Click event of the button33 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void button33_Click(object sender, EventArgs e)
        {
            Ertekeles_Modositas_Gomb();
        }

        /// <summary>
        /// Handles the Layout event of the tabPage21 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="LayoutEventArgs"/> instance containing the event data.</param>
        private void tabPage21_Layout(object sender, LayoutEventArgs e)
        {
            Admin_Ertekeles_Betoltes();
        }

        /// <summary>
        /// Handles the SelectedIndexChanged event of the listBox4 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void listBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            Ertekeles_Hallgatok();
        }

        /// <summary>
        /// Handles the SelectedIndexChanged event of the listBox3 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void listBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            Ertekeles_Hallgato_Targyai();
        }

        /// <summary>
        /// Handles the SelectedIndexChanged event of the comboBox19 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void comboBox19_SelectedIndexChanged(object sender, EventArgs e)
        {
            Ertekeles_Alairas_Gomb();
        }

        /// <summary>
        /// Handles the SelectedIndexChanged event of the comboBox22 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void comboBox22_SelectedIndexChanged(object sender, EventArgs e)
        {
            Ertekeles_Jegy_Gomb();
        }

        /// <summary>
        /// Loads the admin user's evaluation values.
        /// </summary>
        private void Admin_Ertekeles_Betoltes()
        {
            listBox3.Items.Clear();
            listBox4.Items.Clear();
            comboBox19.Text = "";
            comboBox22.Text = "";
            button33.Enabled = false;
            jegy_change = false;
            alairas_change = false;
        }

        #endregion Ertekeles

        #region Targykezeles

        /// <summary>
        /// Handles the Layout event of the tabPage22 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="LayoutEventArgs"/> instance containing the event data.</param>
        private void tabPage22_Layout(object sender, LayoutEventArgs e)
        {
            Admin_Targykezeles_Betoltes();
        }

        /// <summary>
        /// Handles the Click event of the button35 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void button35_Click(object sender, EventArgs e)
        {
            Admin_Targykezeles_Hozzaadas();
        }

        /// <summary>
        /// Handles the Click event of the button36 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void button36_Click(object sender, EventArgs e)
        {
            Admin_Targykezeles_Modositas();
        }

        /// <summary>
        /// Handles the CellClick event of the dataGridView8 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="DataGridViewCellEventArgs"/> instance containing the event data.</param>
        private void dataGridView8_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            Admin_Targykezeles_Kijeloles();
        }

        /// <summary>
        /// Handles the Click event of the button44 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void button44_Click(object sender, EventArgs e)
        {
            Admin_Targykezeles_Torles();
        }

        /// <summary>
        /// Handles the CheckedChanged event of the radioButton1 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            Admin_Targykezeles_Gomb_Engedelyezes_Uj();
        }

        /// <summary>
        /// Handles the CheckedChanged event of the radioButton2 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            Admin_Targykezeles_Gomb_Engedelyezes_Uj();
        }

        /// <summary>
        /// Handles the TextChanged event of the textBox60 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void textBox60_TextChanged(object sender, EventArgs e)
        {
            Admin_Targykezeles_Nev();
        }

        /// <summary>
        /// Handles the TextChanged event of the textBox61 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void textBox61_TextChanged(object sender, EventArgs e)
        {
            Admin_Targykezeles_Kod();
        }

        /// <summary>
        /// Handles the TextChanged event of the textBox76 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void textBox76_TextChanged(object sender, EventArgs e)
        {
            Admin_Targykezeles_Kredit();
        }

        /// <summary>
        /// Handles the TextChanged event of the textBox77 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void textBox77_TextChanged(object sender, EventArgs e)
        {
            Admin_Targykezeles_Ido();
        }

        /// <summary>
        /// Handles the TextChanged event of the textBox83 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void textBox83_TextChanged(object sender, EventArgs e)
        {
            Admin_Targykezeles_Elokov();
        }

        /// <summary>
        /// Handles the TextChanged event of the comboBox8 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void comboBox8_TextChanged(object sender, EventArgs e)
        {
            Admin_Targykezeles_Oktato();
        }

        /// <summary>
        /// Handles the TextChanged event of the comboBox9 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void comboBox9_TextChanged(object sender, EventArgs e)
        {
            Admin_Targykezeles_Felev();
        }

        /// <summary>
        /// Select a subject.
        /// </summary>
        private void Admin_Targykezeles_Kijeloles()
        {
            Admin_Targykezeles_Urites();

            // Kijelölés esetén átadjuk az értékeket.
            if (dataGridView8.CurrentCellAddress.X >= 0)
            {
                try
                {
                    button35.Enabled = true;
                    button44.Enabled = true;

                    // A tárgy kódjának megszerzése.
                    int kijeloltsorindex = dataGridView8.SelectedCells[0].RowIndex;
                    DataGridViewRow kijeloltsor = dataGridView8.Rows[kijeloltsorindex];
                    string a_targy_id = kijeloltsor.Cells["Column2"].Value.ToString();
                    box = a_targy_id;

                    // A tárgy megkeresése.
                    string megy = "admin_targykezeles_kijeloles";
                    Targy[] eredmeny = server.Targy_Lista(megy, a_targy_id);
                    textBox14.Text = eredmeny[0].Neve.ToString();
                    textBox15.Text = eredmeny[0].Kodja.ToString();
                    textBox16.Text = eredmeny[0].Kreditje.ToString();
                    textBox17.Text = eredmeny[0].Ideje.ToString();
                    textBox18.Text = eredmeny[0].Elokovetelmenye.ToString();
                    textBox80.Text = eredmeny[0].Oktatoja.ToString();
                    textBox81.Text = eredmeny[0].Feleve.ToString();
                    textBox82.Text = eredmeny[0].Kovetelmenye.ToString();
                }
                catch
                { }
            }
        }

        /// <summary>
        /// Loads the subjects.
        /// </summary>
        private void Admin_Targykezeles_Betoltes()
        {
            dataGridView8.Rows.Clear();
            Admin_Targykezeles_Urites();
            button35.Enabled = false;
            button36.Enabled = false;
            button44.Enabled = false;
            textBox14.Enabled = false;
            textBox15.Enabled = false;

            try
            {
                string megy = "admin_targykezeles_betoltes";
                string kod = "";
                Targy[] eredmeny = server.Targy_Lista(megy, kod);

                for (int i = 0; i < eredmeny.Length; i++)
                {
                    string[] oszlop = new string[8];
                    oszlop[0] = eredmeny[i].Neve.ToString();
                    oszlop[1] = eredmeny[i].Kodja.ToString();
                    oszlop[2] = eredmeny[i].Kreditje.ToString();
                    oszlop[3] = eredmeny[i].Oktatoja.ToString();
                    oszlop[4] = eredmeny[i].Ideje.ToString();
                    oszlop[5] = eredmeny[i].Kovetelmenye.ToString();
                    oszlop[6] = eredmeny[i].Elokovetelmenye.ToString();
                    oszlop[7] = eredmeny[i].Feleve.ToString();
                    dataGridView8.Rows.Add(oszlop);
                }
            }
            catch
            { }
        }

        /// <summary>
        /// Clears the texts.
        /// </summary>
        private void Admin_Targykezeles_Urites()
        {
            textBox14.Text = "";
            textBox15.Text = "";
            textBox16.Text = "";
            textBox17.Text = "";
            textBox18.Text = "";
            textBox60.Text = "";
            textBox61.Text = "";
            textBox76.Text = "";
            textBox77.Text = "";
            textBox80.Text = "";
            textBox81.Text = "";
            textBox82.Text = "";
            textBox83.Text = "";
            radioButton1.Checked = false;
            radioButton2.Checked = false;
            comboBox8.Text = "";
            comboBox9.Text = "";
        }

        /// <summary>
        /// Delete subject.
        /// </summary>
        private void Admin_Targykezeles_Torles()
        {
            try
            {
                string megy = "admin_targykezeles_torles";
                string kod = box.ToString();
                Targy[] eredmeny = server.Targy_Lista(megy, kod);

                if (eredmeny.Length == 1)
                {
                    Targy mit = new Targy(eredmeny[0].Neve.ToString(), eredmeny[0].Torolve.ToString(), eredmeny[0].Kodja.ToString(),
                        eredmeny[0].Kreditje.ToString(), eredmeny[0].Ideje.ToString(), eredmeny[0].Oktatoja.ToString(), eredmeny[0].Feleve.ToString(),
                        eredmeny[0].Kovetelmenye.ToString(), eredmeny[0].Elokovetelmenye.ToString(), eredmeny[0].Idje.ToString());
                    server.Targy_Torles(mit);
                    MessageBox.Show("A tárgy sikeresen törölve.");
                    // ...
                }
                else
                {
                    MessageBox.Show("Nincs ilyen tárgy.");
                }

                Admin_Targykezeles_Betoltes();
            }
            catch
            { }
        }

        /// <summary>
        /// Edit subject.
        /// </summary>
        private void Admin_Targykezeles_Modositas()
        {
            try
            {
                string megy = "admin_targykezeles_modositas";
                string kod = box.ToString();
                Targy[] eredmeny = server.Targy_Lista(megy, kod);
                Targy mit = new Targy(eredmeny[0].Neve.ToString(), eredmeny[0].Torolve.ToString(), eredmeny[0].Kodja.ToString(),
                    eredmeny[0].Kreditje.ToString(), eredmeny[0].Ideje.ToString(), eredmeny[0].Oktatoja.ToString(), eredmeny[0].Feleve.ToString(),
                    eredmeny[0].Kovetelmenye.ToString(), eredmeny[0].Elokovetelmenye.ToString(), eredmeny[0].Idje.ToString());
                Targy mire = new Targy(eredmeny[0].Neve.ToString(), eredmeny[0].Torolve.ToString(), eredmeny[0].Kodja.ToString(),
                    eredmeny[0].Kreditje.ToString(), eredmeny[0].Ideje.ToString(), eredmeny[0].Oktatoja.ToString(), eredmeny[0].Feleve.ToString(),
                    eredmeny[0].Kovetelmenye.ToString(), eredmeny[0].Elokovetelmenye.ToString(), eredmeny[0].Idje.ToString());

                if (eredmeny.Length == 1)
                {
                    if (textBox14.Text.ToString() != eredmeny[0].Neve.ToString())
                    {
                        mire.Neve = textBox14.Text.ToString();
                    }
                    if (textBox15.Text.ToString() != eredmeny[0].Kodja.ToString())
                    {
                        string megy2 = "admin_targykezeles_modositas_ell";
                        string kod2 = textBox15.Text.ToString();
                        Targy[] van = server.Targy_Lista(megy2, kod2);

                        if (van.Length == 0)
                        {
                            mire.Kodja = textBox15.Text.ToString();
                        }
                    }
                    if (textBox16.Text.ToString() != eredmeny[0].Kreditje.ToString())
                    {
                        mire.Kreditje = textBox16.Text.ToString();
                    }
                    if (textBox17.Text.ToString() != eredmeny[0].Ideje.ToString())
                    {
                        mire.Ideje = textBox17.Text.ToString();
                    }
                    if (textBox18.Text.ToString() != eredmeny[0].Elokovetelmenye.ToString())
                    {
                        mire.Elokovetelmenye = textBox18.Text.ToString();
                    }
                    if (textBox80.Text.ToString() != eredmeny[0].Oktatoja.ToString())
                    {
                        mire.Oktatoja = textBox80.Text.ToString();
                    }
                    if (textBox81.Text.ToString() != eredmeny[0].Feleve.ToString())
                    {
                        mire.Feleve = textBox81.Text.ToString();
                    }
                    if (textBox82.Text.ToString() != eredmeny[0].Kovetelmenye.ToString())
                    {
                        mire.Kovetelmenye = textBox82.Text.ToString();
                    }
                }

                server.Targy_Modosit(mit, mire);
                // ...
                Admin_Targykezeles_Betoltes();
            }
            catch
            { }
        }

        /// <summary>
        /// Add subject.
        /// </summary>
        private void Admin_Targykezeles_Hozzaadas()
        {
            try
            {
                string megy = "admin_targykezeles_hozzaadas";
                string kod = textBox61.Text.ToString();
                Targy[] eredmeny = server.Targy_Lista(megy, kod);

                if (eredmeny.Length == 0)
                {
                    string kov = "";
                    if (radioButton1.Checked == true)
                    {
                        kov = "évközi jegy";
                    }
                    else
                    {
                        kov = "vizsga";
                    }

                    Targy mit = new Targy(textBox60.Text.ToString(), "false", textBox61.Text.ToString(), textBox76.Text.ToString(),
                        textBox77.Text.ToString(), comboBox8.Text.ToString(), comboBox9.Text.ToString(), kov, textBox83.Text.ToString(), targy_kulcs.ToString());
                    targy_kulcs++;
                    server.Targy_Hozzaad(mit);
                    // ...
                }

                Admin_Targykezeles_Betoltes();
            }
            catch
            { }
        }

        /// <summary>
        /// Sets the button.
        /// </summary>
        private void Admin_Targykezeles_Gomb_Engedelyezes_Uj()
        {
            if (button36.Enabled == false)
            {
                button36.Enabled = true;
            }
        }

        /// <summary>
        /// Sets the button.
        /// </summary>
        private void Admin_Targykezeles_Nev()
        {
            if (textBox60.Text != "")
            {
                Admin_Targykezeles_Gomb_Engedelyezes_Uj();
            }
        }

        /// <summary>
        /// Sets the button.
        /// </summary>
        private void Admin_Targykezeles_Kod()
        {
            if (textBox61.Text != "")
            {
                Admin_Targykezeles_Gomb_Engedelyezes_Uj();
            }
        }

        /// <summary>
        /// Sets the button.
        /// </summary>
        private void Admin_Targykezeles_Kredit()
        {
            if (textBox76.Text != "")
            {
                Admin_Targykezeles_Gomb_Engedelyezes_Uj();
            }
        }

        /// <summary>
        /// Sets the button.
        /// </summary>
        private void Admin_Targykezeles_Ido()
        {
            if (textBox77.Text != "")
            {
                Admin_Targykezeles_Gomb_Engedelyezes_Uj();
            }
        }

        /// <summary>
        /// Sets the button.
        /// </summary>
        private void Admin_Targykezeles_Elokov()
        {
            if (textBox83.Text != "")
            {
                Admin_Targykezeles_Gomb_Engedelyezes_Uj();
            }
        }

        /// <summary>
        /// Sets the button.
        /// </summary>
        private void Admin_Targykezeles_Oktato()
        {
            if (comboBox8.Text != "")
            {
                Admin_Targykezeles_Gomb_Engedelyezes_Uj();
            }
        }

        /// <summary>
        /// Sets the button.
        /// </summary>
        private void Admin_Targykezeles_Felev()
        {
            if (comboBox9.Text != "")
            {
                Admin_Targykezeles_Gomb_Engedelyezes_Uj();
            }
        }

        #endregion Targykezeles

        #region Hallgatokezeles

        /// <summary>
        /// Handles the Layout event of the tabPage23 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="LayoutEventArgs"/> instance containing the event data.</param>
        private void tabPage23_Layout(object sender, LayoutEventArgs e)
        {
            Admin_Hallgatokezeles_Betoltes();
        }

        /// <summary>
        /// Handles the Click event of the button37 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void button37_Click(object sender, EventArgs e)
        {
            Admin_Hallgatokezeles_Hozzaadas();
        }

        /// <summary>
        /// Handles the Click event of the button38 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void button38_Click(object sender, EventArgs e)
        {
            Admin_Hallgatokezeles_Modositas();
        }

        /// <summary>
        /// Handles the CellClick event of the dataGridView16 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="DataGridViewCellEventArgs"/> instance containing the event data.</param>
        private void dataGridView16_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            Admin_Hallgatokezeles_Kijeloles();
        }

        /// <summary>
        /// Handles the Click event of the button45 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void button45_Click(object sender, EventArgs e)
        {
            Admin_Hallgatokezeles_Torles();
        }

        /// <summary>
        /// Handles the TextChanged event of the textBox67 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void textBox67_TextChanged(object sender, EventArgs e)
        {
            Admin_Hallgatokezeles_Nev();
        }

        /// <summary>
        /// Handles the TextChanged event of the textBox68 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void textBox68_TextChanged(object sender, EventArgs e)
        {
            Admin_Hallgatokezeles_Kod();
        }

        /// <summary>
        /// Handles the TextChanged event of the textBox69 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void textBox69_TextChanged(object sender, EventArgs e)
        {
            Admin_Hallgatokezeles_Jelszo();
        }

        /// <summary>
        /// Handles the TextChanged event of the textBox70 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void textBox70_TextChanged(object sender, EventArgs e)
        {
            Admin_Hallgatokezeles_Email();
        }

        /// <summary>
        /// Handles the TextChanged event of the textBox71 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void textBox71_TextChanged(object sender, EventArgs e)
        {
            Admin_Hallgatokezeles_Fizetes();
        }

        /// <summary>
        /// Handles the TextChanged event of the textBox72 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void textBox72_TextChanged(object sender, EventArgs e)
        {
            Admin_Hallgatokezeles_SzHely();
        }

        /// <summary>
        /// Handles the TextChanged event of the textBox73 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void textBox73_TextChanged(object sender, EventArgs e)
        {
            Admin_Hallgatokezeles_SzIdo();
        }

        /// <summary>
        /// Handles the TextChanged event of the textBox74 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void textBox74_TextChanged(object sender, EventArgs e)
        {
            Admin_Hallgatokezeles_Anya();
        }

        /// <summary>
        /// Handles the TextChanged event of the textBox75 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void textBox75_TextChanged(object sender, EventArgs e)
        {
            Admin_Hallgatokezeles_Bankszamla();
        }

        /// <summary>
        /// Handles the TextChanged event of the textBox84 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void textBox84_TextChanged(object sender, EventArgs e)
        {
            Admin_Hallgatokezeles_Statusz();
        }

        /// <summary>
        /// Handles the TextChanged event of the textBox78 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void textBox78_TextChanged(object sender, EventArgs e)
        {
            Admin_Hallgatokezeles_Lakcim();
        }

        /// <summary>
        /// Handles the TextChanged event of the textBox79 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void textBox79_TextChanged(object sender, EventArgs e)
        {
            Admin_Hallgatokezeles_Telefon();
        }

        /// <summary>
        /// Handles the TextChanged event of the comboBox20 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void comboBox20_TextChanged(object sender, EventArgs e)
        {
            Admin_Hallgatokezeles_Beiratkozas();
        }

        /// <summary>
        /// Handles the TextChanged event of the comboBox10 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void comboBox10_TextChanged(object sender, EventArgs e)
        {
            Admin_Hallgatokezeles_Nem();
        }

        /// <summary>
        /// Handles the TextChanged event of the comboBox11 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void comboBox11_TextChanged(object sender, EventArgs e)
        {
            Admin_Hallgatokezeles_Jogviszony();
        }

        /// <summary>
        /// Handles the TextChanged event of the comboBox14 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void comboBox14_TextChanged(object sender, EventArgs e)
        {
            Admin_Hallgatokezeles_Beosztas();
        }

        /// <summary>
        /// Loads the students.
        /// </summary>
        private void Admin_Hallgatokezeles_Betoltes()
        {
            dataGridView16.Rows.Clear();
            Admin_Hallgatokezeles_Urites();
            button37.Enabled = false;
            button38.Enabled = false;
            button45.Enabled = false;
            textBox19.Enabled = false;
            textBox20.Enabled = false;

            try
            {
                string megy = "admin_hallgatokezeles_betoltes";
                string kod = "hallgato";
                Felhasznalo[] eredmeny = server.Felhasznalo_Lista(megy, kod);

                for (int i = 0; i < eredmeny.Length; i++)
                {
                    string[] oszlop = new string[15];
                    oszlop[0] = eredmeny[i].Neve.ToString();
                    oszlop[1] = eredmeny[i].Kodja.ToString();
                    oszlop[2] = eredmeny[i].Jelszava.ToString();
                    oszlop[3] = eredmeny[i].Emailje.ToString();
                    oszlop[4] = eredmeny[i].Fizetese.ToString();
                    oszlop[5] = eredmeny[i].Szuletesi_helye.ToString();
                    oszlop[6] = eredmeny[i].Szuletesi_ideje.ToString();
                    oszlop[7] = eredmeny[i].Anyja_neve.ToString();
                    oszlop[8] = eredmeny[i].Bankszamla.ToString();
                    oszlop[9] = eredmeny[i].Statusza.ToString();
                    oszlop[10] = eredmeny[i].Beiratkozva.ToString();
                    oszlop[11] = eredmeny[i].Neme.ToString();
                    oszlop[12] = eredmeny[i].Lakcime.ToString();
                    oszlop[13] = eredmeny[i].Jogviszonya.ToString();
                    oszlop[14] = eredmeny[i].Telefonja.ToString();
                    dataGridView16.Rows.Add(oszlop);
                }
            }
            catch
            { }
        }

        /// <summary>
        /// Select a student.
        /// </summary>
        private void Admin_Hallgatokezeles_Kijeloles()
        {
            Admin_Hallgatokezeles_Urites();

            // Kijelölés esetén átadjuk az értékeket.
            if (dataGridView16.CurrentCellAddress.X >= 0)
            {
                try
                {
                    button38.Enabled = true;
                    button45.Enabled = true;

                    // A hallgató kraken kódjának megszerzése.
                    int kijeloltsorindex = dataGridView16.SelectedCells[0].RowIndex;
                    DataGridViewRow kijeloltsor = dataGridView16.Rows[kijeloltsorindex];
                    string a_hallgato_id = kijeloltsor.Cells["Column11"].Value.ToString();
                    box = a_hallgato_id;
                    string megy = "admin_hallgatokezeles_kijeloles";

                    // A hallgató megkeresése.
                    Felhasznalo[] eredmeny = server.Felhasznalo_Lista(megy, a_hallgato_id);

                    // A hallgató adatainak megjelenítése.
                    textBox19.Text = eredmeny[0].Neve.ToString();
                    textBox20.Text = eredmeny[0].Kodja.ToString();
                    textBox21.Text = eredmeny[0].Jelszava.ToString();
                    textBox22.Text = eredmeny[0].Emailje.ToString();
                    textBox23.Text = eredmeny[0].Fizetese.ToString();
                    textBox24.Text = eredmeny[0].Szuletesi_helye.ToString();
                    textBox25.Text = eredmeny[0].Szuletesi_ideje.ToString();
                    textBox26.Text = eredmeny[0].Anyja_neve.ToString();
                    textBox27.Text = eredmeny[0].Bankszamla.ToString();
                    textBox28.Text = eredmeny[0].Statusza.ToString();
                    textBox29.Text = eredmeny[0].Beiratkozva.ToString();
                    textBox30.Text = eredmeny[0].Lakcime.ToString();
                    textBox47.Text = eredmeny[0].Telefonja.ToString();
                    textBox64.Text = eredmeny[0].Neme.ToString();
                    textBox65.Text = eredmeny[0].Jogviszonya.ToString();
                    textBox66.Text = "hallgató";
                }
                catch
                { }
            }
        }

        /// <summary>
        /// Clears the texts.
        /// </summary>
        private void Admin_Hallgatokezeles_Urites()
        {
            textBox19.Text = "";
            textBox20.Text = "";
            textBox21.Text = "";
            textBox22.Text = "";
            textBox23.Text = "";
            textBox24.Text = "";
            textBox25.Text = "";
            textBox26.Text = "";
            textBox27.Text = "";
            textBox28.Text = "";
            textBox29.Text = "";
            textBox30.Text = "";
            textBox47.Text = "";
            textBox64.Text = "";
            textBox65.Text = "";
            textBox66.Text = "";
            textBox67.Text = "";
            textBox68.Text = "";
            textBox69.Text = "";
            textBox70.Text = "";
            textBox71.Text = "";
            textBox72.Text = "";
            textBox73.Text = "";
            textBox74.Text = "";
            textBox75.Text = "";
            textBox78.Text = "";
            textBox79.Text = "";
            textBox84.Text = "";
            comboBox10.Text = "";
            comboBox11.Text = "";
            comboBox14.Text = "";
            comboBox20.Text = "";
        }

        /// <summary>
        /// Delete a student.
        /// </summary>
        private void Admin_Hallgatokezeles_Torles()
        {
            try
            {
                string megy = "admin_hallgatokezeles_torles";
                string kod = box.ToString();
                Felhasznalo[] eredmeny = server.Felhasznalo_Lista(megy, kod);

                if (eredmeny.Length == 1)
                {
                    Felhasznalo mit = new Felhasznalo(eredmeny[0].Idje.ToString(), eredmeny[0].Torolve.ToString(), eredmeny[0].Kodja.ToString(),
                        eredmeny[0].Neve.ToString(), eredmeny[0].Jelszava.ToString(), eredmeny[0].Beosztasa.ToString(), eredmeny[0].Emailje.ToString(),
                        eredmeny[0].Szuletesi_helye.ToString(), eredmeny[0].Szuletesi_ideje.ToString(), eredmeny[0].Fizetese.ToString(),
                        eredmeny[0].Anyja_neve.ToString(), eredmeny[0].Telefonja.ToString(), eredmeny[0].Bankszamla.ToString(), eredmeny[0].Statusza.ToString(),
                        eredmeny[0].Beiratkozva.ToString(), eredmeny[0].Neme.ToString(), eredmeny[0].Jogviszonya.ToString(), eredmeny[0].Lakcime.ToString());
                    server.Felhasznalo_Torles(mit);
                    // ...
                    MessageBox.Show("A hallgató sikeresen törölve.");
                }
                else
                {
                    MessageBox.Show("Nincs ilyen hallgató.");
                }
                Admin_Hallgatokezeles_Betoltes();
            }
            catch
            { }
        }

        /// <summary>
        /// Edit a student.
        /// </summary>
        private void Admin_Hallgatokezeles_Modositas()
        {
            try
            {
                string megy = "admin_hallgatokezeles_modositas";
                string kod = box.ToString();
                Felhasznalo[] eredmeny = server.Felhasznalo_Lista(megy, kod);
                Felhasznalo mit = new Felhasznalo(eredmeny[0].Idje.ToString(), eredmeny[0].Torolve.ToString(), eredmeny[0].Kodja.ToString(),
                    eredmeny[0].Neve.ToString(), eredmeny[0].Jelszava.ToString(), eredmeny[0].Beosztasa.ToString(), eredmeny[0].Emailje.ToString(),
                    eredmeny[0].Szuletesi_helye.ToString(), eredmeny[0].Szuletesi_ideje.ToString(), eredmeny[0].Fizetese.ToString(), eredmeny[0].Anyja_neve.ToString(),
                    eredmeny[0].Telefonja.ToString(), eredmeny[0].Bankszamla.ToString(), eredmeny[0].Statusza.ToString(), eredmeny[0].Beiratkozva.ToString(),
                    eredmeny[0].Neme.ToString(), eredmeny[0].Jogviszonya.ToString(), eredmeny[0].Lakcime.ToString());
                Felhasznalo mire = new Felhasznalo(eredmeny[0].Idje.ToString(), eredmeny[0].Torolve.ToString(), eredmeny[0].Kodja.ToString(),
                    eredmeny[0].Neve.ToString(), eredmeny[0].Jelszava.ToString(), eredmeny[0].Beosztasa.ToString(), eredmeny[0].Emailje.ToString(),
                    eredmeny[0].Szuletesi_helye.ToString(), eredmeny[0].Szuletesi_ideje.ToString(), eredmeny[0].Fizetese.ToString(), eredmeny[0].Anyja_neve.ToString(),
                    eredmeny[0].Telefonja.ToString(), eredmeny[0].Bankszamla.ToString(), eredmeny[0].Statusza.ToString(), eredmeny[0].Beiratkozva.ToString(),
                    eredmeny[0].Neme.ToString(), eredmeny[0].Jogviszonya.ToString(), eredmeny[0].Lakcime.ToString());

                if (eredmeny.Length == 1)
                {
                    if (textBox19.Text.ToString() != mit.Neve.ToString())
                    {
                        mire.Neve = textBox19.Text.ToString();
                    }
                    if (textBox20.Text.ToString() != mit.Kodja.ToString())
                    {
                        string megy2 = "admin_hallgatokezeles_modositas_ell";
                        string kod2 = textBox20.Text.ToString();
                        Felhasznalo[] van = server.Felhasznalo_Lista(megy2, kod2);

                        if (van.Length == 0)
                        {
                            mire.Kodja = textBox20.Text.ToString();
                        }
                    }
                    if (textBox21.Text.ToString() != mit.Jelszava.ToString())
                    {
                        mire.Jelszava = textBox21.Text.ToString();
                    }
                    if (textBox22.Text.ToString() != mit.Emailje.ToString())
                    {
                        mire.Emailje = textBox22.Text.ToString();
                    }
                    if (textBox23.Text.ToString() != mit.Fizetese.ToString())
                    {
                        mire.Fizetese = textBox23.Text.ToString();
                    }
                    if (textBox24.Text.ToString() != mit.Szuletesi_helye.ToString())
                    {
                        mire.Szuletesi_helye = textBox24.Text.ToString();
                    }
                    if (textBox25.Text.ToString() != mit.Szuletesi_ideje.ToString())
                    {
                        mire.Szuletesi_ideje = textBox25.Text.ToString();
                    }
                    if (textBox26.Text.ToString() != mit.Anyja_neve.ToString())
                    {
                        mire.Anyja_neve = textBox26.Text.ToString();
                    }
                    if (textBox27.Text.ToString() != mit.Bankszamla.ToString())
                    {
                        mire.Bankszamla = textBox27.Text.ToString();
                    }
                    if (textBox28.Text.ToString() != mit.Statusza.ToString())
                    {
                        mire.Statusza = textBox28.Text.ToString();
                    }
                    if (textBox29.Text.ToString() != mit.Beiratkozva.ToString())
                    {
                        mire.Beiratkozva = textBox29.Text.ToString();
                    }
                    if (textBox30.Text.ToString() != mit.Lakcime.ToString())
                    {
                        mire.Lakcime = textBox30.Text.ToString();
                    }
                    if (textBox47.Text.ToString() != mit.Telefonja.ToString())
                    {
                        mire.Telefonja = textBox47.Text.ToString();
                    }
                    if (textBox64.Text.ToString() != mit.Neme.ToString())
                    {
                        mire.Neme = textBox64.Text.ToString();
                    }
                    if (textBox65.Text.ToString() != mit.Jogviszonya.ToString())
                    {
                        mire.Jogviszonya = textBox65.Text.ToString();
                    }
                    if (textBox66.Text.ToString() != mit.Beosztasa.ToString())
                    {
                        mire.Beosztasa = textBox66.Text.ToString();
                    }
                }

                server.Felhasznalo_Modosit(mit, mire);
                // ...
                Admin_Hallgatokezeles_Betoltes();
            }
            catch
            { }
        }

        /// <summary>
        /// Add a student.
        /// </summary>
        private void Admin_Hallgatokezeles_Hozzaadas()
        {
            try
            {
                string megy = "admin_hallgatokezeles_hozzaadas";
                string kod = textBox68.Text.ToString();
                Felhasznalo[] eredmeny = server.Felhasznalo_Lista(megy, kod);

                if (eredmeny.Length == 0)
                {
                    Felhasznalo mit = new Felhasznalo(felhasznalo_kulcs.ToString(), "false", textBox68.Text.ToString(), textBox67.Text.ToString(),
                        textBox69.Text.ToString(), comboBox14.Text.ToString(), textBox70.Text.ToString(), textBox72.Text.ToString(), textBox73.Text.ToString(),
                        textBox71.Text.ToString(), textBox74.Text.ToString(), textBox63.Text.ToString(), textBox75.Text.ToString(), textBox84.Text.ToString(),
                        comboBox20.Text.ToString(), comboBox10.Text.ToString(), comboBox11.Text.ToString(), textBox78.Text.ToString());
                    felhasznalo_kulcs++;
                    server.Felhasznalo_Hozzaad(mit);
                    // ...
                }

                Admin_Hallgatokezeles_Betoltes();
            }
            catch
            { }
        }

        /// <summary>
        /// Set the button.
        /// </summary>
        private void Admin_Hallgatokezeles_Gomb_Engedelyezes_Uj()
        {
            if (button37.Enabled == false)
            {
                button37.Enabled = true;
            }
        }

        /// <summary>
        /// Set the button.
        /// </summary>
        private void Admin_Hallgatokezeles_Nev()
        {
            if (textBox67.Text != "")
            {
                Admin_Hallgatokezeles_Gomb_Engedelyezes_Uj();
            }
        }

        /// <summary>
        /// Set the button.
        /// </summary>
        private void Admin_Hallgatokezeles_Kod()
        {
            if (textBox68.Text != "")
            {
                Admin_Hallgatokezeles_Gomb_Engedelyezes_Uj();
            }
        }

        /// <summary>
        /// Set the button.
        /// </summary>
        private void Admin_Hallgatokezeles_Jelszo()
        {
            if (textBox69.Text != "")
            {
                Admin_Hallgatokezeles_Gomb_Engedelyezes_Uj();
            }
        }

        /// <summary>
        /// Set the button.
        /// </summary>
        private void Admin_Hallgatokezeles_Email()
        {
            if (textBox70.Text != "")
            {
                Admin_Hallgatokezeles_Gomb_Engedelyezes_Uj();
            }
        }

        /// <summary>
        /// Set the button.
        /// </summary>
        private void Admin_Hallgatokezeles_Fizetes()
        {
            if (textBox71.Text != "")
            {
                Admin_Hallgatokezeles_Gomb_Engedelyezes_Uj();
            }
        }

        /// <summary>
        /// Set the button.
        /// </summary>
        private void Admin_Hallgatokezeles_SzHely()
        {
            if (textBox72.Text != "")
            {
                Admin_Hallgatokezeles_Gomb_Engedelyezes_Uj();
            }
        }

        /// <summary>
        /// Set the button.
        /// </summary>
        private void Admin_Hallgatokezeles_SzIdo()
        {
            if (textBox73.Text != "")
            {
                Admin_Hallgatokezeles_Gomb_Engedelyezes_Uj();
            }
        }

        /// <summary>
        /// Set the button.
        /// </summary>
        private void Admin_Hallgatokezeles_Anya()
        {
            if (textBox74.Text != "")
            {
                Admin_Hallgatokezeles_Gomb_Engedelyezes_Uj();
            }
        }

        /// <summary>
        /// Set the button.
        /// </summary>
        private void Admin_Hallgatokezeles_Bankszamla()
        {
            if (textBox75.Text != "")
            {
                Admin_Hallgatokezeles_Gomb_Engedelyezes_Uj();
            }
        }

        /// <summary>
        /// Set the button.
        /// </summary>
        private void Admin_Hallgatokezeles_Statusz()
        {
            if (textBox84.Text != "")
            {
                Admin_Hallgatokezeles_Gomb_Engedelyezes_Uj();
            }
        }

        /// <summary>
        /// Set the button.
        /// </summary>
        private void Admin_Hallgatokezeles_Lakcim()
        {
            if (textBox78.Text != "")
            {
                Admin_Hallgatokezeles_Gomb_Engedelyezes_Uj();
            }
        }

        /// <summary>
        /// Set the button.
        /// </summary>
        private void Admin_Hallgatokezeles_Telefon()
        {
            if (textBox79.Text != "")
            {
                Admin_Hallgatokezeles_Gomb_Engedelyezes_Uj();
            }
        }

        /// <summary>
        /// Set the button.
        /// </summary>
        private void Admin_Hallgatokezeles_Beiratkozas()
        {
            if (comboBox20.Text != "")
            {
                Admin_Hallgatokezeles_Gomb_Engedelyezes_Uj();
            }
        }

        /// <summary>
        /// Set the button.
        /// </summary>
        private void Admin_Hallgatokezeles_Nem()
        {
            if (comboBox10.Text != "")
            {
                Admin_Hallgatokezeles_Gomb_Engedelyezes_Uj();
            }
        }

        /// <summary>
        /// Set the button.
        /// </summary>
        private void Admin_Hallgatokezeles_Jogviszony()
        {
            if (comboBox11.Text != "")
            {
                Admin_Hallgatokezeles_Gomb_Engedelyezes_Uj();
            }
        }

        /// <summary>
        /// Set the button.
        /// </summary>
        private void Admin_Hallgatokezeles_Beosztas()
        {
            if (comboBox14.Text != "")
            {
                Admin_Hallgatokezeles_Gomb_Engedelyezes_Uj();
            }
        }

        #endregion Hallgatokezeles

        #region Oktatokezeles

        /// <summary>
        /// Handles the Layout event of the tabPage24 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="LayoutEventArgs"/> instance containing the event data.</param>
        private void tabPage24_Layout(object sender, LayoutEventArgs e)
        {
            Admin_Oktatokezeles_Betoltes();
        }

        /// <summary>
        /// Handles the Click event of the button39 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void button39_Click(object sender, EventArgs e)
        {
            Admin_Oktatokezeles_Modositas();
        }

        /// <summary>
        /// Handles the Click event of the button40 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void button40_Click(object sender, EventArgs e)
        {
            Admin_Oktatokezeles_Hozzaadas();
        }

        /// <summary>
        /// Handles the CellClick event of the dataGridView17 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="DataGridViewCellEventArgs"/> instance containing the event data.</param>
        private void dataGridView17_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            Admin_Oktatokezeles_Kijeloles();
        }

        /// <summary>
        /// Handles the Click event of the button46 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void button46_Click(object sender, EventArgs e)
        {
            Admin_Oktatokezeles_Torles();
        }

        /// <summary>
        /// Handles the TextChanged event of the textBox51 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void textBox51_TextChanged(object sender, EventArgs e)
        {
            Admin_Oktatokezeles_Nev();
        }

        /// <summary>
        /// Handles the TextChanged event of the textBox52 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void textBox52_TextChanged(object sender, EventArgs e)
        {
            Admin_Oktatokezeles_Kod();
        }

        /// <summary>
        /// Handles the TextChanged event of the textBox53 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void textBox53_TextChanged(object sender, EventArgs e)
        {
            Admin_Oktatokezeles_Jelszo();
        }

        /// <summary>
        /// Handles the TextChanged event of the textBox54 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void textBox54_TextChanged(object sender, EventArgs e)
        {
            Admin_Oktatokezeles_Email();
        }

        /// <summary>
        /// Handles the TextChanged event of the textBox55 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void textBox55_TextChanged(object sender, EventArgs e)
        {
            Admin_Oktatokezeles_Fizetes();
        }

        /// <summary>
        /// Handles the TextChanged event of the textBox56 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void textBox56_TextChanged(object sender, EventArgs e)
        {
            Admin_Oktatokezeles_SzHely();
        }

        /// <summary>
        /// Handles the TextChanged event of the textBox57 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void textBox57_TextChanged(object sender, EventArgs e)
        {
            Admin_Oktatokezeles_SzIdo();
        }

        /// <summary>
        /// Handles the TextChanged event of the textBox58 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void textBox58_TextChanged(object sender, EventArgs e)
        {
            Admin_Oktatokezeles_Anya();
        }

        /// <summary>
        /// Handles the TextChanged event of the textBox59 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void textBox59_TextChanged(object sender, EventArgs e)
        {
            Admin_Oktatokezeles_Bankszamla();
        }

        /// <summary>
        /// Handles the TextChanged event of the textBox85 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void textBox85_TextChanged(object sender, EventArgs e)
        {
            Admin_Oktatokezeles_Statusz();
        }

        /// <summary>
        /// Handles the TextChanged event of the textBox62 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void textBox62_TextChanged(object sender, EventArgs e)
        {
            Admin_Oktatokezeles_Lakcim();
        }

        /// <summary>
        /// Handles the TextChanged event of the textBox63 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void textBox63_TextChanged(object sender, EventArgs e)
        {
            Admin_Oktatokezeles_Telefon();
        }

        /// <summary>
        /// Handles the TextChanged event of the comboBox21 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void comboBox21_TextChanged(object sender, EventArgs e)
        {
            Admin_Oktatokezeles_Ido();
        }

        /// <summary>
        /// Handles the TextChanged event of the comboBox13 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void comboBox13_TextChanged(object sender, EventArgs e)
        {
            Admin_Oktatokezeles_Nem();
        }

        /// <summary>
        /// Handles the TextChanged event of the comboBox12 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void comboBox12_TextChanged(object sender, EventArgs e)
        {
            Admin_Oktatokezeles_Jogviszony();
        }

        /// <summary>
        /// Handles the TextChanged event of the comboBox15 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void comboBox15_TextChanged(object sender, EventArgs e)
        {
            Admin_Oktatokezeles_Beosztas();
        }

        /// <summary>
        /// Loads the teachers.
        /// </summary>
        private void Admin_Oktatokezeles_Betoltes()
        {
            dataGridView17.Rows.Clear();
            Admin_Oktatokezeles_Urites();
            button39.Enabled = false;
            button40.Enabled = false;
            button46.Enabled = false;
            textBox41.Enabled = false;
            textBox42.Enabled = false;

            try
            {
                string megy = "admin_oktatokezeles_betoltes";
                string kod = "oktato";
                Felhasznalo[] eredmeny = server.Felhasznalo_Lista(megy, kod);

                for (int i = 0; i < eredmeny.Length; i++)
                {
                    string[] oszlop = new string[15];
                    oszlop[0] = eredmeny[i].Neve.ToString();
                    oszlop[1] = eredmeny[i].Kodja.ToString();
                    oszlop[2] = eredmeny[i].Jelszava.ToString();
                    oszlop[3] = eredmeny[i].Emailje.ToString();
                    oszlop[4] = eredmeny[i].Fizetese.ToString();
                    oszlop[5] = eredmeny[i].Szuletesi_helye.ToString();
                    oszlop[6] = eredmeny[i].Szuletesi_ideje.ToString();
                    oszlop[7] = eredmeny[i].Anyja_neve.ToString();
                    oszlop[8] = eredmeny[i].Bankszamla.ToString();
                    oszlop[9] = eredmeny[i].Statusza.ToString();
                    oszlop[10] = eredmeny[i].Beiratkozva.ToString();
                    oszlop[11] = eredmeny[i].Neme.ToString();
                    oszlop[12] = eredmeny[i].Lakcime.ToString();
                    oszlop[13] = eredmeny[i].Jogviszonya.ToString();
                    oszlop[14] = eredmeny[i].Telefonja.ToString();
                    dataGridView17.Rows.Add(oszlop);
                }
            }
            catch
            { }
        }

        /// <summary>
        /// Delets the teacher.
        /// </summary>
        private void Admin_Oktatokezeles_Torles()
        {
            try
            {
                string megy = "admin_oktatokezeles_torles";
                string kod = box.ToString();
                Felhasznalo[] eredmeny = server.Felhasznalo_Lista(megy, kod);

                if (eredmeny.Length == 1)
                {
                    Felhasznalo mit = new Felhasznalo(eredmeny[0].Idje.ToString(), eredmeny[0].Torolve.ToString(), eredmeny[0].Kodja.ToString(),
                        eredmeny[0].Neve.ToString(), eredmeny[0].Jelszava.ToString(), eredmeny[0].Beosztasa.ToString(), eredmeny[0].Emailje.ToString(),
                        eredmeny[0].Szuletesi_helye.ToString(), eredmeny[0].Szuletesi_ideje.ToString(), eredmeny[0].Fizetese.ToString(), eredmeny[0].Anyja_neve.ToString(),
                        eredmeny[0].Telefonja.ToString(), eredmeny[0].Bankszamla.ToString(), eredmeny[0].Statusza.ToString(), eredmeny[0].Beiratkozva.ToString(),
                        eredmeny[0].Neme.ToString(), eredmeny[0].Jogviszonya.ToString(), eredmeny[0].Lakcime.ToString());
                    server.Felhasznalo_Torles(mit);
                    // ...
                    MessageBox.Show("Az oktató sikeresen törölve.");
                }
                else
                {
                    MessageBox.Show("Nincs ilyen oktató.");
                }

                Admin_Oktatokezeles_Betoltes();
            }
            catch
            { }
        }

        /// <summary>
        /// Add a teacher.
        /// </summary>
        private void Admin_Oktatokezeles_Hozzaadas()
        {
            try
            {
                string megy = "admin_oktatokezeles_hozzaadas";
                string kod = textBox52.Text.ToString();
                Felhasznalo[] eredmeny = server.Felhasznalo_Lista(megy, kod);

                if (eredmeny.Length == 0)
                {
                    Felhasznalo mit = new Felhasznalo(felhasznalo_kulcs.ToString(), "false", textBox52.Text.ToString(), textBox51.Text.ToString(),
                        textBox53.Text.ToString(), comboBox15.Text.ToString(), textBox54.Text.ToString(), textBox56.Text.ToString(), textBox57.Text.ToString(),
                        textBox55.Text.ToString(), textBox58.Text.ToString(), textBox63.Text.ToString(), textBox59.Text.ToString(), textBox85.Text.ToString(),
                        comboBox21.Text.ToString(), comboBox13.Text.ToString(), comboBox21.Text.ToString(), textBox62.Text.ToString());
                    felhasznalo_kulcs++;
                    server.Felhasznalo_Hozzaad(mit);
                    // ...
                }

                Admin_Oktatokezeles_Betoltes();
            }
            catch
            { }
        }

        /// <summary>
        /// Select a teacher.
        /// </summary>
        private void Admin_Oktatokezeles_Kijeloles()
        {
            Admin_Oktatokezeles_Urites();

            // Kijelölés esetén átadjuk az értékeket.
            if (dataGridView17.CurrentCellAddress.X >= 0)
            {
                try
                {
                    button46.Enabled = true;
                    button39.Enabled = true;
                    string megy = "admin_oktatokezeles_kijeloles";

                    // Oktató Kraken kódjának megszerzése.
                    int kijeloltsorindex = dataGridView17.SelectedCells[0].RowIndex;
                    DataGridViewRow kijeloltsor = dataGridView17.Rows[kijeloltsorindex];
                    string a_oktato_id = kijeloltsor.Cells["dataGridViewTextBoxColumn24"].Value.ToString();
                    box = a_oktato_id;

                    // Az oktató adatainak betöltése.
                    Felhasznalo[] eredmeny = server.Felhasznalo_Lista(megy, a_oktato_id);
                    textBox42.Text = eredmeny[0].Neve.ToString();
                    textBox41.Text = eredmeny[0].Kodja.ToString();
                    textBox40.Text = eredmeny[0].Jelszava.ToString();
                    textBox39.Text = eredmeny[0].Emailje.ToString();
                    textBox38.Text = eredmeny[0].Fizetese.ToString();
                    textBox37.Text = eredmeny[0].Szuletesi_helye.ToString();
                    textBox36.Text = eredmeny[0].Szuletesi_ideje.ToString();
                    textBox35.Text = eredmeny[0].Anyja_neve.ToString();
                    textBox34.Text = eredmeny[0].Bankszamla.ToString();
                    textBox33.Text = eredmeny[0].Statusza.ToString();
                    textBox32.Text = eredmeny[0].Beiratkozva.ToString();
                    textBox31.Text = eredmeny[0].Lakcime.ToString();
                    textBox46.Text = eredmeny[0].Telefonja.ToString();
                    textBox49.Text = eredmeny[0].Jogviszonya.ToString();
                    textBox48.Text = eredmeny[0].Neme.ToString();
                    textBox50.Text = "oktató";
                }
                catch
                { }
            }
        }

        /// <summary>
        /// Edit a teacher.
        /// </summary>
        private void Admin_Oktatokezeles_Modositas()
        {
            try
            {
                string megy = "admin_oktatokezeles_modositas";
                string kod = box.ToString();
                Felhasznalo[] eredmeny = server.Felhasznalo_Lista(megy, kod);
                Felhasznalo mit = new Felhasznalo(eredmeny[0].Idje.ToString(), eredmeny[0].Torolve.ToString(), eredmeny[0].Kodja.ToString(),
                    eredmeny[0].Neve.ToString(), eredmeny[0].Jelszava.ToString(), eredmeny[0].Beosztasa.ToString(), eredmeny[0].Emailje.ToString(),
                    eredmeny[0].Szuletesi_helye.ToString(), eredmeny[0].Szuletesi_ideje.ToString(), eredmeny[0].Fizetese.ToString(), eredmeny[0].Anyja_neve.ToString(),
                    eredmeny[0].Telefonja.ToString(), eredmeny[0].Bankszamla.ToString(), eredmeny[0].Statusza.ToString(), eredmeny[0].Beiratkozva.ToString(),
                    eredmeny[0].Neme.ToString(), eredmeny[0].Jogviszonya.ToString(), eredmeny[0].Lakcime.ToString());
                Felhasznalo mire = new Felhasznalo(eredmeny[0].Idje.ToString(), eredmeny[0].Torolve.ToString(), eredmeny[0].Kodja.ToString(),
                    eredmeny[0].Neve.ToString(), eredmeny[0].Jelszava.ToString(), eredmeny[0].Beosztasa.ToString(), eredmeny[0].Emailje.ToString(),
                    eredmeny[0].Szuletesi_helye.ToString(), eredmeny[0].Szuletesi_ideje.ToString(), eredmeny[0].Fizetese.ToString(), eredmeny[0].Anyja_neve.ToString(),
                    eredmeny[0].Telefonja.ToString(), eredmeny[0].Bankszamla.ToString(), eredmeny[0].Statusza.ToString(), eredmeny[0].Beiratkozva.ToString(),
                    eredmeny[0].Neme.ToString(), eredmeny[0].Jogviszonya.ToString(), eredmeny[0].Lakcime.ToString());

                if (eredmeny.Length == 1)
                {
                    if (textBox42.Text.ToString() != mit.Neve.ToString())
                    {
                        mire.Neve = textBox42.Text.ToString();
                    }
                    if (textBox41.Text.ToString() != mit.Neve.ToString())
                    {
                        string megy2 = "admin_oktatokezeles_modositas_ell";
                        string kod2 = textBox41.Text.ToString();
                        Felhasznalo[] van = server.Felhasznalo_Lista(megy2, kod2);
                        if (van.Length == 0)
                        {
                            mire.Neve = textBox41.Text.ToString();
                        }
                    }
                    if (textBox40.Text.ToString() != mit.Jelszava.ToString())
                    {
                        mire.Jelszava = textBox40.Text.ToString();
                    }
                    if (textBox39.Text.ToString() != mit.Emailje.ToString())
                    {
                        mire.Emailje = textBox39.Text.ToString();
                    }
                    if (textBox38.Text.ToString() != mit.Fizetese.ToString())
                    {
                        mire.Fizetese = textBox38.Text.ToString();
                    }
                    if (textBox37.Text.ToString() != mit.Szuletesi_helye.ToString())
                    {
                        mire.Szuletesi_helye = textBox37.Text.ToString();
                    }
                    if (textBox36.Text.ToString() != mit.Szuletesi_ideje.ToString())
                    {
                        mire.Szuletesi_ideje = textBox36.Text.ToString();
                    }
                    if (textBox35.Text.ToString() != mit.Anyja_neve.ToString())
                    {
                        mire.Anyja_neve = textBox35.Text.ToString();
                    }
                    if (textBox34.Text.ToString() != mit.Bankszamla.ToString())
                    {
                        mire.Bankszamla = textBox34.Text.ToString();
                    }
                    if (textBox33.Text.ToString() != mit.Statusza.ToString())
                    {
                        mire.Statusza = textBox33.Text.ToString();
                    }
                    if (textBox32.Text.ToString() != mit.Beiratkozva.ToString())
                    {
                        mire.Beiratkozva = textBox32.Text.ToString();
                    }
                    if (textBox31.Text.ToString() != mit.Lakcime.ToString())
                    {
                        mire.Lakcime = textBox31.Text.ToString();
                    }
                    if (textBox48.Text.ToString() != mit.Neme.ToString())
                    {
                        mire.Neme = textBox48.Text.ToString();
                    }
                    if (textBox49.Text.ToString() != mit.Jogviszonya.ToString())
                    {
                        mire.Jogviszonya = textBox49.Text.ToString();
                    }
                    if (textBox50.Text.ToString() != mit.Beosztasa.ToString())
                    {
                        mire.Beosztasa = textBox50.Text.ToString();
                    }

                    server.Felhasznalo_Modosit(mit, mire);
                }

                Admin_Oktatokezeles_Betoltes();
            }
            catch
            { }
        }

        /// <summary>
        /// Clear texts.
        /// </summary>
        private void Admin_Oktatokezeles_Urites()
        {
            textBox31.Text = "";
            textBox32.Text = "";
            textBox33.Text = "";
            textBox34.Text = "";
            textBox35.Text = "";
            textBox36.Text = "";
            textBox37.Text = "";
            textBox38.Text = "";
            textBox39.Text = "";
            textBox40.Text = "";
            textBox41.Text = "";
            textBox42.Text = "";
            textBox46.Text = "";
            textBox48.Text = "";
            textBox49.Text = "";
            textBox50.Text = "";
            textBox51.Text = "";
            textBox52.Text = "";
            textBox53.Text = "";
            textBox54.Text = "";
            textBox55.Text = "";
            textBox56.Text = "";
            textBox57.Text = "";
            textBox58.Text = "";
            textBox59.Text = "";
            textBox62.Text = "";
            textBox63.Text = "";
            textBox85.Text = "";
            comboBox12.Text = "";
            comboBox13.Text = "";
            comboBox15.Text = "";
            comboBox21.Text = "";
        }

        /// <summary>
        /// Set the button.
        /// </summary>
        private void Admin_Oktatokezeles_Gomb_Engedelyezes_Uj()
        {
            if (button40.Enabled == false)
            {
                button40.Enabled = true;
            }
        }

        /// <summary>
        /// Set the button.
        /// </summary>
        private void Admin_Oktatokezeles_Nev()
        {
            if (textBox51.Text != "")
            {
                Admin_Oktatokezeles_Gomb_Engedelyezes_Uj();
            }
        }

        /// <summary>
        /// Set the button.
        /// </summary>
        private void Admin_Oktatokezeles_Kod()
        {
            if (textBox52.Text != "")
            {
                Admin_Oktatokezeles_Gomb_Engedelyezes_Uj();
            }
        }

        /// <summary>
        /// Set the button.
        /// </summary>
        private void Admin_Oktatokezeles_Jelszo()
        {
            if (textBox53.Text != "")
            {
                Admin_Oktatokezeles_Gomb_Engedelyezes_Uj();
            }
        }

        /// <summary>
        /// Set the button.
        /// </summary>
        private void Admin_Oktatokezeles_Email()
        {
            if (textBox54.Text != "")
            {
                Admin_Oktatokezeles_Gomb_Engedelyezes_Uj();
            }
        }

        /// <summary>
        /// Set the button.
        /// </summary>
        private void Admin_Oktatokezeles_Fizetes()
        {
            if (textBox55.Text != "")
            {
                Admin_Oktatokezeles_Gomb_Engedelyezes_Uj();
            }
        }

        /// <summary>
        /// Set the button.
        /// </summary>
        private void Admin_Oktatokezeles_SzHely()
        {
            if (textBox56.Text != "")
            {
                Admin_Oktatokezeles_Gomb_Engedelyezes_Uj();
            }
        }

        /// <summary>
        /// Set the button.
        /// </summary>
        private void Admin_Oktatokezeles_SzIdo()
        {
            if (textBox57.Text != "")
            {
                Admin_Oktatokezeles_Gomb_Engedelyezes_Uj();
            }
        }

        /// <summary>
        /// Set the button.
        /// </summary>
        private void Admin_Oktatokezeles_Anya()
        {
            if (textBox58.Text != "")
            {
                Admin_Oktatokezeles_Gomb_Engedelyezes_Uj();
            }
        }

        /// <summary>
        /// Set the button.
        /// </summary>
        private void Admin_Oktatokezeles_Bankszamla()
        {
            if (textBox59.Text != "")
            {
                Admin_Oktatokezeles_Gomb_Engedelyezes_Uj();
            }
        }

        // Admin_Oktatókezelés státusz változása.
        private void Admin_Oktatokezeles_Statusz()
        {
            if (textBox85.Text != "")
            {
                Admin_Oktatokezeles_Gomb_Engedelyezes_Uj();
            }
        }

        /// <summary>
        /// Set the button.
        /// </summary>
        private void Admin_Oktatokezeles_Lakcim()
        {
            if (textBox62.Text != "")
            {
                Admin_Oktatokezeles_Gomb_Engedelyezes_Uj();
            }
        }

        /// <summary>
        /// Set the button.
        /// </summary>
        private void Admin_Oktatokezeles_Telefon()
        {
            if (textBox63.Text != "")
            {
                Admin_Oktatokezeles_Gomb_Engedelyezes_Uj();
            }
        }

        /// <summary>
        /// Set the button.
        /// </summary>
        private void Admin_Oktatokezeles_Ido()
        {
            if (comboBox21.Text != "")
            {
                Admin_Oktatokezeles_Gomb_Engedelyezes_Uj();
            }
        }

        /// <summary>
        /// Set the button.
        /// </summary>
        private void Admin_Oktatokezeles_Nem()
        {
            if (comboBox13.Text != "")
            {
                Admin_Oktatokezeles_Gomb_Engedelyezes_Uj();
            }
        }

        /// <summary>
        /// Set the button.
        /// </summary>
        private void Admin_Oktatokezeles_Jogviszony()
        {
            if (comboBox12.Text != "")
            {
                Admin_Oktatokezeles_Gomb_Engedelyezes_Uj();
            }
        }

        /// <summary>
        /// Set the button.
        /// </summary>
        private void Admin_Oktatokezeles_Beosztas()
        {
            if (comboBox15.Text != "")
            {
                Admin_Oktatokezeles_Gomb_Engedelyezes_Uj();
            }
        }

        #endregion Oktatokezeles

        #region Targyfelvetel

        /// <summary>
        /// Handles the Click event of the button47 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void button47_Click(object sender, EventArgs e)
        {
            Admin_Targyfelvetel_Hallgatok();
        }

        /// <summary>
        /// Handles the SelectedIndexChanged event of the listBox5 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void listBox5_SelectedIndexChanged(object sender, EventArgs e)
        {
            Admin_Targyfelvetel_Hallgato_Targyai();
        }

        /// <summary>
        /// Handles the SelectedIndexChanged event of the listBox6 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void listBox6_SelectedIndexChanged(object sender, EventArgs e)
        {
            Admin_Targyfelvetel_Gomb_Ellenorzes();
        }

        /// <summary>
        /// Handles the Click event of the button48 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void button48_Click(object sender, EventArgs e)
        {
            Admin_Targyfelvetel_Targy_Felvetele();
        }

        /// <summary>
        /// Handles the SelectedIndexChanged event of the listBox7 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void listBox7_SelectedIndexChanged(object sender, EventArgs e)
        {
            Admin_Targyfelvetel_Gomb_Ellenorzes();
        }

        /// <summary>
        /// Handles the Click event of the button49 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void button49_Click(object sender, EventArgs e)
        {
            Admin_Targyfelvetel_Targy_Leadasa();
        }

        /// <summary>
        /// Handles the Layout event of the tabPage12 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="LayoutEventArgs"/> instance containing the event data.</param>
        private void tabPage12_Layout(object sender, LayoutEventArgs e)
        {
            Admin_Targyfelvetel_Betoltes();
        }

        /// <summary>
        /// Presets for the page.
        /// </summary>
        private void Admin_Targyfelvetel_Betoltes()
        {
            listBox5.Items.Clear();
            listBox6.Items.Clear();
            listBox7.Items.Clear();
            button49.Enabled = false;
            button48.Enabled = false;
        }

        /// <summary>
        /// Loads the students.
        /// </summary>
        private void Admin_Targyfelvetel_Hallgatok()
        {
            if (comboBox18.Text.ToString() != "")
            {
                try
                {
                    listBox5.Items.Clear();
                    listBox6.Items.Clear();
                    listBox7.Items.Clear();
                    string megy = "admin_targyfelvetel_hallgatok";
                    string kod = "hallgato";
                    Felhasznalo[] eredmeny = server.Felhasznalo_Lista(megy, kod);

                    for (int i = 0; i < eredmeny.Length; i++)
                    {
                        listBox5.Items.Add(eredmeny[i].Kodja.ToString());
                    }
                }
                catch
                { }
            }
        }

        /// <summary>
        /// Loads the selected student's subjects.
        /// </summary>
        private void Admin_Targyfelvetel_Hallgato_Targyai()
        {
            try
            {
                string megy = "admin_targyfelvetel_hallgato_targyai";
                string kod = listBox5.SelectedItem.ToString();
                string felev = comboBox18.Text.ToString();
                Hallgato_index[] eredmeny = server.Hallgato_Index_Lista(megy, felev, kod);
                string megy2 = "admin_targyfelvetel_hallgato_targyai";
                string kod2 = comboBox8.Text.ToString();
                int k = comboBox18.Text.Length;

                if (kod[k - 1].ToString() == "1")
                {
                    kod2 = "2013/14/1";
                }
                else
                {
                    kod2 = "2013/14/2";
                }

                Targy[] eredmeny2 = server.Targy_Lista(megy2, kod2);

                for (int i = 0; i < eredmeny.Length; i++)
                {
                    listBox7.Items.Add(eredmeny[i].Neve);
                }

                for (int j = 0; j < eredmeny2.Length; j++)
                {
                    bool van = false;

                    for (int m = 0; m < eredmeny.Length; m++)
                    {
                        if (eredmeny[m].Kodja.ToString() == eredmeny2[j].Kodja.ToString())
                        {
                            van = true;
                        }
                    }
                    if (van == false)
                    {
                        listBox6.Items.Add(eredmeny2[j].Neve.ToString());
                    }
                }
            }
            catch
            { }
        }

        /// <summary>
        /// Delete subject from student index.
        /// </summary>
        private void Admin_Targyfelvetel_Targy_Leadasa()
        {
            try
            {
                string megy = "admin_targyfelvetel_leadas";
                string felev = aktualis_felev.ToString();
                string kod = listBox5.SelectedItem.ToString() + "*" + listBox6.SelectedItem.ToString();
                Hallgato_index[] eredmeny = server.Hallgato_Index_Lista(megy, felev, kod);
                Hallgato_index mit = new Hallgato_index(eredmeny[0].Neve.ToString(), eredmeny[0].Torolve.ToString(), eredmeny[0].Kodja.ToString(),
                    eredmeny[0].Kreditje.ToString(), eredmeny[0].Ideje.ToString(), eredmeny[0].Oktatoja.ToString(), eredmeny[0].Feleve.ToString(),
                    eredmeny[0].Kovetelmenye.ToString(), eredmeny[0].Elokovetelmenye.ToString(), eredmeny[0].Idje.ToString(), eredmeny[0].Krakenje.ToString(),
                    eredmeny[0].Jegye.ToString(), eredmeny[0].Alairasa.ToString());

                server.Hallgato_Index_Torles(mit);
            }
            catch
            { }

            try
            {
                // Üzenet.
                DateTime idoo = DateTime.Now;
                string formatum = "yyyy.MM.dd,HH:mm:ss";
                string text = "A(z) " + listBox6.SelectedItem.ToString() + " tárgy leadásra került, módosító: " + belepo.Neve.ToString();
                string subject = "Tárgy Leadás";
                Uzenet mit2 = new Uzenet(idoo.ToString(formatum), "false", belepo.Kodja.ToString(), listBox5.SelectedItem.ToString(), subject.ToString(),
                    text.ToString(), "true", "true", "false", "false", uzenet_kulcs.ToString());
                uzenet_kulcs++;

                server.Uzenet_Hozzaad(mit2);
            }
            catch
            { }
        }

        /// <summary>
        /// Adds a subject to the seleced student's index.
        /// </summary>
        private void Admin_Targyfelvetel_Targy_Felvetele()
        {
            try
            {
                string megy = "admin_targyfelvetel_felvetel";
                string kod = listBox7.SelectedItem.ToString();
                Targy[] eredmeny = server.Targy_Lista(megy, kod);
                Hallgato_index mit = new Hallgato_index(eredmeny[0].Neve.ToString(), eredmeny[0].Torolve.ToString(), eredmeny[0].Kodja.ToString(),
                    eredmeny[0].Kreditje.ToString(), eredmeny[0].Ideje.ToString(), eredmeny[0].Oktatoja.ToString(), aktualis_felev.ToString(), eredmeny[0].Kovetelmenye.ToString(),
                    eredmeny[0].Elokovetelmenye.ToString(), hallgato_kulcs.ToString(), listBox5.SelectedItem.ToString(), "", "");
                hallgato_kulcs++;

                server.Hallgato_Index_Hozzaad(mit);
            }
            catch
            { }

            try
            {
                // Üzenet.
                DateTime idoo = DateTime.Now;
                string formatum = "yyyy.MM.dd,HH:mm:ss";
                string text = "A(z) " + listBox7.SelectedItem.ToString() + " tárgy felvételre került, módosító: " + belepo.Neve.ToString();
                string subject = "Tárgy Felvétel";
                Uzenet mit2 = new Uzenet(idoo.ToString(formatum), "false", belepo.Kodja.ToString(), listBox5.SelectedItem.ToString(), subject.ToString(),
                    text.ToString(), "true", "true", "false", "false", uzenet_kulcs.ToString());
                uzenet_kulcs++;

                server.Uzenet_Hozzaad(mit2);
            }
            catch
            { }
        }

        /// <summary>
        /// Check the buttons.
        /// </summary>
        private void Admin_Targyfelvetel_Gomb_Ellenorzes()
        {
            if (comboBox18.Text.ToString() == aktualis_felev.ToString())
            {
                if (listBox6.Items.Count != 0)
                {
                    button48.Enabled = true;
                }
                if (listBox7.Items.Count != 0)
                {
                    button49.Enabled = true;
                }
            }
            else
            {
                button48.Enabled = false;
                button49.Enabled = false;
            }
        }

        #endregion Targyfelvetel

        #endregion AdminGUI

        #endregion Methods
    }
}
