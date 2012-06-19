﻿//--------------------------------------------------------------------------
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
//--    Make it climb in sections.
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
using GameLibrary.Managers;
using GameLibrary.Assists;
using GameLibrary.Objects.Triggers;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using FarseerPhysics.Dynamics.Joints;
using FarseerPhysics.Dynamics.Contacts;
using GameLibrary.Drawing;
#endregion

namespace GameLibrary.Objects
{
    public class Ladder : StaticObject
    {
        #region Fields

        [ContentSerializer]
        private new Direction _orientation;

        [ContentSerializer]
        private int _climbableSections;

        [ContentSerializerIgnore]
        private bool _inGrabbingRange;

        [ContentSerializerIgnore]
        private bool _grabbed;

        [ContentSerializerIgnore]
        List<Fixture> TouchingBodies = new List<Fixture>();

#if EDITOR
        private Texture2D editorTexture;
#endif

        #endregion

        #region Properties

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
#if EDITOR
            set
            {
                _orientation = value;
                GetRotationFromOrientation();
            }
#endif
        }

        [ContentSerializerIgnore]
        public override float Height
        {
            get
            {
                if (Orientation == Direction.Horizontal)
                    return _width * _climbableSections;
                return _height * _climbableSections;
            }
#if EDITOR
            set
            {
                //if (Orientation == Direction.Horizontal)
                //    _width = value;
                //else
                //    _height = value;
            }
#endif
        }

        [ContentSerializerIgnore]
        public override float Width
        {
            get
            {
                if (Orientation == Direction.Horizontal)
                    return _height * _climbableSections;
                return _width;
            }
#if EDITOR
            set
            {
                //if (Orientation == Direction.Horizontal)
                //    _height = value;
                //else
                //    _width = value;
            }
#endif
        }

        [ContentSerializerIgnore]
        public override float TextureRotation
        {
            get
            {
                return _rotation;
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
#if EDITOR
            set
            {
                _climbableSections = value;
            }
#endif
        }

        #endregion

        #region Constructor
        public Ladder()
            : base()
        {

        }

        public override void Init(Vector2 position, float height, string texLoc)
        {
            this._height = height;
            this._width = 20;
            this._position = position;
            this._tint = Color.White;
            this._textureAsset = texLoc;
            this._mass = 10.0f;
            this._useBodyRotation = false;
            this._orientation = Direction.Vertical;
            this._climbableSections = 1;
        }
        #endregion

        #region Load and Setup
        public override void Load(ContentManager content, World world)
        {
            this.Texture = content.Load<Texture2D>
                (_textureAsset);

            //  Ladder grows up
            this._origin = new Vector2(Texture.Width / 2, Texture.Height / 2);
            this._width = this.Texture.Width;


#if EDITOR
            if (this.Texture.Height > 10)
                this._height = this.Texture.Height;
            else
                this._height = 10;

            if (this.Texture.Width > 10)
                this._width = this.Texture.Width;
            else
                this._width = 10;

            editorTexture = content.Load<Texture2D>("Assets/Other/Dev/Trigger");
            return;
#endif
            this.SetupPhysics(world);
        }
        #endregion

        public override void Update(GameTime gameTime)
        {
            if (_orientation == Direction.Vertical && (Camera.UpIs == upIs.Left || Camera.UpIs == upIs.Right)) return;
            if (_orientation == Direction.Horizontal && (Camera.UpIs == upIs.Up || Camera.UpIs == upIs.Down)) return;

            if (_inGrabbingRange)
            {
                if (Input.Grab())
                {
                    if (Grabbed)
                        DisconnectPlayer();
                    else
                        ConnectPlayer();
                }
            }

            //  Make sure the player isnt grabbing the ladder if they aren't climbing.
            if (Player.Instance.PlayerState != pState.Climbing && Grabbed)
                DisconnectPlayer();
        }

        #region Collisions
        /// <summary>
        /// When the player separates from the body we'll need to set the player as 
        /// out of range of the body and disconnect the player from it. We'll need to disconnect
        /// the player in case they go higher than the ladder.
        /// </summary>
        protected override void Body_OnSeparation(Fixture fixtureA, Fixture fixtureB)
        {
            //  Whichever body just separated, remove it form the list.
            TouchingBodies.Remove(fixtureB);

            //  If the list isn't empty, go no further.
            if (TouchingBodies.Count > 0)
            {
                return;
            }

            //  Initiate disconnecting the player.
            _inGrabbingRange = false;

            if (_grabbed)
            {
                DisconnectPlayer();
            }
        }
        /// <summary>
        /// Set the player in range to enable climbing
        /// </summary>
        protected override bool Body_OnCollision(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            //  If the list doesn't already have this fixture as touching the Body, add it.
            if (!TouchingBodies.Contains(fixtureB))
            {
                TouchingBodies.Add(fixtureB);
            }

            //  If they can already grab the rope, no need to continue.
            if (_inGrabbingRange)
            {
                return true;
            }
            else
            {
                _inGrabbingRange = true;
            }

            return true;
        }
        #endregion

        #region Draw Calls
#if EDITOR
        public override void Draw(SpriteBatch sb)
        {
            sb.Draw(this.editorTexture, Position - new Vector2(this.editorTexture.Width / 2, this.Height / 2),
                new Rectangle(0, 0, (int)this.editorTexture.Width, (int)Height),
                Color.White * 0.3f, 0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0.0f);

            sb.Draw(this.Texture, Position,
                new Rectangle(0, 0, (int)this.Width, (int)this.Height),
                this.Tint, this.TextureRotation, new Vector2(this.Width / 2, this.Height / 2), 1.0f, SpriteEffects.None, zLayer);
        }
#else

        public override void Draw(SpriteBatch sb)
        {
        #region Development
#if Development
            sb.DrawString(Fonts.DebugFont, "Grabbed: " + Grabbed.ToString(), Position + new Vector2(20, 0), Color.Red, 0f, Vector2.Zero, 2f, SpriteEffects.None, 0f);
            sb.DrawString(Fonts.DebugFont, "InRange: " + PlayerInRange.ToString(), Position + new Vector2(20, 15), Color.Red, 0f, Vector2.Zero, 2f, SpriteEffects.None, 0f);
            sb.DrawString(Fonts.DebugFont, "Rotation: "  + Rotation, Position + new Vector2(20,30), Color.Red, 0f, Vector2.Zero, 2f, SpriteEffects.None, 0f);
            sb.DrawString(Fonts.DebugFont, "Orientation: " + _orientation, Position + new Vector2(20, 45), Color.Red, 0f, Vector2.Zero, 2f, SpriteEffects.None, 0f);
#endif
            #endregion

            sb.Draw(this.Texture, ConvertUnits.ToDisplayUnits(this.Body.Position) + new Vector2(Width / 2, Height / 2),
                new Rectangle(0, 0, (int)-_width, (int)-_height),
                this.Tint, this.TextureRotation, Vector2.Zero, 1.0f, SpriteEffects.None, zLayer);
        }
#endif
        #endregion

        #region Private Methods

        #region Player connection handling
        /// <summary>
        /// Break the player off the ladder.
        /// </summary>
        private void DisconnectPlayer()
        {
            this.Grabbed = false;

            Camera.AllowRotation = true;
            Player.Instance.ToggleBodies(true);
            Player.Instance.ForceFall();
        }

        /// <summary>
        /// Connect the player to the ladder allowing climbing
        /// </summary>
        private void ConnectPlayer()
        {
            if (Camera.LevelRotating) return;

            Vector2 Moveto = Vector2.Zero;
            Camera.AllowRotation = false;

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
        #endregion

        #region Setup Body
        private void SetupPhysics(World world)
        {
            float newWidth = Width;
            //  If the width is negative, make it positive.
            if (newWidth < 0)
                newWidth *= -1;

            if (Orientation == Direction.Vertical)
                this.Body = BodyFactory.CreateRectangle(world, ConvertUnits.ToSimUnits(newWidth / 4), ConvertUnits.ToSimUnits(Height), ConvertUnits.ToSimUnits(_mass));
            else
                this.Body = BodyFactory.CreateRectangle(world, ConvertUnits.ToSimUnits(newWidth), ConvertUnits.ToSimUnits(Height / 4), ConvertUnits.ToSimUnits(_mass));

            this.Body.Position = ConvertUnits.ToSimUnits(Position);
            this.Body.IsSensor = true;
            this.Body.CollidesWith = Category.Cat10;
            this.Body.OnCollision += Body_OnCollision;
            this.Body.OnSeparation += Body_OnSeparation;

        }

        #endregion

        protected override void GetRotationFromOrientation()
        {
            if (Orientation == Direction.Vertical)
                _rotation = 0;
            else
                _rotation = MathHelper.PiOver2;
        }

        #endregion
    }
}
