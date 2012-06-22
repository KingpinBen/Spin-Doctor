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
using FarseerPhysics.Factories;
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

#if EDITOR

#else
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
#endif



        #endregion

        #region Constructor
        public Collectable()
            : base()
        {

        }

        public override void Init(Vector2 Position, string texLoc)
        {
            base.Init(Position, texLoc);

            this.ShowHelp = true;
            this._message = " to pick up.";
        }
        #endregion

        public override void Load(ContentManager content, World world)
        {
            base.Load(content, world);

#if EDITOR
            this.TriggerWidth = 25.0f;
            this.TriggerHeight = 25.0f;
#else
            //LookAtMeSprite = new Sprite();
            //LookAtMeSprite.Init(new Point(64, 64), new Point(8, 4), -1);
            //LookAtMeSprite.Load(content, "Assets/Sprites/Effects/Explosions");
            //LookAtMeSprite.Position = Position;
            //LookAtMeSprite.Alpha = 0.7f;
            //LookAtMeSprite.Scale = 1.5f;
#endif
        }

        public override void Update(GameTime gameTime)
        {
#if EDITOR

#else
            //LookAtMeSprite.Update(gameTime);
#endif
        }

        #region Draw

#if EDITOR
        public override void Draw(SpriteBatch sb)
        {
            //base.Draw(sb);
        }
#else
        public override void Draw(SpriteBatch sb)
        {
            if (!BeenCollected)
            {
                //LookAtMeSprite.Draw(sb);
                sb.Draw(this._texture, this._position, null, this._tint, 0f, this._origin, 1f, SpriteEffects.None, this.zLayer);
            }

            base.Draw(sb);
        }
#endif
        #endregion

        #region Private Methods

        

        #region Collisions
        protected override bool Body_OnCollision(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
#if EDITOR 
            return true;
#else
            if (BeenCollected)
            {
                return true;
            }

            base.Body_OnCollision(fixtureA, fixtureB, contact);

            return true;
#endif
        }
        #endregion

        #endregion
    }
}
