namespace AccountGeneratorDiscord
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.StartRegisterButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // StartRegisterButton
            // 
            this.StartRegisterButton.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.StartRegisterButton.Location = new System.Drawing.Point(211, 361);
            this.StartRegisterButton.Name = "StartRegisterButton";
            this.StartRegisterButton.Size = new System.Drawing.Size(303, 61);
            this.StartRegisterButton.TabIndex = 0;
            this.StartRegisterButton.Text = "Start Registering";
            this.StartRegisterButton.UseVisualStyleBackColor = true;
            this.StartRegisterButton.Click += new System.EventHandler(this.StartRegisterButton_clickAsync);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(796, 450);
            this.Controls.Add(this.StartRegisterButton);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private Button StartRegisterButton;
    }
}