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
using FarseerPhysics.Factories;

namespace GameLibrary.Objects.Triggers
{
    public class Spike : StaticObject
    {
        [ContentSerializerIgnore]
        private List<Fixture> TouchingFixtures = new List<Fixture>();

        public Spike()
            : base()
        {

        }

        public override void Init(Vector2 position,  string tex)
        {
            base.Init(position, tex);
        }

        public override void Load(ContentManager content, World world)
        {
            this._texture = content.Load<Texture2D>(_textureAsset);
            this._origin = new Vector2(_texture.Width / 2, _texture.Height);

#if EDITOR
            if (_width == 0 || _height == 0)
            {
                this.Width = this._texture.Width;
                this.Height = this._texture.Height;
            }
#else
            this.SetUpPhysics(world);
            this.GetRotationFromOrientation();
#endif
        }

        #region Draw
#if EDITOR
        public override void Draw(SpriteBatch sb)
        {
            sb.Draw(_texture, new Rectangle((int)_position.X, (int)_position.Y, (int)_width,(int)_height), new Rectangle(0, 0, (int)_width, (int)_height),
                Color.White, this.TextureRotation, new Vector2(_width / 2, _height / 2), SpriteEffects.None, this.zLayer);
        }
#else
        public override void Draw(SpriteBatch sb)
        {
            sb.Draw(this._texture, new Rectangle((int)(Position.X), (int)(Position.Y), (int)_width, (int)_height),
                new Rectangle(0,0,(int)_width, (int)_height), this.Tint, this.TextureRotation, new Vector2(_width/2, _height/2), SpriteEffects.None, zLayer);
        }
#endif
        #endregion

        #region Private Methods

        protected override void SetUpPhysics(World world)
        {
#if EDITOR
#else
            this.Body = BodyFactory.CreateRoundedRectangle(world, ConvertUnits.ToSimUnits(Width), ConvertUnits.ToSimUnits(Height), ConvertUnits.ToSimUnits(5), ConvertUnits.ToSimUnits(5), 3, ConvertUnits.ToSimUnits(_mass));
            this.Body.BodyType = BodyType.Static;
            this.Body.Position = ConvertUnits.ToSimUnits(this.Position);
            this.Body.IsSensor = true;
            //this.Body.Rotation = SpinAssist.RotationByOrientation(_orientation);
            this.Body.OnCollision += Body_OnCollision;
            this.Body.OnSeparation += Body_OnSeparation;
#endif
        }

        #region Collisions
        protected override bool Body_OnCollision(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
#if EDITOR
            return true;
#else
            if (!TouchingFixtures.Contains(fixtureB))
            {
                TouchingFixtures.Add(fixtureB);
            }

            if (fixtureB == Player.Instance.Body.FixtureList[0] || fixtureB == Player.Instance.WheelBody.FixtureList[0])
            {
                Player.Instance.Kill();
            }

            return true;
#endif
        }

        protected override void Body_OnSeparation(Fixture fixtureA, Fixture fixtureB)
        {
#if EDITOR

#else
            if (TouchingFixtures.Contains(fixtureB))
            {
                TouchingFixtures.Remove(fixtureB);
            }
#endif
        }
        #endregion

        #endregion
    }
}
