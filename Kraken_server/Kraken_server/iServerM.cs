using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Threading.Tasks;

namespace Kraken_server
{
    /// <summary>
    /// Interaction logic for IServerM.
    /// </summary>
    [ServiceContract()]
    public interface IServerM
    {
        #region Methods

        /// <summary>
        /// Login.
        /// </summary>
        /// <param name="userName">The userName.</param>
        /// <param name="pass">The pass.</param>
        /// <returns>The user.</returns>
        [OperationContract()]
        Felhasznalo Belep(string userName, string pass);

        /// <summary>
        /// Get the XML keys.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns>The key.</returns>
        [OperationContract()]
        int Kulcs_Betoltes(string input);

        #region List

        /// <summary>
        /// List the users.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="kod">The kod.</param>
        /// <returns>The list of users.</returns>
        [OperationContract()]
        Felhasznalo[] Felhasznalo_Lista(string input, string kod);

        /// <summary>
        /// List the subjects.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="kod">The kod.</param>
        /// <returns>The list of subjects.</returns>
        [OperationContract()]
        Targy[] Targy_Lista(string input, string kod);

        /// <summary>
        /// List the messages.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="kuldo">The kuldo.</param>
        /// <param name="cimzett">The cimzett.</param>
        /// <param name="targy">The targy.</param>
        /// <param name="szoveg">The szoveg.</param>
        /// <param name="ido">The ido.</param>
        /// <returns>The list of messages.</returns>
        [OperationContract()]
        Uzenet[] Uzenet_Lista(string input, string kuldo, string cimzett, string targy, string szoveg, string ido);

        /// <summary>
        /// List the Teacher_Indexes.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="felev">The felev.</param>
        /// <param name="kod">The kod.</param>
        /// <returns>The list of teacher_indexes.</returns>
        [OperationContract()]
        Oktato_index[] Oktato_Index_Lista(string input, string felev, string kod);

        /// <summary>
        /// List the Student_Indexes.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="felev">The felev.</param>
        /// <param name="kod">The kod.</param>
        /// <returns>The list of student_indexes.</returns>
        [OperationContract()]
        Hallgato_index[] Hallgato_Index_Lista(string input, string felev, string kod);

        #endregion List

        #region Delete

        /// <summary>
        /// Deletes a message.
        /// </summary>
        /// <param name="input">The message to delete.</param>
        [OperationContract()]
        void Uzenet_Torles(Uzenet input);

        /// <summary>
        /// Deletes a student_index.
        /// </summary>
        /// <param name="input">The student_index to delete.</param>
        [OperationContract()]
        void Hallgato_Index_Torles(Hallgato_index input);

        /// <summary>
        /// Deletes a teacher_index.
        /// </summary>
        /// <param name="input">The teacher_index to delete.</param>
        [OperationContract()]
        void Oktato_Index_Torles(Oktato_index input);

        /// <summary>
        /// Deletes a user.
        /// </summary>
        /// <param name="input">The user to delete.</param>
        [OperationContract()]
        void Felhasznalo_Torles(Felhasznalo input);

        /// <summary>
        /// Deletes a subject.
        /// </summary>
        /// <param name="input">The subject to delete.</param>
        [OperationContract()]
        void Targy_Torles(Targy input);

        #endregion Delete

        #region Add

        /// <summary>
        /// Adds a message.
        /// </summary>
        /// <param name="input">The message to add.</param>
        [OperationContract()]
        void Uzenet_Hozzaad(Uzenet input);

        /// <summary>
        /// Adds a user.
        /// </summary>
        /// <param name="input">The user to add.</param>
        [OperationContract()]
        void Felhasznalo_Hozzaad(Felhasznalo input);

        /// <summary>
        /// Adds a subject.
        /// </summary>
        /// <param name="input">The subject to add.</param>
        [OperationContract()]
        void Targy_Hozzaad(Targy input);

        /// <summary>
        /// Adds a student_index.
        /// </summary>
        /// <param name="input">The student_index to add.</param>
        [OperationContract()]
        void Hallgato_Index_Hozzaad(Hallgato_index input);

        /// <summary>
        /// Adds a teacher_index.
        /// </summary>
        /// <param name="input">The teacher_index to add.</param>
        [OperationContract()]
        void Oktato_Index_Hozzaad(Oktato_index input);

        #endregion Add

        #region Edit

        /// <summary>
        /// Edits a message.
        /// </summary>
        /// <param name="mit">The old message.</param>
        /// <param name="mire">The new message.</param>
        [OperationContract()]
        void Uzenet_Modosit(Uzenet mit, Uzenet mire);

        /// <summary>
        /// Edits a subject.
        /// </summary>
        /// <param name="mit">The old subject.</param>
        /// <param name="mire">The new subject.</param>
        [OperationContract()]
        void Targy_Modosit(Targy mit, Targy mire);

        /// <summary>
        /// Edits a teacher_index.
        /// </summary>
        /// <param name="mit">The old teacher_index.</param>
        /// <param name="mire">The new teacher_index.</param>
        [OperationContract()]
        void Oktato_Index_Modosit(Oktato_index mit, Oktato_index mire);

        /// <summary>
        /// Edits a student_index.
        /// </summary>
        /// <param name="mit">The old student_index.</param>
        /// <param name="mire">The new student_index.</param>
        [OperationContract()]
        void Hallgato_Index_Modosit(Hallgato_index mit, Hallgato_index mire);

        /// <summary>
        /// Edits a user.
        /// </summary>
        /// <param name="mit">The old user.</param>
        /// <param name="mire">The new user.</param>
        [OperationContract()]
        void Felhasznalo_Modosit(Felhasznalo mit, Felhasznalo mire);

        #endregion Edit

        #endregion Methods
    }
}
