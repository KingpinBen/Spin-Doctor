//--------------------------------------------------------------------------
//--    
//--    Spin Doctor - Gears
//--    
//--    Current Version: 1.000
//--    
//--    Description
//--    ===============
//--    Adds wheels to the level
//--    
//--    Revision List
//--    ===============
//--    BenG - Initial
//--    BenG - Got it all working
//--    BenG - Fixed to work with the RoomType system.
//--    
//--    
//--    TBD
//--    ==============
//--    Complete it.
//--
//--    
//--    
//--------------------------------------------------------------------------

#region Using Statements
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using GameLibrary.Graphics.Camera;
#endregion

namespace GameLibrary.Levels
{
    public class Gears
    {
        #region Variables
        private Texture2D _gearsTex;
        private Texture2D _background;
        private Vector2 _topLeftGear; 
        private Vector2 _topRightGear;
        private Vector2 _botLeftGear; 
        private Vector2 _botRightGear;
        private float _scale = 2.5f;
        private float _gearRotation;
        #endregion

        #region Constructor
        public Gears()
        {
            
        }
        #endregion

        #region Load
        /// <summary>
        /// Calculate where to place the gears
        /// </summary>
        /// <param name="LC">Rotation Centre</param>
        /// <param name="CR">Radius of the Circle</param>
        public void Load(ContentManager content, float CR)
        {
            _gearsTex = content.Load<Texture2D>("Assets/Images/Textures/Environment/Cog 9T");
            _background = content.Load<Texture2D>("Assets/Images/Textures/RoomSetup/rock");

            //float texHyp = (float)Math.Sqrt((this._gearsTex.Width * this._gearsTex.Width));
            float offset = Camera.Instance.LevelDiameter * 0.5f;// + (texHyp * Scale));
            _scale = (Camera.Instance.LevelDiameter / _gearsTex.Width) * 0.35f;

            _topLeftGear = Vector2.Zero + new Vector2(-offset, -offset);
            _topRightGear = Vector2.Zero + new Vector2(offset, -offset);
            _botLeftGear = Vector2.Zero + new Vector2(-offset, offset);
            _botRightGear = Vector2.Zero + new Vector2(offset, offset);
        }
        #endregion

        #region Update
        public void Update(float delta)
        {
            if (Camera.Instance.IsLevelRotating)
            {
                _gearRotation = (float)-Camera.Instance.Rotation * 2;
            }
        }
        #endregion

        #region Draw
        public void Draw(SpriteBatch sb)
        {
            Vector2 origin = new Vector2(this._gearsTex.Width, this._gearsTex.Height) * 0.5f;

            sb.Draw(_background, Vector2.Zero, null, Color.White * 1.0f, 0.0f, new Vector2(_background.Width, _background.Height) * 0.5f, _scale * 2.0f, SpriteEffects.None, 1.0f);
            sb.Draw(_gearsTex, _topLeftGear, null, Color.White, _gearRotation, origin, _scale, SpriteEffects.None, 0.3f);
            sb.Draw(_gearsTex, _topRightGear, null, Color.White, _gearRotation, origin, _scale, SpriteEffects.None, 0.3f);
            sb.Draw(_gearsTex, _botLeftGear, null, Color.White, _gearRotation, origin, _scale, SpriteEffects.None, 0.3f);
            sb.Draw(_gearsTex, _botRightGear, null, Color.White, _gearRotation, origin, _scale, SpriteEffects.None, 0.3f);
        }
        #endregion
    }
}
