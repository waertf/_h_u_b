namespace hub
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.kmPerHour = new System.Windows.Forms.Label();
            this.speedLabel = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.persent = new System.Windows.Forms.Label();
            this.batteryLabel = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.Task = new System.Windows.Forms.Label();
            this.taskLabel = new System.Windows.Forms.Label();
            this.panel4 = new System.Windows.Forms.Panel();
            this.panel5 = new System.Windows.Forms.Panel();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.kmPerHour);
            this.panel1.Controls.Add(this.speedLabel);
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(400, 200);
            this.panel1.TabIndex = 0;
            // 
            // kmPerHour
            // 
            this.kmPerHour.AutoSize = true;
            this.kmPerHour.Font = new System.Drawing.Font("PMingLiU", 32F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.kmPerHour.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(178)))), ((int)(((byte)(241)))));
            this.kmPerHour.Location = new System.Drawing.Point(295, 81);
            this.kmPerHour.Name = "kmPerHour";
            this.kmPerHour.Size = new System.Drawing.Size(105, 43);
            this.kmPerHour.TabIndex = 1;
            this.kmPerHour.Text = "km/h";
            // 
            // speedLabel
            // 
            this.speedLabel.AutoSize = true;
            this.speedLabel.Font = new System.Drawing.Font("PMingLiU", 150F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.speedLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(178)))), ((int)(((byte)(241)))));
            this.speedLabel.Location = new System.Drawing.Point(-34, 0);
            this.speedLabel.Name = "speedLabel";
            this.speedLabel.Size = new System.Drawing.Size(369, 200);
            this.speedLabel.TabIndex = 0;
            this.speedLabel.Text = "000";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.persent);
            this.panel2.Controls.Add(this.batteryLabel);
            this.panel2.Location = new System.Drawing.Point(0, 200);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(400, 200);
            this.panel2.TabIndex = 1;
            // 
            // persent
            // 
            this.persent.AutoSize = true;
            this.persent.Font = new System.Drawing.Font("PMingLiU", 32F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.persent.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(178)))), ((int)(((byte)(241)))));
            this.persent.Location = new System.Drawing.Point(319, 82);
            this.persent.Name = "persent";
            this.persent.Size = new System.Drawing.Size(54, 43);
            this.persent.TabIndex = 2;
            this.persent.Text = "%";
            // 
            // batteryLabel
            // 
            this.batteryLabel.AutoSize = true;
            this.batteryLabel.Font = new System.Drawing.Font("PMingLiU", 150F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.batteryLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(178)))), ((int)(((byte)(241)))));
            this.batteryLabel.Location = new System.Drawing.Point(-32, 0);
            this.batteryLabel.Name = "batteryLabel";
            this.batteryLabel.Size = new System.Drawing.Size(369, 200);
            this.batteryLabel.TabIndex = 2;
            this.batteryLabel.Text = "000";
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.Task);
            this.panel3.Controls.Add(this.taskLabel);
            this.panel3.Location = new System.Drawing.Point(0, 400);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(400, 200);
            this.panel3.TabIndex = 2;
            // 
            // Task
            // 
            this.Task.AutoSize = true;
            this.Task.Font = new System.Drawing.Font("PMingLiU", 32F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.Task.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(178)))), ((int)(((byte)(241)))));
            this.Task.Location = new System.Drawing.Point(295, 129);
            this.Task.Name = "Task";
            this.Task.Size = new System.Drawing.Size(102, 43);
            this.Task.TabIndex = 4;
            this.Task.Text = "Task";
            // 
            // taskLabel
            // 
            this.taskLabel.AutoSize = true;
            this.taskLabel.Font = new System.Drawing.Font("PMingLiU", 150F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.taskLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(178)))), ((int)(((byte)(241)))));
            this.taskLabel.Location = new System.Drawing.Point(-32, 3);
            this.taskLabel.Name = "taskLabel";
            this.taskLabel.Size = new System.Drawing.Size(369, 200);
            this.taskLabel.TabIndex = 3;
            this.taskLabel.Text = "000";
            // 
            // panel4
            // 
            this.panel4.Location = new System.Drawing.Point(400, 0);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(400, 300);
            this.panel4.TabIndex = 3;
            // 
            // panel5
            // 
            this.panel5.Location = new System.Drawing.Point(400, 300);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(400, 300);
            this.panel5.TabIndex = 4;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(800, 600);
            this.Controls.Add(this.panel5);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Form1";
            this.Text = "Form1";
            this.TopMost = true;
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Label speedLabel;
        private System.Windows.Forms.Label kmPerHour;
        private System.Windows.Forms.Label batteryLabel;
        private System.Windows.Forms.Label persent;
        private System.Windows.Forms.Label Task;
        private System.Windows.Forms.Label taskLabel;

    }
}

