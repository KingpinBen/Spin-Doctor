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
#endregion

namespace GameLibrary.Levels
{
    public class Level
    {
        #region Fields
#if EDITOR

#else
        ContentManager _content;
        Gears _gears;
        bool _initialized = false;
        LevelBackdrop _levelBackdrop;
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
        public RoomTypeEnum RoomType
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
        public bool IsInitialized
        {
            get { return _initialized; }
        }
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

#endif
        }

        public void Init()
        {
#if EDITOR
#else
            this._content = new ContentManager(ScreenManager.Game.Services, "Content");
            this._levelBackdrop = new LevelBackdrop();
            this._initialized = false;

            _gears = new Gears();
#endif
        }
        #endregion

        #region Load and Unload
        public void Load(World world)
        {
#if EDITOR
#else
            Init();
            this.SetupCamera();
            Player.Instance.Load(_content, world, _spawnLocation);

            this._levelBackdrop.Tint = this._backgroundTint;
            this._levelBackdrop.Load(_content, _roomDimensions, _roomTheme, _roomType, _backgroundFile);

            if (_roomType != RoomType.NonRotating)
            {
                _gears.Load(_content, Camera.Instance.LevelDiameter);
            }

            for (int i = 0; i < this._objectList.Count; i++)
            {
                this._objectList[i].Load(_content, world);
            }

            this._decalManager.Load();

            _initialized = true;
#endif
        }

        public void Unload()
        {
#if EDITOR
#else
            _content.Unload();
#endif
        }
        #endregion

        public void Update(GameTime gameTime)
        {
#if EDITOR

#else
            Player.Instance.Update(gameTime);

            for (int i = this._objectList.Count; i > 0; i--)
            {
                this._objectList[i - 1].Update(gameTime);
            }

            if (_roomType != RoomType.NonRotating)
            {
                _gears.Update(gameTime);
            }
#endif
        }

        #region Draw Calls
#if EDITOR

#else
        #region Draw NonRotating
        public void DrawBackground(SpriteBatch sb)
        {
            if (_roomType == RoomType.NonRotating)
            {
                return;
            }
            else
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

            this._decalManager.Draw(sb);
        }
        #endregion
#endif
        #endregion

        #region Private Methods

        void SetupCamera()
        {
            float largestLevelDimension = 0;
            bool canRotate = false;

            largestLevelDimension = (float)Math.Sqrt((int)(_roomDimensions.X * _roomDimensions.X) + (int)(_roomDimensions.Y * _roomDimensions.Y));

            if (this._roomType == RoomType.Rotating)
                canRotate = true;

            Camera.Instance.Load(canRotate, largestLevelDimension * 0.5f);
        }

        #endregion
    }
}
