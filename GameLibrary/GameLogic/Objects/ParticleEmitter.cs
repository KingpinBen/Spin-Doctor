﻿using System;
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
using GameLibrary.Graphics.Drawing;

namespace GameLibrary.GameLogic.Objects
{
    public class ParticleEmitter : NodeObject
    {
        #region Fields
        
#if EDITOR
        private Texture2D _devTexture;
        private Texture2D _arrow;
        private int _devHeight;
        private int _devWidth;
#else
        List<Particle> _particles;
        Queue<Particle> _queuedParticles;
        float _elapsed = 0.0f;
        Texture2D _texture;
        World _world;
        Vector2 _velocity;
#endif

        [ContentSerializer(Optional = true)]
        bool _useGravity = true;
        [ContentSerializer(Optional = true)]
        string _textureAsset;

        [ContentSerializer(Optional = true)]
        float _timeNewParticle = 3.0f;
        [ContentSerializer(Optional = true)]
        float _minSpawnAngle = 0;
        [ContentSerializer(Optional = true)]
        float _maxSpawnAngle = 360;
        [ContentSerializer(Optional = true)]
        float _gravityModifier = 1.0f;
        [ContentSerializer(Optional = true)]
        float _minLifeTime = 0.5f;
        [ContentSerializer(Optional = true)]
        float _maxLifeTime = 2.0f;
        [ContentSerializer(Optional = true)]
        int _particleCount = 30;
        [ContentSerializer(Optional = true)]
        int _minParticles = 5;
        [ContentSerializer(Optional = true)]
        int _maxParticles = 20;
        [ContentSerializer(Optional = true)]
        float _minSpeed = 10;
        [ContentSerializer(Optional = true)]
        float _maxSpeed = 100;
        [ContentSerializer(Optional = true)]
        float _scaleDelta = 0.0f;
        [ContentSerializer(Optional = true)]
        float _scale = 1.0f;
        [ContentSerializer(Optional = true)]
        float _alpha = 1.0f;
        [ContentSerializer(Optional = true)]
        float _alphaDelta = 0.0f;
        [ContentSerializer(Optional = true)]
        Color _tint = Color.White;
        [ContentSerializer(Optional = true)]
        bool _useRandomRotation = false;

        #endregion

        #region Properties

#if EDITOR
        [ContentSerializerIgnore]
        public bool UseRandomParticleRotation
        {
            get
            {
                return _useRandomRotation;
            }
            set
            {
                _useRandomRotation = value;
            }
        }
        [ContentSerializerIgnore]
        public Color Tint
        {
            get
            {
                return _tint;
            }
            set
            {
                _tint = value;
            }
        }
        [ContentSerializerIgnore]
        public float SpeedMin
        {
            get
            {
                return _minSpeed;
            }
            set
            {
                _minSpeed = value;
            }
        }
        [ContentSerializerIgnore]
        public float SpeedMax
        {
            get
            {
                return _maxSpeed;
            }
            set
            {
                _maxSpeed = value;
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
                _minParticles = value;
            }
        }
        [ContentSerializerIgnore]
        public float TimeBetweenParticlesSpawn
        {
            get
            {
                return _timeNewParticle * 60.0f;
            }
            set
            {   
                //  Approx. 1/60
                _timeNewParticle = value / 60;
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
                _minSpawnAngle = MathHelper.Clamp(value, 0, 360);
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
                _maxSpawnAngle = MathHelper.Clamp(value, 0, 360);
            }
        }
        [ContentSerializerIgnore]
        public float Alpha
        {
            get
            {
                return _alpha;
            }
            set
            {
                _alpha = value;
            }
        }
        [ContentSerializerIgnore]
        public float AlphaDecay
        {
            get
            {
                return _alphaDelta;
            }
            set
            {
                _alphaDelta = value;
            }
        }
        [ContentSerializerIgnore]
        public float Scale
        {
            get
            {
                return _scale;
            }
            set
            {
                _scale = value;
            }
        }
        [ContentSerializerIgnore]
        public float ScaleGrowth
        {
            get
            {
                return _scaleDelta;
            }
            set
            {
                _scaleDelta = value;
            }
        }
#endif

        #endregion

        public ParticleEmitter()
        {

        }

        public void Init(Vector2 position, string texAsset)
        {
            this._position = position;
            this._textureAsset = texAsset;
        }

        public override void Load(ContentManager content, World world)
        {
            base.Load(content, world);

#if EDITOR
            _devTexture = content.Load<Texture2D>("Assets/Other/Dev/Trigger");
            _arrow = content.Load<Texture2D>("Assets/Other/Dev/arrow");

            _devHeight = _devTexture.Height;
            _devWidth = _devTexture.Width;
#else
            this._texture = content.Load<Texture2D>(_textureAsset);
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

        #region Update and Draw

        public override void Update(float delta)
        {
#if !EDITOR
            //_velocity = this._position;

            for (int i = _particles.Count - 1; i >= 0; i--)
            {
                Particle particle = _particles[i];

                if (particle.IsAlive)
                {
                    if (_useGravity)
                    {
                        Vector2 acceleration = (_world.Gravity * _gravityModifier) * delta;

                        particle.Acceleration.X += acceleration.X;
                        particle.Acceleration.Y += acceleration.Y;
                    }

                    particle.Alpha -= _alphaDelta;
                    particle.Scale += _scaleDelta;

                    particle.Update(delta);
                    

                    if (!particle.IsAlive)
                    {
                        _queuedParticles.Enqueue(particle);
                    }
                }
            }

            if (_enabled)
            {
                _elapsed += delta;

                if (_elapsed >= _timeNewParticle)
                {
                    AddParticle();
                    _elapsed -= _timeNewParticle;
                }
            }
#endif
        }

        #region Draw
#if EDITOR
        public override void Draw(SpriteBatch sb)
        {
            sb.Draw(_devTexture, this._position, new Rectangle(0, 0, (int)_devTexture.Width, (int)_devTexture.Height), Color.White * 0.7f, 0.0f,
                new Vector2(_devTexture.Width, _devTexture.Height) * 0.5f, 1.0f, SpriteEffects.None, _zLayer);

            float max = MathHelper.ToRadians(_maxSpawnAngle);
            float min = MathHelper.ToRadians(_minSpawnAngle);
            float result = max- min;

            sb.Draw(_arrow, this._position, null, Color.White, max , new Vector2(0, _arrow.Height) * 0.5f, 1.0f, SpriteEffects.None, _zLayer);
            sb.Draw(_arrow, this._position, null, Color.White, min, new Vector2(0, _arrow.Height) * 0.5f, 1.0f, SpriteEffects.None, _zLayer);
            sb.Draw(_arrow, this._position, null, Color.Black, min + (result * 0.5f), new Vector2(0, _arrow.Height) * 0.5f, 0.75f, SpriteEffects.None, _zLayer);
        }
#else
        public override void Draw(SpriteBatch sb, GraphicsDevice graphics)
        {
            Particle particle;
            for (int i = 0; i < _particles.Count; i++)
            {
                particle = _particles[i];

                if (!particle.IsAlive)
                {
                    continue;
                }

                sb.Draw(_texture, particle.Position, null, _tint * particle.Alpha, particle.Rotation, new Vector2(_texture.Width, _texture.Height) * 0.5f, particle.Scale, SpriteEffects.None, this._zLayer);
            }
        }
#endif
        #endregion

        #endregion

        #region Private Methods

        void AddParticle()
        {
#if EDITOR
#else
            int numParticles = SpinAssist.GetRandom(_minParticles, _maxParticles);

            for (int j = 0; j < numParticles; j++)
            {
                if (_queuedParticles.Count == 0)
                {
                    for (int i = 0; i < numParticles; i++)
                    {
                        Particle p = new Particle();
                        _particles.Add(p);
                        _queuedParticles.Enqueue(p);

                    }
                }

                Particle particle = _queuedParticles.Dequeue();
                SetupParticle(particle);
            }
#endif
        }

        void SetupParticle(Particle particle)
        {
#if EDITOR
#else
            //  Life
            float life = SpinAssist.GetRandom(_minLifeTime, _maxLifeTime);

            //  Velocity
            float angle = SpinAssist.GetRandom(this._minSpawnAngle, this._maxSpawnAngle);
            angle = MathHelper.ToRadians(angle);
            Vector2 direction = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));

            float speed = SpinAssist.GetRandom(this._minSpeed, this._maxSpeed);
            _velocity = direction * speed;

            Vector2 acceleration = Vector2.Zero;

            //if (_useGravity)
            //{
            //    acceleration = _world.Gravity * _gravityModifier;
            //}

            
            particle.Init(this._position, _velocity, acceleration, life);
            particle.Alpha = _alpha;
            particle.Scale = _scale;
#endif
        }

        #region Events
#if !EDITOR

        public override void Start()
        {
            base.Enable();
        }

        public override void Stop()
        {
            base.Disable();
        }
#endif
        #endregion

        #endregion
    }
}
