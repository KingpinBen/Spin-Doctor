﻿//--------------------------------------------------------------------------
//--    
//--    Spin Doctor - RotateRoomButton
//--    
//--    Description
//--    ===============
//--    An interactable object ingame to initiate a level rotate.
//--    
//--    Revision List
//--    ===============
//--    BenG - Initial
//--    BenG - More common Init function added. Better for editor and initializes
//--           some more data, and allows custom textures.
//--    BenG - Updated to keep in line with Trigger changes.
//--    BenG - Added delay timer before world rotates.
//--    BenG - Fixed some silly mistakes in update
//--    
//--    TBD
//--    ==============
//--    
//--
//--    
//--    
//--------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework.Graphics;
using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Dynamics.Contacts;
using GameLibrary.Graphics.Camera;
using GameLibrary.GameLogic.Controls;
using GameLibrary.Helpers;
using GameLibrary.GameLogic.Objects.Triggers;
using System.ComponentModel;
using GameLibrary.GameLogic.Characters;

namespace GameLibrary.GameLogic.Objects
{
    public class RotateRoomButton : Trigger
    {
        #region Fields

        [ContentSerializer(Optional=true)]
        private RotateDirection enumDirection = RotateDirection.Clockwise;
        [ContentSerializer(Optional = true)]
        private float _delayBeforeRotate = 1.0f;
#if EDITOR

#else
        private bool _aboutToRotate;
        private float _elapsed;
        private bool _firedRecently;
#endif

        #endregion

        #region Properties

#if EDITOR
        [ContentSerializerIgnore, CategoryAttribute("Object Specific")]
        public override TriggerType TriggerType
        {
            get
            {
                return _triggerType;
            }
            set { }
        }

        [ContentSerializerIgnore, CategoryAttribute("Object Specific")]
        public RotateDirection RotationDirection
        {
            get
            {
                return enumDirection;
            }

            set
            {
                enumDirection = value;
            }
        }
        [ContentSerializerIgnore, CategoryAttribute("Object Specific")]
        public override string Message
        {
            get
            {
                return _message;
            }
            set
            {
            }
        }
        [ContentSerializerIgnore, CategoryAttribute("Object Specific")]
        public float DelayBeforeRotation
        {
            get
            {
                return _delayBeforeRotate;
            }
            set
            {
                _delayBeforeRotate = value;
            }
        }
#else
        [ContentSerializerIgnore]
        public string RDirection
        {
            get 
            {
                if (enumDirection == RotateDirection.Clockwise)
                {
                    return "clock-wise";
                }

                return "counter clock-wise";
            }
        }
#endif

        #endregion

        #region Constructor

        public RotateRoomButton() : base() { }

        public override void Init(Vector2 position, string tex)
        {
            this._textureAsset = tex;
            this._message = "AUTO CHOSEN";
            this._position = position;
            this._zLayer = 0.5f;
            this._triggerHeight = this._triggerWidth = 30;
        }
        #endregion

        public override void Load(ContentManager content, World world)
        {
            this._texture = content.Load<Texture2D>(_textureAsset);
            this._origin = new Vector2(this._texture.Width, this._texture.Height) * 0.5f;
            this._triggerType = TriggerType.PlayerInput;

#if EDITOR
            if (_width == 0 || _height == 0)
            {
                this._width = this._texture.Width;
                this._height = this._texture.Height;
            }
#else
            //  This object has a unique message to display which way it'll rotate
            //  the level.
            this._message = " to rotate " + RDirection;

            this._triggerWidth = this._triggerHeight = this._texture.Width * 0.5f;
            this.SetupPhysics(world);
            this.RegisterObject();
#endif
        }

        public override void Update(float delta)
        {
#if !EDITOR
            if (_triggered) 
            {
                if (Player.Instance.PlayerState == PlayerState.Dead)
                {
                    ChangeTriggered(false);
                    return;
                }

                if (!_aboutToRotate && !_firedRecently)
                {
                    //  If the player is touching the trigger and it's not
                    //  about to rotate, check for an interation input.
                    if (InputManager.Instance.Interact(true))
                    {
                        //  If it's been pushed, push it over to aboutToRotate
                        //  to countdown for room rotation.
                        this._aboutToRotate = true;
                    }
                }
            }

            if (_aboutToRotate)
            {
                //  If the room has been triggered for a rotate, do a 
                //  countdown.
                this._elapsed += delta;

                //  Check if the elapsed since trigger has passed the delay
                //  timer
                if (_elapsed >= _delayBeforeRotate)
                {
                    //  Check which way the room is supposed to spin.
                    if (enumDirection == RotateDirection.Clockwise)
                    {
                        Camera.Instance.ForceRotateRight();
                    }
                    else
                    {
                        Camera.Instance.ForceRotateLeft();
                    }

                    //  Reset the object.
                    this._elapsed = 0.0f;
                    this._aboutToRotate = false;
                    //  Set the object as having been fired recently.
                    this._firedRecently = true;
                }
            }
            else
            {
                if (_firedRecently)
                {
                    this._elapsed += delta;

                    //  Has the elapsed timer exceeded the CD timer?
                    if (_elapsed >= 1.5)
                    {
                        //  Reset the ability to rotate.
                        this._firedRecently = false;
                    }
                }
            }
#endif
        }

        #region Draw
#if EDITOR
        public override void Draw(SpriteBatch sb)
        {
            sb.Draw(this._texture, this._position, null, this._tint, this._rotation, this._origin, 1.0f, SpriteEffects.None, this._zLayer);
        }

#else
        public override void Draw(SpriteBatch sb, GraphicsDevice graphics)
        {
            sb.Draw(this._texture, this._position, null, this._tint, this._rotation, this._origin, 1.0f, SpriteEffects.None, this._zLayer);
        }
#endif
        #endregion

    }
}
