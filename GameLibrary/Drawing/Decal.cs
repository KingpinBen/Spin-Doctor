//#define EDITOR

#region Using
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
#endregion

namespace GameLibrary.Drawing
{
    public class Decal
    {
        #region Fields
        [ContentSerializer]
        private float _width;
        [ContentSerializer]
        private float _height;
        [ContentSerializer]
        private Vector2 _position;
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
        [ContentSerializerIgnore]
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
        [ContentSerializerIgnore]
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
        [ContentSerializerIgnore]
        public string AssetLocation
        {
            get
            {
                return _decalAsset;
            }
            set
            {
                _decalAsset = value;
            }
        }
        [ContentSerializerIgnore]
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
        [ContentSerializerIgnore]
        public float Rotation
        {
            get
            {
                return _rotation;
            }
            set
            {
                _rotation = value;
            }
        }
        [ContentSerializerIgnore]
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
        [ContentSerializerIgnore]
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
            this._decalTexture = Content.Load<Texture2D>(AssetLocation);
            this.Width = _decalTexture.Width;
            this.Height = _decalTexture.Height;
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(this._decalTexture, this._position, null, this._tint, this._rotation, new Vector2(this._height / 2, this._height / 2), 
                this._scale, this._flip, this._zLayer);
        }
    }
}
