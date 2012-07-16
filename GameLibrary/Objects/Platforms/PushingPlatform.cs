//--------------------------------------------------------------------------
//--    
//--    Spin Doctor - Pushing Platform
//--    
//--    
//--    
//--    Description
//--    ===============
//--    Small platform that goes on a surface and pushes the player 
//--    away (air/steam etc)
//--    
//--    Revision List
//--    ===============
//--    BenG - Initial
//--    BenG - Changed how the player is pushed. May need readjusting
//--    BenG - Redone the pushing again. Linear velocity wasn't being reset
//--           and caused breakages in forces.
//--    BenG - Fixed to work multidirectionally. Also changed collision handling
//--           to better detect player. Also pushing direction is now based on orientation.
//--    
//--    TBD
//--    ==============
//--    
//--
//--------------------------------------------------------------------------

#region Using Statements
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameLibrary.Objects.Triggers;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GameLibrary.Managers;
using Microsoft.Xna.Framework.Content;
using FarseerPhysics.Dynamics.Contacts;
using GameLibrary.Drawing;
using GameLibrary.Assists;
using FarseerPhysics.Factories;
using System.ComponentModel;
#endregion

namespace GameLibrary.Objects
{
    public class PushingPlatform : Trigger
    {
        #region Fields

#if EDITOR
        private Texture2D _devTexture;
#else
        private float _elapsed;
        private float createDelay;
        private Sprite _exhaustSprite;
#endif

        [ContentSerializer]
        private float _timeBetweenPulses;
        [ContentSerializer]
        private string _exhaustTextureAsset;
        [ContentSerializer]
        private float _timePulsing;
        [ContentSerializer]
        private Vector2 _triggerOffset;

        #endregion

        #region Properties

#if EDITOR
        [ContentSerializerIgnore, CategoryAttribute("Object Specific")]
        public float PushCooldown
        {
            get
            {
                return _timeBetweenPulses;
            }
            set
            {
                _timeBetweenPulses = value;
            }
        }

        [ContentSerializerIgnore, CategoryAttribute("Object Specific")]
        public string ExhaustTextureAsset
        {
            get
            {
                return _exhaustTextureAsset;
            }
            set
            {
                _exhaustTextureAsset = value;
            }
        }

        [ContentSerializerIgnore, CategoryAttribute("Object Specific")]
        public float PushLength
        {
            get
            {
                return _timePulsing;
            }
            set
            {
                _timePulsing = value;
            }
        }

        [ContentSerializerIgnore, CategoryAttribute("Object Specific")]
        public override float TriggerWidth
        {
            get
            {
                if (_orientation == Objects.Orientation.Up || _orientation == Objects.Orientation.Down)
                {
                    return _triggerWidth;
                }
                else
                {
                    return _triggerHeight;
                }
            }
            set
            {
                if (_orientation == Objects.Orientation.Up || _orientation == Objects.Orientation.Down)
                {
                    _triggerWidth = value;
                }
                else
                {
                    _triggerHeight = value;
                }
            }
        }

        [ContentSerializerIgnore, CategoryAttribute("Object Specific")]
        public override float TriggerHeight
        {
            get
            {
                if (_orientation == Objects.Orientation.Up || _orientation == Objects.Orientation.Down)
                {
                    return _triggerHeight;
                }
                else
                {
                    return _triggerWidth;
                }
            }
            set
            {
                if (_orientation == Objects.Orientation.Up || _orientation == Objects.Orientation.Down)
                {
                    _triggerHeight = value;
                }
                else
                {
                    _triggerWidth = value;
                }
            }
        }

        [ContentSerializerIgnore, CategoryAttribute("Object Specific")]
        public Vector2 TriggerOffset
        {
            get
            {
                return _triggerOffset;
            }
            set
            {
                _triggerOffset = value;
            }
        }
#else
#endif
        

        #endregion

        #region Constructor
        public PushingPlatform()
        {
            
        }

        public void Init(Vector2 position, string texLoc, string fxTexLoc)
        {
            base.Init(position);

            this._textureAsset = texLoc;
            this._tint = Color.White;
            this._exhaustTextureAsset = fxTexLoc;
            this._showHelp = false;
            this._message = "";
            this._timePulsing = 1.0f;
            this._timeBetweenPulses = 3.0f;
        }
        #endregion

        #region Load
        public override void Load(ContentManager content, World world)
        {
            this._texture = content.Load<Texture2D>(this._textureAsset);

            this._origin = new Vector2(this._texture.Width * 0.5f, this._texture.Height * 0.5f);

            this.GetRotationFromOrientation();

#if EDITOR
            _devTexture = content.Load<Texture2D>(FileLoc.DevTexture());

            if (Width == 0 || Height == 0)
            {
                Width = _texture.Width;
                Height = _texture.Height; 
            }

            if (this.TriggerHeight == 0 || this.TriggerWidth == 0)
            {
                this._triggerWidth = _texture.Width * 0.33f;
                this._triggerHeight = 250.0f;
            }
#else
            this.SetupTrigger(world);
            this.CreateSprite(content);
#endif
        }
        #endregion

        public override void Update(GameTime gameTime)
        {
#if EDITOR
#else
            _elapsed += (float)gameTime.ElapsedGameTime.TotalMilliseconds * 0.001f;

            if (Triggered)
            {
                //  While it's triggered, we want to make some sprites come out of it.
                createDelay -= (float)gameTime.ElapsedGameTime.TotalMilliseconds * 0.001f;

                if (createDelay <= 0.0f)
                {
                    Sprite newSprite = (Sprite)_exhaustSprite.Clone();

                    Sprite_Manager.AddSprite(newSprite);
                    createDelay = 0.13f;
                }

                if (TouchingFixtures.Count > 0)
                {
                    ApplyForces();
                }

                //  Check if it should stop pulsing
                if (_elapsed >= _timePulsing)
                {
                    _elapsed = 0;
                    Triggered = false;
                    createDelay = 0.0f;
                }
            }
            else 
            {
                //  Check if it should activate.
                if (_elapsed >= _timeBetweenPulses)
                {
                    Triggered = true;
                    _elapsed = 0;
                }
            }
#endif
        }

        #region Draw
#if EDITOR
        public override void Draw(SpriteBatch sb)
        {
            sb.Draw(this._texture, this._position, null, this._tint, this._rotation, this._origin, 1.0f, SpriteEffects.None, this._zLayer);

            sb.Draw(this._devTexture, this.GetTriggerPosition(false), new Rectangle(0, 0, (int)_triggerWidth, (int)_triggerHeight), Color.White * 0.5f, 
                this._rotation, new Vector2(_triggerWidth * 0.5f, _triggerHeight * 0.5f), 1.0f, SpriteEffects.None, this._zLayer);
        }
#else
        public override void Draw(SpriteBatch sb)
        {
            sb.Draw(this._texture, this._position, null, this._tint, this.TextureRotation, this._origin, 1f, SpriteEffects.None, this._zLayer);

#if Development
            sb.DrawString(Fonts.DebugFont, "Count: " + TouchingFixtures.Count, this.Position + new Vector2(0, 50), Color.Red);
            base.Draw(sb);
#endif
        }
#endif
        #endregion

        #region Private Methods

        #region CreateSprite
        private void CreateSprite(ContentManager Content)
        {
#if EDITOR

#else
            _exhaustSprite = new Sprite();
            _exhaustSprite.Init(this._position, new Point(68, 64), new Point(14, 1), 1);
            _exhaustSprite.Velocity = SpinAssist.ModifyVectorByOrientation(new Vector2(0, -20), _orientation);
            _exhaustSprite.Alpha = 0.6f;
            _exhaustSprite.Scale = 2.0f;
            _exhaustSprite.Rotation = this.TextureRotation;
            _exhaustSprite.Tint = Color.White;
            _exhaustSprite.TimesToPlay = 1;
            _exhaustSprite.SetTexture(Content.Load<Texture2D>(_exhaustTextureAsset));
            _exhaustSprite.Animate = false;
            //_exhaustSprite.Load();
#endif
        }
        #endregion

        #region Collisions
        protected override void Body_OnSeparation(Fixture fixtureA, Fixture fixtureB)
        {
#if EDITOR
#else
            TouchingFixtures.Remove(fixtureB);
#endif
        }

        protected override bool Body_OnCollision(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
#if EDITOR
            return true;
#else
            if (!TouchingFixtures.Contains(fixtureB))
            {
                TouchingFixtures.Add(fixtureB);
            }

            return true;
#endif
        }
        #endregion

        private Vector2 GetTriggerPosition(bool convert)
        {
            Vector2 bodyPos = Vector2.Zero;
            bodyPos.Y = (this.Height + this.TriggerHeight) * 0.5f;
            bodyPos = SpinAssist.ModifyVectorByOrientation(bodyPos, _orientation);
            Vector2 newOffset = SpinAssist.ModifyVectorByOrientation(_triggerOffset, _orientation);
            if (convert)
                return ConvertUnits.ToSimUnits(this._position - bodyPos + newOffset);// + bodyPos);// +newOffset;
            else
                return this._position - bodyPos + newOffset;
        }

        #region SetupTrigger
        protected override void SetupTrigger(World world)
        {
#if EDITOR

#else
            //Vector2 bodyPos = Vector2.Zero;
            //bodyPos.Y -= this._texture.Height + (this.TriggerHeight * 0.5f);
            //bodyPos = SpinAssist.ModifyVectorByOrientation(bodyPos, _orientation);

            this.Body = BodyFactory.CreateRectangle(world, ConvertUnits.ToSimUnits(TriggerWidth), ConvertUnits.ToSimUnits(TriggerHeight), 1.0f); 
            this.Body.Position = GetTriggerPosition(true);
            this.Body.IsSensor = true;
            this.Body.OnCollision += Body_OnCollision;
            this.Body.OnSeparation += Body_OnSeparation;
#endif
        }
        #endregion

        private void ApplyForces()
        {
#if EDITOR

#else
            Vector2 dir = SpinAssist.ModifyVectorByOrientation(new Vector2(0, -100), _orientation);
            dir.Normalize();
            dir *= 15;

            for (int i = TouchingFixtures.Count - 1; i >= 0; i--)
            {
                if (TouchingFixtures[i] == Player.Instance.WheelBody.FixtureList[0] || 
                    TouchingFixtures[i] == Player.Instance.Body.FixtureList[1])
                {
                    Player.Instance.ApplyForce(dir, 70.0f);
                    continue;
                }

                TouchingFixtures[i].Body.ApplyForce(dir);
            }
#endif            
        }

        #endregion
    }
}
