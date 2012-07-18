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
using GameLibrary.Graphics;

namespace SpinEditor
{
    public class XNA_RenderControl : GraphicsDeviceControl
    {
        #region Fields

        Stopwatch elapsed = new Stopwatch();
        Stopwatch total = new Stopwatch();
        Timer timer;
        GameTime gameTime; //create the game time using the above values
        SpriteBatch spriteBatch;
        Random randomizer;
        Texture2D debugOverlay;
        Texture2D crosshair;
        PrimitiveBatch primBatch;
        SpriteFont font;

        int xLineCount;
        int yLineCount;
        float xOffset;
        float yOffset;
        float xySpacing;
        string localCoOrds = "";
        string worldCoOrds = "";

        public Texture2D levelBackground { get; set; }
        private Texture2D devCharacter;
        public Vector2 levelDimensions { get; set; }
        public ContentManager contentMan { get; set; }
        public Camera2D Camera { get; set; }
        public Form1 form1 { get; set; }
        public bool bDoNotDraw = false;

        #endregion

        #region Properties

        public bool HideOverlay { get; set; }
        public bool HideCoordinates { get; set; }
        public bool HideMovementPath { get; set; }
        public bool HideGrid { get; set; }
        public bool HidePlayerSpawn { get; set; }

        #endregion

        #region Init
        /// <summary>
        /// Initializes the control, creating the ContentManager
        /// and using it to load a SpriteFont
        /// </summary>
        protected override void Initialize()
        {
            //  Initialize,
            contentMan = new ContentManager(Services, "Content");
            spriteBatch = new SpriteBatch(GraphicsDevice);
            primBatch = new PrimitiveBatch(GraphicsDevice);
            randomizer = new Random();
            Camera = new Camera2D();

            
            //  hook the mouse to the XNA graphic display control,
            if (Mouse.WindowHandle != this.Handle)
                Mouse.WindowHandle = this.Handle;

            //  Load some textures and fonts,
            debugOverlay = contentMan.Load<Texture2D>("Assets/Images/Basics/BlankPixel");
            crosshair = contentMan.Load<Texture2D>("Assets/Other/Dev/9pxCrosshair");
            devCharacter = contentMan.Load<Texture2D>("Assets/Other/Dev/devHarland");
            font = contentMan.Load<SpriteFont>("Assets/Fonts/Debug");

            //  Setup the grid initially
            RefreshGrid();

            //  Set up tick,
            timer = new Timer();
            timer.Interval = (int)((1.0f / 60f) * 1000);//  Lock to 60fps max.
            timer.Tick += new EventHandler(tick);
            timer.Start();
            total.Start();
        }

        
        #endregion

        #region Updates
        public void UpdateCoOrds()
        {
            Point topLeftPoint = Camera.GetTopLeftCameraPoint(GraphicsDevice);

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

            form1.Update(gameTime);

            elapsed.Reset();
            elapsed.Start();
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
                UpdateTime();

                GraphicsDevice.Clear(Color.CornflowerBlue);

                #region Objects and selection overlay
                spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, SamplerState.PointWrap, null, null, null, Camera.get_transformation(new Vector2(this.Width, this.Height)));

                #region Draw Generics
                if (levelBackground != null)
                {
                    spriteBatch.Draw(levelBackground, new Rectangle(-(int)levelDimensions.X / 2, -(int)levelDimensions.Y / 2, (int)levelDimensions.X, (int)levelDimensions.Y),
                        new Rectangle(0, 0, (int)this.levelDimensions.X, (int)this.levelDimensions.Y), Color.White, 0.0f, Vector2.Zero, SpriteEffects.None, 1.0f);
                }

                if (!HidePlayerSpawn)
                {
                    spriteBatch.Draw(devCharacter, STATIC_EDITOR_MODE.levelInstance.PlayerSpawnLocation,
                    null, Color.White, 0.0f, new Vector2(this.devCharacter.Width / 2, this.devCharacter.Height / 2),
                    0.43f, SpriteEffects.None, 0.3f);
                }
                #endregion

                #region Draw PhysicsObjects and Decals
                if (STATIC_EDITOR_MODE.levelInstance.ObjectsList.Count > 0)
                {
                    for (int i = STATIC_EDITOR_MODE.levelInstance.ObjectsList.Count; i > 0; i--)
                    {
                        STATIC_EDITOR_MODE.levelInstance.ObjectsList[i - 1].Draw(spriteBatch);

                        #region Selection Draw
                        if (!HideOverlay && STATIC_EDITOR_MODE.selectedObjectIndices.Count > 0)
                        {
                            for (int j = 0; j < STATIC_EDITOR_MODE.selectedObjectIndices.Count; j++)
                            {
                                if (STATIC_EDITOR_MODE.selectedObjectIndices[j].Type == OBJECT_TYPE.Physics)
                                {
                                    if (STATIC_EDITOR_MODE.selectedObjectIndices[j].Index == i - 1)
                                    {
                                        spriteBatch.Draw(debugOverlay,
                                            new Rectangle((int)(STATIC_EDITOR_MODE.levelInstance.ObjectsList[i - 1].Position.X - STATIC_EDITOR_MODE.levelInstance.ObjectsList[i - 1].Width / 2),
                                                (int)(STATIC_EDITOR_MODE.levelInstance.ObjectsList[i - 1].Position.Y - STATIC_EDITOR_MODE.levelInstance.ObjectsList[i - 1].Height / 2),
                                                (int)(STATIC_EDITOR_MODE.levelInstance.ObjectsList[i - 1].Width),
                                                (int)(STATIC_EDITOR_MODE.levelInstance.ObjectsList[i - 1].Height)),
                                            new Rectangle(0, 0,
                                                (int)STATIC_EDITOR_MODE.levelInstance.ObjectsList[i - 1].Width,
                                                (int)STATIC_EDITOR_MODE.levelInstance.ObjectsList[i - 1].Height),
                                            Color.Green * 0.4f, 0f, Vector2.Zero, SpriteEffects.None, 0.0f);
                                    }
                                }
                            }
                        }
                        #endregion
                    }
                }

                if (STATIC_EDITOR_MODE.levelInstance.DecalManager.DecalList.Count > 0)
                {
                    for (int i = 0; i < STATIC_EDITOR_MODE.levelInstance.DecalManager.DecalList.Count; i++)
                    {
                        STATIC_EDITOR_MODE.levelInstance.DecalManager.DecalList[i].Draw(spriteBatch);

                        #region Selection Draw
                        if (!HideOverlay && STATIC_EDITOR_MODE.selectedObjectIndices.Count > 0)
                        {
                            for (int j = 0; j < STATIC_EDITOR_MODE.selectedObjectIndices.Count; j++)
                            {
                                if (STATIC_EDITOR_MODE.selectedObjectIndices[j].Type == OBJECT_TYPE.Decal)
                                {
                                    if (STATIC_EDITOR_MODE.selectedObjectIndices[j].Index == i)
                                    {
                                        float width = STATIC_EDITOR_MODE.levelInstance.DecalManager.DecalList[i].Width * STATIC_EDITOR_MODE.levelInstance.DecalManager.DecalList[i].Scale;
                                        float height = STATIC_EDITOR_MODE.levelInstance.DecalManager.DecalList[i].Height * STATIC_EDITOR_MODE.levelInstance.DecalManager.DecalList[i].Scale;

                                        spriteBatch.Draw(debugOverlay,
                                            STATIC_EDITOR_MODE.levelInstance.DecalManager.DecalList[i].Position,
                                            new Rectangle(0, 0, (int)width, (int)height),
                                            Color.Green * 0.3f, STATIC_EDITOR_MODE.levelInstance.DecalManager.DecalList[i].Rotation, new Vector2(width * 0.5f, height * 0.5f), 1.0f, SpriteEffects.None, 0.0f);
                                    }
                                }
                            }
                        }
                        #endregion
                    }
                }

                #endregion

                spriteBatch.End();
                #endregion

                #region Primitives

                primBatch.Begin(PrimitiveType.LineList);
                
                #region Movement paths
                if (!HideMovementPath)
                {
                    Vector2 offset = new Vector2((GraphicsDevice.Viewport.Width / 2 / Camera.Zoom) - Camera.Pos.X, (GraphicsDevice.Viewport.Height / 2 / Camera.Zoom) - Camera.Pos.Y);
                    for (int i = 0; i < STATIC_EDITOR_MODE.levelInstance.ObjectsList.Count; i++)
                    {
                        Type t = STATIC_EDITOR_MODE.levelInstance.ObjectsList[i].GetType();
                        if (t.BaseType == typeof(DynamicObject))
                        {
                            DynamicObject dyObj = (DynamicObject)STATIC_EDITOR_MODE.levelInstance.ObjectsList[i];
                            primBatch.AddVertex((dyObj.Position + offset) * Camera.Zoom, Color.Red);
                            primBatch.AddVertex((dyObj.EndPosition + offset) * Camera.Zoom, Color.Red);

                        }
                        else if (t == typeof(Rope))
                        {
                            Rope ropeObj = (Rope)STATIC_EDITOR_MODE.levelInstance.ObjectsList[i];
                            primBatch.AddVertex((ropeObj.Position + offset) * Camera.Zoom, Color.Green);
                            primBatch.AddVertex((ropeObj.EndPosition + offset) * Camera.Zoom, Color.Green);
                        }
                    }
                }
                #endregion

                #region Grid
                if (!HideGrid)
                {
                    for (int i = 0; i < xLineCount + 2; i++)
                    {
                        primBatch.AddVertex(new Vector2(xOffset + xySpacing * i, 0), Color.White * 0.2f);
                        primBatch.AddVertex(new Vector2(xOffset + xySpacing * i, GraphicsDevice.Viewport.Height), Color.White * 0.2f);

                    }
                    for (int i = 0; i < yLineCount + 2; i++)
                    {
                        primBatch.AddVertex(new Vector2(0, yOffset + xySpacing * i), Color.White * 0.2f);
                        primBatch.AddVertex(new Vector2(GraphicsDevice.Viewport.Width, yOffset + xySpacing * i), Color.White * 0.2f);
                    }
                }
                #endregion

                primBatch.End();
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
                if (!HideCoordinates)
                {
                    spriteBatch.Begin();
                    //spriteBatch.DrawString(font, "Screen Co-ords: ",
                    //     Vector2.Zero,
                    //     Color.White);
                    //spriteBatch.DrawString(font, localCoOrds,
                    //    new Vector2(0, 20),
                    //    Color.White);
                    spriteBatch.DrawString(font, "Level Co-ords: ",
                        new Vector2(0, 0),
                        Color.White);
                    spriteBatch.DrawString(font, worldCoOrds,
                        new Vector2(0, 20),
                        Color.White);

                    //spriteBatch.DrawString(font, "x: " + this.Width + "   y: " + this.Height,
                    //new Vector2(0, 80),
                    //Color.White);

                    spriteBatch.End();
                }
                #endregion
            }
        }
        #endregion

        #region Update Grid Methods
        public void WindowChangedSize(object sender, EventArgs e)
        {
            primBatch = new PrimitiveBatch(this.GraphicsDevice);
        }

        public void RefreshGrid()
        {
            xySpacing = 32 * Camera.Zoom;
            xLineCount = (int)Math.Ceiling(this.GraphicsDevice.Viewport.Width / xySpacing);
            yLineCount = (int)Math.Ceiling(this.GraphicsDevice.Viewport.Height / xySpacing);
            xOffset = (this.GraphicsDevice.Viewport.Width * 0.5f) - (xLineCount * 0.5f * xySpacing) - ((Camera.Pos.X * Camera.Zoom) % xySpacing);
            yOffset = (this.GraphicsDevice.Viewport.Height * 0.5f) - (yLineCount * 0.5f * xySpacing) - ((Camera.Pos.Y * Camera.Zoom) % xySpacing);
        }
        #endregion
    }
}
