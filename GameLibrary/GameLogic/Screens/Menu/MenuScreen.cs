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
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using GameLibrary.GameLogic.Controls;
using GameLibrary.GameLogic;
using GameLibrary.GameLogic.Screens.Menu.Options;

#endregion

namespace GameLibrary.GameLogic.Screens.Menu
{
    public class MenuScreen : Screen
    {
        #region Fields/Variables

        protected MenuItem[,] _menuItemArray;

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

        public MenuScreen(string name, GraphicsDevice graphics)
            : base(name, graphics)
        {
            _selectionOption = Point.Zero;
        }
        public MenuScreen(GraphicsDevice graphics)
            : base("PauseMenu", graphics)
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
                sb.Draw(ScreenManager.Textures[0],
                    new Rectangle(0, 0, (int)ScreenManager.GraphicsDevice.Viewport.Width, (int)ScreenManager.GraphicsDevice.Viewport.Height),
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

        #region HandleInput
        /// <summary>
        /// Method contains all functionality for checking user input.
        /// </summary>
        private void HandleInput()
        {
            Point selectedOption = this.SelectionOption;

            if (InputManager.Instance.GP_DPDown || InputManager.Instance.IsNewKeyPress(Keys.Down))
            {
                //  Increment Y selected position
                if (SelectionOption.Y + 1 >= _menuArrayCount.Y)
                    selectedOption.Y = 0;
                else
                    selectedOption.Y++;
            }

            if (InputManager.Instance.GP_DPUp || InputManager.Instance.IsNewKeyPress(Keys.Up))
            {
                //  Decrement Y selected position
                if (SelectionOption.Y - 1 < 0)
                    selectedOption.Y = _menuArrayCount.Y - 1;
                else
                    selectedOption.Y--;
            }

            if (InputManager.Instance.GP_DPLeft || InputManager.Instance.IsNewKeyPress(Keys.Left))
            {
                //  Decrement X selected position
                if (SelectionOption.X - 1 < 0)
                    selectedOption.X = _menuArrayCount.X - 1;
                else
                    selectedOption.X--;
            }

            if (InputManager.Instance.GP_DPRight || InputManager.Instance.IsNewKeyPress(Keys.Right))
            {
                //  Increment X selected position
                if (SelectionOption.X + 1 >= _menuArrayCount.X)
                    selectedOption.X = 0;
                else
                    selectedOption.X++;
            }

            if (InputManager.Instance.MenuSelect())
            {
                CompleteAction(_menuItemArray[SelectionOption.X, SelectionOption.Y].OptionType);
            }

            if (InputManager.Instance.Return())
            {
                ScreenManager.DeleteScreen();
            }

            this.SelectionOption = selectedOption;


            for (int x = 0; x < _menuArrayCount.X; x++)
            {
                for (int y = 0; y < _menuArrayCount.Y; y++)
                {
                    if (x == SelectionOption.X && y == SelectionOption.Y)
                    {
                        _menuItemArray[x, y].Highlighted = true;
                    }
                    else
                    {
                        _menuItemArray[x, y].Highlighted = false;
                    }
                }
            }
        }
        #endregion

        #region CompleteAction

        protected virtual void CompleteAction(OptionType type)
        {

        }
        #endregion
    }
}