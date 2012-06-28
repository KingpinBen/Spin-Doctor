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
        public FullscreenPicture picture { get; set; }

        private ContentManager content;

        #endregion

        #region Constructor
        public LoadingScreen()
            : base("LoadingScreen")
        {
            content = new ContentManager(Screen_Manager.Game.Services, "Content");

            sprite = new Sprite();
            picture = new FullscreenPicture();
        }
        #endregion

        #region Load
        public override void Load()
        {
            int locx = Screen_Manager.GraphicsDevice.Viewport.Width - (int)(102 * sprite.Scale);
            int locy = Screen_Manager.GraphicsDevice.Viewport.Height - (int)(102 * sprite.Scale);

            sprite.Init(new Point(102,102), new Point(8, 1), -1);
            sprite.Scale = 0.4f;
            sprite.Load(content, "Assets/Other/Game/LoadingIcon");
            sprite.Position = new Vector2(locx, locy);

            ContinueText = new AnimatedText("to Continue", ImgType.Continue,1.0f, Alignment.Left);
            ContinueText.AnchorPoint = ScreenAnchorLocation.BottomLeft;
            ContinueText.Offset = new Vector2(20, -10);
            ContinueText.Load(content);
            ContinueText.Scale = 0.7f;

            picture.scaleStyle = ScaleStyle.MaxWidth;
            picture.Load(content, "Assets/Other/Images/backgroundlandscape");

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
            if (Screen_Manager.LoadingContent)
                sprite.Update(gameTime);
            else
                ContinueText.Update(gameTime);

            if (!Screen_Manager.LoadingContent && Input.Jump())
                Screen_Manager.FadeOut(null);

            base.Update(gameTime);
        }
        #endregion

        #region Draw
        public override void Draw(SpriteBatch sb)
        {
            sb.GraphicsDevice.Clear(Color.Black);

            sb.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);

            picture.Draw(sb);
            if (Screen_Manager.LoadingContent)
                sprite.Draw(sb);
            else
                ContinueText.Draw(sb);

            base.Draw(sb);
            sb.End();
        }
        #endregion
    }
}
