using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace Kraken_server
{
    /// <summary>
    /// Base class for Message.
    /// </summary>
    [DataContract(Name = "Uzenet", Namespace = "net.tcp://localhost:6666/KrakenService")]
    public class Uzenet
    {
        #region Fields

        // The time field of the Message class.
        string ideje;

        // The sender field of the Message class.
        string kuldoje;

        // The addressee field of the Message class.
        string cimzettje;

        // The subject field of the Message class.
        string targya;

        // The text field of the Message class.
        string szovege;

        // The isDeletedAtSender field of the Message class.
        string megvan_kuldonek;

        // The isDeletedAtAddressee field of the Message class.
        string megvan_cimzettnek;

        // The isDraft field of the Message class.
        string mentve_e;

        // The isRead field of the Message class.
        string olvasva_e;

        // The id field of the Message class.
        private string idje;

        // The isDeleted field of the Message class.
        private string torolve;

        #endregion Fields

        #region Properties

        /// <summary>
        /// Gets or sets the ideje.
        /// </summary>
        /// <value>
        /// The ideje.
        /// </value>
        [DataMember]
        public string Ideje
        {
            get { return ideje; }
            set { ideje = value; }
        }

        /// <summary>
        /// Gets or sets the kuldoje.
        /// </summary>
        /// <value>
        /// The kuldoje.
        /// </value>
        [DataMember]
        public string Kuldoje
        {
            get { return kuldoje; }
            set { kuldoje = value; }
        }

        /// <summary>
        /// Gets or sets the cimzettje.
        /// </summary>
        /// <value>
        /// The cimzettje.
        /// </value>
        [DataMember]
        public string Cimzettje
        {
            get { return cimzettje; }
            set { cimzettje = value; }
        }

        /// <summary>
        /// Gets or sets the targya.
        /// </summary>
        /// <value>
        /// The targya.
        /// </value>
        [DataMember]
        public string Targya
        {
            get { return targya; }
            set { targya = value; }
        }

        /// <summary>
        /// Gets or sets the szovege.
        /// </summary>
        /// <value>
        /// The szovege.
        /// </value>
        [DataMember]
        public string Szovege
        {
            get { return szovege; }
            set { szovege = value; }
        }

        /// <summary>
        /// Gets or sets the megvan_kuldonek.
        /// </summary>
        /// <value>
        /// The megvan_kuldonek.
        /// </value>
        [DataMember]
        public string Megvan_kuldonek
        {
            get { return megvan_kuldonek; }
            set { megvan_kuldonek = value; }
        }

        /// <summary>
        /// Gets or sets the megvan_cimzettnek.
        /// </summary>
        /// <value>
        /// The megvan_cimzettnek.
        /// </value>
        [DataMember]
        public string Megvan_cimzettnek
        {
            get { return megvan_cimzettnek; }
            set { megvan_cimzettnek = value; }
        }

        /// <summary>
        /// Gets or sets the mentve_e.
        /// </summary>
        /// <value>
        /// The mentve_e.
        /// </value>
        [DataMember]
        public string Mentve_e
        {
            get { return mentve_e; }
            set { mentve_e = value; }
        }

        /// <summary>
        /// Gets or sets the olvasva_e.
        /// </summary>
        /// <value>
        /// The olvasva_e.
        /// </value>
        [DataMember]
        public string Olvasva_e
        {
            get { return olvasva_e; }
            set { olvasva_e = value; }
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
        /// Initializes a new instance of the <see cref="Uzenet"/> class.
        /// </summary>
        /// <param name="ideje">The input value for the ideje field.</param>
        /// <param name="torolve">The input value for the torolve field.</param>
        /// <param name="kuldoje">The input value for the kuldoje field.</param>
        /// <param name="cimzettje">The input value for the cimzettje field.</param>
        /// <param name="targya">The input value for the targya field.</param>
        /// <param name="szovege">The input value for the szovege field.</param>
        /// <param name="megvan_kuldonek">The input value for the megvan_kuldonek field.</param>
        /// <param name="megvan_cimzettnek">The input value for the megvan_cimzettnek field.</param>
        /// <param name="mentve_e">The input value for the mentve_e field.</param>
        /// <param name="olvasva_e">The input value for the olvasva_e field.</param>
        /// <param name="idje">The input value for the idje field.</param>
        public Uzenet(string ideje, string torolve, string kuldoje, string cimzettje, string targya, string szovege, string megvan_kuldonek,
            string megvan_cimzettnek, string mentve_e, string olvasva_e, string idje)
        {
            this.ideje = ideje;
            this.kuldoje = kuldoje;
            this.cimzettje = cimzettje;
            this.targya = targya;
            this.szovege = szovege;
            this.megvan_cimzettnek = megvan_cimzettnek;
            this.megvan_kuldonek = megvan_kuldonek;
            this.mentve_e = mentve_e;
            this.olvasva_e = olvasva_e;
            this.idje = idje;
            this.torolve = torolve;
        }
		
		/// <summary>
        /// Destroys the instance of the <see cref="Uzenet"/> class.
        /// </summary>
        ~Uzenet()
        { }

        #endregion Constructors

        #region Methods

        #endregion Methods
    }
}
