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
        Color dPrimaryColor;
        Color dSecondaryColor;
        int brushSize;

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
            dPrimaryColor = Color.Black;
            brushSize = 10;
            trackBar1.Value = brushSize;
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
                    //canvasGraphics.DrawLine(getPenFromColor(dPrimaryColor), startPoint, e.Location);

                    float width = (float)brushSize;

                    if (width <= 2)
                    {
                        // Simple line (primary color only)
                        using (Pen pen = new Pen(dPrimaryColor, width))
                        {
                            pen.StartCap = LineCap.Round;
                            pen.EndCap = LineCap.Round;
                            canvasGraphics.DrawLine(pen, startPoint, e.Location);
                        }
                    }
                    else
                    {
                        // Outer line (border - primary color)
                        using (Pen outerPen = new Pen(dPrimaryColor, width))
                        {
                            outerPen.StartCap = LineCap.Round;
                            outerPen.EndCap = LineCap.Round;
                            canvasGraphics.DrawLine(outerPen, startPoint, e.Location);
                        }

                        // Inner line (fill - secondary color, 1px inset on each side)
                        using (Pen innerPen = new Pen(dSecondaryColor, width - 2))
                        {
                            innerPen.StartCap = LineCap.Round;
                            innerPen.EndCap = LineCap.Round;
                            canvasGraphics.DrawLine(innerPen, startPoint, e.Location);
                        }
                    }

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
                canvasGraphics.FillEllipse(getBrushFromColor(dPrimaryColor), e.X, e.Y, (float)brushSize, (float)brushSize);
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
            if (isDrawingLine)
            {
                float width = (float)brushSize;

                if (width <= 2)
                {
                    using (Pen pen = new Pen(dPrimaryColor, width))
                    {
                        pen.StartCap = LineCap.Round;
                        pen.EndCap = LineCap.Round;
                        e.Graphics.DrawLine(pen, startPoint, currentPoint);
                    }
                }
                else
                {
                    using (Pen outerPen = new Pen(dPrimaryColor, width))
                    {
                        outerPen.StartCap = LineCap.Round;
                        outerPen.EndCap = LineCap.Round;
                        e.Graphics.DrawLine(outerPen, startPoint, currentPoint);
                    }

                    using (Pen innerPen = new Pen(dSecondaryColor, width - 2))
                    {
                        innerPen.StartCap = LineCap.Round;
                        innerPen.EndCap = LineCap.Round;
                        e.Graphics.DrawLine(innerPen, startPoint, currentPoint);
                    }
                }
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

            dPrimaryColor = senderButton.BackColor;
            primaryColorBtn.BackColor = dPrimaryColor;
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
                pbPlatno.Image = new Bitmap(openFileDialog1.FileName);
            }
        }

        private void uložitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFileDialog1.Filter = "bmp files (*.bmp)|*.bmp";
            ImageFormat format = ImageFormat.Jpeg;

            if(saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                pbPlatno.Image.Save(saveFileDialog1.FileName, format);
            }
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown numeric = (NumericUpDown)sender;

            if (numeric.Value > trackBar1.Maximum)
            {
                numeric.Value = trackBar1.Maximum;
                trackBar1.Value = trackBar1.Maximum;
                brushSize = trackBar1.Maximum;
                return;
            }

            brushSize = (int)numeric.Value;
            trackBar1.Value = (int)numeric.Value;
        }

        private void secondaryColorBtn_Click(object sender, EventArgs e)
        {
            Button secondaryButton = (Button)sender;

            Button primaryButton = primaryColorBtn;

            Color secondaryColor = secondaryButton.BackColor;

            secondaryColorBtn.BackColor = primaryButton.BackColor;
            dSecondaryColor = primaryButton.BackColor;

            primaryButton.BackColor = secondaryColor;
            dPrimaryColor = secondaryColor;
        }

        private void trackBar1_ValueChanged(object sender, EventArgs e)
        {
            TrackBar numeric = (TrackBar)sender;

            brushSize = numeric.Value;
            numericUpDown1.Value = (decimal)numeric.Value;
        }
    }
}