namespace NatureOfCodeTest
{
    partial class DistanceForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.pnlDistanceGraph = new System.Windows.Forms.Panel();
            this.pnlControls = new System.Windows.Forms.Panel();
            this.lblXScale = new System.Windows.Forms.Label();
            this.trkXScale = new System.Windows.Forms.TrackBar();
            this.pnlControls.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trkXScale)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlDistanceGraph
            // 
            this.pnlDistanceGraph.BackColor = System.Drawing.Color.Black;
            this.pnlDistanceGraph.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlDistanceGraph.Location = new System.Drawing.Point(0, 0);
            this.pnlDistanceGraph.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.pnlDistanceGraph.Name = "pnlDistanceGraph";
            this.pnlDistanceGraph.Size = new System.Drawing.Size(1600, 750);
            this.pnlDistanceGraph.TabIndex = 0;
            // 
            // pnlControls
            // 
            this.pnlControls.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(20)))));
            this.pnlControls.Controls.Add(this.lblXScale);
            this.pnlControls.Controls.Add(this.trkXScale);
            this.pnlControls.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlControls.Location = new System.Drawing.Point(0, 750);
            this.pnlControls.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.pnlControls.Name = "pnlControls";
            this.pnlControls.Size = new System.Drawing.Size(1600, 115);
            this.pnlControls.TabIndex = 1;
            // 
            // lblXScale
            // 
            this.lblXScale.AutoSize = true;
            this.lblXScale.ForeColor = System.Drawing.Color.White;
            this.lblXScale.Location = new System.Drawing.Point(24, 13);
            this.lblXScale.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lblXScale.Name = "lblXScale";
            this.lblXScale.Size = new System.Drawing.Size(283, 25);
            this.lblXScale.TabIndex = 1;
            this.lblXScale.Text = "X-Axis Scale (Time Window)";
            // 
            // trkXScale
            // 
            this.trkXScale.Location = new System.Drawing.Point(24, 44);
            this.trkXScale.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.trkXScale.Maximum = 730;
            this.trkXScale.Minimum = 30;
            this.trkXScale.Name = "trkXScale";
            this.trkXScale.Size = new System.Drawing.Size(800, 90);
            this.trkXScale.TabIndex = 0;
            this.trkXScale.Value = 365;
            this.trkXScale.Scroll += new System.EventHandler(this.trkXScale_Scroll);
            // 
            // DistanceForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1600, 865);
            this.Controls.Add(this.pnlDistanceGraph);
            this.Controls.Add(this.pnlControls);
            this.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.Name = "DistanceForm";
            this.Text = "Relative Distance to Observer";
            this.Load += new System.EventHandler(this.DistanceForm_Load);
            this.pnlControls.ResumeLayout(false);
            this.pnlControls.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trkXScale)).EndInit();
            this.ResumeLayout(false);

        }

        private System.Windows.Forms.Panel pnlDistanceGraph;
        private System.Windows.Forms.Panel pnlControls;
        private System.Windows.Forms.TrackBar trkXScale;
        private System.Windows.Forms.Label lblXScale;
    }
}
