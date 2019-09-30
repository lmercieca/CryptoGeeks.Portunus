namespace CruptoGeeks.Server.Host
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
            this.gbServerOptions = new System.Windows.Forms.GroupBox();
            this.btnStart = new System.Windows.Forms.Button();
            this.nudPort = new System.Windows.Forms.NumericUpDown();
            this.lblServerPort = new System.Windows.Forms.Label();
            this.dgvDetails = new System.Windows.Forms.DataGridView();
            this.gbServerOptions.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudPort)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDetails)).BeginInit();
            this.SuspendLayout();
            // 
            // gbServerOptions
            // 
            this.gbServerOptions.Controls.Add(this.btnStart);
            this.gbServerOptions.Controls.Add(this.nudPort);
            this.gbServerOptions.Controls.Add(this.lblServerPort);
            this.gbServerOptions.Dock = System.Windows.Forms.DockStyle.Top;
            this.gbServerOptions.Location = new System.Drawing.Point(0, 0);
            this.gbServerOptions.Name = "gbServerOptions";
            this.gbServerOptions.Size = new System.Drawing.Size(452, 71);
            this.gbServerOptions.TabIndex = 0;
            this.gbServerOptions.TabStop = false;
            this.gbServerOptions.Text = "Server Options";
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(182, 27);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(75, 23);
            this.btnStart.TabIndex = 2;
            this.btnStart.Text = "Start Server";
            this.btnStart.UseVisualStyleBackColor = true;
            // 
            // nudPort
            // 
            this.nudPort.Location = new System.Drawing.Point(60, 30);
            this.nudPort.Maximum = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.nudPort.Name = "nudPort";
            this.nudPort.Size = new System.Drawing.Size(104, 20);
            this.nudPort.TabIndex = 1;
            // 
            // lblServerPort
            // 
            this.lblServerPort.AutoSize = true;
            this.lblServerPort.Location = new System.Drawing.Point(28, 32);
            this.lblServerPort.Name = "lblServerPort";
            this.lblServerPort.Size = new System.Drawing.Size(26, 13);
            this.lblServerPort.TabIndex = 0;
            this.lblServerPort.Text = "Port";
            // 
            // dgvDetails
            // 
            this.dgvDetails.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvDetails.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvDetails.Location = new System.Drawing.Point(0, 71);
            this.dgvDetails.Name = "dgvDetails";
            this.dgvDetails.Size = new System.Drawing.Size(452, 155);
            this.dgvDetails.TabIndex = 1;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(452, 226);
            this.Controls.Add(this.dgvDetails);
            this.Controls.Add(this.gbServerOptions);
            this.Name = "Form1";
            this.Text = "Form1";
            this.gbServerOptions.ResumeLayout(false);
            this.gbServerOptions.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudPort)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDetails)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbServerOptions;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.NumericUpDown nudPort;
        private System.Windows.Forms.Label lblServerPort;
        private System.Windows.Forms.DataGridView dgvDetails;
    }
}

