namespace SpinEditor
{
    partial class RoomSetupForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RoomSetupForm));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.drawWallsCheckBox1 = new System.Windows.Forms.CheckBox();
            this.label7 = new System.Windows.Forms.Label();
            this.backgroundTextBox = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.roomSizeY = new System.Windows.Forms.NumericUpDown();
            this.roomSizeX = new System.Windows.Forms.NumericUpDown();
            this.themeComboBox = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.typeComboBox = new System.Windows.Forms.ComboBox();
            this.BUTTON_CREATE_ROOM = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.BUTTON_LOAD_ROOM = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.roomSizeY)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.roomSizeX)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.drawWallsCheckBox1);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.backgroundTextBox);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.roomSizeY);
            this.groupBox1.Controls.Add(this.roomSizeX);
            this.groupBox1.Controls.Add(this.themeComboBox);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.typeComboBox);
            this.groupBox1.Controls.Add(this.BUTTON_CREATE_ROOM);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Location = new System.Drawing.Point(5, 32);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(183, 232);
            this.groupBox1.TabIndex = 13;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "New Room";
            // 
            // drawWallsCheckBox1
            // 
            this.drawWallsCheckBox1.AutoSize = true;
            this.drawWallsCheckBox1.Location = new System.Drawing.Point(9, 173);
            this.drawWallsCheckBox1.Name = "drawWallsCheckBox1";
            this.drawWallsCheckBox1.Size = new System.Drawing.Size(87, 17);
            this.drawWallsCheckBox1.TabIndex = 16;
            this.drawWallsCheckBox1.Text = "Insert Walls?";
            this.drawWallsCheckBox1.UseVisualStyleBackColor = true;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(51, 146);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(121, 13);
            this.label7.TabIndex = 15;
            this.label7.Text = "(filename - no extension)";
            // 
            // backgroundTextBox
            // 
            this.backgroundTextBox.Location = new System.Drawing.Point(71, 123);
            this.backgroundTextBox.Name = "backgroundTextBox";
            this.backgroundTextBox.Size = new System.Drawing.Size(100, 20);
            this.backgroundTextBox.TabIndex = 14;
            this.backgroundTextBox.TextChanged += new System.EventHandler(this.backgroundTextBox_TextChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 126);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(65, 13);
            this.label6.TabIndex = 13;
            this.label6.Text = "Background";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 98);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(40, 13);
            this.label5.TabIndex = 12;
            this.label5.Text = "Setting";
            // 
            // roomSizeY
            // 
            this.roomSizeY.Location = new System.Drawing.Point(134, 51);
            this.roomSizeY.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.roomSizeY.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.roomSizeY.Name = "roomSizeY";
            this.roomSizeY.Size = new System.Drawing.Size(38, 20);
            this.roomSizeY.TabIndex = 11;
            this.roomSizeY.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.roomSizeY.ValueChanged += new System.EventHandler(this.roomSizeY_ValueChanged);
            // 
            // roomSizeX
            // 
            this.roomSizeX.Location = new System.Drawing.Point(71, 51);
            this.roomSizeX.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.roomSizeX.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.roomSizeX.Name = "roomSizeX";
            this.roomSizeX.Size = new System.Drawing.Size(38, 20);
            this.roomSizeX.TabIndex = 10;
            this.roomSizeX.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.roomSizeX.ValueChanged += new System.EventHandler(this.roomSizeX_ValueChanged);
            // 
            // themeComboBox
            // 
            this.themeComboBox.FormattingEnabled = true;
            this.themeComboBox.Items.AddRange(new object[] {
            "Industrial",
            "Medical",
            "Mansion",
            "Other"});
            this.themeComboBox.Location = new System.Drawing.Point(71, 95);
            this.themeComboBox.Name = "themeComboBox";
            this.themeComboBox.Size = new System.Drawing.Size(101, 21);
            this.themeComboBox.TabIndex = 9;
            this.themeComboBox.SelectedIndexChanged += new System.EventHandler(this.themeComboBox_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "RoomType";
            // 
            // typeComboBox
            // 
            this.typeComboBox.FormattingEnabled = true;
            this.typeComboBox.Items.AddRange(new object[] {
            "Rotating",
            "Non Rotating",
            "Hub"});
            this.typeComboBox.Location = new System.Drawing.Point(71, 23);
            this.typeComboBox.Name = "typeComboBox";
            this.typeComboBox.Size = new System.Drawing.Size(101, 21);
            this.typeComboBox.TabIndex = 2;
            this.typeComboBox.SelectedIndexChanged += new System.EventHandler(this.typeComboBox_SelectedIndexChanged);
            // 
            // BUTTON_CREATE_ROOM
            // 
            this.BUTTON_CREATE_ROOM.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.BUTTON_CREATE_ROOM.Location = new System.Drawing.Point(9, 196);
            this.BUTTON_CREATE_ROOM.Name = "BUTTON_CREATE_ROOM";
            this.BUTTON_CREATE_ROOM.Size = new System.Drawing.Size(162, 23);
            this.BUTTON_CREATE_ROOM.TabIndex = 8;
            this.BUTTON_CREATE_ROOM.Text = "Create Room";
            this.BUTTON_CREATE_ROOM.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 53);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(58, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Room Size";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(114, 53);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(14, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "X";
            // 
            // BUTTON_LOAD_ROOM
            // 
            this.BUTTON_LOAD_ROOM.DialogResult = System.Windows.Forms.DialogResult.Yes;
            this.BUTTON_LOAD_ROOM.Location = new System.Drawing.Point(5, 3);
            this.BUTTON_LOAD_ROOM.Name = "BUTTON_LOAD_ROOM";
            this.BUTTON_LOAD_ROOM.Size = new System.Drawing.Size(85, 23);
            this.BUTTON_LOAD_ROOM.TabIndex = 12;
            this.BUTTON_LOAD_ROOM.Text = "Load Room";
            this.BUTTON_LOAD_ROOM.UseVisualStyleBackColor = true;
            // 
            // RoomSetupForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(193, 268);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.BUTTON_LOAD_ROOM);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "RoomSetupForm";
            this.Text = "Spin EDITOR";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.roomSizeY)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.roomSizeX)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox backgroundTextBox;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.NumericUpDown roomSizeY;
        private System.Windows.Forms.NumericUpDown roomSizeX;
        private System.Windows.Forms.ComboBox themeComboBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox typeComboBox;
        private System.Windows.Forms.Button BUTTON_CREATE_ROOM;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button BUTTON_LOAD_ROOM;
        public System.Windows.Forms.CheckBox drawWallsCheckBox1;
    }
}