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
using Microsoft.Xna.Framework.Audio;
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
        [ContentSerializer(Optional = true)]
        protected MaterialType _materialType = MaterialType.Metal;

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
        public MaterialType MaterialType
        {
            get
            {
                return _materialType;
            }
            set
            {
                _materialType = value;
            }
        }
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
        public override float Rotation
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
        public virtual MaterialType MaterialType
        {
            get
            {
                return _materialType;
            }
        }
#endif
        #endregion

        #region Constructor and Load



        /// <summary>
        /// Constructor. 
        /// 
        /// Most game objects will want to have CastShadows as true
        /// so it gets set here as NodeObjects may not want to.
        /// </summary>
        public StaticObject() : base() 
        {
            //  Generally we want all the game objects to cast
            //  shadows by default.
            this._castShadows = true;
        }



        /// <summary>
        /// Initialize the object. 
        /// 
        ///  * Only used by the editor.
        /// </summary>
        /// <param name="position">The position to set the object at.</param>
        /// <param name="tex">The texture to apply.</param>
        public virtual void Init(Vector2 position, string tex)
        {
            //  Set the position
            base.Init(position);

            //  Set the texture
            this._textureAsset = tex;

            //  Behind the player.
            this._zLayer = 0.5f;

            //  Static objects are given high mass for collision calculations.
            this._mass = 1000.0f;
        }



        /// <summary>
        /// Load the texture of the object.
        /// 
        /// Game: 
        ///  * Generate the body for the object,
        ///  * Register the object for events.
        ///  
        /// Editor:
        ///  * Check that the object has positive width/height.
        /// </summary>
        /// <param name="content"></param>
        /// <param name="world"></param>
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


        #endregion

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
            sb.Draw(_texture, _position, new Rectangle(0, 0, (int)_width, (int)_height), 
                _tint, _rotation, _origin, 1.0f, SpriteEffects.None, _zLayer);
        }
#endif
        #endregion

        #region Private Methods

        protected virtual void SetupPhysics(World world)
        {
#if !EDITOR
            //  Create the intial body
            this.Body = BodyFactory.CreateRectangle(world, ConvertUnits.ToSimUnits(_width), 
                ConvertUnits.ToSimUnits(_height), ConvertUnits.ToSimUnits(this._mass));

            //  Set the position
            this.Body.Position = ConvertUnits.ToSimUnits(_position);

            this.Body.Rotation = _rotation;

            // Elastic (>1) <->Non-elastic collisions (0).
            this.Body.Restitution = 0f;

            //  Set collision categories so that it doesn't collide 
            //  with other Cat20 objects.
            this.Body.CollisionCategories = Category.Cat20;
            this.Body.CollidesWith = Category.All & ~Category.Cat20;

            // Default friction the body has when colliding with other objects.
            this.Body.Friction = 5.0f;
            this.Body.Enabled = _enabled;

            this.Body.UserData = _materialType;
#endif
        }

#if !EDITOR

        #region Collisions


        protected virtual bool Body_OnCollision(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            if (!_touchingFixtures.Contains(fixtureB))
            {
                _touchingFixtures.Add(fixtureB);
            }

            return true;
        }

        protected virtual void Body_OnSeparation(Fixture fixtureA, Fixture fixtureB)
        {
            _touchingFixtures.Remove(fixtureB);
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
