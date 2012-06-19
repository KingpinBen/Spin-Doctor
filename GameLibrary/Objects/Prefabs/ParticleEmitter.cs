using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameLibrary.Drawing;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace GameLibrary.Objects
{
    public static class ParticleEmitter
    {
        /// <summary>
        /// The particles that it will copy.
        /// </summary>
        private static List<Particle> baseParticles;
        public static List<string> particleTextureToUse;
        private static List<Texture2D> particleTextures;
        private static List<Particle> listOfParticles;

        public static void Load(ContentManager Content)
        {
            for (int i = 0; i < particleTextureToUse.Count; i++)
            {
                particleTextures.Add(Content.Load<Texture2D>(particleTextureToUse[i]));
            }
        }

        public static void Draw(SpriteBatch sb)
        {
            for (int i = listOfParticles.Count; i >= 0; i--)
            {

            }
        }
    }
}
