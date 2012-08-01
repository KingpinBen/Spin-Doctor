using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using GameLibrary.GameLogic.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace GameLibrary.GameLogic.Screens
{
    public class MessageOverlay : GameScreen
    {
        string textureAsset;
        Texture2D _texture;

        public MessageOverlay(string textureAsset)
            : base()
        {
            this.textureAsset = textureAsset;
        }

        public override void Activate()
        {
            ContentManager content = new ContentManager(this.ScreenManager.Game.Services, "Content");

            _texture = content.Load<Texture2D>(textureAsset);

            base.Activate();
        }

        public override void HandleInput(float delta, InputState input)
        {
            base.HandleInput(delta, input);
        }
    }
}
