//--------------------------------------------------------------------------
//--    
//--    Spin Doctor - Loading Screen
//--    
//--    
//--    Description
//--    ===============
//--    Loadingscreen 'overlay'
//--    
//--    Revision List
//--    ===============
//--    BenG - Initial 
//--    BenG - Allows alphas on on-screen items (texts/icons)
//--    
//--    
//--    TBD
//--    ==============
//--    
//--    Allow onscreen hints/tips to be written?
//--    
//--------------------------------------------------------------------------

#define Development

#region Using Statements
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using GameLibrary.GameLogic.Controls;
using GameLibrary.GameLogic;
using GameLibrary.Graphics.UI;
using GameLibrary.Graphics.Drawing;
#endregion

namespace GameLibrary.GameLogic.Screens
{
    public class LoadingScreen : Screen
    {
        #region Fields
        private Sprite sprite;
        private AnimatedText ContinueText;
        //public FullscreenPicture picture { get; set; }
        private FullscreenPicture _banner;

        private ContentManager content;

        #endregion

        #region Constructor
        public LoadingScreen(GraphicsDevice graphics)
            : base("LoadingScreen", graphics)
        {
            content = new ContentManager(ScreenManager.Game.Services, "Content");

            
            //picture = new FullscreenPicture();
        }
        #endregion

        #region Load
        public override void Load()
        {
            sprite = new Sprite();
            
            sprite.Scale = 0.4f;

            int locx = this._graphicsDevice.PresentationParameters.BackBufferWidth - (int)(102 * sprite.Scale);
            int locy = this._graphicsDevice.PresentationParameters.BackBufferHeight - (int)(102 * sprite.Scale);
            sprite.Init(new Vector2(locx, locy), new Point(102, 102), new Point(8, 1), -1);
            sprite.ZLayer = 0.3f;
            sprite.Load(content.Load<Texture2D>("Assets/Other/Game/LoadingIcon"));

            ContinueText = new AnimatedText("to Continue", ButtonIcon.Continue, 1.0f, Alignment.Left);
            ContinueText.AnchorPoint = ScreenAnchorLocation.BottomLeft;
            ContinueText.Offset = new Vector2(20, -10);
            ContinueText.Load(content);
            ContinueText.Scale = 0.5f;

            _banner = new FullscreenPicture();
            _banner.Load(content, "Assets/Other/Game/SDBanner", ScaleType.MaxWidth);
            _banner.Position = new Vector2(this._graphicsDevice.Viewport.Width * 0.5f, this._graphicsDevice.Viewport.Height);
            _banner.Origin = new Vector2(_banner.texture.Width * 0.5f, _banner.texture.Height);
            base.Load();
        }

        public override void Unload()
        {
            content.Unload();
        }
        #endregion

        #region Update
        public override void Update(GameTime gameTime)
        {
            if (ScreenManager.LoadingContent)
            {

            }
            else
            {
                ContinueText.Update(gameTime);
                sprite.Update(gameTime);
            }

            if (!ScreenManager.LoadingContent && InputManager.Instance.Jump())
            {
                ScreenManager.FadeOut(null);
            }

            base.Update(gameTime);
        }
        #endregion

        #region Draw
        public override void Draw(SpriteBatch sb)
        {
            sb.GraphicsDevice.Clear(Color.Black);

            sb.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);

            //picture.Draw(sb);
            _banner.Draw(sb);

            if (ScreenManager.LoadingContent)
            { }
            else
            {
                sprite.Draw(sb);
                ContinueText.Draw(sb);
            }

            base.Draw(sb);
            sb.End();
        }
        #endregion
    }
}
