﻿//--------------------------------------------------------------------------
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
#endregion

namespace GameLibrary.Objects
{
    public class PushingPlatform : Trigger
    {
        #region Fields

        [ContentSerializer]
        private float _timeBetweenPulses;
        [ContentSerializer]
        private string _exhaustTextureAsset;
        [ContentSerializer]
        private float _timePulsing;

        [ContentSerializerIgnore]
        private float _elapsed;
        [ContentSerializerIgnore]
        private float createDelay;
        [ContentSerializerIgnore]
        private Sprite _exhaustSprite;
        #endregion

        #region Properties

        [ContentSerializerIgnore]
        public float PulseCooldown
        {
            get
            {
                return _timeBetweenPulses;
            }
#if EDITOR
            set
            {
                _timeBetweenPulses = value;
            }
#endif
        }

        [ContentSerializerIgnore]
        public string ExhaustTextureAsset
        {
            get
            {
                return _exhaustTextureAsset;
            }
#if EDITOR
            set
            {
                _exhaustTextureAsset = value;
            }
#endif
        }
        [ContentSerializerIgnore]
        public float TimePulsing
        {
            get
            {
                return _timePulsing;
            }
#if EDITOR
            set
            {
                _timePulsing = value;
            }
#endif
        }
        #endregion

        #region Constructor
        public PushingPlatform()
        {
            
        }

        public void Init(Vector2 position, float tWidth, float tHeight, string texLoc, string fxTexLoc)
        {
            this._textureAsset = texLoc;
            this._exhaustTextureAsset = fxTexLoc;
            this.ShowHelp = false;
            this._timePulsing = 1.0f;
            this._timeBetweenPulses = 3.0f;

            base.Init(position, tWidth, tHeight);
        }
        #endregion

        #region Load
        public override void Load(ContentManager content, World world)
        {
            this.Texture = content.Load
                <Texture2D>(this._textureAsset);
            this._origin = new Vector2
                (Texture.Width / 2, Texture.Height);

            this.SetUpTrigger(world);
            this.GetRotationFromOrientation();
            this.CreateSprite(content);
        }
        #endregion

        #region Update
        public override void Update(GameTime gameTime)
        {
            _elapsed += (float)gameTime.ElapsedGameTime.TotalMilliseconds * 0.001f;

            if (Triggered)
            {
                //  While it's triggered, we want to make some sprites come out of it.
                createDelay -= (float)gameTime.ElapsedGameTime.TotalMilliseconds * 0.001f;

                if (createDelay <= 0.0f)
                {
                    Sprite newSprite = _exhaustSprite.Clone();

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
        }
        #endregion

        #region Draw
        public override void Draw(SpriteBatch sb)
        {
            sb.Draw(Texture, Position, null, _tint, this.TextureRotation, Origin, 1f, SpriteEffects.None, zLayer);

#if Development
            sb.DrawString(Fonts.DebugFont, "Count: " + TouchingFixtures.Count, this.Position + new Vector2(0, 50), Color.Red);
            base.Draw(sb);
#endif
        }
        #endregion

        #region Private Methods

        #region CreateSprite
        private void CreateSprite(ContentManager Content)
        {
            _exhaustSprite = new Sprite();
            _exhaustSprite.Init(new Point(68, 64), new Point(14, 1), 1);
            _exhaustSprite.Position = this.Position;
            _exhaustSprite.Velocity = SpinAssist.ModifyVectorByOrientation(new Vector2(0, -20), _orientation);
            _exhaustSprite.Alpha = 0.6f;
            _exhaustSprite.Scale = 2.0f;
            _exhaustSprite.Rotation = this.TextureRotation;
            _exhaustSprite.Tint = Color.White;
            _exhaustSprite.TimesToPlay = 1;
            _exhaustSprite.Texture = Content.Load<Texture2D>(_exhaustTextureAsset);
            _exhaustSprite.Load();
        }
        #endregion

        #region Collisions
        protected override void Body_OnSeparation(Fixture fixtureA, Fixture fixtureB)
        {
            TouchingFixtures.Remove(fixtureB);
        }

        protected override bool Body_OnCollision(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            if (!TouchingFixtures.Contains(fixtureB))
            {
                TouchingFixtures.Add(fixtureB);
            }

            return true;
        }
        #endregion

        #region SetupTrigger
        protected override void SetUpTrigger(World world)
        {
            Vector2 bodyPos = Vector2.Zero;
            bodyPos.Y -= this.Texture.Height + (_height / 2);
            bodyPos = SpinAssist.ModifyVectorByOrientation(bodyPos, _orientation);

            this.Body = BodyFactory.CreateRectangle(world, ConvertUnits.ToSimUnits(Width), ConvertUnits.ToSimUnits(Height), 1.0f); 
            this.Body.Position = ConvertUnits.ToSimUnits(this.Position + bodyPos);
            this.Body.BodyType = BodyType.Static;
            this.Body.IsSensor = true;
            this.Body.IgnoreCollisionWith(Player.Instance.Body);
            this.Body.OnCollision += Body_OnCollision;
            this.Body.OnSeparation += Body_OnSeparation;

        }
        #endregion

        private void ApplyForces()
        {
            Vector2 dir = SpinAssist.ModifyVectorByOrientation(new Vector2(0, -100), _orientation);
            dir.Normalize();
            dir *= 15;

            for (int i = TouchingFixtures.Count - 1; i >= 0; i--)
            {
                if (TouchingFixtures[i] == Player.Instance.WheelBody.FixtureList[0])
                {
                    Player.Instance.ApplyForce(dir, 70.0f);
                    continue;
                }

                TouchingFixtures[i].Body.ApplyForce(dir);
            }

            
        }

        #endregion
    }
}
