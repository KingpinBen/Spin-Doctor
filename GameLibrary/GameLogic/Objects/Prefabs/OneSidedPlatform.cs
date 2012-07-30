//--------------------------------------------------------------------------------
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

#if EDITOR || Development
        protected Texture2D displayTexture;
#else
        private float top;
        private float radius;
        private Fixture _platform;
        private World _world;
#endif
        #endregion

        public OneSidedPlatform() : base() { }

        public override void Load(ContentManager content, World world)
        {
#if Development
            displayTexture = content.Load<Texture2D>(FileLoc.DevTexture());
#endif
#if EDITOR
            
            displayTexture = content.Load<Texture2D>(FileLoc.DevTexture());

            if (Width == 0 || Height == 0)
            {
                Width = displayTexture.Width;
                Height = displayTexture.Height;
            }


#else
            SetupPhysics(world);
#endif
        }

        #region Draw
#if EDITOR
        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(displayTexture, this._position, new Rectangle(0, 0, (int)this._width, (int)this._height), 
                Color.White * 0.5f, this.TextureRotation, new Vector2(this._width, this._height) * 0.5f, 1.0f, SpriteEffects.None, 0.2f);
        }

#else
        public override void Draw(SpriteBatch spriteBatch, GraphicsDevice graphics)
        {
#if Development
            spriteBatch.DrawString(Fonts.DebugFont, "PosY: " + Player.Instance.WheelBody.Position.Y, this.Position + new Vector2(0, -100), Color.Red, 0.0f, Vector2.Zero, 0.5f, SpriteEffects.None, 0.0f);
            spriteBatch.DrawString(Fonts.DebugFont, "TopY: " + ConvertUnits.ToDisplayUnits(this.Body.Position.Y - top), this.Position + new Vector2(0, -85), Color.Red, 0.0f, Vector2.Zero, 0.5f, SpriteEffects.None, 0.0f);
            spriteBatch.DrawString(Fonts.DebugFont, top + " " + radius, this.Position + new Vector2(0, -70), Color.Red, 0.0f, Vector2.Zero, 0.5f, SpriteEffects.None, 0.0f);

            spriteBatch.Draw(displayTexture, this._position, new Rectangle(0, 0, (int)this._width, (int)this._height),
                Color.White * 0.7f, this.TextureRotation, new Vector2(this._width, this._height) * 0.5f, 1.0f, SpriteEffects.None, 0.0f);
#endif
            //base.Draw(spriteBatch);
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
                //  We only want a contact with the player
                //  As it's not the player, disable the contact
                contact.Enabled = false;

                if (fixtureB == Player.Instance.WheelBody.FixtureList[0])
                {
                    //  If the centre of the player wheel is above (depending on the orientation 
                    //  and the world rotation) to the top point (1/2 w/h depending on ^) and the
                    //  radius of the wheel + a small bounding number.

                    switch (_orientation)
                    {
                        case Orientation.Up:
                            if (Player.Instance.WheelBody.Position.Y < top - radius * Settings.LinearSlop &&
                                Camera.Instance.UpIs == UpIs.Up)
                            {
                                contact.Enabled = true;
                            }
                            break;
                        case Orientation.Down:
                            if (Player.Instance.WheelBody.Position.Y > top - radius * Settings.LinearSlop &&
                                Camera.Instance.UpIs == UpIs.Down)
                            {
                                contact.Enabled = true;
                            }
                            break;
                        case Orientation.Left:
                            if (Player.Instance.WheelBody.Position.X > top - radius * Settings.LinearSlop &&
                                Camera.Instance.UpIs == UpIs.Left)
                            {
                                contact.Enabled = true;
                            }
                            break;
                        case Orientation.Right:
                            if (Player.Instance.WheelBody.Position.X < top + radius * Settings.LinearSlop &&
                                Camera.Instance.UpIs == UpIs.Right)
                            {
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
            this._world = world;
            this.Body = BodyFactory.CreateBody(world);
            this.Body.Position = ConvertUnits.ToSimUnits(this.Position);
            this.Body.CollidesWith = Category.Cat10;
            
            Fixture fixture = FixtureFactory.AttachRectangle(ConvertUnits.ToSimUnits(_width), ConvertUnits.ToSimUnits(_height), 1.0f, Vector2.Zero, this.Body);
            this.Body.Mass = ConvertUnits.ToSimUnits(10000);
            this.Body.Rotation = _rotation;

            
            //  The Player wheel radius.
            radius = ConvertUnits.ToSimUnits(28);
            switch (_orientation)
            {
                case Orientation.Up:
                    top = ConvertUnits.ToSimUnits(this._position.Y - (_height * 0.5f));
                    break;
                case Orientation.Down:
                    top = ConvertUnits.ToSimUnits(this._position.Y + (_height * 0.5f));
                    break;
                case Orientation.Left:
                    top = ConvertUnits.ToSimUnits(this._position.X - (_width * 0.5f));
                    break;
                case Orientation.Right:
                    top = ConvertUnits.ToSimUnits(this._position.X + (_width * 0.5f));
                    break;
            }

            _world.ContactManager.PreSolve += this.PreSolve;

            //  Apply it after attaching the fixture so it applies to all fixtures
            //  without having to redo it to newer ones later.
            this.Body.Friction = 2.0f;
#endif
        }

        #endregion
    }
}
