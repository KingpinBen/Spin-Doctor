using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using GameLibrary.Assists;
using GameLibrary.Managers;

namespace GameLibrary.Objects.Triggers
{
    public class ChangeLevelZone : Door
    {
        public ChangeLevelZone() : base() { }

        public virtual void Init(Vector2 position, float width, float height, int nextLevelid)
        {
            this._position = position;
            this._width = width;
            this._height = height;
            this.nextLevel = nextLevelid;
        }

        public override void Load(ContentManager content, World world)
        {
            this.Body = BodyFactory.CreateRectangle(world, ConvertUnits.ToSimUnits(this.Width), ConvertUnits.ToSimUnits(this.Height), 1.0f);
            this.Body.Position = ConvertUnits.ToSimUnits(Position);
            this.Body.BodyType = BodyType.Static;
            this.Body.IsSensor = true;
            this.Body.CollidesWith = Category.Cat10;
            this.Body.OnCollision += Body_OnCollision;
            this.Body.OnSeparation += Body_OnSeparation;

            this.Triggered = false;
            this.ShowHelp = false;
        }

        public override void Update(GameTime gameTime)
        {
            if (Triggered)
                Screen_Manager.LoadLevel(nextLevel);
        }

        public override void Draw(SpriteBatch sb)
        {
        }
    }
}
