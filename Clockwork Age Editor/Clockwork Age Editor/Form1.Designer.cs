namespace Clockwork_Age_Editor
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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sceneFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.xnaViewControl1 = new XnaViewControl();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(984, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem,
            this.exportToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(137, 22);
            this.openToolStripMenuItem.Text = "Open Scene";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.OpenMenuClicked);
            // 
            // exportToolStripMenuItem
            // 
            this.exportToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.sceneFileToolStripMenuItem});
            this.exportToolStripMenuItem.Name = "exportToolStripMenuItem";
            this.exportToolStripMenuItem.Size = new System.Drawing.Size(137, 22);
            this.exportToolStripMenuItem.Text = "Export";
            // 
            // sceneFileToolStripMenuItem
            // 
            this.sceneFileToolStripMenuItem.Name = "sceneFileToolStripMenuItem";
            this.sceneFileToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.sceneFileToolStripMenuItem.Text = "Scene file";
            this.sceneFileToolStripMenuItem.Click += new System.EventHandler(this.sceneFileToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(137, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            // 
            // treeView1
            // 
            this.treeView1.Location = new System.Drawing.Point(12, 37);
            this.treeView1.Name = "treeView1";
            this.treeView1.Size = new System.Drawing.Size(195, 513);
            this.treeView1.TabIndex = 4;
            // 
            // xnaViewControl1
            // 
            this.xnaViewControl1.Location = new System.Drawing.Point(213, 37);
            this.xnaViewControl1.Name = "xnaViewControl1";
            this.xnaViewControl1.Size = new System.Drawing.Size(759, 513);
            this.xnaViewControl1.TabIndex = 2;
            this.xnaViewControl1.Text = "xnaViewControl1";
            this.xnaViewControl1.Click += new System.EventHandler(this.xnaViewControl1_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.ClientSize = new System.Drawing.Size(984, 562);
            this.Controls.Add(this.treeView1);
            this.Controls.Add(this.xnaViewControl1);
            this.Controls.Add(this.menuStrip1);
            this.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Text = "Clockwork Age Editor";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private XnaViewControl xnaViewControl1;
        private System.Windows.Forms.ToolStripMenuItem exportToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem sceneFileToolStripMenuItem;
        private System.Windows.Forms.TreeView treeView1;

    }
}

