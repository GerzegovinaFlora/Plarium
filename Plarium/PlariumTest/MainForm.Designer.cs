namespace Plarium
{
    using Plarium.Resources;

    partial class MainForm
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.FolderForWorkButton = new System.Windows.Forms.Button();
            this.ResultTreeView = new System.Windows.Forms.TreeView();
            this.WorkFolderPath = new System.Windows.Forms.Label();
            this.SaveFolderPath = new System.Windows.Forms.Label();
            this.StartButton = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // FolderForWorkButton
            // 
            this.FolderForWorkButton.Location = new System.Drawing.Point(221, 23);
            this.FolderForWorkButton.Name = "FolderForWorkButton";
            this.FolderForWorkButton.Size = new System.Drawing.Size(81, 23);
            this.FolderForWorkButton.TabIndex = 0;
            this.FolderForWorkButton.Text = "Select";
            this.FolderForWorkButton.UseVisualStyleBackColor = true;
            this.FolderForWorkButton.Click += new System.EventHandler(this.OnFolderForWorkButtonClick);
            // 
            // ResultTreeView
            // 
            this.ResultTreeView.Location = new System.Drawing.Point(12, 83);
            this.ResultTreeView.Name = "ResultTreeView";
            this.ResultTreeView.Size = new System.Drawing.Size(290, 164);
            this.ResultTreeView.TabIndex = 2;
            // 
            // WorkFolderPath
            // 
            this.WorkFolderPath.AutoSize = true;
            this.WorkFolderPath.Location = new System.Drawing.Point(311, 28);
            this.WorkFolderPath.Name = "WorkFolderPath";
            this.WorkFolderPath.Size = new System.Drawing.Size(0, 13);
            this.WorkFolderPath.TabIndex = 6;
            // 
            // StartButton
            // 
            this.StartButton.Enabled = false;
            this.StartButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.StartButton.Location = new System.Drawing.Point(12, 51);
            this.StartButton.Name = "StartButton";
            this.StartButton.Size = new System.Drawing.Size(78, 26);
            this.StartButton.TabIndex = 8;
            this.StartButton.Text = "Strart";
            this.StartButton.UseVisualStyleBackColor = true;
            this.StartButton.Click += new System.EventHandler(this.OnStartButtonClick);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(12, 25);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(206, 20);
            this.textBox1.TabIndex = 9;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(312, 257);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.StartButton);
            this.Controls.Add(this.SaveFolderPath);
            this.Controls.Add(this.WorkFolderPath);
            this.Controls.Add(this.ResultTreeView);
            this.Controls.Add(this.FolderForWorkButton);
            this.Name = "MainForm";
            this.Text = "Главная страница";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button FolderForWorkButton;
        private System.Windows.Forms.TreeView ResultTreeView;
        private System.Windows.Forms.Label WorkFolderPath;
        private System.Windows.Forms.Label SaveFolderPath;
        private System.Windows.Forms.Button StartButton;
        private System.Windows.Forms.TextBox textBox1;

    }
}

