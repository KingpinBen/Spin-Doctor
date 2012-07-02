//--------------------------------------------------------------------------
//--    
//--    Spin Doctor - MenuScreen
//--    
//--    Current Revision 1.005
//--    
//--    Description
//--    ===============
//--    
//--    
//--    Revision List
//--    ===============
//--    BenG - Initial
//--    BenG - Added an easy shading to the underlying screen (Gameplay)
//--    BenG - Introduced input methods to find what options
//--    BenG - Now working, but needs to be made better/towards desired
//--           design...
//--    BenG - Fixed mouse selection detection and correctly spaced out options
//--    BenG - Updated menuscreent to be a base class. Old MenuScreen is now 
//--           GameMenu. Moved completeAction() from MenuOptions into Menu for
//--           greater global option control.
//--    BenG - Better memory handling
//--    
//--    TBD
//--    ==============
//--    Make selecting "Return to HUB" do something.
//--    
//--
//--
//--------------------------------------------------------------------------

//#define Development

#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using GameLibrary.Assists;
using GameLibrary.Drawing;
using GameLibrary.Managers;
using Microsoft.Xna.Framework.Content;

#endregion

namespace GameLibrary.Screens.Menu
{
    public class MenuScreen : Screen
    {
        #region Fields/Variables

        protected MenuOption[,] _menuItemArray;

        protected Point _menuArrayCount;
        private Point _selectionOption;

        protected bool isPopUp;

        private SpriteFont _font;

        public SpriteFont Font
        {
            get
            {
                return _font;
            }

            protected set
            {
                _font = value;
            }
        }
        public Point SelectionOption
        {
            get
            {
                return _selectionOption;
            }
            protected set
            {
                _selectionOption = value;
            }
        }


        #endregion

        #region Constructor

        public MenuScreen(string name)
            : base(name)
        {
            _selectionOption = Point.Zero;
        }
        public MenuScreen()
            : base("PauseMenu")
        {
            _selectionOption = Point.Zero;
        }
        #endregion

        #region Load
        public override void Load()
        {

        }
        #endregion

        #region Update
        public override void Update(GameTime gameTime)
        {
            HandleInput();

            for (int x = 0; x < _menuArrayCount.X; x++)
            {
                for (int y = 0; y < _menuArrayCount.Y; y++)
                {
                    _menuItemArray[x, y].Update(gameTime);
                }
            }
        }
        #endregion

        #region Draw
        public override void Draw(SpriteBatch sb)
        {
            if (!isPopUp)
            {
                //  Adds this on top to hide more gameplay.
                sb.Draw(Screen_Manager.Textures[0],
                    new Rectangle(0, 0, (int)Screen_Manager.GraphicsDevice.Viewport.Width, (int)Screen_Manager.GraphicsDevice.Viewport.Height),
                    Color.Black * 0.8f);
            }

            for (int x = 0; x < _menuArrayCount.X; x++)
            {
                for (int y = 0; y < _menuArrayCount.Y; y++)
                {
                    _menuItemArray[x, y].Draw(sb);
                }
            }
        }
        #endregion

        #region FindMenuOption
        /// <summary>
        /// Checks the menu options area to see if its been selected.
        /// PC ONLY
        /// </summary>
        /// <returns>Menu Item index</returns>
        private void FindMenuOption()
        {
            for (int x = 0; x < _menuArrayCount.X; x++) 
            {
                for (int y = 0; y < _menuArrayCount.Y; y++)
                {
                    int height = _menuItemArray[x, y].Height;
                    int width = _menuItemArray[x, y].Width;

                    Rectangle itemArea = new Rectangle(
                        (int)_menuItemArray[x, y].Position.X - width / 2,
                        (int)_menuItemArray[x, y].Position.Y - height / 2,
                        width, height);

                    Vector2 mousePos = Input.Cursor;

                    if (itemArea.Contains((int)mousePos.X, (int)mousePos.Y))
                    {
                        SelectionOption = new Point(x, y);
                    }
                }
            }
        }

        #endregion

        #region HandleInput
        /// <summary>
        /// Method contains all functionality for checking user input.
        /// </summary>
        private void HandleInput()
        {
            if (Input.isGamePad)
            {
                Point selectedOption = this.SelectionOption;

                #region Gamepad
                if (Input.GP_DPDown)
                {
                    //  Increment Y selected position
                    if (SelectionOption.Y + 1 >= _menuArrayCount.Y)
                        selectedOption.Y = 0;
                    else
                        selectedOption.Y++;
                }

                if (Input.GP_DPUp)
                {
                    //  Decrement Y selected position
                    if (SelectionOption.Y - 1 < 0)
                        selectedOption.Y = _menuArrayCount.Y - 1;
                    else
                        selectedOption.Y--;
                }

                if (Input.GP_DPLeft)
                {
                    //  Decrement X selected position
                    if (SelectionOption.X - 1 < 0)
                        selectedOption.X = _menuArrayCount.X - 1;
                    else
                        selectedOption.X--;
                }

                if (Input.GP_DPRight)
                {
                    //  Increment X selected position
                    if (SelectionOption.X + 1 >= _menuArrayCount.X)
                        selectedOption.X = 0;
                    else
                        selectedOption.X++;
                }

                if (Input.MenuSelect())
                    CompleteAction(_menuItemArray[SelectionOption.X, SelectionOption.Y].OptionType);

                if (Input.Return())
                    Screen_Manager.DeleteScreen();

                this.SelectionOption = selectedOption;
                #endregion
            }
            else
            {
                // Keyboard and Mouse

                if (SelectionOption.X < 0 || SelectionOption.Y < 0) return;

                FindMenuOption();

                if (Input.MenuSelect())
                    CompleteAction(_menuItemArray[SelectionOption.X, SelectionOption.Y].OptionType);
            }

            for (int x = 0; x < _menuArrayCount.X; x++)
            {
                for (int y = 0; y < _menuArrayCount.Y; y++)
                {
                    if (x == SelectionOption.X && y == SelectionOption.Y)
                        _menuItemArray[x, y].Highlighted = true;
                    else
                        _menuItemArray[x, y].Highlighted = false;
                }
            }
        }
        #endregion

        #region CompleteAction

        protected virtual void CompleteAction(optionType type)
        {

        }
        #endregion
    }
}