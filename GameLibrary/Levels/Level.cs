//--------------------------------------------------------------------------------
//--    
//--    Spin Doctor - Level Class
//--
//--    
//--    Description
//--    ===============
//--    
//--
//--    
//--    Revision List
//--    ===============
//--    BenG - Initial
//--    BenG - Handles memory better
//--    BenG - Renamed and added some variables for future level requirements
//--    BenG - Moved new Content call into constructor
//--    BenG - Renamed enums for better naming conventions.
//--    BenG - Changed some things to work better with the editor. Camera setup is more dynamic now too.
//--    
//--    TBD
//--    ==============
//--    
//--    
//--    
//-------------------------------------------------------------------------------

#region Using Statements
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Intermediate;
using FarseerPhysics.Dynamics;
using System.Threading;
using GameLibrary.Graphics;
using GameLibrary.GameLogic.Objects;
using GameLibrary.Graphics.Camera;
using GameLibrary.GameLogic;
using GameLibrary.GameLogic.Screens;
using GameLibrary.GameLogic.Events;
using GameLibrary.GameLogic.Characters;
#endregion

namespace GameLibrary.Levels
{
    public class Level : IDisposable
    {
        #region Fields

        ContentManager _content;
        Gears _gears;
        LevelBackdrop _levelBackdrop;

#if EDITOR
#else
        GameplayScreen _gameScreen;
        
#endif

        [ContentSerializer]
        private List<NodeObject> _objectList = new List<NodeObject>();
        [ContentSerializer]
        private Vector2 _spawnLocation = Vector2.Zero;
        [ContentSerializer]
        private RoomType _roomType = RoomType.NonRotating;
        [ContentSerializer]
        private RoomTheme _roomTheme = RoomTheme.General;
        [ContentSerializer]
        private DecalManager _decalManager = new DecalManager();
        [ContentSerializer]
        private Vector2 _roomDimensions = Vector2.Zero;
        [ContentSerializer]
        private string _backgroundFile = String.Empty;
        [ContentSerializer]
        private Color _backgroundTint = Color.White;

        #endregion

        #region Properties
            
#if EDITOR
        [ContentSerializerIgnore]
        public Vector2 RoomDimensions
        {
            get
            {
                return _roomDimensions;
            }
            set
            {
                _roomDimensions = value;
            }
        }
        [ContentSerializerIgnore]
        public string BackgroundFile
        {
            get
            {
                return _backgroundFile;
            }
            set
            {
                _backgroundFile = value;
            }
        }
        [ContentSerializerIgnore]
        public List<NodeObject> ObjectsList
        {
            get
            {
                return _objectList;
            }
            set
            {
                _objectList = value;
            }
        }
        [ContentSerializerIgnore]
        public DecalManager DecalManager
        {
            get
            {
                return _decalManager;
            }
            set
            {
                _decalManager = value;
            }
        }
        [ContentSerializerIgnore]
        public RoomType RoomType
        {
            get
            {
                return _roomType;
            }
            set
            {
                _roomType = value;
            }
        }
        [ContentSerializerIgnore]
        public Color BackgroundTint
        {
            get
            {
                return this._backgroundTint;
            }
            set
            {
                this._backgroundTint = value;
            }
        }
#else
        [ContentSerializerIgnore]
        public ContentManager Content
        {
            get
            {
                return _content;
            }
        }
        [ContentSerializerIgnore]
        public List<NodeObject> ObjectsList
        {
            get
            {
                return _objectList;
            }
        }
        [ContentSerializerIgnore]
        public DecalManager DecalManager
        {
            get
            {
                return _decalManager;
            }
        }
        [ContentSerializerIgnore]
        public RoomType RoomType
        {
            get
            {
                return _roomType;
            }
        }
#endif

        [ContentSerializerIgnore]
        public Vector2 PlayerSpawnLocation
        {
            get { return _spawnLocation; }
#if EDITOR
            set
#else
            protected set
#endif
            {
                _spawnLocation = value;
            }
        }

        #endregion

        #region Constructor
        public Level()
        {
            this._objectList = new List<NodeObject>();
            this._decalManager = new DecalManager();
#if EDITOR
            this._backgroundTint = Color.White;
#else
            this._levelBackdrop = new LevelBackdrop();
            this._gears = new Gears();
#endif
        }

        #endregion

        #region Load and Unload


        #region Load

        /// <summary>
        /// Setup and load the level and everything in it.
        /// </summary>
        /// <param name="screen">The gameplay screen</param>
        public void Load(GameplayScreen screen)
        {
            //  This should only be called for the game so we ignore the
            //  editor. Plus, some fields used aren't for the editor.
#if !EDITOR
            this._content = new ContentManager(screen.ScreenManager.Game.Services, "Content");

            //  We want a reference to the gameplay screen in case
            //  we need it later on.
            this._gameScreen = screen;

            //  Set up the camera to work correctly with the level.
            this.SetupCamera();

            //  Setup the player to start on the spawn position.
            Player.Instance.Load(screen.ScreenManager.Game, screen.World, 
                _spawnLocation);

            //  Refresh the decal manager
            this._decalManager.Load(screen);

            //  Setup the backgrounds of the level so it looks the way it should.
            this._levelBackdrop.Tint = this._backgroundTint;
            this._levelBackdrop.Load(_content, _roomDimensions, _roomTheme,
                _roomType, _backgroundFile);

            //  We only need the gears for a rotating room, so don't bother
            //  if it's not.
            if (_roomType == RoomType.Rotating)
            {
                _gears.Load(_content, Camera.Instance.LevelDiameter);
            }

            //  Create all the objects for the level world.
            for (int i = this._objectList.Count - 1; i >= 0; i--)
            {
                this._objectList[i].Load(_content, _gameScreen.World);
            }
#endif
        }

        #endregion

        #region Unload


        /// <summary>
        /// Unload all the content
        /// </summary>
        public void Unload()
        {
            _content.Unload();
        }


        #endregion


        #endregion

        #region Update and Draw

        public void Update(float delta)
        {
#if EDITOR

#else
            for (int i = this._objectList.Count - 1; i >= 0; i--)
            {
                this._objectList[i].Update(delta);
            }

            EventManager.Instance.Update(delta);

            Player.Instance.Update(delta, _gameScreen.World);

            if (_roomType != RoomType.NonRotating)
            {
                _gears.Update(delta);
            }
#endif
        }

        #region Draw Calls
#if !EDITOR

        #region Draw NonRotating

        /// <summary>
        /// Draw the non-rotating background elements.
        /// </summary>
        public void DrawBackground()
        {
            SpriteBatch spriteBatch = this._gameScreen.ScreenManager.SpriteBatch;

            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, null, null,
                    Matrix.CreateTranslation(new Vector3(-Camera.Instance.Position, 0)) *
                    Matrix.CreateRotationZ(0) *
                    Matrix.CreateScale(new Vector3(Camera.Instance.Zoom, Camera.Instance.Zoom, 1)) *
                    Matrix.CreateTranslation(new Vector3(this._gameScreen.ScreenManager.GraphicsDevice.Viewport.Width * 0.5f, this._gameScreen.ScreenManager.GraphicsDevice.Viewport.Height * 0.5f, 0f)));

            this._gears.Draw(spriteBatch);

            spriteBatch.End();
        }


        #endregion

        #region Draw Gameplay

        public void DrawBackdrop(ref Matrix cameraTransform)
        {
            SpriteBatch spriteBatch = this._gameScreen.ScreenManager.SpriteBatch;

            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied, SamplerState.AnisotropicWrap, null, null, null, cameraTransform);
            this._levelBackdrop.Draw(spriteBatch);
            spriteBatch.End();
        }
        #endregion
#endif
        #endregion

        #endregion

        #region Private Methods

        void SetupCamera()
        {
#if EDITOR

#else
            float largestLevelDimension = 0;
            bool canRotate = false;

            if (_roomType == RoomType.Rotating)
            {
                canRotate = true;
                largestLevelDimension = (float)Math.Sqrt((int)(_roomDimensions.X * _roomDimensions.X) + (int)(_roomDimensions.Y * _roomDimensions.Y));
            }
            else
            {
                if (_roomDimensions.X > _roomDimensions.Y)
                {
                    largestLevelDimension = _roomDimensions.X;
                }
                else
                {
                    largestLevelDimension = _roomDimensions.Y;
                }
            }

            Camera.Instance.Load(_gameScreen, canRotate, largestLevelDimension);
#endif
        }

        #endregion

        public void Dispose()
        {
#if !EDITOR
            this._gameScreen = null;
            this._content.Unload();
            this._content = null;
            this._gears = null;
            this._levelBackdrop = null;
#endif
        }


    }
}
