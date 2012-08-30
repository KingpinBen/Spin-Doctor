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
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using FarseerPhysics.Dynamics.Contacts;
using FarseerPhysics.Factories;
using System.ComponentModel;
using GameLibrary.GameLogic.Objects.Triggers;
using GameLibrary.Helpers;
using GameLibrary.Graphics.Drawing;
using GameLibrary.Graphics;
using GameLibrary.GameLogic.Characters;
using Microsoft.Xna.Framework.Audio;
using GameLibrary.Audio;
#endregion

namespace GameLibrary.GameLogic.Objects
{
    public class PushingPlatform : Trigger
    {
        #region Fields

#if EDITOR
        private Texture2D _devTexture;
#else
        private float _elapsed;
        private float _createDelay;
        private Sprite _exhaustSprite;
        private Cue _soundEffect;
        private string _soundEffectAsset;
#endif

        [ContentSerializer(Optional = true)]
        private float _timeBetweenPulses = 3.0f;
        [ContentSerializer(Optional = true)]
        private string _exhaustTextureAsset = "Assets/Images/Effects/steam";
        [ContentSerializer(Optional = true)]
        private float _timePulsing = 1.0f;
        [ContentSerializer(Optional = true)]
        private Vector2 _triggerOffset = Vector2.Zero;

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

        public PushingPlatform() : base() { }

        public override void Load(ContentManager content, World world)
        {
            this._texture = content.Load<Texture2D>(this._textureAsset);
            this._origin = new Vector2(this._texture.Width, this._texture.Height) * 0.5f;

#if EDITOR
            _devTexture = content.Load<Texture2D>(Defines.DEVELOPMENT_TEXTURE);

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
            this.SetupPhysics(world);
            this.RegisterObject();
            this._soundEffectAsset = "Steam_Emit_Constant";

            #region Setup the steam sprite
            _exhaustSprite = new Sprite();
            _exhaustSprite.Init(ConvertUnits.ToDisplayUnits(this.Body.Position) + SpinAssist.ModifyVectorByOrientation(new Vector2(0, _triggerHeight * 0.5f), _orientation));
            _exhaustSprite.SetTexture(content.Load<Texture2D>(_exhaustTextureAsset));
            _exhaustSprite.Alpha = 0.4f;
            _exhaustSprite.AlphaDecay = 0.02f;
            _exhaustSprite.ZLayer = this._zLayer + 0.01f;
            _exhaustSprite.Scale = 0.3f;
            _exhaustSprite.RotationSpeed = 0.001f;
            _exhaustSprite.ScaleFactor = SpinAssist.GetRandom(0.0005f, 0.01f);
            #endregion
#endif
        }

        public override void Update(float delta)
        {
#if !EDITOR
            if (_enabled)
            {
                _elapsed += delta;

                if (_triggered)
                {
                    //  While it's triggered, we want to make some sprites come out of it.
                    _createDelay -= delta;

                    if (_createDelay <= 0.0f)
                    {
                        AddSprite();
                    }

                    if (_touchingFixtures.Count > 0)
                    {
                        ApplyForces();
                    }

                    //  Check if it should stop pulsing
                    if (_elapsed >= _timePulsing)
                    {
                        _elapsed = 0;
                        this.Stop();
                        _createDelay = 0.0f;
                    }
                }
                else
                {
                    //  Check if it should activate.
                    if (_elapsed >= _timeBetweenPulses)
                    {
                        this.Start();
                        _elapsed = 0;
                    }
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
                this._rotation, new Vector2(_triggerWidth * 0.5f, _triggerHeight * 0.5f), 1.0f, SpriteEffects.None, this._zLayer - 0.01f);
        }
#else
        public override void Draw(SpriteBatch sb, GraphicsDevice graphics)
        {
            sb.Draw(this._texture, this._position, null, this._tint, this._rotation, this._origin, 1f, SpriteEffects.None, this._zLayer);
        }
#endif
        #endregion

        #region Private Methods

        /// <summary>
        /// Get the position of the trigger.
        /// </summary>
        /// <param name="convert">Should the output return it converted to sim units?</param>
        /// <returns>The trigger position</returns>
        private Vector2 GetTriggerPosition(bool convert)
        {
            Vector2 bodyPos = Vector2.Zero;

            //  We want the bottom of the trigger placed at the top of the trigger.
            bodyPos.Y = (this.Height + this.TriggerHeight) * 0.5f;

            //  Modify it for the objects rotation
            bodyPos = SpinAssist.ModifyVectorByOrientation(bodyPos, _orientation);

            //  Modify the offset for later calculation
            Vector2 newOffset = SpinAssist.ModifyVectorByOrientation(_triggerOffset, _orientation);

            if (convert)
            {
                return ConvertUnits.ToSimUnits(this._position - bodyPos + newOffset);
            }
            else
            {
                return this._position - bodyPos + newOffset;
            }
        }

#if !EDITOR

        #region Collision Events

        protected override void Body_OnSeparation(Fixture fixtureA, Fixture fixtureB)
        {
            _touchingFixtures.Remove(fixtureB);
        }

        protected override bool Body_OnCollision(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            //  We want to add all the bodies that come into contact with it.
            if (!_touchingFixtures.Contains(fixtureB))
            {
                //  We don't want to readd Harland if he's dead.
                if (Player.Instance.CheckBodyBox(fixtureB))
                {
                    return true;
                }

                _touchingFixtures.Add(fixtureB);
            }

            return true;
        }

#endregion

        protected override void SetupPhysics(World world)
        {
            this.Body = BodyFactory.CreateRectangle(world, ConvertUnits.ToSimUnits(_triggerWidth), ConvertUnits.ToSimUnits(_triggerHeight), 1.0f); 
            this.Body.Position = GetTriggerPosition(true);
            this.Body.IsSensor = true;

            this.Body.Rotation = _rotation;
            this.Body.OnCollision += Body_OnCollision;
            this.Body.OnSeparation += Body_OnSeparation;
        }

        #region Apply the force
        /// <summary>
        /// Apply the force to the player.
        /// </summary>
        private void ApplyForces()
        {
            Player playerInstance = Player.Instance;

            //  What direction the player should be pushed. In this case, Up before modify.
            Vector2 dir = SpinAssist.ModifyVectorByOrientation(new Vector2(0, -100), _orientation);
            
            //  Normalize the direction...
            dir.Normalize();

            //  Then apply a speed.
            dir *= 8;

            //  Check all the fixtures that are touching the trigger and apply a force to them.
            for (int i = _touchingFixtures.Count - 1; i >= 0; i--)
            {
                //  We have a special way to push the player, so check if 'i' is a player fixture.
                if (playerInstance.CheckHitBoxFixture(_touchingFixtures[i]))
                {
                    //  If Harland is dead, remove him and continue.
                    if (playerInstance.PlayerState == PlayerState.Dead)
                    {
                        this._touchingFixtures.RemoveAt(i);
                        continue;
                    }

                    //  It is the player, so apply the force
                    playerInstance.ApplyForce(dir, 10.0f);
                    continue;
                }

                //  It's a random object, so apply a force to it too.
                this._touchingFixtures[i].Body.ApplyForce(dir);
            }
        }
#endregion

        #region Add + Randomize sprite
        /// <summary>
        /// Add and randomize a steam sprite that is expelled from the platform.
        /// </summary>
        private void AddSprite()
        {
            //  Get a random offset for the x acceleration to differentiate the numerous clouds
            float xVelo = SpinAssist.GetRandom(-2.0f, 2.0f);

            //  Create a new velocity and rotation for this sprite to fire.
            this._exhaustSprite.Velocity = SpinAssist.ModifyVectorByOrientation(new Vector2(xVelo, -20), _orientation);
            this._exhaustSprite.Rotation = SpinAssist.GetRandom(0, MathHelper.TwoPi);

            //  Clone the editted base steam sprite to expell
            Sprite newSprite = (Sprite)_exhaustSprite.Clone();

            SpriteManager.Instance.AddSprite(newSprite);

            //  How long should it wait before expelling another steam sprite.
            _createDelay = 0.07f;
        }
        #endregion


#endif
        #endregion

        #if !EDITOR
        public override void Start()
        {
            this._triggered = true;

            if (_soundEffectAsset != "")
            {
                _soundEffect = AudioManager.Instance.PlayCue(_soundEffectAsset, true);
            }
        }

        public override void Stop()
        {
            this._triggered = false;

            if (_soundEffect != null && !_soundEffect.IsStopped)
            {
                _soundEffect.Stop(AudioStopOptions.AsAuthored);
            }
        }

        public override void Toggle()
        {
            if (_triggered)
            {
                this.Stop();
            }
            else
            {
                this.Start();
            }
        }
#endif
    }
}
