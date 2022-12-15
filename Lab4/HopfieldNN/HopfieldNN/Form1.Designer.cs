
namespace HopfieldNN
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
            this.SelectTrainDataFolderButton = new System.Windows.Forms.Button();
            this.TrainButton = new System.Windows.Forms.Button();
            this.TestButton = new System.Windows.Forms.Button();
            this.OutputPictureBox = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.OutputPictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // SelectTrainDataFolderButton
            // 
            this.SelectTrainDataFolderButton.Location = new System.Drawing.Point(12, 12);
            this.SelectTrainDataFolderButton.Name = "SelectTrainDataFolderButton";
            this.SelectTrainDataFolderButton.Size = new System.Drawing.Size(150, 23);
            this.SelectTrainDataFolderButton.TabIndex = 0;
            this.SelectTrainDataFolderButton.Text = "Select Train Data Folder";
            this.SelectTrainDataFolderButton.UseVisualStyleBackColor = true;
            this.SelectTrainDataFolderButton.Click += new System.EventHandler(this.SelectTrainDataFolderButton_Click);
            // 
            // TrainButton
            // 
            this.TrainButton.Location = new System.Drawing.Point(12, 41);
            this.TrainButton.Name = "TrainButton";
            this.TrainButton.Size = new System.Drawing.Size(150, 23);
            this.TrainButton.TabIndex = 1;
            this.TrainButton.Text = "Train";
            this.TrainButton.UseVisualStyleBackColor = true;
            this.TrainButton.Click += new System.EventHandler(this.TrainButton_Click);
            // 
            // TestButton
            // 
            this.TestButton.Location = new System.Drawing.Point(12, 70);
            this.TestButton.Name = "TestButton";
            this.TestButton.Size = new System.Drawing.Size(150, 23);
            this.TestButton.TabIndex = 2;
            this.TestButton.Text = "Test";
            this.TestButton.UseVisualStyleBackColor = true;
            this.TestButton.Click += new System.EventHandler(this.TestButton_Click);
            // 
            // OutputPictureBox
            // 
            this.OutputPictureBox.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.OutputPictureBox.Location = new System.Drawing.Point(168, 12);
            this.OutputPictureBox.Name = "OutputPictureBox";
            this.OutputPictureBox.Size = new System.Drawing.Size(224, 224);
            this.OutputPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.OutputPictureBox.TabIndex = 3;
            this.OutputPictureBox.TabStop = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(406, 248);
            this.Controls.Add(this.OutputPictureBox);
            this.Controls.Add(this.TestButton);
            this.Controls.Add(this.TrainButton);
            this.Controls.Add(this.SelectTrainDataFolderButton);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.OutputPictureBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button SelectTrainDataFolderButton;
        private System.Windows.Forms.Button TrainButton;
        private System.Windows.Forms.Button TestButton;
        private System.Windows.Forms.PictureBox OutputPictureBox;
    }
}

