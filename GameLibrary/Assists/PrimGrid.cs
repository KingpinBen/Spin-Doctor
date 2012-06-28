using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using GameLibrary.Managers;
using Microsoft.Xna.Framework;

namespace GameLibrary.Assists
{
    public class PrimGrid
    {
        private PrimitiveBatch pb;
        private float ySpacing;
        private float xSpacing;
        private Vector2 ScreenSize;

        public int yLineCount { get; internal set; }
        public int xLineCount { get; internal set; }

        public Color Color { get; set; }

        /// <summary>
        /// Defined x,y grid spacings
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public PrimGrid(int x, int y)
        {
            Color = Color.White;

            xLineCount = x;
            yLineCount = y;

            ScreenSize = new Vector2(Screen_Manager.GraphicsDevice.Viewport.Width, Screen_Manager.GraphicsDevice.Viewport.Height);

            xSpacing = ScreenSize.X / xLineCount;
            ySpacing = ScreenSize.Y / yLineCount;

            pb = new PrimitiveBatch(Screen_Manager.GraphicsDevice);
        }

        /// <summary>
        /// Equally spread grid. Uses the width for divisions. e.g: entering 2 will 
        /// make the cubes half the width of the screen.
        /// </summary>
        /// <param name="x"></param>
        public PrimGrid(int x)
        {
            xLineCount = x;
            yLineCount = x;

            ScreenSize = new Vector2(Screen_Manager.GraphicsDevice.Viewport.Width, Screen_Manager.GraphicsDevice.Viewport.Height);

            xSpacing = ScreenSize.X / xLineCount;
            ySpacing = xSpacing;

            pb = new PrimitiveBatch(Screen_Manager.GraphicsDevice);
        }

        public void Draw()
        {
            pb.Begin(PrimitiveType.LineList);

            for (int i = 0; i < xLineCount; i++)
            {
                pb.AddVertex(new Vector2(xSpacing * i, 0), Color.White);
                pb.AddVertex(new Vector2(xSpacing * i, ScreenSize.Y), Color.White);
            }

            for (int i = 0; i < yLineCount; i++)
            {
                pb.AddVertex(new Vector2(0, ySpacing * i), Color.White);
                pb.AddVertex(new Vector2(ScreenSize.X, ySpacing * i), Color.White);
            }

            pb.End();
        }

    }
}
