//--------------------------------------------------------------------------
//--    
//--    Spin Doctor
//--    
//--    Description
//--    ===============
//--    Climbable ladder
//--    
//--    Revision List
//--    ===============
//--    BenG - Initial
//--    BenG - Added better handling of the ladder
//--    BenG - PC support
//--    BenG - Players position is pushed on the X towards the centre of the
//--           ladder. Needs corrections though.
//--    BenG - Texture and locations added/fixed, new Tex required
//--    BenG - Displays correctly in Editor.
//--    
//--    TBD
//--    ==============
//--    Fix the jump thing that happens when the player goes higher than the
//--    ladder (when disconnected).
//--
//--------------------------------------------------------------------------

//#define Development

#region Using Statements
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using FarseerPhysics.Dynamics.Joints;
using FarseerPhysics.Dynamics.Contacts;
using System.ComponentModel;
using GameLibrary.Graphics.Camera;
using GameLibrary.GameLogic.Controls;
using GameLibrary.Helpers;
using GameLibrary.Graphics;
using GameLibrary.GameLogic.Characters;
#endregion

namespace GameLibrary.GameLogic.Objects
{
    public class Ladder : StaticObject
    {
        #region Fields

        [ContentSerializer(Optional = true)]
        private new Direction _orientation;
        [ContentSerializer(Optional = true)]
        private int _climbableSections;

#if EDITOR
        private Texture2D editorTexture;
#else
        private bool _inGrabbingRange;
        private bool _grabbed;
        List<Fixture> _touchingBodies = new List<Fixture>();
        Fixture[] targetFixtures;
#endif

        #endregion

        #region Properties

#if EDITOR
        [ContentSerializerIgnore, CategoryAttribute("General")]
        public new Direction Orientation
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
        public override float Height
        {
            get
            {
                if (Orientation == Direction.Horizontal)
                    return _width;
                return _height * _climbableSections;
            }
            set { }
        }
        [ContentSerializerIgnore, CategoryAttribute("General")]
        public override float Width
        {
            get
            {
                if (Orientation == Direction.Horizontal)
                    return _height * _climbableSections;
                return _width;
            }
            set { }
        }
        [ContentSerializerIgnore, CategoryAttribute("Object Specific")]
        public int Bars
        {
            get
            {
                return _climbableSections;
            }
            set
            {
                _climbableSections = value;
            }
        }
        [ContentSerializerIgnore, CategoryAttribute("General")]
        public override float Rotation
        {
            get
            {
                return _rotation;
            }
            set { }
        }
#else
        [ContentSerializerIgnore]
        public bool PlayerInRange
        {
            get
            {
                return _inGrabbingRange;
            }
        }

        [ContentSerializerIgnore]
        public new Direction Orientation
        {
            get
            {
                return _orientation;
            }
        }

        [ContentSerializerIgnore]
        public override float Height
        {
            get
            {
                if (Orientation == Direction.Horizontal)
                    return -_width;
                return _height * _climbableSections;
            }
        }

        [ContentSerializerIgnore]
        public override float Width
        {
            get
            {
                if (Orientation == Direction.Horizontal)
                    return -_height * _climbableSections;
                return _width;
            }
        }

        [ContentSerializerIgnore]
        public bool Grabbed
        {
            get
            {
                return _grabbed;
            }
            internal set
            {
                _grabbed = value;
            }
        }

        [ContentSerializerIgnore]
        public int Bars
        {
            get
            {
                return _climbableSections;
            }
        }
#endif
        #endregion

        #region Constructor and Load
        public Ladder()
            : base()
        {

        }

        public override void Init(Vector2 position, string texLoc)
        {
            base.Init(position, texLoc);

            this._orientation = Direction.Vertical;
            this._climbableSections = 1;
        }
        

        public override void Load(ContentManager content, World world)
        {
            //base.Load(content, world);
            this._texture = content.Load<Texture2D>(_textureAsset);

#if EDITOR
            if (this._texture.Height > 10)
                this._height = this._texture.Height;
            else
                this._height = 10;

            if (this._texture.Width > 10)
                this._width = this._texture.Width;
            else
                this._width = 10;

            editorTexture = content.Load<Texture2D>("Assets/Other/Dev/Trigger");
#else
            this.SetupPhysics(world);
            this.targetFixtures = new Fixture[1] { Player.Instance.GrabFixture };
#endif
        }

        #endregion

        #region Update and Draw

        public override void Update(float delta)
        {
#if !EDITOR
            if (_orientation == Direction.Vertical && (Camera.Instance.UpIs == UpIs.Left || Camera.Instance.UpIs == UpIs.Right))
            {
                return;
            }

            if (_orientation == Direction.Horizontal && (Camera.Instance.UpIs == UpIs.Up || Camera.Instance.UpIs == UpIs.Down))
            {
                return;
            }

            if (_inGrabbingRange)
            {
                if (!_grabbed)
                {
                    if (InputManager.Instance.Grab(true) || InputManager.Instance.MoveUp(false) || InputManager.Instance.MoveDown(false))
                    {
                        ConnectPlayer();
                    }
                }
                else
                {
                    if (InputManager.Instance.Grab(true))// || InputManager.Instance.MoveLeft(true) || InputManager.Instance.MoveRight(true)
                    {
                        DisconnectPlayer();
                    }
                }
            }

            //  Just a quick error grab.
            if (Grabbed && Player.Instance.PlayerState != PlayerState.Climbing)
            {
                DisconnectPlayer();
            }
#endif
        }

        #region Draw Calls
#if EDITOR
        public override void Draw(SpriteBatch sb)
        {
            sb.Draw(this._texture, Position,
                new Rectangle(0, 0, (int)_width, (int)_height * _climbableSections),
                this._tint, this._rotation, new Vector2(this._texture.Width, (this._texture.Height * _climbableSections)) * 0.5f, 1.0f, SpriteEffects.None, this._zLayer);
        }
#else

        public override void Draw(SpriteBatch sb, GraphicsDevice graphics)
        {
#if Development
            sb.DrawString(FontManager.Instance.GetFont(FontList.Debug), "Grabbed: " + Grabbed.ToString(), Position + new Vector2(20, 0), Color.Red, 0f, Vector2.Zero, 2f, SpriteEffects.None, 0f);
            sb.DrawString(FontManager.Instance.GetFont(FontList.Debug), "InRange: " + PlayerInRange.ToString(), Position + new Vector2(20, 15), Color.Red, 0f, Vector2.Zero, 2f, SpriteEffects.None, 0f);
            sb.DrawString(FontManager.Instance.GetFont(FontList.Debug), "Orientation: " + _orientation, Position + new Vector2(20, 45), Color.Red, 0f, Vector2.Zero, 2f, SpriteEffects.None, 0f);
#endif

            sb.Draw(this._texture, ConvertUnits.ToDisplayUnits(this.Body.Position),
                new Rectangle(0, 0, (int)_width, (int)_height * _climbableSections),
                this.Tint, this._rotation, new Vector2(this._texture.Width, (this._texture.Height * _climbableSections)) * 0.5f, 1.0f, SpriteEffects.None, zLayer);
        }
#endif
        #endregion

        #endregion

        #region Private Methods



        #region Player Connections


        #region Disconnect
        /// <summary>
        /// Break the player off the ladder.
        /// </summary>
        private void DisconnectPlayer()
        {
#if !EDITOR
            if (_grabbed)
            {
                this._grabbed = false;
                Camera.Instance.AllowRotation = true;
                Player.Instance.ForceFall();
            }
#endif
        }
        #endregion


        #region Connect
        /// <summary>
        /// Connect the player to the ladder allowing climbing
        /// </summary>
        private void ConnectPlayer()
        {
#if EDITOR

#else
            if (!Camera.Instance.IsLevelRotating)
            {
                Vector2 Moveto = Vector2.Zero;
                Camera.Instance.AllowRotation = false;

                if (!Grabbed)
                {
                    this.Grabbed = true;
                }

                if (Orientation == Direction.Vertical)
                {
                    Moveto = new Vector2(this.Position.X, 0);
                }
                else if (Orientation == Direction.Horizontal)
                {
                    Moveto = new Vector2(0, this.Position.Y);
                }

                Player.Instance.JoinLadder(ConvertUnits.ToSimUnits(Moveto));
            }
#endif
        }
        #endregion



        #endregion



        protected override void SetupPhysics(World world)
        {
#if EDITOR

#else
            float newWidth = _width;
            //  If the width is negative, make it positive.
            if (newWidth < 0)
                newWidth *= -1;

            if (Orientation == Direction.Vertical)
                this.Body = BodyFactory.CreateRectangle(world, ConvertUnits.ToSimUnits(newWidth * 0.25f), ConvertUnits.ToSimUnits((_height * _climbableSections) - 18), ConvertUnits.ToSimUnits(_mass));
            else
                this.Body = BodyFactory.CreateRectangle(world, ConvertUnits.ToSimUnits((_height * _climbableSections) - 18), ConvertUnits.ToSimUnits(newWidth / 4), ConvertUnits.ToSimUnits(_mass));

            this.Body.Position = ConvertUnits.ToSimUnits(Position);
            this.Body.IsSensor = true;
            this.Body.CollidesWith = Category.Cat10;
            this.Body.OnCollision += Body_OnCollision;
            this.Body.OnSeparation += Body_OnSeparation;
#endif
        }



        protected override void GetRotationFromOrientation()
        {
            if (Orientation == Direction.Vertical)
                _rotation = 0;
            else
                _rotation = -MathHelper.PiOver2;
        }



        #region Collisions

#if !EDITOR

        #region OnSeparation
        /// <summary>
        /// When the player separates from the body we'll need to set the player as 
        /// out of range of the body and disconnect the player from it. We'll need to disconnect
        /// the player in case they go higher than the ladder.
        /// </summary>
        protected override void Body_OnSeparation(Fixture fixtureA, Fixture fixtureB)
        {
            if (fixtureB == targetFixtures[0])
            {
                if (_touchingBodies.Contains(fixtureB))
                {
                    _touchingBodies.Remove(fixtureB);

                    //  Initiate disconnecting the player.
                    _inGrabbingRange = false;
                    DisconnectPlayer();

                }
            }
        }
#endregion

        #region OnCollision
        /// <summary>
        /// Set the player in range to enable climbing
        /// </summary>
        protected override bool Body_OnCollision(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            if (fixtureB == targetFixtures[0])
            {
                if (!_touchingBodies.Contains(fixtureB))
                {
                    _touchingBodies.Add(fixtureB);
                    _inGrabbingRange = true;
                }
            }

            return true;
        }
        #endregion

#endif

        #endregion

        #endregion
    }
}
