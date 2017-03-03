using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace Speliotojas
{
    public static class Speliotojas
    {
        private static char spejamaRaide;
        private static List<char> atspetos_raides = new List<char>();
        private static List<char> neatspetos_raides = new List<char>();
        private static string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Wegis\Documents\Zodziai.mdf;Integrated Security=True;Connect Timeout=30";
        //private static string connectionString = @"Data Source = (LocalDB)\MSSQLLocalDB; AttachDbFilename = C:\Users\Paulius\Desktop\Kartuves\GUI\GUI\Zodziai.mdf; Integrated Security = True";
        

        /// <summary>
        /// Dar tuscias metodas, veliau jis turetu priimti masyva raidziu ar kokia struktura (in development)
        /// </summary>
        /// <returns>Grazina spejama raide</returns>
        public static char spekRaide()
        {
            string bandytosRaides = string.Empty;
            foreach(char raide in atspetos_raides)
            {
                bandytosRaides += raide;
            }
            foreach (char raide in neatspetos_raides)
            {
                bandytosRaides += raide;
            }
            if(bandytosRaides == string.Empty)
            {
                bandytosRaides = "''";
            }

            string gautRaide = "SELECT top 1 Raide from dbo.Raides where Raide not in ("+ bandytosRaides + ") order by NEWID()";
            //string gautRaide = "exec GautRandomRaide " + bandytosRaides; //SELECT top 1 Raide from dbo.Raides where Raide not in (@raides) order by NEWID()

            DataTable tbl = new DataTable();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = gautRaide;
                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            da.Fill(tbl);
                        }
                    }
                }
            }
            catch (SqlException ex) { }
            spejamaRaide = tbl.Rows[0].Field<char>(0);
            return spejamaRaide;
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
            string atnaujint = "exec AtnaujintiKiekius";
            //string irasytZodi = "exec IterptZodi N'" + spejamasZodis + "'";
            string irasytZodi = "exec IterptZodiIrSekme "+ pasisekimas + ", N'" + spejamasZodis + "'"; //exec IterptZodiIrSekme false , N'niekas'
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
