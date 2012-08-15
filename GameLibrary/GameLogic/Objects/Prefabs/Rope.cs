//--------------------------------------------------------------------------
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

//#define Development

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
using Microsoft.Xna.Framework.Content;
using System.ComponentModel;
using FarseerPhysics.Dynamics.Contacts;
using GameLibrary.Graphics;
using GameLibrary.Graphics.Camera;
using GameLibrary.GameLogic.Controls;
using GameLibrary.Helpers;
using GameLibrary.GameLogic.Screens;
using GameLibrary.GameLogic.Characters;
#endregion

namespace GameLibrary.GameLogic.Objects
{
    public class Rope : PhysicsObject
    {
        #region Fields

        private Texture2D _endTexture;
        [ContentSerializer(Optional = true)]
        private Vector2 _endPosition = Vector2.Zero;
        [ContentSerializer(Optional = true)]
        private int _chainCount;
        [ContentSerializer(Optional = true)]
        private string _endTextureAsset;

#if EDITOR
#else
        private List<Body> _pathBodies;
        private List<Fixture> _touchedRopeFixtures = new List<Fixture>();
        private RevoluteJoint _ropePlayerJoint;
        private RopeJoint _ropeJoint;
        private World _world;

        private bool _inRange;
        private int _grabbedIndex;
#endif

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

        public Rope() : base() { }

        public void Init(Vector2 StartVec, string texLoc, string texLoc2)
        {
            base.Init(StartVec, texLoc);

            this._chainCount = 10;
            this._endTextureAsset = texLoc;
            this._textureAsset = texLoc2;
        }

        public override void Load(ContentManager content, World world)
        {
            _endTexture = content.Load<Texture2D>(_endTextureAsset);
            _texture = content.Load<Texture2D>(_textureAsset);

#if EDITOR
            if (this.Width == 0 || this.Height == 0)
            {
                this._width = _endTexture.Width;
                this._height = _endTexture.Height;
                
            }

            if (_endPosition == Vector2.Zero)
                this._endPosition = this._position + new Vector2(0, _texture.Height * ChainCount);
#else
            _world = world;
            SetupPhysics(world);
            this.RegisterObject();
#endif
        }

        public override void Update(float delta)
        {
#if EDITOR

#else
            if (Camera.Instance.IsLevelRotating)
            {
                if (_pathBodies[_pathBodies.Count - 1].Awake == false)
                {
                    _pathBodies[_pathBodies.Count - 1].Awake = true;
                }  
            }

            if (!_world.JointList.Contains(_ropeJoint))
            {
                if (InputManager.Instance.Grab(true) && _inRange)
                {
                    int index = 0;
                    float smallestDistance = (float)(Math.Pow(Player.Instance.Body.Position.X - _touchedRopeFixtures[0].Body.Position.X, 2.0) + Math.Pow(Player.Instance.Body.Position.Y - _touchedRopeFixtures[0].Body.Position.Y, 2.0));
                    
                    for(int i = 1; i < _touchedRopeFixtures.Count; i++)
                    {
                        float distance = (float)(Math.Pow(Player.Instance.Body.Position.X - _touchedRopeFixtures[i].Body.Position.X, 2.0) + Math.Pow(Player.Instance.Body.Position.Y - _touchedRopeFixtures[i].Body.Position.Y, 2.0));

                        if (distance < smallestDistance)
                        {
                            smallestDistance = distance;
                            _grabbedIndex = i;
                        }
                    }

                    _ropeJoint = new RopeJoint(_pathBodies[0], Player.Instance.Body, Vector2.Zero, Vector2.Zero);
                    _world.AddJoint(_ropeJoint);

                    _ropePlayerJoint = new RevoluteJoint(_touchedRopeFixtures[index].Body, Player.Instance.Body, Vector2.Zero, Vector2.Zero);
                    _world.AddJoint(_ropePlayerJoint);

                    Player.Instance.GrabRope();
                }
            }
            else
            {
                float rotation = 0.0f;
                if (_grabbedIndex < _pathBodies.Count - 1 && _grabbedIndex > 1)
                {
                    rotation = (_pathBodies[_grabbedIndex].Rotation + _pathBodies[_grabbedIndex - 1].Rotation + _pathBodies[_grabbedIndex + 1].Rotation) * 0.3333f;
                }
                else
                {
                    if (_grabbedIndex == _pathBodies.Count - 1)
                    {
                        rotation = (_pathBodies[_grabbedIndex].Rotation + _pathBodies[_grabbedIndex - 1].Rotation + _pathBodies[_grabbedIndex - 2].Rotation) * 0.3333f;
                    }
                    else
                    {
                        rotation = (_pathBodies[_grabbedIndex].Rotation + _pathBodies[_grabbedIndex + 1].Rotation + _pathBodies[_grabbedIndex + 2].Rotation) * 0.3333f;
                    }
                    
                }

                Player.Instance.GrabRotation = rotation;

                if (InputManager.Instance.Jump(true) || InputManager.Instance.Grab(true) || Player.Instance.PlayerState == PlayerState.Dead)
                {
                    _world.RemoveJoint(_ropeJoint);
                    _world.RemoveJoint(_ropePlayerJoint);

                    if (Player.Instance.PlayerState == PlayerState.Swinging)
                    {
                        Player.Instance.ForceFall();
                    }
                }
            }
#endif
        }

        #region Draw
#if EDITOR
        public override void Draw(SpriteBatch sb)
        {
            sb.Draw(_endTexture, this._position, null, Color.Tomato, 0.0f, new Vector2(_endTexture.Width, _endTexture.Height) * 0.5f, 1.0f, SpriteEffects.None, this._zLayer);
            sb.Draw(_endTexture, this._endPosition, null, Color.White, 0.0f, new Vector2(_endTexture.Width, _endTexture.Height) * 0.5f, 0.6f, SpriteEffects.None, this._zLayer);
        }
#else
        public override void Draw(SpriteBatch sb, GraphicsDevice graphics)
        {
            sb.Draw(_endTexture, ConvertUnits.ToDisplayUnits(_pathBodies[0].Position), null,
                    Tint, _pathBodies[0].Rotation, new Vector2(_endTexture.Width, _endTexture.Height) * 0.5f, 1f,
                    SpriteEffects.None, zLayer);

            for (int i = 1; i < _pathBodies.Count; i++)
            {
                sb.Draw(Texture, ConvertUnits.ToDisplayUnits(_pathBodies[i].Position), null,
                    Tint, _pathBodies[i].Rotation, new Vector2(Texture.Width, Texture.Height) * 0.5f, 1f,
                    SpriteEffects.None, zLayer);
            }
        }
#endif
        #endregion

        #region Private Methods

        protected override void SetupPhysics(World world)
        {
#if EDITOR
#else
            this._pathBodies = new List<Body>();
            float width = ConvertUnits.ToSimUnits(this._texture.Width * 0.5f);
            float height = ConvertUnits.ToSimUnits(this._texture.Height * 0.5f);
            Vector2 startPos = ConvertUnits.ToSimUnits(this._position);
            Vector2 endPos = ConvertUnits.ToSimUnits(this._endPosition);

            Path _ropePath = new Path();
            _ropePath.Add(startPos);
            _ropePath.Add(endPos);

            PolygonShape rotationPointShape = new PolygonShape(PolygonTools.CreateCircle(height, 8), 25);
            PolygonShape shape = new PolygonShape(PolygonTools.CreateRectangle(width, height), ConvertUnits.ToSimUnits(1.0f));
            PolygonShape sensorShape = new PolygonShape(PolygonTools.CreateCircle(height * 1.5f, 6), 1.0f);

            Body prevBody = new Body(world); ;
            for (int i = 0; i < _chainCount; ++i)
            {
                Body body = new Body(world);
                body.BodyType = BodyType.Dynamic;
                body.Position = startPos + new Vector2(0, height * i);

                if (i == 0)
                {
                    Fixture fixture = body.CreateFixture(rotationPointShape);
                    fixture.Friction = 0.2f;
                    body.AngularDamping = 0.4f;
                    
                    FixedRevoluteJoint fixedJoint = JointFactory.CreateFixedRevoluteJoint(world, body, Vector2.Zero, startPos);
                }
                else
                {
                    Fixture fixture = body.CreateFixture(shape);
                    fixture.Friction = 0.2f;
                    Fixture sensorFix = body.CreateFixture(sensorShape);
                    sensorFix.IsSensor = true;

                    fixture.CollidesWith = Category.All & ~Category.Cat10 & ~Category.Cat12;

                    RopeJoint rj = new RopeJoint(prevBody, body, new Vector2(0.0f, height), new Vector2(0.0f, -height * 0.5f));

                    rj.CollideConnected = false;
                    world.AddJoint(rj);

                    body.FixtureList[1].Body.OnCollision += Body_OnCollision;
                    body.FixtureList[1].Body.OnSeparation += Body_OnSeparation;
                }

                prevBody = body;
                _pathBodies.Add(body);
            }
#endif
        }

        #region Collisions



#if !EDITOR

        protected override void Body_OnSeparation(Fixture fixtureA, Fixture fixtureB)
        {
            if (_touchedRopeFixtures.Contains(fixtureA) && fixtureB == Player.Instance.Body.FixtureList[0])
            {
                _touchedRopeFixtures.Remove(fixtureA);
            }

            if (_touchedRopeFixtures.Count == 0)
            {
                _inRange = false;
            }

        }

        protected override bool Body_OnCollision(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            if (fixtureB != Player.Instance.Body.FixtureList[0])
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


#endif



        #endregion

        #endregion
    }
}
