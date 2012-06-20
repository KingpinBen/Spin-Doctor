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
using GameLibrary.Drawing;
using GameLibrary.Screens;
using GameLibrary.Managers;
using Microsoft.Xna.Framework.Content;
#endregion

namespace GameLibrary.Objects
{
    public class Gears
    {
        #region Variables
        private Texture2D _gearsTex;
        private Texture2D _background;
        private Vector2 TopLeftGear; 
        private Vector2 TopRightGear; 
        private Vector2 BotLeftGear; 
        private Vector2 BotRightGear;
        private float Scale; 
        private float GearRotation;
        #endregion

        #region Constructor
        public Gears()
        {
            Scale = 1.5f;
        }
        #endregion

        #region Load
        /// <summary>
        /// Calculate where to place the gears
        /// </summary>
        /// <param name="LC">Rotation Centre</param>
        /// <param name="CR">Radius of the Circle</param>
        public void Load(ContentManager content, Vector2 LC, float CR)
        {
            _gearsTex = content.Load<Texture2D>("Assets/Sprites/Textures/RoomSetup/gear");
            _background = content.Load<Texture2D>("Assets/Sprites/Textures/RoomSetup/rock");
            float offset = (CR - _gearsTex.Width) + 25f;

            TopLeftGear = new Vector2(LC.X - offset, LC.Y - offset);
            TopRightGear = new Vector2(LC.X + offset, LC.Y - offset);
            BotLeftGear = new Vector2(LC.X - offset, LC.Y + offset);
            BotRightGear = new Vector2(LC.X + offset, LC.Y + offset);
        }
        #endregion

        #region Update
        public void Update(GameTime gameTime)
        {
            if (Camera.LevelRotating)
                GearRotation = (float)-Camera.Rotation * 3;
            else
                GearRotation = 0.0f;
        }
        #endregion

        #region Draw
        public void Draw(SpriteBatch sb)
        {
            sb.Draw(_gearsTex, TopLeftGear,     null, Color.DimGray, GearRotation, new Vector2(_gearsTex.Width / 2, _gearsTex.Height / 2), Scale, SpriteEffects.None, 0.3f);
            sb.Draw(_gearsTex, TopRightGear,    null, Color.DimGray, GearRotation, new Vector2(_gearsTex.Width / 2, _gearsTex.Height / 2), Scale, SpriteEffects.None, 0.3f);
            sb.Draw(_gearsTex, BotLeftGear,     null, Color.DimGray, GearRotation, new Vector2(_gearsTex.Width / 2, _gearsTex.Height / 2), Scale, SpriteEffects.None, 0.3f);
            sb.Draw(_gearsTex, BotRightGear,    null, Color.DimGray, GearRotation, new Vector2(_gearsTex.Width / 2, _gearsTex.Height / 2), Scale, SpriteEffects.None, 0.3f);

            sb.Draw(_background, Vector2.Zero, null, Color.White * 1.0f, 0.0f, new Vector2(_background.Width / 2, _background.Height / 2), 5f, SpriteEffects.None, 1.0f);
        }
        #endregion
    }
}
