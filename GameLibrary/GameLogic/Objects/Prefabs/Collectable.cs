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
using GameLibrary.GameLogic.Objects.Triggers;
#endregion

namespace GameLibrary.GameLogic.Objects
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

        #endregion

        public override void Load(ContentManager content, World world)
        {
            base.Load(content, world);

#if !EDITOR
            this._objectEvents.Add(new Event(this._name, this._name, EventType.TRIGGER_REMOVE, 0.0f, 0));
#endif
        }

        public override void Update(float delta)
        {
#if !EDITOR
            if (_triggered)
            {
                if (Player.Instance.PlayerState == PlayerState.Dead)
                {
                    ChangeTriggered(false);
                }
                else
                {
                    this.FireEvent();
                }
            }
#endif
        }

        #region Private Methods

#if !EDITOR 

        protected override void SetupPhysics(World world)
        {
            this.Body = BodyFactory.CreateRectangle(world, ConvertUnits.ToSimUnits(30), ConvertUnits.ToSimUnits(_texture.Height), _mass);
            this.Body.Position = ConvertUnits.ToSimUnits(this._position);
            this.Body.Rotation = _rotation;
            //  Give it it's own category but make it only collide with 10 (Player) and nothing else.
            this.Body.CollisionCategories = Category.Cat3;
            this.Body.CollidesWith = Category.Cat10;

            this.Body.OnCollision += Body_OnCollision;
            this.Body.OnSeparation += Body_OnSeparation;
            this.Body.Enabled = _enabled;
            this.Body.IsSensor = true;
        }

        #region Collisions

        protected override bool Body_OnCollision(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            if (Player.Instance.CheckBodyBox(fixtureB))
            {
                if (!_touchingFixtures.Contains(fixtureB))
                {
                    _touchingFixtures.Add(fixtureB);

                    ChangeTriggered(true);
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
                ChangeTriggered(false);
            }
        }

        protected void ChangeTriggered(bool state)
        {
            if (state)
            {
                if (_triggerType == TriggerType.PlayerInput)
                {
                    HUD.Instance.ShowOnScreenMessage(true, _message, ButtonIcon.Action3);
                }
            }
            else
            {
                HUD.Instance.ShowOnScreenMessage(false, "", ButtonIcon.Action3);
            }

            this._triggered = state;
        }
        #endregion

#endif
        #endregion
    }
}
