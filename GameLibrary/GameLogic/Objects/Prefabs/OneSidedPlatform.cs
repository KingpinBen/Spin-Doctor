﻿//--------------------------------------------------------------------------------
//--    
//--    Spin Doctor - 1-way platform
//--    
//--    Description
//--    ===============
//--    Only has 1 solid plane to climb on. Can walk through etc.
//--
//--    
//--    Revision List
//--    ===============
//--    BenG - Initial
//--    BenG - Added texture so the editor/developmentmode can properly see it 
//--           without the need for farseer debug.
//--    
//--    
//-------------------------------------------------------------------------------

//#define Development

#region Using Statements
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Contacts;
using FarseerPhysics.Collision;
using Microsoft.Xna.Framework;
using FarseerPhysics;
using FarseerPhysics.Factories;
using FarseerPhysics.Collision.Shapes;
using Microsoft.Xna.Framework.Graphics;
using GameLibrary.Helpers;
using GameLibrary.Graphics.Camera;
using GameLibrary.GameLogic.Characters;
#endregion

namespace GameLibrary.GameLogic.Objects
{
    /// <summary>
    /// Notes:
    /// Radius is the radius of the Players wheel.
    /// Top is calculated in SetupPhysics
    /// 
    /// Enable/Disable are the only triggers that'll change
    /// how this object behaves
    /// </summary>
    public class OneSidedPlatform : StaticObject
    {
        #region Fields

        [ContentSerializer(Optional = true)]
        protected bool _orientationDependant = false;

#if EDITOR

#else
        private float top;
        private float radius;
        private World _world;
#endif
        #endregion

        #region Properties

#if EDITOR
        [ContentSerializerIgnore]
        public bool OrientationDependant
        {
            get
            {
                return _orientationDependant;
            }
            set
            {
                _orientationDependant = value;
            }
        }
#endif

        #endregion

        public OneSidedPlatform() : base() { }

        public override void Load(ContentManager content, World world)
        {
#if EDITOR
            this._texture = content.Load<Texture2D>(Defines.DEVELOPMENT_TEXTURE);

            if (Width == 0 || Height == 0)
            {
                Width = _texture.Width;
                Height = _texture.Height;
            }


#else
            this.SetupPhysics(world);
#endif
        }

        #region Draw
#if !EDITOR
        public override void Draw(SpriteBatch spriteBatch, GraphicsDevice graphics)
        {
#if Development
            spriteBatch.Draw(_texture, this._position, new Rectangle(0, 0, (int)this._width, (int)this._height),
                Color.White * 0.5f, this.TextureRotation, new Vector2(_width, _height) * 0.5f, 1.0f, SpriteEffects.None, 0.0f);
#endif
        }
#endif
        #endregion

        #region Private Methods

        /// <summary>
        /// Change the presolve for the world so that it can understand when a collision
        /// should occur ahead of time for one sided platforms.
        /// </summary>
        /// <param name="contact"></param>
        /// <param name="oldManifold"></param>
        protected void PreSolve(Contact contact, ref Manifold oldManifold)
        {
#if EDITOR

#else
            //  Get both collision bodies.
            Fixture fixtureA = contact.FixtureA;    //  A = Surface
            Fixture fixtureB = contact.FixtureB;    //  B = Player

            if (fixtureA == this.Body.FixtureList[0] || fixtureB == this.Body.FixtureList[0])
            {
                Player instance = Player.Instance;

                //  We only want a contact with the player
                //  As it's not the player, disable the contact
                contact.Enabled = false;

                if (instance.PlayerState == PlayerState.Climbing)
                {
                    return;
                }

                if (instance.CheckWheelFixture(fixtureB))
                {
                    UpIs upIs = Camera.Instance.GetUpIs();

                    //  If the centre of the player wheel is above (depending on the orientation 
                    //  and the world rotation) to the top point (1/2 w/h depending on ^) and the
                    //  radius of the wheel + a small bounding number.
                    switch (_orientation)
                    {
                        case Orientation.Up:
                            if (instance.GetWheelBody().Position.Y < top - radius * Settings.LinearSlop)
                            {
                                if (_orientationDependant)
                                {
                                    if (upIs == UpIs.Up)
                                    {
                                        contact.Enabled = true;
                                    }
                                }
                                else
                                {
                                    contact.Enabled = true;
                                }
                            }
                            break;
                        case Orientation.Down:
                            if (instance.GetWheelBody().Position.Y > top - radius * Settings.LinearSlop)
                            {
                                if (_orientationDependant)
                                {
                                    if (upIs == UpIs.Down)
                                    {
                                        contact.Enabled = true;
                                    }
                                }
                                else
                                {
                                    contact.Enabled = true;
                                }

                                contact.Enabled = true;
                            }
                            break;
                        case Orientation.Left:
                            if (instance.GetWheelBody().Position.X > top - radius * Settings.LinearSlop)
                            {
                                if (_orientationDependant)
                                {
                                    if (upIs == UpIs.Left)
                                    {
                                        contact.Enabled = true;
                                    }
                                }
                                else
                                {
                                    contact.Enabled = true;
                                }

                                contact.Enabled = true;
                            }
                            break;
                        case Orientation.Right:
                            if (instance.GetWheelBody().Position.X < top + radius * Settings.LinearSlop)
                            {
                                if (_orientationDependant)
                                {
                                    if (upIs == UpIs.Right)
                                    {
                                        contact.Enabled = true;
                                    }
                                }
                                else
                                {
                                    contact.Enabled = true;
                                }

                                contact.Enabled = true;
                            }
                            break;
                    }
                }
            }
#endif
        }

        protected override void SetupPhysics(World world)
        {
#if EDITOR
#else
            //  Try and only convert everything once.
            Vector2 simPosition = ConvertUnits.ToSimUnits(this._position);
            float simHeight = ConvertUnits.ToSimUnits(_height);
            float simWidth = ConvertUnits.ToSimUnits(_width);

            //  Assign the world so we can easily access/replace the presolver.
            this._world = world;
            this._world.ContactManager.PreSolve += this.PreSolve;

            //  Set up the objects body.
            this.Body = BodyFactory.CreateBody(world);
            this.Body.Position = simPosition;
            this.Body.CollidesWith = Category.Cat10;
            
            Fixture fixture = FixtureFactory.AttachRectangle(simWidth, simHeight, 1.0f, Vector2.Zero, this.Body);
            this.Body.Mass = ConvertUnits.ToSimUnits(10000);
            this.Body.Rotation = _rotation;
            this.Body.Friction = 3.0f;
            this.Body.UserData = MaterialType.None;
            
            //  The Player wheel radius.
            radius = ConvertUnits.ToSimUnits(28);

            //  Find the 'top'(side) based on the orientation.
            switch (_orientation)
            {
                case Orientation.Up:
                    top = simPosition.Y - (simHeight * 0.5f);
                    //top = ConvertUnits.ToSimUnits(this._position.Y - (_height * 0.5f))
                    break;
                case Orientation.Down:
                    top = simPosition.Y + (simHeight * 0.5f);
                    //top = ConvertUnits.ToSimUnits(this._position.Y + (_height * 0.5f))
                    break;
                case Orientation.Left:
                    top = simPosition.X - (simWidth * 0.5f);
                    //top = ConvertUnits.ToSimUnits(this._position.X - (_width * 0.5f))
                    break;
                case Orientation.Right:
                    top = simPosition.X + (simWidth * 0.5f);
                    //top = ConvertUnits.ToSimUnits(this._position.X + (_width * 0.5f))
                    break;
            }

            
#endif
        }

        #endregion
    }
}
