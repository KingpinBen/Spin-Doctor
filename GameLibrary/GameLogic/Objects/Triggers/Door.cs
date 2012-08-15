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
//-------------------------------------------------------------------------------

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
using GameLibrary.GameLogic.Events;
using GameLibrary.GameLogic.Characters;
#endregion

namespace GameLibrary.GameLogic.Objects.Triggers
{
    public class Door : StaticObject
    {
        #region Fields

#if EDITOR

#else
        private bool _triggered = false;
        private TriggerType _triggerType = TriggerType.Automatic;
#endif
        
        [ContentSerializer(Optional = true)]
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
            base.Init(position);

            this._nextLevel = 0;
            this._textureAsset = texLoc;
            this._name = "ExitDoor";
            this._castShadows = false;
        }

        #endregion

        #region Load
        public override void Load(ContentManager content, World world)
        {
            this._texture = content.Load<Texture2D>(_textureAsset);
            this._origin = new Vector2(this._texture.Width, this._texture.Height) * 0.5f;

#if EDITOR
            if (_width == 0 || _height == 0)
            {
                this.Width = this._texture.Width;
                this.Height = this._texture.Height;
            }
#else
            this._triggerType = Triggers.TriggerType.PlayerInput;

            this.SetupTrigger(world);

            if (this.Name == null || this.Name == "")
            {
                throw new Exception("Doors must be given a name to properly change level.");
            }

            this._objectEvents.Add(new Event(this.Name, null, EventType.CHANGE_LEVEL, 0.0f, _nextLevel));

            RegisterObject();
#endif
        }
        #endregion

        public override void Update(float delta)
        {
#if EDITOR

#else
            if (Player.Instance.PlayerState != PlayerState.Dead)
            {
                if (InputManager.Instance.Interact(true) && _triggered)
                {
                    FireEvent();
                }
            }
            else
            {
                _triggered = false;
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
        public override void Draw(SpriteBatch sb, GraphicsDevice graphics)
        {
            sb.Draw(Texture, new Rectangle(
                (int)(this._position.X),
                (int)(this._position.Y),
                (int)(_width),
                (int)(_height)), null, this._tint, _rotation, _origin, SpriteEffects.None, this._zLayer);
        }
#endif
        #endregion

        #region Private Methods

#if !EDITOR

        #region Collisions
        protected override bool Body_OnCollision(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            //  We only care for fixtureB if it's part of the player.
            if (Player.Instance.CheckBodyBox(fixtureB))
            {
                //  Check if the touching list contains B.
                if (!_touchingFixtures.Contains(fixtureB))
                {
                    _touchingFixtures.Add(fixtureB);

                    #region World-Door Orientation Check

                    if (!_triggered)
                    {
                        if (Camera.Instance.UpIs == UpIs.Up &&
                            _orientation == Orientation.Up)
                        {
                            ChangeTriggered(true);
                        }
                        else if (Camera.Instance.UpIs == UpIs.Down &&
                          _orientation == Orientation.Down)
                        {
                            ChangeTriggered(true);
                        }
                        else if (Camera.Instance.UpIs == UpIs.Left &&
                          _orientation == Orientation.Right)
                        {
                            ChangeTriggered(true);
                        }
                        else if (Camera.Instance.UpIs == UpIs.Right &&
                          _orientation == Orientation.Left)
                        {
                            ChangeTriggered(true);
                        }
                    }
                    #endregion
                }
                
                //  FixtureB was the player and it's been acted on.
                return true;
            }

            //  It wasn't the player, so ignore it.
            return false;
        }

        protected override void Body_OnSeparation(Fixture fixtureA, Fixture fixtureB)
        {
            if (_touchingFixtures.Contains(fixtureB) && Player.Instance.CheckBodyBox(fixtureB))
            {
                _touchingFixtures.Remove(fixtureB);

                if (_touchingFixtures.Count == 0)
                {
                    ChangeTriggered(false);
                }
            }
        }

        void ChangeTriggered(bool state)
        {
            if (state)
            {
                if (_triggerType == TriggerType.PlayerInput)
                {
                    HUD.Instance.ShowOnScreenMessage(true, " to use.");
                }
            }
            else
            {
                HUD.Instance.ShowOnScreenMessage(false, "");
            }

            this._triggered = state;
        }
        #endregion

        protected void SetupTrigger(World world)
        {
            this.Body = BodyFactory.CreateRectangle(world, 
                ConvertUnits.ToSimUnits(30), 
                ConvertUnits.ToSimUnits(30), 
                1.0f);

            //  Change the position to it's above the lowest point of the doors
            //  texture
            this.Body.Position = ConvertUnits.ToSimUnits(_position + SpinAssist.ModifyVectorByOrientation(
                new Vector2(0,(this._height * 0.5f) - (30 * 0.5f)), 
                this._orientation));

            this.Body.OnSeparation += Body_OnSeparation;
            this.Body.OnCollision += Body_OnCollision;
            
            this.Body.IsSensor = true;
            this.Body.Enabled = _enabled;
            //this.Body.CollisionCategories = Category.Cat10;
        }

        public override void Enable()
        {
            ChangeTriggered(false);
            base.Enable();
        }

        public override void Disable()
        {
            ChangeTriggered(false);
            base.Disable();
        }

        public override void Toggle()
        {
            ChangeTriggered(false);
            base.Toggle();
        }

#endif
        #endregion
    }
}
