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
        int gyvybes = 5;
        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Length > 0) //input apribojimai
            {
                Zodis zodis = new Zodis(textBox1.Text);
                textBox2.Text = zodis.atvaizdavimas();
                
                //pradedamas zaidimas
                zaidimas = true;
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
                testavimoZaidimas(zodis);
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

        private void testavimoZaidimas(Zodis zodis)
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

            foreach(char c in spejimai)
            {
                busena = 0;
                animacija();
                Thread.Sleep(2000); //tipo galvoja
                if (zodis.spejimas(c))
                {
                    this.BeginInvoke(new MethodInvoker(() => { textBox2.Text = zodis.atvaizdavimas(); }));
                    if (!zodis.arAtspejoZodi())
                    {
                        busena = 1;
                        animacija();
                        //ka pasakyti ai?
                    }
                    else //zaidimas baigtas, AI laimejo
                    {
                        Debug.Write("pergale");
                        zaidimas = false;
                        //ideti animacija, ar kaip kitaip atvaizduoti pergale
                        //duomenu irasymas?
                        break;
                    }
                }
                else
                {
                    gyvybes--;
                    this.BeginInvoke(new MethodInvoker(() => { label3.Text = gyvybes.ToString(); }));
                    if (gyvybes != 0) //zaidimas baigtas - AI pralaimejo
                    {
                        busena = 2;
                        animacija();
                    }
                    else
                    {
                        Debug.Write("pralaimejimas");
                        zaidimas = false;
                        //ideti animacija, ar kaip kitaip atvaizduoti pralaimejima
                        //duomenu irasymas?
                        break;
                    }
                }
                Thread.Sleep(1000);// atspejo/neatspejo animacijai isskirtas laikas
            }
            zaidimas = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //Speliotojas.Speliotojas.gautAtsakyma(false,"bezdžionė"); //cia bandziau ar veikia gerai, ats kaip ir gerai
            Speliotojas.Speliotojas.gautAtsakyma(false,textBox1.Text);
        }
    }
}
