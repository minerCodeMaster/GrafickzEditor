using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace GrafickzEditor
{
    public partial class FormMain : Form
    {
        // Bitmapa = naše plátno (uložený obrázek)
        Bitmap canvasBitmap;

        // Objekt pro kreslení na bitmapu
        Graphics canvasGraphics;

        // Stav kreslení čáry
        bool isDrawingLine = false;

        // Body pro čáru
        Point startPoint;
        Point currentPoint;

        //Bod pro kreslení
        Point lastPoint;

        // Barvy a velikost
        Color primaryColor = Color.Black;
        Color secondaryColor = Color.White;
        int brushSize = 10;

        public FormMain()
        {
            InitializeComponent();

            // Nastavení UI prvků podle výchozí velikosti
            trackBar1.Value = brushSize;
            numericUpDown1.Value = brushSize;
            trackBar1.SetRange(1, 100);
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            // Vytvoření prázdného plátna
            canvasBitmap = new Bitmap(pbPlatno.Width, pbPlatno.Height);
            canvasGraphics = Graphics.FromImage(canvasBitmap);
            canvasGraphics.SmoothingMode = SmoothingMode.AntiAlias;

            // Vyplnění bílou barvou
            canvasGraphics.Clear(Color.White);

            pbPlatno.Image = canvasBitmap;
        }

        // ====== HLAVNÍ KRESLÍCÍ FUNKCE ======
        private void DrawLine(Graphics g, Point start, Point end)
        {
            float width = brushSize;

            if (width <= 2)
            {
                // Jednoduchá čára (jen primary barva)
                using (Pen pen = new Pen(primaryColor, width))
                {
                    pen.StartCap = LineCap.Round;
                    pen.EndCap = LineCap.Round;
                    g.DrawLine(pen, start, end);
                }
            }
            else
            {
                // Vnější čára (primary barva)
                using (Pen outerPen = new Pen(primaryColor, width))
                {
                    outerPen.StartCap = LineCap.Round;
                    outerPen.EndCap = LineCap.Round;
                    g.DrawLine(outerPen, start, end);
                }

                // Vnitřní čára (secondary barva)
                using (Pen innerPen = new Pen(secondaryColor, width - 2))
                {
                    innerPen.StartCap = LineCap.Round;
                    innerPen.EndCap = LineCap.Round;
                    g.DrawLine(innerPen, start, end);
                }
            }
        }

        private void pbPlatno_MouseDown(object sender, MouseEventArgs e)
        {
            // Levé tlačítko pro kreslení
            if (e.Button == MouseButtons.Left)
            {
                lastPoint = e.Location;
            }

            // Pravé tlačítko = kreslení čáry (klik → klik)
            if (e.Button == MouseButtons.Right)
            {
                if (!isDrawingLine)
                {
                    // Začátek čáry
                    isDrawingLine = true;
                    startPoint = e.Location;
                    currentPoint = e.Location;
                }
                else
                {
                    // Konec čáry → vykreslení do bitmapy
                    DrawLine(canvasGraphics, startPoint, e.Location);

                    isDrawingLine = false;
                    pbPlatno.Invalidate();
                }
            }
        }

        private void pbPlatno_MouseMove(object sender, MouseEventArgs e)
        {
            // Zobrazení souřadnic myši
            statusCoordsLbl.Text = $"x: {e.X}, y: {e.Y}";

            // Levé tlačítko = kreslení štětcem (plynulé)
            if (e.Button == MouseButtons.Left)
            {
                float dx = e.X - lastPoint.X;
                float dy = e.Y - lastPoint.Y;

                float distance = (float)Math.Sqrt(dx * dx + dy * dy);

                int steps = (int)(distance / 2f) + 1;

                for (int i = 0; i < steps; i++)
                {
                    float t = (float)i / steps;

                    float x = lastPoint.X + dx * t;
                    float y = lastPoint.Y + dy * t;

                    using (Brush b = new SolidBrush(primaryColor))
                    {
                        canvasGraphics.FillEllipse(b, x, y, brushSize, brushSize);
                    }
                }

                lastPoint = e.Location;
                pbPlatno.Invalidate();
            }
            else if (isDrawingLine)
            {
                // Náhled čáry (tam pořád zůstává inner/outer efekt)
                currentPoint = e.Location;
                pbPlatno.Invalidate();
            }
        }

        private void pbPlatno_Paint(object sender, PaintEventArgs e)
        {
            // Náhled čáry (zatím se nekreslí do bitmapy)
            if (isDrawingLine)
            {
                DrawLine(e.Graphics, startPoint, currentPoint);
            }
        }

        private void clearButton_Click(object sender, EventArgs e)
        {
            // Vymazání plátna
            canvasGraphics.Clear(Color.White);
            pbPlatno.Invalidate();
        }

        private void btnColor_Click(object sender, EventArgs e)
        {
            // Nastavení primární barvy
            Button btn = (Button)sender;

            primaryColor = btn.BackColor;
            primaryColorBtn.BackColor = primaryColor;
        }

        private void secondaryColorBtn_Click(object sender, EventArgs e)
        {
            // Prohození primary a secondary barvy
            Color temp = primaryColor;
            primaryColor = secondaryColor;
            secondaryColor = temp;

            primaryColorBtn.BackColor = primaryColor;
            secondaryColorBtn.BackColor = secondaryColor;
        }

        private void novýToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Nový obrázek
            canvasGraphics.Clear(Color.White);
            pbPlatno.Invalidate();
        }

        private void otevřítToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "Image files (*.bmp;*.jpg)|*.bmp;*.jpg";

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                // Načtení obrázku
                canvasBitmap = new Bitmap(openFileDialog1.FileName);
                canvasGraphics = Graphics.FromImage(canvasBitmap);

                pbPlatno.Image = canvasBitmap;
                pbPlatno.Invalidate();
            }
        }

        private void uložitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFileDialog1.Filter = "JPEG (*.jpg)|*.jpg";

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                canvasBitmap.Save(saveFileDialog1.FileName, ImageFormat.Jpeg);
            }
        }

        private void trackBar1_ValueChanged(object sender, EventArgs e)
        {
            // Změna velikosti štětce (slider)
            brushSize = trackBar1.Value;
            numericUpDown1.Value = brushSize;
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            // Změna velikosti štětce (číselník)
            brushSize = (int)numericUpDown1.Value;
            trackBar1.Value = brushSize;
        }
    }
}