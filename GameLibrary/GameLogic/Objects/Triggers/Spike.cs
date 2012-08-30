using System;
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
using GameLibrary.Audio;

namespace GameLibrary.GameLogic.Objects.Triggers
{
    public class Spike : StaticObject
    {
        public Spike()
            : base()
        {

        }

        #region Private Methods

#if !EDITOR

        protected override void SetupPhysics(World world)
        {

            this.Body = BodyFactory.CreateRoundedRectangle(world, ConvertUnits.ToSimUnits(_width - 12), ConvertUnits.ToSimUnits(_height * 0.75f), ConvertUnits.ToSimUnits(5), ConvertUnits.ToSimUnits(5), 3, ConvertUnits.ToSimUnits(_mass));
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
            Player playerInstance = Player.Instance;

            if (!_touchingFixtures.Contains(fixtureB) && playerInstance.CheckHitBoxFixture(fixtureB))
            {
                _touchingFixtures.Add(fixtureB);
                playerInstance.Kill();
                AudioManager.Instance.PlayCue("Death_Spikes", true);
            }

            return true;
        }

        protected override void Body_OnSeparation(Fixture fixtureA, Fixture fixtureB)
        {
            if (_touchingFixtures.Contains(fixtureB))
            {
                _touchingFixtures.Remove(fixtureB);
            }
        }
        #endregion

#endif

        #endregion
    }
}
