namespace HiSum.Forms
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
            this.btnGetURL = new System.Windows.Forms.Button();
            this.tbURL = new System.Windows.Forms.TextBox();
            this.tbResponse = new System.Windows.Forms.TextBox();
            this.btnTop100 = new System.Windows.Forms.Button();
            this.btnGetItem = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnGetURL
            // 
            this.btnGetURL.Location = new System.Drawing.Point(568, 35);
            this.btnGetURL.Name = "btnGetURL";
            this.btnGetURL.Size = new System.Drawing.Size(75, 23);
            this.btnGetURL.TabIndex = 0;
            this.btnGetURL.Text = "Get URL";
            this.btnGetURL.UseVisualStyleBackColor = true;
            this.btnGetURL.Click += new System.EventHandler(this.btnGetURL_Click);
            // 
            // tbURL
            // 
            this.tbURL.Location = new System.Drawing.Point(75, 35);
            this.tbURL.Name = "tbURL";
            this.tbURL.Size = new System.Drawing.Size(443, 20);
            this.tbURL.TabIndex = 1;
            // 
            // tbResponse
            // 
            this.tbResponse.Location = new System.Drawing.Point(75, 93);
            this.tbResponse.Multiline = true;
            this.tbResponse.Name = "tbResponse";
            this.tbResponse.Size = new System.Drawing.Size(779, 295);
            this.tbResponse.TabIndex = 2;
            // 
            // btnTop100
            // 
            this.btnTop100.Location = new System.Drawing.Point(672, 35);
            this.btnTop100.Name = "btnTop100";
            this.btnTop100.Size = new System.Drawing.Size(75, 23);
            this.btnTop100.TabIndex = 3;
            this.btnTop100.Text = "Get Top 100";
            this.btnTop100.UseVisualStyleBackColor = true;
            this.btnTop100.Click += new System.EventHandler(this.btnTop100_Click);
            // 
            // btnGetItem
            // 
            this.btnGetItem.Location = new System.Drawing.Point(769, 35);
            this.btnGetItem.Name = "btnGetItem";
            this.btnGetItem.Size = new System.Drawing.Size(75, 23);
            this.btnGetItem.TabIndex = 4;
            this.btnGetItem.Text = "Get Item";
            this.btnGetItem.UseVisualStyleBackColor = true;
            this.btnGetItem.Click += new System.EventHandler(this.btnGetItem_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(889, 435);
            this.Controls.Add(this.btnGetItem);
            this.Controls.Add(this.btnTop100);
            this.Controls.Add(this.tbResponse);
            this.Controls.Add(this.tbURL);
            this.Controls.Add(this.btnGetURL);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnGetURL;
        private System.Windows.Forms.TextBox tbURL;
        private System.Windows.Forms.TextBox tbResponse;
        private System.Windows.Forms.Button btnTop100;
        private System.Windows.Forms.Button btnGetItem;
    }
}

