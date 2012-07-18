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

        private float _timeLeft;
        private bool _isAlive;
        public bool Alive
        {
            get
            {
                return _isAlive;
            }
        }
        #endregion

        public Particle()
        {
            this._isAlive = false;
        }

        public void Init(Vector2 position, Vector2 velocity, Vector2 acceleration, float life)
        {
            this.Position = position;
            this.Velocity = velocity;
            this.Acceleration = acceleration;
            this._isAlive = true;
            this._timeLeft = life;
        }

        public void Update(float delta)
        {
            Velocity += Acceleration * delta;
            Position += Velocity * delta;

            _timeLeft -= delta;

            if (_timeLeft <= 0)
            {
                this._isAlive = false;
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
