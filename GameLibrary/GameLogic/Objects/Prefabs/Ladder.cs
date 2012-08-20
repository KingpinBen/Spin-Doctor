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
        private new Direction _orientation = Direction.Vertical;
        [ContentSerializer(Optional = true)]
        private int _climbableSections = 1;

#if EDITOR
        private Texture2D editorTexture;
#else
        private bool _inRange;
        private bool _grabbed;
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

                if (value == Direction.Vertical)
                {
                    _rotation = 0;
                }
                else
                { 
                    _rotation = -MathHelper.PiOver2;
                }
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
        public override float Height
        {
            get
            {
                if (_orientation == Direction.Horizontal)
                    return -_width;
                return _height * _climbableSections;
            }
        }

        [ContentSerializerIgnore]
        public override float Width
        {
            get
            {
                if (_orientation == Direction.Horizontal)
                    return -_height * _climbableSections;
                return _width;
            }
        }
#endif
        #endregion

        #region Constructor and Load
        public Ladder() : base() { }


        public override void Load(ContentManager content, World world)
        {
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
            this.RegisterObject();
#endif
        }

        #endregion

        #region Update and Draw

        public override void Update(float delta)
        {
#if !EDITOR
            if (Player.Instance.PlayerState == PlayerState.Dead)
            {
                return;
            }
            else
            {
                if (_orientation == Direction.Vertical && (Camera.Instance.UpIs == UpIs.Left || Camera.Instance.UpIs == UpIs.Right))
                {
                    return;
                }

                if (_orientation == Direction.Horizontal && (Camera.Instance.UpIs == UpIs.Up || Camera.Instance.UpIs == UpIs.Down))
                {
                    return;
                }

                if (_inRange)
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
            }

            //  Just a quick error grab.
            if (_grabbed && Player.Instance.PlayerState != PlayerState.Climbing)
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
                _tint, this._rotation, new Vector2(_texture.Width, (_texture.Height * _climbableSections)) * 0.5f, 1.0f, SpriteEffects.None, _zLayer);
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

                if (!_grabbed)
                {
                    this._grabbed = true;
                }

                if (_orientation == Direction.Vertical)
                {
                    Moveto = new Vector2(this.Position.X, 0);
                }
                else if (_orientation == Direction.Horizontal)
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
            {
                newWidth *= -1;
            }

            if (_orientation == Direction.Vertical)
            {
                this.Body = BodyFactory.CreateRectangle(world, ConvertUnits.ToSimUnits(newWidth * 0.25f), ConvertUnits.ToSimUnits((_height * _climbableSections) - 18), 1);
            }
            else
            {
                this.Body = BodyFactory.CreateRectangle(world, ConvertUnits.ToSimUnits((_height * _climbableSections) - 18), ConvertUnits.ToSimUnits(newWidth * 0.25f), 1);
            }

            this.Body.Position = ConvertUnits.ToSimUnits(Position);
            this.Body.IsSensor = true;
            this.Body.CollidesWith = Category.Cat10;
            this.Body.OnCollision += Body_OnCollision;
            this.Body.OnSeparation += Body_OnSeparation;
#endif
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
            if (fixtureB == Player.Instance.GrabFixture)
            {
                if (_touchingFixtures.Contains(fixtureB))
                {
                    _touchingFixtures.Remove(fixtureB);

                    //  Initiate disconnecting the player.
                    _inRange = false;
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
            if (fixtureB == Player.Instance.GrabFixture)
            {
                if (!_touchingFixtures.Contains(fixtureB))
                {
                    _touchingFixtures.Add(fixtureB);
                    _inRange = true;
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
