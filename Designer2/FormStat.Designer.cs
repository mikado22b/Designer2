namespace Designer2
{
    partial class FormStat
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormStat));
            this.treeView = new System.Windows.Forms.TreeView();
            this.cMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.collapseAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.expandAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.closewindowToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.imageList = new System.Windows.Forms.ImageList(this.components);
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.tsWarning = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsLabel3 = new System.Windows.Forms.ToolStripStatusLabel();
            this.cMenu.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // treeView
            // 
            this.treeView.ContextMenuStrip = this.cMenu;
            this.treeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView.ImageIndex = 0;
            this.treeView.ImageList = this.imageList;
            this.treeView.Location = new System.Drawing.Point(0, 0);
            this.treeView.Name = "treeView";
            this.treeView.SelectedImageIndex = 0;
            this.treeView.Size = new System.Drawing.Size(721, 517);
            this.treeView.TabIndex = 0;
            // 
            // cMenu
            // 
            this.cMenu.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.cMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.collapseAllToolStripMenuItem,
            this.expandAllToolStripMenuItem,
            this.toolStripSeparator1,
            this.closewindowToolStripMenuItem});
            this.cMenu.Name = "cMenu";
            this.cMenu.Size = new System.Drawing.Size(171, 82);
            // 
            // collapseAllToolStripMenuItem
            // 
            this.collapseAllToolStripMenuItem.Name = "collapseAllToolStripMenuItem";
            this.collapseAllToolStripMenuItem.Size = new System.Drawing.Size(170, 24);
            this.collapseAllToolStripMenuItem.Text = "&Collapse all";
            this.collapseAllToolStripMenuItem.Click += new System.EventHandler(this.collapseAllToolStripMenuItem_Click);
            // 
            // expandAllToolStripMenuItem
            // 
            this.expandAllToolStripMenuItem.Name = "expandAllToolStripMenuItem";
            this.expandAllToolStripMenuItem.Size = new System.Drawing.Size(170, 24);
            this.expandAllToolStripMenuItem.Text = "&Expand all";
            this.expandAllToolStripMenuItem.Click += new System.EventHandler(this.expandAllToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(167, 6);
            // 
            // closewindowToolStripMenuItem
            // 
            this.closewindowToolStripMenuItem.Name = "closewindowToolStripMenuItem";
            this.closewindowToolStripMenuItem.Size = new System.Drawing.Size(170, 24);
            this.closewindowToolStripMenuItem.Text = "Close &window";
            this.closewindowToolStripMenuItem.Click += new System.EventHandler(this.closewindowToolStripMenuItem_Click);
            // 
            // imageList
            // 
            this.imageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList.ImageStream")));
            this.imageList.TransparentColor = System.Drawing.Color.White;
            this.imageList.Images.SetKeyName(0, "Count.bmp");
            this.imageList.Images.SetKeyName(1, "Count.bmp");
            this.imageList.Images.SetKeyName(2, "Warning.bmp");
            this.imageList.Images.SetKeyName(3, "Flash.bmp");
            this.imageList.Images.SetKeyName(4, "Point.bmp");
            this.imageList.Images.SetKeyName(5, "mLine.bmp");
            this.imageList.Images.SetKeyName(6, "Circle.bmp");
            this.imageList.Images.SetKeyName(7, "Ellipse.bmp");
            this.imageList.Images.SetKeyName(8, "Triangle.bmp");
            this.imageList.Images.SetKeyName(9, "Rectangle.bmp");
            this.imageList.Images.SetKeyName(10, "roundRect.bmp");
            this.imageList.Images.SetKeyName(11, "Polygon.bmp");
            this.imageList.Images.SetKeyName(12, "ArcUl.bmp");
            this.imageList.Images.SetKeyName(13, "Color.bmp");
            this.imageList.Images.SetKeyName(14, "ICO.bmp");
            this.imageList.Images.SetKeyName(15, "List.bmp");
            this.imageList.Images.SetKeyName(16, "Address.bmp");
            this.imageList.Images.SetKeyName(17, "ArcUr.bmp");
            this.imageList.Images.SetKeyName(18, "ArcDl.bmp");
            this.imageList.Images.SetKeyName(19, "ArcDr.bmp");
            this.imageList.Images.SetKeyName(20, "ArcFill.bmp");
            this.imageList.Images.SetKeyName(21, "ArcFillUr.bmp");
            this.imageList.Images.SetKeyName(22, "ArcFillDl.bmp");
            this.imageList.Images.SetKeyName(23, "ArcFillDr.bmp");
            this.imageList.Images.SetKeyName(24, "CircleFill.bmp");
            this.imageList.Images.SetKeyName(25, "EllipseFill.bmp");
            this.imageList.Images.SetKeyName(26, "TriangleFill.bmp");
            this.imageList.Images.SetKeyName(27, "RectangleFill.bmp");
            this.imageList.Images.SetKeyName(28, "roundRectFill.bmp");
            this.imageList.Images.SetKeyName(29, "PolygonFill.bmp");
            this.imageList.Images.SetKeyName(30, "mLineClosed.bmp");
            this.imageList.Images.SetKeyName(31, "Position.bmp");
            this.imageList.Images.SetKeyName(32, "Xpositon.bmp");
            this.imageList.Images.SetKeyName(33, "Ypositon.bmp");
            this.imageList.Images.SetKeyName(34, "Width.bmp");
            this.imageList.Images.SetKeyName(35, "Height.bmp");
            this.imageList.Images.SetKeyName(36, "Radius.bmp");
            this.imageList.Images.SetKeyName(37, "EllipseRadiusX.bmp");
            this.imageList.Images.SetKeyName(38, "EllipseRadiusY.bmp");
            this.imageList.Images.SetKeyName(39, "Empty.bmp");
            this.imageList.Images.SetKeyName(40, "ArcRadius.bmp");
            this.imageList.Images.SetKeyName(41, "UL.bmp");
            this.imageList.Images.SetKeyName(42, "UR.bmp");
            this.imageList.Images.SetKeyName(43, "DL.bmp");
            this.imageList.Images.SetKeyName(44, "DR.bmp");
            this.imageList.Images.SetKeyName(45, "Skin.bmp");
            this.imageList.Images.SetKeyName(46, "PositionWar.bmp");
            this.imageList.Images.SetKeyName(47, "XpositonWar.bmp");
            this.imageList.Images.SetKeyName(48, "YpositonWar.bmp");
            this.imageList.Images.SetKeyName(49, "ICOwar.bmp");
            this.imageList.Images.SetKeyName(50, "PointWar.bmp");
            this.imageList.Images.SetKeyName(51, "mLinewar.bmp");
            this.imageList.Images.SetKeyName(52, "mLineClosedwar.bmp");
            this.imageList.Images.SetKeyName(53, "Circlewar.bmp");
            this.imageList.Images.SetKeyName(54, "CircleFillwar.bmp");
            this.imageList.Images.SetKeyName(55, "Ellipsewar.bmp");
            this.imageList.Images.SetKeyName(56, "EllipseFillwar.bmp");
            this.imageList.Images.SetKeyName(57, "Trianglewar.bmp");
            this.imageList.Images.SetKeyName(58, "TriangleFillwar.bmp");
            this.imageList.Images.SetKeyName(59, "Rectanglewar.bmp");
            this.imageList.Images.SetKeyName(60, "RectangleFillwar.bmp");
            this.imageList.Images.SetKeyName(61, "ArcUlwar.bmp");
            this.imageList.Images.SetKeyName(62, "ArcUrwar.bmp");
            this.imageList.Images.SetKeyName(63, "ArcDlwar.bmp");
            this.imageList.Images.SetKeyName(64, "ArcDrwar.bmp");
            this.imageList.Images.SetKeyName(65, "ArcFillwar.bmp");
            this.imageList.Images.SetKeyName(66, "ArcFillUrwar.bmp");
            this.imageList.Images.SetKeyName(67, "ArcFillDlwar.bmp");
            this.imageList.Images.SetKeyName(68, "ArcFillDrwar.bmp");
            this.imageList.Images.SetKeyName(69, "ArcRadiusWar.bmp");
            this.imageList.Images.SetKeyName(70, "RadiusWar.bmp");
            this.imageList.Images.SetKeyName(71, "EllipseRadiusXwar.bmp");
            this.imageList.Images.SetKeyName(72, "EllipseRadiusYwar.bmp");
            this.imageList.Images.SetKeyName(73, "WidthWar.bmp");
            this.imageList.Images.SetKeyName(74, "HeightWar.bmp");
            this.imageList.Images.SetKeyName(75, "ListWar.bmp");
            this.imageList.Images.SetKeyName(76, "CountWar.bmp");
            this.imageList.Images.SetKeyName(77, "ColorWar.bmp");
            this.imageList.Images.SetKeyName(78, "ICOlist.bmp");
            // 
            // statusStrip1
            // 
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsWarning,
            this.tsLabel1,
            this.tsLabel2,
            this.tsLabel3});
            this.statusStrip1.Location = new System.Drawing.Point(0, 517);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.ShowItemToolTips = true;
            this.statusStrip1.Size = new System.Drawing.Size(721, 29);
            this.statusStrip1.TabIndex = 2;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // tsWarning
            // 
            this.tsWarning.AutoToolTip = true;
            this.tsWarning.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
            this.tsWarning.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsWarning.Image = global::Designer2.Properties.Resources.Warning;
            this.tsWarning.ImageTransparentColor = System.Drawing.Color.White;
            this.tsWarning.Name = "tsWarning";
            this.tsWarning.Size = new System.Drawing.Size(24, 24);
            this.tsWarning.ToolTipText = "They are warnings.";
            // 
            // tsLabel1
            // 
            this.tsLabel1.AutoToolTip = true;
            this.tsLabel1.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
            this.tsLabel1.Image = global::Designer2.Properties.Resources.ICO;
            this.tsLabel1.Name = "tsLabel1";
            this.tsLabel1.Size = new System.Drawing.Size(115, 24);
            this.tsLabel1.Text = "Icons count :";
            this.tsLabel1.ToolTipText = "Icons count.";
            // 
            // tsLabel2
            // 
            this.tsLabel2.AutoToolTip = true;
            this.tsLabel2.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
            this.tsLabel2.Image = global::Designer2.Properties.Resources.List;
            this.tsLabel2.ImageTransparentColor = System.Drawing.Color.White;
            this.tsLabel2.Name = "tsLabel2";
            this.tsLabel2.Size = new System.Drawing.Size(117, 24);
            this.tsLabel2.Text = "Items count :";
            this.tsLabel2.ToolTipText = "Total items count.";
            // 
            // tsLabel3
            // 
            this.tsLabel3.AutoToolTip = true;
            this.tsLabel3.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
            this.tsLabel3.Image = global::Designer2.Properties.Resources.Flash;
            this.tsLabel3.ImageTransparentColor = System.Drawing.Color.White;
            this.tsLabel3.Name = "tsLabel3";
            this.tsLabel3.Size = new System.Drawing.Size(131, 24);
            this.tsLabel3.Text = "Bytes in FLASH";
            this.tsLabel3.ToolTipText = "Bytes in FLASH.";
            // 
            // FormStat
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(721, 546);
            this.Controls.Add(this.treeView);
            this.Controls.Add(this.statusStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormStat";
            this.Text = "Statistic";
            this.cMenu.ResumeLayout(false);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TreeView treeView;
        private System.Windows.Forms.ContextMenuStrip cMenu;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ImageList imageList;
        private System.Windows.Forms.ToolStripMenuItem collapseAllToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem expandAllToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem closewindowToolStripMenuItem;
        private System.Windows.Forms.ToolStripStatusLabel tsWarning;
        private System.Windows.Forms.ToolStripStatusLabel tsLabel1;
        private System.Windows.Forms.ToolStripStatusLabel tsLabel2;
        private System.Windows.Forms.ToolStripStatusLabel tsLabel3;
    }
}