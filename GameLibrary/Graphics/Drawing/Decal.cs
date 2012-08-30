
#region Using
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System.ComponentModel;
using FarseerPhysics.Common;
#endregion

namespace GameLibrary.Graphics.Drawing
{
    public class Decal : ICloneable
    {
        #region Fields
        [ContentSerializer]
        private float _width = 0;
        [ContentSerializer]
        private float _height = 0;
        [ContentSerializer]
        private Vector2 _position = Vector2.Zero;
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
            this._zLayer = 0.8f;
            this._flip = SpriteEffects.None;
            this._tint = Color.White;
        }

        public void Load(ContentManager Content)
        {
            this._decalTexture = Content.Load<Texture2D>(_decalAsset);
            this._origin = new Vector2(this._decalTexture.Width, this._decalTexture.Height) * 0.5f;
#if EDITOR
            if (this.Width == 0 || this.Height == 0)
            {
                this.Width = this._decalTexture.Width;
                this.Height = this._decalTexture.Height;
            }
#endif
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(_decalTexture, _position, new Rectangle(0,0,(int)_width, (int)_height), 
                    _tint, _rotation, new Vector2(_width * 0.5f, _height * 0.5f), 
                    _scale, _flip, _zLayer);
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
