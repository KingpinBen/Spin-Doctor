//--------------------------------------------------------------------------
//--    
//--    Spin Doctor - Collectable
//--    
//--    Description
//--    ===============
//--    
//--    
//--    Revision List
//--    ===============
//--    BenG - Initial
//--    BenG - Fixed a messaging issue.
//--    
//--    
//--    
//--    TBD
//--    ==============
//--
//--    
//--    
//--------------------------------------------------------------------------

#region Using
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using FarseerPhysics.Dynamics;
using GameLibrary.Managers;
using Microsoft.Xna.Framework.Graphics;
using GameLibrary.Objects.Triggers;
using GameLibrary.Assists;
using GameLibrary.Screens.Messages;
using Microsoft.Xna.Framework.Content;
using FarseerPhysics.Dynamics.Contacts;
using GameLibrary.Drawing;
#endregion

namespace GameLibrary.Objects
{
    public class Collectable : Trigger
    {
        #region Fields

        protected bool _beenCollected = false;
        //private Sprite LookAtMeSprite;
        #endregion

        #region Properties

        [ContentSerializerIgnore]
        public bool BeenCollected
        {
            get
            {
                return _beenCollected;
            }
            protected set
            {
                _beenCollected = value;
            }
        }

        #endregion

        #region Constructor
        public Collectable()
            : base()
        {

        }

        public override void Init(Vector2 Position, string texLoc)
        {
            base.Init(Position, 25.0f, 25.0f, texLoc);

            this.ShowHelp = true;
            this._message = " to pick up.";
        }
        #endregion

        #region Load
        public override void Load(ContentManager content, World world)
        {
            base.Load(content, world);

            this._texture = content.Load<Texture2D>(_textureAsset);

            //LookAtMeSprite = new Sprite();
            //LookAtMeSprite.Init(new Point(64, 64), new Point(8, 4), -1);
            //LookAtMeSprite.Load(content, "Assets/Sprites/Effects/Explosions");
            //LookAtMeSprite.Position = Position;
            //LookAtMeSprite.Alpha = 0.7f;
            //LookAtMeSprite.Scale = 1.5f;

            this._origin = new Vector2(this._texture.Width / 2, this._texture.Height / 2);
        }
        #endregion

        #region Update
        public override void Update(GameTime gameTime)
        {
            //LookAtMeSprite.Update(gameTime);

            if (Input.Interact() && Triggered)
            {
                CreatePopUp();
            }
        }
        #endregion

        #region Draw
        public override void Draw(SpriteBatch sb)
        {
            if (!BeenCollected)
            {
                //LookAtMeSprite.Draw(sb);
                sb.Draw(this._texture, this._position, null, this._tint, 0f, this._origin, 1f, SpriteEffects.None, this.zLayer);
            }

            base.Draw(sb);
        }
        #endregion

        #region CreatePopup
        private void CreatePopUp()
        {
            MessageOverlay newOverlay = new MessageOverlay(MessageType.FullScreen, 1);
            newOverlay.Load();

            Screen_Manager.AddScreen(newOverlay);

            HUD.ShowOnScreenMessage(false);
            this.Triggered = false;
            this.BeenCollected = true;
        }
        #endregion

        #region Collisions
        protected override bool Body_OnCollision(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            if (BeenCollected)
            {
                return true;
            }

            base.Body_OnCollision(fixtureA, fixtureB, contact);

            return true;
        }
        #endregion
    }
}
