
#region Using
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System.ComponentModel;
#endregion

namespace GameLibrary.Drawing
{
    public class Decal : ICloneable
    {
        #region Fields
        [ContentSerializer]
        private float _width;
        [ContentSerializer]
        private float _height;
        [ContentSerializer]
        private Vector2 _position;
        [ContentSerializer]
        private Vector2 _origin;
        [ContentSerializer]
        private string _decalAsset;
        [ContentSerializer]
        private float _scale;
        [ContentSerializer]
        private float _rotation;
        [ContentSerializer]
        private float _zLayer;
        [ContentSerializer]
        private SpriteEffects _flip;
        [ContentSerializer]
        private Color _tint;
        [ContentSerializerIgnore]
        private Texture2D _decalTexture;

        #endregion

        #region Properties
#if EDITOR
        [ContentSerializerIgnore, CategoryAttribute("General")]
        public float Width
        {
            get
            {
                return _width;
            }

            set
            {
                _width = value;
            }
        }
        [ContentSerializerIgnore, CategoryAttribute("General")]
        public float Height
        {
            get
            {
                return _height;
            }
            set
            {
                _height = value;
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
        public SpriteEffects FlipEffect
        {
            get
            {
                return _flip;
            }
            set
            {
                _flip = value;
            }
        }
        [ContentSerializerIgnore, CategoryAttribute("Hidden")]
        public string AssetLocation
        {
            get
            {
                return _decalAsset;
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
                value = MathHelper.Clamp(value, 0.1f, 0.99f);
                _zLayer = value;
            }
        }
        [ContentSerializerIgnore, CategoryAttribute("General")]
        public float Rotation
        {
            get
            {
                return _rotation;
            }
            set
            {
                _rotation = MathHelper.ToRadians(value);
            }
        }
        [ContentSerializerIgnore, CategoryAttribute("General")]
        public float Scale
        {
            get
            {
                return _scale;
            }
            set
            {
                _scale = value;
            }
        }
        [ContentSerializerIgnore, CategoryAttribute("General")]
        public Color Tint
        {
            get
            {
                return _tint;
            }
            set
            {
                _tint = value;
            }
        }
        [ContentSerializerIgnore, CategoryAttribute("Hidden")]
        public Vector2 Origin
        {
            get
            {
                return _origin;
            }
        }
#endif
        #endregion

        public Decal() { }

        public void Init(Vector2 pos, string assetLoc)
        {
            this._position = pos;
            this._scale = 1.0f;
            this._rotation = 0.0f;
            this._decalAsset = assetLoc;
            this._zLayer = 0.4f;
            this._flip = SpriteEffects.None;
            this._tint = Color.White * 1.0f;
        }

        public void Load(ContentManager Content)
        {
            this._decalTexture = Content.Load<Texture2D>(_decalAsset);
            this._origin = new Vector2(this._decalTexture.Width / 2, this._decalTexture.Height / 2);
#if EDITOR
            if (this.Width == 0 || this.Height == 0)
            {
                this.Width = _decalTexture.Width;
                this.Height = _decalTexture.Height;
            }
#else

#endif
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(this._decalTexture, this._position, null, this._tint, this._rotation, this._origin, 
                this._scale, this._flip, this._zLayer);
        }

        #region Private Methods

        object ICloneable.Clone()
        {
            return this.Clone();
        }

        public Decal Clone()
        {
            return (Decal)this.MemberwiseClone();
        }

        #endregion
    }
}
