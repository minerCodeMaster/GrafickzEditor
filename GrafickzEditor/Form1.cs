using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;
using System.Xml.Schema;

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
        Color dColor;
        decimal brushSize;

        public Pen getPenFromColor(Color color)
        {
            if (color == null) { return Pens.Black; }
            return new Pen(color, (float)brushSize);
        }

        public Brush getBrushFromColor(Color color)
        {
            if (color == null) { return new SolidBrush(Color.Black); }
            return new SolidBrush(color);
        }

        public FormMain()
        {
            InitializeComponent();
            dColor = Color.Black;
            brushSize = 10;
            numericUpDown1.Value = brushSize;
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
                    canvasGraphics.DrawLine(getPenFromColor(dColor), startPoint, e.Location);
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
                canvasGraphics.FillEllipse(getBrushFromColor(dColor), e.X, e.Y, (float)brushSize, (float)brushSize);
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
                e.Graphics.DrawLine(getPenFromColor(dColor), startPoint, currentPoint);
            }
        }

        private void clearButton_Click(object sender, EventArgs e)
        {
            // Absolutní mazání
            canvasGraphics.Clear(Color.White);
            pbPlatno.Invalidate();
        }

        private void btnColor_Click(object sender, EventArgs e)
        {
            Button senderButton = (Button)sender;

            dColor = senderButton.BackColor;
        }

        private void novýToolStripMenuItem_Click(object sender, EventArgs e)
        {
            canvasGraphics.Clear(Color.White);
            pbPlatno.Invalidate();
        }

        private void otevřítToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "bmp files (*.bmp)|*.bmp";
            ImageFormat format = ImageFormat.Jpeg;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                //string ext = Path.GetExtension(openFileDialog1.FileName);
                //switch (ext)
                //{
                //    case ".jpg":
                //        format = ImageFormat.Jpeg;
                //        break;
                //    case ".png":
                //        format = ImageFormat.Png;
                //        break;
                //}
                pbPlatno.Image = new Bitmap(openFileDialog1.FileName);
            }
        }

        private void uložitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFileDialog1.Filter = "bmp files (*.bmp)|*.bmp";
            ImageFormat format = ImageFormat.Jpeg;

            if(saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                //string ext = Path.GetExtension(saveFileDialog1.FileName);
                //switch(ext)
                //{
                //    case ".jpg":
                //        format = ImageFormat.Jpeg;
                //        break;
                //    case ".png":
                //        format = ImageFormat.Png;
                //        break;
                //}
                pbPlatno.Image.Save(saveFileDialog1.FileName, format);
            }
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown numeric = (NumericUpDown)sender;

            brushSize = numeric.Value;
        }
    }
}