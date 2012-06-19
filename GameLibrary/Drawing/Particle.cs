using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace GameLibrary.Drawing
{
    public class Particle
    {
        #region Fields and Properties
        public Vector2 Position
        {
            get
            {
                return position;
            }
        }
        private Vector2 position;

        public Vector2 Velocity
        {
            get
            {
                return velocity;
            }
        }
        private Vector2 velocity;

        public float Alpha
        {
            get
            {
                return alpha;
            }
        }
        private float alpha;

        public float Scale
        {
            get
            {
                return scale;
            }
        }
        private float scale;

        public Color Tint
        {
            get
            {
                return tint;
            }
        }
        private Color tint;

        public int ParticleTextureIndex
        {
            get
            {
                return particleTextureIndex;
            }
        }
        private int particleTextureIndex;
        #endregion

        public Particle() { }

        public void Update(GameTime gameTime)
        {

        }
    }
}
