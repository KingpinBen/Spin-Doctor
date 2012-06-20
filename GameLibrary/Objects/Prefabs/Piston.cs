//--------------------------------------------------------------------------------
//--    
//--    Spin Doctor
//--
//--    
//--    Description
//--    ===============
//--    Allows the player to be pushed/crushed
//--
//--    
//--    Revision List
//--    ===============
//--    BenG - Initial
//--    BenG - Switched over to a dynamicObject
//--    BenG - Added functionality to the editor. Not complete (needs testing).
//--    
//--    
//--    TBD
//--    ==============
//--    Complete extra bodies 
//--    
//-------------------------------------------------------------------------------

#region Usings
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework.Graphics;
using GameLibrary.Assists;
using FarseerPhysics.Dynamics.Joints;
using GameLibrary.Managers;
#endregion

namespace GameLibrary.Objects
{
    /// <summary>
    /// Static object in spirit. PhysicsObject at heart. (Doesn't use staticObject methods).
    /// </summary>
    public class Piston : DynamicObject
    {
        #region Fields

        /// <summary>
        /// Inherited body is the static mainbody from which the others
        /// 'pop out' of (making the whole structure use 4 bodies).
        /// 
        /// We can't use fixtures and only 1 body unfortunately as all the
        /// bodies will be joined with joints.
        /// </summary>
        /// 
        protected Body firstBody;
        protected Body secondBody;
        protected Body endBody;

        protected Texture2D firstBodyTexture;
        protected Texture2D secondBodyTexture;
        protected Texture2D endBodyTexture;

        protected FixedPrismaticJoint secondBodyJoint;
        protected FixedPrismaticJoint firstBodyJoint;

        [ContentSerializer]
        private string firstBodyTexAsset;
        [ContentSerializer]
        private string secondBodyTexAsset;
        [ContentSerializer]
        private string endBodyTexAsset;

        #endregion

        #region Properties
        [ContentSerializerIgnore]
        public new Vector2 EndPosition
        {
            get { return _endPosition; }

            //  Internal as the end point should be calculated on texture size.
            internal set
            {
                _endPosition = value;
            }
        }

        #endregion

        #region Constructor and Initialize
        public Piston()
            : base()
        {
        }

        public void Init(Vector2 position, string texLoc0, string texLoc1, string texLoc2, string texLoc3)
        {
            this._position = position;
            this._textureAsset = texLoc0;
            this.firstBodyTexAsset = texLoc1;
            this.secondBodyTexAsset = texLoc2;
            this.endBodyTexAsset = texLoc3;
        }
        #endregion

        #region Load
        public override void Load(ContentManager content, World world)
        {
            Texture = content.Load<Texture2D>(_textureAsset);

            firstBodyTexture = content.Load<Texture2D>(firstBodyTexAsset);
            secondBodyTexture = content.Load<Texture2D>(secondBodyTexAsset);
            endBodyTexture = content.Load<Texture2D>(endBodyTexAsset);

#if EDITOR
            this._width = Texture.Width;
            this._height = Texture.Height;
            return;
#else
            this.CreateBodies(world);
            this._isMoving = this.StartsMoving;

            if (!this._isMoving)
            {
                this._prismaticJoint.MotorSpeed = 0.0f;
            }
            else
            {
                this._prismaticJoint.MotorSpeed = MotorSpeed;
            }
#endif
        }
        #endregion

        #region Update
        public override void Update(GameTime gameTime)
        {
#if EDITOR

#else
            base.Update(gameTime);

            HandleExtraJoint(gameTime, firstBodyJoint);
            HandleExtraJoint(gameTime, secondBodyJoint);
#endif
            
        }
        #endregion

        #region Draw
#if EDITOR
        public override void Draw(SpriteBatch sb)
        {
                sb.Draw(Texture, Position, null,
                Color.White, this.Body.Rotation, this.Origin, 1.0f,
                SpriteEffects.None, zLayer);
        }

#else
        public override void Draw(SpriteBatch sb)
        {
            sb.Draw(Texture, ConvertUnits.ToDisplayUnits(this.Body.Position), null,
                _tint, this.Body.Rotation, new Vector2(Texture.Width / 2, Texture.Height / 2), 1.0f,
                SpriteEffects.None, zLayer);

            sb.Draw(firstBodyTexture,
                ConvertUnits.ToDisplayUnits(this.firstBody.Position), null,
                Color.White, this.firstBody.Rotation,
                new Vector2(this.firstBodyTexture.Width / 2, this.firstBodyTexture.Height / 2),
                1.0f, SpriteEffects.None, zLayer + 0.02f);

            sb.Draw(secondBodyTexture,
                ConvertUnits.ToDisplayUnits(this.secondBody.Position), null,
                Color.White, this.secondBody.Rotation,
                new Vector2(secondBodyTexture.Width/2, secondBodyTexture.Height/2),
                1.0f, SpriteEffects.None, zLayer + 0.01f);

            //  TODO : Replace the rectangle sizes when we have a final end piece texture. + SpinAssist.ModifyVectorByOrientation(new Vector2(0, 5), this._orientation)
            sb.Draw(endBodyTexture, ConvertUnits.ToDisplayUnits(this.endBody.Position),
                null, _tint, this.endBody.Rotation,
                this.Origin,
                0.98f, SpriteEffects.None, zLayer + 0.03f);

#if Development
            //  The length of the limit can be set for pixels if it's entered as sim'd units.
            //sb.Draw(Screen_Manager.BlackPixel, new Rectangle(
            //    (int)ConvertUnits.ToDisplayUnits(this.Body.Position.X),
            //    (int)ConvertUnits.ToDisplayUnits(this.Body.Position.Y) - (int)(this.Texture.Width * 2),
            //    5, (int)this.Texture.Width * 2),
            //    null, Color.Red, 0.0f, Vector2.Zero, SpriteEffects.None, 0.0f);
            sb.DrawString(Fonts.DebugFont, "Translation: " + this._prismaticJoint.JointTranslation.ToString(), this.Position + new Vector2(200, -60), Color.Red);
            sb.DrawString(Fonts.DebugFont, "UpL: " + this._prismaticJoint.UpperLimit.ToString() + ". LoL: " + this._prismaticJoint.LowerLimit.ToString(), this.Position + new Vector2(200, -45), Color.Red);
            sb.DrawString(Fonts.DebugFont, "Enabled: " + this._prismaticJoint.MotorEnabled + ". Speed: " + this._prismaticJoint.MotorSpeed, this.Position + new Vector2(200, -30), Color.Red);
            sb.DrawString(Fonts.DebugFont, "Elapsed: " + this._elapsedTimer + ". PauseTime: " + this.TimeToReverse, this.Position + new Vector2(200, -15), Color.Red);
#endif
        }
#endif
        #endregion

        #region Private Methods

        void CreateBodies(World world)
        {
#if EDITOR

#else

            Vector2 baseTexture = new Vector2(ConvertUnits.ToSimUnits(this.Texture.Width), ConvertUnits.ToSimUnits(this.Texture.Height));
            Vector2 Texture1 = new Vector2(ConvertUnits.ToSimUnits(this.firstBodyTexture.Width), ConvertUnits.ToSimUnits(this.firstBodyTexture.Height));
            Vector2 Texture2 = new Vector2(ConvertUnits.ToSimUnits(this.secondBodyTexture.Width), ConvertUnits.ToSimUnits(this.secondBodyTexture.Height));
            Vector2 endTexture = new Vector2(ConvertUnits.ToSimUnits(this.endBodyTexture.Width), ConvertUnits.ToSimUnits(this.endBodyTexture.Height));

            Vector2 offset = SpinAssist.ModifyVectorByOrientation(new Vector2(0, -100), _orientation);
            Vector2 axis = SpinAssist.ModifyVectorByOrientation(new Vector2(0, -1), _orientation);

            //  Main body
            this.Body = BodyFactory.CreateRectangle(world, baseTexture.X, baseTexture.Y, ConvertUnits.ToSimUnits(20.0f));
            this.Body.Rotation = SpinAssist.RotationByOrientation(_orientation);
            this.Body.Position = ConvertUnits.ToSimUnits(Position);
            this.Body.Friction = 1.0f;
            this.Body.Restitution = 0.0f;
            this.Body.BodyType = BodyType.Static;

            //  First out of main
            this.firstBody = BodyFactory.CreateRectangle(world, Texture1.X, Texture1.Y, ConvertUnits.ToSimUnits(10.0f));
            this.firstBody.Rotation = SpinAssist.RotationByOrientation(_orientation);
            this.firstBody.Position = ConvertUnits.ToSimUnits(Position);
            this.firstBody.Friction = 1.0f;
            this.firstBody.BodyType = BodyType.Dynamic;

            this.secondBody = BodyFactory.CreateRectangle(world, Texture2.X, Texture2.Y, ConvertUnits.ToSimUnits(10.0f));
            this.secondBody.Rotation = SpinAssist.RotationByOrientation(_orientation);
            this.secondBody.Position = ConvertUnits.ToSimUnits(Position);
            this.secondBody.Friction = 1.0f;
            this.secondBody.BodyType = BodyType.Dynamic;

            //  End piece
            //this.endBody = BodyFactory.CreateRectangle(world, ConvertUnits.ToSimUnits(endTexture.Width * 0.8), ConvertUnits.ToSimUnits(endTexture.Height * 0.8), ConvertUnits.ToSimUnits(10f));

            TexVertOutput input = SpinAssist.TexToVert(world, endBodyTexture, ConvertUnits.ToSimUnits(10.0f), 1.0f);

            this._origin = input.Origin;

            this.endBody = input.Body;
            this.endBody.Rotation = SpinAssist.RotationByOrientation(_orientation);
            this.endBody.Position = ConvertUnits.ToSimUnits(Position + offset);
            this.endBody.Restitution = 0.0f;
            this.endBody.Friction = 1.0f;
            this.endBody.BodyType = BodyType.Dynamic;

            this._prismaticJoint = JointFactory.CreateFixedPrismaticJoint(world, this.endBody, ConvertUnits.ToSimUnits(Position), axis);
            this._prismaticJoint.UpperLimit = Texture1.Y + Texture2.Y;
            this._prismaticJoint.LowerLimit = -(endTexture.Y / 2) + ConvertUnits.ToSimUnits(40);   //  Give a 25 pixel offset on the y when UpIs == Up
            this._prismaticJoint.LimitEnabled = true;
            this._prismaticJoint.MotorEnabled = true;
            this._prismaticJoint.MaxMotorForce = float.MaxValue;

            this.firstBodyJoint = JointFactory.CreateFixedPrismaticJoint(world, this.firstBody, ConvertUnits.ToSimUnits(Position), axis);
            this.firstBodyJoint.UpperLimit = Texture1.Y * 2;
            this.firstBodyJoint.LowerLimit = -(Texture1.Y / 2) + ConvertUnits.ToSimUnits(75);   //  Give a 25 pixel offset on the y when UpIs == Up
            this.firstBodyJoint.LimitEnabled = true;
            this.firstBodyJoint.MotorEnabled = true;
            this.firstBodyJoint.MaxMotorForce = float.MaxValue;

            this.secondBodyJoint = JointFactory.CreateFixedPrismaticJoint(world, this.secondBody, ConvertUnits.ToSimUnits(Position), axis);
            this.secondBodyJoint.UpperLimit = Texture2.Y;
            this.secondBodyJoint.LowerLimit = -(Texture2.Y / 2) + ConvertUnits.ToSimUnits(70);   //  Give a 25 pixel offset on the y when UpIs == Up
            this.secondBodyJoint.LimitEnabled = true;
            this.secondBodyJoint.MotorEnabled = true;
            this.secondBodyJoint.MaxMotorForce = float.MaxValue;

            //this.endSecondJoint = JointFactory.CreatePrismaticJoint(world, endBody, firstBody, Vector2.Zero, axis);
            //this.endSecondJoint.MaxMotorForce = float.MaxValue;
            //this.endSecondJoint.LimitEnabled = true;

            this.Body.IgnoreCollisionWith(this.firstBody);
            this.Body.IgnoreCollisionWith(this.secondBody);
            this.Body.IgnoreCollisionWith(this.endBody);
            this.firstBody.IgnoreCollisionWith(this.endBody);
            this.firstBody.IgnoreCollisionWith(this.secondBody);
            this.secondBody.IgnoreCollisionWith(this.endBody);
#endif

        }

        void HandleExtraJoint(GameTime gameTime, FixedPrismaticJoint joint)
        {
#if EDITOR

#else
            if ((joint.JointTranslation >= joint.UpperLimit && !this.MovingToStart) ||
                (joint.JointTranslation <= joint.LowerLimit && this.MovingToStart))
            {
                if (joint.MotorSpeed != 0)
                {
                    joint.MotorSpeed = 0.0f;
                }
            }
            else
            {
                if (joint.MotorSpeed != MotorSpeed)
                {
                    joint.MotorSpeed = MotorSpeed;
                }
            }
#endif
        }
        #endregion
    }
}
