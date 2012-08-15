//--------------------------------------------------------------------------
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
using FarseerPhysics.Factories;
using System.Diagnostics;
using Poly2Tri.Triangulation.Polygon;
using FarseerPhysics.Collision.Shapes;
using GameLibrary.Helpers;
#endregion

namespace GameLibrary.GameLogic.Objects
{
    public class StaticObject : NodeObject
    {
        #region Fields
        [ContentSerializer(Optional = true)]
        protected float _width;
        [ContentSerializer(Optional = true)]
        protected float _height;
        [ContentSerializer(Optional = true)]
        protected float _mass = 0.0f;
        [ContentSerializer(Optional = true)]
        protected string _textureAsset = String.Empty;

        [ContentSerializer(Optional = true)]
        protected Color _tint = Color.White;
        [ContentSerializer(Optional = true)]
        protected bool _useBodyRotation;
        [ContentSerializer(Optional = true)]
        protected float _rotation = 0;
        [ContentSerializer(Optional = true)]
        protected Orientation _orientation = Orientation.Up;

        protected Texture2D _texture;
        protected Vector2 _origin;

#if EDITOR
        
#else
        protected List<Fixture> _touchingFixtures = new List<Fixture>();
#endif

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
                return new Vector2(this.Width, this.Height) * 0.5f;
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
        public virtual float Rotation
        {
            get
            {
                return MathHelper.ToDegrees(_rotation);
            }
            set
            {
                _rotation = MathHelper.ToRadians(value);
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
                if (value == Orientation.Up)
                    _rotation = 0.0f;
                else if (value == Orientation.Down)
                    _rotation = MathHelper.Pi;
                else if (value == Orientation.Left)
                    _rotation = MathHelper.PiOver2;
                else if (value == Orientation.Right)
                    _rotation = -MathHelper.PiOver2;
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
                return _origin;
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
        public virtual float Rotation
        {
            get
            {
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
        public virtual Orientation Orientation
        {
            get
            {
                return _orientation;
            }
            protected set
            {
                _orientation = value;
            }
        }       
#endif
        #endregion

        public StaticObject() : base() 
        {
            //  Generally we want all the game objects to cast
            //  shadows by default.
            this._castShadows = true;
        }

        public virtual void Init(Vector2 position, string tex)
        {
            base.Init(position);

            //  Set the texture
            this._textureAsset = tex;

            //  Behind the player.
            this._zLayer = 0.5f;

            //  Static objects are given high mass for collision calculations.
            this._mass = 1000.0f;
        }

        public override void Load(ContentManager content, World world)
        {
            this._texture = content.Load<Texture2D>(_textureAsset);
            this._origin = new Vector2(_width, _height) * 0.5f;
#if EDITOR
            if (this.Width == 0.0f || this.Height == 0.0f)
            {
                this.Width = this._texture.Width;
                this.Height = this._texture.Height;
            }
#else

            this.SetupPhysics(world);
            base.Load(content, world);
#endif

            
        }

        #region Draw
#if EDITOR
        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, this._position,
                new Rectangle(0, 0, (int)this._width, (int)this._height),
                this._tint, this._rotation, new Vector2(this._width, this._height) * 0.5f, 1.0f, SpriteEffects.None, _zLayer);
        }
#else
        public override void Draw(SpriteBatch sb, GraphicsDevice graphics)
        {
            sb.Draw(_texture, _position,
                new Rectangle(0, 0, (int)_width, (int)_height), _tint, Body.Rotation, 
                _origin, 1.0f, SpriteEffects.None, _zLayer);
        }
#endif
        #endregion

        #region Private Methods

        protected virtual void SetupPhysics(World world)
        {
#if !EDITOR
            //  This function will have to be changed if we have things that aren't going to be square/rectangles.
            //  Fortunately, Farseer will allow us to use the Texture to find the outline. Haven't tried it with 
            //  full coloured images, only outlines. Will have to research! More demos!
            this.Body = BodyFactory.CreateRectangle(world, ConvertUnits.ToSimUnits(_width), ConvertUnits.ToSimUnits(_height), ConvertUnits.ToSimUnits(this._mass));
            this.Body.Position = ConvertUnits.ToSimUnits(_position);
            //  Give it a 0 for no flags.
            this.Body.FixtureList[0].UserData = (int)0;
            this.Body.Rotation = _rotation;
            // Elastic (>1) <->Non-elastic collisions (0).
            this.Body.Restitution = 0f;
            this.Body.CollisionCategories = Category.Cat20;
            this.Body.CollidesWith = Category.All & ~Category.Cat20;
            // Default friction the body has when colliding with other objects.
            this.Body.Friction = 5.0f;
            this.Body.Enabled = _enabled;
#endif
        }

#if !EDITOR

        #region Collisions


        protected virtual bool Body_OnCollision(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            return true;
        }

        protected virtual void Body_OnSeparation(Fixture fixtureA, Fixture fixtureB)
        {

        }
        

        #endregion

        #region Events

        public override void Enable()
        {
            this.Body.Enabled = true;
            base.Enable();
        }

        public override void Disable()
        {
            this.Body.Enabled = false;
            base.Disable();
        }

        public override void Toggle()
        {
            this.Body.Enabled = !this.Body.Enabled;
            base.Toggle();
        }

        public override Body GetBody()
        {
            return this.Body;
        }

        #endregion

#endif

        #endregion
    }
}
