namespace ImageCompressor
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
            this.openSecondFrameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openMotionStuffToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveMotionStuffToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.stuffToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.compressImgToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.compressvideoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pb = new System.Windows.Forms.ProgressBar();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.stuffToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(784, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem,
            this.openSecondFrameToolStripMenuItem,
            this.openFileToolStripMenuItem,
            this.openMotionStuffToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.saveMotionStuffToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(181, 22);
            this.openToolStripMenuItem.Text = "Open Image";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // openSecondFrameToolStripMenuItem
            // 
            this.openSecondFrameToolStripMenuItem.Name = "openSecondFrameToolStripMenuItem";
            this.openSecondFrameToolStripMenuItem.Size = new System.Drawing.Size(181, 22);
            this.openSecondFrameToolStripMenuItem.Text = "Open Second Frame";
            this.openSecondFrameToolStripMenuItem.Click += new System.EventHandler(this.openSecondFrameToolStripMenuItem_Click);
            // 
            // openFileToolStripMenuItem
            // 
            this.openFileToolStripMenuItem.Name = "openFileToolStripMenuItem";
            this.openFileToolStripMenuItem.Size = new System.Drawing.Size(181, 22);
            this.openFileToolStripMenuItem.Text = "Open stuff";
            this.openFileToolStripMenuItem.Click += new System.EventHandler(this.openFileToolStripMenuItem_Click);
            // 
            // openMotionStuffToolStripMenuItem
            // 
            this.openMotionStuffToolStripMenuItem.Name = "openMotionStuffToolStripMenuItem";
            this.openMotionStuffToolStripMenuItem.Size = new System.Drawing.Size(181, 22);
            this.openMotionStuffToolStripMenuItem.Text = "Open MotionStuff";
            this.openMotionStuffToolStripMenuItem.Click += new System.EventHandler(this.openMotionStuffToolStripMenuItem_Click);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(181, 22);
            this.saveToolStripMenuItem.Text = "Save stuff";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // saveMotionStuffToolStripMenuItem
            // 
            this.saveMotionStuffToolStripMenuItem.Name = "saveMotionStuffToolStripMenuItem";
            this.saveMotionStuffToolStripMenuItem.Size = new System.Drawing.Size(181, 22);
            this.saveMotionStuffToolStripMenuItem.Text = "Save MotionStuff";
            this.saveMotionStuffToolStripMenuItem.Click += new System.EventHandler(this.saveMotionStuffToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(181, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // stuffToolStripMenuItem
            // 
            this.stuffToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.compressImgToolStripMenuItem,
            this.compressvideoToolStripMenuItem});
            this.stuffToolStripMenuItem.Name = "stuffToolStripMenuItem";
            this.stuffToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.stuffToolStripMenuItem.Text = "Stuff";
            // 
            // compressImgToolStripMenuItem
            // 
            this.compressImgToolStripMenuItem.Name = "compressImgToolStripMenuItem";
            this.compressImgToolStripMenuItem.Size = new System.Drawing.Size(165, 22);
            this.compressImgToolStripMenuItem.Text = "Compress img";
            this.compressImgToolStripMenuItem.Click += new System.EventHandler(this.compressImgToolStripMenuItem_Click);
            // 
            // compressvideoToolStripMenuItem
            // 
            this.compressvideoToolStripMenuItem.Name = "compressvideoToolStripMenuItem";
            this.compressvideoToolStripMenuItem.Size = new System.Drawing.Size(165, 22);
            this.compressvideoToolStripMenuItem.Text = "Compress \'video\'";
            this.compressvideoToolStripMenuItem.Click += new System.EventHandler(this.compressvideoToolStripMenuItem_Click);
            // 
            // pb
            // 
            this.pb.Location = new System.Drawing.Point(0, 27);
            this.pb.Name = "pb";
            this.pb.Size = new System.Drawing.Size(284, 23);
            this.pb.Step = 1;
            this.pb.TabIndex = 1;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 562);
            this.Controls.Add(this.pb);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Text = "Form1";
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.Form1_Paint);
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
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openFileToolStripMenuItem;
        private System.Windows.Forms.ProgressBar pb;
        private System.Windows.Forms.ToolStripMenuItem openSecondFrameToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem stuffToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem compressImgToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem compressvideoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openMotionStuffToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveMotionStuffToolStripMenuItem;
    }
}

