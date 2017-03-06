using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUI
{
    class Zodis
    {
        private string zodis { get; set; }
        public string pasleptasZodis { get; private set; }
        public List<char> pasleptoZodzioRaidziuSarasas { get; private set; }

        public Zodis(string zodis)
        {
            this.zodis = zodis;
            foreach(char c in zodis)
                pasleptasZodis += "_";
            pasleptoZodzioRaidziuSarasas = pasleptasZodis.ToList();
        }

        public bool Spejimas(char spejimas)
        {
            bool atspejo = false;
            for(int i=0; i<zodis.Length; i++)
            {
                if(zodis[i] == spejimas)
                {
                    pasleptoZodzioRaidziuSarasas[i] = spejimas;
                    Konvertavimas();
                    atspejo = true;
                }
            }
            if(atspejo)
                return true;
            return false;
        }

        private void Konvertavimas()
        {
            pasleptasZodis = string.Empty;
            foreach (char c in pasleptoZodzioRaidziuSarasas)
            {
                pasleptasZodis += c;
            }
        }

        public string Atvaizdavimas()
        {
            string dto = string.Empty;
            foreach (char c in pasleptasZodis)
                dto += c + " ";
            return dto;
        }

        public bool ArAtspejoZodi()
        {
            if(zodis == pasleptasZodis)
            {
                return true;
            }
            return false;
        }

        public string GautiZodi()
        {
            return zodis;
        }
    }
}
