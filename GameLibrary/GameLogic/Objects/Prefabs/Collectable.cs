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
#endregion

namespace GameLibrary.GameLogic.Objects.Triggers
{
    public class Collectable : StaticObject
    {
        #region Fields

        
        [ContentSerializer]
        protected InteractType _interactType = InteractType.PickUp;
        [ContentSerializer]
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
            this.SetupTrigger(world);
            this.RegisterObject();
#endif

            this._origin = new Vector2(this._width, this._height) * 0.5f;
        }

        public override void Update(float delta)
        {
#if EDITOR

#else
            
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

            this.Body.IsSensor = true;
            this.Body.OnCollision += Body_OnCollision;
            this.Body.OnSeparation += Body_OnSeparation;
        }

        #region Collisions
        protected override bool Body_OnCollision(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            if (_beenCollected)
            {
                return true;
            }

            base.Body_OnCollision(fixtureA, fixtureB, contact);

            return true;
        }

        protected override void Body_OnSeparation(Fixture fixtureA, Fixture fixtureB)
        {
            base.Body_OnSeparation(fixtureA, fixtureB);
        }
        #endregion
#endif
        #endregion
    }
}
