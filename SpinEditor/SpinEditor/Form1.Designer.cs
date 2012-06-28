﻿using System.Windows.Forms;
namespace SpinEditor
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.NewFileToolstripOption = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.undoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.redoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.deselectAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ViewMenuHideOverlay = new System.Windows.Forms.ToolStripMenuItem();
            this.ViewMenuHideGrid = new System.Windows.Forms.ToolStripMenuItem();
            this.ViewMenuHideCoordinates = new System.Windows.Forms.ToolStripMenuItem();
            this.ViewMenuHideMovementPath = new System.Windows.Forms.ToolStripMenuItem();
            this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.BUTTON_EDITOR_MODE_SELECT = new System.Windows.Forms.ToolStripButton();
            this.BUTTON_EDITOR_MODE_MOVE = new System.Windows.Forms.ToolStripButton();
            this.BUTTON_EDITOR_MODE_PLACE = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.BUTTON_DELETE = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.BUTTON_EDIT_ROOM = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.BUTTON_UNDO = new System.Windows.Forms.ToolStripButton();
            this.BUTTON_REDO = new System.Windows.Forms.ToolStripButton();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.hScrollBar1 = new System.Windows.Forms.HScrollBar();
            this.vScrollBar1 = new System.Windows.Forms.VScrollBar();
            this.propertyGrid1 = new System.Windows.Forms.PropertyGrid();
            this.BUTTON_IMPORT_ASSET = new System.Windows.Forms.Button();
            this.listBox_Classes = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.listBox_Assets1 = new System.Windows.Forms.ListBox();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.listBox_Assets2 = new System.Windows.Forms.ListBox();
            this.panel4 = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.ZOOM = new System.Windows.Forms.NumericUpDown();
            this.panel3 = new System.Windows.Forms.Panel();
            this.Align_Relative = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.BUTTON_ALIGN_10 = new System.Windows.Forms.Button();
            this.BUTTON_ALIGN_1 = new System.Windows.Forms.Button();
            this.BUTTON_ALIGN_6 = new System.Windows.Forms.Button();
            this.BUTTON_ALIGN_2 = new System.Windows.Forms.Button();
            this.BUTTON_ALIGN_7 = new System.Windows.Forms.Button();
            this.BUTTON_ALIGN_3 = new System.Windows.Forms.Button();
            this.BUTTON_ALIGN_8 = new System.Windows.Forms.Button();
            this.BUTTON_ALIGN_4 = new System.Windows.Forms.Button();
            this.BUTTON_ALIGN_9 = new System.Windows.Forms.Button();
            this.BUTTON_ALIGN_5 = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.xnA_RenderControl1 = new SpinEditor.XNA_RenderControl();
            this.listBox_Assets0 = new System.Windows.Forms.ListBox();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.menuStrip1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.panel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ZOOM)).BeginInit();
            this.panel3.SuspendLayout();
            this.panel2.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.editToolStripMenuItem,
            this.viewToolStripMenuItem,
            this.toolsToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1350, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.NewFileToolstripOption,
            this.toolStripSeparator5,
            this.openToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // NewFileToolstripOption
            // 
            this.NewFileToolstripOption.Name = "NewFileToolstripOption";
            this.NewFileToolstripOption.Size = new System.Drawing.Size(103, 22);
            this.NewFileToolstripOption.Text = "New";
            this.NewFileToolstripOption.Click += new System.EventHandler(this.NewFileToolstripOption_Click);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(100, 6);
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(103, 22);
            this.openToolStripMenuItem.Text = "Open";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(103, 22);
            this.saveToolStripMenuItem.Text = "Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(103, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.undoToolStripMenuItem,
            this.redoToolStripMenuItem,
            this.toolStripSeparator4,
            this.deselectAllToolStripMenuItem});
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(39, 20);
            this.editToolStripMenuItem.Text = "Edit";
            // 
            // undoToolStripMenuItem
            // 
            this.undoToolStripMenuItem.Name = "undoToolStripMenuItem";
            this.undoToolStripMenuItem.Size = new System.Drawing.Size(135, 22);
            this.undoToolStripMenuItem.Text = "Undo";
            this.undoToolStripMenuItem.Click += new System.EventHandler(this.undoToolStripMenuItem_Click);
            // 
            // redoToolStripMenuItem
            // 
            this.redoToolStripMenuItem.Name = "redoToolStripMenuItem";
            this.redoToolStripMenuItem.Size = new System.Drawing.Size(135, 22);
            this.redoToolStripMenuItem.Text = "Redo";
            this.redoToolStripMenuItem.Click += new System.EventHandler(this.redoToolStripMenuItem_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(132, 6);
            // 
            // deselectAllToolStripMenuItem
            // 
            this.deselectAllToolStripMenuItem.Name = "deselectAllToolStripMenuItem";
            this.deselectAllToolStripMenuItem.Size = new System.Drawing.Size(135, 22);
            this.deselectAllToolStripMenuItem.Text = "Deselect All";
            this.deselectAllToolStripMenuItem.Click += new System.EventHandler(this.deselectAllToolStripMenuItem_Click);
            // 
            // viewToolStripMenuItem
            // 
            this.viewToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ViewMenuHideOverlay,
            this.ViewMenuHideGrid,
            this.ViewMenuHideCoordinates,
            this.ViewMenuHideMovementPath});
            this.viewToolStripMenuItem.Name = "viewToolStripMenuItem";
            this.viewToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.viewToolStripMenuItem.Text = "View";
            // 
            // ViewMenuHideOverlay
            // 
            this.ViewMenuHideOverlay.Name = "ViewMenuHideOverlay";
            this.ViewMenuHideOverlay.Size = new System.Drawing.Size(190, 22);
            this.ViewMenuHideOverlay.Text = "Hide Object Overlay";
            this.ViewMenuHideOverlay.Click += new System.EventHandler(this.ViewMenuHideOverlay_Click);
            // 
            // ViewMenuHideGrid
            // 
            this.ViewMenuHideGrid.Name = "ViewMenuHideGrid";
            this.ViewMenuHideGrid.Size = new System.Drawing.Size(190, 22);
            this.ViewMenuHideGrid.Text = "Hide Grid";
            this.ViewMenuHideGrid.Click += new System.EventHandler(this.ViewMenuHideGrid_Click);
            // 
            // ViewMenuHideCoordinates
            // 
            this.ViewMenuHideCoordinates.Name = "ViewMenuHideCoordinates";
            this.ViewMenuHideCoordinates.Size = new System.Drawing.Size(190, 22);
            this.ViewMenuHideCoordinates.Text = "Hide Coordinates";
            this.ViewMenuHideCoordinates.Click += new System.EventHandler(this.ViewMenuHideCoordinates_Click);
            // 
            // ViewMenuHideMovementPath
            // 
            this.ViewMenuHideMovementPath.Name = "ViewMenuHideMovementPath";
            this.ViewMenuHideMovementPath.Size = new System.Drawing.Size(190, 22);
            this.ViewMenuHideMovementPath.Text = "Hide Movement Lines";
            this.ViewMenuHideMovementPath.Click += new System.EventHandler(this.ViewMenuHideMovementPath_Click);
            // 
            // toolsToolStripMenuItem
            // 
            this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            this.toolsToolStripMenuItem.Size = new System.Drawing.Size(48, 20);
            this.toolsToolStripMenuItem.Text = "Tools";
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel1,
            this.BUTTON_EDITOR_MODE_SELECT,
            this.BUTTON_EDITOR_MODE_MOVE,
            this.BUTTON_EDITOR_MODE_PLACE,
            this.toolStripSeparator1,
            this.BUTTON_DELETE,
            this.toolStripSeparator2,
            this.BUTTON_EDIT_ROOM,
            this.toolStripSeparator3,
            this.BUTTON_UNDO,
            this.BUTTON_REDO});
            this.toolStrip1.Location = new System.Drawing.Point(0, 24);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(1350, 25);
            this.toolStrip1.TabIndex = 2;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(75, 22);
            this.toolStripLabel1.Text = "Editor Mode:";
            // 
            // BUTTON_EDITOR_MODE_SELECT
            // 
            this.BUTTON_EDITOR_MODE_SELECT.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.BUTTON_EDITOR_MODE_SELECT.Image = ((System.Drawing.Image)(resources.GetObject("BUTTON_EDITOR_MODE_SELECT.Image")));
            this.BUTTON_EDITOR_MODE_SELECT.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.BUTTON_EDITOR_MODE_SELECT.MergeAction = System.Windows.Forms.MergeAction.MatchOnly;
            this.BUTTON_EDITOR_MODE_SELECT.Name = "BUTTON_EDITOR_MODE_SELECT";
            this.BUTTON_EDITOR_MODE_SELECT.Size = new System.Drawing.Size(23, 22);
            this.BUTTON_EDITOR_MODE_SELECT.Text = "Select";
            this.BUTTON_EDITOR_MODE_SELECT.ToolTipText = "Select objects (hold shift for multi-select)";
            this.BUTTON_EDITOR_MODE_SELECT.Click += new System.EventHandler(this.BUTTON_EDITOR_MODE_SELECT_Click);
            // 
            // BUTTON_EDITOR_MODE_MOVE
            // 
            this.BUTTON_EDITOR_MODE_MOVE.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.BUTTON_EDITOR_MODE_MOVE.Image = ((System.Drawing.Image)(resources.GetObject("BUTTON_EDITOR_MODE_MOVE.Image")));
            this.BUTTON_EDITOR_MODE_MOVE.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.BUTTON_EDITOR_MODE_MOVE.Name = "BUTTON_EDITOR_MODE_MOVE";
            this.BUTTON_EDITOR_MODE_MOVE.Size = new System.Drawing.Size(23, 22);
            this.BUTTON_EDITOR_MODE_MOVE.Text = "Move";
            this.BUTTON_EDITOR_MODE_MOVE.ToolTipText = "Move selected objects.";
            this.BUTTON_EDITOR_MODE_MOVE.Click += new System.EventHandler(this.BUTTON_EDITOR_MODE_MOVE_Click);
            // 
            // BUTTON_EDITOR_MODE_PLACE
            // 
            this.BUTTON_EDITOR_MODE_PLACE.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.BUTTON_EDITOR_MODE_PLACE.Image = ((System.Drawing.Image)(resources.GetObject("BUTTON_EDITOR_MODE_PLACE.Image")));
            this.BUTTON_EDITOR_MODE_PLACE.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.BUTTON_EDITOR_MODE_PLACE.Name = "BUTTON_EDITOR_MODE_PLACE";
            this.BUTTON_EDITOR_MODE_PLACE.Size = new System.Drawing.Size(23, 22);
            this.BUTTON_EDITOR_MODE_PLACE.Text = "Place";
            this.BUTTON_EDITOR_MODE_PLACE.ToolTipText = "Place a new object.";
            this.BUTTON_EDITOR_MODE_PLACE.Click += new System.EventHandler(this.BUTTON_EDITOR_MODE_PLACE_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // BUTTON_DELETE
            // 
            this.BUTTON_DELETE.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.BUTTON_DELETE.Image = ((System.Drawing.Image)(resources.GetObject("BUTTON_DELETE.Image")));
            this.BUTTON_DELETE.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.BUTTON_DELETE.Name = "BUTTON_DELETE";
            this.BUTTON_DELETE.Size = new System.Drawing.Size(23, 22);
            this.BUTTON_DELETE.Text = "Delete";
            this.BUTTON_DELETE.ToolTipText = "Deletes currently selected object.";
            this.BUTTON_DELETE.Click += new System.EventHandler(this.BUTTON_DELETE_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // BUTTON_EDIT_ROOM
            // 
            this.BUTTON_EDIT_ROOM.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.BUTTON_EDIT_ROOM.Checked = true;
            this.BUTTON_EDIT_ROOM.CheckState = System.Windows.Forms.CheckState.Checked;
            this.BUTTON_EDIT_ROOM.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.BUTTON_EDIT_ROOM.Image = ((System.Drawing.Image)(resources.GetObject("BUTTON_EDIT_ROOM.Image")));
            this.BUTTON_EDIT_ROOM.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.BUTTON_EDIT_ROOM.Name = "BUTTON_EDIT_ROOM";
            this.BUTTON_EDIT_ROOM.Size = new System.Drawing.Size(122, 22);
            this.BUTTON_EDIT_ROOM.Text = "Edit Room Properties";
            this.BUTTON_EDIT_ROOM.Click += new System.EventHandler(this.BUTTON_EDIT_ROOM_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            // 
            // BUTTON_UNDO
            // 
            this.BUTTON_UNDO.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.BUTTON_UNDO.Image = ((System.Drawing.Image)(resources.GetObject("BUTTON_UNDO.Image")));
            this.BUTTON_UNDO.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.BUTTON_UNDO.Name = "BUTTON_UNDO";
            this.BUTTON_UNDO.Size = new System.Drawing.Size(40, 22);
            this.BUTTON_UNDO.Text = "Undo";
            this.BUTTON_UNDO.ToolTipText = "Undo last action.";
            this.BUTTON_UNDO.Click += new System.EventHandler(this.BUTTON_UNDO_Click);
            // 
            // BUTTON_REDO
            // 
            this.BUTTON_REDO.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.BUTTON_REDO.Image = ((System.Drawing.Image)(resources.GetObject("BUTTON_REDO.Image")));
            this.BUTTON_REDO.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.BUTTON_REDO.Name = "BUTTON_REDO";
            this.BUTTON_REDO.Size = new System.Drawing.Size(38, 22);
            this.BUTTON_REDO.Text = "Redo";
            this.BUTTON_REDO.ToolTipText = "Redo last undone action.";
            this.BUTTON_REDO.Click += new System.EventHandler(this.BUTTON_REDO_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // hScrollBar1
            // 
            this.hScrollBar1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.hScrollBar1.Location = new System.Drawing.Point(0, 635);
            this.hScrollBar1.Name = "hScrollBar1";
            this.hScrollBar1.Size = new System.Drawing.Size(882, 16);
            this.hScrollBar1.TabIndex = 4;
            this.hScrollBar1.Scroll += new System.Windows.Forms.ScrollEventHandler(this.hScrollBar1_Scroll);
            // 
            // vScrollBar1
            // 
            this.vScrollBar1.Dock = System.Windows.Forms.DockStyle.Right;
            this.vScrollBar1.Location = new System.Drawing.Point(882, 0);
            this.vScrollBar1.Name = "vScrollBar1";
            this.vScrollBar1.Size = new System.Drawing.Size(16, 651);
            this.vScrollBar1.TabIndex = 5;
            this.vScrollBar1.Scroll += new System.Windows.Forms.ScrollEventHandler(this.vScrollBar1_Scroll);
            // 
            // propertyGrid1
            // 
            this.propertyGrid1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.propertyGrid1.Location = new System.Drawing.Point(10, 3);
            this.propertyGrid1.Name = "propertyGrid1";
            this.propertyGrid1.Size = new System.Drawing.Size(430, 349);
            this.propertyGrid1.TabIndex = 6;
            // 
            // BUTTON_IMPORT_ASSET
            // 
            this.BUTTON_IMPORT_ASSET.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BUTTON_IMPORT_ASSET.Location = new System.Drawing.Point(146, 612);
            this.BUTTON_IMPORT_ASSET.Name = "BUTTON_IMPORT_ASSET";
            this.BUTTON_IMPORT_ASSET.Size = new System.Drawing.Size(74, 23);
            this.BUTTON_IMPORT_ASSET.TabIndex = 8;
            this.BUTTON_IMPORT_ASSET.Text = "Import Asset";
            this.BUTTON_IMPORT_ASSET.UseVisualStyleBackColor = true;
            this.BUTTON_IMPORT_ASSET.Click += new System.EventHandler(this.BUTTON_IMPORT_ASSET_Click);
            // 
            // listBox_Classes
            // 
            this.listBox_Classes.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.listBox_Classes.FormattingEnabled = true;
            this.listBox_Classes.Items.AddRange(new object[] {
            "Bounce Pad",
            "Decal",
            "Door",
            "Ladder",
            "Moving Platform",
            "Note",
            "One-Sided Platform",
            "Piston",
            "Pullable Object",
            "Pushing Platform",
            "Rope",
            "Rotate Room Button",
            "Rotating Platform",
            "Saw Blade",
            "Spikes",
            "Static Object"});
            this.listBox_Classes.Location = new System.Drawing.Point(10, 374);
            this.listBox_Classes.Name = "listBox_Classes";
            this.listBox_Classes.Size = new System.Drawing.Size(210, 95);
            this.listBox_Classes.TabIndex = 9;
            this.listBox_Classes.SelectedIndexChanged += new System.EventHandler(this.listBox_Classes_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 476);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(102, 13);
            this.label1.TabIndex = 10;
            this.label1.Text = "New Object Texture";
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(10, 358);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(90, 13);
            this.label2.TabIndex = 11;
            this.label2.Text = "New Object Type";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.tabControl1);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.panel4);
            this.panel1.Controls.Add(this.panel3);
            this.panel1.Controls.Add(this.listBox_Classes);
            this.panel1.Controls.Add(this.propertyGrid1);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.BUTTON_IMPORT_ASSET);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel1.Location = new System.Drawing.Point(898, 49);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(452, 651);
            this.panel1.TabIndex = 12;
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Location = new System.Drawing.Point(10, 492);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(210, 118);
            this.tabControl1.TabIndex = 6;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.listBox_Assets1);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(202, 92);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Tex 2";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // listBox_Assets1
            // 
            this.listBox_Assets1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.listBox_Assets1.FormattingEnabled = true;
            this.listBox_Assets1.Location = new System.Drawing.Point(3, 5);
            this.listBox_Assets1.Name = "listBox_Assets1";
            this.listBox_Assets1.Size = new System.Drawing.Size(196, 82);
            this.listBox_Assets1.TabIndex = 8;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.listBox_Assets2);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(202, 92);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Tex 3";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // listBox_Assets2
            // 
            this.listBox_Assets2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.listBox_Assets2.FormattingEnabled = true;
            this.listBox_Assets2.Location = new System.Drawing.Point(3, 5);
            this.listBox_Assets2.Name = "listBox_Assets2";
            this.listBox_Assets2.Size = new System.Drawing.Size(196, 82);
            this.listBox_Assets2.TabIndex = 8;
            // 
            // panel4
            // 
            this.panel4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.panel4.Controls.Add(this.label3);
            this.panel4.Controls.Add(this.ZOOM);
            this.panel4.Location = new System.Drawing.Point(226, 495);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(214, 33);
            this.panel4.TabIndex = 28;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(18, 7);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(134, 13);
            this.label3.TabIndex = 25;
            this.label3.Text = "Camera Zoom Percentage:";
            // 
            // ZOOM
            // 
            this.ZOOM.DecimalPlaces = 1;
            this.ZOOM.Increment = new decimal(new int[] {
            25,
            0,
            0,
            65536});
            this.ZOOM.Location = new System.Drawing.Point(157, 5);
            this.ZOOM.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.ZOOM.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.ZOOM.Name = "ZOOM";
            this.ZOOM.Size = new System.Drawing.Size(54, 20);
            this.ZOOM.TabIndex = 28;
            this.ZOOM.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.ZOOM.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.ZOOM.ValueChanged += new System.EventHandler(this.ZOOM_ValueChanged);
            // 
            // panel3
            // 
            this.panel3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.panel3.Controls.Add(this.Align_Relative);
            this.panel3.Controls.Add(this.BUTTON_ALIGN_10);
            this.panel3.Controls.Add(this.BUTTON_ALIGN_1);
            this.panel3.Controls.Add(this.BUTTON_ALIGN_6);
            this.panel3.Controls.Add(this.BUTTON_ALIGN_2);
            this.panel3.Controls.Add(this.BUTTON_ALIGN_7);
            this.panel3.Controls.Add(this.BUTTON_ALIGN_3);
            this.panel3.Controls.Add(this.BUTTON_ALIGN_8);
            this.panel3.Controls.Add(this.BUTTON_ALIGN_4);
            this.panel3.Controls.Add(this.BUTTON_ALIGN_9);
            this.panel3.Controls.Add(this.BUTTON_ALIGN_5);
            this.panel3.Location = new System.Drawing.Point(226, 374);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(214, 115);
            this.panel3.TabIndex = 27;
            // 
            // Align_Relative
            // 
            this.Align_Relative.FormattingEnabled = true;
            this.Align_Relative.Items.AddRange(new object[] {
            "Last Selected",
            "First Selected",
            "Selection"});
            this.Align_Relative.Location = new System.Drawing.Point(6, 3);
            this.Align_Relative.Name = "Align_Relative";
            this.Align_Relative.Size = new System.Drawing.Size(202, 21);
            this.Align_Relative.TabIndex = 26;
            this.Align_Relative.SelectedIndexChanged += new System.EventHandler(this.Align_Relative_SelectedIndexChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(229, 358);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(91, 13);
            this.label5.TabIndex = 25;
            this.label5.Text = "Align Relative To:";
            // 
            // BUTTON_ALIGN_10
            // 
            this.BUTTON_ALIGN_10.Image = ((System.Drawing.Image)(resources.GetObject("BUTTON_ALIGN_10.Image")));
            this.BUTTON_ALIGN_10.Location = new System.Drawing.Point(172, 70);
            this.BUTTON_ALIGN_10.Name = "BUTTON_ALIGN_10";
            this.BUTTON_ALIGN_10.Size = new System.Drawing.Size(34, 34);
            this.BUTTON_ALIGN_10.TabIndex = 18;
            this.BUTTON_ALIGN_10.UseVisualStyleBackColor = true;
            // 
            // BUTTON_ALIGN_1
            // 
            this.BUTTON_ALIGN_1.Image = ((System.Drawing.Image)(resources.GetObject("BUTTON_ALIGN_1.Image")));
            this.BUTTON_ALIGN_1.Location = new System.Drawing.Point(6, 30);
            this.BUTTON_ALIGN_1.Name = "BUTTON_ALIGN_1";
            this.BUTTON_ALIGN_1.Size = new System.Drawing.Size(34, 34);
            this.BUTTON_ALIGN_1.TabIndex = 13;
            this.BUTTON_ALIGN_1.UseVisualStyleBackColor = true;
            // 
            // BUTTON_ALIGN_6
            // 
            this.BUTTON_ALIGN_6.Image = ((System.Drawing.Image)(resources.GetObject("BUTTON_ALIGN_6.Image")));
            this.BUTTON_ALIGN_6.Location = new System.Drawing.Point(6, 70);
            this.BUTTON_ALIGN_6.Name = "BUTTON_ALIGN_6";
            this.BUTTON_ALIGN_6.Size = new System.Drawing.Size(34, 34);
            this.BUTTON_ALIGN_6.TabIndex = 22;
            this.BUTTON_ALIGN_6.UseVisualStyleBackColor = true;
            // 
            // BUTTON_ALIGN_2
            // 
            this.BUTTON_ALIGN_2.Image = ((System.Drawing.Image)(resources.GetObject("BUTTON_ALIGN_2.Image")));
            this.BUTTON_ALIGN_2.Location = new System.Drawing.Point(46, 30);
            this.BUTTON_ALIGN_2.Name = "BUTTON_ALIGN_2";
            this.BUTTON_ALIGN_2.Size = new System.Drawing.Size(34, 34);
            this.BUTTON_ALIGN_2.TabIndex = 14;
            this.BUTTON_ALIGN_2.UseVisualStyleBackColor = true;
            // 
            // BUTTON_ALIGN_7
            // 
            this.BUTTON_ALIGN_7.Image = ((System.Drawing.Image)(resources.GetObject("BUTTON_ALIGN_7.Image")));
            this.BUTTON_ALIGN_7.Location = new System.Drawing.Point(46, 70);
            this.BUTTON_ALIGN_7.Name = "BUTTON_ALIGN_7";
            this.BUTTON_ALIGN_7.Size = new System.Drawing.Size(34, 34);
            this.BUTTON_ALIGN_7.TabIndex = 21;
            this.BUTTON_ALIGN_7.UseVisualStyleBackColor = true;
            // 
            // BUTTON_ALIGN_3
            // 
            this.BUTTON_ALIGN_3.Image = ((System.Drawing.Image)(resources.GetObject("BUTTON_ALIGN_3.Image")));
            this.BUTTON_ALIGN_3.Location = new System.Drawing.Point(90, 30);
            this.BUTTON_ALIGN_3.Name = "BUTTON_ALIGN_3";
            this.BUTTON_ALIGN_3.Size = new System.Drawing.Size(34, 34);
            this.BUTTON_ALIGN_3.TabIndex = 15;
            this.BUTTON_ALIGN_3.UseVisualStyleBackColor = true;
            // 
            // BUTTON_ALIGN_8
            // 
            this.BUTTON_ALIGN_8.Image = ((System.Drawing.Image)(resources.GetObject("BUTTON_ALIGN_8.Image")));
            this.BUTTON_ALIGN_8.Location = new System.Drawing.Point(90, 70);
            this.BUTTON_ALIGN_8.Name = "BUTTON_ALIGN_8";
            this.BUTTON_ALIGN_8.Size = new System.Drawing.Size(34, 34);
            this.BUTTON_ALIGN_8.TabIndex = 20;
            this.BUTTON_ALIGN_8.UseVisualStyleBackColor = true;
            // 
            // BUTTON_ALIGN_4
            // 
            this.BUTTON_ALIGN_4.Image = ((System.Drawing.Image)(resources.GetObject("BUTTON_ALIGN_4.Image")));
            this.BUTTON_ALIGN_4.Location = new System.Drawing.Point(132, 30);
            this.BUTTON_ALIGN_4.Name = "BUTTON_ALIGN_4";
            this.BUTTON_ALIGN_4.Size = new System.Drawing.Size(34, 34);
            this.BUTTON_ALIGN_4.TabIndex = 16;
            this.BUTTON_ALIGN_4.UseVisualStyleBackColor = true;
            // 
            // BUTTON_ALIGN_9
            // 
            this.BUTTON_ALIGN_9.Image = ((System.Drawing.Image)(resources.GetObject("BUTTON_ALIGN_9.Image")));
            this.BUTTON_ALIGN_9.Location = new System.Drawing.Point(132, 70);
            this.BUTTON_ALIGN_9.Name = "BUTTON_ALIGN_9";
            this.BUTTON_ALIGN_9.Size = new System.Drawing.Size(34, 34);
            this.BUTTON_ALIGN_9.TabIndex = 19;
            this.BUTTON_ALIGN_9.UseVisualStyleBackColor = true;
            // 
            // BUTTON_ALIGN_5
            // 
            this.BUTTON_ALIGN_5.Image = ((System.Drawing.Image)(resources.GetObject("BUTTON_ALIGN_5.Image")));
            this.BUTTON_ALIGN_5.Location = new System.Drawing.Point(172, 30);
            this.BUTTON_ALIGN_5.Name = "BUTTON_ALIGN_5";
            this.BUTTON_ALIGN_5.Size = new System.Drawing.Size(34, 34);
            this.BUTTON_ALIGN_5.TabIndex = 17;
            this.BUTTON_ALIGN_5.UseVisualStyleBackColor = true;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.hScrollBar1);
            this.panel2.Controls.Add(this.vScrollBar1);
            this.panel2.Controls.Add(this.xnA_RenderControl1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 49);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(898, 651);
            this.panel2.TabIndex = 13;
            // 
            // xnA_RenderControl1
            // 
            this.xnA_RenderControl1.Camera = null;
            this.xnA_RenderControl1.contentMan = null;
            this.xnA_RenderControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.xnA_RenderControl1.form1 = null;
            this.xnA_RenderControl1.HideCoordinates = false;
            this.xnA_RenderControl1.HideGrid = false;
            this.xnA_RenderControl1.HideMovementPath = false;
            this.xnA_RenderControl1.HideOverlay = false;
            this.xnA_RenderControl1.levelBackground = null;
            this.xnA_RenderControl1.levelDimensions = new Microsoft.Xna.Framework.Vector2(0F, 0F);
            this.xnA_RenderControl1.Location = new System.Drawing.Point(0, 0);
            this.xnA_RenderControl1.Name = "xnA_RenderControl1";
            this.xnA_RenderControl1.Size = new System.Drawing.Size(898, 651);
            this.xnA_RenderControl1.TabIndex = 0;
            this.xnA_RenderControl1.Text = "xnA_RenderControl1";
            this.xnA_RenderControl1.Click += new System.EventHandler(this.xnA_RenderControl1_Click);
            this.xnA_RenderControl1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.xnA_RenderControl1_MouseDown);
            this.xnA_RenderControl1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.xnA_RenderControl1_MouseMove);
            // 
            // listBox_Assets0
            // 
            this.listBox_Assets0.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.listBox_Assets0.FormattingEnabled = true;
            this.listBox_Assets0.Location = new System.Drawing.Point(3, 5);
            this.listBox_Assets0.Name = "listBox_Assets0";
            this.listBox_Assets0.Size = new System.Drawing.Size(196, 82);
            this.listBox_Assets0.TabIndex = 7;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.listBox_Assets0);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(202, 92);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Tex 1";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1350, 700);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.Text = "Spin Doctor Editor";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ZOOM)).EndInit();
            this.panel3.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private XNA_RenderControl xnA_RenderControl1;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem fileToolStripMenuItem;
        private ToolStripMenuItem openToolStripMenuItem;
        private ToolStripMenuItem saveToolStripMenuItem;
        private ToolStripMenuItem exitToolStripMenuItem;
        private ToolStripMenuItem editToolStripMenuItem;
        private ToolStripMenuItem toolsToolStripMenuItem;
        private ToolStripMenuItem helpToolStripMenuItem;
        private ToolStrip toolStrip1;
        private OpenFileDialog openFileDialog1;
        private SaveFileDialog saveFileDialog1;
        private ToolStripLabel toolStripLabel1;
        private ToolStripButton BUTTON_EDITOR_MODE_SELECT;
        private ToolStripButton BUTTON_EDITOR_MODE_MOVE;
        private ToolStripButton BUTTON_EDITOR_MODE_PLACE;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripButton BUTTON_DELETE;
        private HScrollBar hScrollBar1;
        private VScrollBar vScrollBar1;
        private Button BUTTON_IMPORT_ASSET;
        private ListBox listBox_Classes;
        private Label label1;
        private Label label2;
        private ToolStripSeparator toolStripSeparator2;
        private ToolStripButton BUTTON_EDIT_ROOM;
        private Panel panel1;
        private Panel panel2;
        private Panel panel3;
        private ComboBox Align_Relative;
        private Label label5;
        private Button BUTTON_ALIGN_10;
        private Button BUTTON_ALIGN_1;
        private Button BUTTON_ALIGN_6;
        private Button BUTTON_ALIGN_2;
        private Button BUTTON_ALIGN_7;
        private Button BUTTON_ALIGN_3;
        private Button BUTTON_ALIGN_8;
        private Button BUTTON_ALIGN_4;
        private Button BUTTON_ALIGN_9;
        private Button BUTTON_ALIGN_5;
        private NumericUpDown ZOOM;
        public PropertyGrid propertyGrid1;
        private Panel panel4;
        private Label label3;
        private TabControl tabControl1;
        private TabPage tabPage2;
        private ListBox listBox_Assets1;
        private TabPage tabPage3;
        private ListBox listBox_Assets2;
        private ToolStripSeparator toolStripSeparator3;
        private ToolStripButton BUTTON_UNDO;
        private ToolStripButton BUTTON_REDO;
        private ToolStripMenuItem undoToolStripMenuItem;
        private ToolStripMenuItem redoToolStripMenuItem;
        private ToolStripMenuItem viewToolStripMenuItem;
        private ToolStripMenuItem ViewMenuHideOverlay;
        private ToolStripMenuItem ViewMenuHideGrid;
        private ToolStripMenuItem ViewMenuHideCoordinates;
        private ToolStripMenuItem ViewMenuHideMovementPath;
        private ToolStripMenuItem NewFileToolstripOption;
        private ToolStripSeparator toolStripSeparator5;
        private ToolStripSeparator toolStripSeparator4;
        private ToolStripMenuItem deselectAllToolStripMenuItem;
        private TabPage tabPage1;
        private ListBox listBox_Assets0;


    }
}

