using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WinFormsGraphicsDevice;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;
using GameLibrary.Assists;
using Microsoft.Xna.Framework;
using GameLibrary;
using FarseerPhysics;
using GameLibrary.Objects;

namespace SpinEditor
{
    public class XNA_RenderControl : GraphicsDeviceControl
    {
        #region Fields
        Stopwatch elapsed = new Stopwatch();
        Stopwatch total = new Stopwatch();
        Timer timer;

        GameTime gameTime; //create the game time using the above values

        public ContentManager contentMan;
        SpriteBatch spriteBatch;
        SpriteFont font;
        Random randomizer;
        public Color backgroundCol = Color.CornflowerBlue;
        private Texture2D debugOverlay;
        public Texture2D levelBackground { get; set; }
        public Vector2 levelDimensions { get; set; }

        string localCoOrds = "";
        string worldCoOrds = "";

        public bool bDoNotDraw = false;
        bool hideOverlay;

        public Camera2D Camera { get; set; }

        private Texture2D crosshair;

        private PrimitiveBatch grid;
        private float xySpacing;
        private int xLineCount;
        private int yLineCount;
        private float xOffset;
        private float yOffset;

        private PrimitiveBatch movements;

        #endregion

        #region Init
        /// <summary>
        /// Initializes the control, creating the ContentManager
        /// and using it to load a SpriteFont
        /// </summary>
        protected override void Initialize()
        {
            this.SizeChanged += WindowChangedSize;
            this.ClientSizeChanged += WindowChangedSize;

            contentMan = new ContentManager(Services, "Content");
            STATIC_EDITOR_MODE.contentMan = this.contentMan;
            spriteBatch = new SpriteBatch(GraphicsDevice);
            font = contentMan.Load<SpriteFont>("hudFont");

            randomizer = new Random();

            Camera = new Camera2D();
            Camera.Pos = new Vector2(0, 0);


            //  Hook the idle event to constantly redraw our animation
            //  Application.Idle += delegate { Invalidate(); };

            debugOverlay = contentMan.Load<Texture2D>("Assets/Sprites/Basics/BlankPixel");

            //  hook the mouse to the XNA graphic display control
            if (Mouse.WindowHandle != this.Handle)
                Mouse.WindowHandle = this.Handle;

            crosshair = contentMan.Load<Texture2D>("9pxCrosshair");

            grid = new PrimitiveBatch(GraphicsDevice);
            movements = new PrimitiveBatch(GraphicsDevice);

            RefreshGrid();

            total.Start();
            timer = new Timer();
            //  Lock to 60fps max.
            timer.Interval = (int)((1.0f / 60f) * 1000);
            timer.Tick += new EventHandler(tick);
            timer.Start();
        }
        #endregion

        #region Updates
        public void UpdateCoOrds()
        {
            Microsoft.Xna.Framework.Point topLeftPoint = Camera.GetTopLeftCameraPoint(GraphicsDevice);

            localCoOrds = "X: " + Mouse.GetState().X +
                        "  Y: " + Mouse.GetState().Y;

            worldCoOrds = "X: " + (int)(Mouse.GetState().X / Camera.Zoom  + topLeftPoint.X) +
                        "  Y: " + (int)(Mouse.GetState().Y / Camera.Zoom  + topLeftPoint.Y);

        }

        void UpdateTime()
        {
            RefreshGrid();

            // create the game time using these values
            gameTime = new GameTime(total.Elapsed, elapsed.Elapsed, false);

            elapsed.Reset();
            elapsed.Start();

            STATIC_EDITOR_MODE.world.Step((float)(gameTime.ElapsedGameTime.TotalMilliseconds * 0.001));

            STATIC_EDITOR_MODE.oldState = STATIC_EDITOR_MODE.keyState;
            STATIC_EDITOR_MODE.keyState = Keyboard.GetState();

            if (STATIC_EDITOR_MODE.keyState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.F1) && STATIC_EDITOR_MODE.oldState.IsKeyUp(Microsoft.Xna.Framework.Input.Keys.F1))
            {
                hideOverlay = !hideOverlay;
            }

            if (STATIC_EDITOR_MODE.keyState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Q))
            {
                Camera.Zoom += 0.01f;
            }

            if (STATIC_EDITOR_MODE.keyState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.E))
            {
                Camera.Zoom -= 0.01f;
            }
        }

        void tick(object sender, EventArgs e)
        {
            this.Invalidate();

        }
        #endregion

        #region Draw
        /// <summary>
        /// Draws the control, using SpriteBatch, PrimitiveBatch and SpriteFont.
        /// </summary>
        protected override void Draw()
        {
            if (!bDoNotDraw)
            {
                GraphicsDevice.Clear(backgroundCol);

                #region Objects and selection overlay
                spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, SamplerState.PointWrap, null, null, null, Camera.get_transformation(new Vector2(this.Width, this.Height)));

                if (levelBackground != null)
                {
                    spriteBatch.Draw(levelBackground, new Rectangle(-(int)levelDimensions.X / 2, -(int)levelDimensions.Y / 2, (int)levelDimensions.X, (int)levelDimensions.Y),
                    new Rectangle(0, 0, (int)this.levelDimensions.X, (int)this.levelDimensions.Y), Color.White, 0.0f, Vector2.Zero, SpriteEffects.None, 1.0f);
                }

                if (STATIC_EDITOR_MODE.levelInstance.PhysicsObjectsList.Count > 0)
                {
                    Microsoft.Xna.Framework.Color color = Microsoft.Xna.Framework.Color.Red * 0.0f;

                    for (int i = STATIC_EDITOR_MODE.levelInstance.PhysicsObjectsList.Count; i > 0; i--)
                    {
                        STATIC_EDITOR_MODE.levelInstance.PhysicsObjectsList[i - 1].Draw(spriteBatch);

                        if (hideOverlay)
                        {
                            continue;
                        }

                        if (STATIC_EDITOR_MODE.selectedObjectIndices.Count > 0)
                        {
                            for (int j = 0; j < STATIC_EDITOR_MODE.selectedObjectIndices.Count; j++)
                            {
                                if (STATIC_EDITOR_MODE.selectedObjectIndices[j].Index != i - 1 || STATIC_EDITOR_MODE.selectedObjectIndices[j].Type != OBJECT_TYPE.Physics)
                                    color = Microsoft.Xna.Framework.Color.Red * 0.0f;
                                else
                                {
                                    color = Microsoft.Xna.Framework.Color.Green * 0.8f;
                                    break;
                                }
                            }
                        }

                        //Type t = STATIC_EDITOR_MODE.levelInstance.PhysicsObjectsList[i - 1].GetType();
                        //Color newcolor = Color.Red;
                        //spriteBatch.DrawString(font, t.BaseType.ToString(), STATIC_EDITOR_MODE.levelInstance.PhysicsObjectsList[i - 1].Position, newcolor);

                        spriteBatch.Draw(debugOverlay, STATIC_EDITOR_MODE.levelInstance.PhysicsObjectsList[i - 1].Position,
                            new Rectangle(0, 0, (int)STATIC_EDITOR_MODE.levelInstance.PhysicsObjectsList[i - 1].Width, (int)STATIC_EDITOR_MODE.levelInstance.PhysicsObjectsList[i - 1].Height),
                            color, 0f, STATIC_EDITOR_MODE.levelInstance.PhysicsObjectsList[i - 1].Origin, 1.0f, SpriteEffects.None, 0f);
                    }
                }
                spriteBatch.End();
                #endregion

                #region Movement paths
                movements.Begin(PrimitiveType.LineList);
                {
                    Vector2 offset = new Vector2((GraphicsDevice.Viewport.Width / 2 / Camera.Zoom) - Camera.Pos.X, (GraphicsDevice.Viewport.Height / 2 / Camera.Zoom) - Camera.Pos.Y);
                    for (int i = 0; i < STATIC_EDITOR_MODE.levelInstance.PhysicsObjectsList.Count; i++)
                    {
                        Type t = STATIC_EDITOR_MODE.levelInstance.PhysicsObjectsList[i].GetType();
                        if (t.BaseType == typeof(DynamicObject))
                        {
                            DynamicObject dyObj = (DynamicObject)STATIC_EDITOR_MODE.levelInstance.PhysicsObjectsList[i];
                            movements.AddVertex((dyObj.Position + offset) * Camera.Zoom, Color.Red);
                            movements.AddVertex((dyObj.EndPosition + offset) * Camera.Zoom, Color.Red);

                        }

                        if (t == typeof(Rope))
                        {
                            Rope ropeObj = (Rope)STATIC_EDITOR_MODE.levelInstance.PhysicsObjectsList[i];
                            movements.AddVertex((ropeObj.Position + offset) * Camera.Zoom, Color.Green);
                            movements.AddVertex((ropeObj.EndPosition + offset) * Camera.Zoom, Color.Green);
                        }
                    }
                }
                movements.End();
                #endregion

                #region Grid
                grid.Begin(PrimitiveType.LineList);
                for (int i = 0; i < xLineCount + 2; i++)
                {
                    grid.AddVertex(new Vector2(xOffset + xySpacing * i, 0), Color.White * 0.2f);
                    grid.AddVertex(new Vector2(xOffset + xySpacing * i, GraphicsDevice.Viewport.Height), Color.White * 0.2f);

                }
                for (int i = 0; i < yLineCount + 2; i++)
                {
                    grid.AddVertex(new Vector2(0, yOffset + xySpacing * i), Color.White * 0.2f);
                    grid.AddVertex(new Vector2(GraphicsDevice.Viewport.Width, yOffset + xySpacing * i), Color.White * 0.2f);
                }
                grid.End();

                #endregion

                #region Screen Rotation Point
                //if (STATIC_EDITOR_MODE.levelInstance.CanLevelRotate)
                //{
                spriteBatch.Begin();
                spriteBatch.Draw(crosshair,
                                new Vector2((GraphicsDevice.Viewport.Width / 2 / Camera.Zoom) - Camera.Pos.X - (crosshair.Width / 2 / Camera.Zoom),
                                            (GraphicsDevice.Viewport.Height / 2 / Camera.Zoom) - Camera.Pos.Y - (crosshair.Height / 2 / Camera.Zoom)) * Camera.Zoom,
                                            Color.White * 0.2f);
                    spriteBatch.End();
                //}
                #endregion

                #region Text
                spriteBatch.Begin();
                spriteBatch.DrawString(font, "Local Co-ords: ",
                    Microsoft.Xna.Framework.Vector2.Zero,
                    Microsoft.Xna.Framework.Color.White);

                spriteBatch.DrawString(font, localCoOrds,
                    new Microsoft.Xna.Framework.Vector2(0, 20),
                    Microsoft.Xna.Framework.Color.White);

                spriteBatch.DrawString(font, "World Co-ords: ",
                    new Microsoft.Xna.Framework.Vector2(0, 40),
                    Microsoft.Xna.Framework.Color.White);

                spriteBatch.DrawString(font, worldCoOrds,
                    new Microsoft.Xna.Framework.Vector2(0, 60),
                    Microsoft.Xna.Framework.Color.White);

                spriteBatch.DrawString(font, "x: " + this.Width + "   y: " + this.Height,
                    new Microsoft.Xna.Framework.Vector2(0, 80),
                    Microsoft.Xna.Framework.Color.White);

                if (gameTime != null)
                {
                    spriteBatch.DrawString(font, gameTime.TotalGameTime.Seconds.ToString(),
                        new Microsoft.Xna.Framework.Vector2(0, 100),
                        Microsoft.Xna.Framework.Color.White);
                }

                spriteBatch.End();
                #endregion
                
                //Draw_DebugData();

                UpdateTime();
            }
        }
        #endregion

        #region Update Grid Methods
        void WindowChangedSize(object sender, EventArgs e)
        {
            grid = new GameLibrary.Assists.PrimitiveBatch(this.GraphicsDevice);
        }

        void RefreshGrid()
        {
            xySpacing = 32 * Camera.Zoom;
            xLineCount = (int)Math.Ceiling(this.GraphicsDevice.Viewport.Width / xySpacing);
            yLineCount = (int)Math.Ceiling(this.GraphicsDevice.Viewport.Height / xySpacing);
            xOffset = (this.GraphicsDevice.Viewport.Width / 2) - (xLineCount / 2 * xySpacing) - ((Camera.Pos.X * Camera.Zoom) % xySpacing);
            yOffset = (this.GraphicsDevice.Viewport.Height / 2) - (yLineCount / 2 * xySpacing) - ((Camera.Pos.Y * Camera.Zoom) % xySpacing);
        }
        #endregion
    }
}
