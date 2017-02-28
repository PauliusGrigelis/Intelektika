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
                //Thread.Sleep(2000);
                //Random rng = new Random();
                //busena = rng.Next(0, 3);
                //testavimoZaidimas(zodis);
                ai(zodis);
            }
        }

        private void animacija()
        {
            try
            {

                if (busena == 0) //galvoja
                {
                    pictureBox1.Image = Image.FromFile(".\\..\\..\\src\\ajax-loader.gif");
                }
                else if (busena == 1) //atspejo
                {
                    pictureBox1.Image = Image.FromFile(".\\..\\..\\src\\check2.png");
                }
                else //neatspejo
                {
                    pictureBox1.Image = Image.FromFile(".\\..\\..\\src\\X.gif");
                }
            }
            catch { }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //animacijos ar dar kas
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
                    busena = 1;
                    animacija();
                    this.BeginInvoke(new MethodInvoker(() => { textBox2.Text = zodis.atvaizdavimas(); }));
                }
                else
                {
                    busena = 2;
                    animacija();
                }
                Thread.Sleep(1000);// atspejo/neatspejo animacijai isskirtas laikas
            }
            zaidimas = false;
        }

        private void ai(Zodis zodis)
        {
            List<object> spejimai = new List<object>();
            spejimai.Add(new { raide = 'c', kiekis = 3 });
            spejimai.Add(new {raide = 'a', kiekis = 10 });
            spejimai.Add(new { raide = 'g', kiekis = 1 });
            spejimai.Add(new { raide = 'h', kiekis = 0 });
            spejimai.Add(new { raide = 'v', kiekis = 5 });
            spejimai.Add(new { raide = 'x', kiekis = 0 });
            spejimai.Add(new { raide = 'z', kiekis = 4 });
            spejimai.Add(new { raide = 'l', kiekis = 6 });

            List<object> speta = new List<object>();
            var temp = new
                {
                    raide = 'a',
                    kiekis = 10
                };
            for (int j = 0; j<spejimai.Count; j++)
            {
                for (int x = 0; x < spejimai.Count; x++)
                {
                    while (true)
                    {
                        temp = new
                        {
                            raide = (char)spejimai[j].GetType().GetProperty("raide").GetValue(spejimai[j], null),
                            kiekis = (int)spejimai[j].GetType().GetProperty("kiekis").GetValue(spejimai[j], null)
                        };
                        if (!speta.Contains(temp))
                            break;
                    }
                }
                for (int i = 0; i < spejimai.Count; i++)
                {
                    if (!speta.Contains(spejimai[i]) && temp.kiekis < (int)spejimai[i].GetType().GetProperty("kiekis").GetValue(spejimai[i], null))
                    {
                        temp = new
                        {
                            raide = (char)spejimai[i].GetType().GetProperty("raide").GetValue(spejimai[i], null),
                            kiekis = (int)spejimai[i].GetType().GetProperty("kiekis").GetValue(spejimai[i], null)
                        };
                    }
                }


                busena = 0;
                animacija();
                Thread.Sleep(2000); //tipo galvoja
                if (zodis.spejimas(temp.raide))
                {
                    busena = 1;
                    animacija();
                    this.BeginInvoke(new MethodInvoker(() => { textBox2.Text = zodis.atvaizdavimas(); }));
                }
                else
                {
                    busena = 2;
                    animacija();
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
