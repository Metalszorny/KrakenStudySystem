using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace Kraken
{
    /// <summary>
    /// Base class for Subject.
    /// </summary>
    [DataContract(Name = "Targy", Namespace = "net.tcp://localhost:6666/KrakenService")]
    public class Targy
    {
        #region Fields

        // The name field for the Subject class.
        private string neve;

        // The code field fot the Subject class.
        private string kodja;

        // The credit field fot the Subject class.
        private string kreditje;

        // The teacher field of the Subject class.
        private string oktatoja;

        // The time field of the Subject class.
        private string ideje;

        // The semester field of the Subject class.
        private string feleve;

        // The pre-requirement field of the Subject class.
        private string elokovetelmenye;

        // The requirement field of the Subject class.
        private string kovetelmenye;

        // The id field of the Subject class.
        private string idje;

        // The isDeleted field of the Subject class.
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
        /// Gets or sets the kreditje.
        /// </summary>
        /// <value>
        /// The kreditje.
        /// </value>
        [DataMember]
        public string Kreditje
        {
            get { return kreditje; }
            set { kreditje = value; }
        }

        /// <summary>
        /// Gets or sets the oktatoja.
        /// </summary>
        /// <value>
        /// The oktatoja.
        /// </value>
        [DataMember]
        public string Oktatoja
        {
            get { return oktatoja; }
            set { oktatoja = value; }
        }

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
        /// Gets or sets the feleve.
        /// </summary>
        /// <value>
        /// The feleve.
        /// </value>
        [DataMember]
        public string Feleve
        {
            get { return feleve; }
            set { feleve = value; }
        }

        /// <summary>
        /// Gets or sets the elokovetelmenye.
        /// </summary>
        /// <value>
        /// The elokovetelmenye.
        /// </value>
        [DataMember]
        public string Elokovetelmenye
        {
            get { return elokovetelmenye; }
            set { elokovetelmenye = value; }
        }

        /// <summary>
        /// Gets or sets the kovetelmenye.
        /// </summary>
        /// <value>
        /// The kovetelmenye.
        /// </value>
        [DataMember]
        public string Kovetelmenye
        {
            get { return kovetelmenye; }
            set { kovetelmenye = value; }
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
        /// Initializes a new instance of the <see cref="Targy"/> class.
        /// </summary>
        /// <param name="neve">The input value for the neve field.</param>
        /// <param name="torolve">The input value for the torolve field.</param>
        /// <param name="kodja">The input value for the kodja field.</param>
        /// <param name="kreditje">The input value for the kreditje field.</param>
        /// <param name="ideje">The input value for the ideje field.</param>
        /// <param name="oktatoja">The input value for the oktatoja field.</param>
        /// <param name="feleve">The input value for the feleve field.</param>
        /// <param name="kovetelmenye">The input value for the kovetelmenye field.</param>
        /// <param name="elokovetelmenye">The input value for the elokovetelmenye field.</param>
        /// <param name="idje">The input value for the idje field.</param>
        public Targy(string neve, string torolve, string kodja, string kreditje, string ideje, string oktatoja, string feleve,
            string kovetelmenye, string elokovetelmenye, string idje)
        {
            this.neve = neve;
            this.kodja = kodja;
            this.kreditje = kreditje;
            this.ideje = ideje;
            this.oktatoja = oktatoja;
            this.feleve = feleve;
            this.elokovetelmenye = elokovetelmenye;
            this.kovetelmenye = kovetelmenye;
            this.idje = idje;
            this.torolve = torolve;
        }

        #endregion Constructors

        #region Methods

        #endregion Methods
    }

    /// <summary>
    /// Base class for Teacher_Index.
    /// </summary>
    [DataContract(Name = "Oktato_index", Namespace = "net.tcp://localhost:6666/KrakenService")]
    public class Oktato_index : Targy
    {
        #region Fields

        // The userCode field of the Teacher_Index class.
        private string krakenje;

        #endregion Fields

        #region Properties

        /// <summary>
        /// Gets or sets the krakenje.
        /// </summary>
        /// <value>
        /// The krakenje.
        /// </value>
        [DataMember]
        public string Krakenje
        {
            get { return krakenje; }
            set { krakenje = value; }
        }

        #endregion Properties

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Oktato_index"/> class.
        /// </summary>
        /// <param name="neve">The input value for the neve field.</param>
        /// <param name="torolve">The input value for the torolve field.</param>
        /// <param name="kodja">The input value for the kodja field.</param>
        /// <param name="kreditje">The input value for the kreditje field.</param>
        /// <param name="ideje">The input value for the ideje field.</param>
        /// <param name="oktatoja">The input value for the oktatoja field.</param>
        /// <param name="feleve">The input value for the feleve field.</param>
        /// <param name="kovetelmenye">The input value for the kovetelmenye field.</param>
        /// <param name="elokovetelmenye">The input value for the elokovetelmenye field.</param>
        /// <param name="idje">The input value for the idje field.</param>>
        /// <param name="krakenje">The input value for the krakenje field.</param>
        public Oktato_index(string neve, string torolve, string kodja, string kreditje, string ideje, string oktatoja, string feleve,
            string kovetelmenye, string elokovetelmenye, string idje, string krakenje)
            : base(neve, torolve, kodja, kreditje, ideje, oktatoja, feleve, kovetelmenye, elokovetelmenye, idje)
        {
            this.krakenje = krakenje;
        }

        #endregion Constructors

        #region Methods

        #endregion Methods
    }

    /// <summary>
    /// Base class for Student_Index.
    /// </summary>
    [DataContract(Name = "Hallgato_index", Namespace = "net.tcp://localhost:6666/KrakenService")]
    public class Hallgato_index : Targy
    {
        #region Fields

        // The userCode field of the Student_Index class.
        private string krakenje;

        // The grade field of the Student_Index class.
        private string jegye;

        // The signature field of the Student_Index class.
        private string alairasa;

        #endregion Fields

        #region Properties

        /// <summary>
        /// Gets or sets the krakenje.
        /// </summary>
        /// <value>
        /// The krakenje.
        /// </value>
        [DataMember]
        public string Krakenje
        {
            get { return krakenje; }
            set { krakenje = value; }
        }

        /// <summary>
        /// Gets or sets the jegye.
        /// </summary>
        /// <value>
        /// The jegye.
        /// </value>
        [DataMember]
        public string Jegye
        {
            get { return jegye; }
            set { jegye = value; }
        }

        /// <summary>
        /// Gets or sets the alairasa.
        /// </summary>
        /// <value>
        /// The alairasa.
        /// </value>
        [DataMember]
        public string Alairasa
        {
            get { return alairasa; }
            set { alairasa = value; }
        }

        #endregion Properties

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Hallgato_index"/> class.
        /// </summary>
        /// <param name="neve">The input value for the neve field.</param>
        /// <param name="torolve">The input value for the torolve field.</param>
        /// <param name="kodja">The input value for the kodja field.</param>
        /// <param name="kreditje">The input value for the kreditje field.</param>
        /// <param name="ideje">The input value for the ideje field.</param>
        /// <param name="oktatoja">The input value for the oktatoja field.</param>
        /// <param name="feleve">The input value for the feleve field.</param>
        /// <param name="kovetelmenye">The input value for the kovetelmenye field.</param>
        /// <param name="elokovetelmenye">The input value for the elokovetelmenye field.</param>
        /// <param name="idje">The input value for the idje field.</param>
        /// <param name="krakenje">The input value for the krakenje field.</param>
        /// <param name="jegye">The input value for the jegye field.</param>
        /// <param name="alairasa">The input value for the alairasa field.</param>
        public Hallgato_index(string neve, string torolve, string kodja, string kreditje, string ideje, string oktatoja, string feleve,
            string kovetelmenye, string elokovetelmenye, string idje, string krakenje, string jegye, string alairasa)
            : base(neve, torolve, kodja, kreditje, ideje, oktatoja, feleve, kovetelmenye, elokovetelmenye, idje)
        {
            this.krakenje = krakenje;
            this.alairasa = alairasa;
            this.jegye = jegye;
        }

        #endregion Constructors

        #region Methods

        #endregion Methods
    }
}
