using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Traffilogg
{
    public partial class Form1 : Form
    {
        public Form1()
        {
 
            InitializeComponent();
            
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            string line="";
            string preLine="";
            string[] names = textBox2.Text.Split(',');


            // Read the file and display it line by line.  
            System.IO.StreamReader file = new System.IO.StreamReader(openFileDialog1.FileName);
            int money = 0,
                totalspeed = 0,
                counter = 0,
                maxmoney = 0,
                maxspeed=0;

            while ((line = file.ReadLine()) != null)
            {
                bool linecontains = false;
                bool prelinecontains = false;
                foreach (string name in names) {
                    if (line.Contains(name)) {
                        linecontains = true;
                        break;
                    }
                    if (preLine.Contains(name))
                    {
                        prelinecontains = true;
                        break;
                    }
                }
                
                if (line.Contains("[SeeMTA - Traffipax]:") && linecontains && line.EndsWith("$"))
                {
                    string[] splitted = line.Split(' ');
                    int currentmoney = int.Parse(splitted[splitted.Length - 1].Replace("$", ""));
                    money += currentmoney;

                    if (currentmoney > maxmoney) { maxmoney = currentmoney; }

                    counter++;

                }

                
                if (line.Contains("[SeeMTA - Traffipax]:") && prelinecontains && line.EndsWith("KM/h"))
                {
                    string[] splitted = line.Split(' ');
                    int limit = int.Parse(splitted[splitted.Length - 1].Replace("KM/h", ""));
                    int speed = int.Parse(splitted[splitted.Length - 4].Split('.')[0]);

                    totalspeed += speed - limit;

                    if (speed > maxspeed) { maxspeed = speed; }
                }
                preLine = line;
            }

            file.Close();
            if (counter == 0) {
                MessageBox.Show("Nem található rekord a megadott paraméterekkel.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            textBox1.Text = "";
            textBox1.Text +=    "Büntetések száma: " + counter + Environment.NewLine +
                                "Büntetések összege: " + money + "$" + Environment.NewLine +
                                "Legmagasabb büntetés: " + maxmoney + "$" + Environment.NewLine +
                                "Legmagasabb sebesség: " + maxspeed.ToString("#.##") + "KM/h" + Environment.NewLine +
                                "Átlagos büntetés: " + ((double)money / counter).ToString("#.##") + "$" + Environment.NewLine +
                                "Átlagos sebességhatár átlépés: " + ((double)totalspeed / counter).ToString("#.##") + "KM/h";


        }


        private void Form1_Load(object sender, EventArgs e)
        {
            MaximizeBox = false;
            MinimizeBox = false;
            

            string pathName = "";

            RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\\WOW6432Node\\Multi Theft Auto: San Andreas All\\1.5");
            if (registryKey != null)
            {
                pathName = (string)registryKey.GetValue("Last Run Location") + "/MTA/logs";
            }
            else
            {
                pathName = "c:\\";
            }

            this.openFileDialog1.InitialDirectory = pathName;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

            if (textBox2.Text.Equals(""))
            {
                MessageBox.Show("Adj meg legalább egy nevet!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else {
                openFileDialog1.ShowDialog();
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
