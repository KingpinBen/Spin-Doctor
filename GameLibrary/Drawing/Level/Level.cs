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
using GameLibrary.Managers;
using GameLibrary.Objects;
using GameLibrary.Objects.Triggers;
using GameLibrary.Screens;
using System.Threading;
#endregion

namespace GameLibrary.Drawing
{
    public enum RoomThemeEnum
    {
        Industrial, Medical, Study, General
    }

    public enum RoomTypeEnum
    {
        Rotating, NonRotating, Hub
    }

    public class Level
    {
        #region Fields

        private Player _player;
        private ContentManager _content;
        private Gears _gears;
        private bool _initialized;
        private LevelBackdrop _levelBackdrop;

        [ContentSerializer]
        private List<PhysicsObject> physicsObjectsList;
        //[ContentSerializer]
        //private bool _levelRotates;
        [ContentSerializer]
        private Vector2 _spawnLocation = Vector2.Zero;
        [ContentSerializer]
        private RoomTypeEnum _roomType = RoomTypeEnum.NonRotating;
        [ContentSerializer]
        private RoomThemeEnum _roomTheme = RoomThemeEnum.General;
        [ContentSerializer]
        private Decal_Manager _decalManager;
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
        public List<PhysicsObject> PhysicsObjectsList
        {
            get
            {
                return physicsObjectsList;
            }
            set
            {
                physicsObjectsList = value;
            }
        }
        [ContentSerializerIgnore]
        public Decal_Manager DecalManager
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
                return this._levelBackdrop.Tint;
            }
            set
            {
                this._levelBackdrop.Tint = value;
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
        public Vector2 RoomDimensions
        {
            get
            {
                return _roomDimensions;
            }
        }        
        //[ContentSerializerIgnore]
        //public bool CanLevelRotate
        //{
        //    get 
        //    { 
        //        return _levelRotates; 
        //    }
        //    protected set
        //    {
        //        _levelRotates = value;
        //    }
        //}
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
            this.physicsObjectsList = new List<PhysicsObject>();
            this._decalManager = new Decal_Manager();
            this._levelBackdrop = new LevelBackdrop();
        }

        public void Init()
        {
            this._content = new ContentManager(Screen_Manager.Game.Services, "Content");
            this._player = Player.Instance;
            this._initialized = false;

            _gears = new Gears();
        }
        #endregion

        #region Load and Unload
        public void Load(World world)
        {
            Init();
            this.SetupCamera();
            Player.Instance.Load(_content, world, _spawnLocation, true);
            
            this._levelBackdrop.Load(_content, _roomDimensions, _roomTheme, _backgroundFile);

            if (_roomType != RoomTypeEnum.NonRotating)
            {
                _gears.Load(_content, Vector2.Zero, Camera.LevelDiameter);
            }

            for (int i = 0; i < physicsObjectsList.Count; i++)
            {
                physicsObjectsList[i].Load(_content, world);
            }

            this._decalManager.Load();

            _initialized = true;
        }

        public void Unload()
        {
            _content.Unload();
        }
        #endregion

        #region Update
        public void Update(GameTime gameTime)
        {
#if EDITOR

#else
            Player.Instance.Update(gameTime);

            for (int i = physicsObjectsList.Count; i > 0; i--)
            {
                physicsObjectsList[i - 1].Update(gameTime);
            }

            if (_roomType != RoomTypeEnum.NonRotating)
            {
                _gears.Update(gameTime);
            }
#endif
        }
        #endregion

        #region Draw Calls
#if EDITOR

#else
        #region Draw NonRotating
        public void DrawBackground(SpriteBatch sb)
        {
            if (_roomType == RoomTypeEnum.NonRotating)
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
        public void DrawGameplay(SpriteBatch sb)
        {
            this._decalManager.Draw(sb);
            this._player.Draw(sb);

            if (_roomType == RoomTypeEnum.Rotating)
            {
                this._levelBackdrop.DrawShell(sb);
            }
            else
            {
                this._levelBackdrop.Draw(sb);
            }

            for (int i = physicsObjectsList.Count; i > 0; i--)
            {
                this.physicsObjectsList[i - 1].Draw(sb);
            }
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

            if (this._roomType == RoomTypeEnum.Rotating)
                canRotate = true;

            Camera.Load(canRotate, largestLevelDimension / 2);
        }

        #endregion
    }
}
