﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework.Graphics;
using FarseerPhysics.Dynamics.Contacts;
using FarseerPhysics.Factories;
using GameLibrary.Helpers;
using GameLibrary.GameLogic.Characters;

namespace GameLibrary.GameLogic.Objects.Triggers
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
            

#if EDITOR
            if (_width == 0 || _height == 0)
            {
                this.Width = this._texture.Width;
                this.Height = this._texture.Height;
            }
#else
            this.SetupPhysics(world);
#endif

            this._origin = new Vector2(_width, _height) * 0.5f;
        }

        #region Draw
#if EDITOR
        public override void Draw(SpriteBatch sb)
        {
            sb.Draw(_texture, this._position, new Rectangle(0, 0, (int)_width, (int)_height),
                this._tint, this._rotation, new Vector2(this._width, this._height) * 0.5f, 1.0f, SpriteEffects.None, this._zLayer);
        }
#else
        public override void Draw(SpriteBatch sb, GraphicsDevice graphics)
        {
            sb.Draw(this._texture, new Rectangle((int)(Position.X), (int)(Position.Y), (int)_width, (int)_height),
                new Rectangle(0, 0, (int)_width, (int)_height), this._tint, this.Body.Rotation, this._origin, SpriteEffects.None, this._zLayer);
        }
#endif
        #endregion

        #region Private Methods
#if !EDITOR
        protected override void SetupPhysics(World world)
        {

            this.Body = BodyFactory.CreateRoundedRectangle(world, ConvertUnits.ToSimUnits(_width), ConvertUnits.ToSimUnits(_height), ConvertUnits.ToSimUnits(5), ConvertUnits.ToSimUnits(5), 3, ConvertUnits.ToSimUnits(_mass));
            this.Body.BodyType = BodyType.Static;
            this.Body.Position = ConvertUnits.ToSimUnits(this.Position);
            this.Body.Rotation = _rotation;
            this.Body.IsSensor = true;
            this.Body.OnCollision += Body_OnCollision;
            this.Body.OnSeparation += Body_OnSeparation;

            this.Body.CollisionCategories = Category.Cat20;
            this.Body.CollisionCategories = Category.All & ~Category.Cat20;

        }

        #region Collisions
        protected override bool Body_OnCollision(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            if (!TouchingFixtures.Contains(fixtureB) && fixtureB == Player.Instance.PlayerHitBox)
            {
                TouchingFixtures.Add(fixtureB);
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
        #endregion
#endif
        #endregion
    }
}
