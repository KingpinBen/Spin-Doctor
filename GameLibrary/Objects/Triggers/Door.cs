//--------------------------------------------------------------------------------
//--    
//--    Spin Doctor - Door
//--
//--    
//--    Description
//--    ===============
//--    Allows the use of changelevels.
//--
//--    
//--    Revision List
//--    ===============
//--    BenG - Initial
//--    BenG - (Just added the comments). Changed the Init to include int to 
//--           the next level.
//--    BenG - Updated the message popup to work with the HUD
//--    
//--    
//--    TBD
//--    ==============
//--    Make it work properly with other objects
//--    Make it work with orientation
//--    
//-------------------------------------------------------------------------------

//#define EDITOR
//#define Development

#region Using Statements
using System;
using System.Collections.Generic;
using GameLibrary.Objects;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using FarseerPhysics.Factories;
using GameLibrary.Screens;
using GameLibrary.Assists;
using GameLibrary.Objects.Triggers;
using GameLibrary.Screens.Messages;
using Microsoft.Xna.Framework.Content;
using GameLibrary.Managers;
using GameLibrary.Drawing;
using FarseerPhysics.Dynamics.Contacts;
#endregion

namespace GameLibrary.Objects.Triggers
{
    public class Door : Trigger
    {
        #region Fields
        [ContentSerializer]
        protected int nextLevel;
        #endregion

        #region Properties
        [ContentSerializerIgnore]
        public int NextLevel
        {
            get
            {
                return nextLevel;
            }
#if EDITOR
            set
#else
            protected set
#endif
            {
                nextLevel = value;
            }
        }
        #endregion

        #region Constructor
        public Door()
            : base()
        {

        }

        public void Init(Vector2 position, int levelLinkID, string texLoc)
        {
            this.nextLevel = levelLinkID;
            this._textureAsset = texLoc;
            this.ShowHelp = true;
            base.Init(position, 30, 30);
        }
        #endregion

        #region Load
        public override void Load(ContentManager content, World world)
        {
            Texture = content.Load<Texture2D>(_textureAsset);

            base.Load(content, world);

            _rotation = SpinAssist.RotationByOrientation(_orientation);
        }
        #endregion

        public override void Update(GameTime gameTime)
        {
            if (Input.Interact() && Triggered)
            {
                Screen_Manager.LoadLevel(nextLevel);
            }

            base.Update(gameTime);
        }

        #region Draw
#if EDITOR
        public override void Draw(SpriteBatch sb)
        {
            sb.Draw(Texture, this.Position, null, _tint, this._rotation, new Vector2(Texture.Width / 2, Texture.Height), 1.0f, SpriteEffects.None, zLayer);
        }
#else
        public override void Draw(SpriteBatch sb)
        {
#if Development
            sb.DrawString(Fonts.DebugFont, "Touching: " + TouchingFixtures.Count, ConvertUnits.ToDisplayUnits(this.Body.Position) + new Vector2(0, -70), Color.Blue, 0f, Vector2.Zero, 2f, SpriteEffects.None, 0.0f);
#endif
            sb.Draw(Texture, ConvertUnits.ToDisplayUnits(this.Body.Position) + SpinAssist.ModifyVectorByOrientation(new Vector2(0, Height / 2), this._orientation), null, Color.White, _rotation, new Vector2(Texture.Width / 2, Texture.Height), 1f, SpriteEffects.None, zLayer);
        }
#endif
        #endregion

        #region Collisions
        protected override bool Body_OnCollision(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            if (!TouchingFixtures.Contains(fixtureB))
            {
                TouchingFixtures.Add(fixtureB);
            }

            //if (TouchingFixtures.Count > 0 && this.Triggered) return true;

            if (Camera.UpIs == upIs.Up &&
                _orientation == Orientation.Up)
            {
                this.Triggered = true;
            }
            else if (Camera.UpIs == upIs.Down &&
              _orientation == Orientation.Down)
            {
                this.Triggered = true;
            }
            else if (Camera.UpIs == upIs.Left &&
              _orientation == Orientation.Right)
            {
                this.Triggered = true;
            }
            else if (Camera.UpIs == upIs.Right &&
              _orientation == Orientation.Left)
            {
                this.Triggered = true;
            }

            if (this.ShowHelp && !HUD.ShowPopup) HUD.ShowOnScreenMessage(true, " to use.");

            return true;
        }

        protected override void Body_OnSeparation(Fixture fixtureA, Fixture fixtureB)
        {
            this.TouchingFixtures.Remove(fixtureB);

            if (TouchingFixtures.Count == 0)
            {
                this.Triggered = false;
                HUD.ShowOnScreenMessage(false);
            }
            else 
                return;
        }
        #endregion
    }
}
