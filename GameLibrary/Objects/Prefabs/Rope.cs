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

        private List<Body> _pathBodies = new List<Body>();
        private bool _inRange;
        private Texture2D endTexture;

        //  We need to keep the world in here to create the grabbing joint
        //  when it's needed.
        private World World;
        #endregion

        #region Properties

        [ContentSerializerIgnore]
        public Vector2 EndPosition
        {
            get { return _endPosition; }
#if EDITOR
            set
#else
            protected set
#endif
            {
                _endPosition = value;
            }
        }
        [ContentSerializerIgnore]
        public int ChainCount
        {
            get { return _chainCount; }
#if EDITOR
            set
#else
            protected set
#endif
            {
                _chainCount = value;
            }
        }        
#if EDITOR
        [ContentSerializerIgnore]
        public override float Height { get; set; }
        [ContentSerializerIgnore]
        public override float Width { get; set; }
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
            this.ChainCount = 10;

            this._textureAsset = texLoc;
        }
        #endregion

        #region Load
        public override void Load(ContentManager content, World world)
        {
            endTexture = content.Load<Texture2D>("Assets/Images/Textures/Rope/ropeEnd");
            _texture = content.Load<Texture2D>(_textureAsset);

#if EDITOR
            this.Width = endTexture.Width;
            this.Height = endTexture.Height;
            this._endPosition = this._position + new Vector2(0, _texture.Height * ChainCount);

#else
            this.World = world;
            SetUpPhysics(world);
#endif
        }
        #endregion

        #region Update
        public override void Update(GameTime gameTime)
        {
            if (Camera.LevelRotating)
            {
                //  Only awakens the ropes bodies if the last one is asleep
                //  and the level rotates. The last one will drag the rest.
                //  saves a bit of time.

                if (_pathBodies[_pathBodies.Count - 1].Awake == false)
                    _pathBodies[_pathBodies.Count - 1].Awake = true;       
            }

            //if (the joint == null)
            //{
            //    Player.Instance.GrabRotation = _pathBodies[_pathBodies.Count - 2].Rotation;

            //    if (_inRange && Input.Interact())
            //    {
            //        //  Make the joint
            //        Player.Instance.GrabRope();
            //    }
            //}
            //else
            //{
            //    if (Input.Jump())
            //    {
            //        //  World.RemoveJoint(the joint);
                    
            //    }
            //}

            ////  The rope already has the fix that the base does, so no point in using it. 
            ////  Also makes it crash..
            //  base.Update(gameTime);
        }
        #endregion

        #region Collisions
        protected override void Body_OnSeparation(Fixture fixtureA, Fixture fixtureB)
        {
            if (fixtureB.Body != Player.Instance.Body)
                return;

            _inRange = false;
        }

        protected override bool Body_OnCollision(Fixture fixtureA, Fixture fixtureB, FarseerPhysics.Dynamics.Contacts.Contact contact)
        {
            if (fixtureB.Body != Player.Instance.Body)
                return true;

            //if (the joint != null)
            //{
            //    return true;
            //}

            _inRange = true;
            return true;
        }
        #endregion

        #region Draw
#if EDITOR
        public override void Draw(SpriteBatch sb)
        {
            sb.Draw(endTexture, this._position, null, Color.Tomato, 0.0f, new Vector2(endTexture.Width / 2, endTexture.Height / 2), 1.0f, SpriteEffects.None, this.zLayer);
            sb.Draw(endTexture, this._endPosition, null, Color.White, 0.0f, new Vector2(endTexture.Width / 2, endTexture.Height / 2), 0.6f, SpriteEffects.None, this.zLayer);
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

        #region SetupPhysics
        protected override void SetUpPhysics(World world)
        {
            Path _ropePath = new Path();
            _ropePath.Add(ConvertUnits.ToSimUnits(this._position));
            _ropePath.Add(ConvertUnits.ToSimUnits(this._endPosition));

            float width = ConvertUnits.ToSimUnits(this._texture.Width / 2);//    0.2
            float height = ConvertUnits.ToSimUnits(this._texture.Height / 2);//  0.3
            float extraRoom = ConvertUnits.ToSimUnits(2.0f);

            PolygonShape shape = new PolygonShape(PolygonTools.CreateCircle(height, 8), ConvertUnits.ToSimUnits(_mass));
            _pathBodies = PathManager.EvenlyDistributeShapesAlongPath(world, _ropePath, shape, BodyType.Dynamic, ChainCount);

            FixedRevoluteJoint fixedJoint = JointFactory.CreateFixedRevoluteJoint(world, _pathBodies[0], new Vector2(0, 0), _pathBodies[0].Position);
            fixedJoint.MaxMotorTorque = 0f;
            PathManager.AttachBodiesWithRevoluteJoint(world, _pathBodies, new Vector2(0f, height + extraRoom), new Vector2(0f, -(height + extraRoom)), false, true);
        }
        #endregion
    }
}
