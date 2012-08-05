using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System.ComponentModel;
using FarseerPhysics.Dynamics;
using GameLibrary.GameLogic.Screens;
using GameLibrary.GameLogic.Events;

namespace GameLibrary.GameLogic.Objects
{
    public class NodeObject : ICloneable, IGameObject
    {
        #region Fields

        [ContentSerializer(Optional = true)]
        protected string _name = String.Empty;
        [ContentSerializer(Optional = true)]
        protected bool _enabled = true;
        [ContentSerializer(Optional = true)]
        protected Vector2 _position = Vector2.Zero;
        [ContentSerializer(Optional = true)]
        protected float _zLayer = 0.6f;
        [ContentSerializer(Optional = true)]
        protected List<Event> _objectEvents = new List<Event>();
        [ContentSerializer(Optional = true)]
        protected bool _castShadows;
        
        #endregion

        #region Properties

#if EDITOR
        [ContentSerializerIgnore, CategoryAttribute("Events")]
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
                for(int i = 0; i < _objectEvents.Count; i++)
                {
                    _objectEvents[i].ObjectName = this._name;
                }
            }
        }
        [ContentSerializerIgnore, CategoryAttribute("General")]
        public virtual bool CastShadow
        {
            get
            {
                return _castShadows;
            }
            set
            {
                _castShadows = value;
            }
        }
        [ContentSerializerIgnore, CategoryAttribute("General")]
        public virtual bool Enabled
        {
            get
            {
                return _enabled;
            }
            set
            {
                _enabled = value;
            }
        }

        [ContentSerializerIgnore, CategoryAttribute("General")]
        public virtual Vector2 Position
        {
            get
            {
                return _position;
            }
            set
            {
                _position = value;
            }
        }
        [ContentSerializerIgnore, CategoryAttribute("General")]
        public float ZLayer
        {
            get
            {
                return _zLayer;
            }
            set
            {
                _zLayer = MathHelper.Clamp(value, 0.1f, 0.95f);
            }
        }
        [ContentSerializerIgnore, CategoryAttribute("General")]
        //  Only used for objects that don't have a width
        public virtual float Width
        {
            get
            {
                return 25.0f;
            }
            set { }
        }
        [ContentSerializerIgnore, CategoryAttribute("General")]
        //  Only used for objects that don't have a height
        public virtual float Height
        {
            get
            {
                return 25.0f;
            }
            set { }
        }
        [ContentSerializerIgnore, CategoryAttribute("Events")]
        public List<Event> EventList
        {
            get
            {
                return _objectEvents;
            }
            set
            {
                _objectEvents = value;
                for(int i = 0; i < _objectEvents.Count; i++)
                {
                    _objectEvents[i].ObjectName = this._name;
                }
            }
        }

#else
        [ContentSerializerIgnore]
        public Vector2 Position
        {
            get
            {
                return _position;
            }
            set
            {
                _position = value;
            }
        }
        [ContentSerializerIgnore]
        public string Name
        {
            get
            {
                return _name;
            }
        }
        [ContentSerializerIgnore]
        public List<Event> ObjectEvents
        {
            get
            {
                return _objectEvents;
            }
        }
        [ContentSerializerIgnore]
        public virtual float ZLayer
        {
            get
            {
                return this._zLayer;
            }
        }
        [ContentSerializerIgnore]
        public bool CastShadows
        {
            get
            {
                return _castShadows;
            }
            set
            {
                _castShadows = value;
            }
        }
#endif

        #endregion

        public NodeObject()
        {

        }

        public virtual void Init(Vector2 position) 
        {
            this._position.X = position.X;
            this._position.Y = position.Y;
        }

        public virtual void Load(ContentManager content, World world) 
        {
#if EDITOR
#else
            this.RegisterObject();
#endif
        }

        public virtual void Update(float delta) { }

        #region Draw
#if EDITOR
        public virtual void Draw(SpriteBatch sb)
        {
        }
#else
        public virtual void Draw(SpriteBatch sb, GraphicsDevice graphics)
        {

        }
#endif
        #endregion

        #region Clone
        object ICloneable.Clone()
        {
            return this.Clone();
        }

        public NodeObject Clone()
        {
            return (NodeObject)this.MemberwiseClone();
        }
        #endregion

        #region Event Stuff

#if EDITOR
        
#else
        /// <summary>
        /// Register this objects and all of its events with the 
        /// EventManager. If the object has no events but has a name,
        /// it'll also be added for use. Call once from Load.
        /// </summary>
        protected void RegisterObject()
        {
            EventManager.Instance.RegisterObject(this);
        }

        /// <summary>
        /// Fire all of the objects events. Timed events will have delays.
        /// </summary>
        protected void FireEvent() 
        {
            EventManager.Instance.FireEvent(this._name);
        }

        public virtual void Toggle() 
        {
            this._enabled = !this._enabled;
        }

        public virtual void Enable() 
        {
            this._enabled = true;
        }

        public virtual void Disable() 
        {
            this._enabled = false;
        }

        public virtual void Start() { }
        public virtual void Stop() { }
        public virtual void Change(object sent) { }
        public virtual Body GetBody()
        {
            return null;
        }

#endif

        

        #endregion
    }
}
