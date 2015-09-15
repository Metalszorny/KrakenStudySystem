using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace Kraken_server
{
    /// <summary>
    /// Base class for User.
    /// </summary>
    [DataContract(Name = "Felhasznalo", Namespace = "net.tcp://localhost:6666/KrakenService")]
    public class Felhasznalo
    {
        #region Fields

        // The name field of the User class.
        private string neve;

        // The password field of the User class.
        private string jelszava;

        // The email field of the User class.
        private string emailje;

        // The birthPlace field of the User class.
        private string szuletesi_helye;

        // The code field of the User class.
        private string kodja;

        // The assignment field of the User class.
        private string beosztasa;

        // The pay field of the User class.
        private string fizetese;

        // The birthDate field of the User class.
        private string szuletesi_ideje;

        // The id field of the User class.
        private string idje;

        // The motherName field of the User class.
        private string anyja_neve;

        // The dateOfJoin field of the User class.
        private string beiratkozva;

        // The sex field of the User class.
        private string neme;

        // The status field of the User class.
        private string statusza;

        // The bankAccount field of the User class.
        private string bankszamla;

        // The relationship field of the User class.
        private string jogviszonya;

        // The address field of the User class.
        private string lakcime;

        // The phoneNumber field of the User class.
        private string telefonja;

        // The isDeleted field of the User class.
        private string torolve;

        #endregion Fields

        #region Properties

        /// <summary>
        /// Gets or sets the neve.
        /// </summary>
        /// <value>
        /// The neve.
        /// </value>
        [DataMember]
        public string Neve
        {
            get { return neve; }
            set { neve = value; }
        }

        /// <summary>
        /// Gets or sets the jelszava.
        /// </summary>
        /// <value>
        /// The jelszava.
        /// </value>
        [DataMember]
        public string Jelszava
        {
            get { return jelszava; }
            set { jelszava = value; }
        }

        /// <summary>
        /// Gets or sets the emailje.
        /// </summary>
        /// <value>
        /// The emailje.
        /// </value>
        [DataMember]
        public string Emailje
        {
            get { return emailje; }
            set { emailje = value; }
        }

        /// <summary>
        /// Gets or sets the szuletesi_helye.
        /// </summary>
        /// <value>
        /// The szuletesi_helye.
        /// </value>
        [DataMember]
        public string Szuletesi_helye
        {
            get { return szuletesi_helye; }
            set { szuletesi_helye = value; }
        }

        /// <summary>
        /// Gets or sets the kodja.
        /// </summary>
        /// <value>
        /// The kodja.
        /// </value>
        [DataMember]
        public string Kodja
        {
            get { return kodja; }
            set { kodja = value; }
        }

        /// <summary>
        /// Gets or sets the beosztasa.
        /// </summary>
        /// <value>
        /// The beosztasa.
        /// </value>
        [DataMember]
        public string Beosztasa
        {
            get { return beosztasa; }
            set { beosztasa = value; }
        }

        /// <summary>
        /// Gets or sets the fizetese.
        /// </summary>
        /// <value>
        /// The fizetese.
        /// </value>
        [DataMember]
        public string Fizetese
        {
            get { return fizetese; }
            set { fizetese = value; }
        }

        /// <summary>
        /// Gets or sets the szuletesi_ideje.
        /// </summary>
        /// <value>
        /// The szuletesi_ideje.
        /// </value>
        [DataMember]
        public string Szuletesi_ideje
        {
            get { return szuletesi_ideje; }
            set { szuletesi_ideje = value; }
        }

        /// <summary>
        /// Gets or sets the idje.
        /// </summary>
        /// <value>
        /// The idje.
        /// </value>
        [DataMember]
        public string Idje
        {
            get { return idje; }
            set { idje = value; }
        }

        /// <summary>
        /// Gets or sets the anyja_neve.
        /// </summary>
        /// <value>
        /// The anyja_neve.
        /// </value>
        [DataMember]
        public string Anyja_neve
        {
            get { return anyja_neve; }
            set { anyja_neve = value; }
        }

        /// <summary>
        /// Gets or sets the beiratkozva.
        /// </summary>
        /// <value>
        /// The beiratkozva.
        /// </value>
        [DataMember]
        public string Beiratkozva
        {
            get { return beiratkozva; }
            set { beiratkozva = value; }
        }

        /// <summary>
        /// Gets or sets the neme.
        /// </summary>
        /// <value>
        /// The neme.
        /// </value>
        [DataMember]
        public string Neme
        {
            get { return neme; }
            set { neme = value; }
        }

        /// <summary>
        /// Gets or sets the statusza.
        /// </summary>
        /// <value>
        /// The statusza.
        /// </value>
        [DataMember]
        public string Statusza
        {
            get { return statusza; }
            set { statusza = value; }
        }

        /// <summary>
        /// Gets or sets the bankszamla.
        /// </summary>
        /// <value>
        /// The bankszamla.
        /// </value>
        [DataMember]
        public string Bankszamla
        {
            get { return bankszamla; }
            set { bankszamla = value; }
        }

        /// <summary>
        /// Gets or sets the jogviszonya.
        /// </summary>
        /// <value>
        /// The jogviszonya.
        /// </value>
        [DataMember]
        public string Jogviszonya
        {
            get { return jogviszonya; }
            set { jogviszonya = value; }
        }

        /// <summary>
        /// Gets or sets the lakcime.
        /// </summary>
        /// <value>
        /// The lakcime.
        /// </value>
        [DataMember]
        public string Lakcime
        {
            get { return lakcime; }
            set { lakcime = value; }
        }

        /// <summary>
        /// Gets or sets the telefonja.
        /// </summary>
        /// <value>
        /// The telefonja.
        /// </value>
        [DataMember]
        public string Telefonja
        {
            get { return telefonja; }
            set { telefonja = value; }
        }

        /// <summary>
        /// Gets or sets the torolve.
        /// </summary>
        /// <value>
        /// The torolve.
        /// </value>
        [DataMember]
        public string Torolve
        {
            get { return torolve; }
            set { torolve = value; }
        }

        #endregion Properties

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Felhasznalo"/> class.
        /// </summary>
        /// <param name="idje">The input value of the ideje field.</param>
        /// <param name="torolve">The input value of the torolve field.</param>
        /// <param name="kodja">The input value of the kodja field.</param>
        /// <param name="neve">The input value of the neve field.</param>
        /// <param name="jelszava">The input value of the jelszava field.</param>
        /// <param name="beosztasa">The input value of the beosztasa field.</param>
        /// <param name="emailje">The input value of the emailje field.</param>
        /// <param name="szuletesi_helye">The input value of the szuletesi_helye field.</param>
        /// <param name="szuletesi_ideje">The input value of the szuletesi_ideje field.</param>
        /// <param name="fizetese">The input value of the fizetese field.</param>
        /// <param name="anyja_neve">The input value of the anyja_neve field.</param>
        /// <param name="telefonja">The input value of the telefonja field.</param>
        /// <param name="bankszamla">The input value of the bankszamla field.</param>
        /// <param name="statusza">The input value of the statusza field.</param>
        /// <param name="beiratkozva">The input value of the beiratkozva field.</param>
        /// <param name="neme">The input value of the neme field.</param>
        /// <param name="jogviszonya">The input value of the jogviszonya field.</param>
        /// <param name="lakcime">The input value of the lakcime field.</param>
        public Felhasznalo(string idje, string torolve, string kodja, string neve, string jelszava, string beosztasa, string emailje,
            string szuletesi_helye, string szuletesi_ideje, string fizetese, string anyja_neve, string telefonja, string bankszamla,
            string statusza, string beiratkozva, string neme, string jogviszonya, string lakcime)
        {
            this.neve = neve;
            this.idje = idje;
            this.jelszava = jelszava;
            this.szuletesi_helye = szuletesi_helye;
            this.emailje = emailje;
            this.kodja = kodja;
            this.beosztasa = beosztasa;
            this.fizetese = fizetese;
            this.szuletesi_ideje = szuletesi_ideje;
            this.anyja_neve = anyja_neve;
            this.beiratkozva = beiratkozva;
            this.neme = neme;
            this.statusza = statusza;
            this.bankszamla = bankszamla;
            this.jogviszonya = jogviszonya;
            this.lakcime = lakcime;
            this.telefonja = telefonja;
            this.torolve = torolve;
        }

        #endregion Constructors

        #region Methods

        #endregion Methods
    }
}