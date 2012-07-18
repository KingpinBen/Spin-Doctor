﻿//--------------------------------------------------------------------------------
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

//#define Development

#region Using Statements
using System;
using System.Collections.Generic;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework.Content;
using FarseerPhysics.Dynamics.Contacts;
using System.ComponentModel;
using GameLibrary.Helpers;
using GameLibrary.Graphics.Camera;
using GameLibrary.GameLogic.Controls;
using GameLibrary.Graphics.UI;
#endregion

namespace GameLibrary.GameLogic.Objects.Triggers
{
    public class Door : Trigger
    {
        #region Fields
        [ContentSerializer]
        protected int _nextLevel;
        #endregion

        #region Properties
#if EDITOR
        [ContentSerializerIgnore, CategoryAttribute("Object Specific")]
        public int NextLevel
        {
            get
            {
                return _nextLevel;
            }
            set
            {
                _nextLevel = value;
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
        [ContentSerializerIgnore, CategoryAttribute("Hidden")]
        public override float TriggerWidth
        {
            get
            {
                return base.TriggerWidth;
            }
        }
        [ContentSerializerIgnore, CategoryAttribute("Hidden")]
        public override float TriggerHeight
        {
            get
            {
                return base.TriggerHeight;
            }
        }
        
#else

#endif
        #endregion

        #region Constructor
        public Door()
            : base()
        {

        }

        public override void Init(Vector2 position, string texLoc)
        {
            this._nextLevel = 0;
            this._textureAsset = texLoc;

            base.Init(position, 30, 30);
        }
        #endregion

        #region Load
        public override void Load(ContentManager content, World world)
        {
            this._texture = content.Load<Texture2D>(_textureAsset);
            this._origin = new Vector2(this._texture.Width / 2, this._texture.Height / 2);

#if EDITOR
            if (_width == 0 || _height == 0)
            {
                this.Width = this._texture.Width;
                this.Height = this._texture.Height;
            }
#else
            this.TriggerWidth = 30;
            this.TriggerHeight = 30;
            this.SetupTrigger(world);
#endif
        }
        #endregion

        public override void Update(GameTime gameTime)
        {
#if EDITOR

#else
            if (InputManager.Instance.Interact() && Triggered)
            {
                ScreenManager.LoadLevel(_nextLevel);
            }
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
                this._origin, 
                SpriteEffects.None, this._zLayer);
        }
#else
        public override void Draw(SpriteBatch sb)
        {
#if Development
            sb.DrawString(Fonts.DebugFont, "Touching: " + TouchingFixtures.Count, ConvertUnits.ToDisplayUnits(this.Body.Position) + new Vector2(0, -70), Color.Blue, 0f, Vector2.Zero, 2f, SpriteEffects.None, 0.0f);
#endif
            sb.Draw(Texture, new Rectangle(
                (int)(this._position.X),
                (int)(this._position.Y),
                (int)(_width),
                (int)(_height)), null, this._tint, _rotation, _origin, SpriteEffects.None, this._zLayer);
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
            if (fixtureB != Player.Instance.WheelBody.FixtureList[0])
            {
                return false;
            }

            if (!TouchingFixtures.Contains(fixtureB))
            {
                TouchingFixtures.Add(fixtureB);
            }

            if (Camera.Instance.UpIs == UpIs.Up &&
                _orientation == Orientation.Up)
            {
                this.Triggered = true;
            }
            else if (Camera.Instance.UpIs == UpIs.Down &&
              _orientation == Orientation.Down)
            {
                this.Triggered = true;
            }
            else if (Camera.Instance.UpIs == UpIs.Left &&
              _orientation == Orientation.Right)
            {
                this.Triggered = true;
            }
            else if (Camera.Instance.UpIs == UpIs.Right &&
              _orientation == Orientation.Left)
            {
                this.Triggered = true;
            }

            if (this.ShowHelp && !HUD.ShowPopup) 
            {
                HUD.ShowOnScreenMessage(true, " to use.");
            }

            return true;
#endif
        }

        protected override void Body_OnSeparation(Fixture fixtureA, Fixture fixtureB)
        {
#if EDITOR

#else
            if (TouchingFixtures.Contains(fixtureB))
            {
                TouchingFixtures.Remove(fixtureB);
            }

            if (fixtureB == Player.Instance.WheelBody.FixtureList[0])
            {
                this.Triggered = false;
                HUD.ShowOnScreenMessage(false);
            } 
#endif
        }
        #endregion

        protected override void SetupTrigger(World world)
        {
#if EDITOR

#else
            this.Body = BodyFactory.CreateRectangle(world, 
                ConvertUnits.ToSimUnits(TriggerWidth), 
                ConvertUnits.ToSimUnits(TriggerHeight), 
                1.0f);

            //  Change the position to it's above the lowest point of the doors
            //  texture
            this.Body.Position = ConvertUnits.ToSimUnits(_position + SpinAssist.ModifyVectorByOrientation(
                new Vector2(0,(this._height * 0.5f) - (TriggerHeight * 0.5f)), 
                this._orientation));

            this.Body.OnSeparation += Body_OnSeparation;
            this.Body.OnCollision += Body_OnCollision;
            
            this.Body.IsSensor = true;
            //this.Body.CollisionCategories = Category.Cat10;
#endif
        }

        #endregion
    }
}