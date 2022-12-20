﻿namespace SimpleRosterImporter
{
    partial class Importer
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Importer));
            this.label2 = new System.Windows.Forms.Label();
            this.rosterOutput = new System.Windows.Forms.TextBox();
            this.saveButton = new System.Windows.Forms.Button();
            this.status = new System.Windows.Forms.Label();
            this.updateStatusTick = new System.Windows.Forms.Timer(this.components);
            this.messageQueue = new System.Windows.Forms.Timer(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.nameText = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label2.Location = new System.Drawing.Point(12, 13);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(143, 15);
            this.label2.TabIndex = 7;
            this.label2.Text = "Imported Roster Output";
            // 
            // rosterOutput
            // 
            this.rosterOutput.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.rosterOutput.Location = new System.Drawing.Point(12, 40);
            this.rosterOutput.Multiline = true;
            this.rosterOutput.Name = "rosterOutput";
            this.rosterOutput.ReadOnly = true;
            this.rosterOutput.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.rosterOutput.Size = new System.Drawing.Size(666, 327);
            this.rosterOutput.TabIndex = 6;
            // 
            // saveButton
            // 
            this.saveButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.saveButton.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.saveButton.Location = new System.Drawing.Point(485, 381);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(193, 29);
            this.saveButton.TabIndex = 8;
            this.saveButton.Text = "Save Roster";
            this.saveButton.UseVisualStyleBackColor = true;
            this.saveButton.Click += new System.EventHandler(this.saveButton_Click);
            // 
            // status
            // 
            this.status.Location = new System.Drawing.Point(523, 13);
            this.status.Name = "status";
            this.status.Size = new System.Drawing.Size(155, 24);
            this.status.TabIndex = 14;
            this.status.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // updateStatusTick
            // 
            this.updateStatusTick.Enabled = true;
            this.updateStatusTick.Interval = 300;
            this.updateStatusTick.Tick += new System.EventHandler(this.updateStatusTick_Tick);
            // 
            // messageQueue
            // 
            this.messageQueue.Enabled = true;
            this.messageQueue.Tick += new System.EventHandler(this.messageQueue_Tick);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label1.Location = new System.Drawing.Point(12, 388);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(83, 15);
            this.label1.TabIndex = 10;
            this.label1.Text = "Roster Name:";
            // 
            // nameText
            // 
            this.nameText.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.nameText.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.nameText.Location = new System.Drawing.Point(101, 381);
            this.nameText.Name = "nameText";
            this.nameText.Size = new System.Drawing.Size(378, 29);
            this.nameText.TabIndex = 9;
            this.nameText.TextChanged += new System.EventHandler(this.nameText_TextChanged);
            // 
            // Importer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(690, 431);
            this.Controls.Add(this.status);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.nameText);
            this.Controls.Add(this.saveButton);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.rosterOutput);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Importer";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Importer";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Importer_FormClosing);
            this.Load += new System.EventHandler(this.Importer_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.Importer_Paint);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Label label2;
        private TextBox rosterOutput;
        private Button saveButton;
        private Label status;
        private System.Windows.Forms.Timer updateStatusTick;
        private System.Windows.Forms.Timer messageQueue;
        private Label label1;
        private TextBox nameText;
    }
}