using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SharedGameData.Camera_2D
{
    public class Camera2D
    {
        #region Member Data
        protected float _zoom; // Camera Zoom
        protected float _rotation; // Camera Rotation
        public Vector2 _pos; // Camera Position
        public Matrix _transform; // Matrix Transform

        #endregion

        #region Properties
        // Sets and gets zoom
        public float Zoom
        {
            get { return _zoom; }
            set { _zoom = value; if (_zoom < 0.1f) _zoom = 0.1f; } // Negative zoom will flip image
        }

        public float Rotation
        {
            get { return _rotation; }
            set { _rotation = value; }
        }

        // Auxiliary function to move the camera
        public void Move(Vector2 amount)
        {
            _pos += amount;
        }

        // Get set position
        public Vector2 Pos
        {
            get { return _pos; }
            set { _pos = value; }
        }
        #endregion

        #region Constructor
        public Camera2D()
        {
            _zoom = 1.0f;
            _rotation = 0.0f;
            _pos = Vector2.Zero;
        }
        #endregion

        #region Matrix Functions
        /// <summary>
        /// Gets the transformation matrix that represents the camera.
        /// </summary>
        /// <param name="graphicsDevice"></param>
        /// <returns></returns>
        public Matrix get_transformation(Vector2 stuff)
        {
            _transform =       // Thanks to o KB o for this solution
              Matrix.CreateTranslation(new Vector3(-_pos.X, -_pos.Y, 0)) *
                                         Matrix.CreateRotationZ(Rotation) *
                                         Matrix.CreateScale(new Vector3(Zoom, Zoom, 1)) *
                                         Matrix.CreateTranslation(new Vector3(
                                             stuff.X * 0.5f,
                                             stuff.Y * 0.5f,
                                             0));
            return _transform;
        }
        #endregion

        #region Helper Functions
        /// <summary>
        ///  Returns the top left pixel of the camera's viewable area/viewport. This can be used to obtain the world co-ordinates of the mouse cursor.
        /// </summary>
        /// <param name="graphicsDevice">The graphics device</param>
        /// <returns></returns>
        public Point GetTopLeftCameraPoint(GraphicsDevice graphicsDevice)
        {
            return new Point((int)(Pos.X - (graphicsDevice.Viewport.Width / 2 / Zoom)), (int)(Pos.Y - (graphicsDevice.Viewport.Height / 2 / Zoom)));
        }
        #endregion

    }
}
