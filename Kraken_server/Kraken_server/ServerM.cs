using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.Xml.Linq;
using System.IO;

namespace Kraken_server
{
    /// <summary>
    /// Interaction logic for ServerM.
    /// </summary>
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Single, InstanceContextMode = InstanceContextMode.Single)]
    public class ServerM : IServerM
    {
        #region Fields

        // The key for subjects.
        int targy_kulcs = 0;

        // The key for the students.
        int hallgato_kulcs = 0;

        // The key for the teachers.
        int oktato_kulcs = 0;

        // The key for the users.
        int felhasznalo_kulcs = 0;

        // The key fot the messages.
        int uzenet_kulcs = 0;

        #endregion Fields

        #region Methods

        /// <summary>
        /// Reports the message.
        /// </summary>
        /// <param name="reportMessage">The reportMessage.</param>
        public void Report(string reportMessage)
        {
            Console.WriteLine("[" + DateTime.Now.Year + "." + DateTime.Now.Month + "." + DateTime.Now.Day + " "
                + DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second + "] " + reportMessage);
        }

        /// <summary>
        /// Login.
        /// </summary>
        /// <param name="userName">The userName.</param>
        /// <param name="pass">The pass.</param>
        /// <returns>The user.</returns>
        public Felhasznalo Belep(string userName, string pass)
        {
            // DEBUG tesztgomb
            if (pass == "teszt" && userName == "teszt")
            {

                Report(userName + " felhasználó sikeresen belépett.");
                return new Felhasznalo(null, null, null, "teszt", "teszt", null, null, null, null, null, null, null, null, null, null, null, null, null);
            }

            // Felhasználó keresése az XML-ben
            XDocument kereses = XDocument.Load("felhasznalok.xml");
            var felhasznalok = kereses.Descendants("felhasznalo");
            var keres = from x in felhasznalok
                        where (string)x.Element("kod") == userName.ToUpper().ToString() && (string)x.Element("jelszo") == pass.ToString()
                        select new { Id = x.Attribute("id").Value };

            // Ha van ilyen
            if (keres.Count() == 1)
            {
                // Adatok átadása 
                var felhasznalo = from x in kereses.Descendants("felhasznalo")
                                  where (string)x.Element("kod") == userName.ToUpper().ToString() && (string)x.Element("jelszo") == pass.ToString()
                                  let neve = (string)x.Element("nev")
                                  let idje = (string)x.Attribute("id")
                                  let bankszama = (string)x.Element("bankszamla")
                                  let jelszava = (string)x.Element("jelszo")
                                  let emailje = (string)x.Element("email")
                                  let telefonja = (string)x.Element("telefon")
                                  let anyja_neve = (string)x.Element("anya")
                                  let beiratkozva = (string)x.Element("beiratkozas")
                                  let szuletesi_helye = (string)x.Element("szuletes_helye")
                                  let neme = (string)x.Element("neme")
                                  let fizetese = (string)x.Element("fizetes")
                                  let statusza = (string)x.Element("statusz")
                                  let kodja = (string)x.Element("kod")
                                  let beosztasa = (string)x.Element("beosztas")
                                  let szuletesi_ideje = (string)x.Element("szuletes_ideje")
                                  let jogviszonya = (string)x.Element("jogviszony")
                                  let lakcime = (string)x.Element("cim")
                                  let torolve = (string)x.Element("torolt")
                                  select new Felhasznalo(idje, torolve, kodja, neve, jelszava, beosztasa, emailje, szuletesi_helye, szuletesi_ideje,
                                      fizetese, anyja_neve, telefonja, bankszama, statusza, beiratkozva, neme, jogviszonya, lakcime);
                Report(userName + " felhasználó sikeresen belépett.");

                return (Felhasznalo)felhasznalo.Single();
            }
            else
            {
                // Hiba a belépésnél
                Report("Sikertelen bejelentkezés: " + userName);

                return null;
            }
        }

        /// <summary>
        /// Get the XML keys.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns>The key.</returns>
        public int Kulcs_Betoltes(string input)
        {
            int vissza = 0;

            try
            {
                switch (input.ToLower().ToString())
                {
                    case "felhasználók":
                        if (File.Exists("felhasznalok.xml"))
                        {
                            XDocument doc1 = XDocument.Load("felhasznalok.xml");
                            var valami1 = doc1.Descendants("felhasznalo");

                            if (valami1.Count() != 0)
                            {
                                vissza = doc1.Descendants("felhasznalo").Max(x => (int)x.Attribute("id")) + 1;
                            }
                        }
                        else
                        {
                            XElement valami2 = new XElement("felhasznalo");
                            XDocument doc2 = new XDocument(valami2);
                            doc2.Save("felhasznalok.xml");
                        }
                        break;
                    case "tárgyak":
                        if (File.Exists("targyak.xml"))
                        {
                            XDocument doc3 = XDocument.Load("targyak.xml");
                            var valami3 = doc3.Descendants("targy");

                            if (valami3.Count() != 0)
                            {
                                vissza = doc3.Descendants("targy").Max(x => (int)x.Attribute("id")) + 1;
                            }
                        }
                        else
                        {
                            XElement valami4 = new XElement("targy");
                            XDocument doc4 = new XDocument(valami4);
                            doc4.Save("targyak.xml");
                        }
                        break;
                    case "üzenetek":
                        if (File.Exists("uzenetek.xml"))
                        {
                            XDocument doc5 = XDocument.Load("uzenetek.xml");
                            var valami5 = doc5.Descendants("uzenet");

                            if (valami5.Count() != 0)
                            {
                                vissza = doc5.Descendants("uzenet").Max(x => (int)x.Attribute("id")) + 1;
                            }
                        }
                        else
                        {
                            XElement valami6 = new XElement("uzenet");
                            XDocument doc6 = new XDocument(valami6);
                            doc6.Save("uzenetek.xml");
                        }
                        break;
                    case "hallgatók":
                        if (File.Exists("hallgatok.xml"))
                        {
                            XDocument doc7 = XDocument.Load("hallgatok.xml");
                            var valami7 = doc7.Descendants("targy");

                            if (valami7.Count() != 0)
                            {
                                vissza = doc7.Descendants("targy").Max(x => (int)x.Attribute("id")) + 1;
                            }
                        }
                        else
                        {
                            XElement valami8 = new XElement("targy");
                            XDocument doc8 = new XDocument(valami8);
                            doc8.Save("hallgatok.xml");
                        }
                        break;
                    case "oktatók":
                        if (File.Exists("oktatok.xml"))
                        {
                            XDocument doc9 = XDocument.Load("oktatok.xml");
                            var valami9 = doc9.Descendants("targy");

                            if (valami9.Count() != 0)
                            {
                                vissza = doc9.Descendants("targy").Max(x => (int)x.Attribute("id")) + 1;
                            }
                        }
                        else
                        {
                            XElement valami10 = new XElement("targy");
                            XDocument doc10 = new XDocument(valami10);
                            doc10.Save("oktatok.xml");
                        }
                        break;
                }
            }
            catch
            {

            }

            switch (input.ToString())
            {
                case "felhasználók":
                    felhasznalo_kulcs = vissza;
                    break;
                case "tárgyak":
                    targy_kulcs = vissza;
                    break;
                case "oktatók":
                    oktato_kulcs = vissza;
                    break;
                case "hallgatók":
                    hallgato_kulcs = vissza;
                    break;
                case "üzenetek":
                    uzenet_kulcs = vissza;
                    break;
            }

            return vissza;
        }

        #region List

        /// <summary>
        /// List the users.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="kod">The kod.</param>
        /// <returns>The list of users.</returns>
        public Felhasznalo[] Felhasznalo_Lista(string input, string kod)
        {
            int kulcs = 1;

            switch (input.ToString())
            {
                case "oktato_nevek":
                    XDocument keres1 = XDocument.Load("felhasznalok.xml");
                    var talalat1 = from x1 in keres1.Root.Descendants("felhasznalo")
                                   where (string)x1.Element("torolt") == "false" && (string)x1.Element("beosztas") == "oktato"
                                   select x1;

                    kulcs = talalat1.Count();
                    break;
                case "admin_hallgatokezeles_betoltes":
                    XDocument keres2 = XDocument.Load("felhasznalok.xml");
                    var talalat2 = from x2 in keres2.Root.Descendants("felhasznalo")
                                   where (string)x2.Element("torolt") == "false" && (string)x2.Element("beosztas") == kod.ToString()
                                   select x2;

                    kulcs = talalat2.Count();
                    break;
                case "admin_hallgatokezeles_kijeloles":
                    XDocument keres3 = XDocument.Load("felhasznalok.xml");
                    var talalat3 = from x3 in keres3.Root.Descendants("felhasznalo")
                                   where (string)x3.Element("torolt") == "false" && (string)x3.Element("kod") == kod.ToString() && (string)x3.Element("beosztas") == "hallgato"
                                   select x3;

                    kulcs = talalat3.Count();
                    break;
                case "admin_hallgatokezeles_torles":
                    XDocument keres4 = XDocument.Load("felhasznalok.xml");
                    var talalat4 = from x4 in keres4.Root.Descendants("felhasznalo")
                                   where (string)x4.Element("torolt") == "false" && (string)x4.Element("kod") == kod.ToString() && (string)x4.Element("beosztas") == "hallgato"
                                   select x4;

                    kulcs = talalat4.Count();
                    break;
                case "admin_hallgatokezeles_modositas":
                    XDocument keres5 = XDocument.Load("felhasznalok.xml");
                    var talalat5 = from x5 in keres5.Root.Descendants("felhasznalo")
                                   where (string)x5.Element("torolt") == "false" && (string)x5.Element("kod") == kod.ToString() && (string)x5.Element("beosztas") == "hallgato"
                                   select x5;

                    kulcs = talalat5.Count();
                    break;
                case "admin_hallgatokezeles_modositas_ell":
                    XDocument keres6 = XDocument.Load("felhasznalok.xml");
                    var talalat6 = from x6 in keres6.Root.Descendants("felhasznalo")
                                   where (string)x6.Element("torolt") == "false" && (string)x6.Element("kod") == kod.ToString() && (string)x6.Element("beosztas") == "hallgato"
                                   select x6;

                    kulcs = talalat6.Count();
                    break;
                case "admin_hallgatokezeles_hozzaadas":
                    XDocument keres7 = XDocument.Load("felhasznalok.xml");
                    var talalat7 = from x7 in keres7.Root.Descendants("felhasznalo")
                                   where (string)x7.Element("torolt") == "false" && (string)x7.Element("kod") == kod.ToString() && (string)x7.Element("beosztas") == "hallgato"
                                   select x7;

                    kulcs = talalat7.Count();
                    break;
                case "admin_oktatokezeles_betoltes":
                    XDocument keres8 = XDocument.Load("felhasznalok.xml");
                    var talalat8 = from x8 in keres8.Root.Descendants("felhasznalo")
                                   where (string)x8.Element("torolt") == "false" && (string)x8.Element("beosztas") == kod.ToString()
                                   select x8;

                    kulcs = talalat8.Count();
                    break;
                case "admin_oktatokezeles_torles":
                    XDocument keres9 = XDocument.Load("felhasznalok.xml");
                    var talalat9 = from x9 in keres9.Root.Descendants("felhasznalo")
                                   where (string)x9.Element("torolt") == "false" && (string)x9.Element("kod") == kod.ToString() && (string)x9.Element("beosztas") == "oktato"
                                   select x9;

                    kulcs = talalat9.Count();
                    break;
                case "admin_oktatokezeles_hozzaadas":
                    XDocument keres10 = XDocument.Load("felhasznalok.xml");
                    var talalat10 = from x10 in keres10.Root.Descendants("felhasznalo")
                                    where (string)x10.Element("torolt") == "false" && (string)x10.Element("kod") == kod.ToString() && (string)x10.Element("beosztas") == "oktato"
                                    select x10;

                    kulcs = talalat10.Count();
                    break;
                case "admin_oktatokezeles_kijeloles":
                    XDocument keres11 = XDocument.Load("felhasznalok.xml");
                    var talalat11 = from x11 in keres11.Root.Descendants("felhasznalo")
                                    where (string)x11.Element("torolt") == "false" && (string)x11.Element("kod") == kod.ToString() && (string)x11.Element("beosztas") == "oktato"
                                    select x11;
                    kulcs = talalat11.Count();
                    break;
                case "admin_oktatokezeles_modositas":
                    XDocument keres12 = XDocument.Load("felhasznalok.xml");
                    var talalat12 = from x12 in keres12.Root.Descendants("felhasznalo")
                                    where (string)x12.Element("torolt") == "false" && (string)x12.Element("kod") == kod.ToString() && (string)x12.Element("beosztas") == "oktato"
                                    select x12;

                    kulcs = talalat12.Count();
                    break;
                case "admin_oktatokezeles_modositas_ell":
                    XDocument keres13 = XDocument.Load("felhasznalok.xml");
                    var talalat13 = from x13 in keres13.Root.Descendants("felhasznalo")
                                    where (string)x13.Element("torolt") == "false" && (string)x13.Element("kod") == kod.ToString() && (string)x13.Element("beosztas") == "oktato"
                                    select x13;

                    kulcs = talalat13.Count();
                    break;
                case "admin_targyfelvetel_hallgatok":
                    XDocument keres14 = XDocument.Load("felhasznalok.xml");
                    var talalat14 = from x14 in keres14.Root.Descendants("felhasznalo")
                                    where (string)x14.Element("torolt") == "false" && (string)x14.Element("beosztas") == kod.ToString()
                                    select x14;

                    kulcs = talalat14.Count();
                    break;
            }

            bool kis = false;

            if (kulcs == 0)
            {
                kis = true;
                kulcs = felhasznalo_kulcs;
            }

            Felhasznalo[] vissza = new Felhasznalo[kulcs];

            if (kis == false)
            {
                int szamlalo = 0;

                switch (input.ToString())
                {
                    case "oktato_nevek":
                        XDocument keres1 = XDocument.Load("felhasznalok.xml");
                        var talalat1 = from x1 in keres1.Root.Descendants("felhasznalo")
                                       where (string)x1.Element("torolt") == "false" && (string)x1.Element("beosztas") == "oktato"
                                       select x1;

                        foreach (var q in talalat1)
                        {
                            vissza[szamlalo] = new Felhasznalo((string)q.Attribute("id"), (string)q.Element("torolt"), (string)q.Element("kod"), (string)q.Element("nev"), (string)q.Element("jelszo"), (string)q.Element("beosztas"), (string)q.Element("email"), (string)q.Element("szuletes_helye"), (string)q.Element("szuletes_ideje"), (string)q.Element("fizetes"), (string)q.Element("anya"), (string)q.Element("telefon"), (string)q.Element("bankszamla"), (string)q.Element("statusz"), (string)q.Element("beiratkozas"), (string)q.Element("neme"), (string)q.Element("jogviszony"), (string)q.Element("cim"));
                            szamlalo++;
                        }
                        break;
                    case "admin_hallgatokezeles_betoltes":
                        XDocument keres2 = XDocument.Load("felhasznalok.xml");
                        var talalat2 = from x2 in keres2.Root.Descendants("felhasznalo")
                                       where (string)x2.Element("torolt") == "false" && (string)x2.Element("beosztas") == kod.ToString()
                                       select x2;

                        foreach (var q in talalat2)
                        {
                            vissza[szamlalo] = new Felhasznalo((string)q.Attribute("id"), (string)q.Element("torolt"), (string)q.Element("kod"), (string)q.Element("nev"), (string)q.Element("jelszo"), (string)q.Element("beosztas"), (string)q.Element("email"), (string)q.Element("szuletes_helye"), (string)q.Element("szuletes_ideje"), (string)q.Element("fizetes"), (string)q.Element("anya"), (string)q.Element("telefon"), (string)q.Element("bankszamla"), (string)q.Element("statusz"), (string)q.Element("beiratkozas"), (string)q.Element("neme"), (string)q.Element("jogviszony"), (string)q.Element("cim"));
                            szamlalo++;
                        }
                        break;
                    case "admin_hallgatokezeles_kijeloles":
                        XDocument keres3 = XDocument.Load("felhasznalok.xml");
                        var talalat3 = from x3 in keres3.Root.Descendants("felhasznalo")
                                       where (string)x3.Element("torolt") == "false" && (string)x3.Element("kod") == kod.ToString() && (string)x3.Element("beosztas") == "hallgato"
                                       select x3;

                        foreach (var q in talalat3)
                        {
                            vissza[szamlalo] = new Felhasznalo((string)q.Attribute("id"), (string)q.Element("torolt"), (string)q.Element("kod"), (string)q.Element("nev"), (string)q.Element("jelszo"), (string)q.Element("beosztas"), (string)q.Element("email"), (string)q.Element("szuletes_helye"), (string)q.Element("szuletes_ideje"), (string)q.Element("fizetes"), (string)q.Element("anya"), (string)q.Element("telefon"), (string)q.Element("bankszamla"), (string)q.Element("statusz"), (string)q.Element("beiratkozas"), (string)q.Element("neme"), (string)q.Element("jogviszony"), (string)q.Element("cim"));
                            szamlalo++;
                        }
                        break;
                    case "admin_hallgatokezeles_torles":
                        XDocument keres4 = XDocument.Load("felhasznalok.xml");
                        var talalat4 = from x4 in keres4.Root.Descendants("felhasznalo")
                                       where (string)x4.Element("torolt") == "false" && (string)x4.Element("kod") == kod.ToString() && (string)x4.Element("beosztas") == "hallgato"
                                       select x4;

                        foreach (var q in talalat4)
                        {
                            vissza[szamlalo] = new Felhasznalo((string)q.Attribute("id"), (string)q.Element("torolt"), (string)q.Element("kod"), (string)q.Element("nev"), (string)q.Element("jelszo"), (string)q.Element("beosztas"), (string)q.Element("email"), (string)q.Element("szuletes_helye"), (string)q.Element("szuletes_ideje"), (string)q.Element("fizetes"), (string)q.Element("anya"), (string)q.Element("telefon"), (string)q.Element("bankszamla"), (string)q.Element("statusz"), (string)q.Element("beiratkozas"), (string)q.Element("neme"), (string)q.Element("jogviszony"), (string)q.Element("cim"));
                            szamlalo++;
                        }
                        break;
                    case "admin_hallgatokezeles_modositas":
                        XDocument keres5 = XDocument.Load("felhasznalok.xml");
                        var talalat5 = from x5 in keres5.Root.Descendants("felhasznalo")
                                       where (string)x5.Element("torolt") == "false" && (string)x5.Element("kod") == kod.ToString() && (string)x5.Element("beosztas") == "hallgato"
                                       select x5;

                        foreach (var q in talalat5)
                        {
                            vissza[szamlalo] = new Felhasznalo((string)q.Attribute("id"), (string)q.Element("torolt"), (string)q.Element("kod"), (string)q.Element("nev"), (string)q.Element("jelszo"), (string)q.Element("beosztas"), (string)q.Element("email"), (string)q.Element("szuletes_helye"), (string)q.Element("szuletes_ideje"), (string)q.Element("fizetes"), (string)q.Element("anya"), (string)q.Element("telefon"), (string)q.Element("bankszamla"), (string)q.Element("statusz"), (string)q.Element("beiratkozas"), (string)q.Element("neme"), (string)q.Element("jogviszony"), (string)q.Element("cim"));
                            szamlalo++;
                        }
                        break;
                    case "admin_hallgatokezeles_modositas_ell":
                        XDocument keres6 = XDocument.Load("felhasznalok.xml");
                        var talalat6 = from x6 in keres6.Root.Descendants("felhasznalo")
                                       where (string)x6.Element("torolt") == "false" && (string)x6.Element("kod") == kod.ToString() && (string)x6.Element("beosztas") == "hallgato"
                                       select x6;

                        foreach (var q in talalat6)
                        {
                            vissza[szamlalo] = new Felhasznalo((string)q.Attribute("id"), (string)q.Element("torolt"), (string)q.Element("kod"), (string)q.Element("nev"), (string)q.Element("jelszo"), (string)q.Element("beosztas"), (string)q.Element("email"), (string)q.Element("szuletes_helye"), (string)q.Element("szuletes_ideje"), (string)q.Element("fizetes"), (string)q.Element("anya"), (string)q.Element("telefon"), (string)q.Element("bankszamla"), (string)q.Element("statusz"), (string)q.Element("beiratkozas"), (string)q.Element("neme"), (string)q.Element("jogviszony"), (string)q.Element("cim"));
                            szamlalo++;
                        }
                        break;
                    case "admin_hallgatokezeles_hozzaadas":
                        XDocument keres7 = XDocument.Load("felhasznalok.xml");
                        var talalat7 = from x7 in keres7.Root.Descendants("felhasznalo")
                                       where (string)x7.Element("torolt") == "false" && (string)x7.Element("kod") == kod.ToString() && (string)x7.Element("beosztas") == "hallgato"
                                       select x7;

                        foreach (var q in talalat7)
                        {
                            vissza[szamlalo] = new Felhasznalo((string)q.Attribute("id"), (string)q.Element("torolt"), (string)q.Element("kod"), (string)q.Element("nev"), (string)q.Element("jelszo"), (string)q.Element("beosztas"), (string)q.Element("email"), (string)q.Element("szuletes_helye"), (string)q.Element("szuletes_ideje"), (string)q.Element("fizetes"), (string)q.Element("anya"), (string)q.Element("telefon"), (string)q.Element("bankszamla"), (string)q.Element("statusz"), (string)q.Element("beiratkozas"), (string)q.Element("neme"), (string)q.Element("jogviszony"), (string)q.Element("cim"));
                            szamlalo++;
                        }
                        break;
                    case "admin_oktatokezeles_betoltes":
                        XDocument keres8 = XDocument.Load("felhasznalok.xml");
                        var talalat8 = from x8 in keres8.Root.Descendants("felhasznalo")
                                       where (string)x8.Element("torolt") == "false" && (string)x8.Element("beosztas") == kod.ToString()
                                       select x8;

                        foreach (var q in talalat8)
                        {
                            vissza[szamlalo] = new Felhasznalo((string)q.Attribute("id"), (string)q.Element("torolt"), (string)q.Element("kod"), (string)q.Element("nev"), (string)q.Element("jelszo"), (string)q.Element("beosztas"), (string)q.Element("email"), (string)q.Element("szuletes_helye"), (string)q.Element("szuletes_ideje"), (string)q.Element("fizetes"), (string)q.Element("anya"), (string)q.Element("telefon"), (string)q.Element("bankszamla"), (string)q.Element("statusz"), (string)q.Element("beiratkozas"), (string)q.Element("neme"), (string)q.Element("jogviszony"), (string)q.Element("cim"));
                            szamlalo++;
                        }
                        break;
                    case "admin_oktatokezeles_torles":
                        XDocument keres9 = XDocument.Load("felhasznalok.xml");
                        var talalat9 = from x9 in keres9.Root.Descendants("felhasznalo")
                                       where (string)x9.Element("torolt") == "false" && (string)x9.Element("kod") == kod.ToString() && (string)x9.Element("beosztas") == "oktato"
                                       select x9;

                        foreach (var q in talalat9)
                        {
                            vissza[szamlalo] = new Felhasznalo((string)q.Attribute("id"), (string)q.Element("torolt"), (string)q.Element("kod"), (string)q.Element("nev"), (string)q.Element("jelszo"), (string)q.Element("beosztas"), (string)q.Element("email"), (string)q.Element("szuletes_helye"), (string)q.Element("szuletes_ideje"), (string)q.Element("fizetes"), (string)q.Element("anya"), (string)q.Element("telefon"), (string)q.Element("bankszamla"), (string)q.Element("statusz"), (string)q.Element("beiratkozas"), (string)q.Element("neme"), (string)q.Element("jogviszony"), (string)q.Element("cim"));
                            szamlalo++;
                        }
                        break;
                    case "admin_oktatokezeles_hozzaadas":
                        XDocument keres10 = XDocument.Load("felhasznalok.xml");
                        var talalat10 = from x10 in keres10.Root.Descendants("felhasznalo")
                                        where (string)x10.Element("torolt") == "false" && (string)x10.Element("kod") == kod.ToString() && (string)x10.Element("beosztas") == "oktato"
                                        select x10;

                        foreach (var q in talalat10)
                        {
                            vissza[szamlalo] = new Felhasznalo((string)q.Attribute("id"), (string)q.Element("torolt"), (string)q.Element("kod"), (string)q.Element("nev"), (string)q.Element("jelszo"), (string)q.Element("beosztas"), (string)q.Element("email"), (string)q.Element("szuletes_helye"), (string)q.Element("szuletes_ideje"), (string)q.Element("fizetes"), (string)q.Element("anya"), (string)q.Element("telefon"), (string)q.Element("bankszamla"), (string)q.Element("statusz"), (string)q.Element("beiratkozas"), (string)q.Element("neme"), (string)q.Element("jogviszony"), (string)q.Element("cim"));
                            szamlalo++;
                        }
                        break;
                    case "admin_oktatokezeles_kijeloles":
                        XDocument keres11 = XDocument.Load("felhasznalok.xml");
                        var talalat11 = from x11 in keres11.Root.Descendants("felhasznalo")
                                        where (string)x11.Element("torolt") == "false" && (string)x11.Element("kod") == kod.ToString() && (string)x11.Element("beosztas") == "oktato"
                                        select x11;

                        foreach (var q in talalat11)
                        {
                            vissza[szamlalo] = new Felhasznalo((string)q.Attribute("id"), (string)q.Element("torolt"), (string)q.Element("kod"), (string)q.Element("nev"), (string)q.Element("jelszo"), (string)q.Element("beosztas"), (string)q.Element("email"), (string)q.Element("szuletes_helye"), (string)q.Element("szuletes_ideje"), (string)q.Element("fizetes"), (string)q.Element("anya"), (string)q.Element("telefon"), (string)q.Element("bankszamla"), (string)q.Element("statusz"), (string)q.Element("beiratkozas"), (string)q.Element("neme"), (string)q.Element("jogviszony"), (string)q.Element("cim"));
                            szamlalo++;
                        }
                        break;
                    case "admin_oktatokezeles_modositas":
                        XDocument keres12 = XDocument.Load("felhasznalok.xml");
                        var talalat12 = from x12 in keres12.Root.Descendants("felhasznalo")
                                        where (string)x12.Element("torolt") == "false" && (string)x12.Element("kod") == kod.ToString() && (string)x12.Element("beosztas") == "oktato"
                                        select x12;

                        foreach (var q in talalat12)
                        {
                            vissza[szamlalo] = new Felhasznalo((string)q.Attribute("id"), (string)q.Element("torolt"), (string)q.Element("kod"), (string)q.Element("nev"), (string)q.Element("jelszo"), (string)q.Element("beosztas"), (string)q.Element("email"), (string)q.Element("szuletes_helye"), (string)q.Element("szuletes_ideje"), (string)q.Element("fizetes"), (string)q.Element("anya"), (string)q.Element("telefon"), (string)q.Element("bankszamla"), (string)q.Element("statusz"), (string)q.Element("beiratkozas"), (string)q.Element("neme"), (string)q.Element("jogviszony"), (string)q.Element("cim"));
                            szamlalo++;
                        }
                        break;
                    case "admin_oktatokezeles_modositas_ell":
                        XDocument keres13 = XDocument.Load("felhasznalok.xml");
                        var talalat13 = from x13 in keres13.Root.Descendants("felhasznalo")
                                        where (string)x13.Element("torolt") == "false" && (string)x13.Element("kod") == kod.ToString() && (string)x13.Element("beosztas") == "oktato"
                                        select x13;

                        foreach (var q in talalat13)
                        {
                            vissza[szamlalo] = new Felhasznalo((string)q.Attribute("id"), (string)q.Element("torolt"), (string)q.Element("kod"), (string)q.Element("nev"), (string)q.Element("jelszo"), (string)q.Element("beosztas"), (string)q.Element("email"), (string)q.Element("szuletes_helye"), (string)q.Element("szuletes_ideje"), (string)q.Element("fizetes"), (string)q.Element("anya"), (string)q.Element("telefon"), (string)q.Element("bankszamla"), (string)q.Element("statusz"), (string)q.Element("beiratkozas"), (string)q.Element("neme"), (string)q.Element("jogviszony"), (string)q.Element("cim"));
                            szamlalo++;
                        }
                        break;
                    case "admin_targyfelvetel_hallgatok":
                        XDocument keres14 = XDocument.Load("felhasznalok.xml");
                        var talalat14 = from x14 in keres14.Root.Descendants("felhasznalo")
                                        where (string)x14.Element("torolt") == "false" && (string)x14.Element("beosztas") == kod.ToString()
                                        select x14;

                        foreach (var q in talalat14)
                        {
                            vissza[szamlalo] = new Felhasznalo((string)q.Attribute("id"), (string)q.Element("torolt"), (string)q.Element("kod"), (string)q.Element("nev"), (string)q.Element("jelszo"), (string)q.Element("beosztas"), (string)q.Element("email"), (string)q.Element("szuletes_helye"), (string)q.Element("szuletes_ideje"), (string)q.Element("fizetes"), (string)q.Element("anya"), (string)q.Element("telefon"), (string)q.Element("bankszamla"), (string)q.Element("statusz"), (string)q.Element("beiratkozas"), (string)q.Element("neme"), (string)q.Element("jogviszony"), (string)q.Element("cim"));
                            szamlalo++;
                        }
                        break;
                }
            }

            return vissza;
        }

        /// <summary>
        /// List the subjects.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="kod">The kod.</param>
        /// <returns>The list of subjects.</returns>
        public Targy[] Targy_Lista(string input, string kod)
        {
            int kulcs = 1;

            switch (input.ToString())
            {
                case "admin_ertekeles_targy_lista":
                    XDocument keres1 = XDocument.Load("targyak.xml");
                    var talalat1 = from x1 in keres1.Root.Descendants("targy")
                                   where (string)x1.Element("torolt") == "false" && (string)x1.Element("felev") == kod.ToString()
                                   select x1;

                    kulcs = talalat1.Count();
                    break;
                case "hallgato_targyfelvetel_lista":
                    XDocument keres2 = XDocument.Load("targyak.xml");
                    var talalat2 = from x2 in keres2.Root.Descendants("targy")
                                   where (string)x2.Element("torolt") == "false" && (string)x2.Element("felev") == kod.ToString()
                                   select x2;

                    kulcs = talalat2.Count();
                    break;
                case "hallgato_targyfelvetel_felvetel":
                    bool volt3 = false;
                    string hallgato3 = "";
                    string targy3 = "";

                    for (int i = 0; i < kod.Length; i++)
                    {
                        if (kod[i].ToString() != "*")
                        {
                            if (volt3 == false)
                            {
                                hallgato3 += kod[i].ToString();
                            }
                            else
                            {
                                targy3 += kod[i].ToString();
                            }
                        }
                        else
                        {
                            volt3 = true;
                        }
                    }
                    XDocument keres3 = XDocument.Load("targyak.xml");
                    var talalat3 = from x3 in keres3.Root.Descendants("targy")
                                   where (string)x3.Element("torolt") == "false" && (string)x3.Element("felev") == hallgato3.ToString() && (string)x3.Element("kod") == targy3.ToString()
                                   select x3;

                    kulcs = talalat3.Count();
                    break;
                case "admin_targykezeles_kijeloles":
                    XDocument keres4 = XDocument.Load("targyak.xml");
                    var talalat4 = from x4 in keres4.Root.Descendants("targy")
                                   where (string)x4.Element("torolt") == "false" && (string)x4.Element("kod") == kod.ToString()
                                   select x4;

                    kulcs = talalat4.Count();
                    break;
                case "admin_targykezeles_betoltes":
                    XDocument keres5 = XDocument.Load("targyak.xml");
                    var talalat5 = from x5 in keres5.Root.Descendants("targy")
                                   where (string)x5.Element("torolt") == "false"
                                   select x5;

                    kulcs = talalat5.Count();
                    break;
                case "admin_targykezeles_torles":
                    XDocument keres6 = XDocument.Load("targyak.xml");
                    var talalat6 = from x6 in keres6.Root.Descendants("targy")
                                   where (string)x6.Element("torolt") == "false" && (string)x6.Element("kod") == kod.ToString()
                                   select x6;

                    kulcs = talalat6.Count();
                    break;
                case "admin_targykezeles_modositas":
                    XDocument keres7 = XDocument.Load("targyak.xml");
                    var talalat7 = from x7 in keres7.Root.Descendants("targy")
                                   where (string)x7.Element("torolt") == "false" && (string)x7.Element("kod") == kod.ToString()
                                   select x7;

                    kulcs = talalat7.Count();
                    break;
                case "admin_targykezeles_modositas_ell":
                    XDocument keres8 = XDocument.Load("targyak.xml");
                    var talalat8 = from x8 in keres8.Root.Descendants("targy")
                                   where (string)x8.Element("torolt") == "false" && (string)x8.Element("kod") == kod.ToString()
                                   select x8;

                    kulcs = talalat8.Count();
                    break;
                case "admin_targykezeles_hozzaadas":
                    XDocument keres9 = XDocument.Load("targyak.xml");
                    var talalat9 = from x9 in keres9.Root.Descendants("targy")
                                   where (string)x9.Element("torolt") == "false" && (string)x9.Element("kod") == kod.ToString()
                                   select x9;

                    kulcs = talalat9.Count();
                    break;
                case "admin_targyfelvetel_hallgato_targyai":
                    XDocument keres10 = XDocument.Load("targyak.xml");
                    var talalat10 = from x10 in keres10.Root.Descendants("targy")
                                    where (string)x10.Element("torolt") == "false" && (string)x10.Element("felev") == kod.ToString()
                                    select x10;

                    kulcs = talalat10.Count();
                    break;
                case "admin_targyfelvetel_felvetel":
                    XDocument keres11 = XDocument.Load("targyak.xml");
                    var talalat11 = from x11 in keres11.Root.Descendants("targy")
                                    where (string)x11.Element("torolt") == "false" && (string)x11.Element("nev") == kod.ToString()
                                    select x11;

                    kulcs = talalat11.Count();
                    break;
            }

            bool kis = false;

            if (kulcs == 0)
            {
                kis = true;
                kulcs = targy_kulcs;
            }

            Targy[] valasz = new Targy[kulcs];

            if (kis == false)
            {
                int szamlalo = 0;

                switch (input.ToString())
                {
                    case "admin_ertekeles_targy_lista":
                        XDocument keres1 = XDocument.Load("targyak.xml");
                        var talalat1 = from x1 in keres1.Root.Descendants("targy")
                                       where (string)x1.Element("torolt") == "false" && (string)x1.Element("felev") == kod.ToString()
                                       select x1;

                        foreach (var q in talalat1)
                        {
                            valasz[szamlalo] = new Targy((string)q.Element("nev"), (string)q.Element("torolt"), (string)q.Element("kod"), (string)q.Element("kredit"), (string)q.Element("ido"), (string)q.Element("oktato"), (string)q.Element("felev"), (string)q.Element("kov"), (string)q.Element("elokov"), (string)q.Attribute("id"));
                            szamlalo++;
                        }
                        break;
                    case "hallgato_targyfelvetel_lista":
                        XDocument keres2 = XDocument.Load("targyak.xml");
                        var talalat2 = from x2 in keres2.Root.Descendants("targy")
                                       where (string)x2.Element("torolt") == "false" && (string)x2.Element("felev") == kod.ToString()
                                       select x2;

                        foreach (var q in talalat2)
                        {
                            valasz[szamlalo] = new Targy((string)q.Element("nev"), (string)q.Element("torolt"), (string)q.Element("kod"), (string)q.Element("kredit"), (string)q.Element("ido"), (string)q.Element("oktato"), (string)q.Element("felev"), (string)q.Element("kov"), (string)q.Element("elokov"), (string)q.Attribute("id"));
                            szamlalo++;
                        }
                        break;
                    case "hallgato_targyfelvetel_felvetel":
                        bool volt3 = false;
                        string hallgato3 = "";
                        string targy3 = "";

                        for (int i = 0; i < kod.Length; i++)
                        {
                            if (kod[i].ToString() != "*")
                            {
                                if (volt3 == false)
                                {
                                    hallgato3 += kod[i].ToString();
                                }
                                else
                                {
                                    targy3 += kod[i].ToString();
                                }
                            }
                            else
                            {
                                volt3 = true;
                            }
                        }
                        XDocument keres3 = XDocument.Load("targyak.xml");
                        var talalat3 = from x3 in keres3.Root.Descendants("targy")
                                       where (string)x3.Element("torolt") == "false" && (string)x3.Element("felev") == hallgato3.ToString() && (string)x3.Element("kod") == targy3.ToString()
                                       select x3;

                        foreach (var q in talalat3)
                        {
                            valasz[szamlalo] = new Targy((string)q.Element("nev"), (string)q.Element("torolt"), (string)q.Element("kod"), (string)q.Element("kredit"), (string)q.Element("ido"), (string)q.Element("oktato"), (string)q.Element("felev"), (string)q.Element("kov"), (string)q.Element("elokov"), (string)q.Attribute("id"));
                            szamlalo++;
                        }
                        break;
                    case "admin_targykezeles_kijeloles":
                        XDocument keres4 = XDocument.Load("targyak.xml");
                        var talalat4 = from x4 in keres4.Root.Descendants("targy")
                                       where (string)x4.Element("torolt") == "false" && (string)x4.Element("kod") == kod.ToString()
                                       select x4;

                        foreach (var q in talalat4)
                        {
                            valasz[szamlalo] = new Targy((string)q.Element("nev"), (string)q.Element("torolt"), (string)q.Element("kod"), (string)q.Element("kredit"), (string)q.Element("ido"), (string)q.Element("oktato"), (string)q.Element("felev"), (string)q.Element("kov"), (string)q.Element("elokov"), (string)q.Attribute("id"));
                            szamlalo++;
                        }
                        break;
                    case "admin_targykezeles_betoltes":
                        XDocument keres5 = XDocument.Load("targyak.xml");
                        var talalat5 = from x5 in keres5.Root.Descendants("targy")
                                       where (string)x5.Element("torolt") == "false"
                                       select x5;

                        foreach (var q in talalat5)
                        {
                            valasz[szamlalo] = new Targy((string)q.Element("nev"), (string)q.Element("torolt"), (string)q.Element("kod"), (string)q.Element("kredit"), (string)q.Element("ido"), (string)q.Element("oktato"), (string)q.Element("felev"), (string)q.Element("kov"), (string)q.Element("elokov"), (string)q.Attribute("id"));
                            szamlalo++;
                        }
                        break;
                    case "admin_targykezeles_torles":
                        XDocument keres6 = XDocument.Load("targyak.xml");
                        var talalat6 = from x6 in keres6.Root.Descendants("targy")
                                       where (string)x6.Element("torolt") == "false" && (string)x6.Element("kod") == kod.ToString()
                                       select x6;

                        foreach (var q in talalat6)
                        {
                            valasz[szamlalo] = new Targy((string)q.Element("nev"), (string)q.Element("torolt"), (string)q.Element("kod"), (string)q.Element("kredit"), (string)q.Element("ido"), (string)q.Element("oktato"), (string)q.Element("felev"), (string)q.Element("kov"), (string)q.Element("elokov"), (string)q.Attribute("id"));
                            szamlalo++;
                        }
                        break;
                    case "admin_targykezeles_modositas":
                        XDocument keres7 = XDocument.Load("targyak.xml");
                        var talalat7 = from x7 in keres7.Root.Descendants("targy")
                                       where (string)x7.Element("torolt") == "false" && (string)x7.Element("kod") == kod.ToString()
                                       select x7;

                        foreach (var q in talalat7)
                        {
                            valasz[szamlalo] = new Targy((string)q.Element("nev"), (string)q.Element("torolt"), (string)q.Element("kod"), (string)q.Element("kredit"), (string)q.Element("ido"), (string)q.Element("oktato"), (string)q.Element("felev"), (string)q.Element("kov"), (string)q.Element("elokov"), (string)q.Attribute("id"));
                            szamlalo++;
                        }
                        break;
                    case "admin_targykezeles_modositas_ell":
                        XDocument keres8 = XDocument.Load("targyak.xml");
                        var talalat8 = from x8 in keres8.Root.Descendants("targy")
                                       where (string)x8.Element("torolt") == "false" && (string)x8.Element("kod") == kod.ToString()
                                       select x8;

                        foreach (var q in talalat8)
                        {
                            valasz[szamlalo] = new Targy((string)q.Element("nev"), (string)q.Element("torolt"), (string)q.Element("kod"), (string)q.Element("kredit"), (string)q.Element("ido"), (string)q.Element("oktato"), (string)q.Element("felev"), (string)q.Element("kov"), (string)q.Element("elokov"), (string)q.Attribute("id"));
                            szamlalo++;
                        }
                        break;
                    case "admin_targykezeles_hozzaadas":
                        XDocument keres9 = XDocument.Load("targyak.xml");
                        var talalat9 = from x9 in keres9.Root.Descendants("targy")
                                       where (string)x9.Element("torolt") == "false" && (string)x9.Element("kod") == kod.ToString()
                                       select x9;

                        foreach (var q in talalat9)
                        {
                            valasz[szamlalo] = new Targy((string)q.Element("nev"), (string)q.Element("torolt"), (string)q.Element("kod"), (string)q.Element("kredit"), (string)q.Element("ido"), (string)q.Element("oktato"), (string)q.Element("felev"), (string)q.Element("kov"), (string)q.Element("elokov"), (string)q.Attribute("id"));
                            szamlalo++;
                        }
                        break;
                    case "admin_targyfelvetel_hallgato_targyai":
                        XDocument keres10 = XDocument.Load("targyak.xml");
                        var talalat10 = from x10 in keres10.Root.Descendants("targy")
                                        where (string)x10.Element("torolt") == "false" && (string)x10.Element("felev") == kod.ToString()
                                        select x10;

                        foreach (var q in talalat10)
                        {
                            valasz[szamlalo] = new Targy((string)q.Element("nev"), (string)q.Element("torolt"), (string)q.Element("kod"), (string)q.Element("kredit"), (string)q.Element("ido"), (string)q.Element("oktato"), (string)q.Element("felev"), (string)q.Element("kov"), (string)q.Element("elokov"), (string)q.Attribute("id"));
                            szamlalo++;
                        }
                        break;
                    case "admin_targyfelvetel_felvetel":
                        XDocument keres11 = XDocument.Load("targyak.xml");
                        var talalat11 = from x11 in keres11.Root.Descendants("targy")
                                        where (string)x11.Element("torolt") == "false" && (string)x11.Element("nev") == kod.ToString()
                                        select x11;

                        foreach (var q in talalat11)
                        {
                            valasz[szamlalo] = new Targy((string)q.Element("nev"), (string)q.Element("torolt"), (string)q.Element("kod"), (string)q.Element("kredit"), (string)q.Element("ido"), (string)q.Element("oktato"), (string)q.Element("felev"), (string)q.Element("kov"), (string)q.Element("elokov"), (string)q.Attribute("id"));
                            szamlalo++;
                        }
                        break;
                }
            }

            return valasz;
        }

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
        public Uzenet[] Uzenet_Lista(string input, string kuldo, string cimzett, string targy, string szoveg, string ido)
        {
            int kulcs = 1;

            switch (input.ToString())
            {
                case "uzenetek_bejovo_olvasatlan":
                    XDocument keres1 = XDocument.Load("uzenetek.xml");
                    var talalat1 = from x1 in keres1.Root.Descendants("uzenet")
                                   where (string)x1.Element("torolt") == "false" && (string)x1.Element("cimzett") == cimzett.ToString() && (string)x1.Element("megvan_cimzett") == "true" && (string)x1.Element("mentve") == "false" && (string)x1.Element("olvasva") == "false"
                                   select x1;

                    kulcs = talalat1.Count();
                    break;
                case "uzenetek_bejovo_lista":
                    XDocument keres2 = XDocument.Load("uzenetek.xml");
                    var talalat2 = from x2 in keres2.Root.Descendants("uzenet")
                                   where (string)x2.Element("torolt") == "false" && (string)x2.Element("cimzett") == cimzett.ToString() && (string)x2.Element("megvan_cimzett") == "true" && (string)x2.Element("mentve") == "false"
                                   select x2;

                    kulcs = talalat2.Count();
                    break;
                case "uzenetek_beerkezett_kijeloles":
                    XDocument keres3 = XDocument.Load("uzenetek.xml");
                    var talalat3 = from x3 in keres3.Root.Descendants("uzenet")
                                   where (string)x3.Element("torolt") == "false" && (string)x3.Element("kuldo") == kuldo.ToString() && (string)x3.Element("ido") == ido.ToString() && (string)x3.Element("cimzett") == cimzett.ToString() && (string)x3.Element("targy") == targy.ToString() && (string)x3.Element("szoveg") == szoveg.ToString() && (string)x3.Element("mentve") == "false" && (string)x3.Element("megvan_cimzett") == "true"
                                   select x3;

                    kulcs = talalat3.Count();
                    break;
                case "uzenetek_beerkezett_torles":
                    XDocument keres4 = XDocument.Load("uzenetek.xml");
                    var talalat4 = from x4 in keres4.Root.Descendants("uzenet")
                                   where (string)x4.Element("torolt") == "false" && (string)x4.Element("kuldo") == kuldo.ToString() && (string)x4.Element("ido") == ido.ToString() && (string)x4.Element("cimzett") == cimzett.ToString() && (string)x4.Element("targy") == targy.ToString() && (string)x4.Element("szoveg") == szoveg.ToString() && (string)x4.Element("mentve") == "false" && (string)x4.Element("megvan_cimzett") == "true"
                                   select x4;

                    kulcs = talalat4.Count();
                    break;
                case "uzenetek_elkuldott_lista":
                    XDocument keres5 = XDocument.Load("uzenetek.xml");
                    var talalat5 = from x5 in keres5.Root.Descendants("uzenet")
                                   where (string)x5.Element("torolt") == "false" && (string)x5.Element("kuldo") == kuldo.ToString() && (string)x5.Element("megvan_kuldo") == "true" && (string)x5.Element("mentve") == "false"
                                   select x5;

                    kulcs = talalat5.Count();
                    break;
                case "uzenetek_elkuldott_kijeloles":
                    XDocument keres6 = XDocument.Load("uzenetek.xml");
                    var talalat6 = from x6 in keres6.Root.Descendants("uzenet")
                                   where (string)x6.Element("torolt") == "false" && (string)x6.Element("kuldo") == kuldo.ToString() && (string)x6.Element("ido") == ido.ToString() && (string)x6.Element("cimzett") == cimzett.ToString() && (string)x6.Element("targy") == targy.ToString() && (string)x6.Element("szoveg") == szoveg.ToString() && (string)x6.Element("mentve") == "false" && (string)x6.Element("megvan_kuldo") == "true"
                                   select x6;

                    kulcs = talalat6.Count();
                    break;
                case "uzenetek_elkuldott_torles":
                    XDocument keres7 = XDocument.Load("uzenetek.xml");
                    var talalat7 = from x7 in keres7.Root.Descendants("uzenet")
                                   where (string)x7.Element("torolt") == "false" && (string)x7.Element("kuldo") == kuldo.ToString() && (string)x7.Element("ido") == ido.ToString() && (string)x7.Element("cimzett") == cimzett.ToString() && (string)x7.Element("targy") == targy.ToString() && (string)x7.Element("szoveg") == szoveg.ToString() && (string)x7.Element("mentve") == "false" && (string)x7.Element("megvan_kuldo") == "true"
                                   select x7;

                    kulcs = talalat7.Count();
                    break;
                case "uzenetek_piszkozat_lista":
                    XDocument keres8 = XDocument.Load("uzenetek.xml");
                    var talalat8 = from x8 in keres8.Root.Descendants("uzenet")
                                   where (string)x8.Element("torolt") == "false" && (string)x8.Element("kuldo") == kuldo.ToString() && (string)x8.Element("mentve") == "true"
                                   select x8;

                    kulcs = talalat8.Count();
                    break;
                case "uzenetek_piszkozat_kijeloles":
                    XDocument keres9 = XDocument.Load("uzenetek.xml");
                    var talalat9 = from x9 in keres9.Root.Descendants("uzenet")
                                   where (string)x9.Element("torolt") == "false" && (string)x9.Element("kuldo") == kuldo.ToString() && (string)x9.Element("ido") == ido.ToString() && (string)x9.Element("cimzett") == cimzett.ToString() && (string)x9.Element("targy") == targy.ToString() && (string)x9.Element("szoveg") == szoveg.ToString() && (string)x9.Element("mentve") == "true"
                                   select x9;

                    kulcs = talalat9.Count();
                    break;
                case "uzenetek_piszkozat_kuldes":
                    XDocument keres10 = XDocument.Load("uzenetek.xml");
                    var talalat10 = from x10 in keres10.Root.Descendants("uzenet")
                                    where (string)x10.Element("torolt") == "false" && (string)x10.Element("kuldo") == kuldo.ToString() && (string)x10.Element("ido") == ido.ToString() && (string)x10.Element("cimzett") == cimzett.ToString() && (string)x10.Element("targy") == targy.ToString() && (string)x10.Element("szoveg") == szoveg.ToString() && (string)x10.Element("mentve") == "true"
                                    select x10;

                    kulcs = talalat10.Count();
                    break;
                case "uzenetek_piszkozat_mentes":
                    XDocument keres11 = XDocument.Load("uzenetek.xml");
                    var talalat11 = from x11 in keres11.Root.Descendants("uzenet")
                                    where (string)x11.Element("torolt") == "false" && (string)x11.Element("kuldo") == kuldo.ToString() && (string)x11.Element("ido") == ido.ToString() && (string)x11.Element("cimzett") == cimzett.ToString() && (string)x11.Element("targy") == targy.ToString() && (string)x11.Element("szoveg") == szoveg.ToString() && (string)x11.Element("mentve") == "true"
                                    select x11;

                    kulcs = talalat11.Count();
                    break;
                case "uzenetek_piszkozat_torles":
                    XDocument keres12 = XDocument.Load("uzenetek.xml");
                    var talalat12 = from x12 in keres12.Root.Descendants("uzenet")
                                    where (string)x12.Element("torolt") == "false" && (string)x12.Element("kuldo") == kuldo.ToString() && (string)x12.Element("ido") == ido.ToString() && (string)x12.Element("cimzett") == cimzett.ToString() && (string)x12.Element("targy") == targy.ToString() && (string)x12.Element("szoveg") == szoveg.ToString() && (string)x12.Element("mentve") == "true"
                                    select x12;

                    kulcs = talalat12.Count();
                    break;
            }

            bool kis = false;

            if (kulcs == 0)
            {
                kis = true;
                kulcs = uzenet_kulcs;
            }

            Uzenet[] valasz = new Uzenet[kulcs];

            if (kis == false)
            {
                int szamlalo = 0;

                switch (input.ToString())
                {
                    case "uzenetek_bejovo_olvasatlan":
                        XDocument keres1 = XDocument.Load("uzenetek.xml");
                        var talalat1 = from x1 in keres1.Root.Descendants("uzenet")
                                       where (string)x1.Element("torolt") == "false" && (string)x1.Element("cimzett") == cimzett.ToString() && (string)x1.Element("megvan_cimzett") == "true" && (string)x1.Element("mentve") == "false" && (string)x1.Element("olvasva") == "false"
                                       select x1;

                        foreach (var q in talalat1)
                        {
                            valasz[szamlalo] = new Uzenet((string)q.Element("ido"), (string)q.Element("torolt"), (string)q.Element("kuldo"), (string)q.Element("cimzett"), (string)q.Element("targy"), (string)q.Element("szoveg"), (string)q.Element("megvan_kuldo"), (string)q.Element("megvan_cimzett"), (string)q.Element("mentve"), (string)q.Element("olvasva"), (string)q.Attribute("id"));
                            szamlalo++;
                        }
                        break;
                    case "uzenetek_bejovo_lista":
                        XDocument keres2 = XDocument.Load("uzenetek.xml");
                        var talalat2 = from x2 in keres2.Root.Descendants("uzenet")
                                       where (string)x2.Element("torolt") == "false" && (string)x2.Element("cimzett") == cimzett.ToString() && (string)x2.Element("megvan_cimzett") == "true" && (string)x2.Element("mentve") == "false"
                                       select x2;

                        foreach (var q in talalat2)
                        {
                            valasz[szamlalo] = new Uzenet((string)q.Element("ido"), (string)q.Element("torolt"), (string)q.Element("kuldo"), (string)q.Element("cimzett"), (string)q.Element("targy"), (string)q.Element("szoveg"), (string)q.Element("megvan_kuldo"), (string)q.Element("megvan_cimzett"), (string)q.Element("mentve"), (string)q.Element("olvasva"), (string)q.Attribute("id"));
                            szamlalo++;
                        }
                        break;
                    case "uzenetek_beerkezett_kijeloles":
                        XDocument keres3 = XDocument.Load("uzenetek.xml");
                        var talalat3 = from x3 in keres3.Root.Descendants("uzenet")
                                       where (string)x3.Element("torolt") == "false" && (string)x3.Element("kuldo") == kuldo.ToString() && (string)x3.Element("ido") == ido.ToString() && (string)x3.Element("cimzett") == cimzett.ToString() && (string)x3.Element("targy") == targy.ToString() && (string)x3.Element("szoveg") == szoveg.ToString() && (string)x3.Element("mentve") == "false" && (string)x3.Element("megvan_cimzett") == "true"
                                       select x3;

                        foreach (var q in talalat3)
                        {
                            valasz[szamlalo] = new Uzenet((string)q.Element("ido"), (string)q.Element("torolt"), (string)q.Element("kuldo"), (string)q.Element("cimzett"), (string)q.Element("targy"), (string)q.Element("szoveg"), (string)q.Element("megvan_kuldo"), (string)q.Element("megvan_cimzett"), (string)q.Element("mentve"), (string)q.Element("olvasva"), (string)q.Attribute("id"));
                            szamlalo++;
                        }
                        break;
                    case "uzenetek_beerkezett_torles":
                        XDocument keres4 = XDocument.Load("uzenetek.xml");
                        var talalat4 = from x4 in keres4.Root.Descendants("uzenet")
                                       where (string)x4.Element("torolt") == "false" && (string)x4.Element("kuldo") == kuldo.ToString() && (string)x4.Element("ido") == ido.ToString() && (string)x4.Element("cimzett") == cimzett.ToString() && (string)x4.Element("targy") == targy.ToString() && (string)x4.Element("szoveg") == szoveg.ToString() && (string)x4.Element("mentve") == "false" && (string)x4.Element("megvan_cimzett") == "true"
                                       select x4;

                        foreach (var q in talalat4)
                        {
                            valasz[szamlalo] = new Uzenet((string)q.Element("ido"), (string)q.Element("torolt"), (string)q.Element("kuldo"), (string)q.Element("cimzett"), (string)q.Element("targy"), (string)q.Element("szoveg"), (string)q.Element("megvan_kuldo"), (string)q.Element("megvan_cimzett"), (string)q.Element("mentve"), (string)q.Element("olvasva"), (string)q.Attribute("id"));
                            szamlalo++;
                        }
                        break;
                    case "uzenetek_elkuldott_lista":
                        XDocument keres5 = XDocument.Load("uzenetek.xml");
                        var talalat5 = from x5 in keres5.Root.Descendants("uzenet")
                                       where (string)x5.Element("torolt") == "false" && (string)x5.Element("kuldo") == kuldo.ToString() && (string)x5.Element("megvan_kuldo") == "true" && (string)x5.Element("mentve") == "false"
                                       select x5;

                        foreach (var q in talalat5)
                        {
                            valasz[szamlalo] = new Uzenet((string)q.Element("ido"), (string)q.Element("torolt"), (string)q.Element("kuldo"), (string)q.Element("cimzett"), (string)q.Element("targy"), (string)q.Element("szoveg"), (string)q.Element("megvan_kuldo"), (string)q.Element("megvan_cimzett"), (string)q.Element("mentve"), (string)q.Element("olvasva"), (string)q.Attribute("id"));
                            szamlalo++;
                        }
                        break;
                    case "uzenetek_elkuldott_kijeloles":
                        XDocument keres6 = XDocument.Load("uzenetek.xml");
                        var talalat6 = from x6 in keres6.Root.Descendants("uzenet")
                                       where (string)x6.Element("torolt") == "false" && (string)x6.Element("kuldo") == kuldo.ToString() && (string)x6.Element("ido") == ido.ToString() && (string)x6.Element("cimzett") == cimzett.ToString() && (string)x6.Element("targy") == targy.ToString() && (string)x6.Element("szoveg") == szoveg.ToString() && (string)x6.Element("mentve") == "false" && (string)x6.Element("megvan_kuldo") == "true"
                                       select x6;

                        foreach (var q in talalat6)
                        {
                            valasz[szamlalo] = new Uzenet((string)q.Element("ido"), (string)q.Element("torolt"), (string)q.Element("kuldo"), (string)q.Element("cimzett"), (string)q.Element("targy"), (string)q.Element("szoveg"), (string)q.Element("megvan_kuldo"), (string)q.Element("megvan_cimzett"), (string)q.Element("mentve"), (string)q.Element("olvasva"), (string)q.Attribute("id"));
                            szamlalo++;
                        }
                        break;
                    case "uzenetek_elkuldott_torles":
                        XDocument keres7 = XDocument.Load("uzenetek.xml");
                        var talalat7 = from x7 in keres7.Root.Descendants("uzenet")
                                       where (string)x7.Element("torolt") == "false" && (string)x7.Element("kuldo") == kuldo.ToString() && (string)x7.Element("ido") == ido.ToString() && (string)x7.Element("cimzett") == cimzett.ToString() && (string)x7.Element("targy") == targy.ToString() && (string)x7.Element("szoveg") == szoveg.ToString() && (string)x7.Element("mentve") == "false" && (string)x7.Element("megvan_kuldo") == "true"
                                       select x7;

                        foreach (var q in talalat7)
                        {
                            valasz[szamlalo] = new Uzenet((string)q.Element("ido"), (string)q.Element("torolt"), (string)q.Element("kuldo"), (string)q.Element("cimzett"), (string)q.Element("targy"), (string)q.Element("szoveg"), (string)q.Element("megvan_kuldo"), (string)q.Element("megvan_cimzett"), (string)q.Element("mentve"), (string)q.Element("olvasva"), (string)q.Attribute("id"));
                            szamlalo++;
                        }
                        break;
                    case "uzenetek_piszkozat_lista":
                        XDocument keres8 = XDocument.Load("uzenetek.xml");
                        var talalat8 = from x8 in keres8.Root.Descendants("uzenet")
                                       where (string)x8.Element("torolt") == "false" && (string)x8.Element("kuldo") == kuldo.ToString() && (string)x8.Element("mentve") == "true"
                                       select x8;

                        foreach (var q in talalat8)
                        {
                            valasz[szamlalo] = new Uzenet((string)q.Element("ido"), (string)q.Element("torolt"), (string)q.Element("kuldo"), (string)q.Element("cimzett"), (string)q.Element("targy"), (string)q.Element("szoveg"), (string)q.Element("megvan_kuldo"), (string)q.Element("megvan_cimzett"), (string)q.Element("mentve"), (string)q.Element("olvasva"), (string)q.Attribute("id"));
                            szamlalo++;
                        }
                        break;
                    case "uzenetek_piszkozat_kijeloles":
                        XDocument keres9 = XDocument.Load("uzenetek.xml");
                        var talalat9 = from x9 in keres9.Root.Descendants("uzenet")
                                       where (string)x9.Element("torolt") == "false" && (string)x9.Element("kuldo") == kuldo.ToString() && (string)x9.Element("ido") == ido.ToString() && (string)x9.Element("cimzett") == cimzett.ToString() && (string)x9.Element("targy") == targy.ToString() && (string)x9.Element("szoveg") == szoveg.ToString() && (string)x9.Element("mentve") == "true"
                                       select x9;

                        foreach (var q in talalat9)
                        {
                            valasz[szamlalo] = new Uzenet((string)q.Element("ido"), (string)q.Element("torolt"), (string)q.Element("kuldo"), (string)q.Element("cimzett"), (string)q.Element("targy"), (string)q.Element("szoveg"), (string)q.Element("megvan_kuldo"), (string)q.Element("megvan_cimzett"), (string)q.Element("mentve"), (string)q.Element("olvasva"), (string)q.Attribute("id"));
                            szamlalo++;
                        }
                        break;
                    case "uzenetek_piszkozat_kuldes":
                        XDocument keres10 = XDocument.Load("uzenetek.xml");
                        var talalat10 = from x10 in keres10.Root.Descendants("uzenet")
                                        where (string)x10.Element("torolt") == "false" && (string)x10.Element("kuldo") == kuldo.ToString() && (string)x10.Element("ido") == ido.ToString() && (string)x10.Element("cimzett") == cimzett.ToString() && (string)x10.Element("targy") == targy.ToString() && (string)x10.Element("szoveg") == szoveg.ToString() && (string)x10.Element("mentve") == "true"
                                        select x10;

                        foreach (var q in talalat10)
                        {
                            valasz[szamlalo] = new Uzenet((string)q.Element("ido"), (string)q.Element("torolt"), (string)q.Element("kuldo"), (string)q.Element("cimzett"), (string)q.Element("targy"), (string)q.Element("szoveg"), (string)q.Element("megvan_kuldo"), (string)q.Element("megvan_cimzett"), (string)q.Element("mentve"), (string)q.Element("olvasva"), (string)q.Attribute("id"));
                            szamlalo++;
                        }
                        break;
                    case "uzenetek_piszkozat_mentes":
                        XDocument keres11 = XDocument.Load("uzenetek.xml");
                        var talalat11 = from x11 in keres11.Root.Descendants("uzenet")
                                        where (string)x11.Element("torolt") == "false" && (string)x11.Element("kuldo") == kuldo.ToString() && (string)x11.Element("ido") == ido.ToString() && (string)x11.Element("cimzett") == cimzett.ToString() && (string)x11.Element("targy") == targy.ToString() && (string)x11.Element("szoveg") == szoveg.ToString() && (string)x11.Element("mentve") == "true"
                                        select x11;

                        foreach (var q in talalat11)
                        {
                            valasz[szamlalo] = new Uzenet((string)q.Element("ido"), (string)q.Element("torolt"), (string)q.Element("kuldo"), (string)q.Element("cimzett"), (string)q.Element("targy"), (string)q.Element("szoveg"), (string)q.Element("megvan_kuldo"), (string)q.Element("megvan_cimzett"), (string)q.Element("mentve"), (string)q.Element("olvasva"), (string)q.Attribute("id"));
                            szamlalo++;
                        }
                        break;
                    case "uzenetek_piszkozat_torles":
                        XDocument keres12 = XDocument.Load("uzenetek.xml");
                        var talalat12 = from x12 in keres12.Root.Descendants("uzenet")
                                        where (string)x12.Element("torolt") == "false" && (string)x12.Element("kuldo") == kuldo.ToString() && (string)x12.Element("ido") == ido.ToString() && (string)x12.Element("cimzett") == cimzett.ToString() && (string)x12.Element("targy") == targy.ToString() && (string)x12.Element("szoveg") == szoveg.ToString() && (string)x12.Element("mentve") == "true"
                                        select x12;

                        foreach (var q in talalat12)
                        {
                            valasz[szamlalo] = new Uzenet((string)q.Element("ido"), (string)q.Element("torolt"), (string)q.Element("kuldo"), (string)q.Element("cimzett"), (string)q.Element("targy"), (string)q.Element("szoveg"), (string)q.Element("megvan_kuldo"), (string)q.Element("megvan_cimzett"), (string)q.Element("mentve"), (string)q.Element("olvasva"), (string)q.Attribute("id"));
                            szamlalo++;
                        }
                        break;
                }
            }

            return valasz;
        }

        /// <summary>
        /// List the Teacher_Indexes.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="felev">The felev.</param>
        /// <param name="kod">The kod.</param>
        /// <returns>The list of teacher_indexes.</returns>
        public Oktato_index[] Oktato_Index_Lista(string input, string felev, string kod)
        {
            int kulcs = 1;

            switch (input.ToString())
            {
                case "oktato_ertekeles_targy_lista":
                    XDocument keres1 = XDocument.Load("oktatok.xml");
                    var talalat1 = from x1 in keres1.Root.Descendants("targy")
                                   where (string)x1.Element("torolt") == "false" && (string)x1.Element("felev") == felev.ToString() && (string)x1.Element("kraken") == kod.ToString()
                                   select x1;

                    kulcs = talalat1.Count();
                    break;
                case "oktato_oktatott_targyak":
                    XDocument keres2 = XDocument.Load("oktatok.xml");
                    var talalat2 = from x2 in keres2.Root.Descendants("targy")
                                   where (string)x2.Element("torolt") == "false" && (string)x2.Element("felev") == felev.ToString() && (string)x2.Element("kraken") == kod.ToString()
                                   select x2;

                    kulcs = talalat2.Count();
                    break;
            }

            bool kis = false;

            if (kulcs == 0)
            {
                kis = true;
                kulcs = oktato_kulcs;
            }

            Oktato_index[] valasz = new Oktato_index[kulcs];

            if (kis == false)
            {
                int szamlalo = 0;
                switch (input.ToString())
                {
                    case "oktato_ertekeles_targy_lista":
                        XDocument keres1 = XDocument.Load("oktatok.xml");
                        var talalat1 = from x1 in keres1.Root.Descendants("targy")
                                       where (string)x1.Element("torolt") == "false" && (string)x1.Element("felev") == felev.ToString() && (string)x1.Element("kraken") == kod.ToString()
                                       select x1;

                        foreach (var q in talalat1)
                        {
                            valasz[szamlalo] = new Oktato_index((string)q.Element("nev"), (string)q.Element("torolt"), (string)q.Element("kod"), (string)q.Element("kredit"), (string)q.Element("ido"), (string)q.Element("oktato"), (string)q.Element("felev"), (string)q.Element("kov"), (string)q.Element("elokov"), (string)q.Attribute("id"), (string)q.Element("kraken"));
                            szamlalo++;
                        }
                        break;
                    case "oktato_oktatott_targyak":
                        XDocument keres2 = XDocument.Load("oktatok.xml");
                        var talalat2 = from x2 in keres2.Root.Descendants("targy")
                                       where (string)x2.Element("torolt") == "false" && (string)x2.Element("felev") == felev.ToString() && (string)x2.Element("kraken") == kod.ToString()
                                       select x2;

                        foreach (var q in talalat2)
                        {
                            valasz[szamlalo] = new Oktato_index((string)q.Element("nev"), (string)q.Element("torolt"), (string)q.Element("kod"), (string)q.Element("kredit"), (string)q.Element("ido"), (string)q.Element("oktato"), (string)q.Element("felev"), (string)q.Element("kov"), (string)q.Element("elokov"), (string)q.Attribute("id"), (string)q.Element("kraken"));
                            szamlalo++;
                        }
                        break;
                }
            }

            return valasz;
        }

        /// <summary>
        /// List the Student_Indexes.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="felev">The felev.</param>
        /// <param name="kod">The kod.</param>
        /// <returns>The list of student_indexes.</returns>
        public Hallgato_index[] Hallgato_Index_Lista(string input, string felev, string kod)
        {
            int kulcs = 1;

            switch (input.ToString())
            {
                case "oktato_ertekeles_hallgato_lista":
                    XDocument keres1 = XDocument.Load("hallgatok.xml");
                    var talalat1 = from x1 in keres1.Root.Descendants("targy")
                                   where (string)x1.Element("torolt") == "false" && (string)x1.Element("felev") == felev.ToString() && (string)x1.Element("nev") == kod.ToString()
                                   select x1;

                    kulcs = talalat1.Count();
                    break;
                case "admin_ertekeles_hallgato_lista":
                    XDocument keres2 = XDocument.Load("hallgatok.xml");
                    var talalat2 = from x2 in keres2.Root.Descendants("targy")
                                   where (string)x2.Element("torolt") == "false" && (string)x2.Element("felev") == felev.ToString() && (string)x2.Element("nev") == kod.ToString()
                                   select x2;

                    kulcs = talalat2.Count();
                    break;
                case "oktato_ertekeles_hallgato_targy_lista":
                    bool volt3 = false;
                    string hallgato3 = "";
                    string targy3 = "";

                    for (int i = 0; i < kod.Length; i++)
                    {
                        if (kod[i].ToString() != "*")
                        {
                            if (volt3 == false)
                            {
                                hallgato3 += kod[i].ToString();
                            }
                            else
                            {
                                targy3 += kod[i].ToString();
                            }
                        }
                        else
                        {
                            volt3 = true;
                        }
                    }
                    XDocument keres3 = XDocument.Load("hallgatok.xml");
                    var talalat3 = from x3 in keres3.Root.Descendants("targy")
                                   where (string)x3.Element("torolt") == "false" && (string)x3.Element("felev") == felev.ToString() && (string)x3.Element("kraken") == hallgato3.ToString() && (string)x3.Element("nev") == targy3.ToString()
                                   select x3;

                    kulcs = talalat3.Count();
                    break;
                case "admin_ertekeles_hallgato_targy_lista":
                    bool volt4 = false;
                    string hallgato4 = "";
                    string targy4 = "";

                    for (int i = 0; i < kod.Length; i++)
                    {
                        if (kod[i].ToString() != "*")
                        {
                            if (volt4 == false)
                            {
                                hallgato4 += kod[i].ToString();
                            }
                            else
                            {
                                targy4 += kod[i].ToString();
                            }
                        }
                        else
                        {
                            volt4 = true;
                        }
                    }
                    XDocument keres4 = XDocument.Load("hallgatok.xml");
                    var talalat4 = from x4 in keres4.Root.Descendants("targy")
                                   where (string)x4.Element("torolt") == "false" && (string)x4.Element("felev") == felev.ToString() && (string)x4.Element("kraken") == hallgato4.ToString() && (string)x4.Element("nev") == targy4.ToString()
                                   select x4;

                    kulcs = talalat4.Count();
                    break;
                case "ertekeles_alairas":
                    bool volt5 = false;
                    string hallgato5 = "";
                    string targy5 = "";

                    for (int i = 0; i < kod.Length; i++)
                    {
                        if (kod[i].ToString() != "*")
                        {
                            if (volt5 == false)
                            {
                                hallgato5 += kod[i].ToString();
                            }
                            else
                            {
                                targy5 += kod[i].ToString();
                            }
                        }
                        else
                        {
                            volt5 = true;
                        }
                    }
                    XDocument keres5 = XDocument.Load("hallgatok.xml");
                    var talalat5 = from x5 in keres5.Root.Descendants("targy")
                                   where (string)x5.Element("torolt") == "false" && (string)x5.Element("felev") == felev.ToString() && (string)x5.Element("kraken") == hallgato5.ToString() && (string)x5.Element("nev") == targy5.ToString()
                                   select x5;

                    kulcs = talalat5.Count();
                    break;
                case "ertekeles_jegy":
                    bool volt6 = false;
                    string hallgato6 = "";
                    string targy6 = "";

                    for (int i = 0; i < kod.Length; i++)
                    {
                        if (kod[i].ToString() != "*")
                        {
                            if (volt6 == false)
                            {
                                hallgato6 += kod[i].ToString();
                            }
                            else
                            {
                                targy6 += kod[i].ToString();
                            }
                        }
                        else
                        {
                            volt6 = true;
                        }
                    }
                    XDocument keres6 = XDocument.Load("hallgatok.xml");
                    var talalat6 = from x6 in keres6.Root.Descendants("targy")
                                   where (string)x6.Element("torolt") == "false" && (string)x6.Element("felev") == felev.ToString() && (string)x6.Element("kraken") == hallgato6.ToString() && (string)x6.Element("nev") == targy6.ToString()
                                   select x6;

                    kulcs = talalat6.Count();
                    break;
                case "hallgato_targyfelvetel_lista":
                    XDocument keres7 = XDocument.Load("hallgatok.xml");
                    var talalat7 = from x7 in keres7.Root.Descendants("targy")
                                   where (string)x7.Element("torolt") == "false" && (string)x7.Element("felev") == felev.ToString() && (string)x7.Element("kraken") == kod.ToString()
                                   select x7;

                    kulcs = talalat7.Count();
                    break;
                case "hallgato_targyfelvetel_felvetel":
                    bool volt8 = false;
                    string hallgato8 = "";
                    string targy8 = "";

                    for (int i = 0; i < felev.Length; i++)
                    {
                        if (felev[i].ToString() != "*")
                        {
                            if (volt8 == false)
                            {
                                hallgato8 += felev[i].ToString();
                            }
                            else
                            {
                                targy8 += felev[i].ToString();
                            }
                        }
                        else
                        {
                            volt8 = true;
                        }
                    }
                    XDocument keres8 = XDocument.Load("hallgatok.xml");
                    var talalat8 = from x8 in keres8.Root.Descendants("targy")
                                   where (string)x8.Element("torolt") == "false" && (string)x8.Element("kod") == targy8.ToString() && (string)x8.Element("kraken") == kod.ToString() && (string)x8.Element("felev") == hallgato8.ToString()
                                   select x8;

                    kulcs = talalat8.Count();
                    break;
                case "hallgato_targyfelvetel_leadas":
                    bool volt9 = false;
                    string hallgato9 = "";
                    string targy9 = "";

                    for (int i = 0; i < kod.Length; i++)
                    {
                        if (kod[i].ToString() != "*")
                        {
                            if (volt9 == false)
                            {
                                hallgato9 += kod[i].ToString();
                            }
                            else
                            {
                                targy9 += kod[i].ToString();
                            }
                        }
                        else
                        {
                            volt9 = true;
                        }
                    }
                    XDocument keres9 = XDocument.Load("hallgatok.xml");
                    var talalat9 = from x9 in keres9.Root.Descendants("targy")
                                   where (string)x9.Element("torolt") == "false" && (string)x9.Element("kod") == targy9.ToString() && (string)x9.Element("kraken") == hallgato9.ToString() && (string)x9.Element("felev") == felev.ToString()
                                   select x9;

                    kulcs = talalat9.Count();
                    break;
                case "hallgato_leckekonyv":
                    XDocument keres10 = XDocument.Load("hallgatok.xml");
                    var talalat10 = from x10 in keres10.Root.Descendants("targy")
                                    where (string)x10.Element("torolt") == "false" && (string)x10.Element("felev") == felev.ToString() && (string)x10.Element("kraken") == kod.ToString()
                                    select x10;

                    kulcs = talalat10.Count();
                    break;
                case "hallgato_felvett_targyak":
                    XDocument keres11 = XDocument.Load("hallgatok.xml");
                    var talalat11 = from x11 in keres11.Root.Descendants("targy")
                                    where (string)x11.Element("torolt") == "false" && (string)x11.Element("felev") == felev.ToString() && (string)x11.Element("kraken") == kod.ToString()
                                    select x11;

                    kulcs = talalat11.Count();
                    break;
                case "hallgato_teljesitett_targyak":
                    XDocument keres12 = XDocument.Load("hallgatok.xml");
                    var talalat12 = from x12 in keres12.Root.Descendants("targy")
                                    where (string)x12.Element("torolt") == "false" && (string)x12.Element("jegy") != "" && (int)x12.Element("jegy") > 1 && (string)x12.Element("kraken") == kod.ToString()
                                    select x12;

                    kulcs = talalat12.Count();
                    break;
                case "admin_targyfelvetel_hallgato_targyai":
                    XDocument keres13 = XDocument.Load("hallgatok.xml");
                    var talalat13 = from x13 in keres13.Root.Descendants("targy")
                                    where (string)x13.Element("torolt") == "false" && (string)x13.Element("felev") == felev.ToString() && (string)x13.Element("kraken") == kod.ToString()
                                    select x13;

                    kulcs = talalat13.Count();
                    break;
                case "admin_targyfelvetel_leadas":
                    bool volt14 = false;
                    string hallgato14 = "";
                    string targy14 = "";

                    for (int i = 0; i < kod.Length; i++)
                    {
                        if (kod[i].ToString() != "*")
                        {
                            if (volt14 == false)
                            {
                                hallgato14 += kod[i].ToString();
                            }
                            else
                            {
                                targy14 += kod[i].ToString();
                            }
                        }
                        else
                        {
                            volt14 = true;
                        }
                    }
                    XDocument keres14 = XDocument.Load("hallgatok.xml");
                    var talalat14 = from x14 in keres14.Root.Descendants("targy")
                                    where (string)x14.Element("torolt") == "false" && (string)x14.Element("nev") == targy14.ToString() && (string)x14.Element("kraken") == hallgato14.ToString() && (string)x14.Element("felev") == felev.ToString()
                                    select x14;

                    kulcs = talalat14.Count();
                    break;
            }

            bool kis = false;

            if (kulcs == 0)
            {
                kis = true;
                kulcs = hallgato_kulcs;
            }

            Hallgato_index[] valasz = new Hallgato_index[kulcs];

            if (kis == false)
            {
                int szamlalo = 0;

                switch (input.ToString())
                {
                    case "oktatko_ertekeles_hallgato_lista":
                        XDocument keres1 = XDocument.Load("hallgatok.xml");
                        var talalat1 = from x1 in keres1.Root.Descendants("targy")
                                       where (string)x1.Element("torolt") == "false" && (string)x1.Element("felev") == felev.ToString() && (string)x1.Element("nev") == kod.ToString()
                                       select x1;

                        foreach (var q in talalat1)
                        {
                            valasz[szamlalo] = new Hallgato_index((string)q.Element("nev"), (string)q.Element("torolt"), (string)q.Element("kod"), (string)q.Element("kredit"), (string)q.Element("ido"), (string)q.Element("oktato"), (string)q.Element("felev"), (string)q.Element("kov"), (string)q.Element("elokov"), (string)q.Attribute("id"), (string)q.Element("kraken"), (string)q.Element("jegy"), (string)q.Element("alairas"));
                            szamlalo++;
                        }
                        break;
                    case "admin_ertekeles_hallgato_lista":
                        XDocument keres2 = XDocument.Load("hallgatok.xml");
                        var talalat2 = from x2 in keres2.Root.Descendants("targy")
                                       where (string)x2.Element("torolt") == "false" && (string)x2.Element("felev") == felev.ToString() && (string)x2.Element("nev") == kod.ToString()
                                       select x2;

                        foreach (var q in talalat2)
                        {
                            valasz[szamlalo] = new Hallgato_index((string)q.Element("nev"), (string)q.Element("torolt"), (string)q.Element("kod"), (string)q.Element("kredit"), (string)q.Element("ido"), (string)q.Element("oktato"), (string)q.Element("felev"), (string)q.Element("kov"), (string)q.Element("elokov"), (string)q.Attribute("id"), (string)q.Element("kraken"), (string)q.Element("jegy"), (string)q.Element("alairas"));
                            szamlalo++;
                        }
                        break;
                    case "oktatko_ertekeles_hallgato_targy_lista":
                        bool volt3 = false;
                        string hallgato3 = "";
                        string targy3 = "";

                        for (int i = 0; i < kod.Length; i++)
                        {
                            if (kod[i].ToString() != "*")
                            {
                                if (volt3 == false)
                                {
                                    hallgato3 += kod[i].ToString();
                                }
                                else
                                {
                                    targy3 += kod[i].ToString();
                                }
                            }
                            else
                            {
                                volt3 = true;
                            }
                        }
                        XDocument keres3 = XDocument.Load("hallgatok.xml");
                        var talalat3 = from x3 in keres3.Root.Descendants("targy")
                                       where (string)x3.Element("torolt") == "false" && (string)x3.Element("felev") == felev.ToString() && (string)x3.Element("kraken") == hallgato3.ToString() && (string)x3.Element("nev") == targy3.ToString()
                                       select x3;

                        foreach (var q in talalat3)
                        {
                            valasz[szamlalo] = new Hallgato_index((string)q.Element("nev"), (string)q.Element("torolt"), (string)q.Element("kod"), (string)q.Element("kredit"), (string)q.Element("ido"), (string)q.Element("oktato"), (string)q.Element("felev"), (string)q.Element("kov"), (string)q.Element("elokov"), (string)q.Attribute("id"), (string)q.Element("kraken"), (string)q.Element("jegy"), (string)q.Element("alairas"));
                            szamlalo++;
                        }
                        break;
                    case "admin_ertekeles_hallgato_targy_lista":
                        bool volt4 = false;
                        string hallgato4 = "";
                        string targy4 = "";

                        for (int i = 0; i < kod.Length; i++)
                        {
                            if (kod[i].ToString() != "*")
                            {
                                if (volt4 == false)
                                {
                                    hallgato4 += kod[i].ToString();
                                }
                                else
                                {
                                    targy4 += kod[i].ToString();
                                }
                            }
                            else
                            {
                                volt4 = true;
                            }
                        }
                        XDocument keres4 = XDocument.Load("hallgatok.xml");
                        var talalat4 = from x4 in keres4.Root.Descendants("targy")
                                       where (string)x4.Element("torolt") == "false" && (string)x4.Element("felev") == felev.ToString() && (string)x4.Element("kraken") == hallgato4.ToString() && (string)x4.Element("nev") == targy4.ToString()
                                       select x4;

                        foreach (var q in talalat4)
                        {
                            valasz[szamlalo] = new Hallgato_index((string)q.Element("nev"), (string)q.Element("torolt"), (string)q.Element("kod"), (string)q.Element("kredit"), (string)q.Element("ido"), (string)q.Element("oktato"), (string)q.Element("felev"), (string)q.Element("kov"), (string)q.Element("elokov"), (string)q.Attribute("id"), (string)q.Element("kraken"), (string)q.Element("jegy"), (string)q.Element("alairas"));
                            szamlalo++;
                        }
                        break;
                    case "ertekeles_alairas":
                        bool volt5 = false;
                        string hallgato5 = "";
                        string targy5 = "";

                        for (int i = 0; i < kod.Length; i++)
                        {
                            if (kod[i].ToString() != "*")
                            {
                                if (volt5 == false)
                                {
                                    hallgato5 += kod[i].ToString();
                                }
                                else
                                {
                                    targy5 += kod[i].ToString();
                                }
                            }
                            else
                            {
                                volt5 = true;
                            }
                        }
                        XDocument keres5 = XDocument.Load("hallgatok.xml");
                        var talalat5 = from x5 in keres5.Root.Descendants("targy")
                                       where (string)x5.Element("torolt") == "false" && (string)x5.Element("felev") == felev.ToString() && (string)x5.Element("kraken") == hallgato5.ToString() && (string)x5.Element("nev") == targy5.ToString()
                                       select x5;

                        foreach (var q in talalat5)
                        {
                            valasz[szamlalo] = new Hallgato_index((string)q.Element("nev"), (string)q.Element("torolt"), (string)q.Element("kod"), (string)q.Element("kredit"), (string)q.Element("ido"), (string)q.Element("oktato"), (string)q.Element("felev"), (string)q.Element("kov"), (string)q.Element("elokov"), (string)q.Attribute("id"), (string)q.Element("kraken"), (string)q.Element("jegy"), (string)q.Element("alairas"));
                            szamlalo++;
                        }
                        break;
                    case "ertekeles_jegy":
                        bool volt6 = false;
                        string hallgato6 = "";
                        string targy6 = "";

                        for (int i = 0; i < kod.Length; i++)
                        {
                            if (kod[i].ToString() != "*")
                            {
                                if (volt6 == false)
                                {
                                    hallgato6 += kod[i].ToString();
                                }
                                else
                                {
                                    targy6 += kod[i].ToString();
                                }
                            }
                            else
                            {
                                volt6 = true;
                            }
                        }
                        XDocument keres6 = XDocument.Load("hallgatok.xml");
                        var talalat6 = from x6 in keres6.Root.Descendants("targy")
                                       where (string)x6.Element("torolt") == "false" && (string)x6.Element("felev") == felev.ToString() && (string)x6.Element("kraken") == hallgato6.ToString() && (string)x6.Element("nev") == targy6.ToString()
                                       select x6;

                        foreach (var q in talalat6)
                        {
                            valasz[szamlalo] = new Hallgato_index((string)q.Element("nev"), (string)q.Element("torolt"), (string)q.Element("kod"), (string)q.Element("kredit"), (string)q.Element("ido"), (string)q.Element("oktato"), (string)q.Element("felev"), (string)q.Element("kov"), (string)q.Element("elokov"), (string)q.Attribute("id"), (string)q.Element("kraken"), (string)q.Element("jegy"), (string)q.Element("alairas"));
                            szamlalo++;
                        }
                        break;
                    case "hallgato_targyfelvetel_lista":
                        XDocument keres7 = XDocument.Load("hallgatok.xml");
                        var talalat7 = from x7 in keres7.Root.Descendants("targy")
                                       where (string)x7.Element("torolt") == "false" && (string)x7.Element("felev") == felev.ToString() && (string)x7.Element("kraken") == kod.ToString()
                                       select x7;

                        foreach (var q in talalat7)
                        {
                            valasz[szamlalo] = new Hallgato_index((string)q.Element("nev"), (string)q.Element("torolt"), (string)q.Element("kod"), (string)q.Element("kredit"), (string)q.Element("ido"), (string)q.Element("oktato"), (string)q.Element("felev"), (string)q.Element("kov"), (string)q.Element("elokov"), (string)q.Attribute("id"), (string)q.Element("kraken"), (string)q.Element("jegy"), (string)q.Element("alairas"));
                            szamlalo++;
                        }
                        break;
                    case "hallgato_targyfelvetel_felvetel":
                        bool volt8 = false;
                        string hallgato8 = "";
                        string targy8 = "";

                        for (int i = 0; i < felev.Length; i++)
                        {
                            if (felev[i].ToString() != "*")
                            {
                                if (volt8 == false)
                                {
                                    hallgato8 += felev[i].ToString();
                                }
                                else
                                {
                                    targy8 += felev[i].ToString();
                                }
                            }
                            else
                            {
                                volt8 = true;
                            }
                        }
                        XDocument keres8 = XDocument.Load("hallgatok.xml");
                        var talalat8 = from x8 in keres8.Root.Descendants("targy")
                                       where (string)x8.Element("torolt") == "false" && (string)x8.Element("kod") == targy8.ToString() && (string)x8.Element("kraken") == kod.ToString() && (string)x8.Element("felev") == hallgato8.ToString()
                                       select x8;

                        foreach (var q in talalat8)
                        {
                            valasz[szamlalo] = new Hallgato_index((string)q.Element("nev"), (string)q.Element("torolt"), (string)q.Element("kod"), (string)q.Element("kredit"), (string)q.Element("ido"), (string)q.Element("oktato"), (string)q.Element("felev"), (string)q.Element("kov"), (string)q.Element("elokov"), (string)q.Attribute("id"), (string)q.Element("kraken"), (string)q.Element("jegy"), (string)q.Element("alairas"));
                            szamlalo++;
                        }
                        break;
                    case "hallgato_targyfelvetel_leadas":
                        bool volt9 = false;
                        string hallgato9 = "";
                        string targy9 = "";

                        for (int i = 0; i < kod.Length; i++)
                        {
                            if (kod[i].ToString() != "*")
                            {
                                if (volt9 == false)
                                {
                                    hallgato9 += kod[i].ToString();
                                }
                                else
                                {
                                    targy9 += kod[i].ToString();
                                }
                            }
                            else
                            {
                                volt9 = true;
                            }
                        }
                        XDocument keres9 = XDocument.Load("hallgatok.xml");
                        var talalat9 = from x9 in keres9.Root.Descendants("targy")
                                       where (string)x9.Element("torolt") == "false" && (string)x9.Element("kod") == targy9.ToString() && (string)x9.Element("kraken") == hallgato9.ToString() && (string)x9.Element("felev") == felev.ToString()
                                       select x9;

                        foreach (var q in talalat9)
                        {
                            valasz[szamlalo] = new Hallgato_index((string)q.Element("nev"), (string)q.Element("torolt"), (string)q.Element("kod"), (string)q.Element("kredit"), (string)q.Element("ido"), (string)q.Element("oktato"), (string)q.Element("felev"), (string)q.Element("kov"), (string)q.Element("elokov"), (string)q.Attribute("id"), (string)q.Element("kraken"), (string)q.Element("jegy"), (string)q.Element("alairas"));
                            szamlalo++;
                        }
                        break;
                    case "hallgato_leckekonyv":
                        XDocument keres10 = XDocument.Load("hallgatok.xml");
                        var talalat10 = from x10 in keres10.Root.Descendants("targy")
                                        where (string)x10.Element("torolt") == "false" && (string)x10.Element("felev") == felev.ToString() && (string)x10.Element("kraken") == kod.ToString()
                                        select x10;

                        foreach (var q in talalat10)
                        {
                            valasz[szamlalo] = new Hallgato_index((string)q.Element("nev"), (string)q.Element("torolt"), (string)q.Element("kod"), (string)q.Element("kredit"), (string)q.Element("ido"), (string)q.Element("oktato"), (string)q.Element("felev"), (string)q.Element("kov"), (string)q.Element("elokov"), (string)q.Attribute("id"), (string)q.Element("kraken"), (string)q.Element("jegy"), (string)q.Element("alairas"));
                            szamlalo++;
                        }
                        break;
                    case "hallgato_felvett_targyak":
                        XDocument keres11 = XDocument.Load("hallgatok.xml");
                        var talalat11 = from x11 in keres11.Root.Descendants("targy")
                                        where (string)x11.Element("torolt") == "false" && (string)x11.Element("felev") == felev.ToString() && (string)x11.Element("kraken") == kod.ToString()
                                        select x11;

                        foreach (var q in talalat11)
                        {
                            valasz[szamlalo] = new Hallgato_index((string)q.Element("nev"), (string)q.Element("torolt"), (string)q.Element("kod"), (string)q.Element("kredit"), (string)q.Element("ido"), (string)q.Element("oktato"), (string)q.Element("felev"), (string)q.Element("kov"), (string)q.Element("elokov"), (string)q.Attribute("id"), (string)q.Element("kraken"), (string)q.Element("jegy"), (string)q.Element("alairas"));
                            szamlalo++;
                        }
                        break;
                    case "hallgato_teljesitett_targyak":
                        XDocument keres12 = XDocument.Load("hallgatok.xml");
                        var talalat12 = from x12 in keres12.Root.Descendants("targy")
                                        where (string)x12.Element("torolt") == "false" && (string)x12.Element("jegy") != "" && (int)x12.Element("jegy") > 1 && (string)x12.Element("kraken") == kod.ToString()
                                        select x12;

                        foreach (var q in talalat12)
                        {
                            valasz[szamlalo] = new Hallgato_index((string)q.Element("nev"), (string)q.Element("torolt"), (string)q.Element("kod"), (string)q.Element("kredit"), (string)q.Element("ido"), (string)q.Element("oktato"), (string)q.Element("felev"), (string)q.Element("kov"), (string)q.Element("elokov"), (string)q.Attribute("id"), (string)q.Element("kraken"), (string)q.Element("jegy"), (string)q.Element("alairas"));
                            szamlalo++;
                        }
                        break;
                    case "admin_targyfelvetel_hallgato_targyai":
                        XDocument keres13 = XDocument.Load("hallgatok.xml");
                        var talalat13 = from x13 in keres13.Root.Descendants("targy")
                                        where (string)x13.Element("torolt") == "false" && (string)x13.Element("felev") == felev.ToString() && (string)x13.Element("kraken") == kod.ToString()
                                        select x13;

                        foreach (var q in talalat13)
                        {
                            valasz[szamlalo] = new Hallgato_index((string)q.Element("nev"), (string)q.Element("torolt"), (string)q.Element("kod"), (string)q.Element("kredit"), (string)q.Element("ido"), (string)q.Element("oktato"), (string)q.Element("felev"), (string)q.Element("kov"), (string)q.Element("elokov"), (string)q.Attribute("id"), (string)q.Element("kraken"), (string)q.Element("jegy"), (string)q.Element("alairas"));
                            szamlalo++;
                        }
                        break;
                    case "admin_targyfelvetel_leadas":
                        bool volt14 = false;
                        string hallgato14 = "";
                        string targy14 = "";

                        for (int i = 0; i < kod.Length; i++)
                        {
                            if (kod[i].ToString() != "*")
                            {
                                if (volt14 == false)
                                {
                                    hallgato14 += kod[i].ToString();
                                }
                                else
                                {
                                    targy14 += kod[i].ToString();
                                }
                            }
                            else
                            {
                                volt14 = true;
                            }
                        }
                        XDocument keres14 = XDocument.Load("hallgatok.xml");
                        var talalat14 = from x14 in keres14.Root.Descendants("targy")
                                        where (string)x14.Element("torolt") == "false" && (string)x14.Element("nev") == targy14.ToString() && (string)x14.Element("kraken") == hallgato14.ToString() && (string)x14.Element("felev") == felev.ToString()
                                        select x14;

                        foreach (var q in talalat14)
                        {
                            valasz[szamlalo] = new Hallgato_index((string)q.Element("nev"), (string)q.Element("torolt"), (string)q.Element("kod"), (string)q.Element("kredit"), (string)q.Element("ido"), (string)q.Element("oktato"), (string)q.Element("felev"), (string)q.Element("kov"), (string)q.Element("elokov"), (string)q.Attribute("id"), (string)q.Element("kraken"), (string)q.Element("jegy"), (string)q.Element("alairas"));
                            szamlalo++;
                        }
                        break;
                }
            }

            return valasz;
        }

        #endregion List

        #region Delete

        /// <summary>
        /// Deletes a message.
        /// </summary>
        /// <param name="input">The message to delete.</param>
        public void Uzenet_Torles(Uzenet input)
        {
            try
            {
                XDocument keres = XDocument.Load("uzenetek.xml");
                var talalat = from x in keres.Root.Descendants("uzenet")
                              where (string)x.Element("torolt") == "false" && (string)x.Element("kuldo") == input.Kuldoje.ToString() && (string)x.Element("cimzett") == input.Cimzettje.ToString() && (string)x.Element("targy") == input.Targya.ToString() && (string)x.Element("szoveg") == input.Szovege.ToString() && (string)x.Element("ido") == input.Ideje.ToString()
                              select x;

                talalat.Single().Element("torolt").Value = "true";
                keres.Save("uzenetek.xml");
            }
            catch
            {

            }
        }

        /// <summary>
        /// Deletes a student_index.
        /// </summary>
        /// <param name="input">The student_index to delete.</param>
        public void Hallgato_Index_Torles(Hallgato_index input)
        {
            try
            {
                XDocument keres = XDocument.Load("hallgatok.xml");
                var talalat = from x in keres.Root.Descendants("targy")
                              where (string)x.Element("torolt") == "false" && (string)x.Element("kod") == input.Kodja.ToString() && (string)x.Element("kraken") == input.Krakenje.ToString()
                              select x;

                talalat.Single().Element("torolt").Value = "true";
                keres.Save("hallgatok.xml");
            }
            catch
            {

            }
        }

        /// <summary>
        /// Deletes a teacher_index.
        /// </summary>
        /// <param name="input">The teacher_index to delete.</param>
        public void Oktato_Index_Torles(Oktato_index input)
        {
            try
            {
                XDocument keres = XDocument.Load("oktatok.xml");
                var talalat = from x in keres.Root.Descendants("targy")
                              where (string)x.Element("torolt") == "false" && (string)x.Element("kod") == input.Kodja.ToString() && (string)x.Element("kraken") == input.Krakenje.ToString()
                              select x;

                talalat.Single().Element("torolt").Value = "true";
                keres.Save("oktatok.xml");
            }
            catch
            {

            }
        }

        /// <summary>
        /// Deletes a user.
        /// </summary>
        /// <param name="input">The user to delete.</param>
        public void Felhasznalo_Torles(Felhasznalo input)
        {
            try
            {
                XDocument keres = XDocument.Load("felhasznalok.xml");
                var talalat = from x in keres.Root.Descendants("felhasznalo")
                              where (string)x.Element("torolt") == "false" && (string)x.Element("kod") == input.Kodja.ToString()
                              select x;

                talalat.Single().Element("torolt").Value = "true";
                keres.Save("felhasznalok.xml");
            }
            catch
            {

            }
        }

        /// <summary>
        /// Deletes a subject.
        /// </summary>
        /// <param name="input">The subject to delete.</param>
        public void Targy_Torles(Targy input)
        {
            try
            {
                XDocument keres = XDocument.Load("targyak.xml");
                var talalat = from x in keres.Root.Descendants("targy")
                              where (string)x.Element("torolt") == "false" && (string)x.Element("kod") == input.Kodja.ToString()
                              select x;

                talalat.Single().Element("torolt").Value = "true";
                keres.Save("targyak.xml");
            }
            catch
            {

            }
        }

        #endregion Delete

        #region Add

        /// <summary>
        /// Adds a message.
        /// </summary>
        /// <param name="input">The message to add.</param>
        public void Uzenet_Hozzaad(Uzenet input)
        {
            try
            {
                XDocument mentes = XDocument.Load("uzenetek.xml");
                var talalat = from x in mentes.Root.Descendants("uzenet")
                              where (string)x.Element("ido") == input.Ideje.ToString() && (string)x.Element("kuldo") == input.Kuldoje.ToString() && (string)x.Element("cimzett") == input.Cimzettje.ToString() && (string)x.Element("targy") == input.Targya.ToString() && (string)x.Element("szoveg") == input.Szovege.ToString()
                              select x;

                if (talalat.Count() == 0)
                {
                    XAttribute idje = new XAttribute("id", input.Idje);
                    uzenet_kulcs++;
                    XElement torolve = new XElement("torolt", "false");
                    XElement cimzett = new XElement("cimzett", input.Cimzettje.ToString());
                    XElement kuldo = new XElement("kuldo", input.Kuldoje.ToString());
                    XElement targy = new XElement("targy", input.Targya.ToString());
                    XElement ido = new XElement("ido", input.Ideje.ToString());
                    XElement megvan_kuldo = new XElement("megvan_kuldo", input.Megvan_kuldonek.ToString());
                    XElement megvan_cimzett = new XElement("megvan_cimzett", input.Megvan_cimzettnek.ToString());
                    XElement szoveg = new XElement("szoveg", input.Szovege.ToString());
                    XElement mentve = new XElement("mentve", input.Mentve_e.ToString());
                    XElement olvasva = new XElement("olvasva", input.Olvasva_e.ToString());
                    XElement ujuzenet = new XElement("uzenet", idje, torolve, ido, kuldo, cimzett, targy, megvan_kuldo, megvan_cimzett, szoveg, mentve, olvasva);
                    mentes.Element("uzenetek").Add(ujuzenet);

                    mentes.Save("uzenetek.xml");
                }
            }
            catch
            {

            }
        }

        /// <summary>
        /// Adds a user.
        /// </summary>
        /// <param name="input">The user to add.</param>
        public void Felhasznalo_Hozzaad(Felhasznalo input)
        {
            try
            {
                XDocument mentes = XDocument.Load("felhasznalok.xml");
                var talalat = from x in mentes.Root.Descendants("felhasznalo")
                              where (string)x.Element("kod") == input.Kodja.ToString()
                              select x;

                if (talalat.Count() == 0)
                {
                    XAttribute idje = new XAttribute("id", input.Idje);
                    felhasznalo_kulcs++;
                    XElement torolve = new XElement("torolt", "false");
                    XElement kodja = new XElement("kod", input.Kodja.ToString());
                    XElement neve = new XElement("nev", input.Neve.ToString());
                    XElement jelszava = new XElement("jelszo", input.Jelszava.ToString());
                    XElement beosztasa = new XElement("beosztas", input.Beosztasa.ToString());
                    XElement emailje = new XElement("email", input.Emailje.ToString());
                    XElement szul_hely = new XElement("szuletes_helye", input.Szuletesi_helye.ToString());
                    XElement szul_ido = new XElement("szuletes_ideje", input.Szuletesi_ideje.ToString());
                    XElement fizetese = new XElement("fizetes", input.Fizetese.ToString());
                    XElement anyja = new XElement("anya", input.Anyja_neve.ToString());
                    XElement telefonja = new XElement("telefon", input.Telefonja.ToString());
                    XElement bankszama = new XElement("bankszamla", input.Bankszamla.ToString());
                    XElement statusza = new XElement("statusz", input.Statusza.ToString());
                    XElement ido = new XElement("beiratkozas", input.Beiratkozva.ToString());
                    XElement neme = new XElement("nem", input.Neme.ToString());
                    XElement jog = new XElement("jogviszony", input.Jogviszonya.ToString());
                    XElement cime = new XElement("cim", input.Lakcime.ToString());
                    XElement ujtag = new XElement("felhasznalo", idje, torolve, kodja, neve, jelszava, beosztasa, emailje, szul_hely, szul_ido, fizetese, anyja, telefonja, bankszama, statusza, ido, neme, jog, cime);
                    mentes.Element("felhasznalok").Add(ujtag);

                    mentes.Save("felhasznalok.xml");
                }
            }
            catch
            {

            }
        }

        /// <summary>
        /// Adds a subject.
        /// </summary>
        /// <param name="input">The subject to add.</param>
        public void Targy_Hozzaad(Targy input)
        {
            try
            {
                XDocument mentes = XDocument.Load("targyak.xml");
                var talalat = from x in mentes.Root.Descendants("targy")
                              where (string)x.Element("kod") == input.Kodja.ToString()
                              select x;

                if (talalat.Count() == 0)
                {
                    XAttribute idje = new XAttribute("id", input.Idje);
                    targy_kulcs++;
                    XElement torolve = new XElement("torolt", "false");
                    XElement kodja = new XElement("kod", input.Kodja.ToString());
                    XElement neve = new XElement("nev", input.Neve.ToString());
                    XElement kreditje = new XElement("kredit", input.Kreditje.ToString());
                    XElement kovje = new XElement("kov", input.Kovetelmenye.ToString());
                    XElement elokovje = new XElement("elokov", input.Elokovetelmenye.ToString());
                    XElement feleve = new XElement("felev", input.Feleve.ToString());
                    XElement oktatoja = new XElement("oktato", input.Oktatoja.ToString());
                    XElement ideje = new XElement("ido", input.Ideje.ToString());
                    XElement ujtargy = new XElement("targy", idje, torolve, kodja, neve, kreditje, oktatoja, ideje, feleve, elokovje, kovje);
                    mentes.Element("targyak").Add(ujtargy);

                    mentes.Save("targyak.xml");
                }
            }
            catch
            {

            }
        }

        /// <summary>
        /// Adds a student_index.
        /// </summary>
        /// <param name="input">The student_index to add.</param>
        public void Hallgato_Index_Hozzaad(Hallgato_index input)
        {
            try
            {
                XDocument mentes = XDocument.Load("hallgatok.xml");
                var talalat = from x in mentes.Root.Descendants("targy")
                              where (string)x.Element("kod") == input.Kodja.ToString() && (string)x.Element("kraken") == input.Krakenje.ToString()
                              select x;

                if (talalat.Count() == 0)
                {
                    XAttribute idje = new XAttribute("id", input.Idje);
                    hallgato_kulcs++;
                    XElement torolve = new XElement("torolt", "false");
                    XElement kodja = new XElement("kod", input.Kodja.ToString());
                    XElement neve = new XElement("nev", input.Neve.ToString());
                    XElement kreditje = new XElement("kredit", input.Kreditje.ToString());
                    XElement kovje = new XElement("kov", input.Kovetelmenye.ToString());
                    XElement elokovje = new XElement("elokov", input.Elokovetelmenye.ToString());
                    XElement feleve = new XElement("felev", input.Feleve.ToString());
                    XElement oktatoja = new XElement("oktato", input.Oktatoja.ToString());
                    XElement ideje = new XElement("ido", input.Ideje.ToString());
                    XElement krakenje = new XElement("kraken", input.Krakenje.ToString());
                    XElement jegye = new XElement("jegy", input.Jegye.ToString());
                    XElement alairasa = new XElement("alairas", input.Alairasa.ToString());
                    XElement ujtargy = new XElement("targy", idje, torolve, kodja, neve, kreditje, oktatoja, ideje, feleve, elokovje, kovje, krakenje, jegye, alairasa);
                    mentes.Element("targyak").Add(ujtargy);

                    mentes.Save("targyak.xml");
                }
            }
            catch
            {

            }
        }

        /// <summary>
        /// Adds a teacher_index.
        /// </summary>
        /// <param name="input">The teacher_index to add.</param>
        public void Oktato_Index_Hozzaad(Oktato_index input)
        {
            try
            {
                XDocument mentes = XDocument.Load("oktatok.xml");
                var talalat = from x in mentes.Root.Descendants("targy")
                              where (string)x.Element("kod") == input.Kodja.ToString() && (string)x.Element("kraken") == input.Krakenje.ToString()
                              select x;

                if (talalat.Count() == 0)
                {
                    XAttribute idje = new XAttribute("id", input.Idje);
                    oktato_kulcs++;
                    XElement torolve = new XElement("torolt", "false");
                    XElement kodja = new XElement("kod", input.Kodja.ToString());
                    XElement neve = new XElement("nev", input.Neve.ToString());
                    XElement kreditje = new XElement("kredit", input.Kreditje.ToString());
                    XElement kovje = new XElement("kov", input.Kovetelmenye.ToString());
                    XElement elokovje = new XElement("elokov", input.Elokovetelmenye.ToString());
                    XElement feleve = new XElement("felev", input.Feleve.ToString());
                    XElement oktatoja = new XElement("oktato", input.Oktatoja.ToString());
                    XElement ideje = new XElement("ido", input.Ideje.ToString());
                    XElement krakenje = new XElement("kraken", input.Krakenje.ToString());
                    XElement ujtargy = new XElement("targy", idje, torolve, kodja, neve, kreditje, oktatoja, ideje, feleve, elokovje, kovje, krakenje);
                    mentes.Element("targyak").Add(ujtargy);

                    mentes.Save("targyak.xml");
                }
            }
            catch
            {

            }
        }

        #endregion Add

        #region Edit

        /// <summary>
        /// Edits a message.
        /// </summary>
        /// <param name="mit">The old message.</param>
        /// <param name="mire">The new message.</param>
        public void Uzenet_Modosit(Uzenet mit, Uzenet mire)
        {
            try
            {
                XDocument keres = XDocument.Load("uzenetek.xml");
                var talalat = from x in keres.Root.Descendants("uzenet")
                              where (string)x.Element("cimzett") == mit.Cimzettje.ToString() && (string)x.Element("kuldo") == mit.Kuldoje.ToString() && (string)x.Element("targy") == mit.Targya.ToString() && (string)x.Element("szoveg") == mit.Szovege.ToString() && (string)x.Element("ido") == mit.Ideje.ToString()
                              select x;

                if (mit.Ideje.ToString() != mire.Ideje.ToString())
                {
                    talalat.Single().Element("ido").Value = mire.Ideje.ToString();
                }
                if (mit.Kuldoje.ToString() != mire.Kuldoje.ToString())
                {
                    talalat.Single().Element("kuldo").Value = mire.Kuldoje.ToString();
                }
                if (mit.Cimzettje.ToString() != mire.Cimzettje.ToString())
                {
                    talalat.Single().Element("cimzett").Value = mire.Cimzettje.ToString();
                }
                if (mit.Targya.ToString() != mire.Targya.ToString())
                {
                    talalat.Single().Element("targy").Value = mire.Targya.ToString();
                }
                if (mit.Megvan_kuldonek.ToString() != mire.Megvan_kuldonek.ToString())
                {
                    talalat.Single().Element("megvan_kuldo").Value = mire.Megvan_kuldonek.ToString();
                }
                if (mit.Megvan_cimzettnek.ToString() != mire.Megvan_cimzettnek.ToString())
                {
                    talalat.Single().Element("megvan_cimzett").Value = mire.Megvan_cimzettnek.ToString();
                }
                if (mit.Szovege.ToString() != mire.Szovege.ToString())
                {
                    talalat.Single().Element("szoveg").Value = mire.Szovege.ToString();
                }
                if (mit.Mentve_e.ToString() != mire.Mentve_e.ToString())
                {
                    talalat.Single().Element("mentve").Value = mire.Mentve_e.ToString();
                }
                if (mit.Olvasva_e.ToString() != mire.Olvasva_e.ToString())
                {
                    talalat.Single().Element("olvasva").Value = mire.Olvasva_e.ToString();
                }

                keres.Save("uzenetek.xml");
            }
            catch
            {

            }
        }

        /// <summary>
        /// Edits a subject.
        /// </summary>
        /// <param name="mit">The old subject.</param>
        /// <param name="mire">The new subject.</param>
        public void Targy_Modosit(Targy mit, Targy mire)
        {
            try
            {
                XDocument keres = XDocument.Load("targyak.xml");
                var talalat = from x in keres.Root.Descendants("targy")
                              where (string)x.Element("kod") == mit.Kodja.ToString()
                              select x;

                if (mit.Neve.ToString() != mire.Neve.ToString())
                {
                    talalat.Single().Element("nev").Value = mire.Neve.ToString();
                }
                if (mit.Kodja.ToString() != mire.Kodja.ToString())
                {
                    talalat.Single().Element("kod").Value = mire.Kodja.ToString();
                }
                if (mit.Kreditje.ToString() != mire.Kreditje.ToString())
                {
                    talalat.Single().Element("kredit").Value = mire.Kreditje.ToString();
                }
                if (mit.Oktatoja.ToString() != mire.Oktatoja.ToString())
                {
                    talalat.Single().Element("oktato").Value = mire.Oktatoja.ToString();
                }
                if (mit.Ideje.ToString() != mire.Ideje.ToString())
                {
                    talalat.Single().Element("ido").Value = mire.Ideje.ToString();
                }
                if (mit.Feleve.ToString() != mire.Feleve.ToString())
                {
                    talalat.Single().Element("felev").Value = mire.Feleve.ToString();
                }
                if (mit.Elokovetelmenye.ToString() != mire.Elokovetelmenye.ToString())
                {
                    talalat.Single().Element("elokov").Value = mire.Elokovetelmenye.ToString();
                }
                if (mit.Kovetelmenye.ToString() != mire.Kovetelmenye.ToString())
                {
                    talalat.Single().Element("kov").Value = mire.Kovetelmenye.ToString();
                }

                keres.Save("targyak.xml");
            }
            catch
            {

            }
        }

        /// <summary>
        /// Edits a teacher_index.
        /// </summary>
        /// <param name="mit">The old teacher_index.</param>
        /// <param name="mire">The new teacher_index.</param>
        public void Oktato_Index_Modosit(Oktato_index mit, Oktato_index mire)
        {
            try
            {
                XDocument keres = XDocument.Load("oktatok.xml");
                var talalat = from x in keres.Root.Descendants("targy")
                              where (string)x.Element("kod") == mit.Kodja.ToString() && (string)x.Element("kraken") == mit.Krakenje.ToString()
                              select x;

                if (mit.Neve.ToString() != mire.Neve.ToString())
                {
                    talalat.Single().Element("nev").Value = mire.Neve.ToString();
                }
                if (mit.Kodja.ToString() != mire.Kodja.ToString())
                {
                    talalat.Single().Element("kod").Value = mire.Kodja.ToString();
                }
                if (mit.Kreditje.ToString() != mire.Kreditje.ToString())
                {
                    talalat.Single().Element("kredit").Value = mire.Kreditje.ToString();
                }
                if (mit.Oktatoja.ToString() != mire.Oktatoja.ToString())
                {
                    talalat.Single().Element("oktato").Value = mire.Oktatoja.ToString();
                }
                if (mit.Ideje.ToString() != mire.Ideje.ToString())
                {
                    talalat.Single().Element("ido").Value = mire.Ideje.ToString();
                }
                if (mit.Feleve.ToString() != mire.Feleve.ToString())
                {
                    talalat.Single().Element("felev").Value = mire.Feleve.ToString();
                }
                if (mit.Elokovetelmenye.ToString() != mire.Elokovetelmenye.ToString())
                {
                    talalat.Single().Element("elokov").Value = mire.Elokovetelmenye.ToString();
                }
                if (mit.Kovetelmenye.ToString() != mire.Kovetelmenye.ToString())
                {
                    talalat.Single().Element("kov").Value = mire.Kovetelmenye.ToString();
                }
                if (mit.Krakenje.ToString() != mire.Krakenje.ToString())
                {
                    talalat.Single().Element("kraken").Value = mire.Krakenje.ToString();
                }

                keres.Save("targyak.xml");
            }
            catch
            {

            }
        }

        /// <summary>
        /// Edits a student_index.
        /// </summary>
        /// <param name="mit">The old student_index.</param>
        /// <param name="mire">The new student_index.</param>
        public void Hallgato_Index_Modosit(Hallgato_index mit, Hallgato_index mire)
        {
            try
            {
                XDocument keres = XDocument.Load("hallgatok.xml");
                var talalat = from x in keres.Root.Descendants("targy")
                              where (string)x.Element("kod") == mit.Kodja.ToString() && (string)x.Element("kraken") == mit.Krakenje.ToString()
                              select x;

                if (mit.Neve.ToString() != mire.Neve.ToString())
                {
                    talalat.Single().Element("nev").Value = mire.Neve.ToString();
                }
                if (mit.Kodja.ToString() != mire.Kodja.ToString())
                {
                    talalat.Single().Element("kod").Value = mire.Kodja.ToString();
                }
                if (mit.Kreditje.ToString() != mire.Kreditje.ToString())
                {
                    talalat.Single().Element("kredit").Value = mire.Kreditje.ToString();
                }
                if (mit.Oktatoja.ToString() != mire.Oktatoja.ToString())
                {
                    talalat.Single().Element("oktato").Value = mire.Oktatoja.ToString();
                }
                if (mit.Ideje.ToString() != mire.Ideje.ToString())
                {
                    talalat.Single().Element("ido").Value = mire.Ideje.ToString();
                }
                if (mit.Feleve.ToString() != mire.Feleve.ToString())
                {
                    talalat.Single().Element("felev").Value = mire.Feleve.ToString();
                }
                if (mit.Elokovetelmenye.ToString() != mire.Elokovetelmenye.ToString())
                {
                    talalat.Single().Element("elokov").Value = mire.Elokovetelmenye.ToString();
                }
                if (mit.Kovetelmenye.ToString() != mire.Kovetelmenye.ToString())
                {
                    talalat.Single().Element("kov").Value = mire.Kovetelmenye.ToString();
                }
                if (mit.Krakenje.ToString() != mire.Krakenje.ToString())
                {
                    talalat.Single().Element("kraken").Value = mire.Krakenje.ToString();
                }
                if (mit.Alairasa.ToString() != mire.Alairasa.ToString())
                {
                    talalat.Single().Element("alairas").Value = mire.Alairasa.ToString();
                }
                if (mit.Jegye.ToString() != mire.Jegye.ToString())
                {
                    talalat.Single().Element("jegy").Value = mire.Jegye.ToString();
                }

                keres.Save("targyak.xml");
            }
            catch
            {

            }
        }

        /// <summary>
        /// Edits a user.
        /// </summary>
        /// <param name="mit">The old user.</param>
        /// <param name="mire">The new user.</param>
        public void Felhasznalo_Modosit(Felhasznalo mit, Felhasznalo mire)
        {
            try
            {
                XDocument keres = XDocument.Load("felhasznalok.xml");
                var talalat = from x in keres.Root.Descendants("felhasznalo")
                              where (string)x.Element("kod") == mit.Kodja.ToString()
                              select x;

                if (mit.Kodja.ToString() != mire.Kodja.ToString())
                {
                    talalat.Single().Element("kod").Value = mire.Kodja.ToString();
                }
                if (mit.Neve.ToString() != mire.Neve.ToString())
                {
                    talalat.Single().Element("nev").Value = mire.Neve.ToString();
                }
                if (mit.Jelszava.ToString() != mire.Jelszava.ToString())
                {
                    talalat.Single().Element("jelszo").Value = mire.Jelszava.ToString();
                }
                if (mit.Beosztasa.ToString() != mire.Beosztasa.ToString())
                {
                    talalat.Single().Element("beosztas").Value = mire.Beosztasa.ToString();
                }
                if (mit.Emailje.ToString() != mire.Beosztasa.ToString())
                {
                    talalat.Single().Element("email").Value = mire.Emailje.ToString();
                }
                if (mit.Szuletesi_helye.ToString() != mire.Szuletesi_helye.ToString())
                {
                    talalat.Single().Element("szuletes_helye").Value = mire.Szuletesi_helye.ToString();
                }
                if (mit.Szuletesi_ideje.ToString() != mire.Szuletesi_ideje.ToString())
                {
                    talalat.Single().Element("szuletes_ideje").Value = mire.Szuletesi_ideje.ToString();
                }
                if (mit.Fizetese.ToString() != mire.Fizetese.ToString())
                {
                    talalat.Single().Element("fizetes").Value = mire.Fizetese.ToString();
                }
                if (mit.Anyja_neve.ToString() != mire.Anyja_neve.ToString())
                {
                    talalat.Single().Element("anya").Value = mire.Anyja_neve.ToString();
                }
                if (mit.Telefonja.ToString() != mire.Telefonja.ToString())
                {
                    talalat.Single().Element("telefon").Value = mire.Telefonja.ToString();
                }
                if (mit.Bankszamla.ToString() != mire.Bankszamla.ToString())
                {
                    talalat.Single().Element("bankszamla").Value = mire.Bankszamla.ToString();
                }
                if (mit.Statusza.ToString() != mire.Statusza.ToString())
                {
                    talalat.Single().Element("statusz").Value = mire.Statusza.ToString();
                }
                if (mit.Beiratkozva.ToString() != mire.Beiratkozva.ToString())
                {
                    talalat.Single().Element("beiratkozas").Value = mire.Beiratkozva.ToString();
                }
                if (mit.Neme.ToString() != mire.Neme.ToString())
                {
                    talalat.Single().Element("neme").Value = mire.Neme.ToString();
                }
                if (mit.Jogviszonya.ToString() != mire.Jogviszonya.ToString())
                {
                    talalat.Single().Element("jogviszony").Value = mire.Jogviszonya.ToString();
                }
                if (mit.Lakcime.ToString() != mire.Lakcime.ToString())
                {
                    talalat.Single().Element("cim").Value = mire.Lakcime.ToString();
                }

                keres.Save("felhasznalok.xml");
            }
            catch
            {

            }
        }

        #endregion Edit

        #endregion Methods
    }
}
