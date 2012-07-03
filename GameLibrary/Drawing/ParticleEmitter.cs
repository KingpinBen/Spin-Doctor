using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GameLibrary.Screens;
using Microsoft.Xna.Framework.Content;
using GameLibrary.Assists;
using GameLibrary.Objects;
using FarseerPhysics.Dynamics;

namespace GameLibrary.Drawing
{
    public class ParticleEmitter : NodeObject
    {
        #region Fields
        
#if EDITOR

#else
        
#endif
        List<Particle> _particles;
        Queue<Particle> _queuedParticles;
        float _elapsed;

        Texture2D _texture;
        Vector2 _position;
        bool _isActive;
        bool _useGravity;
        string _textureAsset;

        float _timeNewParticle;
        float _minSpawnAngle;
        float _maxSpawnAngle;
        float _gravityModifier;
        float _minLifeTime;
        float _maxLifeTime;

        int _particleCount;
        int _minParticles;
        int _maxParticles;
        #endregion

        #region Properties

#if EDITOR
        [ContentSerializerIgnore]
        public Vector2 Position
        {
            get
            {
                return _position;
            }
            set
            {
                _position = value;
            }
        }
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
        }

        public override void  Load(ContentManager content, World world)
        {
#if EDITOR

#else
            _texture = content.Load<Texture2D>(_textureAsset);
            _isActive = true;

            this._particles = new List<Particle>(_particleCount);
            this._queuedParticles = new Queue<Particle>(_particleCount);

            for (int i = 0; i < _particleCount; i++)
            {
                _particles.Add(new Particle());
                _queuedParticles.Enqueue(_particles[i]);
            }
#endif
        }

        public void Update(GameTime gameTime)
        {
#if EDITOR

#else
            if (!_isActive) return;

            _elapsed += (float)gameTime.ElapsedGameTime.TotalMilliseconds * 0.001f;

            for (int i = _particles.Count - 1; i >= 0; i--)
            {
                if (_particles[i].Alive)
                {
                    if (_useGravity)
                    {
                        _particles[i].Acceleration += (GameplayScreen.World.Gravity * _gravityModifier)* _elapsed;
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

        public void Draw(SpriteBatch sb)
        {
            for (int i = 0; i < _particles.Count; i++)
            {
                if (!_particles[i].Alive)
                {
                    continue;
                }
            }
        }

        void AddParticle()
        {
            int numParticles = SpinAssist.GetRandom(_minParticles, _maxParticles);

            for (int i = 0; i < numParticles; i++)
            {
                Particle particle = _queuedParticles.Dequeue();
                SetupParticle(particle);
            }
        }

        void SetupParticle(Particle particle)
        {
            float life = SpinAssist.GetRandom(_minLifeTime, _maxLifeTime);
            Vector2 acceleration = Vector2.Zero;
            particle.Init(this._position, Vector2.Zero, acceleration, life);
        }
    }
}
