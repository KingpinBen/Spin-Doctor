//--------------------------------------------------------------------------
//--    
//--    Spin Doctor - PhysicsObjects
//--    
//--    Description
//--    ===============
//--    Creates character bodies and wheels
//--    
//--    Revision List
//--    ===============
//--    BenG - Initial
//--    BenG - Allows color to be passed in for shading.
//--    BenG - Can pass in a shapetype while TexToPoly collision isnt in.
//--    BenG - Changed how all PhysicsObjects are loaded. Makes loading more
//--           similar between different types.
//--    BenG - Added an orientation. Not sure how I'm going to use it yet though.
//--    BenG - Rearraged and renamed all variables. Added a define for easier editor changes.
//--    BenG - Fixed all collisions on this and all deriving objects.
//--    BenG - Added name for use with triggers and targetting.
//--    BenG - Removed the events from the physicsobject class. They can be added, but it's 
//--           only really used by a few classes.
//--    
//--    TBD
//--    ==============
//--    
//--
//--    
//--    
//--------------------------------------------------------------------------

#region Using Statements
using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using FarseerPhysics.Collision.Shapes;
using GameLibrary.Screens;
using GameLibrary.Assists;
using Microsoft.Xna.Framework.Content;
using GameLibrary.Managers;
using GameLibrary.Drawing;
using FarseerPhysics.Dynamics.Contacts;
using System.ComponentModel;
#endregion

namespace GameLibrary.Objects
{
    public class PhysicsObject : ICloneable
    {
        #region Fields

        //[ContentSerializer]
        //protected string _name;
        [ContentSerializer]
        protected Vector2 _position;
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
        protected float _zLayer;
        [ContentSerializer]
        protected bool _useBodyRotation;
        [ContentSerializer]
        protected float _rotation;
        [ContentSerializer]
        protected Orientation _orientation;
        //[ContentSerializer]
        //protected float _scale;

        // Can't serialize Texture2D files. Texture is init'd in Load() anyway.
        [ContentSerializerIgnore]
        protected Texture2D _texture;
        protected Vector2 _origin;

        #endregion

        #region Properties

#if EDITOR
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
        public virtual float Height
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
        public virtual float Width
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
        public virtual float zLayer
        {
            get
            {
                return _zLayer;
            }
            set
            {
                if (value == 0)
                    _zLayer = 0.01f;
                else if (value == 1)
                    _zLayer = 0.99f;
                else
                    _zLayer = value;
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
        public virtual Vector2 Position
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
                    _height = value;
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

        #region Construct and Initialize
        public PhysicsObject()
        {

        }

        public virtual void Init(Vector2 position, string tex)
        {
            this._position = position;
            this._textureAsset = tex;
            this._mass = 20;
            this.Tint = Color.White;
            this._zLayer = 0.01f;
        }
        #endregion

        #region Load
        /// <summary>
        /// Load object game content
        /// </summary>
        /// <param name="tex">Texture location</param>
        public virtual void Load(ContentManager content, World world)
        {
            if (this._textureAsset != "")
                _texture = content.Load<Texture2D>(this._textureAsset);

            this._origin = new Vector2(_texture.Width / 2, _texture.Height / 2);

#if EDITOR
            //  To fix it regenerating the values when loading them in the editor
            if (this.Width == 0.0f || this.Height == 0.0f)
            {
                this.Width = this._texture.Width;
                this.Height = this._texture.Height;
            }
#else
            SetUpPhysics(world);
#endif
            //GetRotationFromOrientation();
        }
        #endregion

        public virtual void Update(GameTime gameTime)
        {
#if EDITOR

#else
            if (!Camera.LevelRotating)
            {
                return;
            }

            if (!this.Body.Awake)
            {
                this.Body.Awake = true;
            }
#endif
        }

        #region Draw
        /// <summary>
        /// Draw the object obviously.
        /// </summary>
        /// <param name="spriteBatch"></param>
#if EDITOR
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            //  Doesn't strech image. Tiles instead
            spriteBatch.Draw(_texture, Position,
                new Rectangle(0, 0, (int)this.Width, (int)this.Height),
                Tint, TextureRotation, new Vector2(this.Width / 2, this.Height / 2), 1f, SpriteEffects.None, zLayer);
        }
#else
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            //  Doesn't strech image. Tiles instead
            spriteBatch.Draw(Texture, ConvertUnits.ToDisplayUnits(this.Body.Position),
                new Rectangle(0, 0, (int)this._width, (int)this._height),
                Tint, this.TextureRotation, new Vector2(this._width / 2, this._height / 2), 1f, SpriteEffects.None, zLayer);
        }
#endif
        #endregion

        #region SetUpPhysics
        /// <summary>
        /// Set up the objects physics
        /// </summary>
        /// <param name="world"></param>
        /// <param name="position"></param>
        /// <param name="shape"></param>
        /// <param name="mass"></param>
        protected virtual void SetUpPhysics(World world)
        {
#if EDITOR

#else
            //  This function will have to be changed if we have things that aren't going to be square/rectangles.
            //  Fortunately, Farseer will allow us to use the Texture to find the outline. Haven't tried it with 
            //  full coloured images, only outlines. Will have to research! More demos!
            this.Body = BodyFactory.CreateRectangle(world, ConvertUnits.ToSimUnits(this.Width), ConvertUnits.ToSimUnits(this.Height), ConvertUnits.ToSimUnits(this.Mass), ConvertUnits.ToSimUnits(this.Position));

            // This object will move under collision/gravity
            this.Body.BodyType = BodyType.Dynamic;
            //this.Body.CollisionCategories = Category.Cat10;

            // Elastic (>1) <->Non-elastic collisions (0).
            this.Body.Restitution = 0f;

            // Default friction the body has when colliding with other objects.
            this.Body.Friction = 5.0f;
#endif
        }
        #endregion

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

        object ICloneable.Clone()
        {
            return this.Clone();
        }

        public PhysicsObject Clone()
        {
            return (PhysicsObject)this.MemberwiseClone();
        }
    }
}
