using System;
using System.Drawing;
using System.Windows.Forms;

namespace GrafickzEditor
{
    public partial class FormMain : Form
    {
        // Bitmap pro uložení canvasu aby se nemazal protože to je trošku trapný co si budem
        Bitmap canvasBitmap;
        Graphics canvasGraphics;

        // Proměnné k přímce
        bool isDrawingLine = false;
        Point startPoint;
        Point currentPoint;

        public FormMain()
        {
            InitializeComponent();
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            // Inicializace Bitmapy na velikost PictureBoxu
            canvasBitmap = new Bitmap(pbPlatno.Width, pbPlatno.Height);
            canvasGraphics = Graphics.FromImage(canvasBitmap);
            canvasGraphics.Clear(Color.White);

            pbPlatno.Image = canvasBitmap;
        }

        private void pbPlatno_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (!isDrawingLine)
                {
                    isDrawingLine = true;
                    startPoint = e.Location;
                    currentPoint = e.Location;
                }
                else
                {
                    canvasGraphics.DrawLine(Pens.Black, startPoint, e.Location);
                    isDrawingLine = false;
                    pbPlatno.Invalidate();
                }
            }
        }

        private void pbPlatno_MouseMove(object sender, MouseEventArgs e)
        {
            statusCoordsLbl.Text = "x: " + e.X + ", y: " + e.Y;

            if (e.Button == MouseButtons.Left)
            {
                // Volnokresba
                canvasGraphics.FillEllipse(Brushes.Black, e.X, e.Y, 10, 10);
                pbPlatno.Invalidate();
            }
            else if (isDrawingLine)
            {
                // Update projekce přímky
                currentPoint = e.Location;
                pbPlatno.Invalidate();
            }
        }

        private void pbPlatno_Paint(object sender, PaintEventArgs e)
        {
            // Projekce přímky
            if (isDrawingLine)
            {
                e.Graphics.DrawLine(Pens.Black, startPoint, currentPoint);
            }
        }

        private void clearButton_Click(object sender, EventArgs e)
        {
            // Absolutní mazání
            canvasGraphics.Clear(Color.White);
            pbPlatno.Invalidate();
        }
    }
}