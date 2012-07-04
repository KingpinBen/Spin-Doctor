using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System.ComponentModel;
using FarseerPhysics.Dynamics;

namespace GameLibrary.Objects
{
    public class NodeObject : ICloneable
    {
        #region Fields
        [ContentSerializer]
        protected Vector2 _position;
        [ContentSerializer]
        protected float _zLayer;

        #endregion

        #region Properties

#if EDITOR
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


#else
        public Vector2 Position
        {
            get
            {
                return _position;
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

        public virtual void Update(GameTime gameTime)
        {

        }

        public virtual void Draw(SpriteBatch sb)
        {

        }

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
    }
}
