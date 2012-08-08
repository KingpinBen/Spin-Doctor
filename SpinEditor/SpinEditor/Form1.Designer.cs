using System.Windows.Forms;
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
            this.ViewMenuHideSpawn = new System.Windows.Forms.ToolStripMenuItem();
            this.ViewMenuHideEvents = new System.Windows.Forms.ToolStripMenuItem();
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
            this.listBox_Classes = new System.Windows.Forms.ListBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.AlignmentBox = new System.Windows.Forms.GroupBox();
            this.BUTTON_ALIGN_10 = new System.Windows.Forms.Button();
            this.Align_Relative = new System.Windows.Forms.ComboBox();
            this.BUTTON_ALIGN_9 = new System.Windows.Forms.Button();
            this.BUTTON_ALIGN_8 = new System.Windows.Forms.Button();
            this.BUTTON_ALIGN_7 = new System.Windows.Forms.Button();
            this.BUTTON_ALIGN_6 = new System.Windows.Forms.Button();
            this.Horizontal_Align_Button = new System.Windows.Forms.Button();
            this.BUTTON_ALIGN_2 = new System.Windows.Forms.Button();
            this.BUTTON_ALIGN_3 = new System.Windows.Forms.Button();
            this.BUTTON_ALIGN_4 = new System.Windows.Forms.Button();
            this.BUTTON_ALIGN_5 = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.NewObjectBox = new System.Windows.Forms.GroupBox();
            this.assetLocTextBox1 = new System.Windows.Forms.TextBox();
            this.assetSelectDialog3 = new System.Windows.Forms.Button();
            this.assetSelectDialog1 = new System.Windows.Forms.Button();
            this.assetLocTextBox3 = new System.Windows.Forms.TextBox();
            this.assetSelectDialog2 = new System.Windows.Forms.Button();
            this.assetLocTextBox2 = new System.Windows.Forms.TextBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.xnA_RenderControl1 = new SpinEditor.XNA_RenderControl();
            this.openAssetFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.menuStrip1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.AlignmentBox.SuspendLayout();
            this.NewObjectBox.SuspendLayout();
            this.panel2.SuspendLayout();
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
            this.ViewMenuHideMovementPath,
            this.ViewMenuHideSpawn,
            this.ViewMenuHideEvents});
            this.viewToolStripMenuItem.Name = "viewToolStripMenuItem";
            this.viewToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.viewToolStripMenuItem.Text = "View";
            // 
            // ViewMenuHideOverlay
            // 
            this.ViewMenuHideOverlay.Name = "ViewMenuHideOverlay";
            this.ViewMenuHideOverlay.Size = new System.Drawing.Size(204, 22);
            this.ViewMenuHideOverlay.Text = "Hide Object Overlay";
            this.ViewMenuHideOverlay.Click += new System.EventHandler(this.ViewMenuHideOverlay_Click);
            // 
            // ViewMenuHideGrid
            // 
            this.ViewMenuHideGrid.Name = "ViewMenuHideGrid";
            this.ViewMenuHideGrid.Size = new System.Drawing.Size(204, 22);
            this.ViewMenuHideGrid.Text = "Hide Grid";
            this.ViewMenuHideGrid.Click += new System.EventHandler(this.ViewMenuHideGrid_Click);
            // 
            // ViewMenuHideCoordinates
            // 
            this.ViewMenuHideCoordinates.Name = "ViewMenuHideCoordinates";
            this.ViewMenuHideCoordinates.Size = new System.Drawing.Size(204, 22);
            this.ViewMenuHideCoordinates.Text = "Hide Coordinates";
            this.ViewMenuHideCoordinates.Click += new System.EventHandler(this.ViewMenuHideCoordinates_Click);
            // 
            // ViewMenuHideMovementPath
            // 
            this.ViewMenuHideMovementPath.Name = "ViewMenuHideMovementPath";
            this.ViewMenuHideMovementPath.Size = new System.Drawing.Size(204, 22);
            this.ViewMenuHideMovementPath.Text = "Hide Movement Lines";
            this.ViewMenuHideMovementPath.Click += new System.EventHandler(this.ViewMenuHideMovementPath_Click);
            // 
            // ViewMenuHideSpawn
            // 
            this.ViewMenuHideSpawn.Name = "ViewMenuHideSpawn";
            this.ViewMenuHideSpawn.Size = new System.Drawing.Size(204, 22);
            this.ViewMenuHideSpawn.Text = "Hide Player Spawn";
            this.ViewMenuHideSpawn.Click += new System.EventHandler(this.ViewMenuHideSpawn_Click);
            // 
            // ViewMenuHideEvents
            // 
            this.ViewMenuHideEvents.Checked = true;
            this.ViewMenuHideEvents.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ViewMenuHideEvents.Name = "ViewMenuHideEvents";
            this.ViewMenuHideEvents.Size = new System.Drawing.Size(204, 22);
            this.ViewMenuHideEvents.Text = "Hide Event Objects Lines";
            this.ViewMenuHideEvents.Click += new System.EventHandler(this.ViewMenuHideEvents_Click);
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
            // saveFileDialog1
            // 
            this.saveFileDialog1.Title = "Save Level";
            // 
            // hScrollBar1
            // 
            this.hScrollBar1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.hScrollBar1.Location = new System.Drawing.Point(0, 635);
            this.hScrollBar1.Name = "hScrollBar1";
            this.hScrollBar1.Size = new System.Drawing.Size(946, 16);
            this.hScrollBar1.TabIndex = 4;
            this.hScrollBar1.Scroll += new System.Windows.Forms.ScrollEventHandler(this.hScrollBar1_Scroll);
            // 
            // vScrollBar1
            // 
            this.vScrollBar1.Dock = System.Windows.Forms.DockStyle.Right;
            this.vScrollBar1.Location = new System.Drawing.Point(946, 0);
            this.vScrollBar1.Name = "vScrollBar1";
            this.vScrollBar1.Size = new System.Drawing.Size(20, 651);
            this.vScrollBar1.TabIndex = 5;
            this.vScrollBar1.Scroll += new System.Windows.Forms.ScrollEventHandler(this.vScrollBar1_Scroll);
            // 
            // propertyGrid1
            // 
            this.propertyGrid1.Dock = System.Windows.Forms.DockStyle.Top;
            this.propertyGrid1.Location = new System.Drawing.Point(0, 0);
            this.propertyGrid1.Name = "propertyGrid1";
            this.propertyGrid1.Size = new System.Drawing.Size(367, 349);
            this.propertyGrid1.TabIndex = 6;
            // 
            // listBox_Classes
            // 
            this.listBox_Classes.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.listBox_Classes.BackColor = System.Drawing.SystemColors.Window;
            this.listBox_Classes.FormattingEnabled = true;
            this.listBox_Classes.Items.AddRange(new object[] {
            "Bounce Pad",
            "Cushioned Platform",
            "Decal",
            "Door",
            "Ladder",
            "Moving Platform",
            "Note",
            "One-Sided Platform",
            "Particle Emitter",
            "Piston",
            "Pushing Platform",
            "Physics Object",
            "Rope",
            "Rotate Room Button",
            "Rotating Platform",
            "Saw",
            "Spikes",
            "Sprite",
            "Static Object",
            "Trigger"});
            this.listBox_Classes.Location = new System.Drawing.Point(21, 16);
            this.listBox_Classes.Name = "listBox_Classes";
            this.listBox_Classes.Size = new System.Drawing.Size(338, 95);
            this.listBox_Classes.TabIndex = 9;
            this.listBox_Classes.SelectedIndexChanged += new System.EventHandler(this.listBox_Classes_SelectedIndexChanged);
            // 
            // panel1
            // 
            this.panel1.AutoScroll = true;
            this.panel1.Controls.Add(this.AlignmentBox);
            this.panel1.Controls.Add(this.NewObjectBox);
            this.panel1.Controls.Add(this.propertyGrid1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel1.Location = new System.Drawing.Point(966, 49);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(384, 651);
            this.panel1.TabIndex = 12;
            // 
            // AlignmentBox
            // 
            this.AlignmentBox.Controls.Add(this.BUTTON_ALIGN_10);
            this.AlignmentBox.Controls.Add(this.Align_Relative);
            this.AlignmentBox.Controls.Add(this.BUTTON_ALIGN_9);
            this.AlignmentBox.Controls.Add(this.BUTTON_ALIGN_8);
            this.AlignmentBox.Controls.Add(this.BUTTON_ALIGN_7);
            this.AlignmentBox.Controls.Add(this.BUTTON_ALIGN_6);
            this.AlignmentBox.Controls.Add(this.Horizontal_Align_Button);
            this.AlignmentBox.Controls.Add(this.BUTTON_ALIGN_2);
            this.AlignmentBox.Controls.Add(this.BUTTON_ALIGN_3);
            this.AlignmentBox.Controls.Add(this.BUTTON_ALIGN_4);
            this.AlignmentBox.Controls.Add(this.BUTTON_ALIGN_5);
            this.AlignmentBox.Controls.Add(this.label5);
            this.AlignmentBox.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.AlignmentBox.Location = new System.Drawing.Point(0, 349);
            this.AlignmentBox.Name = "AlignmentBox";
            this.AlignmentBox.Size = new System.Drawing.Size(367, 140);
            this.AlignmentBox.TabIndex = 36;
            this.AlignmentBox.TabStop = false;
            this.AlignmentBox.Text = "Alignment";
            // 
            // BUTTON_ALIGN_10
            // 
            this.BUTTON_ALIGN_10.Image = ((System.Drawing.Image)(resources.GetObject("BUTTON_ALIGN_10.Image")));
            this.BUTTON_ALIGN_10.Location = new System.Drawing.Point(265, 85);
            this.BUTTON_ALIGN_10.Name = "BUTTON_ALIGN_10";
            this.BUTTON_ALIGN_10.Size = new System.Drawing.Size(34, 34);
            this.BUTTON_ALIGN_10.TabIndex = 18;
            this.BUTTON_ALIGN_10.UseVisualStyleBackColor = true;
            this.BUTTON_ALIGN_10.Click += new System.EventHandler(this.TB_Vertical_Align_Click);
            // 
            // Align_Relative
            // 
            this.Align_Relative.FormattingEnabled = true;
            this.Align_Relative.Items.AddRange(new object[] {
            "Last Selected",
            "First Selected",
            "Selection"});
            this.Align_Relative.Location = new System.Drawing.Point(103, 18);
            this.Align_Relative.Name = "Align_Relative";
            this.Align_Relative.Size = new System.Drawing.Size(196, 21);
            this.Align_Relative.TabIndex = 26;
            this.Align_Relative.SelectedIndexChanged += new System.EventHandler(this.Align_Relative_SelectedIndexChanged);
            // 
            // BUTTON_ALIGN_9
            // 
            this.BUTTON_ALIGN_9.Image = ((System.Drawing.Image)(resources.GetObject("BUTTON_ALIGN_9.Image")));
            this.BUTTON_ALIGN_9.Location = new System.Drawing.Point(225, 85);
            this.BUTTON_ALIGN_9.Name = "BUTTON_ALIGN_9";
            this.BUTTON_ALIGN_9.Size = new System.Drawing.Size(34, 34);
            this.BUTTON_ALIGN_9.TabIndex = 19;
            this.BUTTON_ALIGN_9.UseVisualStyleBackColor = true;
            this.BUTTON_ALIGN_9.Click += new System.EventHandler(this.B_Vertical_Align_Click);
            // 
            // BUTTON_ALIGN_8
            // 
            this.BUTTON_ALIGN_8.Image = ((System.Drawing.Image)(resources.GetObject("BUTTON_ALIGN_8.Image")));
            this.BUTTON_ALIGN_8.Location = new System.Drawing.Point(185, 85);
            this.BUTTON_ALIGN_8.Name = "BUTTON_ALIGN_8";
            this.BUTTON_ALIGN_8.Size = new System.Drawing.Size(34, 34);
            this.BUTTON_ALIGN_8.TabIndex = 20;
            this.BUTTON_ALIGN_8.UseVisualStyleBackColor = true;
            this.BUTTON_ALIGN_8.Click += new System.EventHandler(this.C_Vertical_Align_Click);
            // 
            // BUTTON_ALIGN_7
            // 
            this.BUTTON_ALIGN_7.Image = ((System.Drawing.Image)(resources.GetObject("BUTTON_ALIGN_7.Image")));
            this.BUTTON_ALIGN_7.Location = new System.Drawing.Point(145, 85);
            this.BUTTON_ALIGN_7.Name = "BUTTON_ALIGN_7";
            this.BUTTON_ALIGN_7.Size = new System.Drawing.Size(34, 34);
            this.BUTTON_ALIGN_7.TabIndex = 21;
            this.BUTTON_ALIGN_7.UseVisualStyleBackColor = true;
            this.BUTTON_ALIGN_7.Click += new System.EventHandler(this.T_Vertical_Align_Click);
            // 
            // BUTTON_ALIGN_6
            // 
            this.BUTTON_ALIGN_6.Image = ((System.Drawing.Image)(resources.GetObject("BUTTON_ALIGN_6.Image")));
            this.BUTTON_ALIGN_6.Location = new System.Drawing.Point(103, 85);
            this.BUTTON_ALIGN_6.Name = "BUTTON_ALIGN_6";
            this.BUTTON_ALIGN_6.Size = new System.Drawing.Size(34, 34);
            this.BUTTON_ALIGN_6.TabIndex = 22;
            this.BUTTON_ALIGN_6.UseVisualStyleBackColor = true;
            this.BUTTON_ALIGN_6.Click += new System.EventHandler(this.BT_Vertical_Align_Click);
            // 
            // Horizontal_Align_Button
            // 
            this.Horizontal_Align_Button.Image = ((System.Drawing.Image)(resources.GetObject("Horizontal_Align_Button.Image")));
            this.Horizontal_Align_Button.Location = new System.Drawing.Point(105, 45);
            this.Horizontal_Align_Button.Name = "Horizontal_Align_Button";
            this.Horizontal_Align_Button.Size = new System.Drawing.Size(34, 34);
            this.Horizontal_Align_Button.TabIndex = 13;
            this.Horizontal_Align_Button.UseVisualStyleBackColor = true;
            this.Horizontal_Align_Button.Click += new System.EventHandler(this.RL_Horizontal_Align_Click);
            // 
            // BUTTON_ALIGN_2
            // 
            this.BUTTON_ALIGN_2.Image = ((System.Drawing.Image)(resources.GetObject("BUTTON_ALIGN_2.Image")));
            this.BUTTON_ALIGN_2.Location = new System.Drawing.Point(145, 45);
            this.BUTTON_ALIGN_2.Name = "BUTTON_ALIGN_2";
            this.BUTTON_ALIGN_2.Size = new System.Drawing.Size(34, 34);
            this.BUTTON_ALIGN_2.TabIndex = 14;
            this.BUTTON_ALIGN_2.UseVisualStyleBackColor = true;
            this.BUTTON_ALIGN_2.Click += new System.EventHandler(this.L_Horizontal_Align_Click);
            // 
            // BUTTON_ALIGN_3
            // 
            this.BUTTON_ALIGN_3.Image = ((System.Drawing.Image)(resources.GetObject("BUTTON_ALIGN_3.Image")));
            this.BUTTON_ALIGN_3.Location = new System.Drawing.Point(185, 45);
            this.BUTTON_ALIGN_3.Name = "BUTTON_ALIGN_3";
            this.BUTTON_ALIGN_3.Size = new System.Drawing.Size(34, 34);
            this.BUTTON_ALIGN_3.TabIndex = 15;
            this.BUTTON_ALIGN_3.UseVisualStyleBackColor = true;
            this.BUTTON_ALIGN_3.Click += new System.EventHandler(this.C_Horizontal_Align_Click);
            // 
            // BUTTON_ALIGN_4
            // 
            this.BUTTON_ALIGN_4.Image = ((System.Drawing.Image)(resources.GetObject("BUTTON_ALIGN_4.Image")));
            this.BUTTON_ALIGN_4.Location = new System.Drawing.Point(225, 45);
            this.BUTTON_ALIGN_4.Name = "BUTTON_ALIGN_4";
            this.BUTTON_ALIGN_4.Size = new System.Drawing.Size(34, 34);
            this.BUTTON_ALIGN_4.TabIndex = 16;
            this.BUTTON_ALIGN_4.UseVisualStyleBackColor = true;
            this.BUTTON_ALIGN_4.Click += new System.EventHandler(this.R_Horizontal_Align_Click);
            // 
            // BUTTON_ALIGN_5
            // 
            this.BUTTON_ALIGN_5.Image = ((System.Drawing.Image)(resources.GetObject("BUTTON_ALIGN_5.Image")));
            this.BUTTON_ALIGN_5.Location = new System.Drawing.Point(265, 45);
            this.BUTTON_ALIGN_5.Name = "BUTTON_ALIGN_5";
            this.BUTTON_ALIGN_5.Size = new System.Drawing.Size(34, 34);
            this.BUTTON_ALIGN_5.TabIndex = 17;
            this.BUTTON_ALIGN_5.UseVisualStyleBackColor = true;
            this.BUTTON_ALIGN_5.Click += new System.EventHandler(this.LR_Horizontal_Align_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 21);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(91, 13);
            this.label5.TabIndex = 25;
            this.label5.Text = "Align Relative To:";
            // 
            // NewObjectBox
            // 
            this.NewObjectBox.Controls.Add(this.assetLocTextBox1);
            this.NewObjectBox.Controls.Add(this.assetSelectDialog3);
            this.NewObjectBox.Controls.Add(this.assetSelectDialog1);
            this.NewObjectBox.Controls.Add(this.assetLocTextBox3);
            this.NewObjectBox.Controls.Add(this.listBox_Classes);
            this.NewObjectBox.Controls.Add(this.assetSelectDialog2);
            this.NewObjectBox.Controls.Add(this.assetLocTextBox2);
            this.NewObjectBox.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.NewObjectBox.Location = new System.Drawing.Point(0, 489);
            this.NewObjectBox.Name = "NewObjectBox";
            this.NewObjectBox.Size = new System.Drawing.Size(367, 207);
            this.NewObjectBox.TabIndex = 35;
            this.NewObjectBox.TabStop = false;
            this.NewObjectBox.Text = "New Object";
            // 
            // assetLocTextBox1
            // 
            this.assetLocTextBox1.BackColor = System.Drawing.SystemColors.Control;
            this.assetLocTextBox1.ForeColor = System.Drawing.SystemColors.WindowText;
            this.assetLocTextBox1.Location = new System.Drawing.Point(12, 119);
            this.assetLocTextBox1.Name = "assetLocTextBox1";
            this.assetLocTextBox1.ReadOnly = true;
            this.assetLocTextBox1.Size = new System.Drawing.Size(236, 20);
            this.assetLocTextBox1.TabIndex = 30;
            // 
            // assetSelectDialog3
            // 
            this.assetSelectDialog3.Location = new System.Drawing.Point(258, 169);
            this.assetSelectDialog3.Name = "assetSelectDialog3";
            this.assetSelectDialog3.Size = new System.Drawing.Size(86, 23);
            this.assetSelectDialog3.TabIndex = 34;
            this.assetSelectDialog3.Text = "Select Texture";
            this.assetSelectDialog3.UseVisualStyleBackColor = true;
            this.assetSelectDialog3.Click += new System.EventHandler(this.assetSelectDialog3_Click);
            // 
            // assetSelectDialog1
            // 
            this.assetSelectDialog1.Location = new System.Drawing.Point(258, 117);
            this.assetSelectDialog1.Name = "assetSelectDialog1";
            this.assetSelectDialog1.Size = new System.Drawing.Size(86, 23);
            this.assetSelectDialog1.TabIndex = 29;
            this.assetSelectDialog1.Text = "Select Texture";
            this.assetSelectDialog1.UseVisualStyleBackColor = true;
            this.assetSelectDialog1.Click += new System.EventHandler(this.assetSelectDialog1_Click);
            // 
            // assetLocTextBox3
            // 
            this.assetLocTextBox3.Location = new System.Drawing.Point(12, 171);
            this.assetLocTextBox3.Name = "assetLocTextBox3";
            this.assetLocTextBox3.ReadOnly = true;
            this.assetLocTextBox3.Size = new System.Drawing.Size(236, 20);
            this.assetLocTextBox3.TabIndex = 32;
            // 
            // assetSelectDialog2
            // 
            this.assetSelectDialog2.Location = new System.Drawing.Point(258, 143);
            this.assetSelectDialog2.Name = "assetSelectDialog2";
            this.assetSelectDialog2.Size = new System.Drawing.Size(86, 23);
            this.assetSelectDialog2.TabIndex = 33;
            this.assetSelectDialog2.Text = "Select Texture";
            this.assetSelectDialog2.UseVisualStyleBackColor = true;
            this.assetSelectDialog2.Click += new System.EventHandler(this.assetSelectDialog2_Click);
            // 
            // assetLocTextBox2
            // 
            this.assetLocTextBox2.Location = new System.Drawing.Point(12, 145);
            this.assetLocTextBox2.Name = "assetLocTextBox2";
            this.assetLocTextBox2.ReadOnly = true;
            this.assetLocTextBox2.Size = new System.Drawing.Size(236, 20);
            this.assetLocTextBox2.TabIndex = 31;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.hScrollBar1);
            this.panel2.Controls.Add(this.vScrollBar1);
            this.panel2.Controls.Add(this.xnA_RenderControl1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 49);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(966, 651);
            this.panel2.TabIndex = 13;
            // 
            // xnA_RenderControl1
            // 
            this.xnA_RenderControl1.Camera = null;
            this.xnA_RenderControl1.contentMan = null;
            this.xnA_RenderControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.xnA_RenderControl1.form1 = null;
            this.xnA_RenderControl1.HideCoordinates = false;
            this.xnA_RenderControl1.HideEventTargets = false;
            this.xnA_RenderControl1.HideGrid = false;
            this.xnA_RenderControl1.HideMovementPath = false;
            this.xnA_RenderControl1.HideOverlay = false;
            this.xnA_RenderControl1.HidePlayerSpawn = false;
            this.xnA_RenderControl1.levelBackground = null;
            this.xnA_RenderControl1.levelDimensions = new Microsoft.Xna.Framework.Vector2(0F, 0F);
            this.xnA_RenderControl1.Location = new System.Drawing.Point(0, 0);
            this.xnA_RenderControl1.Name = "xnA_RenderControl1";
            this.xnA_RenderControl1.Size = new System.Drawing.Size(966, 651);
            this.xnA_RenderControl1.TabIndex = 0;
            this.xnA_RenderControl1.Text = "xnA_RenderControl1";
            this.xnA_RenderControl1.Click += new System.EventHandler(this.xnA_RenderControl1_Click);
            this.xnA_RenderControl1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.xnA_RenderControl1_MouseDown);
            this.xnA_RenderControl1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.xnA_RenderControl1_MouseMove);
            // 
            // openAssetFileDialog1
            // 
            this.openAssetFileDialog1.FileName = "Select Asset";
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
            this.AlignmentBox.ResumeLayout(false);
            this.AlignmentBox.PerformLayout();
            this.NewObjectBox.ResumeLayout(false);
            this.NewObjectBox.PerformLayout();
            this.panel2.ResumeLayout(false);
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
        private ListBox listBox_Classes;
        private ToolStripSeparator toolStripSeparator2;
        private ToolStripButton BUTTON_EDIT_ROOM;
        private Panel panel1;
        private Panel panel2;
        private ComboBox Align_Relative;
        private Label label5;
        private Button BUTTON_ALIGN_10;
        private Button Horizontal_Align_Button;
        private Button BUTTON_ALIGN_6;
        private Button BUTTON_ALIGN_2;
        private Button BUTTON_ALIGN_7;
        private Button BUTTON_ALIGN_3;
        private Button BUTTON_ALIGN_8;
        private Button BUTTON_ALIGN_4;
        private Button BUTTON_ALIGN_9;
        private Button BUTTON_ALIGN_5;
        public PropertyGrid propertyGrid1;
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
        private ToolStripMenuItem ViewMenuHideSpawn;
        private Button assetSelectDialog1;
        private OpenFileDialog openAssetFileDialog1;
        private TextBox assetLocTextBox1;
        private TextBox assetLocTextBox3;
        private TextBox assetLocTextBox2;
        private Button assetSelectDialog3;
        private Button assetSelectDialog2;
        private GroupBox NewObjectBox;
        private ToolStripMenuItem ViewMenuHideEvents;
        private GroupBox AlignmentBox;


    }
}

