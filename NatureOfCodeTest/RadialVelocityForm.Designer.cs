namespace NatureOfCodeTest
{
    partial class RadialVelocityForm
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
            this.pnlRVGraph = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // pnlRVGraph
            // 
            this.pnlRVGraph.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlRVGraph.Location = new System.Drawing.Point(0, 0);
            this.pnlRVGraph.Name = "pnlRVGraph";
            this.pnlRVGraph.Size = new System.Drawing.Size(800, 450);
            this.pnlRVGraph.TabIndex = 0;
            this.pnlRVGraph.BackColor = System.Drawing.Color.Black;
            // 
            // RadialVelocityForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.pnlRVGraph);
            this.Name = "RadialVelocityForm";
            this.Text = "Radial Velocity Graph";
            this.Load += new System.EventHandler(this.RadialVelocityForm_Load);
            this.ResumeLayout(false);

        }

        public System.Windows.Forms.Panel pnlRVGraph;
    }
}
