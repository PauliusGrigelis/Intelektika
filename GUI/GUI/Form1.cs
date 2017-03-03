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
        int busena = 0;
        int gyvybes;
        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Length > 0) //input apribojimai
            {
                Zodis zodis = new Zodis(textBox1.Text);
                textBox2.Text = zodis.atvaizdavimas();
                
                //pradedamas zaidimas
                zaidimas = true;
                gyvybes = 5;
                label3.Text = gyvybes.ToString();
                Task zaisti = new Task(() => pradeti(zodis));
                //Task animuoti = new Task(() => animacija());
                zaisti.Start();
                //animuoti.Start();
            }
            else
            {
                //ka daryt jei netinkamai ivestas zodis
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
                    pictureBox2.Image = Image.FromFile(".\\..\\..\\src\\Homer_simpsonwoohooo.gif");
                }
                else //neatspejo
                {
                    this.BeginInvoke(new MethodInvoker(() => { pictureBox1.Visible = false; }));
                    pictureBox2.Image = Image.FromFile(".\\..\\..\\src\\Homer_simpsondoh.png");
                }
            }
            catch { }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            pictureBox1.Image = Image.FromFile(".\\..\\..\\src\\ajax-loader.gif");
            pictureBox1.Visible = false;
        }

        private void apdorojamasSpejimas(Zodis zodis, char spejimas)
        {
            busena = 0;
            animacija();
            Thread.Sleep(2000); //tipo galvoja
            if (zodis.spejimas(spejimas))
            {
                this.BeginInvoke(new MethodInvoker(() => { textBox2.Text = zodis.atvaizdavimas(); }));
                this.BeginInvoke(new MethodInvoker(() => { textBox3.AppendText("Atspėjo: " + spejimas + "\r\n"); }));
                if (!zodis.arAtspejoZodi())
                {
                    busena = 1;
                    animacija();
                    //ka pasakyti ai?
                }
                else //zaidimas baigtas, AI laimejo
                {
                    this.BeginInvoke(new MethodInvoker(() => { textBox3.AppendText("AI laimėjo.\r\n"); }));
                    Debug.Write("pergale");
                    zaidimas = false;
                    //ideti animacija, ar kaip kitaip atvaizduoti pergale
                    //duomenu irasymas?
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
                }
                else
                {
                    this.BeginInvoke(new MethodInvoker(() => { textBox3.AppendText("AI pralaimėjo.\r\n"); }));
                    Debug.Write("pralaimejimas");
                    zaidimas = false;
                    //ideti animacija, ar kaip kitaip atvaizduoti pralaimejima
                    //duomenu irasymas?
                }
            }
            Thread.Sleep(1000);// atspejo/neatspejo animacijai isskirtas laikas
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
