﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Xna.Framework;
using System.IO;
using GameLibrary.Levels;

namespace SpinEditor
{
    public partial class RoomSetupForm : Form
    {
        public RoomType roomType = RoomType.Rotating;
        public RoomTheme roomTheme = RoomTheme.Industrial;
        public Vector2 roomSize = new Vector2(1, 1);
        public string rearWall = "";

        public RoomSetupForm()
        {
            InitializeComponent();
            typeComboBox.SelectedIndex = 0;
            themeComboBox.SelectedIndex = 0;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
        }

        private void typeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (typeComboBox.SelectedIndex)
            {
                case 0:
                    {
                        roomSizeY.Value = roomSizeX.Value;
                        roomSizeY.Enabled = false;
                        roomType = RoomType.Rotating;
                    }
                    break;
                case 1:
                    {
                        roomSizeY.Enabled = true;
                        roomType = RoomType.NonRotating;
                    }
                    break;
            }
        }

        private void roomSizeX_ValueChanged(object sender, EventArgs e)
        {
            roomSize.X = (int)roomSizeX.Value;
            if (typeComboBox.SelectedIndex == 0)
            {
                roomSizeY.Value = roomSizeX.Value;
                roomSize.Y = (int)roomSizeY.Value;
            }
        }
        
        private void roomSizeY_ValueChanged(object sender, EventArgs e)
        {
            roomSize.Y = (int)roomSizeY.Value;
        }
        
        private void themeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (themeComboBox.SelectedIndex)
            {
                case 0:
                    {
                        roomTheme = RoomTheme.Industrial;
                    }
                    break;
                case 1:
                    {
                        roomTheme = RoomTheme.Medical;
                    }
                    break;
                case 2:
                    {
                        roomTheme = RoomTheme.Study;
                    }
                    break;
                case 3:
                    {
                        roomTheme = RoomTheme.General;
                    }
                    break;
            }
        }

        private void backgroundTextBox_TextChanged(object sender, EventArgs e)
        {
            rearWall = backgroundTextBox.Text;
        }

        /// <summary>
        /// Check to see all the criteria are met to continue to creation.
        /// </summary>
        /// <returns></returns>
        public bool CheckIfComplete()
        {
            if (roomType == RoomType.Rotating)
            {
                string path = Directory.GetCurrentDirectory();

                if (rearWall == "")
                {
                    MessageBox.Show("Put a texture name in for the background.");
                    return false;
                }

                string fileloc = path + "\\Content\\Assets\\Images\\Textures\\RoomSetup\\" + rearWall + ".xnb";

                if (!File.Exists(fileloc))
                {
                    MessageBox.Show("File doesn't exist. Try again.");
                    return false;
                }

                rearWall = "Assets/Images/Textures/RoomSetup/" + rearWall;

                return true;
            }
            else
            {
                rearWall = "";
                return true;
            }
        }
    }
}
