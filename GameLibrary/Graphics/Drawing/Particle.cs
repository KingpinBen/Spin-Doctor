using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace GameLibrary.Graphics
{
    public class Particle : ICloneable
    {
        #region Fields and Properties
        public Vector2 Position;
        public Vector2 Acceleration;
        public Vector2 Velocity;
        public bool IsAlive;

        private float _timeLeft;
        public float Rotation;
        
        #endregion

        public Particle()
        {
            this.IsAlive = false;
        }

        public void Init(Vector2 position, Vector2 velocity, Vector2 acceleration, float life)
        {
            this.Position = position;
            this.Velocity = velocity;
            this.Acceleration = acceleration;
            this.IsAlive = true;
            this._timeLeft = life;
        }

        public void Update(float delta)
        {
            Velocity += Acceleration * delta;
            Position += Velocity * delta;

            Rotation = (float)Math.Atan2(Velocity.Y, Velocity.X) + 1.5705f;//3.141f;

            _timeLeft -= delta;

            if (_timeLeft <= 0)
            {
                this.IsAlive = false;
            }
        }

        object ICloneable.Clone()
        {
            return this.Clone();
        }

        public Particle Clone()
        {
            return (Particle)this.MemberwiseClone();
        }
    }
}
