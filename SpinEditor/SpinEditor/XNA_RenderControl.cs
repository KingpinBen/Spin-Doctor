﻿using System;
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
using Microsoft.Xna.Framework;
using GameLibrary;
using FarseerPhysics;
using GameLibrary.Graphics;
using GameLibrary.Helpers;
using GameLibrary.GameLogic.Objects;
using GameLibrary.Graphics.Drawing;

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

        private Texture2D devCharacter;

        public bool bDoNotDraw = false;

        #endregion

        #region Properties

        public bool HideOverlay { get; set; }
        public bool HideCoordinates { get; set; }
        public bool HideMovementPath { get; set; }
        public bool HideGrid { get; set; }
        public bool HidePlayerSpawn { get; set; }
        public bool HideEventTargets { get; set; }
        public bool HideObjectNames { get; set; }

        public Vector2 levelDimensions { get; set; }
        public ContentManager contentMan { get; set; }
        public Camera2D Camera { get; set; }
        public Form1 form1 { get; set; }
        public Texture2D levelBackground { get; set; }

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
            Vector2 offset = new Vector2(((GraphicsDevice.Viewport.Width * 0.5f) / Camera.Zoom) - Camera.Pos.X, ((GraphicsDevice.Viewport.Height * 0.5f) / Camera.Zoom) - Camera.Pos.Y);

            if (!bDoNotDraw)
            {
                UpdateTime();

                GraphicsDevice.Clear(Color.CornflowerBlue);

                List<Decal> decalList = STATIC_EDITOR_MODE.levelInstance.DecalManager.DecalList;
                List<NodeObject> objectsList = STATIC_EDITOR_MODE.levelInstance.ObjectsList;
                List<ObjectIndex> selectedList = STATIC_EDITOR_MODE.selectedObjectIndices;

                #region Objects and selection overlay
                spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, SamplerState.AnisotropicWrap, null, null, null, Camera.get_transformation(new Vector2(this.Width, this.Height)));

                #region Draw Generics
                if (levelBackground != null)
                {
                    spriteBatch.Draw(levelBackground, new Rectangle(-(int)(levelDimensions.X * 0.5f), -(int)(levelDimensions.Y * 0.5f), (int)levelDimensions.X, (int)levelDimensions.Y),
                        new Rectangle(0, 0, (int)this.levelDimensions.X, (int)this.levelDimensions.Y), Color.White, 0.0f, Vector2.Zero, SpriteEffects.None, 1.0f);
                }

                if (!HidePlayerSpawn)
                {
                    spriteBatch.Draw(devCharacter, STATIC_EDITOR_MODE.levelInstance.PlayerSpawnLocation,
                    null, Color.White, 0.0f, new Vector2(this.devCharacter.Width, this.devCharacter.Height) * 0.5f,
                    0.43f, SpriteEffects.None, 0.3f);
                }

                #endregion

                #region Draw PhysicsObjects and Decals
                if (objectsList.Count > 0)
                {
                    for (int i = objectsList.Count - 1; i >= 0; i--)
                    {
                        objectsList[i].Draw(spriteBatch);
                    }
                }

                if (decalList.Count > 0)
                {
                    for (int i = decalList.Count - 1; i >= 0; i--)
                    {
                        decalList[i].Draw(spriteBatch);
                    }
                }

                #endregion

                #region Draw object overlay

                if (!HideOverlay)
                {
                    for (int i = selectedList.Count - 1; i >= 0; i--)
                    {
                        int index = selectedList[i].Index;

                        if (selectedList[i].Type == OBJECT_TYPE.Physics)
                        {
                            spriteBatch.Draw(debugOverlay,
                                                objectsList[index].Position,
                                                new Rectangle(0, 0,
                                                    (int)objectsList[index].Width,
                                                    (int)objectsList[index].Height),
                                                Color.Green * 0.3f, 0.0f, new Vector2(objectsList[index].Width, objectsList[index].Height) * 0.5f,
                                                1.0f, SpriteEffects.None, 0.001f);
                        }
                        else
                        {
                            float width = decalList[index].Width * decalList[index].Scale;
                            float height = decalList[index].Height * decalList[index].Scale;

                            spriteBatch.Draw(debugOverlay,
                                        decalList[index].Position,
                                        new Rectangle(0, 0, (int)width, (int)height),
                                        Color.Green * 0.3f, decalList[index].Rotation,
                                        new Vector2(width, height) * 0.5f, 1.0f, SpriteEffects.None, 0.001f);
                        }
                    }
                }

                #endregion

                #region Draw Names

                if (!HideObjectNames)
                {
                    for (int i = objectsList.Count - 1; i >= 0; i--)
                    {
                        NodeObject obj = objectsList[i];

                        if (obj.Name != null || obj.Name != "")
                        {
                            Vector2 textOrigin = font.MeasureString(obj.Name) * 0.5f;

                            spriteBatch.DrawString(font, obj.Name, obj.Position, Color.White, 0.0f, textOrigin, 1.5f, SpriteEffects.None, 0.0f);
                        }
                    }
                }

                #endregion

                spriteBatch.End();
                #endregion

                #region Primitives
                //  Primitive batch calls need to be made OUTSIDE of a spriteBatch,
                //  otherwise it causes horrible FPS problems.
                //  Therefore, we'll do it afterwards and apply a basic transform 
                //  on their positions using the camera.
                primBatch.Begin(PrimitiveType.LineList);
                
                #region Movement paths
                if (!HideMovementPath)
                {
                    
                    for (int i = 0; i < objectsList.Count; i++)
                    {
                        Type t = objectsList[i].GetType();
                        if (t.BaseType == typeof(DynamicObject))
                        {
                            DynamicObject dyObj = (DynamicObject)objectsList[i];
                            primBatch.AddVertex((dyObj.Position + offset) * Camera.Zoom, Color.Red);
                            primBatch.AddVertex((dyObj.EndPosition + offset) * Camera.Zoom, Color.Red);

                        }
                        else if (t == typeof(Rope))
                        {
                            Rope ropeObj = (Rope)objectsList[i];
                            primBatch.AddVertex((ropeObj.Position + offset) * Camera.Zoom, Color.Red);
                            primBatch.AddVertex((ropeObj.EndPosition + offset) * Camera.Zoom, Color.Red);
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

                #region Event Targets

                if (!HideEventTargets)
                {
                    NodeObject obj = new NodeObject();

                    for (int i = 0; i < objectsList.Count; i++)
                    {
                        obj = objectsList[i];

                        //  Check if the object has a name (it can't use events if it doesn't).
                        if (obj.Name != null && obj.Name != "")
                        {

                            //  Check it has some events to target.
                            if (obj.EventList.Count > 0)
                            {

                                //  For the next part we need to check the name of each target
                                //  in the event list and compare it with objects in the objectList
                                for (int j = 0; j < obj.EventList.Count; j++)
                                {
                                    for (int x = 0; x < objectsList.Count; x++)
                                    {

                                        //  Check if the name of the object in the objectList matches
                                        //  the target
                                        if (obj.EventList[j].TargetName == objectsList[x].Name)
                                        {
                                            //  It is our target, so draw it.
                                            primBatch.AddVertex((obj.Position + offset) * Camera.Zoom, Color.Pink);
                                            primBatch.AddVertex((objectsList[x].Position + offset) * Camera.Zoom, Color.Pink);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                #endregion

                primBatch.End();
                #endregion

                #region Screen Rotation Point

                spriteBatch.Begin();

                spriteBatch.Draw(crosshair,
                                new Vector2((GraphicsDevice.Viewport.Width / 2 / Camera.Zoom) - Camera.Pos.X - (crosshair.Width / 2 / Camera.Zoom),
                                            (GraphicsDevice.Viewport.Height / 2 / Camera.Zoom) - Camera.Pos.Y - (crosshair.Height / 2 / Camera.Zoom)) * Camera.Zoom,
                                            Color.White * 0.2f);

                spriteBatch.End();
                #endregion

                #region Text Coordinates



                if (!HideCoordinates)
                {
                    spriteBatch.Begin();

                    spriteBatch.DrawString(font, "Level Co-ords: ",
                        new Vector2(0, 0),
                        Color.White);
                    spriteBatch.DrawString(font, worldCoOrds,
                        new Vector2(0, 20),
                        Color.White);

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
