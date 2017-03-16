using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;
using System.Diagnostics;
using System.Media;
using System.IO; //laikinai

namespace GUI
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void panaikintiTarpus()
        {
            string apdorotasTekstas = string.Empty;
            foreach (char c in textBox1.Text)
                if (c != ' ')
                    apdorotasTekstas += c;
            textBox1.Text = apdorotasTekstas;
        }

		//greitas papildymas pries demonstracija, pamastyt apie geresni varianta
        string galimosRaides = "aąbcčdeęėfghiįyjklmnoprsštuūųvzž";
        private bool tikrintiZodi(string tekstas)
        {
            bool leista = false;
            foreach(char c in tekstas)
            {
                leista = false;
                foreach(char raide in galimosRaides)
                {
                    if (c == raide || c == Char.ToUpper(raide))
                    {
                        leista = true; break;
                    }
                }
                if (!leista) break;
            }
            return leista;
        }

        bool zaidimas = false;
        bool sustabdyta = false;
        int busena = 3;
        int gyvybes;
		private void button1_Click(object sender, EventArgs e)
		{

			List<string> zodziai = new List<string>();
			Encoding enc = Encoding.GetEncoding(1257); //"windows-1257"
			foreach(string line in File.ReadLines(@".\\..\\..\\src\\dazninis.txt", enc))
			{
				List<string> words = new List<string>();
				words = line.Split('\t').ToList();
				zodziai.Add(words[0]);
				zodziai.Add(words[2]);
			}
			foreach(string ivestis in zodziai)
			{
				if(tikrintiZodi(ivestis) && ivestis.Length > 1) //input apribojimai
				{
					Random rand = new Random();

					int luck = rand.Next(0, 2);
					if(luck == 0)
						Speliotojas.GautAtsakyma(true, ivestis.ToLower());
					else
						Speliotojas.GautAtsakyma(false, ivestis.ToLower());
				}
			}
			//if(button1.Text == "Atšaukti")
			//{
			//	button1.Text = "Pradėti";
			//	sustabdyta = true;
			//}
			//else
			//{
			//	//panaikintiTarpus(); //ar panaikinti tarpus? ar pranesti kad ivede neatpazistamu simboliu?
			//	if(tikrintiZodi(textBox1.Text) && textBox1.Text.Length > 1) //input apribojimai
			//	{
			//		button1.Text = "Atšaukti";
			//		sustabdyta = false;
			//		this.BeginInvoke(new MethodInvoker(() => { pictureBox3.Visible = false; }));
			//		this.BeginInvoke(new MethodInvoker(() => { pictureBox4.Visible = false; }));
			//		this.BeginInvoke(new MethodInvoker(() => { pictureBox1.Visible = true; }));
			//		this.BeginInvoke(new MethodInvoker(() => { pictureBox2.Visible = true; }));
			//		Zodis zodis = new Zodis(textBox1.Text.ToLower());
			//		textBox2.Text = zodis.Atvaizdavimas();

			//		//pradedamas zaidimas
			//		zaidimas = true;
			//		gyvybes = 5;
			//		label3.Text = gyvybes.ToString();
			//		Speliotojas.Pazadinti(zodis.pasleptasZodis);
			//		Task zaisti = new Task(() => pradeti(zodis));
			//		Thread.Sleep(50);
			//		zaisti.Start();
			//	}
			//	else
			//	{
			//		textBox1.Text = string.Empty;
			//		MessageBox.Show("Įvedėt neatpažįstamų simbolių");
			//	}
			//}
		}

        private void pradeti(Zodis zodis)
        {
            while (zaidimas)
            {
                Speliotojas.GautiSpejamaZodi(zodis.pasleptasZodis);
                apdorojamasSpejimas(zodis, Speliotojas.SpekRaide());
            }
        }

        SoundPlayer doh = new SoundPlayer(@".\\..\\..\\src\\Doh.wav");
        SoundPlayer woohoo = new SoundPlayer(@".\\..\\..\\src\\woohoo.wav");
        private void animacija()
        {
            try
            {

                if (busena == 0) //galvoja
                {
                    this.BeginInvoke(new MethodInvoker(()=> { pictureBox1.Visible = true; }));
                    pictureBox2.Image = Image.FromFile(".\\..\\..\\src\\homer_simpson_thinking.png");
                }
                else if (busena == 1) //atspejo
                {
                    this.BeginInvoke(new MethodInvoker(() => { pictureBox1.Visible = false; }));
                    this.BeginInvoke(new MethodInvoker(() => { woohoo.Play(); }));
                    pictureBox2.Image = Image.FromFile(".\\..\\..\\src\\Homer_simpsonwoohooo.gif");
                }
                else if (busena == 2) //neatspejo
                {
                    this.BeginInvoke(new MethodInvoker(() => { doh.Play(); }));
                    this.BeginInvoke(new MethodInvoker(() => { pictureBox1.Visible = false; }));
                    pictureBox2.Image = Image.FromFile(".\\..\\..\\src\\Homer_simpsondoh.png");
                }
                else //laukia
                {
                    this.BeginInvoke(new MethodInvoker(() => { pictureBox1.Visible = false; }));
                    this.BeginInvoke(new MethodInvoker(() => { pictureBox2.Visible = false; }));
                    this.BeginInvoke(new MethodInvoker(() => { pictureBox3.Visible = true; }));
                    this.BeginInvoke(new MethodInvoker(() => { pictureBox4.Visible = true; }));
                    pictureBox3.Image = Image.FromFile(".\\..\\..\\src\\sleeping.png");
                }
            }
            catch { }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            pictureBox1.Image = Image.FromFile(".\\..\\..\\src\\ajax-loader.gif");
            pictureBox1.Visible = false;
            pictureBox4.Image = Image.FromFile(".\\..\\..\\src\\Zzz.gif");
        }

        private void apdorojamasSpejimas(Zodis zodis, char spejimas)
        {
            busena = 0;
            animacija();
            //Thread.Sleep(2000); //tipo galvoja
            if (!sustabdyta)
            {
                if (zodis.Spejimas(spejimas))
                {
                    this.BeginInvoke(new MethodInvoker(() => { textBox2.Text = zodis.Atvaizdavimas(); }));
                    this.BeginInvoke(new MethodInvoker(() => { textBox3.AppendText("Atspėjo: " + spejimas + "\r\n"); }));
                    if (!zodis.ArAtspejoZodi())
                    {
                        busena = 1;
                        animacija();
                        Speliotojas.RaidesAtspejimoSekme(true, spejimas);
                    }
                    else //zaidimas baigtas, AI laimejo
                    {
                        this.BeginInvoke(new MethodInvoker(() => { textBox3.AppendText("AI laimėjo.\r\n"); }));
                        Debug.Write("pergale");
                        busena = 3;
                        animacija();
                        zaidimas = false;
                        this.BeginInvoke(new MethodInvoker(() => { button1.Text = "Pradėti"; }));
                        Speliotojas.GautAtsakyma(true, zodis.GautiZodi());
                        //ideti animacija, ar kaip kitaip atvaizduoti pergale
                    }
                }
                else
                {
                    gyvybes--;
                    this.BeginInvoke(new MethodInvoker(() => { label3.Text = gyvybes.ToString(); }));
                    this.BeginInvoke(new MethodInvoker(() => { textBox3.AppendText("Neatspėjo: " + spejimas + "\r\n"); }));
                    if (gyvybes != 0) 
                    {
                        busena = 2;
                        animacija();
                        Speliotojas.RaidesAtspejimoSekme(false, spejimas);
                    }
                    else //zaidimas baigtas - AI pralaimejo
                    {
                        this.BeginInvoke(new MethodInvoker(() => { textBox3.AppendText("AI pralaimėjo.\r\n"); }));
                        Debug.Write("pralaimejimas");
                        busena = 3;
                        animacija();
                        zaidimas = false;
                        this.BeginInvoke(new MethodInvoker(() => { button1.Text = "Pradėti"; }));
                        Speliotojas.GautAtsakyma(false, zodis.GautiZodi());
                        //ideti animacija, ar kaip kitaip atvaizduoti pralaimejima
                    }
                }
                //Thread.Sleep(1000);// atspejo/neatspejo animacijai isskirtas laikas
            }
            else
            {
                Debug.Write("sustabdyta");
                busena = 3;
                animacija();
                zaidimas = false;
            }
        }

        bool atidarytasLogas = false;
        Point pozicijaAtidariusLoga = new Point(27, 211);
        Point pozicijaUzdariusLoga = new Point(27, 297);
        private void button3_Click(object sender, EventArgs e)
        {
            if (!atidarytasLogas)
            {
                button3.BackgroundImage = Image.FromFile(".\\..\\..\\src\\grey-button-close.png");
                button3.Location = pozicijaAtidariusLoga;
                textBox3.Visible = true;
                atidarytasLogas = true;
            }
            else
            {
                button3.BackgroundImage = Image.FromFile(".\\..\\..\\src\\grey-button-open.png");
                button3.Location = pozicijaUzdariusLoga;
                textBox3.Visible = false;
                atidarytasLogas = false;
            }
        }
    }
}
