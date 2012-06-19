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

#define EDITOR

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
#endregion

namespace GameLibrary.Objects
{
    public enum Orientation
    {
        Up, Down, Left, Right
    }

    public enum Direction
    {
        Horizontal,
        Vertical
    }

    public class PhysicsObject
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

        //  Body is generated on load so no need to hold it all
        [ContentSerializerIgnore]
        public Body Body { get; set; }

        #endregion

        #region Properties

        [ContentSerializerIgnore]
        public virtual Vector2 Position
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
        public virtual float Height
        {
            get
            {
                if (_orientation == Orientation.Up || _orientation == Orientation.Down)
                    return _height;
                return _width;
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
        public virtual float Width
        {
            get
            {
                if (_orientation == Orientation.Up || _orientation == Orientation.Down)
                    return _width;
                return _height;
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
        public virtual float Mass
        {
            get
            {
                return _mass;
            }
#if EDITOR
            set
#else
            protected set
#endif
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
#if EDITOR
            set
#else
            protected set
#endif
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

#if EDITOR
            set
#else
            protected set
#endif
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
#if EDITOR
            set
#else
            protected set
#endif
            {
                _zLayer = value;
            }
        }
        [ContentSerializerIgnore]
        public virtual float TextureRotation
        {
#if EDITOR
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
#else
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
#endif
        }
        [ContentSerializerIgnore]
        public Texture2D Texture
        {
            get
            {
                return _texture;
            }
#if EDITOR
            set
#else
            protected set
#endif
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
#if EDITOR
            set
#else
            protected set
#endif
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
#if EDITOR
            set
#else
            protected set
#endif
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

        #endregion

        #region Construct and Initialize
        public PhysicsObject()
        {

        }

        public virtual void Init(Vector2 position, string tex)
        {
            this.Position = position;
            this._textureAsset = tex;
            this.Mass = 20;
            this.Tint = Color.White;
        }

        public virtual void Init(Vector2 position, float width, float height, string tex)
        {
            this._position = position;
            this._mass = 20.0f;
            this._tint = Color.White;
            this._width = width;
            this._height = height;
            this._textureAsset = tex;
            //this._scale = 1.0f;
        }

        /// <summary>
        /// Use to make circles
        /// </summary>
        public virtual void Init(Vector2 position, float radius, string tex)
        {
            this._position = position;
            this._width = radius;
            this._tint = Color.White;
            this._mass = 20.0f;
            this._textureAsset = tex;
        }
        #endregion

        #region LoadContent
        /// <summary>
        /// Load object game content
        /// </summary>
        /// <param name="tex">Texture location</param>
        public virtual void Load(ContentManager content, World world)
        {
            if (this._textureAsset != "")
                Texture = content.Load<Texture2D>(this._textureAsset);

            this._origin = new Vector2(Texture.Width / 2, Texture.Height / 2);

            SetUpPhysics(world);
            //GetRotationFromOrientation();
        }
        #endregion

        #region Update
        public virtual void Update(GameTime gameTime)
        {
            if (!Camera.LevelRotating)
            {
                return;
            }

            if (!this.Body.Awake)
            {
                this.Body.Awake = true;
            }
        }
        #endregion

        #region Draw
        /// <summary>
        /// Draw the object obviously.
        /// </summary>
        /// <param name="spriteBatch"></param>
#if EDITOR
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            //  Doesn't strech image. Tiles instead
            spriteBatch.Draw(Texture, Position,
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
        }
        #endregion

        #region Collisions
        protected virtual bool Body_OnCollision(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            if (fixtureA.Body.BodyType == BodyType.Static && fixtureB.Body.BodyType == BodyType.Static)
            {
                return false;
            }

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
                _rotation = MathHelper.Pi + MathHelper.PiOver2;
        }
    }
}
