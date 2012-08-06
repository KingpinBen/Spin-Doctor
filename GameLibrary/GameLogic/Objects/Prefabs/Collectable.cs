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
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using FarseerPhysics.Dynamics.Contacts;
using FarseerPhysics.Factories;
using GameLibrary.Graphics.Drawing;
using GameLibrary.Helpers;
using System.ComponentModel;
using GameLibrary.GameLogic.Characters;
using GameLibrary.Graphics.UI;
using GameLibrary.GameLogic.Controls;
using GameLibrary.GameLogic.Events;
#endregion

namespace GameLibrary.GameLogic.Objects.Triggers
{
    public class Collectable : StaticObject
    {
        #region Fields


        [ContentSerializer(Optional = true)]
        protected InteractType _interactType = InteractType.PickUp;
        [ContentSerializer(Optional = true)]
        protected TriggerType _triggerType = TriggerType.Automatic;
        
#if EDITOR

#else
        protected string _message = String.Empty;
        protected bool _beenCollected = false;
        protected bool _triggered = false;
        protected List<Fixture> _touchingFixtures = new List<Fixture>();
#endif
        #endregion

        #region Properties
        [ContentSerializerIgnore, CategoryAttribute("Object Specific")]
        public virtual InteractType InteractType
        {
            get
            {
                return _interactType;
            }
            set
            {
                _interactType = value;
            }
        }
        [ContentSerializerIgnore, CategoryAttribute("Object Specific")]
        public virtual TriggerType TriggerType
        {
            get
            {
                return _triggerType;
            }
            set
            {
                _triggerType = value;
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
            base.Init(Position, texLoc);
        }
        #endregion

        public override void Load(ContentManager content, World world)
        {
            this._texture = content.Load<Texture2D>(_textureAsset);

#if EDITOR
            if (Width == 0 || Height == 0)
            {
                this.Width = this._texture.Width;
                this.Height = this._texture.Height;
            }
#else
            //  All collectables should be able to be removed at some point,
            //  so set up an event to remove it on collision.
            this._objectEvents.Add(new Event(this._name, this._name, EventType.TRIGGER_REMOVE, 0.0f, null));

            this.SetupTrigger(world);
            this.RegisterObject();
#endif

            this._origin = new Vector2(this._width, this._height) * 0.5f;
        }

        public override void Update(float delta)
        {
#if !EDITOR
            if (_triggered)
            {
                if (Player.Instance.PlayerState == PlayerState.Dead)
                {
                    this._triggered = false;
                }
                else
                {
                    this.FireEvent();
                }
            }
#endif
        }

        #region Draw

#if EDITOR
        public override void Draw(SpriteBatch sb)
        {
            base.Draw(sb);
        }
#else
        public override void Draw(SpriteBatch sb, GraphicsDevice graphics)
        {
            if (!_beenCollected)
            {
                sb.Draw(this._texture, this._position, null, this._tint, 0f, this._origin, 1f, SpriteEffects.None, this.zLayer);
            }
        }
#endif
        #endregion

        #region Private Methods

#if !EDITOR 

        protected void SetupTrigger(World world)
        {
            float height = ConvertUnits.ToSimUnits(25);

            this.Body = BodyFactory.CreateRectangle(world, ConvertUnits.ToSimUnits(25), height, _mass);
            this.Body.Position = ConvertUnits.ToSimUnits(this._position);
            this.Body.Position -= (Vector2.UnitY * (height * 0.5f));

            //  Give it it's own category but make it only collide with 10 (Player) and nothing else.
            this.Body.CollisionCategories = Category.Cat3;
            this.Body.CollidesWith = Category.Cat10 & ~Category.All;

            //  Set up the sensor
            this.Body.IsSensor = true;
            this.Body.OnCollision += Body_OnCollision;
            this.Body.OnSeparation += Body_OnSeparation;
        }

        #region Collisions

        protected override bool Body_OnCollision(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            if (Player.Instance.CheckBodyBox(fixtureB))
            {
                if (!_touchingFixtures.Contains(fixtureB))
                {
                    _touchingFixtures.Add(fixtureB);

                    this.PlayerOnTouch();
                }

                //  It was the player, acknowledge the collision
                return true;
            }

            //  It's not the player, so ignore it.
            return false;
        }

        protected override void Body_OnSeparation(Fixture fixtureA, Fixture fixtureB)
        {
            if (Player.Instance.CheckBodyBox(fixtureB))
            {
                if (_touchingFixtures.Contains(fixtureB))
                {
                    _touchingFixtures.Remove(fixtureB);
                    
                }

                //  We'll want to make sure to turn it all off should the player
                //  be detected leaving, but for some reason, not be in the list.
                this.PlayerOnLeave();
            }
        }

        protected virtual void PlayerOnTouch()
        {
            _triggered = true;

            if (!HUD.Instance.ShowPopup)
            {
                HUD.Instance.ShowOnScreenMessage(true, " to pick up.");
            }
        }

        protected virtual void PlayerOnLeave()
        {
            _triggered = false;
            HUD.Instance.ShowOnScreenMessage(false);
        }
        #endregion

#endif
        #endregion
    }
}
