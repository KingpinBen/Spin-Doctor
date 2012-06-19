using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework.Graphics;
using GameLibrary.Assists;
using FarseerPhysics.Dynamics.Contacts;

namespace GameLibrary.Objects.Triggers
{
    public class Spike : Trigger
    {
        public Spike()
            : base()
        {

        }

        public override void Init(Vector2 position, float width, float height, string tex)
        {
            base.Init(position, width, height, tex);
            this.ShowHelp = false;
            this.Message = "NOT USED.";
        }

        public override void Load(ContentManager content, World world)
        {
            this.Texture = content.Load<Texture2D>(_textureAsset);

#if EDITOR
            return;
#endif
            this.SetUpTrigger(world);
        }

        public override void Draw(SpriteBatch sb)
        {
            sb.Draw(this.Texture, ConvertUnits.ToDisplayUnits(this.Body.Position),
                null, Tint, this.Body.Rotation, Origin, 1.0f, SpriteEffects.None, zLayer);
        }

        protected override void SetUpTrigger(World world)
        {
            TexVertOutput input = SpinAssist.TexToVert(world, this.Texture, ConvertUnits.ToSimUnits(this._mass));

            this.Origin = input.Origin;

            this.Body = input.Body;
            this.Body.BodyType = BodyType.Static;
            this.Body.Position = ConvertUnits.ToSimUnits(this.Position);
            this.Body.IsSensor = true;
            this.Body.Rotation = SpinAssist.RotationByOrientation(_orientation);

            this.Body.OnCollision += Body_OnCollision;
            this.Body.OnSeparation += Body_OnSeparation;
        }

        protected override bool Body_OnCollision(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            if (!TouchingFixtures.Contains(fixtureB))
            {
                TouchingFixtures.Add(fixtureB);
            }

            if (fixtureB == Player.Instance.Body.FixtureList[0] || fixtureB == Player.Instance.WheelBody.FixtureList[0])
            {
                Player.Instance.Kill();
            }

            return true;
        }

        protected override void Body_OnSeparation(Fixture fixtureA, Fixture fixtureB)
        {
            if (TouchingFixtures.Contains(fixtureB))
            {
                TouchingFixtures.Remove(fixtureB);
            }
        }
    }
}
