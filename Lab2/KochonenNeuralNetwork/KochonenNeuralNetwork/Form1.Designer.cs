﻿
namespace KochonenNeuralNetwork
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
            this.GenerateDataButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.InitStudySpeed = new System.Windows.Forms.NumericUpDown();
            this.EpochesToDecreaseStudySpeed = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.EpochesToReduceROI = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.ResetNetworkButton = new System.Windows.Forms.Button();
            this.StartButton = new System.Windows.Forms.Button();
            this.Plot = new ScottPlot.FormsPlot();
            ((System.ComponentModel.ISupportInitialize)(this.InitStudySpeed)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.EpochesToDecreaseStudySpeed)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.EpochesToReduceROI)).BeginInit();
            this.SuspendLayout();
            // 
            // GenerateDataButton
            // 
            this.GenerateDataButton.Location = new System.Drawing.Point(12, 12);
            this.GenerateDataButton.Name = "GenerateDataButton";
            this.GenerateDataButton.Size = new System.Drawing.Size(304, 23);
            this.GenerateDataButton.TabIndex = 0;
            this.GenerateDataButton.Text = "Generate Data";
            this.GenerateDataButton.UseVisualStyleBackColor = true;
            this.GenerateDataButton.Click += new System.EventHandler(this.GenerateDataButton_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 82);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(98, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Initial Study Speed:";
            // 
            // InitStudySpeed
            // 
            this.InitStudySpeed.DecimalPlaces = 5;
            this.InitStudySpeed.Location = new System.Drawing.Point(196, 80);
            this.InitStudySpeed.Name = "InitStudySpeed";
            this.InitStudySpeed.Size = new System.Drawing.Size(120, 20);
            this.InitStudySpeed.TabIndex = 2;
            this.InitStudySpeed.Value = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            // 
            // EpochesToDecreaseStudySpeed
            // 
            this.EpochesToDecreaseStudySpeed.Location = new System.Drawing.Point(195, 106);
            this.EpochesToDecreaseStudySpeed.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.EpochesToDecreaseStudySpeed.Name = "EpochesToDecreaseStudySpeed";
            this.EpochesToDecreaseStudySpeed.Size = new System.Drawing.Size(120, 20);
            this.EpochesToDecreaseStudySpeed.TabIndex = 4;
            this.EpochesToDecreaseStudySpeed.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 108);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(177, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Epoches to Decrease Study Speed:";
            // 
            // EpochesToReduceROI
            // 
            this.EpochesToReduceROI.Location = new System.Drawing.Point(196, 132);
            this.EpochesToReduceROI.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.EpochesToReduceROI.Name = "EpochesToReduceROI";
            this.EpochesToReduceROI.Size = new System.Drawing.Size(120, 20);
            this.EpochesToReduceROI.TabIndex = 6;
            this.EpochesToReduceROI.Value = new decimal(new int[] {
            25,
            0,
            0,
            0});
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 134);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(127, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Epoches to Reduce ROI:";
            // 
            // ResetNetworkButton
            // 
            this.ResetNetworkButton.Location = new System.Drawing.Point(15, 158);
            this.ResetNetworkButton.Name = "ResetNetworkButton";
            this.ResetNetworkButton.Size = new System.Drawing.Size(301, 23);
            this.ResetNetworkButton.TabIndex = 7;
            this.ResetNetworkButton.Text = "Reset Network";
            this.ResetNetworkButton.UseVisualStyleBackColor = true;
            this.ResetNetworkButton.Click += new System.EventHandler(this.ResetNetworkButton_Click);
            // 
            // StartButton
            // 
            this.StartButton.Location = new System.Drawing.Point(15, 187);
            this.StartButton.Name = "StartButton";
            this.StartButton.Size = new System.Drawing.Size(301, 23);
            this.StartButton.TabIndex = 8;
            this.StartButton.Text = "Start";
            this.StartButton.UseVisualStyleBackColor = true;
            this.StartButton.Click += new System.EventHandler(this.StartButton_Click);
            // 
            // Plot
            // 
            this.Plot.Location = new System.Drawing.Point(339, 12);
            this.Plot.Name = "Plot";
            this.Plot.Size = new System.Drawing.Size(449, 426);
            this.Plot.TabIndex = 9;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.Plot);
            this.Controls.Add(this.StartButton);
            this.Controls.Add(this.ResetNetworkButton);
            this.Controls.Add(this.EpochesToReduceROI);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.EpochesToDecreaseStudySpeed);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.InitStudySpeed);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.GenerateDataButton);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.InitStudySpeed)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.EpochesToDecreaseStudySpeed)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.EpochesToReduceROI)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button GenerateDataButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown InitStudySpeed;
        private System.Windows.Forms.NumericUpDown EpochesToDecreaseStudySpeed;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown EpochesToReduceROI;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button ResetNetworkButton;
        private System.Windows.Forms.Button StartButton;
        private ScottPlot.FormsPlot Plot;
    }
}

