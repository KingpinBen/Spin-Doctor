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
        protected string _name;
        [ContentSerializer]
        protected Vector2 _position;
        [ContentSerializer]
        protected float _zLayer;
        [ContentSerializer(Optional = true)]
        protected List<Event> _objectEvents = new List<Event>();

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
            }
        }
        [ContentSerializerIgnore, CategoryAttribute("General")]
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
#endif

        #endregion

        public NodeObject()
        {

        }

        public virtual void Init(Vector2 position)
        {
            this._position = position;
            this._zLayer = 0.6f;
        }

        public virtual void Load(ContentManager content, World world)
        { }

        public virtual void Update(float delta)
        {

        }

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
#if !EDITOR


        protected void RegisterEvent()
        {
            EventManager.Instance.RegisterObject(this);
        }

        protected void FireEvent() 
        {
            EventManager.Instance.FireEvent(this._name);
        }
#endif

        public virtual void Toggle() { }
        public virtual void Enable() { }
        public virtual void Disable() { }
        public virtual void Start() { }
        public virtual void Stop() { }

        #endregion
    }
}
