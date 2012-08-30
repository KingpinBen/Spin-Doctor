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
            this._endTexture = content.Load<Texture2D>(_endTextureAsset);
            this._texture = content.Load<Texture2D>(_textureAsset);

#if EDITOR
            if (this.Width == 0 || this.Height == 0)
            {
                this._width = _endTexture.Width;
                this._height = _endTexture.Height;
                
            }

            if (_endPosition == Vector2.Zero)
                this._endPosition = this._position + new Vector2(0, _texture.Height * ChainCount);
#else
            this._world = world;
            this.SetupPhysics(world);
            this.RegisterObject();
#endif
        }

        public override void Update(float delta)
        {
#if !EDITOR
            Player playerInstance = Player.Instance;

            #region Awaken Body
            //  If the body has fallen asleep, we need to wake it if the 
            //  level rotates.
            if (Camera.Instance.IsLevelRotating)
            {
                if (_pathBodies[_pathBodies.Count - 1].Awake == false)
                {
                    this._pathBodies[_pathBodies.Count - 1].Awake = true;
                }
            }
            #endregion

            if (!_world.JointList.Contains(_ropeJoint))
            {
                if (playerInstance.PlayerState != PlayerState.Dead && 
                    InputManager.Instance.Grab(true) && 
                    _inRange)
                {
                    int index = 0;
                    Body playerBody = playerInstance.GetWheelBody();
                    float smallestDistance = Math.Abs(
                        Vector2.Subtract(playerBody.Position, 
                                         _touchedRopeFixtures[0].Body.Position).Length());
                    
                    //  Starting it at 3 instead of 1 or 2 because they 
                    //  shouldn't really be able to grab the top ones
                    for(int i = 3; i < _touchedRopeFixtures.Count; i++)
                    {
                        float distance = Math.Abs(
                            Vector2.Subtract(playerBody.Position, 
                                             _touchedRopeFixtures[i].Body.Position).Length());

                        if (distance < smallestDistance)
                        {
                            smallestDistance = distance;
                            this._grabbedIndex = i;
                        }
                    }

                    this._ropePlayerJoint = new RevoluteJoint(_touchedRopeFixtures[index].Body, 
                        playerInstance.GetMainBody(), Vector2.Zero, Vector2.Zero);
                    this._world.AddJoint(_ropePlayerJoint);

                    this._ropeJoint = new RopeJoint(_pathBodies[0], 
                        playerInstance.GetMainBody(), Vector2.Zero, Vector2.Zero);
                    this._world.AddJoint(_ropeJoint);

                    playerInstance.GrabRope();
                }
            }
            else
            {
                Vector2 ropeDims = _ropeJoint.WorldAnchorB - _ropeJoint.WorldAnchorA;
                float rotation = (float)Math.Atan2(ropeDims.Y, ropeDims.X);

                rotation -= MathHelper.PiOver2;

                playerInstance.SetRotation(rotation);

                if (InputManager.Instance.Jump(true) || InputManager.Instance.Grab(true) || 
                    playerInstance.PlayerState == PlayerState.Dead)
                {
                    _world.RemoveJoint(_ropeJoint);
                    _world.RemoveJoint(_ropePlayerJoint);

                    if (playerInstance.PlayerState == PlayerState.Swinging)
                    {
                        playerInstance.ForceFall();
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
                    this._tint, _pathBodies[0].Rotation, new Vector2(_endTexture.Width, _endTexture.Height) * 0.5f, 1f,
                    SpriteEffects.None, this._zLayer);

            for (int i = 1; i < _pathBodies.Count; i++)
            {
                sb.Draw(_texture, ConvertUnits.ToDisplayUnits(_pathBodies[i].Position), null,
                    this._tint, _pathBodies[i].Rotation, new Vector2(_texture.Width, _texture.Height) * 0.5f, 1f,
                    SpriteEffects.None, this._zLayer);
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

            List<Shape> shapes = new List<Shape>(2);
            shapes.Add(new PolygonShape(PolygonTools.CreateRectangle(0.5f, 0.5f, new Vector2(-0.1f, 0f), 0f), 1f));
            shapes.Add(new CircleShape(0.5f, 1f));

            _pathBodies = PathManager.EvenlyDistributeShapesAlongPath(world, _ropePath, shapes,
                                                                      BodyType.Dynamic, _chainCount);

            JointFactory.CreateFixedRevoluteJoint(world, _pathBodies[0], Vector2.Zero, startPos);

            PathManager.AttachBodiesWithRevoluteJoint(world, _pathBodies, new Vector2(0f, -0.5f), new Vector2(0f, 0.5f),
                                                      false, true);

            for (int i = 1; i < _pathBodies.Count; i++)
            {
                _pathBodies[i].FixtureList[0].CollidesWith = Category.All & ~Category.Cat10 & ~Category.Cat12;
                _pathBodies[i].FixtureList[1].CollidesWith = Category.All & ~Category.Cat10 & ~Category.Cat12;

                Fixture fix = FixtureFactory.AttachCircle(height * 2, 0.0f, _pathBodies[i]);
                fix.IsSensor = true;

                fix.OnCollision += Body_OnCollision;
                fix.OnSeparation += Body_OnSeparation;
            }

            //Body prevBody = new Body(world); ;
            //for (int i = 0; i < _chainCount; ++i)
            //{
            //    Body body = new Body(world);
            //    body.BodyType = BodyType.Dynamic;
            //    body.Position = startPos + new Vector2(0, height * i);

            //    if (i == 0)
            //    {
            //        Fixture fixture = body.CreateFixture(rotationPointShape);
            //        fixture.Friction = 0.2f;
            //        body.AngularDamping = 0.4f;
                    
            //        FixedRevoluteJoint fixedJoint = JointFactory.CreateFixedRevoluteJoint(world, body, Vector2.Zero, startPos);
            //    }
            //    else
            //    {
            //        Fixture fixture = body.CreateFixture(shape);
            //        fixture.Friction = 0.2f;

            //        Fixture sensorFix = FixtureFactory.AttachCircle(height * 2, 0.0f, body);
            //        sensorFix.IsSensor = true;

            //        fixture.CollidesWith = Category.All & ~Category.Cat10 & ~Category.Cat12;

            //        RopeJoint rj = new RopeJoint(prevBody, body, new Vector2(0.0f, height), new Vector2(0.0f, -height * 0.5f));

            //        rj.CollideConnected = false;
            //        world.AddJoint(rj);

            //        body.FixtureList[1].Body.OnCollision += Body_OnCollision;
            //        body.FixtureList[1].Body.OnSeparation += Body_OnSeparation;
            //    }

            //    prevBody = body;
            //    _pathBodies.Add(body);
            //}
#endif
        }

        #region Collisions



#if !EDITOR

        protected override void Body_OnSeparation(Fixture fixtureA, Fixture fixtureB)
        {
            if (fixtureB == Player.Instance.GetMainBody().FixtureList[0])
            {
                if (_touchedRopeFixtures.Contains(fixtureA))
                {
                    _touchedRopeFixtures.Remove(fixtureA);
                }

                if (_touchedRopeFixtures.Count == 0)
                {
                    _inRange = false;
                }
            }
        }

        protected override bool Body_OnCollision(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            if (fixtureB == Player.Instance.GetMainBody().FixtureList[0])
            {
                if (!_touchedRopeFixtures.Contains(fixtureA))
                {
                    this._touchedRopeFixtures.Add(fixtureA);
                    this._inRange = true;
                }
            }

            return true;
        }


#endif



        #endregion

        #endregion
    }
}
