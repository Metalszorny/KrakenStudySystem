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
using System.ServiceModel;

namespace Kraken
{
    /// <summary>
    /// Interaction logic for Form2.
    /// </summary>
    public partial class Form2 : Form
    {
        #region Fields

        internal Felhasznalo belepo;
        private bool kodtext_change = false;
        private bool jelszotext_change = false;

        // szerverhez csatlakozáshoz szükséges Objektumok.
        private ChannelFactory<IServerM> chanel = new ChannelFactory<IServerM>(new NetTcpBinding(), new EndpointAddress(new Uri("net.tcp://localhost/KrakenService")));
        private IServerM server = null;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Form2"/> class.
        /// </summary>
        public Form2()
        {
            InitializeComponent();
        }
		
		/// <summary>
        /// Destroys the instance of the <see cref="Form2"/> class.
        /// </summary>
        ~Form2()
        { }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Handles the Click event of the button2 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void button2_Click(object sender, EventArgs e)
        {
            // Close
            this.Close();
        }

        /// <summary>
        /// Login.
        /// </summary>
        private void Login()
        {
            // The texts must not be empty.
            if (!string.IsNullOrEmpty(textBox1.Text.ToString()) && !string.IsNullOrEmpty(textBox2.Text.ToString()))
            {
                try
                {
                    // Create a channel.
                    server = chanel.CreateChannel();

                    // Find the user.
                    this.belepo = server.Belep(textBox1.Text.ToString(), textBox2.Text.ToString());

                    if (this.belepo == null)
                    {
                        MessageBox.Show("Belépés sikertelen. \nFelhasználónév vagy jelszó nem megfelelő");
                    }

                    // Close the channel.
                    ((IClientChannel)server).Close();
                }
                catch (Exception ex)
                {
                    if (server != null)
                    {
                        // Abort the channel.
                        ((ICommunicationObject)server).Abort();
                    }

                    MessageBox.Show(ex.ToString());
                }

                // Close
                this.Close();
            }
        }

        /// <summary>
        /// Handles the Click event of the button1 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void button1_Click(object sender, EventArgs e)
        {
            // Login
            Login();
        }

        /// <summary>
        /// Handles the KeyPress event of the textBox2 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="KeyPressEventArgs"/> instance containing the event data.</param>
        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Enter key.
            if (e.KeyChar == (char)(13))
            {
                Login();
            }
        }

        /// <summary>
        /// Handles the TextChanged event of the textBox1 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            Kod_Valtozas();
        }

        /// <summary>
        /// Handles the TextChanged event of the textBox2 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            Jelszo_Valtozas();
        }

        /// <summary>
        /// Username text change.
        /// </summary>
        private void Kod_Valtozas()
        {
            if (textBox1.Text.ToString() != "")
            {
                kodtext_change = true;

                if ((kodtext_change == true) && (jelszotext_change == true))
                {
                    button1.Enabled = true;
                }
                else
                {
                    button1.Enabled = false;
                }
            }
            else
            {
                kodtext_change = false;
            }
        }

        /// <summary>
        /// Password text change.
        /// </summary>
        private void Jelszo_Valtozas()
        {
            if (textBox2.Text.ToString() != "")
            {
                jelszotext_change = true;

                if ((kodtext_change == true) && (jelszotext_change == true))
                {
                    button1.Enabled = true;
                }
                else
                {
                    button1.Enabled = false;
                }
            }
            else
            {
                jelszotext_change = false;
            }
        }

        /// <summary>
        /// Handles the Load event of the Form2 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void Form2_Load(object sender, EventArgs e)
        {
            try
            {
                // Create a channel.
                server = chanel.CreateChannel();

                if (server.Belep("teszt", "teszt") != null)
                {
                    label5.Text = "ONLINE"; label5.ForeColor = Color.Green;
                }

                // Close the channel.
                ((IClientChannel)server).Close();
            }
            catch
            {
                if (server != null)
                {
                    // Abort the channel.
                    ((ICommunicationObject)server).Abort();
                }

                label5.Text = "OFFLINE"; label5.ForeColor = Color.Red;
                button1.Enabled = false;
            }

            button1.Enabled = false;
        }

        #endregion Methods
    }
}
