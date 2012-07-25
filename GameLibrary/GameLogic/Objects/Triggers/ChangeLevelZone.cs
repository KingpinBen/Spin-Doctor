using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using System.ComponentModel;
using GameLibrary.Helpers;

namespace GameLibrary.GameLogic.Objects.Triggers
{
    public class ChangeLevelZone : Door
    {
        #region Fields
#if EDITOR
        Texture2D _devTexture;
#else

#endif
        #endregion

        #region Properties
#if EDITOR
        [ContentSerializerIgnore, CategoryAttribute("Hidden")]
        public override bool ShowHelp
        {
            get
            {
                return base.ShowHelp;
            }
            set
            {
                
            }
        }
#else

#endif

        #endregion

        public ChangeLevelZone() : base() { }

        public override void Init(Vector2 position)
        {
            this._position = position;
            this._width = 50;
            this._height = 50;
            this._nextLevel = 0;
        }

        public override void Load(ContentManager content, World world)
        {
#if EDITOR
            _devTexture = content.Load<Texture2D>("Assets/Other/Dev/Trigger");
#else
            this.Body = BodyFactory.CreateRectangle(world, ConvertUnits.ToSimUnits(this.Width), ConvertUnits.ToSimUnits(this.Height), 1.0f);
            this.Body.Position = ConvertUnits.ToSimUnits(Position);
            this.Body.BodyType = BodyType.Static;
            this.Body.IsSensor = true;
            this.Body.CollidesWith = Category.Cat10;
            this.Body.OnCollision += Body_OnCollision;
            this.Body.OnSeparation += Body_OnSeparation;

            this.Triggered = false;
            
#endif
            this._showHelp = false;
        }

        public override void Update(float delta)
        {
#if EDITOR
#else
            if (Triggered)
            {
                //ScreenManager.LoadLevel(_nextLevel);
            }
#endif
        }

#if EDITOR
        public override void Draw(SpriteBatch sb)
        {
            sb.Draw(_devTexture, this._position, new Rectangle(0, 0, (int)Width, (int)Height), Color.White * 0.4f, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, this._zLayer);
        }
#else


        public override void Draw(SpriteBatch sb, GraphicsDevice graphics)
        {
            
        }
#endif
    }
}
