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
//--    TBD
//--    ==============
//--    Make it work with different orientations.
//--    Fix the y detection. make sure it uses the radius of wheel.
//--    
//-------------------------------------------------------------------------------

#define Development

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
using GameLibrary.Assists;
using Microsoft.Xna.Framework.Graphics;
using GameLibrary.Drawing;
#endregion

namespace GameLibrary.Objects
{
    public class OneSidedPlatform : StaticObject
    {
        #region Fields
        [ContentSerializerIgnore]
        private float top, radius;
        [ContentSerializerIgnore]
        private Fixture _platform;

#if EDITOR || Development
        protected Texture2D displayTexture;
#endif 
        #endregion

        #region Constructor
        public OneSidedPlatform()
            : base()
        {

        }

        public override void Init(Vector2 position)
        {
            base.Init(position);
        }
        #endregion

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

        public override void Update(GameTime gameTime)
        {
            //base.Update(gameTime);
        }

        #region Draw
#if EDITOR
        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(displayTexture, this._position, new Rectangle(0, 0, (int)this._width, (int)this._height), 
                Color.White * 0.5f, this.TextureRotation, new Vector2(this._width / 2, this._height / 2), 1.0f, SpriteEffects.None, 0.2f);
        }

#else
        public override void Draw(SpriteBatch spriteBatch)
        {
#if Development
            spriteBatch.DrawString(Fonts.DebugFont, "PosY: " + Player.Instance.WheelBody.Position.Y, this.Position + new Vector2(0, -100), Color.Red, 0.0f, Vector2.Zero, 0.5f, SpriteEffects.None, 0.0f);
            spriteBatch.DrawString(Fonts.DebugFont, "TopY: " + ConvertUnits.ToDisplayUnits(this.Body.Position.Y - top), this.Position + new Vector2(0, -85), Color.Red, 0.0f, Vector2.Zero, 0.5f, SpriteEffects.None, 0.0f);
            spriteBatch.DrawString(Fonts.DebugFont, top + " " + radius, this.Position + new Vector2(0, -70), Color.Red, 0.0f, Vector2.Zero, 0.5f, SpriteEffects.None, 0.0f);

            spriteBatch.Draw(displayTexture, this._position, new Rectangle(0, 0, (int)this._width, (int)this._height),
                Color.White * 0.7f, this.TextureRotation, new Vector2(this._width / 2, this._height / 2), 1.0f, SpriteEffects.None, 0.0f);
#endif
            //base.Draw(spriteBatch);
        }
#endif
        #endregion

        #region Private Methods

        protected void PreSolve(Contact contact, ref Manifold oldManifold)
        {
#if EDITOR
#else
            Fixture fixtureA = contact.FixtureA;
            Fixture fixtureB = contact.FixtureB;


            if (fixtureA == this.Body.FixtureList[0])
            {
                contact.Enabled = false;

                if (fixtureB == Player.Instance.WheelBody.FixtureList[0])
                {
                    switch (_orientation)
                    {
                        case Orientation.Up:
                            if (Player.Instance.WheelBody.Position.Y < top - radius * Settings.LinearSlop &&
                                Camera.UpIs == UpIs.Up)
                            {
                                contact.Enabled = true;
                            }
                            break;
                        case Orientation.Down:
                            if (Player.Instance.WheelBody.Position.Y > top - radius * Settings.LinearSlop &&
                                Camera.UpIs == UpIs.Down)
                            {
                                contact.Enabled = true;
                            }
                            break;
                        case Orientation.Left:
                            if (Player.Instance.WheelBody.Position.X > top - radius * Settings.LinearSlop &&
                                Camera.UpIs == UpIs.Left)
                            {
                                contact.Enabled = true;
                            }
                            break;
                        case Orientation.Right:
                            if (Player.Instance.WheelBody.Position.X < top + radius * Settings.LinearSlop &&
                                Camera.UpIs == UpIs.Right)
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
            this.Body = BodyFactory.CreateBody(world);
            this.Body.Position = ConvertUnits.ToSimUnits(this.Position);

            PolygonShape shape = new PolygonShape(1);
            shape.SetAsBox(ConvertUnits.ToSimUnits(Width * 0.5f), ConvertUnits.ToSimUnits(Height * 0.5f));
            _platform = this.Body.CreateFixture(shape);

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
            

            world.ContactManager.PreSolve += PreSolve;

            //  Apply it after attaching the fixture so it applies to all fixtures
            //  without having to redo it to newer ones later.
            this.Body.Friction = 2.0f;
#endif
        }

        #endregion
    }
}
