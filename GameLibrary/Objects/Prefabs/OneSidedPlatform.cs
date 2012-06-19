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
        private Texture2D displayTexture;
#endif 
        #endregion

        #region Constructor
        public OneSidedPlatform()
            : base()
        {

        }
        #endregion

        public override void Load(ContentManager content, World world)
        {
            #region Editor & Dev
#if EDITOR
            displayTexture = content.Load<Texture2D>(FileLoc.DevTexture());
            return;
#endif

#if Development
            displayTexture = content.Load<Texture2D>(FileLoc.DevTexture());
#endif
            #endregion

            this.Body = BodyFactory.CreateBody(world);
            this.Body.Position = ConvertUnits.ToSimUnits(this.Position);
            

            PolygonShape shape = new PolygonShape(1);
            shape.SetAsBox(ConvertUnits.ToSimUnits(Width), ConvertUnits.ToSimUnits(Height));
            _platform = this.Body.CreateFixture(shape);

            radius = ConvertUnits.ToSimUnits(28);
            top = this.Body.Position.Y + ConvertUnits.ToSimUnits(Height / 2);

            world.ContactManager.PreSolve += PreSolve; 

            //  Apply it after attaching the fixture so it applies to all fixtures
            //  without having to redo it to newer ones later.
            this.Body.Friction = 3.0f;
        }

        public override void Update(GameTime gameTime)
        {
            //base.Update(gameTime);
        }

        #region Draw
#if EDITOR
        public override void Draw(SpriteBatch spriteBatch)
        {
            //  TODO: Make this work with editor.
        }

#else
        public override void Draw(SpriteBatch spriteBatch)
        {
#if Development
            spriteBatch.DrawString(Fonts.DebugFont, "PosY: " + Player.Instance.WheelBody.Position.Y, this.Position + new Vector2(0, -80), Color.Red);
            spriteBatch.DrawString(Fonts.DebugFont, "Calc: " + (top + radius - 3.0f * Settings.LinearSlop), this.Position + new Vector2(0, -65), Color.Red);
            spriteBatch.DrawString(Fonts.DebugFont, top + " " + radius, this.Position + new Vector2(0, -35), Color.Red);

            spriteBatch.Draw(displayTexture, ConvertUnits.ToDisplayUnits(this.Body.Position) - new Vector2(this.Width / 2, this.Height/2), new Rectangle(0, 0, (int)this.Width * 2, (int)this.Height * 2),
                Color.White * 0.7f, 0.0f, new Vector2(this.Width / 2, this.Height / 2), 1.0f, SpriteEffects.None, 0.0f);
#endif
            //base.Draw(spriteBatch);
        }
#endif
        #endregion

        #region PreSolve Collisions
        protected void PreSolve(Contact contact, ref Manifold oldManifold)
        {
            Fixture fixtureA = contact.FixtureA;
            Fixture fixtureB = contact.FixtureB;

            if (fixtureA == this.Body.FixtureList[0] || fixtureB == this.Body.FixtureList[0])
            {
                if (fixtureB != Player.Instance.WheelBody.FixtureList[0] || fixtureA != Player.Instance.WheelBody.FixtureList[0])
                {
                    Vector2 position = Player.Instance.WheelBody.Position;

                    if (position.Y > top - radius - 3.0f * Settings.LinearSlop)
                    {
                        contact.Enabled = false;
                    }
                }
            }
        }
        #endregion
    }
}
