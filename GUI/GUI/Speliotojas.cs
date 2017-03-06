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
        private static List<char> atspetos_raides = new List<char>();
        private static List<char> neatspetos_raides = new List<char>();
        //private static string connectionString = @"Data Source = (LocalDB)\MSSQLLocalDB; AttachDbFilename = |DataDirectory|\Zodziai.mdf; Integrated Security = True";
        //

        //private static string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Paulius\Desktop\Gallows\GUI\GUI\Zodziai.mdf;Integrated Security=True";
        private static string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Wegis\Documents\visual studio 2015\Projects\GUI\GUI\GUI\Zodziai.mdf;Integrated Security=True";

        /// <summary>
        /// Dar tuscias metodas, veliau jis turetu priimti masyva raidziu ar kokia struktura (in development)
        /// </summary>
        /// <returns>Grazina spejama raide</returns>
        public static char spekRaide()
        {
            return TopRaide();
        }

        private static string GautBandytosRaides()
        {
            string bandytosRaides = string.Empty;
            foreach (char raide in atspetos_raides)
            {
                bandytosRaides += raide;
            }
            foreach (char raide in neatspetos_raides)
            {
                bandytosRaides += raide;
            }
            if (bandytosRaides == string.Empty)
            {
                bandytosRaides = "' '";
            }

            return bandytosRaides;
        }

        private static char TopRaide()
        {
            string gautRaide = "exec GautiTopNesikartojanciaRaide N" + GautBandytosRaides();
            DataTable DT = KreiptisDuombazen(gautRaide);
            char spejamaRaide = Convert.ToChar(DT.Rows[0][0]);

            return spejamaRaide;
        }

        public static void RaidesAtspejimoSekme(bool sekme, char raide)
        {
            if (sekme)
                atspetos_raides.Add(raide);
            else
                neatspetos_raides.Add(raide);
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
            KreiptisDuombazen(irasytZodi);
            KreiptisDuombazen(atnaujint);
        }

        private static DataTable KreiptisDuombazen(string komanda)
        {
            DataTable tbl = new DataTable();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = komanda;
                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            da.Fill(tbl);
                        }
                    }
                }
            }
            catch (SqlException ex) { }
            return tbl;
        }
    }
}
