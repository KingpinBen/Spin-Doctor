﻿//--------------------------------------------------------------------------
//--    
//--    Spin Doctor - Rope
//--    
//--    Current Revision 1.000
//--    
//--    Description
//--    ===============
//--    Sorts out 
//--    
//--    Revision List
//--    ===============
//--    BenG - Initial
//--    BenG - Slightly adjusted setting up the chain so parts are more
//--           evenly distributed.
//--    BenG - Path bodies can now sleep and awoken nicely.
//--    
//--    
//--    TBD
//--    ==============
//--    Fix the joint creation
//--    
//--------------------------------------------------------------------------

#region Using Statements
using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using FarseerPhysics.Dynamics;
using FarseerPhysics.Common;
using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Factories;
using FarseerPhysics.Dynamics.Joints;

using GameLibrary.Drawing;
using GameLibrary.Assists;
using GameLibrary.Screens;
using Microsoft.Xna.Framework.Content;
using GameLibrary.Managers;
using System.ComponentModel;
using FarseerPhysics.Dynamics.Contacts;
#endregion

namespace GameLibrary.Objects
{
    public class Rope : PhysicsObject
    {
        #region Fields

        [ContentSerializer]
        private Vector2 _endPosition;
        [ContentSerializer]
        private int _chainCount;

        private List<Body> _pathBodies;
        private List<Fixture> _touchedRopeFixtures = new List<Fixture>();
        private WeldJoint _ropePlayerJoint;
        private bool _inRange;
        private Texture2D endTexture;
        private RopeJoint _ropeJoint;

        #endregion

        #region Properties

#if EDITOR
        [ContentSerializerIgnore, CategoryAttribute("Object Specific")]
        public Vector2 EndPosition
        {
            get
            {
                return _endPosition;
            }
            set
            {
                _endPosition = value;
            }
        }
        [ContentSerializerIgnore, CategoryAttribute("Hidden")]
        public override float Height
        {
            get
            {
                return base.Height;
            }
            set
            {

            }
        }
        [ContentSerializerIgnore, CategoryAttribute("Hidden")]
        public override float Width
        {
            get
            {
                return base.Width;
            }
            set
            {
                
            }
        }
        [ContentSerializerIgnore, CategoryAttribute("Object Specific")]
        public int ChainCount
        {
            get
            {
                return _chainCount;
            }
            set
            {
                _chainCount = value;
            }
        }        
#endif
        #endregion

        #region Constructor
        /// <summary>
        /// A rope is fixed to a point at one end
        /// </summary>
        public Rope() : base() { }

        public override void Init(Vector2 StartVec, string texLoc)
        {
            base.Init(StartVec, texLoc);

            this._chainCount = 10;
            this._textureAsset = texLoc;
        }
        #endregion

        #region Load
        public override void Load(ContentManager content, World world)
        {
            endTexture = content.Load<Texture2D>("Assets/Images/Textures/Rope/ropeEnd");
            _texture = content.Load<Texture2D>(_textureAsset);

#if EDITOR
            if (this.Width == 0 || this.Height == 0)
            {
                this._width = endTexture.Width;
                this._height = endTexture.Height;
                this._endPosition = this._position + new Vector2(0, _texture.Height * ChainCount);
            }
#else
            SetupPhysics(world);
#endif
        }
        #endregion

        public override void Update(GameTime gameTime)
        {
#if EDITOR

#else

            if (Camera.LevelRotating)
            {
                if (_pathBodies[_pathBodies.Count - 1].Awake == false)
                    _pathBodies[_pathBodies.Count - 1].Awake = true;       
            }

            if (!GameplayScreen.World.JointList.Contains(_ropeJoint))
            {
                if (Input.Grab() && _inRange)
                {
                    //_ropeBodyRevoluteJoint = new RevoluteJoint(_pathBodies[bodyIndex - 1], Player.Instance.Body, Vector2.Zero, Vector2.Zero);
                    _ropePlayerJoint = new WeldJoint(_touchedRopeFixtures[_touchedRopeFixtures.Count - 1].Body, Player.Instance.Body, Vector2.Zero, Vector2.Zero);
                    GameplayScreen.World.AddJoint(_ropePlayerJoint);

                    _ropeJoint = new RopeJoint(_pathBodies[0], Player.Instance.Body, Vector2.Zero, Vector2.Zero);
                    _ropeJoint.MaxLength -= 1.0f;
                    GameplayScreen.World.AddJoint(_ropeJoint);
                    Player.Instance.GrabRope();
                }
            }
            else
            {
                if (Input.Jump())
                {
                    GameplayScreen.World.RemoveJoint(_ropeJoint);
                    GameplayScreen.World.RemoveJoint(_ropePlayerJoint);
                }
            }
#endif
        }

        #region Draw
#if EDITOR
        public override void Draw(SpriteBatch sb)
        {
            sb.Draw(endTexture, this._position, null, Color.Tomato, 0.0f, new Vector2(endTexture.Width / 2, endTexture.Height / 2), 1.0f, SpriteEffects.None, this._zLayer);
            sb.Draw(endTexture, this._endPosition, null, Color.White, 0.0f, new Vector2(endTexture.Width / 2, endTexture.Height / 2), 0.6f, SpriteEffects.None, this._zLayer);
        }
#else
        public override void Draw(SpriteBatch sb)
        {
            sb.Draw(endTexture, ConvertUnits.ToDisplayUnits(_pathBodies[0].Position), null,
                    Tint, _pathBodies[0].Rotation, new Vector2(endTexture.Width / 2, endTexture.Height / 2), 1f,
                    SpriteEffects.None, zLayer);

            for (int i = 1; i < _pathBodies.Count; i++)
            {
                sb.Draw(Texture, ConvertUnits.ToDisplayUnits(_pathBodies[i].Position), null,
                    Tint, _pathBodies[i].Rotation, new Vector2(Texture.Width / 2, Texture.Height / 2), 1f,
                    SpriteEffects.None, zLayer);
            }
        }
        
#endif
        #endregion

        #region Private Methods

        #region SetupPhysics
        protected override void SetupPhysics(World world)
        {
            this._pathBodies = new List<Body>();
            float width = ConvertUnits.ToSimUnits(this._texture.Width / 2);
            float height = ConvertUnits.ToSimUnits(this._texture.Height / 2);

            Path _ropePath = new Path();
            _ropePath.Add(ConvertUnits.ToSimUnits(this._position));
            _ropePath.Add(ConvertUnits.ToSimUnits(this._endPosition));

            PolygonShape rotationPointShape = new PolygonShape(PolygonTools.CreateCircle(height, 8), 25);
            PolygonShape shape = new PolygonShape(PolygonTools.CreateRectangle(width, height), 5);

            Body prevBody = new Body(world); ;
            for (int i = 0; i < _chainCount; ++i)
            {
                Body body = new Body(world);
                body.BodyType = BodyType.Dynamic;
                body.Position = ConvertUnits.ToSimUnits(Position) + new Vector2(0, height * i);

                if (i == 0)
                {
                    Fixture fixture = body.CreateFixture(rotationPointShape);
                    fixture.Friction = 0.2f;
                    fixture.CollisionCategories = Category.Cat1;
                    fixture.CollidesWith = Category.All & ~Category.Cat2;
                    body.AngularDamping = 0.4f;
                    
                    FixedRevoluteJoint fixedJoint = JointFactory.CreateFixedRevoluteJoint(world, body, Vector2.Zero, ConvertUnits.ToSimUnits(Position));
                }
                else
                {
                    Fixture fixture = body.CreateFixture(shape);
                    fixture.Friction = 0.2f;

                    fixture.CollisionCategories = Category.Cat2;
                    fixture.CollidesWith = Category.All & ~Category.Cat1;

                    RopeJoint rj = new RopeJoint(prevBody, body, new Vector2(0.0f, height), new Vector2(0.0f, -height / 2));

                    rj.CollideConnected = false;
                    world.AddJoint(rj);

                    body.OnCollision += Body_OnCollision;
                    body.OnSeparation += Body_OnSeparation;
                    body.IsSensor = true;
                }

                prevBody = body;
                _pathBodies.Add(body);
            }
        }
        #endregion

        #region Collisions
        protected override void Body_OnSeparation(Fixture fixtureA, Fixture fixtureB)
        {
            if (fixtureB.Body != Player.Instance.Body)
            {
                return;
            }

            _touchedRopeFixtures.Remove(fixtureA);

            if (_touchedRopeFixtures.Count == 0)
            {
                _inRange = false;
            }
        }

        protected override bool Body_OnCollision(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            if (fixtureB.Body != Player.Instance.Body)
            {
                return true;
            }

            if (!_touchedRopeFixtures.Contains(fixtureA))
            {
                _touchedRopeFixtures.Add(fixtureA);

                if (!_inRange)
                {
                    _inRange = true;
                }
            }

            return true;
        }
        #endregion

        #endregion
    }
}
