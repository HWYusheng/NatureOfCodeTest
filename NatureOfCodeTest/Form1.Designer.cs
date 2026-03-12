
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
            this.lblData = new System.Windows.Forms.Label();
            this.btnShowRV = new System.Windows.Forms.Button();
            this.btnShowDistance = new System.Windows.Forms.Button();
            this.trkSpeed = new System.Windows.Forms.TrackBar();
            this.lblSpeed = new System.Windows.Forms.Label();
            this.pnlOrbitalMap.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trkSpeed)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlOrbitalMap
            // 
            this.pnlOrbitalMap.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlOrbitalMap.BackColor = System.Drawing.Color.Black;
            this.pnlOrbitalMap.Controls.Add(this.lblData);
            this.pnlOrbitalMap.Location = new System.Drawing.Point(12, 12);
            this.pnlOrbitalMap.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.pnlOrbitalMap.Name = "pnlOrbitalMap";
            this.pnlOrbitalMap.Size = new System.Drawing.Size(936, 381);
            this.pnlOrbitalMap.TabIndex = 0;
            // 
            // lblData
            // 
            this.lblData.AutoSize = true;
            this.lblData.Location = new System.Drawing.Point(12, 6);
            this.lblData.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lblData.Name = "lblData";
            this.lblData.Size = new System.Drawing.Size(70, 25);
            this.lblData.TabIndex = 0;
            this.lblData.Text = "label1";
            // 
            // btnShowRV
            // 
            this.btnShowRV.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnShowRV.Location = new System.Drawing.Point(540, 462);
            this.btnShowRV.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.btnShowRV.Name = "btnShowRV";
            this.btnShowRV.Size = new System.Drawing.Size(150, 44);
            this.btnShowRV.TabIndex = 5;
            this.btnShowRV.Text = "Show RV Graph";
            this.btnShowRV.UseVisualStyleBackColor = true;
            this.btnShowRV.Click += new System.EventHandler(this.btnShowRV_Click);
            // 
            // btnShowDistance
            // 
            this.btnShowDistance.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnShowDistance.Location = new System.Drawing.Point(700, 462);
            this.btnShowDistance.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnShowDistance.Name = "btnShowDistance";
            this.btnShowDistance.Size = new System.Drawing.Size(248, 44);
            this.btnShowDistance.TabIndex = 4;
            this.btnShowDistance.Text = "Show Distance Graph";
            this.btnShowDistance.UseVisualStyleBackColor = true;
            this.btnShowDistance.Click += new System.EventHandler(this.btnShowDistance_Click);
            // 
            // trkSpeed
            // 
            this.trkSpeed.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.trkSpeed.Location = new System.Drawing.Point(12, 456);
            this.trkSpeed.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.trkSpeed.Maximum = 50;
            this.trkSpeed.Minimum = 1;
            this.trkSpeed.Name = "trkSpeed";
            this.trkSpeed.Size = new System.Drawing.Size(600, 90);
            this.trkSpeed.TabIndex = 2;
            this.trkSpeed.Value = 10;
            this.trkSpeed.Scroll += new System.EventHandler(this.trkSpeed_Scroll);
            // 
            // lblSpeed
            // 
            this.lblSpeed.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblSpeed.AutoSize = true;
            this.lblSpeed.Location = new System.Drawing.Point(12, 427);
            this.lblSpeed.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lblSpeed.Name = "lblSpeed";
            this.lblSpeed.Size = new System.Drawing.Size(299, 25);
            this.lblSpeed.TabIndex = 3;
            this.lblSpeed.Text = "Simulation Speed (Days/Step)";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(960, 538);
            this.Controls.Add(this.btnShowDistance);
            this.Controls.Add(this.lblSpeed);
            this.Controls.Add(this.trkSpeed);
            this.Controls.Add(this.btnShowRV);
            this.Controls.Add(this.pnlOrbitalMap);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "Form1";
            this.Text = "Orbital Map";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.pnlOrbitalMap.ResumeLayout(false);
            this.pnlOrbitalMap.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trkSpeed)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel pnlOrbitalMap;
        private System.Windows.Forms.Button btnShowRV;
        private System.Windows.Forms.Button btnShowDistance;
        private System.Windows.Forms.Label lblData;
        private System.Windows.Forms.TrackBar trkSpeed;
        private System.Windows.Forms.Label lblSpeed;
    }
}
