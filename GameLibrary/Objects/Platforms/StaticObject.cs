﻿//--------------------------------------------------------------------------
//--    
//--    Spin Doctor - StaticObject
//--    
//--    
//--    
//--    Description
//--    ===============
//--    Allows static objects that still have a collision model.
//--    
//--    Revision List
//--    ===============
//--    BenG - Initial 
//--    BenG - Added a second constructor for circles.
//--    BenG - Changed restitution to 0 to stop player bouncing off the walls/floor.
//--           
//--    
//--    
//--    TBD
//--    ==============
//--    The rest of it, durrh
//--    
//--    
//--------------------------------------------------------------------------

#region Using Statements
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using FarseerPhysics.Dynamics.Contacts;
using Microsoft.Xna.Framework.Graphics;
using System.ComponentModel;
using GameLibrary.Assists;
using FarseerPhysics.Factories;
using System.Diagnostics;
using GameLibrary.Screens;
using Poly2Tri.Triangulation.Polygon;
using FarseerPhysics.Collision.Shapes;
#endregion

namespace GameLibrary.Objects
{
    public class StaticObject : NodeObject
    {
        #region Fields
        [ContentSerializer]
        protected float _width;
        [ContentSerializer]
        protected float _height;
        [ContentSerializer]
        protected float _mass;
        [ContentSerializer]
        protected string _textureAsset;

        [ContentSerializer]
        protected Color _tint;
        [ContentSerializer]
        protected bool _useBodyRotation;
        [ContentSerializer]
        protected float _rotation;
        [ContentSerializer]
        protected Orientation _orientation;

        // Can't serialize Texture2D files. Texture is init'd in Load() anyway.
        protected Texture2D _texture;
        protected Vector2 _origin;

        #endregion

        #region Properties

#if EDITOR
        [ContentSerializerIgnore, CategoryAttribute("General")]
        public override float Height
        {
            get
            {
                if (_orientation == Orientation.Up || _orientation == Orientation.Down)
                    return _height;
                return _width;
            }
            set
            {
                if (_orientation == Orientation.Up || _orientation == Orientation.Down)
                    _height = value;
                else
                    _width = value;
            }
        }
        [ContentSerializerIgnore, CategoryAttribute("General")]
        public override float Width
        {
            get
            {
                if (_orientation == Orientation.Up || _orientation == Orientation.Down)
                    return _width;
                return _height;
            }
            set
            {
                if (_orientation == Orientation.Up || _orientation == Orientation.Down)
                    _width = value;
                else
                    _height = value;
            }
        }
        [ContentSerializerIgnore, CategoryAttribute("General")]
        public virtual float Mass
        {
            get
            {
                return _mass;
            }
            set
            {
                _mass = value;
            }
        }
        [ContentSerializerIgnore, CategoryAttribute("Hidden")]
        public virtual Vector2 Origin
        {
            get
            {
                return new Vector2(this.Width / 2, this.Height / 2);
            }
            protected set
            {
                _origin = value;
            }
        }
        [ContentSerializerIgnore, CategoryAttribute("General")]
        public virtual Color Tint
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
        [ContentSerializerIgnore, CategoryAttribute("General")]
        public virtual float TextureRotation
        {
            get
            {
                if (_useBodyRotation)
                    return 0;
                return _rotation;
            }
            set
            {
                _rotation = MathHelper.ToRadians(value);
            }
        }
        [ContentSerializerIgnore, CategoryAttribute("General")]
        public virtual bool UseBodyRotation
        {
            get
            {
                return _useBodyRotation;
            }
            set
            {
                _useBodyRotation = value;

                if (value)
                    GetRotationFromOrientation();
            }
        }
        [ContentSerializerIgnore, CategoryAttribute("General")]
        public virtual Orientation Orientation
        {
            get
            {
                return _orientation;
            }
            set
            {
                _orientation = value;
                GetRotationFromOrientation();
            }
        }
        [ContentSerializerIgnore, CategoryAttribute("Hidden")]
        public Texture2D Texture
        {
            get
            {
                return _texture;
            }
        }
#else
        [ContentSerializerIgnore]
        public Body Body { get; set; }
        [ContentSerializerIgnore]
        public new virtual Vector2 Position
        {
            get
            {
                return _position;
            }
            protected set
            {
                _position = value;
            }
        }
        [ContentSerializerIgnore]
        public virtual float Height
        {
            get
            {
                if (_orientation == Orientation.Up || _orientation == Orientation.Down)
                    return _height;
                return _width;
            }
            protected set
            {
                if (_orientation == Orientation.Up || _orientation == Orientation.Down)
                    _height = value;
                else
                    _width = value;
            }
        }
        [ContentSerializerIgnore]
        public virtual float Width
        {
            get
            {
                if (_orientation == Orientation.Up || _orientation == Orientation.Down)
                    return _width;
                return _height;
            }
            protected set
            {
                if (_orientation == Orientation.Up || _orientation == Orientation.Down)
                    _width = value;
                else
                    _height = value;
            }
        }
        [ContentSerializerIgnore]
        public virtual float Mass
        {
            get
            {
                return _mass;
            }
            protected set
            {
                _mass = value;
            }
        }
        [ContentSerializerIgnore]
        public virtual string AssetLocation
        {
            get
            {
                return _textureAsset;
            }
            protected set
            {
                _textureAsset = value;
            }

        }
        [ContentSerializerIgnore]
        public virtual Vector2 Origin
        {
            get
            {
                return new Vector2(this.Width / 2, this.Height / 2);
            }
            protected set
            {
                _origin = value;
            }
        }
        [ContentSerializerIgnore]
        public virtual Color Tint
        {
            get
            {
                return _tint;
            }
            protected set
            {
                _tint = value;
            }
        }
        [ContentSerializerIgnore]
        public virtual float zLayer
        {
            get
            {
                return _zLayer;
            }
            protected set
            {
                _zLayer = value;
            }
        }
        [ContentSerializerIgnore]
        public virtual float TextureRotation
        {
            get
            {

                if (_useBodyRotation)
                    return Body.Rotation;
                return _rotation;
            }
            protected set
            {
                _rotation = value;
            }
        }
        [ContentSerializerIgnore]
        public Texture2D Texture
        {
            get
            {
                return _texture;
            }
            protected set
            {
                _texture = value;
            }
        }
        [ContentSerializerIgnore]
        public virtual bool UseBodyRotation
        {
            get
            {
                return _useBodyRotation;
            }
            protected set
            {
                _useBodyRotation = value;
            }
        }
        [ContentSerializerIgnore]
        public virtual Orientation Orientation
        {
            get
            {
                return _orientation;
            }
            protected set
            {
                _orientation = value;
                GetRotationFromOrientation();
            }
        }
        //[ContentSerializerIgnore]
        //        public float Scale
        //        {
        //            get
        //            {
        //                return _scale;
        //            }
        //#if EDITOR
        //            set
        //#else
        //            protected set
        //#endif
        //            {
        //                _scale = value;
        //            }
        //        }
        //        public string Name
        //        {
        //            get { return _name; }
        //#if EDITOR
        //            set
        //#else
        //            protected set
        //#endif
        //            {
        //                _name = value;
        //            }
        //        }
        
#endif
        #endregion
        
        #region Constructors
        public StaticObject()
            : base()
        {

        }

        public virtual void Init(Vector2 position, string tex)
        {
            this._position = position;
            this._textureAsset = tex;
            this._tint = Color.White;
            this._zLayer = 0.5f;
            this._mass = 1000.0f;
        }
        #endregion

        public override void Load(ContentManager content, World world)
        {
            if (_textureAsset == "")
            {
                throw new Exception("A TEXTURE location in the level doesn't exist.");
            }

            _texture = content.Load<Texture2D>(_textureAsset);
            _origin = new Vector2(_texture.Width / 2, _texture.Height / 2);

#if EDITOR
            if (this.Width == 0.0f || this.Height == 0.0f)
            {
                this.Width = this._texture.Width;
                this.Height = this._texture.Height;
            }
#else
            SetupPhysics(world);
#endif
        }

        #region Draw
#if EDITOR
        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, this._position,
                new Rectangle(0, 0, (int)this._width, (int)this._height),
                Tint, TextureRotation, new Vector2(this._width / 2, this._height / 2), 1.0f, SpriteEffects.None, _zLayer);
        }
#else
        public override void Draw(SpriteBatch sb)
        {
            sb.Draw(_texture, _position,
                new Rectangle(0, 0, (int)this._width, (int)this._height),
                Tint, TextureRotation, new Vector2(this._width / 2, this._height / 2), 1.0f, SpriteEffects.None, _zLayer);
        }
#endif
        #endregion

        #region Private Methods

        protected virtual void SetupPhysics(World world)
        {
#if EDITOR

#else
            //  This function will have to be changed if we have things that aren't going to be square/rectangles.
            //  Fortunately, Farseer will allow us to use the Texture to find the outline. Haven't tried it with 
            //  full coloured images, only outlines. Will have to research! More demos!
            this.Body = BodyFactory.CreateRectangle(world, ConvertUnits.ToSimUnits(_width), ConvertUnits.ToSimUnits(_height), ConvertUnits.ToSimUnits(this.Mass), ConvertUnits.ToSimUnits(this.Position));
            //  Give it a 0 for no flags.
            this.Body.FixtureList[0].UserData = (int)0;
            this.Body.Rotation = _rotation;
            // Elastic (>1) <->Non-elastic collisions (0).
            this.Body.Restitution = 0f;
            this.Body.CollisionCategories = Category.Cat20;
            this.Body.CollidesWith = Category.All & ~Category.Cat20;
            // Default friction the body has when colliding with other objects.
            this.Body.Friction = 5.0f;
#endif
        }

        #region Collisions
        protected virtual bool Body_OnCollision(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            return true;
        }

        protected virtual void Body_OnSeparation(Fixture fixtureA, Fixture fixtureB)
        {

        }
        #endregion

        protected virtual void GetRotationFromOrientation()
        {
            if (_orientation == Orientation.Up)
                _rotation = 0.0f;
            else if (_orientation == Orientation.Down)
                _rotation = MathHelper.Pi;
            else if (_orientation == Orientation.Left)
                _rotation = MathHelper.PiOver2;
            else if (_orientation == Orientation.Right)
                _rotation = -MathHelper.PiOver2;
        }

        #endregion
    }
}
