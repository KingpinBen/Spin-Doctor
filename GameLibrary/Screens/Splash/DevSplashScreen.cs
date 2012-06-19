﻿//--------------------------------------------------------------------------
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
using GameLibrary.Assists;
using GameLibrary.Managers;
using Microsoft.Xna.Framework.Content;

#endregion

namespace GameLibrary.Screens
{
    public class DevSplashScreen : Screen
    {
        private Vector2 _position;
        private Texture2D _logo;
        private Vector2 _origin;
        private string _textureLocation;

        protected ContentManager content;

        public DevSplashScreen(string tex)
            : base("DeveloperSplashScreen", 2f)
        {
            this._textureLocation = tex;
            _position = Screen_Manager.Viewport / 2;
        }

        public override void Load()
        {
            _logo = content.Load<Texture2D>(_textureLocation);
            _origin = new Vector2(_logo.Width / 2, _logo.Height / 2);            
        }

        public override void Unload()
        {
            content.Unload();
        }

        public override void Update(GameTime gameTime)
        {
            if (Input.Escape || Input.GP_Start || Input.lMouseButton)
                Screen_Manager.DeleteScreen();

            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch sb)
        {
            Screen_Manager.Graphics.Clear(Color.Black);

            sb.Begin();
            sb.Draw(_logo, _position, null, Color.White, 0f, _origin, 0.3f, SpriteEffects.None, 0f);
            sb.End();
        }
    }
}
