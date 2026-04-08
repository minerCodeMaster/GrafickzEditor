using System;
using System.Drawing;
using System.Windows.Forms;

namespace GrafickzEditor
{
    public partial class FormMain : Form
    {

        // Objekt pro kreslení
        Graphics mobjGrafika;

        //Bod pro uložení souřadnic výchozího bodu přímky
        Point mouseCoords;

        // ---
        // Konstruktor
        // ---

        public FormMain()
        {
            InitializeComponent();
        }

        // ---
        // Default nastavení
        // ---

        private void FormMain_Load(object sender, EventArgs e)
        {
            
            // Inicializace grafiky na PBox
            mobjGrafika = pbPlatno.CreateGraphics();

        }

        private void pbPlatno_MouseMove(object sender, MouseEventArgs e)
        {

            // Výpis souřadnic do status panelu
            statusCoordsLbl.Text = "x: " + e.X + ", y: " + e.Y;

            // Kreslení s podmínkou levého tlačítka
            if (e.Button == MouseButtons.Left)
            {
                mobjGrafika.FillEllipse(Brushes.Black, e.X, e.Y, 10, 10);
            }

        }

        private void pbPlatno_MouseDown(object sender, MouseEventArgs e)
        {

            
            if (e.Button == MouseButtons.Right)
            {
                if (mouseCoords.X == 0 && mouseCoords.Y == 0)
                {
                    mouseCoords.X = e.X; mouseCoords.Y = e.Y;
                } else
                {
                    mobjGrafika.DrawLine(Pens.Black, mouseCoords.X, mouseCoords.Y, e.X, e.Y);
                    mouseCoords.X = 0; mouseCoords.Y = 0;
                }
            }
        }

        private void clearButton_Click(object sender, EventArgs e)
        {
            mobjGrafika.Clear(Color.White);
        }
    }
}
