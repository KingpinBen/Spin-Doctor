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
using GameLibrary.Drawing;
using GameLibrary.Assists;
using GameLibrary.Managers;
using Microsoft.Xna.Framework.Content;
#endregion

namespace GameLibrary.Screens
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
            content = new ContentManager(Screen_Manager.Game.Services, "Content");

            
            //picture = new FullscreenPicture();
        }
        #endregion

        #region Load
        public override void Load()
        {
            sprite = new Sprite();
            
            sprite.Scale = 0.4f;

            int locx = Screen_Manager.GraphicsDevice.Viewport.Width - (int)(102 * sprite.Scale);
            int locy = Screen_Manager.GraphicsDevice.Viewport.Height - (int)(102 * sprite.Scale);
            sprite.Init(new Vector2(locx, locy), new Point(102, 102), new Point(8, 1), -1);
            sprite.ZLayer = 0.3f;
            sprite.Load(content.Load<Texture2D>("Assets/Other/Game/LoadingIcon"));

            ContinueText = new AnimatedText("to Continue", ImgType.Continue,1.0f, Alignment.Left);
            ContinueText.AnchorPoint = ScreenAnchorLocation.BottomLeft;
            ContinueText.Offset = new Vector2(20, -10);
            ContinueText.Load(content);
            ContinueText.Scale = 0.7f;

            _banner = new FullscreenPicture();
            _banner.Load(content, "Assets/Other/Game/SDBanner", ScaleStyle.MaxWidth);
            _banner.Position = new Vector2(Screen_Manager.GraphicsDevice.Viewport.Width / 2, Screen_Manager.GraphicsDevice.Viewport.Height);
            _banner.Origin = new Vector2(_banner.texture.Width / 2, _banner.texture.Height);
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
            if (Screen_Manager.LoadingContent) { }

            else
            {
                ContinueText.Update(gameTime);
                sprite.Update(gameTime);
            }

            if (!Screen_Manager.LoadingContent && InputManager.Instance.Jump())
                Screen_Manager.FadeOut(null);

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

            if (Screen_Manager.LoadingContent)
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
