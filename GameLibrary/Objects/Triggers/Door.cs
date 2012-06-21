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
        [ContentSerializerIgnore]
        public override Orientation Orientation
        {
            get
            {
                return _orientation;
            }
            set
            {
                _orientation = value;
                _rotation = SpinAssist.RotationByOrientation(_orientation);
            }
        }
        public override float TextureRotation
        {
            get
            {
                return base.TextureRotation;
            }
            set { }
        }
        #endregion

        #region Constructor
        public Door()
            : base()
        {

        }

        public override void Init(Vector2 position, string texLoc)
        {
            this.nextLevel = 0;
            this._textureAsset = texLoc;
            this.ShowHelp = true;
            this._tint = Color.White;
            this._position = position;
            this._message = " to use.";
        }
        #endregion

        #region Load
        public override void Load(ContentManager content, World world)
        {
            this._texture = content.Load<Texture2D>(_textureAsset);

#if EDITOR
            this.Width = this._texture.Width;
            this.Height = this._texture.Height;
#else
            this.TriggerWidth = 30;
            this.TriggerHeight = 30;
#endif

            base.Load(content, world);
        }
        #endregion

        public override void Update(GameTime gameTime)
        {
#if EDITOR

#else
            if (Input.Interact() && Triggered)
            {
                Screen_Manager.LoadLevel(nextLevel);
            }

            base.Update(gameTime);
#endif
        }

        #region Draw
#if EDITOR
        public override void Draw(SpriteBatch sb)
        {
            sb.Draw(this._texture, new Rectangle(
                (int)(this._position.X),
                (int)(this._position.Y),
                (int)(_width),
                (int)(_height)),
                null, this._tint, this._rotation, 
                new Vector2(this._texture.Width/ 2, this._texture.Height/2), 
                SpriteEffects.None, this.zLayer);
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

        #region Private Methods

        #region Collisions
        protected override bool Body_OnCollision(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
#if EDITOR
            return true;
#else
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
#endif
        }

        protected override void Body_OnSeparation(Fixture fixtureA, Fixture fixtureB)
        {
#if EDITOR

#else
            this.TouchingFixtures.Remove(fixtureB);

            if (TouchingFixtures.Count == 0)
            {
                this.Triggered = false;
                HUD.ShowOnScreenMessage(false);
            }
            else 
                return;
#endif
        }
        #endregion

        protected override void SetUpTrigger(World world)
        {
            this.Body = BodyFactory.CreateRectangle(world, 
                ConvertUnits.ToSimUnits(TriggerWidth), 
                ConvertUnits.ToSimUnits(TriggerHeight), 
                1.0f);

            //  Change the position to it's above the lowest point of the doors
            //  texture
            this.Body.Position = _position + SpinAssist.ModifyVectorByOrientation(
                new Vector2(0,(this._height / 2) - (TriggerHeight / 2)), 
                this._orientation);

            this.Body.IsSensor = true;
            this.Body.OnCollision += Body_OnCollision;
            this.Body.OnSeparation += Body_OnSeparation;
            //this.Body.CollisionCategories = Category.Cat10;
        }

        #endregion
    }
}
