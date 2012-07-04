using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GameLibrary.Drawing;
using Microsoft.Xna.Framework;
using System.IO;

namespace SpinEditor
{
    public partial class RoomSetupForm : Form
    {
        public RoomTypeEnum roomType = RoomTypeEnum.Rotating;
        public RoomThemeEnum roomTheme = RoomThemeEnum.Industrial;
        public Vector2 roomSize = new Vector2(1, 1);
        public string rearWall = "";

        public RoomSetupForm()
        {
            InitializeComponent();
            typeComboBox.SelectedIndex = 0;
            themeComboBox.SelectedIndex = 0;
        }

        private void typeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (typeComboBox.SelectedIndex)
            {
                case 0:
                    {
                        roomSizeY.Value = roomSizeX.Value;
                        roomSizeY.Enabled = false;
                        roomType = RoomTypeEnum.Rotating;
                    }
                    break;
                case 1:
                    {
                        roomSizeY.Enabled = true;
                        roomType = RoomTypeEnum.NonRotating;
                    }
                    break;
                case 2:
                    {
                        roomSizeY.Enabled = true;
                        roomType = RoomTypeEnum.Hub;
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
                        roomTheme = RoomThemeEnum.Industrial;
                    }
                    break;
                case 1:
                    {
                        roomTheme = RoomThemeEnum.Medical;
                    }
                    break;
                case 2:
                    {
                        roomTheme = RoomThemeEnum.Study;
                    }
                    break;
                case 3:
                    {
                        roomTheme = RoomThemeEnum.General;
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
            //  The texture must be changed here.
            if (rearWall == "" && roomType != RoomTypeEnum.NonRotating)
            {
                MessageBox.Show("Put a texture name in for the background.");
                return false;
            }
            if (roomType != RoomTypeEnum.NonRotating)
            {
                rearWall = "Assets/Images/Textures/RoomSetup/" + rearWall;
            }
            else
            {
                rearWall = "Nonrotating";
            }

            return true;
        }
    }
}
