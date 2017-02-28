using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
//using System.Data.DataTable;
//using System.Data.SqlClient;

namespace Speliotojas
{
    public static class Speliotojas
    {
        private static int spejimuKiekis = 0;
        private static List<char> atspetos_raides = new List<char>();
        private static List<char> neatspetos_raides = new List<char>();

        /// <summary>
        /// Pradedami spejimai su siuo metodu (in development)
        /// </summary>
        /// <param name="speliojimuKiekis">Nustatomas sio speliojimo seanso spejimu kiekis</param>
        public static void pradetSpeliot(int speliojimuKiekis)
        {
            if(spejimuKiekis > 0)
            {
                throw new Exception("Spėliojimas dar nebaigtas");
            }
            if(speliojimuKiekis <= 0)
            {
                throw new Exception("Mėginamas nustatyti neigiamas ar nulinis spėjimų kiekis");
            }
            spejimuKiekis = speliojimuKiekis;
        }

        /// <summary>
        /// Dar tuscias metodas, veliau jis turetu priimti masyva raidziu ar kokia struktura (in development)
        /// </summary>
        /// <returns>Grazina spejiama raide</returns>
        public static char spekRaide()
        {
            if (spejimuKiekis == 0)
            {
                throw new Exception("Spėjimai baigėsi,\r\nŽaidimo pabaiga");
            }
            spejimuKiekis--;
            return ' ';
        }

        /// <summary>
        /// Speliojimu uzbaigimas, gal is GVSo puses bus koks spejimu uzbaigimas kad vartotojas galetu ivesti nauja zodi (in development)
        /// </summary>
        public static void baigtSpeliot()
        {
            if (spejimuKiekis == 0)
            {
                throw new Exception("Spėjimų nebeliko,\r\nNegalima nutraukti spėjimų kiekio");
            }
            spejimuKiekis = 0;
        }

        /// <summary>
        /// Sis metodas veliau turetu but suristas but su GVSu kad jis periminetu kodel spejamas vienas ar kitas spejimas (in development)
        /// </summary>
        /// <returns></returns>
        public static string iLoga()
        {
            return string.Empty;
        }

        /// <summary>
        /// Išsaugo žodį ir atnaujina raidziu lentele
        /// </summary>
        /// <param name="pasisekimas"></param>
        /// <param name="spejamasZodis"></param>
        public static void gautAtsakyma(bool pasisekimas, string spejamasZodis)
        {
            string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Wegis\Documents\Zodziai.mdf;Integrated Security=True;Connect Timeout=30";
            string atnaujint = "exec AtnaujintiKiekius";
            string irasytZodi = "exec IterptZodi N'" + spejamasZodis + "'";
            DataTable tbl = new DataTable();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = irasytZodi;
                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            da.Fill(tbl);
                        }

                        cmd.CommandText = atnaujint;
                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            da.Fill(tbl); // <-- nors ner ka pildyt, vistiek reikia kad suveiktu procedura
                        }
                    }
                }
            }
            catch (SqlException ex) { }
        }
    }
}
