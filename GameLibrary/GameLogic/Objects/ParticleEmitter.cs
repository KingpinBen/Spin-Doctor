using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using FarseerPhysics.Dynamics;
using GameLibrary.GameLogic.Objects;
using GameLibrary.Helpers;
using GameLibrary.GameLogic.Screens;
using GameLibrary.Graphics;

namespace GameLibrary.GameLogic.Objects
{
    public class ParticleEmitter : NodeObject
    {
        #region Fields
        
#if EDITOR
        private Texture2D _devTexture;
        private int _devHeight = 35;
        private int _devWidth = 35;
#else
        List<Particle> _particles;
        Queue<Particle> _queuedParticles;
        float _elapsed = 0.0f;
        Texture2D _texture;
        World _world;
#endif


        [ContentSerializer]
        bool _isActive = true;
        [ContentSerializer]
        bool _useGravity = true;
        [ContentSerializer]
        string _textureAsset;

        [ContentSerializer]
        float _timeNewParticle;
        [ContentSerializer]
        float _minSpawnAngle;
        [ContentSerializer]
        float _maxSpawnAngle;
        [ContentSerializer]
        float _gravityModifier = 1.0f;
        [ContentSerializer]
        float _minLifeTime = 0.5f;
        [ContentSerializer]
        float _maxLifeTime = 2.0f;
        [ContentSerializer]
        int _particleCount;
        [ContentSerializer]
        int _minParticles;
        [ContentSerializer]
        int _maxParticles;
        #endregion

        #region Properties

#if EDITOR
        [ContentSerializerIgnore]
        public int MaxParticles
        {
            get
            {
                return _maxParticles;
            }
            set
            {
                _maxParticles = value;
            }
        }
        [ContentSerializerIgnore]
        public int MinParticles
        {
            get
            {
                return _minParticles;
            }
            set
            {
                _maxParticles = value;
            }
        }
        [ContentSerializerIgnore]
        public bool StartActive
        {
            get
            {
                return _isActive;
            }
            set
            {
                _isActive = value;
            }
        }
        [ContentSerializerIgnore]
        public float ParticlesPerSecond
        {
            get
            {
                return _timeNewParticle * 60.0f;
            }
            set
            {   
                //  Approx. 1/60
                _timeNewParticle = value * 0.017f;
            }
        }
        [ContentSerializerIgnore]
        public float GravityModifier
        {
            get
            {
                return _gravityModifier;
            }
            set
            {
                _gravityModifier = value;
            }
        }
        [ContentSerializerIgnore]
        public bool UseGravity
        {
            get
            {
                return _useGravity;
            }
            set
            {
                _useGravity = value;
            }

        }
        [ContentSerializerIgnore]
        public float LifeTimeMin
        {
            get
            {
                return _minLifeTime;
            }
            set
            {
                _minLifeTime = value;
            }
        }
        [ContentSerializerIgnore]
        public float LifeTimeMax
        {
            get 
            {
                return _maxLifeTime;
            }
            set
            {
                _maxLifeTime = value;
            }
               
        }
        [ContentSerializerIgnore]
        public float SpawnAngleMin
        {
            get
            {
                return _minSpawnAngle;
            }
            set
            {
                _minSpawnAngle = value;
            }
        }
        [ContentSerializerIgnore]
        public float SpawnAngleMax
        {
            get
            {
                return _maxSpawnAngle;
            }
            set
            {
                _maxSpawnAngle = value;
            }
        }
#else

#endif

        #endregion

        public ParticleEmitter()
        {

        }

        public void Init(Vector2 position, string texAsset)
        {
            this._position = position;
            this._timeNewParticle = 1 / 60;
            this._textureAsset = texAsset;
            this._maxParticles = 60;
            this._minParticles = 10;
        }

        public override void Load(ContentManager content, World world)
        {
#if EDITOR
            _devTexture = content.Load<Texture2D>("Assets/Other/Dev/Trigger");
#else
            this._texture = content.Load<Texture2D>(FileLoc.BlankPixel());
            this._particleCount = 10;
            this._particles = new List<Particle>(_particleCount);
            this._queuedParticles = new Queue<Particle>(_particleCount);

            this._world = world;

            for (int i = 0; i < _particleCount; i++)
            {
                _particles.Add(new Particle());
                _queuedParticles.Enqueue(_particles[i]);
            }
#endif
        }

        public override void Update(float delta)
        {
#if EDITOR

#else
            if (!_isActive) return;

            _elapsed += delta;

            for (int i = _particles.Count - 1; i >= 0; i--)
            {
                if (_particles[i].Alive)
                {
                    if (_useGravity)
                    {
                        _particles[i].Acceleration += (_world.Gravity * _gravityModifier) * _elapsed;
                    }

                    _particles[i].Update(_elapsed);

                    if (!_particles[i].Alive)
                    {
                        _queuedParticles.Enqueue(_particles[i]);
                    }
                }
            }

            if (_elapsed >= _timeNewParticle)
            {
                AddParticle();
                _elapsed = 0.0f;
            }
#endif
        }

        #region Draw
#if EDITOR
        public override void Draw(SpriteBatch sb)
        {
            sb.Draw(_devTexture, this._position, new Rectangle(0,0,_devWidth, _devHeight), Color.White * 0.7f, 0.0f, new Vector2(_devTexture.Width / 2, _devTexture.Height/ 2), 1.0f, SpriteEffects.None, _zLayer); 
        }
#else
        public override void Draw(SpriteBatch sb, GraphicsDevice graphics)
        {
            for (int i = 0; i < _particles.Count; i++)
            {
                if (!_particles[i].Alive)
                {
                    continue;
                }

                sb.Draw(_texture, _particles[i].Position, null, Color.CornflowerBlue, 0.0f, Vector2.Zero, 10.0f, SpriteEffects.None, 0.4f);
            }
        }
#endif
        #endregion

        #region Private Methods

        void AddParticle()
        {
#if EDITOR
#else
            int numParticles = SpinAssist.GetRandom(_minParticles, _maxParticles);

            for (int i = 0; i < numParticles; i++)
            {
                try
                {
                    Particle particle = _queuedParticles.Dequeue();
                    SetupParticle(particle);
                }
                catch (IndexOutOfRangeException e)
                {
                    ErrorReport.GenerateReport("A particle emitter tried making more particles than it could hold. OOR\n" + e.ToString(), null);
                }
            }
#endif
        }

        void SetupParticle(Particle particle)
        {
#if EDITOR
#else

            float life = SpinAssist.GetRandom(_minLifeTime, _maxLifeTime);
            Vector2 acceleration = Vector2.Zero;
            particle.Init(this._position, -_world.Gravity, acceleration, life);
#endif
        }

        #endregion
    }
}
