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
        private List<NodeObject> _objectList;
        [ContentSerializer]
        private Vector2 _spawnLocation = Vector2.Zero;
        [ContentSerializer]
        private RoomType _roomType = RoomType.NonRotating;
        [ContentSerializer]
        private RoomTheme _roomTheme = RoomTheme.General;
        [ContentSerializer]
        private DecalManager _decalManager;
        [ContentSerializer]
        private Vector2 _roomDimensions;
        [ContentSerializer]
        private string _backgroundFile;
        [ContentSerializer]
        private Color _backgroundTint;

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
            for (int i = 0; i < this._objectList.Count; i++)
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

        public void Update(float delta)
        {
#if EDITOR

#else
            Player.Instance.Update(delta, _gameScreen.World);
            EventManager.Instance.Update(delta);

            for (int i = this._objectList.Count; i > 0; i--)
            {
                this._objectList[i - 1].Update(delta);
            }

            if (_roomType != RoomType.NonRotating)
            {
                _gears.Update(delta);
            }
#endif
        }

        #region Draw Calls
#if EDITOR

#else
        #region Draw NonRotating

        /// <summary>
        /// Draw the non-rotating background elements.
        /// </summary>
        /// <param name="SpriteBatch">The non-transformed Spritebatch</param>
        public void DrawBackground(SpriteBatch sb)
        {
            //  Gears can only be drawn if the level is rotating
            if (_roomType == RoomType.Rotating)
            {
                _gears.Draw(sb);
            }
        }


        #endregion

        #region Draw Gameplay

        public void DrawBackdrop(SpriteBatch sb)
        {
            if (_roomType == RoomType.Rotating)
            {
                this._levelBackdrop.Draw(sb);
            }
        }
        #endregion
#endif
        #endregion

        #region Private Methods

        void SetupCamera()
        {
#if EDITOR

#else
            float largestLevelDimension = 0;
            bool canRotate = false;

            largestLevelDimension = (float)Math.Sqrt((int)(_roomDimensions.X * _roomDimensions.X) + (int)(_roomDimensions.Y * _roomDimensions.Y));

            if (this._roomType == RoomType.Rotating)
                canRotate = true;

            Camera.Instance.Load(_gameScreen, canRotate, largestLevelDimension * 0.5f);
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
