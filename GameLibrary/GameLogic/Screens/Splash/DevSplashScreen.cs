//--------------------------------------------------------------------------
//--    
//--    Spin Doctor - Splash Screen
//--    
//--    Current Version: 1.1
//--    
//--    Description
//--    ===============
//--    Displays the splash info of the game.
//--    
//--    Revision List
//--    ===============
//--    1.0 : BenG - Initial
//--    1.1 : BenG - Added new Statevalues and changed positioning.
//--    
//--    
//--    
//--    TBD
//--    ==============
//--    This should be completed later
//--    
//--    
//--------------------------------------------------------------------------

#region Using Statements
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using GameLibrary.GameLogic.Controls;

#endregion

namespace GameLibrary.GameLogic.Screens.Splash
{
    public class DevSplashScreen : Screen
    {
        private Vector2 _position;
        private Texture2D _logo;
        private Vector2 _origin;
        private string _textureLocation;

        protected ContentManager content;

        public DevSplashScreen(string tex, GraphicsDevice graphics)
            : base("DeveloperSplashScreen", graphics)
        {
            this._textureLocation = tex;
            _position = new Vector2(graphics.Viewport.Width, graphics.Viewport.Height) * 0.5f;
        }

        public override void Load()
        {
            _logo = content.Load<Texture2D>(_textureLocation);
            _origin = new Vector2(_logo.Width, _logo.Height) * 0.5f;           
        }

        public override void Unload()
        {
            content.Unload();
        }

        public override void Update(GameTime gameTime)
        {
            if (InputManager.Instance.Escape || InputManager.Instance.GP_Start || InputManager.Instance.lMouseButton)
            {
                ScreenManager.DeleteScreen();
            }

            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch sb)
        {
            this._graphicsDevice.Clear(Color.Black);

            sb.Begin();
            sb.Draw(_logo, _position, null, Color.White, 0f, _origin, 0.3f, SpriteEffects.None, 0f);
            sb.End();
        }
    }
}
