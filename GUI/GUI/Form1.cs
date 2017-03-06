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
using Speliotojas;
using System.Diagnostics;
using System.Media;

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
        bool zaidimas = false;
        bool sustabdyta = false;
        int busena = 3;
        int gyvybes;
        private void button1_Click(object sender, EventArgs e)
        {
            if (button1.Text == "Atšaukti")
            {
                button1.Text = "Pradėti";
                sustabdyta = true;
            }
            else
            {
                button1.Text = "Atšaukti";
                sustabdyta = false;
                if (textBox1.Text.Length > 0) //input apribojimai
                {
                    this.BeginInvoke(new MethodInvoker(() => { pictureBox3.Visible = false; }));
                    this.BeginInvoke(new MethodInvoker(() => { pictureBox4.Visible = false; }));
                    this.BeginInvoke(new MethodInvoker(() => { pictureBox1.Visible = true; }));
                    this.BeginInvoke(new MethodInvoker(() => { pictureBox2.Visible = true; }));
                    Zodis zodis = new Zodis(textBox1.Text.ToLower());
                    textBox2.Text = zodis.atvaizdavimas();

                    //pradedamas zaidimas
                    zaidimas = true;
                    gyvybes = 5;
                    label3.Text = gyvybes.ToString();
                    Task zaisti = new Task(() => pradeti(zodis));
                    zaisti.Start();
                }
                else
                {
                    //ka daryt jei netinkamai ivestas zodis
                }
            }
        }

        private void pradeti(Zodis zodis)
        {
            while (zaidimas)
            {
                apdorojamasSpejimas(zodis, Speliotojas.Speliotojas.spekRaide());
                //apdorojamasSpejimas(zodis, testavimoZaidimas());
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
                if (zodis.spejimas(spejimas))
                {
                    this.BeginInvoke(new MethodInvoker(() => { textBox2.Text = zodis.atvaizdavimas(); }));
                    this.BeginInvoke(new MethodInvoker(() => { textBox3.AppendText("Atspėjo: " + spejimas + "\r\n"); }));
                    if (!zodis.arAtspejoZodi())
                    {
                        busena = 1;
                        animacija();
                        Speliotojas.Speliotojas.RaidesAtspejimoSekme(true, spejimas);
                        //ka pasakyti ai?
                    }
                    else //zaidimas baigtas, AI laimejo
                    {
                        this.BeginInvoke(new MethodInvoker(() => { textBox3.AppendText("AI laimėjo.\r\n"); }));
                        Debug.Write("pergale");
                        busena = 3;
                        animacija();
                        zaidimas = false;
                        this.BeginInvoke(new MethodInvoker(() => { button1.Text = "Pradėti"; }));
                        Speliotojas.Speliotojas.gautAtsakyma(true, zodis.gautiZodi());
                        //ideti animacija, ar kaip kitaip atvaizduoti pergale
                    }
                }
                else
                {
                    gyvybes--;
                    this.BeginInvoke(new MethodInvoker(() => { label3.Text = gyvybes.ToString(); }));
                    this.BeginInvoke(new MethodInvoker(() => { textBox3.AppendText("Neatspėjo: " + spejimas + "\r\n"); }));
                    if (gyvybes != 0) //zaidimas baigtas - AI pralaimejo
                    {
                        busena = 2;
                        animacija();
                        Speliotojas.Speliotojas.RaidesAtspejimoSekme(false, spejimas);
                    }
                    else
                    {
                        this.BeginInvoke(new MethodInvoker(() => { textBox3.AppendText("AI pralaimėjo.\r\n"); }));
                        Debug.Write("pralaimejimas");
                        busena = 3;
                        animacija();
                        zaidimas = false;
                        this.BeginInvoke(new MethodInvoker(() => { button1.Text = "Pradėti"; }));
                        Speliotojas.Speliotojas.gautAtsakyma(false, zodis.gautiZodi());
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


        List<char> speta = new List<char>(); //testavimui
        private char testavimoZaidimas() //nesamone, bet tik testavimui
        {
            List<char> spejimai = new List<char>();
            spejimai.Add('a');
            spejimai.Add('c');
            spejimai.Add('g');
            spejimai.Add('h');
            spejimai.Add('v');
            spejimai.Add('x');
            spejimai.Add('z');
            spejimai.Add('l');
            foreach (char c in spejimai)
                if (!speta.Contains(c)) { speta.Add(c); return c; }
            return 'o';
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
