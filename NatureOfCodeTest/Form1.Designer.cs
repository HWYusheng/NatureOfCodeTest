
namespace NatureOfCodeTest
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.pnlOrbitalMap = new System.Windows.Forms.Panel();
            this.btnShowRV = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // pnlOrbitalMap
            // 
            this.pnlOrbitalMap.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlOrbitalMap.BackColor = System.Drawing.Color.Black;
            this.pnlOrbitalMap.Location = new System.Drawing.Point(12, 12);
            this.pnlOrbitalMap.Name = "pnlOrbitalMap";
            this.pnlOrbitalMap.Size = new System.Drawing.Size(776, 380);
            this.pnlOrbitalMap.TabIndex = 0;
            // 
            // btnShowRV
            // 
            this.btnShowRV.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnShowRV.Location = new System.Drawing.Point(613, 408);
            this.btnShowRV.Name = "btnShowRV";
            this.btnShowRV.Size = new System.Drawing.Size(175, 30);
            this.btnShowRV.TabIndex = 1;
            this.btnShowRV.Text = "Show Radial Velocity Graph";
            this.btnShowRV.UseVisualStyleBackColor = true;
            this.btnShowRV.Click += new System.EventHandler(this.btnShowRV_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.btnShowRV);
            this.Controls.Add(this.pnlOrbitalMap);
            this.Name = "Form1";
            this.Text = "Orbital Map";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlOrbitalMap;
        private System.Windows.Forms.Button btnShowRV;
    }
}
