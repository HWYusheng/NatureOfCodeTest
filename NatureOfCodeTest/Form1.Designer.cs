
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
            this.pnlOrbitalMap.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlOrbitalMap
            // 
            this.pnlOrbitalMap.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlOrbitalMap.BackColor = System.Drawing.Color.Black;
            this.pnlOrbitalMap.Controls.Add(this.lblData);
            this.pnlOrbitalMap.Location = new System.Drawing.Point(6, 6);
            this.pnlOrbitalMap.Margin = new System.Windows.Forms.Padding(2);
            this.pnlOrbitalMap.Name = "pnlOrbitalMap";
            this.pnlOrbitalMap.Size = new System.Drawing.Size(388, 198);
            this.pnlOrbitalMap.TabIndex = 0;
            // 
            // lblData
            // 
            this.lblData.AutoSize = true;
            this.lblData.Location = new System.Drawing.Point(6, 3);
            this.lblData.Name = "lblData";
            this.lblData.Size = new System.Drawing.Size(35, 13);
            this.lblData.TabIndex = 0;
            this.lblData.Text = "label1";
            // 
            // btnShowRV
            // 
            this.btnShowRV.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnShowRV.Location = new System.Drawing.Point(306, 212);
            this.btnShowRV.Margin = new System.Windows.Forms.Padding(2);
            this.btnShowRV.Name = "btnShowRV";
            this.btnShowRV.Size = new System.Drawing.Size(88, 16);
            this.btnShowRV.TabIndex = 1;
            this.btnShowRV.Text = "Show Radial Velocity Graph";
            this.btnShowRV.UseVisualStyleBackColor = true;
            this.btnShowRV.Click += new System.EventHandler(this.btnShowRV_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(400, 234);
            this.Controls.Add(this.btnShowRV);
            this.Controls.Add(this.pnlOrbitalMap);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "Form1";
            this.Text = "Orbital Map";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.pnlOrbitalMap.ResumeLayout(false);
            this.pnlOrbitalMap.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlOrbitalMap;
        private System.Windows.Forms.Button btnShowRV;
        private System.Windows.Forms.Label lblData;
    }
}
