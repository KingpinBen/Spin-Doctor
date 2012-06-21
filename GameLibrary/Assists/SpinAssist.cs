//--------------------------------------------------------------------------------
//--    
//--    Spin Doctor - SpinAssist
//--
//--    Description
//--    ===============
//--    Just allows some useful features throughout the program.
//--
//--    
//--    Revision List
//--    ===============
//--    BenG - Initial
//--    BenG - Added an easy way to get a random number. Statics are better for realiability
//--           and less coding in other functions.
//--    BenG - TextureToVert added. Bodies can now be generated to match the shape of a texture.
//--    BenG - Fixed some issues with TexToVert.
//--    BenG - Added some orientation code. One to work with camera orientation and the other
//--           for object orientation position/rotation fixes.
//--    BenG - Added some rotation returns for easy orientation usage.
//--    BenG - Finally properly fixed TexToVert. Had a slight positioning deviation. Also added
//--           an output struct (below) to grab multiple bits of necessary data for body creation.
//--    
//-------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FarseerPhysics.Common;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using FarseerPhysics.Common.PolygonManipulation;
using FarseerPhysics.Common.Decomposition;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using GameLibrary.Drawing;
using GameLibrary.Objects;
using FarseerPhysics.Dynamics.Joints;
using GameLibrary.Screens;
using System.Runtime.Serialization;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace GameLibrary.Assists
{
    public static class SpinAssist
    {
        private static Random staticRandom = new Random();

        /// <summary>
        /// Pass in a texture to get the vertices for body collision.
        /// </summary>
        /// <param name="world">World to add the body to</param>
        /// <param name="texture">Texture to get vertices from</param>
        /// <param name="mass">Desired mass for the body (SHOULD BE CONVERTED WHEN PASSED IN)</param>
        /// <returns>the Body</returns>
        public static TexVertOutput TexToVert(World world, Texture2D texture, float mass)
        {
            return TexToVert(world, texture, mass, 1.0f);
        }

        public static TexVertOutput TexToVert(World world, Texture2D texture, float mass, float scale)
        {
            Vertices verts;
            TexVertOutput output;
            //  Creates an array for every pixel in the texture
            uint[] data = new uint[texture.Width * texture.Height];

            texture.GetData(data);
            
            verts = PolygonTools.CreatePolygon(data, texture.Width, false);

            //  Origin needs to be altered so it uses the origin of the verts
            //  rather than the texture's centre.
            Vector2 centroid = -verts.GetCentroid();
            verts.Translate(ref centroid);

            Vector2 Scale = new Vector2(ConvertUnits.ToSimUnits(scale), ConvertUnits.ToSimUnits(scale));
            verts.Scale(ref Scale);

            //verts = SimplifyTools.ReduceByDistance(verts, f);

            Body body = BodyFactory.CreateCompoundPolygon(world, BayazitDecomposer.ConvexPartition(verts), mass);
            body.BodyType = BodyType.Dynamic;

            output.Body = body;
            output.Origin = -centroid;

            return output;
        }

        /// <summary>
        /// Convert a vector to be affected with the altered rotation.
        /// 
        /// e.g. moving 10 on x with a 90deg rotation covnerts to -10y
        /// but will still move left.
        /// </summary>
        /// <param name="inVector">Vector to convert</param>
        /// <returns>Vector for use with gravity</returns>
        public static Vector2 ModifyVectorByUp(Vector2 inVector)
        {
            Vector2 tempVec = Vector2.Zero;

            switch (Camera.UpIs)
            {
                case upIs.Up:
                    tempVec = new Vector2(inVector.X, inVector.Y);
                    break;
                case upIs.Down:
                    tempVec = new Vector2(-inVector.X, -inVector.Y);
                    break;
                case upIs.Left:
                    tempVec = new Vector2(-inVector.Y, -inVector.X);
                    break;
                case upIs.Right:
                    tempVec = new Vector2(inVector.Y, inVector.X);
                    break;
            }

            return tempVec;
        }

        public static Vector2 ModifyVectorByOrientation(Vector2 inVector, Orientation orientation)
        {
            Vector2 vector = Vector2.Zero;

            switch (orientation)
            {
                case Orientation.Up:
                    vector = new Vector2(inVector.X, inVector.Y);
                    break;
                case Orientation.Down:
                    vector = new Vector2(inVector.X, -inVector.Y);
                    break;
                case Orientation.Left:
                    vector = new Vector2(-inVector.Y, -inVector.X);
                    break;
                case Orientation.Right:
                    vector = new Vector2(inVector.Y, inVector.X);
                    break;
            }

            return vector;
        }

        /// <summary>
        /// Assuming Up is 0 RADS
        /// </summary>
        /// <param name="orientation"></param>
        /// <returns></returns>
        public static float RotationByOrientation(Orientation orientation)
        {
            switch (orientation)
            {
                case Orientation.Up:
                    return 0.0f;
                case Orientation.Right:
                    return MathHelper.ToRadians(270);
                case Orientation.Left: 
                    return MathHelper.ToRadians(90);
                case Orientation.Down:
                    return MathHelper.ToRadians(180);
            }
            //  Shouldnt ever return this.
            return 0.0f;
        }

        public static int GetRandom(int minValue, int maxValue)
        {
            return staticRandom.Next(minValue, maxValue);
        }

    }
}
