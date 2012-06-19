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
        [ContentSerializerIgnore]
        public float Width
        {
            get
            {
                return _width;
            }
#if EDITOR
            set
#else
            protected set
#endif
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
#if EDITOR
            set
#else
            protected set
#endif
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
#if EDITOR
            set
#else
            protected set
#endif
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
#if EDITOR
            set
#else
            protected set
#endif
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
#if EDITOR
            set
            {
                _decalAsset = value;
            }
#endif
        }
        [ContentSerializerIgnore]
        public float ZLayer
        {
            get
            {
                return _zLayer;
            }
#if EDITOR
            set
            {
                _zLayer = value;
            }
#endif
        }
        [ContentSerializerIgnore]
        public float Rotation
        {
            get
            {
                return _rotation;
            }
#if EDITOR
            set
            {
                _rotation = value;
            }
#endif
        }
        [ContentSerializerIgnore]
        public float Scale
        {
            get
            {
                return _scale;
            }
#if EDITOR
            set
            {
                _scale = value;
            }
#endif
        }
        [ContentSerializerIgnore]
        public Color Tint
        {
            get
            {
                return _tint;
            }
#if EDITOR
            set
            {
                _tint = value;
            }
#endif
        }

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
        }

        public void Load(ContentManager Content)
        {
            this._decalTexture = Content.Load<Texture2D>(AssetLocation);
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(this._decalTexture, this.Position, null, Color.White, this.Rotation, new Vector2(_decalTexture.Width / 2, _decalTexture.Height / 2), 
                this.Scale, this.FlipEffect, this.ZLayer);
        }
    }
}
