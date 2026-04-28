namespace GrafickzEditor
{
    partial class FormMain
    {
        /// <summary>
        /// Vyžaduje se proměnná návrháře.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Uvolněte všechny používané prostředky.
        /// </summary>
        /// <param name="disposing">hodnota true, když by se měl spravovaný prostředek odstranit; jinak false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Kód generovaný Návrhářem Windows Form

        /// <summary>
        /// Metoda vyžadovaná pro podporu Návrháře - neupravovat
        /// obsah této metody v editoru kódu.
        /// </summary>
        private void InitializeComponent()
        {
            this.pbPlatno = new System.Windows.Forms.PictureBox();
            this.clearButton = new System.Windows.Forms.Button();
            this.statusCoords = new System.Windows.Forms.StatusStrip();
            this.statusCoordsLbl = new System.Windows.Forms.ToolStripStatusLabel();
            ((System.ComponentModel.ISupportInitialize)(this.pbPlatno)).BeginInit();
            this.statusCoords.SuspendLayout();
            this.SuspendLayout();
            // 
            // pbPlatno
            // 
            this.pbPlatno.BackColor = System.Drawing.Color.White;
            this.pbPlatno.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pbPlatno.Location = new System.Drawing.Point(108, 12);
            this.pbPlatno.Name = "pbPlatno";
            this.pbPlatno.Size = new System.Drawing.Size(553, 413);
            this.pbPlatno.TabIndex = 0;
            this.pbPlatno.TabStop = false;
            this.pbPlatno.Paint += new System.Windows.Forms.PaintEventHandler(this.pbPlatno_Paint);
            this.pbPlatno.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pbPlatno_MouseDown);
            this.pbPlatno.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pbPlatno_MouseMove);
            // 
            // clearButton
            // 
            this.clearButton.Location = new System.Drawing.Point(12, 12);
            this.clearButton.Name = "clearButton";
            this.clearButton.Size = new System.Drawing.Size(90, 29);
            this.clearButton.TabIndex = 1;
            this.clearButton.Text = "CLear";
            this.clearButton.UseVisualStyleBackColor = true;
            this.clearButton.Click += new System.EventHandler(this.clearButton_Click);
            // 
            // statusCoords
            // 
            this.statusCoords.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusCoordsLbl});
            this.statusCoords.Location = new System.Drawing.Point(0, 428);
            this.statusCoords.Name = "statusCoords";
            this.statusCoords.Size = new System.Drawing.Size(769, 22);
            this.statusCoords.TabIndex = 2;
            this.statusCoords.Text = "statusStrip1";
            // 
            // statusCoordsLbl
            // 
            this.statusCoordsLbl.Name = "statusCoordsLbl";
            this.statusCoordsLbl.Size = new System.Drawing.Size(85, 17);
            this.statusCoordsLbl.Text = "Mouse coords:";
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(769, 450);
            this.Controls.Add(this.statusCoords);
            this.Controls.Add(this.clearButton);
            this.Controls.Add(this.pbPlatno);
            this.Name = "FormMain";
            this.Text = "Okno is calling";
            this.Load += new System.EventHandler(this.FormMain_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pbPlatno)).EndInit();
            this.statusCoords.ResumeLayout(false);
            this.statusCoords.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pbPlatno;
        private System.Windows.Forms.Button clearButton;
        private System.Windows.Forms.StatusStrip statusCoords;
        private System.Windows.Forms.ToolStripStatusLabel statusCoordsLbl;
    }
}

