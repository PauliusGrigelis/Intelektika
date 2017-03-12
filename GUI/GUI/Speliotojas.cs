using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.IO;	

namespace GUI
{
    public static class Speliotojas
    {
		private static bool taisykleNr1 = true;
		private static bool taisykleNr2 = true;
		private static bool taisykleNr3 = true;
		private static List<char> atspetos_raides = new List<char>();
        private static List<char> neatspetos_raides = new List<char>();
        private static string connectionString;
        private static string spejamasZodis;
        private static List<string> galimiVariantai = new List<string>();

        static Speliotojas()
        {
            string bendras = @"Data Source = (LocalDB)\MSSQLLocalDB; AttachDbFilename = |DataDirectory|\Zodziai.mdf; Integrated Security = True";
            string pauliaus = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\pgrig\Desktop\Intelektika\GUI\GUI\Zodziai.mdf;Integrated Security=True;";
            string olego = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Wegis\Documents\visual studio 2015\Projects\GUI\GUI\GUI\Zodziai.mdf;Integrated Security=True";

            
            if (Directory.Exists(@"C:\Users\Wegis\"))
            {
                connectionString = olego;
            }
            else if (Directory.Exists(@"C:\Users\pgrig\"))
            {
                connectionString = pauliaus;
            }
            else
            {
                connectionString = bendras;
            }
        }

        public static void Pazadinti(string zodis)
        {            
            spejamasZodis = zodis;
            string gautRaides = "exec GautZodziusPagalZodzioIlgi " + spejamasZodis.Length;
            DataTable DT = KreiptisDuombazen(gautRaides);
            foreach (DataRow eile in DT.Rows)
            {
                galimiVariantai.Add(panaikintiTarpus(eile[0].ToString()));
            }
			taisykleNr1 = true;
			taisykleNr2 = true;
			taisykleNr3 = true;
			atspetos_raides = new List<char>();
            neatspetos_raides = new List<char>();
        }

        private static string PasalintiBesikartojanciasRaides(string tekstas)
        {
            return new string(tekstas.ToCharArray().Distinct().ToArray());
        }

        public static void GautiSpejamaZodi(string spejamas)
        {
            spejamasZodis = spejamas;
        }

        public static char SpekRaide()
        {
			if(taisykleNr1)
			{
				return TopRaide();
			}
			else if(taisykleNr2)
			{
				char spejimas = IeskotiZodzioSuRegex();
				if(spejimas != '*') return spejimas;
				else
				{
					taisykleNr2 = false;
					return TopXRaidziu();
				}
			}
			else
			{
				return TopXRaidziu();
			}
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
            else
            {
                bandytosRaides = "'"+ bandytosRaides + "'";
            }
            //if(bandytosRaides.Length >15)
            //{
            //    int a = 10;
            //}
            return bandytosRaides;
        }

        private static char TopRaide()
        {
            string gautRaide = "exec GautiTopNesikartojanciaRaide N" + GautBandytosRaides();
            DataTable DT = KreiptisDuombazen(gautRaide);
            char spejamaRaide = Convert.ToChar(DT.Rows[0][0]);

            return spejamaRaide;
        }

        private static char TopXRaidziu()
        {
            string gautRaides = "exec GautTopPagalKieki "+(neatspetos_raides.Count+1).ToString()+", N" + GautBandytosRaides();
            DataTable DT = KreiptisDuombazen(gautRaides);
            List<RaidesKiekis> RKlistas = new List<RaidesKiekis>();
            foreach(DataRow eile in DT.Rows)
            {
                RaidesKiekis raide = new RaidesKiekis { raide = Convert.ToChar(eile[0]), kiekis = Convert.ToInt32(eile[1]) };
                RKlistas.Add(raide);
            }
            char spejamaRaide = AtsitiktinisPagalSvertus(RKlistas);
            return spejamaRaide;
        }

        private static char IeskotiZodzioSuRegex()
        {
            List<string> atrinktiZodziai = new List<string>();
            List<RaidesKiekis> RKlistas = new List<RaidesKiekis>();
            
            string pattern = string.Empty;
            foreach(char c in spejamasZodis)
            {
                if(c == '_') 
                    pattern+="[^"+GautBandytosRaides()+"]";
                else
                    pattern+=c;
            }

            foreach (string zodis in galimiVariantai) 
            {
                if (System.Text.RegularExpressions.Regex.IsMatch(zodis, pattern))
                    atrinktiZodziai.Add(zodis);
            }

            bool rasta = false;
            string apkarpytasZodis;
            foreach(string zodis in atrinktiZodziai)
            {
                apkarpytasZodis = PasalintiBesikartojanciasRaides(zodis);
                foreach (char raide in apkarpytasZodis)
                {
                    rasta = false;
                    if (!atspetos_raides.Contains(raide))
                    {
                        foreach (RaidesKiekis rk in RKlistas)
                        {
                            if (rk.raide == raide)
                            {
                                rk.kiekis++;
                                rasta = true;
                                break;
                            }
                        }
                        if (!rasta) RKlistas.Add(new RaidesKiekis { raide = raide, kiekis = 1 });
                    }
                }
            }

            galimiVariantai = atrinktiZodziai;
            char spejamaRaide = AtsitiktinisPagalSvertus(RKlistas);
            return spejamaRaide;
        }

        private static string panaikintiTarpus(string zodis)
        {
            string apdorotasTekstas = string.Empty;
            foreach (char c in zodis)
                if (c != ' ')
                    apdorotasTekstas += c;
            return apdorotasTekstas;
        }

        public static void RaidesAtspejimoSekme(bool sekme, char raide)
        {
            if (sekme)
                atspetos_raides.Add(raide);
            else
            {
                neatspetos_raides.Add(raide);
				taisykleNr1 = false;
            }
                
        }
        
        public static string iLoga()
        {
            return string.Empty;
        }
        
        public static void GautAtsakyma(bool pasisekimas, string spejamasZodis)
        {
            string irasytZodi = "exec IterptZodiIrSekme "+ pasisekimas + ", N'" + spejamasZodis + "'";
            string atnaujint = "exec AtnaujintiKiekius";
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

        private static char AtsitiktinisPagalSvertus(List<RaidesKiekis> rt)
        {
            int temp = 0;
            foreach (RaidesKiekis c in rt)
            {
                temp += c.kiekis;
            }
            Random rand = new Random();
            int rng = rand.Next(0, temp);
            int range = 0;
            char raide = '*';
            foreach (RaidesKiekis c in rt)
            {
                if (rng >= range && rng < (c.kiekis + range))
                {
                    raide = c.raide;
                    break;
                }
                range += c.kiekis;
            }
            return raide;
        }

        private static char KaireDesine()
        {
            
            return ' ';
        }

        private static char KaireDesineXRaidziu()
        {

            return ' ';
        }
    }
}
