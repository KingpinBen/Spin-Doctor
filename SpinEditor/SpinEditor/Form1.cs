#region Using Statements
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Xml;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Intermediate;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using GameLibrary;
using FarseerPhysics.Dynamics;
using GameLibrary.Graphics;
using GameLibrary.GameLogic.Objects;
using GameLibrary.GameLogic.Objects.Triggers;
using GameLibrary.Levels;
using GameLibrary.Helpers;
using GameLibrary.Graphics.Drawing;
#endregion

namespace SpinEditor
{
    public partial class Form1 : Form
    {
        #region Fields

        static List<ObjectIndex> lst_ObjectsUnderCursor;
        Vector2 mouseDown = Vector2.Zero;
        Vector2 mouseUp = Vector2.Zero;
        bool containsMouse = false;
        bool dragReleased = true;
        int amountOfTexturesNeeded;
        
        #endregion

        #region Properties
        public static List<ObjectIndex> ListThing
        {
            get { return lst_ObjectsUnderCursor; }
        }
        #endregion

        #region Constructor and Load

        public Form1()
        {
            InitializeComponent();
            
            //  Disable fullscreen as it messes up the grid.
            this.MaximizeBox = false;
            lst_ObjectsUnderCursor = new List<ObjectIndex>();

            hScrollBar1.Minimum = -2000;
            vScrollBar1.Minimum = -2000;
            hScrollBar1.Maximum = 2000;
            vScrollBar1.Maximum = 2000;

            //  Allows access to public methods from xna_renderControl, such as Update().
            xnA_RenderControl1.form1 = this;

            //  Hook some events,
            this.SizeChanged += xnA_RenderControl1.WindowChangedSize;
            this.ClientSizeChanged += xnA_RenderControl1.WindowChangedSize;
            this.MouseWheel += new MouseEventHandler(XNA_RenderControl_MouseWheel);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            listBox_Classes.SelectedIndex = -1;
            Align_Relative.SelectedItem = "Last Selected";

            NewFile();

            Handle_Property_Grid_Items();

            if (STATIC_EDITOR_MODE.levelInstance != null)
            {
                xnA_RenderControl1.bDoNotDraw = false;
            }
        }

        #endregion

        public void Update(GameTime gameTime)
        {
            CheckInput(gameTime);
        }

        #region Create an Object




        private void CreateObject(string Type, Vector2 Position)
        {
            string texloc0 = "";
            string texloc1 = "";
            string texloc2 = "";

            if (amountOfTexturesNeeded > 0)
            {
                if (assetLocTextBox1.Text.ToString() != "")
                    texloc0 = assetLocTextBox1.Text.ToString();
                else 
                    return;

                if (amountOfTexturesNeeded > 1)
                {
                    if (assetLocTextBox2.Text.ToString() != "")
                        texloc1 = assetLocTextBox2.Text.ToString();
                    else
                        return;

                    if (amountOfTexturesNeeded > 2)
                    {
                        if (assetLocTextBox3.Text.ToString() != "")
                            texloc2 = assetLocTextBox3.Text.ToString();
                        else
                            return;
                    }
                }
            }

            #region Object Types
            switch (Type)
            {
                case "Bounce Pad":
                    {
                        BouncePad bouncePad = new BouncePad();
                        bouncePad.Init(Position);
                        bouncePad.Load(xnA_RenderControl1.contentMan, STATIC_EDITOR_MODE.world);
                        STATIC_EDITOR_MODE.levelInstance.ObjectsList.Add(bouncePad);
                    }
                    break;
                case "Cushioned Platform":
                    {
                        CushionedPlatform plat = new CushionedPlatform();
                        plat.Init(Position, texloc0);
                        plat.Load(xnA_RenderControl1.contentMan, STATIC_EDITOR_MODE.world);
                        STATIC_EDITOR_MODE.levelInstance.ObjectsList.Add(plat);
                    }
                    break;
                case "Decal":
                    {
                        Decal decal = new Decal();
                        decal.Init(Position, texloc0);
                        decal.Load(xnA_RenderControl1.contentMan);
                        STATIC_EDITOR_MODE.levelInstance.DecalManager.DecalList.Add(decal);
                    }
                    break;
                case "Door":
                    {
                        Door door = new Door();
                        door.Init(Position, texloc0);
                        door.Load(xnA_RenderControl1.contentMan, STATIC_EDITOR_MODE.world);
                        STATIC_EDITOR_MODE.levelInstance.ObjectsList.Add(door);
                    }
                    break;
                case "Ladder":
                    {
                        Ladder ladder = new Ladder();
                        ladder.Init(Position, texloc0);
                        ladder.Load(xnA_RenderControl1.contentMan, STATIC_EDITOR_MODE.world);
                        STATIC_EDITOR_MODE.levelInstance.ObjectsList.Add(ladder);
                    }
                    break;
                case "Moving Platform":
                    {
                        MovingPlatform movPlat = new MovingPlatform();
                        movPlat.Init(Position, texloc0);
                        movPlat.Load(xnA_RenderControl1.contentMan, STATIC_EDITOR_MODE.world);
                        STATIC_EDITOR_MODE.levelInstance.ObjectsList.Add(movPlat);
                    }
                    break;
                case "Note":
                    {
                        Note shiny = new Note();
                        shiny.Init(Position, texloc0);
                        shiny.Load(xnA_RenderControl1.contentMan, STATIC_EDITOR_MODE.world);
                        STATIC_EDITOR_MODE.levelInstance.ObjectsList.Add(shiny);
                    }
                    break;
                case "One-Sided Platform":
                    {
                        OneSidedPlatform oneSidePlat = new OneSidedPlatform();
                        oneSidePlat.Init(Position);
                        oneSidePlat.Load(xnA_RenderControl1.contentMan, STATIC_EDITOR_MODE.world);
                        STATIC_EDITOR_MODE.levelInstance.ObjectsList.Add(oneSidePlat);
                    }
                    break;
                case "Particle Emitter":
                    {
                        ParticleEmitter emitter = new ParticleEmitter();
                        emitter.Init(Position, texloc0);
                        emitter.Load(xnA_RenderControl1.contentMan, STATIC_EDITOR_MODE.world);
                        STATIC_EDITOR_MODE.levelInstance.ObjectsList.Add(emitter);
                    }
                    break;
                case "Piston":
                    {
                        Piston piston = new Piston();
                        piston.Init(Position, texloc0, texloc1);
                        piston.Load(xnA_RenderControl1.contentMan, STATIC_EDITOR_MODE.world);
                        STATIC_EDITOR_MODE.levelInstance.ObjectsList.Add(piston);
                    }
                    break;
                case "Physics Object":
                    {
                        PhysicsObject phyobj = new PhysicsObject();
                        phyobj.Init(Position, texloc0);
                        phyobj.Load(xnA_RenderControl1.contentMan, STATIC_EDITOR_MODE.world);
                        STATIC_EDITOR_MODE.levelInstance.ObjectsList.Add(phyobj);
                    }
                    break;
                case "Pushing Platform":
                    {
                        PushingPlatform pushPlat = new PushingPlatform();
                        pushPlat.Init(Position, texloc0);
                        pushPlat.Load(xnA_RenderControl1.contentMan, STATIC_EDITOR_MODE.world);
                        STATIC_EDITOR_MODE.levelInstance.ObjectsList.Add(pushPlat);
                    }
                    break;
                case "Rope":
                    {
                        Rope rope = new Rope();
                        rope.Init(Position, texloc0, texloc1);
                        rope.Load(xnA_RenderControl1.contentMan, STATIC_EDITOR_MODE.world);
                        STATIC_EDITOR_MODE.levelInstance.ObjectsList.Add(rope);
                    }
                    break;
                case "Rotate Room Button":
                    {
                        RotateRoomButton rotRoomButton = new RotateRoomButton();
                        rotRoomButton.Init(Position, texloc0);
                        rotRoomButton.Load(xnA_RenderControl1.contentMan, STATIC_EDITOR_MODE.world);
                        STATIC_EDITOR_MODE.levelInstance.ObjectsList.Add(rotRoomButton);
                    }
                    break;
                case "Rotating Platform":
                    {
                        RotatingPlatform rotPlat = new RotatingPlatform();
                        rotPlat.Init(Position, texloc0);
                        rotPlat.Load(xnA_RenderControl1.contentMan, STATIC_EDITOR_MODE.world);
                        STATIC_EDITOR_MODE.levelInstance.ObjectsList.Add(rotPlat);
                    }
                    break;
                case "Saw":
                    {
                        Saw sawBlade = new Saw();
                        sawBlade.Init(Position, texloc0, texloc1);
                        sawBlade.Load(xnA_RenderControl1.contentMan, STATIC_EDITOR_MODE.world);
                        STATIC_EDITOR_MODE.levelInstance.ObjectsList.Add(sawBlade);
                    }
                    break;
                case "Spikes":
                    {
                        Spike spike = new Spike();
                        spike.Init(Position, texloc0);
                        spike.Load(xnA_RenderControl1.contentMan, STATIC_EDITOR_MODE.world);
                        STATIC_EDITOR_MODE.levelInstance.ObjectsList.Add(spike);
                    }
                    break;
                case "Sprite":
                    {
                        Sprite spriteObj = new Sprite();
                        spriteObj.Init(Position, texloc0);
                        spriteObj.Load(xnA_RenderControl1.contentMan, STATIC_EDITOR_MODE.world);
                        STATIC_EDITOR_MODE.levelInstance.ObjectsList.Add(spriteObj);
                    }
                    break;
                case "Static Object":
                    {
                        StaticObject staticObj = new StaticObject();
                        staticObj.Init(Position, texloc0);
                        staticObj.Load(xnA_RenderControl1.contentMan, STATIC_EDITOR_MODE.world);
                        STATIC_EDITOR_MODE.levelInstance.ObjectsList.Add(staticObj);
                    }
                    break;
                case "Trigger":
                    {
                        Trigger trigObj = new Trigger();
                        trigObj.Init(Position);
                        trigObj.Load(xnA_RenderControl1.contentMan, STATIC_EDITOR_MODE.world);
                        STATIC_EDITOR_MODE.levelInstance.ObjectsList.Add(trigObj);
                        break;
                    }
                default:
                    return;
            }
            #endregion

            switch (Type)
            {
                case "Decal":
                    {
                        if (!STATIC_EDITOR_MODE.keyboardCurrentState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.LeftShift) && !STATIC_EDITOR_MODE.keyboardCurrentState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.RightShift))
                        {
                            STATIC_EDITOR_MODE.selectedObjectIndices.Clear();
                        }
                        STATIC_EDITOR_MODE.selectedObjectIndices.Add(new ObjectIndex(OBJECT_TYPE.Decal, STATIC_EDITOR_MODE.levelInstance.DecalManager.DecalList.Count - 1, STATIC_EDITOR_MODE.levelInstance.DecalManager.DecalList[STATIC_EDITOR_MODE.levelInstance.DecalManager.DecalList.Count - 1].ZLayer));
                    }
                    break;
                default:
                    {
                        if (!STATIC_EDITOR_MODE.keyboardCurrentState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.LeftShift) && !STATIC_EDITOR_MODE.keyboardCurrentState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.RightShift))
                        {
                            STATIC_EDITOR_MODE.selectedObjectIndices.Clear();
                        }
                        STATIC_EDITOR_MODE.selectedObjectIndices.Add(new ObjectIndex(OBJECT_TYPE.Physics, STATIC_EDITOR_MODE.levelInstance.ObjectsList.Count - 1, STATIC_EDITOR_MODE.levelInstance.ObjectsList[STATIC_EDITOR_MODE.levelInstance.ObjectsList.Count - 1].ZLayer));
                    }
                    break;
            }

        }
        #endregion 

        #region Undo/Redo
        void Update_undoArray()
        {
            if (STATIC_EDITOR_MODE.arrayLength - 1 > STATIC_EDITOR_MODE.arrayIndex)
            {
                for (int i = STATIC_EDITOR_MODE.arrayLength - 1; STATIC_EDITOR_MODE.arrayLength - 1 > STATIC_EDITOR_MODE.arrayIndex; i--)
                {
                    STATIC_EDITOR_MODE.undoObjArray[i] = null;
                    STATIC_EDITOR_MODE.undoDecalArray[i] = null;
                    STATIC_EDITOR_MODE.arrayLength--;
                }
            }
            if (STATIC_EDITOR_MODE.arrayLength == STATIC_EDITOR_MODE.arrayMax)
            {
                for (int i = 0; i < STATIC_EDITOR_MODE.arrayMax - 1; i++)
                {
                    STATIC_EDITOR_MODE.undoObjArray[i] = STATIC_EDITOR_MODE.undoObjArray[i + 1];
                    STATIC_EDITOR_MODE.undoDecalArray[i] = STATIC_EDITOR_MODE.undoDecalArray[i + 1];

                }
                STATIC_EDITOR_MODE.undoObjArray[STATIC_EDITOR_MODE.arrayLength - 1] = null;
                STATIC_EDITOR_MODE.undoDecalArray[STATIC_EDITOR_MODE.arrayLength - 1] = null;
                STATIC_EDITOR_MODE.arrayLength--;
            }

            STATIC_EDITOR_MODE.undoObjArray[STATIC_EDITOR_MODE.arrayLength] = new NodeObject[STATIC_EDITOR_MODE.levelInstance.ObjectsList.Count];
            for (int i = 0; i < STATIC_EDITOR_MODE.levelInstance.ObjectsList.Count; i++)
            {
                STATIC_EDITOR_MODE.undoObjArray[STATIC_EDITOR_MODE.arrayLength][i] = STATIC_EDITOR_MODE.levelInstance.ObjectsList[i];
            }

            STATIC_EDITOR_MODE.undoDecalArray[STATIC_EDITOR_MODE.arrayLength] = new Decal[STATIC_EDITOR_MODE.levelInstance.DecalManager.DecalList.Count];
            for (int i = 0; i < STATIC_EDITOR_MODE.levelInstance.DecalManager.DecalList.Count; i++)
            {
                STATIC_EDITOR_MODE.undoDecalArray[STATIC_EDITOR_MODE.arrayLength][i] = STATIC_EDITOR_MODE.levelInstance.DecalManager.DecalList[i];
            }

            STATIC_EDITOR_MODE.arrayLength++;
            STATIC_EDITOR_MODE.arrayIndex = STATIC_EDITOR_MODE.arrayLength - 1;
        }

        void Undo()
        {
            if (STATIC_EDITOR_MODE.arrayIndex > 0)
            {
                STATIC_EDITOR_MODE.arrayIndex--;
                STATIC_EDITOR_MODE.levelInstance.ObjectsList = null;
                STATIC_EDITOR_MODE.levelInstance.ObjectsList = STATIC_EDITOR_MODE.undoObjArray[STATIC_EDITOR_MODE.arrayIndex].ToList();
                STATIC_EDITOR_MODE.levelInstance.DecalManager.DecalList = null;
                STATIC_EDITOR_MODE.levelInstance.DecalManager.DecalList = STATIC_EDITOR_MODE.undoDecalArray[STATIC_EDITOR_MODE.arrayIndex].ToList();
                STATIC_EDITOR_MODE.selectedObjectIndices.Clear();
                Handle_Property_Grid_Items();
            }
        }

        void Redo()
        {
            if (STATIC_EDITOR_MODE.arrayLength - 1 > STATIC_EDITOR_MODE.arrayIndex)
            {
                STATIC_EDITOR_MODE.arrayIndex++;
                STATIC_EDITOR_MODE.levelInstance.ObjectsList = STATIC_EDITOR_MODE.undoObjArray[STATIC_EDITOR_MODE.arrayIndex].ToList();
                STATIC_EDITOR_MODE.levelInstance.DecalManager.DecalList = STATIC_EDITOR_MODE.undoDecalArray[STATIC_EDITOR_MODE.arrayIndex].ToList();
                STATIC_EDITOR_MODE.selectedObjectIndices.Clear();
                Handle_Property_Grid_Items();
            }
        }

        #endregion

        #region Events

        #region View

        void ViewMenuHideOverlay_Click(object sender, EventArgs e)
        {
            ViewMenuHideOverlay.Checked = !ViewMenuHideOverlay.Checked;
            xnA_RenderControl1.HideOverlay = ViewMenuHideOverlay.Checked;
        }
        void ViewMenuHideCoordinates_Click(object sender, EventArgs e)
        {
            ViewMenuHideCoordinates.Checked = !ViewMenuHideCoordinates.Checked;
            xnA_RenderControl1.HideCoordinates = ViewMenuHideCoordinates.Checked;
        }
        void ViewMenuHideMovementPath_Click(object sender, EventArgs e)
        {
            ViewMenuHideMovementPath.Checked = !ViewMenuHideMovementPath.Checked;
            xnA_RenderControl1.HideMovementPath = ViewMenuHideMovementPath.Checked;
        }
        void ViewMenuHideGrid_Click(object sender, EventArgs e)
        {
            ViewMenuHideGrid.Checked = !ViewMenuHideGrid.Checked;
            xnA_RenderControl1.HideGrid = ViewMenuHideGrid.Checked;
        }

        private void ViewMenuHideSpawn_Click(object sender, EventArgs e)
        {
            ViewMenuHideSpawn.Checked = !ViewMenuHideSpawn.Checked;
            xnA_RenderControl1.HidePlayerSpawn = ViewMenuHideSpawn.Checked;
        }

        void ViewMenuHideEvents_Click(object sender, EventArgs e)
        {
            ViewMenuHideEvents.Checked = !ViewMenuHideEvents.Checked;
            xnA_RenderControl1.HideEventTargets = ViewMenuHideEvents.Checked;
        }

        #endregion

        #region File

        private void NewFileToolstripOption_Click(object sender, EventArgs e)
        {
            NewFile();
        }
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFile();
        }
        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CheckIfOpenLevel())
            {
                OpenFile();
            }            
        }
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        #endregion

        #region Edit

        void BUTTON_UNDO_Click(object sender, EventArgs e)
        {
            Undo();
        }
        void BUTTON_REDO_Click(object sender, EventArgs e)
        {
            Redo();
        }
        void undoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Undo();
        }
        void redoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Redo();
        }
        void deselectAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            STATIC_EDITOR_MODE.selectedObjectIndices.Clear();
        }

        #endregion

        #region Toolstrip

        void BUTTON_EDITOR_MODE_SELECT_Click(object sender, EventArgs e)
        {
            SwitchToSelectMode();
        }

        void BUTTON_EDITOR_MODE_MOVE_Click(object sender, EventArgs e)
        {
            SwitchToMoveMode();
        }

        void BUTTON_EDITOR_MODE_PLACE_Click(object sender, EventArgs e)
        {
            SwitchToPlaceMode();
        }

        void BUTTON_EDIT_ROOM_Click(object sender, EventArgs e)
        {
            SwitchToRoomProperties();
        }

        void BUTTON_DELETE_Click(object sender, EventArgs e)
        {
            DeleteSelectedObjects();
        }

        #endregion

        #region RenderControl Gain Focus
        /// <summary>
        /// We need the render control to regain focus after the screen has been selected otherwise it sticks to the last object.
        /// 
        /// Having it on mouseover would be too frustrating.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void xnA_RenderControl1_Click(object sender, EventArgs e)
        {
            this.xnA_RenderControl1.Focus();
        }
        #endregion

        #endregion

        #region Private Methods



        #region File



        void NewFile()
        {
            if (CheckIfOpenLevel())
            {
                STATIC_EDITOR_MODE.Setup();

                SwitchToSelectMode();

                bool canContinue = false;
                using (RoomSetupForm RoomForm = new RoomSetupForm())
                {
                    while (!canContinue)
                    {
                        DialogResult result = RoomForm.ShowDialog();
                        switch (result)
                        {
                            case (DialogResult.Yes):
                                {
                                    canContinue = OpenFile();
                                    if (!canContinue)
                                        continue;

                                    xnA_RenderControl1.bDoNotDraw = false;
                                }
                                break;
                            case (DialogResult.OK):
                                {
                                    canContinue = RoomForm.CheckIfComplete();
                                    if (!canContinue)
                                    {
                                        break;
                                    }

                                    //  Generate the correct roomsize for the background for size comparison
                                    Vector2 alteredRoomSize = RoomForm.roomSize;
                                    //  multiply by one paper unit:
                                    alteredRoomSize *= 79;
                                    xnA_RenderControl1.levelDimensions = alteredRoomSize;
                                    STATIC_EDITOR_MODE.levelInstance.RoomDimensions = alteredRoomSize;

                                    STATIC_EDITOR_MODE.levelInstance.RoomType = RoomForm.roomType;

                                    //  Load the background texture and attach it to the level.
                                    if (RoomForm.roomType == RoomType.Rotating)
                                    {
                                        STATIC_EDITOR_MODE.levelInstance.BackgroundFile = RoomForm.rearWall;
                                        xnA_RenderControl1.levelBackground = xnA_RenderControl1.contentMan.Load<Texture2D>(RoomForm.rearWall);
                                    }
                                    else
                                    {
                                        STATIC_EDITOR_MODE.levelInstance.BackgroundFile = String.Empty;
                                        xnA_RenderControl1.levelBackground = xnA_RenderControl1.contentMan.Load<Texture2D>("Assets/Images/Basics/BlankPixel");
                                    }


                                    //  If we're to be inserting initial walls, put them in.
                                    if (RoomForm.drawWallsCheckBox1.Checked)
                                    {
                                        //  Intialize the 4 boundaries

                                        StaticObject topWall = new StaticObject();
                                        StaticObject bottomWall = new StaticObject(); 
                                        StaticObject leftWall = new StaticObject();
                                        StaticObject rightWall = new StaticObject();

                                        topWall.Init(Vector2.Zero, "Assets/Images/Textures/Environment/platform_4");
                                        bottomWall.Init(Vector2.Zero, "Assets/Images/Textures/Environment/platform_4");
                                        leftWall.Init(Vector2.Zero, "Assets/Images/Textures/Environment/platform_4");
                                        rightWall.Init(Vector2.Zero, "Assets/Images/Textures/Environment/platform_4");

                                        //  First we need to load them so we can use their Texture values.

                                        topWall.Load(xnA_RenderControl1.contentMan, new World(Vector2.Zero));
                                        bottomWall.Load(xnA_RenderControl1.contentMan, new World(Vector2.Zero));
                                        leftWall.Load(xnA_RenderControl1.contentMan, new World(Vector2.Zero));
                                        rightWall.Load(xnA_RenderControl1.contentMan, new World(Vector2.Zero));

                                        //  Now we grab them for positioning

                                        float textureWidth = topWall.Texture.Width;
                                        float textureHeight = topWall.Texture.Height;

                                        topWall.Position = new Vector2(0, -alteredRoomSize.Y * 0.5f) - new Vector2(0,textureHeight * 0.5f - 4);
                                        bottomWall.Position = new Vector2(0, alteredRoomSize.Y * 0.5f) + new Vector2(0, textureHeight * 0.5f - 4);
                                        leftWall.Position = new Vector2(-alteredRoomSize.X * 0.5f, 0) - new Vector2(textureHeight * 0.5f, 0);
                                        rightWall.Position = new Vector2(alteredRoomSize.X * 0.5f, 0) + new Vector2(textureHeight * 0.5f, 0);

                                        //  Apply a high Zlayer so they make certain conditions make it look like
                                        //  a cube like boundaries.

                                        topWall.ZLayer = 0.2f;
                                        bottomWall.ZLayer = 0.2f;
                                        leftWall.ZLayer = 0.2f;
                                        rightWall.ZLayer = 0.2f;

                                        //  Set up the correct orientation for rotation.

                                        bottomWall.Orientation = GameLibrary.GameLogic.Objects.Orientation.Down;
                                        leftWall.Orientation = GameLibrary.GameLogic.Objects.Orientation.Left;
                                        rightWall.Orientation = GameLibrary.GameLogic.Objects.Orientation.Right;

                                        topWall.Width = bottomWall.Width = alteredRoomSize.X;
                                        leftWall.Height = rightWall.Height = alteredRoomSize.Y + (leftWall.Texture.Height * 2) - 8;

                                        //  Put them immediately back into the object list

                                        STATIC_EDITOR_MODE.levelInstance.ObjectsList.Add(topWall);
                                        STATIC_EDITOR_MODE.levelInstance.ObjectsList.Add(bottomWall);
                                        STATIC_EDITOR_MODE.levelInstance.ObjectsList.Add(leftWall);
                                        STATIC_EDITOR_MODE.levelInstance.ObjectsList.Add(rightWall);
                                    }

                                    //Update_undoArray();
                                }
                                break;
                            default:
                                {
                                    canContinue = true;
                                    Application.Exit();
                                }
                                break;
                        }
                    }
                }
            }
        }




        bool OpenFile()
        {
            openFileDialog1.Filter = "Xml files|*.xml";

            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                using (FileStream stream = new FileStream(openFileDialog1.FileName, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    using (XmlTextReader input = new XmlTextReader(stream))
                    {
                        xnA_RenderControl1.bDoNotDraw = true;

                        STATIC_EDITOR_MODE.Setup();

                        STATIC_EDITOR_MODE.levelInstance = IntermediateSerializer.Deserialize<Level>(input, null);

                        if (STATIC_EDITOR_MODE.levelInstance.RoomType == RoomType.Rotating)
                        {
                            xnA_RenderControl1.levelBackground = xnA_RenderControl1.contentMan.Load<Texture2D>(STATIC_EDITOR_MODE.levelInstance.BackgroundFile);
                        }
                        else
                        {
                            xnA_RenderControl1.levelBackground = xnA_RenderControl1.contentMan.Load<Texture2D>(Defines.BLANK_PIXEL);
                        }

                        
                        xnA_RenderControl1.levelDimensions = STATIC_EDITOR_MODE.levelInstance.RoomDimensions;

                        for (int i = 0; i < STATIC_EDITOR_MODE.levelInstance.ObjectsList.Count; i++)
                        {
                            STATIC_EDITOR_MODE.levelInstance.ObjectsList[i].Load(xnA_RenderControl1.contentMan, STATIC_EDITOR_MODE.world);
                        }

                        for (int i = 0; i < STATIC_EDITOR_MODE.levelInstance.DecalManager.DecalList.Count; i++)
                        {
                            STATIC_EDITOR_MODE.levelInstance.DecalManager.DecalList[i].Load(xnA_RenderControl1.contentMan);
                        }

                        xnA_RenderControl1.bDoNotDraw = false;

                        Update_undoArray();
                        
                        MessageBox.Show("Level Loaded!", "Level loaded info box", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return true;
                    }
                }
            }
            else
            {
                //  If no level is selected/found, it just goes back to main menu form.
                MessageBox.Show("No file selected.");
                return false;
            }
        }




        #region Check if Open Level
        /// <summary>
        /// Checks if a level is currently open and if the client can continue.
        /// </summary>
        /// <returns>Is it safe to start a new level.</returns>
        bool CheckIfOpenLevel()
        {
            if (STATIC_EDITOR_MODE.levelInstance != null)
            {
                DialogResult result = MessageBox.Show("You currently have an open level. Before you leave, would you like to save?", "Save", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button3);

                if (result == DialogResult.Yes)
                {
                    //  Did it save?
                    if (SaveFile())
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else if (result == System.Windows.Forms.DialogResult.No)
                {
                    if (MessageBox.Show("Are you sure?", "Are you sure", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == System.Windows.Forms.DialogResult.Cancel)
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return true;
            }
        }
        #endregion




        bool SaveFile()
        {
            saveFileDialog1.Filter = "XML files|*.xml";
            saveFileDialog1.AddExtension = true;

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                XmlWriterSettings settings = new XmlWriterSettings();
                settings.ConformanceLevel = ConformanceLevel.Auto;
                settings.Indent = true;
                settings.NewLineHandling = NewLineHandling.Entitize;
                settings.NewLineOnAttributes = true;

                using (XmlWriter writer = XmlWriter.Create(saveFileDialog1.FileName, settings))
                {
                    IntermediateSerializer.Serialize(writer, STATIC_EDITOR_MODE.levelInstance, null);
                }

                return true;
            }
            else
            {
                return false;
            }            
        }




        #endregion




        void CopyPaste()
        {
            if (Is_Something_Selected())
            {

                for (int i = STATIC_EDITOR_MODE.selectedObjectIndices.Count; i > 0; i--)
                {
                    switch (STATIC_EDITOR_MODE.selectedObjectIndices[i - 1].Type)
                    {
                        case OBJECT_TYPE.Physics:
                            STATIC_EDITOR_MODE.levelInstance.ObjectsList.Add(STATIC_EDITOR_MODE.levelInstance.ObjectsList[STATIC_EDITOR_MODE.selectedObjectIndices[i - 1].Index].Clone());
                            break;
                        case OBJECT_TYPE.Decal:
                            STATIC_EDITOR_MODE.levelInstance.DecalManager.DecalList.Add(STATIC_EDITOR_MODE.levelInstance.DecalManager.DecalList[STATIC_EDITOR_MODE.selectedObjectIndices[i - 1].Index].Clone());
                            break;
                    }
                }
            }
        }

        void Handle_Property_Grid_Items()
        {
            if (STATIC_EDITOR_MODE.ED_MODE == EDITOR_MODE.EDIT_LEVEL)
            {
                propertyGrid1.SelectedObject = STATIC_EDITOR_MODE.levelInstance;
            }
            else
            {
                if (Is_Something_Selected())
                {
                    List<object> theStuff = new List<object>();

                    for (int i = 0; i < STATIC_EDITOR_MODE.selectedObjectIndices.Count; i++)
                    {
                        switch (STATIC_EDITOR_MODE.selectedObjectIndices[i].Type)
                        {
                            case (OBJECT_TYPE.Physics):
                                {
                                    theStuff.Add(STATIC_EDITOR_MODE.levelInstance.ObjectsList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index]);
                                }
                                break;
                            case (OBJECT_TYPE.Decal):
                                {
                                    theStuff.Add(STATIC_EDITOR_MODE.levelInstance.DecalManager.DecalList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index]);
                                }
                                break;
                        }
                    }

                    propertyGrid1.SelectedObjects = theStuff.ToArray();
                }
                else
                {
                    propertyGrid1.SelectedObject = false;
                }
            }

            propertyGrid1.RefreshTabs(PropertyTabScope.Component);
        }

        bool Is_Something_Selected()
        {
            if (STATIC_EDITOR_MODE.selectedObjectIndices.Count > 0)
            {
                return true;
            }
            return false;
        }

        void ShiftSelection()
        {
            //If Object is already selected, deselect.
            for (int i = 0; i < STATIC_EDITOR_MODE.selectedObjectIndices.Count; i++)
            {
                if (lst_ObjectsUnderCursor[0].Index == STATIC_EDITOR_MODE.selectedObjectIndices[i].Index && lst_ObjectsUnderCursor[0].Type == STATIC_EDITOR_MODE.selectedObjectIndices[i].Type)
                {
                    STATIC_EDITOR_MODE.selectedObjectIndices.Remove(lst_ObjectsUnderCursor[0]);
                    return;
                }
            }
            //If Object is not already selected, add to selection.
            STATIC_EDITOR_MODE.selectedObjectIndices.Add(lst_ObjectsUnderCursor[0]);
        }

        #region Align Methods

        #region Align Relative To
        private void Align_Relative_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (Align_Relative.SelectedItem.ToString())
            {
                case "First Selected":
                    {
                        STATIC_EDITOR_MODE.ALIGN_TO = ALIGN_ANCHOR.FIRST;
                    }
                    break;
                case "Last Selected":
                    {
                        STATIC_EDITOR_MODE.ALIGN_TO = ALIGN_ANCHOR.LAST;
                    }
                    break;
                case "Selection":
                    {
                        STATIC_EDITOR_MODE.ALIGN_TO = ALIGN_ANCHOR.SELECTION;
                    }
                    break;
            }
        }
        #endregion

        #region Align Buttons

        #region Align Right Side to Left of Anchor
        private void RL_Horizontal_Align_Click(object sender, EventArgs e)
        {
            List<ObjectIndex> selectedObjects = STATIC_EDITOR_MODE.selectedObjectIndices;
            List<NodeObject> ObjectsList = STATIC_EDITOR_MODE.levelInstance.ObjectsList;
            List<Decal> DecalList = STATIC_EDITOR_MODE.levelInstance.DecalManager.DecalList;

            switch (Align_Relative.SelectedItem.ToString())
            {
                case "First Selected":
                    {
                        float firstObjXPos = Get_First_Position().X;
                        float firstObjTexWidth = Get_First_TextureDimensions().X;


                        for (int i = 1; i < selectedObjects.Count; i++)
                        {
                            switch (selectedObjects[i].Type)
                            {
                                case (OBJECT_TYPE.Physics):
                                    {
                                        NodeObject obj = ObjectsList[selectedObjects[i].Index];

                                        obj.Position = new Vector2((firstObjXPos - obj.Width * 0.5f) - (firstObjTexWidth * 0.5f), obj.Position.Y);
                                    }
                                    break;
                                case (OBJECT_TYPE.Decal):
                                    {
                                        Decal obj = DecalList[selectedObjects[i].Index];

                                        obj.Position = new Vector2(firstObjXPos - (obj.Width * 0.5f) - (firstObjTexWidth * 0.5f), obj.Position.Y);
                                    }
                                    break;
                            }
                        }
                    }
                    break;
                case "Last Selected":
                    {
                        float lastObjXPos = Get_Last_Position().X;
                        float lastObjTexWidth = Get_Last_TextureDimensions().X;

                        for (int i = 0; i < selectedObjects.Count - 1; i++)
                        {
                            switch (selectedObjects[i].Type)
                            {
                                case (OBJECT_TYPE.Physics):
                                    {
                                        StaticObject obj = (StaticObject)STATIC_EDITOR_MODE.levelInstance.ObjectsList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index];

                                        obj.Position = new Vector2(
                                            lastObjXPos - (obj.Texture.Width - lastObjTexWidth) * 0.5f,
                                            obj.Position.Y);
                                    }
                                    break;
                                case (OBJECT_TYPE.Decal):
                                    {
                                        Decal obj = DecalList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index];

                                        obj.Position = new Vector2(
                                            lastObjXPos - (obj.Width - lastObjTexWidth) * 0.5f,
                                            obj.Position.Y);
                                    }
                                    break;
                            }
                        }
                    }
                    break;
                case "Selection":
                    {
                        float furthestPos = get_furthest_lpos();

                        for (int i = 0; i < selectedObjects.Count; i++)
                        {
                            switch (selectedObjects[i].Type)
                            {
                                case (OBJECT_TYPE.Physics):
                                    {
                                        StaticObject obj = (StaticObject)STATIC_EDITOR_MODE.levelInstance.ObjectsList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index];

                                        obj.Position = new Vector2(
                                            furthestPos - obj.Width * 0.5f,
                                            obj.Position.Y);
                                    }
                                    break;
                                case (OBJECT_TYPE.Decal):
                                    {
                                        Decal obj = DecalList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index];

                                        obj.Position = new Vector2(
                                            furthestPos - obj.Width * 0.5f,
                                            obj.Position.Y);
                                    }
                                    break;
                            }
                        }
                    }
                    break;
            }
            Handle_Property_Grid_Items();
            Update_undoArray();
        }
        #endregion

        #region Align Left Sides Together
        private void L_Horizontal_Align_Click(object sender, EventArgs e)
        {
            List<ObjectIndex> selectedObjects = STATIC_EDITOR_MODE.selectedObjectIndices;
            List<NodeObject> ObjectsList = STATIC_EDITOR_MODE.levelInstance.ObjectsList;
            List<Decal> DecalList = STATIC_EDITOR_MODE.levelInstance.DecalManager.DecalList;

            switch (Align_Relative.SelectedItem.ToString())
            {
                case "First Selected":
                    {
                        float firstObjXPos = Get_First_Position().X;
                        float firstObjTexWidth = Get_First_TextureDimensions().X;

                        for (int i = 1; i < selectedObjects.Count; i++)
                        {
                            switch (STATIC_EDITOR_MODE.selectedObjectIndices[i].Type)
                            {
                                case (OBJECT_TYPE.Physics):
                                    {
                                        StaticObject obj = (StaticObject)ObjectsList[selectedObjects[i].Index];

                                        obj.Position = new Vector2(
                                            firstObjXPos
                                            + (obj.Texture.Width * 0.5f)
                                            - (firstObjTexWidth * 0.5f),
                                            obj.Position.Y);
                                    }
                                    break;
                                case (OBJECT_TYPE.Decal):
                                    {
                                        Decal obj = DecalList[selectedObjects[i].Index];

                                        obj.Position = new Vector2(
                                            firstObjXPos
                                            + (obj.Width * 0.5f)
                                            - (firstObjTexWidth * 0.5f),
                                            obj.Position.Y);
                                    }
                                    break;
                            }
                        }
                    }
                    break;
                case "Last Selected":
                    {
                        float lastObjXPos = Get_Last_Position().X;
                        float lastObjTexWidth = Get_Last_TextureDimensions().X;

                        for (int i = 0; i < selectedObjects.Count - 1; i++)
                        {
                            switch (selectedObjects[i].Type)
                            {
                                case (OBJECT_TYPE.Physics):
                                    {
                                        StaticObject obj = (StaticObject)ObjectsList[selectedObjects[i].Index];

                                        obj.Position = new Vector2(
                                            lastObjXPos
                                            + (obj.Width * 0.5f)
                                            - (lastObjTexWidth * 0.5f),
                                            obj.Position.Y);
                                    }
                                    break;
                                case (OBJECT_TYPE.Decal):
                                    {
                                        Decal obj = DecalList[selectedObjects[i].Index];

                                        obj.Position = new Vector2(
                                            lastObjXPos
                                            + (obj.Width * 0.5f)
                                            - (lastObjTexWidth * 0.5f),
                                            obj.Position.Y);
                                    }
                                    break;
                            }
                        }
                    }
                    break;
                case "Selection":
                    {
                        float furthestPos = get_furthest_lpos();

                        for (int i = 0; i < STATIC_EDITOR_MODE.selectedObjectIndices.Count; i++)
                        {
                            switch (STATIC_EDITOR_MODE.selectedObjectIndices[i].Type)
                            {
                                case (OBJECT_TYPE.Physics):
                                    {
                                        StaticObject obj = (StaticObject)ObjectsList[selectedObjects[i].Index];

                                        obj.Position = new Vector2(
                                            furthestPos + obj.Width * 0.5f,
                                            obj.Position.Y);
                                    }
                                    break;
                                case (OBJECT_TYPE.Decal):
                                    {
                                        Decal obj = DecalList[selectedObjects[i].Index];

                                        obj.Position = new Vector2(
                                            furthestPos + obj.Width * 0.5f,
                                            obj.Position.Y);
                                    }
                                    break;
                            }
                        }
                    }
                    break;
            }
            Handle_Property_Grid_Items();
            Update_undoArray();
        }
        #endregion

        #region Align Centers Horizontally
        void C_Horizontal_Align_Click(object sender, EventArgs e)
        {
            List<ObjectIndex> selectedObjects = STATIC_EDITOR_MODE.selectedObjectIndices;
            List<NodeObject> ObjectsList = STATIC_EDITOR_MODE.levelInstance.ObjectsList;
            List<Decal> DecalList = STATIC_EDITOR_MODE.levelInstance.DecalManager.DecalList;

            switch (Align_Relative.SelectedItem.ToString())
            {
                case "First Selected":
                    {
                        float firstObjXPos = Get_First_Position().X;

                        for (int i = STATIC_EDITOR_MODE.selectedObjectIndices.Count - 1; i > 0; i--)
                        {
                            switch (STATIC_EDITOR_MODE.selectedObjectIndices[i].Type)
                            {
                                case (OBJECT_TYPE.Physics):
                                    {
                                        StaticObject obj = (StaticObject)ObjectsList[selectedObjects[i].Index];

                                        obj.Position = new Vector2(
                                            firstObjXPos,
                                            obj.Position.Y);
                                    }
                                    break;
                                case (OBJECT_TYPE.Decal):
                                    {
                                        Decal obj = DecalList[selectedObjects[i].Index];

                                        obj.Position = new Vector2(
                                            firstObjXPos,
                                            obj.Position.Y);
                                    }
                                    break;
                            }
                        }
                    }
                    break;
                case "Last Selected":
                    {
                        float lastObjXPos = Get_Last_Position().X;

                        for (int i = 0; i < selectedObjects.Count - 1; i++)
                        {
                            switch (STATIC_EDITOR_MODE.selectedObjectIndices[i].Type)
                            {
                                case (OBJECT_TYPE.Physics):
                                    {
                                        StaticObject obj = (StaticObject)ObjectsList[selectedObjects[i].Index];

                                        obj.Position = new Vector2(
                                            lastObjXPos,
                                            obj.Position.Y);
                                    }
                                    break;
                                case (OBJECT_TYPE.Decal):
                                    {
                                        Decal obj = DecalList[selectedObjects[i].Index];

                                        obj.Position = new Vector2(
                                            lastObjXPos,
                                            obj.Position.Y);
                                    }
                                    break;
                            }
                        }
                    }
                    break;
                case "Selection":
                    {
                        float avgXPos = get_avg_xpos();

                        for (int i = 0; i < STATIC_EDITOR_MODE.selectedObjectIndices.Count; i++)
                        {
                            switch (STATIC_EDITOR_MODE.selectedObjectIndices[i].Type)
                            {
                                case (OBJECT_TYPE.Physics):
                                    {
                                        StaticObject obj = (StaticObject)ObjectsList[selectedObjects[i].Index];

                                        obj.Position = new Vector2(
                                            avgXPos, obj.Position.Y);
                                    }
                                    break;
                                case (OBJECT_TYPE.Decal):
                                    {
                                        Decal obj = DecalList[selectedObjects[i].Index];

                                        obj.Position = new Vector2(avgXPos, obj.Position.Y);
                                    }
                                    break;
                            }
                        }
                    }
                    break;
            }
            Handle_Property_Grid_Items();
            Update_undoArray();
        }
        #endregion

        #region Align Right Sides Together
        private void R_Horizontal_Align_Click(object sender, EventArgs e)
        {
            List<ObjectIndex> selectedObjects = STATIC_EDITOR_MODE.selectedObjectIndices;
            List<NodeObject> ObjectsList = STATIC_EDITOR_MODE.levelInstance.ObjectsList;
            List<Decal> DecalList = STATIC_EDITOR_MODE.levelInstance.DecalManager.DecalList;

            switch (Align_Relative.SelectedItem.ToString())
            {
                case "First Selected":
                    {
                        float firstObjXPos = Get_First_Position().X;
                        float firstObjTexWidth = Get_First_TextureDimensions().X;

                        for (int i = STATIC_EDITOR_MODE.selectedObjectIndices.Count - 1; i > 0; i--)
                        {
                            switch (STATIC_EDITOR_MODE.selectedObjectIndices[i].Type)
                            {
                                case (OBJECT_TYPE.Physics):
                                    {
                                        StaticObject obj = (StaticObject)ObjectsList[selectedObjects[i].Index];

                                        obj.Position = new Vector2(
                                            firstObjXPos
                                            - (obj.Width * 0.5f)
                                            + (firstObjTexWidth * 0.5f),
                                            obj.Position.Y);
                                    }
                                    break;
                                case (OBJECT_TYPE.Decal):
                                    {
                                        Decal obj = DecalList[selectedObjects[i].Index];

                                        obj.Position = new Vector2(
                                            firstObjXPos
                                            - (obj.Width * 0.5f)
                                            + (firstObjTexWidth * 0.5f),
                                            obj.Position.Y);
                                    }
                                    break;
                            }
                        }
                    }
                    break;
                case "Last Selected":
                    {
                        float lastObjXPos = Get_Last_Position().X;
                        float lastObjTexWidth = Get_Last_TextureDimensions().X;

                        for (int i = 0; i < STATIC_EDITOR_MODE.selectedObjectIndices.Count - 1; i++)
                        {
                            switch (STATIC_EDITOR_MODE.selectedObjectIndices[i].Type)
                            {
                                case (OBJECT_TYPE.Physics):
                                    {
                                        StaticObject obj = (StaticObject)ObjectsList[selectedObjects[i].Index];

                                        obj.Position = new Vector2(
                                            lastObjXPos
                                            - (obj.Width * 0.5f)
                                            + (lastObjTexWidth * 0.5f),
                                            obj.Position.Y);
                                    }
                                    break;
                                case (OBJECT_TYPE.Decal):
                                    {
                                        Decal obj = DecalList[selectedObjects[i].Index];

                                        obj.Position = new Vector2(
                                            lastObjXPos
                                            - (obj.Width * 0.5f)
                                            + (lastObjTexWidth * 0.5f),
                                            obj.Position.Y);
                                    }
                                    break;
                            }
                        }
                    }
                    break;
                case "Selection":
                    {
                        float furthestPos = get_furthest_rpos();

                        for (int i = 0; i < STATIC_EDITOR_MODE.selectedObjectIndices.Count; i++)
                        {
                            switch (STATIC_EDITOR_MODE.selectedObjectIndices[i].Type)
                            {
                                case (OBJECT_TYPE.Physics):
                                    {
                                        StaticObject obj = (StaticObject)ObjectsList[selectedObjects[i].Index];

                                        obj.Position = new Vector2(
                                            furthestPos - obj.Width * 0.5f,
                                            obj.Position.Y);
                                    }
                                    break;
                                case (OBJECT_TYPE.Decal):
                                    {
                                        Decal obj = DecalList[selectedObjects[i].Index];

                                        obj.Position = new Vector2(
                                            furthestPos - obj.Width * 0.5f,
                                            obj.Position.Y);
                                    }
                                    break;
                            }
                        }
                    }
                    break;
            }
            Handle_Property_Grid_Items();
            Update_undoArray();
        }
        #endregion

        #region Align Left Sides to Right of Anchor
        private void LR_Horizontal_Align_Click(object sender, EventArgs e)
        {
            List<ObjectIndex> selectedObjects = STATIC_EDITOR_MODE.selectedObjectIndices;
            List<NodeObject> ObjectsList = STATIC_EDITOR_MODE.levelInstance.ObjectsList;
            List<Decal> DecalList = STATIC_EDITOR_MODE.levelInstance.DecalManager.DecalList;

            switch (Align_Relative.SelectedItem.ToString())
            {
                case "First Selected":
                    {
                        float firstObjXPos = Get_First_Position().X;
                        float firstObjTexWidth = Get_First_TextureDimensions().X;

                        for (int i = 1; i < selectedObjects.Count; i++)
                        {
                            switch (selectedObjects[i].Type)
                            {
                                case (OBJECT_TYPE.Physics):
                                    {
                                        StaticObject obj = (StaticObject)ObjectsList[selectedObjects[i].Index];

                                        obj.Position = new Vector2(
                                            firstObjXPos
                                            + (obj.Texture.Width * 0.5f)
                                            + (firstObjTexWidth * 0.5f),
                                            obj.Position.Y);
                                    }
                                    break;
                                case (OBJECT_TYPE.Decal):
                                    {
                                        Decal obj = DecalList[selectedObjects[i].Index];

                                        obj.Position = new Vector2(
                                            firstObjXPos
                                            + (obj.Width * 0.5f)
                                            + (firstObjTexWidth * 0.5f),
                                            obj.Position.Y);
                                    }
                                    break;
                            }
                        }
                    }
                    break;
                case "Last Selected":
                    {
                        float lastObjXPos = Get_Last_Position().X;
                        float lastObjTexWidth = Get_Last_TextureDimensions().X;

                        for (int i = 0; i < selectedObjects.Count - 1; i++)
                        {
                            switch (selectedObjects[i].Type)
                            {
                                case (OBJECT_TYPE.Physics):
                                    {
                                        StaticObject obj = (StaticObject)ObjectsList[selectedObjects[i].Index];

                                        obj.Position = new Vector2(
                                            lastObjXPos
                                            + (obj.Width * 0.5f)
                                            + (lastObjTexWidth * 0.5f),
                                            obj.Position.Y);
                                    }
                                    break;
                                case (OBJECT_TYPE.Decal):
                                    {
                                        Decal obj = DecalList[selectedObjects[i].Index];

                                        obj.Position = new Vector2(
                                            lastObjXPos
                                            + (obj.Width * 0.5f)
                                            + (lastObjTexWidth * 0.5f),
                                            obj.Position.Y);
                                    }
                                    break;
                            }
                        }
                    }
                    break;
                case "Selection":
                    {
                        float furthestPos = get_furthest_rpos();

                        for (int i = 0; i < selectedObjects.Count; i++)
                        {
                            switch (selectedObjects[i].Type)
                            {
                                case (OBJECT_TYPE.Physics):
                                    {
                                        StaticObject obj = (StaticObject)ObjectsList[selectedObjects[i].Index];

                                        obj.Position = new Vector2(
                                            furthestPos + obj.Width * 0.5f,
                                            obj.Position.Y);
                                    }
                                    break;
                                case (OBJECT_TYPE.Decal):
                                    {
                                        Decal obj = DecalList[selectedObjects[i].Index];

                                        obj.Position = new Vector2(
                                            furthestPos + obj.Width * 0.5f,
                                            obj.Position.Y);
                                    }
                                    break;
                            }
                        }
                    }
                    break;
            }
            Handle_Property_Grid_Items();
            Update_undoArray();
        }
        #endregion

        #region Align Bottoms to Top of Anchor
        private void BT_Vertical_Align_Click(object sender, EventArgs e)
        {
            List<ObjectIndex> selectedObjects = STATIC_EDITOR_MODE.selectedObjectIndices;
            List<NodeObject> ObjectsList = STATIC_EDITOR_MODE.levelInstance.ObjectsList;
            List<Decal> DecalList = STATIC_EDITOR_MODE.levelInstance.DecalManager.DecalList;

            switch (Align_Relative.SelectedItem.ToString())
            {
                case "First Selected":
                    {
                        float firstObjYPos = Get_First_Position().Y;
                        float firstObjTexHeight = Get_First_TextureDimensions().Y;

                        for (int i = STATIC_EDITOR_MODE.selectedObjectIndices.Count - 1; i > 0; i--)
                        {
                            switch (STATIC_EDITOR_MODE.selectedObjectIndices[i].Type)
                            {
                                case (OBJECT_TYPE.Physics):
                                    {
                                        StaticObject obj = (StaticObject)ObjectsList[selectedObjects[i].Index];

                                        obj.Position = new Vector2(
                                            obj.Position.X,
                                            firstObjYPos
                                            - (obj.Height * 0.5f)
                                            - (firstObjTexHeight * 0.5f));
                                    }
                                    break;
                                case (OBJECT_TYPE.Decal):
                                    {
                                        Decal obj = DecalList[selectedObjects[i].Index];

                                        obj.Position = new Vector2(
                                            obj.Position.X,
                                            firstObjYPos
                                            - (obj.Height * 0.5f)
                                            - (firstObjTexHeight * 0.5f));
                                    }
                                    break;
                            }
                        }
                    }
                    break;
                case "Last Selected":
                    {
                        float lastObjYPos = Get_Last_Position().Y;
                        float lastObjTexHeight = Get_Last_TextureDimensions().Y;

                        for (int i = 0; i < selectedObjects.Count - 1; i++)
                        {
                            switch (selectedObjects[i].Type)
                            {
                                case (OBJECT_TYPE.Physics):
                                    {
                                        StaticObject obj = (StaticObject)ObjectsList[selectedObjects[i].Index];

                                        obj.Position = new Vector2(
                                            obj.Position.X,
                                            lastObjYPos
                                            - (obj.Height * 0.5f)
                                            - (lastObjTexHeight * 0.5f));
                                    }
                                    break;
                                case (OBJECT_TYPE.Decal):
                                    {
                                        Decal obj = DecalList[selectedObjects[i].Index];

                                        obj.Position = new Vector2(
                                            obj.Position.X,
                                            lastObjYPos
                                            - (obj.Height * 0.5f)
                                            - (lastObjTexHeight * 0.5f));
                                    }
                                    break;
                            }
                        }
                    }
                    break;
                case "Selection":
                    {
                        float furthestPos = get_furthest_upos();

                        for (int i = 0; i < selectedObjects.Count; i++)
                        {
                            switch (selectedObjects[i].Type)
                            {
                                case (OBJECT_TYPE.Physics):
                                    {
                                        StaticObject obj = (StaticObject)ObjectsList[selectedObjects[i].Index];

                                        obj.Position = new Vector2(
                                            obj.Position.X,
                                            furthestPos - obj.Height * 0.5f);
                                    }
                                    break;
                                case (OBJECT_TYPE.Decal):
                                    {
                                        Decal obj = DecalList[selectedObjects[i].Index];

                                        obj.Position = new Vector2(
                                            obj.Position.X,
                                            furthestPos - obj.Height * 0.5f);
                                    }
                                    break;
                            }
                        }
                    }
                    break;
            }
            Handle_Property_Grid_Items();
            Update_undoArray();
        }
        #endregion

        #region Align Tops
        private void T_Vertical_Align_Click(object sender, EventArgs e)
        {
            List<ObjectIndex> selectedObjects = STATIC_EDITOR_MODE.selectedObjectIndices;
            List<NodeObject> ObjectsList = STATIC_EDITOR_MODE.levelInstance.ObjectsList;
            List<Decal> DecalList = STATIC_EDITOR_MODE.levelInstance.DecalManager.DecalList;

            switch (Align_Relative.SelectedItem.ToString())
            {
                case "First Selected":
                    {
                        float firstObjYPos = Get_First_Position().Y;
                        float firstObjTexHeight = Get_First_TextureDimensions().Y;

                        for (int i = selectedObjects.Count - 1; i > 0; i--)
                        {
                            switch (selectedObjects[i].Type)
                            {
                                case (OBJECT_TYPE.Physics):
                                    {
                                        StaticObject obj = (StaticObject)ObjectsList[selectedObjects[i].Index];

                                        obj.Position = new Vector2(
                                            obj.Position.X,
                                            firstObjYPos
                                            + (obj.Texture.Height * 0.5f)
                                            - (firstObjTexHeight * 0.5f));
                                    }
                                    break;
                                case (OBJECT_TYPE.Decal):
                                    {
                                        Decal obj = DecalList[selectedObjects[i].Index];

                                        obj.Position = new Vector2(
                                            obj.Position.X,
                                            firstObjYPos
                                            + (obj.Height * 0.5f)
                                            - (firstObjTexHeight * 0.5f));
                                    }
                                    break;
                            }
                        }
                    }
                    break;
                case "Last Selected":
                    {
                        float lastObjYPos = Get_Last_Position().Y;
                        float lastObjTexHeight = Get_Last_TextureDimensions().Y;

                        for (int i = 0; i < selectedObjects.Count - 1; i++)
                        {
                            switch (selectedObjects[i].Type)
                            {
                                case (OBJECT_TYPE.Physics):
                                    {
                                        StaticObject obj = (StaticObject)ObjectsList[selectedObjects[i].Index];

                                        obj.Position = new Vector2(
                                            obj.Position.X,
                                            lastObjYPos
                                            + (obj.Height * 0.5f)
                                            - (lastObjTexHeight * 0.5f));
                                    }
                                    break;
                                case (OBJECT_TYPE.Decal):
                                    {
                                        Decal obj = DecalList[selectedObjects[i].Index];

                                        obj.Position = new Vector2(
                                            obj.Position.X,
                                            lastObjYPos
                                            + (obj.Height * 0.5f)
                                            - (lastObjTexHeight * 0.5f));
                                    }
                                    break;
                            }
                        }
                    }
                    break;
                case "Selection":
                    {
                        float furthestPos = get_furthest_upos();

                        for (int i = 0; i < selectedObjects.Count; i++)
                        {
                            switch (selectedObjects[i].Type)
                            {
                                case (OBJECT_TYPE.Physics):
                                    {
                                        StaticObject obj = (StaticObject)ObjectsList[selectedObjects[i].Index];

                                        obj.Position = new Vector2(
                                            obj.Position.X,
                                            furthestPos + obj.Height * 0.5f);
                                    }
                                    break;
                                case (OBJECT_TYPE.Decal):
                                    {
                                        Decal obj = DecalList[selectedObjects[i].Index];

                                        obj.Position = new Vector2(
                                            obj.Position.X,
                                            furthestPos + obj.Height * 0.5f);
                                    }
                                    break;
                            }
                        }
                    }
                    break;
            }
            Handle_Property_Grid_Items();
            Update_undoArray();
        }
        #endregion

        #region Align Centers Vertically
        private void C_Vertical_Align_Click(object sender, EventArgs e)
        {
            List<ObjectIndex> selectedObjects = STATIC_EDITOR_MODE.selectedObjectIndices;
            List<NodeObject> ObjectsList = STATIC_EDITOR_MODE.levelInstance.ObjectsList;
            List<Decal> DecalList = STATIC_EDITOR_MODE.levelInstance.DecalManager.DecalList;

            switch (Align_Relative.SelectedItem.ToString())
            {
                case "First Selected":
                    {
                        float firstObjYPos = Get_First_Position().Y;

                        for (int i = selectedObjects.Count - 1; i > 0; i--)
                        {
                            switch (selectedObjects[i].Type)
                            {
                                case (OBJECT_TYPE.Physics):
                                    {
                                        StaticObject obj = (StaticObject)ObjectsList[selectedObjects[i].Index];

                                        obj.Position = new Vector2(
                                            obj.Position.X,
                                            firstObjYPos);
                                    }
                                    break;
                                case (OBJECT_TYPE.Decal):
                                    {
                                        Decal obj = DecalList[selectedObjects[i].Index];

                                        obj.Position = new Vector2(
                                            obj.Position.X,
                                            firstObjYPos);
                                    }
                                    break;
                            }
                        }
                    }
                    break;
                case "Last Selected":
                    {
                        float lastObjYPos = Get_Last_Position().Y;

                        for (int i = 0; i < selectedObjects.Count - 1; i++)
                        {
                            switch (selectedObjects[i].Type)
                            {
                                case (OBJECT_TYPE.Physics):
                                    {
                                        StaticObject obj = (StaticObject)ObjectsList[selectedObjects[i].Index];

                                        obj.Position = new Vector2(
                                            obj.Position.X,
                                            lastObjYPos);
                                    }
                                    break;
                                case (OBJECT_TYPE.Decal):
                                    {
                                        Decal obj = DecalList[selectedObjects[i].Index];

                                        obj.Position = new Vector2(
                                            obj.Position.X,
                                            lastObjYPos);
                                    }
                                    break;
                            }
                        }
                    }
                    break;
                case "Selection":
                    {
                        float avgYPos = get_avg_ypos();

                        for (int i = 0; i < selectedObjects.Count; i++)
                        {
                            switch (selectedObjects[i].Type)
                            {
                                case (OBJECT_TYPE.Physics):
                                    {
                                        StaticObject obj = (StaticObject)ObjectsList[selectedObjects[i].Index];

                                        obj.Position = new Vector2(
                                            obj.Position.X, avgYPos);
                                    }
                                    break;
                                case (OBJECT_TYPE.Decal):
                                    {
                                        Decal obj = DecalList[selectedObjects[i].Index];

                                        obj.Position =
                                            new Vector2(obj.Position.X, avgYPos);
                                    }
                                    break;
                            }
                        }
                    }
                    break;
            }
            Handle_Property_Grid_Items();
            Update_undoArray();
        }
        #endregion

        #region Align Bottoms
        private void B_Vertical_Align_Click(object sender, EventArgs e)
        {
            List<ObjectIndex> selectedObjects = STATIC_EDITOR_MODE.selectedObjectIndices;
            List<NodeObject> ObjectsList = STATIC_EDITOR_MODE.levelInstance.ObjectsList;
            List<Decal> DecalList = STATIC_EDITOR_MODE.levelInstance.DecalManager.DecalList;

            switch (Align_Relative.SelectedItem.ToString())
            {
                case "First Selected":
                    {
                        float firstObjYPos = Get_First_Position().Y;
                        float firstObjTexHeight = Get_First_TextureDimensions().Y;

                        for (int i = selectedObjects.Count - 1; i > 0; i--)
                        {
                            switch (selectedObjects[i].Type)
                            {
                                case (OBJECT_TYPE.Physics):
                                    {
                                        StaticObject obj = (StaticObject)ObjectsList[selectedObjects[i].Index];

                                        obj.Position = new Vector2(
                                            obj.Position.X,
                                            firstObjYPos
                                            - (obj.Height * 0.5f)
                                            + (firstObjTexHeight * 0.5f));
                                    }
                                    break;
                                case (OBJECT_TYPE.Decal):
                                    {
                                        Decal obj = DecalList[selectedObjects[i].Index];

                                        obj.Position = new Vector2(
                                            obj.Position.X,
                                            firstObjYPos
                                            - (obj.Height * 0.5f)
                                            + (firstObjTexHeight * 0.5f));
                                    }
                                    break;
                            }
                        }
                    }
                    break;
                case "Last Selected":
                    {
                        float lastObjYPos = Get_Last_Position().Y;
                        float lastObjTexHeight = Get_Last_TextureDimensions().Y;

                        for (int i = 0; i < selectedObjects.Count - 1; i++)
                        {
                            switch (selectedObjects[i].Type)
                            {
                                case (OBJECT_TYPE.Physics):
                                    {
                                        StaticObject obj = (StaticObject)ObjectsList[selectedObjects[i].Index];

                                        obj.Position = new Vector2(
                                            obj.Position.X,
                                            lastObjYPos
                                            - (obj.Height * 0.5f)
                                            + (lastObjTexHeight * 0.5f));
                                    }
                                    break;
                                case (OBJECT_TYPE.Decal):
                                    {
                                        Decal obj = DecalList[selectedObjects[i].Index];

                                        obj.Position = new Vector2(
                                            obj.Position.X,
                                            lastObjYPos
                                            - (obj.Height * 0.5f)
                                            + (lastObjTexHeight * 0.5f));
                                    }
                                    break;
                            }
                        }
                    }
                    break;
                case "Selection":
                    {
                        float furthestPos = get_furthest_dpos();

                        for (int i = 0; i < selectedObjects.Count; i++)
                        {
                            switch (selectedObjects[i].Type)
                            {
                                case (OBJECT_TYPE.Physics):
                                    {
                                        StaticObject obj = (StaticObject)ObjectsList[selectedObjects[i].Index];

                                        obj.Position = new Vector2(
                                            obj.Position.X,
                                            furthestPos - obj.Height * 0.5f);
                                    }
                                    break;
                                case (OBJECT_TYPE.Decal):
                                    {
                                        Decal obj = DecalList[selectedObjects[i].Index];

                                        obj.Position = new Vector2(
                                            obj.Position.X,
                                            furthestPos - obj.Height * 0.5f);
                                    }
                                    break;
                            }
                        }
                    }
                    break;
            }
            Handle_Property_Grid_Items();
            Update_undoArray();
        }
        #endregion

        #region Align Tops to Bottom of Anchor
        private void TB_Vertical_Align_Click(object sender, EventArgs e)
        {
            List<ObjectIndex> selectedObjects = STATIC_EDITOR_MODE.selectedObjectIndices;
            List<NodeObject> ObjectsList = STATIC_EDITOR_MODE.levelInstance.ObjectsList;
            List<Decal> DecalList = STATIC_EDITOR_MODE.levelInstance.DecalManager.DecalList;

            switch (Align_Relative.SelectedItem.ToString())
            {
                case "First Selected":
                    {
                        float firstObjYPos = Get_First_Position().Y;
                        float firstObjTexHeight = Get_First_TextureDimensions().Y;

                        for (int i = selectedObjects.Count - 1; i > 0; i--)
                        {
                            switch (selectedObjects[i].Type)
                            {
                                case (OBJECT_TYPE.Physics):
                                    {
                                        StaticObject obj = (StaticObject)ObjectsList[selectedObjects[i].Index];
                                        
                                        obj.Position = new Vector2(
                                            obj.Position.X,
                                            firstObjYPos
                                            + (obj.Height * 0.5f)
                                            + (firstObjTexHeight * 0.5f));
                                    }
                                    break;
                                case (OBJECT_TYPE.Decal):
                                    {
                                        Decal obj = DecalList[selectedObjects[i].Index];

                                        obj.Position = new Vector2(
                                            obj.Position.X,
                                            firstObjYPos
                                            + (obj.Height * 0.5f)
                                            + (firstObjTexHeight * 0.5f));
                                    }
                                    break;
                            }
                        }
                    }
                    break;
                case "Last Selected":
                    {
                        float lastObjYPos = Get_Last_Position().Y;
                        float lastObjTexHeight = Get_Last_TextureDimensions().Y;

                        for (int i = 0; i < selectedObjects.Count - 1; i++)
                        {
                            switch (selectedObjects[i].Type)
                            {
                                case (OBJECT_TYPE.Physics):
                                    {
                                        StaticObject obj = (StaticObject)ObjectsList[selectedObjects[i].Index];

                                        obj.Position = new Vector2(
                                            obj.Position.X,
                                            lastObjYPos
                                            + (obj.Height * 0.5f)
                                            + (lastObjTexHeight * 0.5f));
                                    }
                                    break;
                                case (OBJECT_TYPE.Decal):
                                    {
                                        Decal obj = DecalList[selectedObjects[i].Index];

                                        obj.Position = new Vector2(
                                            obj.Position.X,
                                            lastObjYPos
                                            + (obj.Height * 0.5f)
                                            + (lastObjTexHeight * 0.5f));
                                    }
                                    break;
                            }
                        }
                    }
                    break;
                case "Selection":
                    {
                        float furthestPos = get_furthest_dpos();

                        for (int i = 0; i < selectedObjects.Count; i++)
                        {
                            switch (selectedObjects[i].Type)
                            {
                                case (OBJECT_TYPE.Physics):
                                    {
                                        StaticObject obj = (StaticObject)ObjectsList[selectedObjects[i].Index];

                                        obj.Position = new Vector2(
                                            obj.Position.X,
                                            furthestPos + obj.Height * 0.5f);
                                    }
                                    break;
                                case (OBJECT_TYPE.Decal):
                                    {
                                        Decal obj = DecalList[selectedObjects[i].Index];

                                        obj.Position = new Vector2(
                                            obj.Position.X,
                                            furthestPos + obj.Height * 0.5f);
                                    }
                                    break;
                            }
                        }
                    }
                    break;
            }
            Handle_Property_Grid_Items();
            Update_undoArray();
        }
        #endregion

        #region Alignment Gets
        private float get_avg_xpos()
        {
            float furthestLeftPos = 0;
            float furthestRightPos = 0;
            List<NodeObject> ObjectsList = STATIC_EDITOR_MODE.levelInstance.ObjectsList;
            List<Decal> DecalList = STATIC_EDITOR_MODE.levelInstance.DecalManager.DecalList;

            switch (STATIC_EDITOR_MODE.selectedObjectIndices[0].Type)
            {
                case (OBJECT_TYPE.Physics):
                    {
                        furthestLeftPos = ObjectsList[STATIC_EDITOR_MODE.selectedObjectIndices[0].Index].Position.X - ObjectsList[STATIC_EDITOR_MODE.selectedObjectIndices[0].Index].Width * 0.5f;
                        furthestRightPos = ObjectsList[STATIC_EDITOR_MODE.selectedObjectIndices[0].Index].Position.X + ObjectsList[STATIC_EDITOR_MODE.selectedObjectIndices[0].Index].Width * 0.5f;
                    }
                    break;
                case (OBJECT_TYPE.Decal):
                    {
                        furthestLeftPos = STATIC_EDITOR_MODE.levelInstance.DecalManager.DecalList[STATIC_EDITOR_MODE.selectedObjectIndices[0].Index].Position.X - STATIC_EDITOR_MODE.levelInstance.DecalManager.DecalList[STATIC_EDITOR_MODE.selectedObjectIndices[0].Index].Width / 2;
                        furthestRightPos = STATIC_EDITOR_MODE.levelInstance.DecalManager.DecalList[STATIC_EDITOR_MODE.selectedObjectIndices[0].Index].Position.X + STATIC_EDITOR_MODE.levelInstance.DecalManager.DecalList[STATIC_EDITOR_MODE.selectedObjectIndices[0].Index].Width / 2;
                    }
                    break;
            }

            for (int i = 0; i < STATIC_EDITOR_MODE.selectedObjectIndices.Count; i++)
            {
                switch (STATIC_EDITOR_MODE.selectedObjectIndices[i].Type)
                {
                    case (OBJECT_TYPE.Physics):
                        {
                            if (furthestLeftPos > ObjectsList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Position.X - ObjectsList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Width / 2)
                                furthestLeftPos = ObjectsList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Position.X - ObjectsList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Width / 2;
                            if (furthestRightPos < ObjectsList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Position.X + ObjectsList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Width / 2)
                                furthestRightPos = ObjectsList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Position.X + ObjectsList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Width / 2;
                        }
                        break;
                    case (OBJECT_TYPE.Decal):
                        {
                            if (furthestLeftPos > STATIC_EDITOR_MODE.levelInstance.DecalManager.DecalList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Position.X - STATIC_EDITOR_MODE.levelInstance.DecalManager.DecalList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Width / 2)
                                furthestLeftPos = STATIC_EDITOR_MODE.levelInstance.DecalManager.DecalList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Position.X - STATIC_EDITOR_MODE.levelInstance.DecalManager.DecalList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Width / 2;
                            if (furthestRightPos < STATIC_EDITOR_MODE.levelInstance.DecalManager.DecalList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Position.X + STATIC_EDITOR_MODE.levelInstance.DecalManager.DecalList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Width / 2)
                                furthestRightPos = STATIC_EDITOR_MODE.levelInstance.DecalManager.DecalList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Position.X + STATIC_EDITOR_MODE.levelInstance.DecalManager.DecalList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Width / 2;
                        }
                        break;
                }
            }

            return ((furthestLeftPos + furthestRightPos) / 2);
        }
        private float get_avg_ypos()
        {
            float furthestUpPos = 0;
            float furthestDownPos = 0;
            List<NodeObject> ObjectsList = STATIC_EDITOR_MODE.levelInstance.ObjectsList;
            List<Decal> DecalList = STATIC_EDITOR_MODE.levelInstance.DecalManager.DecalList;

            switch (STATIC_EDITOR_MODE.selectedObjectIndices[0].Type)
            {
                case (OBJECT_TYPE.Physics):
                    {
                        furthestUpPos = ObjectsList[STATIC_EDITOR_MODE.selectedObjectIndices[0].Index].Position.Y - ObjectsList[STATIC_EDITOR_MODE.selectedObjectIndices[0].Index].Height / 2;
                        furthestDownPos = ObjectsList[STATIC_EDITOR_MODE.selectedObjectIndices[0].Index].Position.Y + ObjectsList[STATIC_EDITOR_MODE.selectedObjectIndices[0].Index].Height / 2;
                    }
                    break;
                case (OBJECT_TYPE.Decal):
                    {
                        furthestUpPos = STATIC_EDITOR_MODE.levelInstance.DecalManager.DecalList[STATIC_EDITOR_MODE.selectedObjectIndices[0].Index].Position.Y - STATIC_EDITOR_MODE.levelInstance.DecalManager.DecalList[STATIC_EDITOR_MODE.selectedObjectIndices[0].Index].Height / 2;
                        furthestDownPos = STATIC_EDITOR_MODE.levelInstance.DecalManager.DecalList[STATIC_EDITOR_MODE.selectedObjectIndices[0].Index].Position.Y + STATIC_EDITOR_MODE.levelInstance.DecalManager.DecalList[STATIC_EDITOR_MODE.selectedObjectIndices[0].Index].Height / 2;
                    }
                    break;
            }

            for (int i = 0; i < STATIC_EDITOR_MODE.selectedObjectIndices.Count; i++)
            {
                switch (STATIC_EDITOR_MODE.selectedObjectIndices[i].Type)
                {
                    case (OBJECT_TYPE.Physics):
                        {
                            if (furthestUpPos > ObjectsList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Position.Y - ObjectsList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Height / 2)
                                furthestUpPos = ObjectsList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Position.Y - ObjectsList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Height / 2;
                            if (furthestDownPos < ObjectsList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Position.Y + ObjectsList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Height / 2)
                                furthestDownPos = ObjectsList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Position.Y + ObjectsList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Height / 2;
                        }
                        break;
                    case (OBJECT_TYPE.Decal):
                        {
                            if (furthestUpPos > STATIC_EDITOR_MODE.levelInstance.DecalManager.DecalList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Position.Y - STATIC_EDITOR_MODE.levelInstance.DecalManager.DecalList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Height / 2)
                                furthestUpPos = STATIC_EDITOR_MODE.levelInstance.DecalManager.DecalList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Position.Y - STATIC_EDITOR_MODE.levelInstance.DecalManager.DecalList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Height / 2;
                            if (furthestDownPos < STATIC_EDITOR_MODE.levelInstance.DecalManager.DecalList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Position.Y + STATIC_EDITOR_MODE.levelInstance.DecalManager.DecalList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Height / 2)
                                furthestDownPos = STATIC_EDITOR_MODE.levelInstance.DecalManager.DecalList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Position.Y + STATIC_EDITOR_MODE.levelInstance.DecalManager.DecalList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Height / 2;
                        }
                        break;
                }
            }

            return ((furthestUpPos + furthestDownPos) / 2);
        }


        #region Alignment Position Gets



        /// <summary>
        /// Returns theposition of the first object in the SelectedObjects list.
        /// </summary>
        Vector2 Get_First_Position()
        {
            Vector2 objPosition = Vector2.Zero;
            List<NodeObject> ObjectsList = STATIC_EDITOR_MODE.levelInstance.ObjectsList;
            List<Decal> DecalList = STATIC_EDITOR_MODE.levelInstance.DecalManager.DecalList;
            ObjectIndex objIndex = STATIC_EDITOR_MODE.selectedObjectIndices[0];

            switch (STATIC_EDITOR_MODE.selectedObjectIndices[0].Type)
            {
                case OBJECT_TYPE.Physics:
                    {
                        StaticObject obj = (StaticObject)STATIC_EDITOR_MODE.levelInstance.ObjectsList[objIndex.Index];

                        objPosition = obj.Position;
                        break;
                    }
                case OBJECT_TYPE.Decal:
                    {
                        objPosition = DecalList[objIndex.Index].Position;
                        break;
                    }
            }

            return objPosition;
        }

        /// <summary>
        /// Returns the position of the last object in the SelectedObjects list.
        /// </summary>
        Vector2 Get_Last_Position()
        {
            Vector2 objPosition = Vector2.Zero;
            List<NodeObject> ObjectsList = STATIC_EDITOR_MODE.levelInstance.ObjectsList;
            List<Decal> DecalList = STATIC_EDITOR_MODE.levelInstance.DecalManager.DecalList;

            switch (STATIC_EDITOR_MODE.selectedObjectIndices[0].Type)
            {
                case OBJECT_TYPE.Physics:
                    {
                        StaticObject obj = (StaticObject)ObjectsList[STATIC_EDITOR_MODE.selectedObjectIndices[STATIC_EDITOR_MODE.selectedObjectIndices.Count - 1].Index];

                        objPosition = obj.Position;
                        break;
                    }
                case OBJECT_TYPE.Decal:
                    {
                        Decal obj = DecalList[STATIC_EDITOR_MODE.selectedObjectIndices[STATIC_EDITOR_MODE.selectedObjectIndices.Count - 1].Index];

                        objPosition = obj.Position;
                        break;
                    }
            }

            return objPosition;
        }



        #endregion


        #region Alignment Texture Dimension Gets



        /// <summary>
        /// Returns the texture dimensions of the first object in the SelectedObjects list.
        /// </summary>
        Vector2 Get_First_TextureDimensions()
        {
            Vector2 textureDimensions = Vector2.Zero;
            List<NodeObject> ObjectsList = STATIC_EDITOR_MODE.levelInstance.ObjectsList;
            List<Decal> DecalList = STATIC_EDITOR_MODE.levelInstance.DecalManager.DecalList;

            switch (STATIC_EDITOR_MODE.selectedObjectIndices[0].Type)
            {
                case OBJECT_TYPE.Physics:
                    {
                        StaticObject obj = (StaticObject)STATIC_EDITOR_MODE.levelInstance.ObjectsList[STATIC_EDITOR_MODE.selectedObjectIndices[0].Index];

                        textureDimensions = new Vector2(obj.Width, obj.Height);
                        break;
                    }
                case OBJECT_TYPE.Decal:
                    {
                        Decal obj = DecalList[STATIC_EDITOR_MODE.selectedObjectIndices[0].Index];

                        textureDimensions = new Vector2(obj.Width, obj.Height);
                        break;
                    }
            }

            return textureDimensions;
        }

        /// <summary>
        /// Returns the texture dimensions of the last object in the SelectedObjects list.
        /// </summary>
        Vector2 Get_Last_TextureDimensions()
        {
            Vector2 textureDimensions = Vector2.Zero;
            List<NodeObject> ObjectsList = STATIC_EDITOR_MODE.levelInstance.ObjectsList;
            List<Decal> DecalList = STATIC_EDITOR_MODE.levelInstance.DecalManager.DecalList;

            switch (STATIC_EDITOR_MODE.selectedObjectIndices[STATIC_EDITOR_MODE.selectedObjectIndices.Count - 1].Type)
            {
                case OBJECT_TYPE.Physics:
                    {
                        StaticObject obj = (StaticObject)STATIC_EDITOR_MODE.levelInstance.ObjectsList[STATIC_EDITOR_MODE.selectedObjectIndices[STATIC_EDITOR_MODE.selectedObjectIndices.Count - 1].Index];

                        textureDimensions =  new Vector2(obj.Width, obj.Height);
                        break;
                    }
                case OBJECT_TYPE.Decal:
                    {
                        Decal obj = DecalList[STATIC_EDITOR_MODE.selectedObjectIndices[STATIC_EDITOR_MODE.selectedObjectIndices.Count - 1].Index];

                        textureDimensions =  new Vector2(obj.Width, obj.Height);
                        break;
                    }
            }

            return textureDimensions;
        }



        #endregion

        private float get_furthest_lpos()
        {
            float furthestPos = 0;
            List<NodeObject> ObjectsList = STATIC_EDITOR_MODE.levelInstance.ObjectsList;
            List<Decal> DecalList = STATIC_EDITOR_MODE.levelInstance.DecalManager.DecalList;

            switch (STATIC_EDITOR_MODE.selectedObjectIndices[0].Type)
            {
                case (OBJECT_TYPE.Physics):
                    {
                        furthestPos = ObjectsList[STATIC_EDITOR_MODE.selectedObjectIndices[0].Index].Position.X - ObjectsList[STATIC_EDITOR_MODE.selectedObjectIndices[0].Index].Width / 2;
                    }
                    break;
                case (OBJECT_TYPE.Decal):
                    {
                        furthestPos = STATIC_EDITOR_MODE.levelInstance.DecalManager.DecalList[STATIC_EDITOR_MODE.selectedObjectIndices[0].Index].Position.X - STATIC_EDITOR_MODE.levelInstance.DecalManager.DecalList[STATIC_EDITOR_MODE.selectedObjectIndices[0].Index].Width / 2;
                    }
                    break;
            }

            for (int i = 0; i < STATIC_EDITOR_MODE.selectedObjectIndices.Count; i++)
            {
                switch (STATIC_EDITOR_MODE.selectedObjectIndices[i].Type)
                {
                    case (OBJECT_TYPE.Physics):
                        {
                            if (furthestPos > ObjectsList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Position.X - ObjectsList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Width / 2)
                                furthestPos = ObjectsList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Position.X - ObjectsList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Width / 2;
                        }
                        break;
                    case (OBJECT_TYPE.Decal):
                        {
                            if (furthestPos > STATIC_EDITOR_MODE.levelInstance.DecalManager.DecalList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Position.X - STATIC_EDITOR_MODE.levelInstance.DecalManager.DecalList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Width / 2)
                                furthestPos = STATIC_EDITOR_MODE.levelInstance.DecalManager.DecalList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Position.X - STATIC_EDITOR_MODE.levelInstance.DecalManager.DecalList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Width / 2;
                        }
                        break;
                }
            }

            return furthestPos;
        }
        private float get_furthest_rpos()
        {
            float furthestPos = 0;
            List<NodeObject> ObjectsList = STATIC_EDITOR_MODE.levelInstance.ObjectsList;
            List<Decal> DecalList = STATIC_EDITOR_MODE.levelInstance.DecalManager.DecalList;

            switch (STATIC_EDITOR_MODE.selectedObjectIndices[0].Type)
            {
                case (OBJECT_TYPE.Physics):
                    {
                        furthestPos = ObjectsList[STATIC_EDITOR_MODE.selectedObjectIndices[0].Index].Position.X + ObjectsList[STATIC_EDITOR_MODE.selectedObjectIndices[0].Index].Width / 2;
                    }
                    break;
                case (OBJECT_TYPE.Decal):
                    {
                        furthestPos = STATIC_EDITOR_MODE.levelInstance.DecalManager.DecalList[STATIC_EDITOR_MODE.selectedObjectIndices[0].Index].Position.X + STATIC_EDITOR_MODE.levelInstance.DecalManager.DecalList[STATIC_EDITOR_MODE.selectedObjectIndices[0].Index].Width / 2;
                    }
                    break;
            }

            for (int i = 0; i < STATIC_EDITOR_MODE.selectedObjectIndices.Count; i++)
            {
                switch (STATIC_EDITOR_MODE.selectedObjectIndices[i].Type)
                {
                    case (OBJECT_TYPE.Physics):
                        {
                            if (furthestPos < ObjectsList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Position.X + ObjectsList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Width / 2)
                                furthestPos = ObjectsList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Position.X + ObjectsList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Width / 2;
                        }
                        break;
                    case (OBJECT_TYPE.Decal):
                        {
                            if (furthestPos < STATIC_EDITOR_MODE.levelInstance.DecalManager.DecalList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Position.X + STATIC_EDITOR_MODE.levelInstance.DecalManager.DecalList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Width / 2)
                                furthestPos = STATIC_EDITOR_MODE.levelInstance.DecalManager.DecalList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Position.X + STATIC_EDITOR_MODE.levelInstance.DecalManager.DecalList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Width / 2;
                        }
                        break;
                }
            }

            return furthestPos;
        }
        private float get_furthest_upos()
        {
            float furthestPos = 0; 
            List<NodeObject> ObjectsList = STATIC_EDITOR_MODE.levelInstance.ObjectsList;
            List<Decal> DecalList = STATIC_EDITOR_MODE.levelInstance.DecalManager.DecalList;

            switch (STATIC_EDITOR_MODE.selectedObjectIndices[0].Type)
            {
                case (OBJECT_TYPE.Physics):
                    {
                        furthestPos = ObjectsList[STATIC_EDITOR_MODE.selectedObjectIndices[0].Index].Position.Y - ObjectsList[STATIC_EDITOR_MODE.selectedObjectIndices[0].Index].Height / 2;
                    }
                    break;
                case (OBJECT_TYPE.Decal):
                    {
                        furthestPos = STATIC_EDITOR_MODE.levelInstance.DecalManager.DecalList[STATIC_EDITOR_MODE.selectedObjectIndices[0].Index].Position.Y - STATIC_EDITOR_MODE.levelInstance.DecalManager.DecalList[STATIC_EDITOR_MODE.selectedObjectIndices[0].Index].Height / 2;
                    }
                    break;
            }

            for (int i = 0; i < STATIC_EDITOR_MODE.selectedObjectIndices.Count; i++)
            {
                switch (STATIC_EDITOR_MODE.selectedObjectIndices[i].Type)
                {
                    case (OBJECT_TYPE.Physics):
                        {
                            if (furthestPos > ObjectsList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Position.Y - ObjectsList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Height / 2)
                                furthestPos = ObjectsList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Position.Y - ObjectsList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Height / 2;
                        }
                        break;
                    case (OBJECT_TYPE.Decal):
                        {
                            if (furthestPos > STATIC_EDITOR_MODE.levelInstance.DecalManager.DecalList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Position.Y - STATIC_EDITOR_MODE.levelInstance.DecalManager.DecalList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Height / 2)
                                furthestPos = STATIC_EDITOR_MODE.levelInstance.DecalManager.DecalList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Position.Y - STATIC_EDITOR_MODE.levelInstance.DecalManager.DecalList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Height / 2;
                        }
                        break;
                }
            }

            return furthestPos;
        }
        private float get_furthest_dpos()
        {
            float furthestPos = 0;
            List<NodeObject> ObjectsList = STATIC_EDITOR_MODE.levelInstance.ObjectsList;
            List<Decal> DecalList = STATIC_EDITOR_MODE.levelInstance.DecalManager.DecalList;

            switch (STATIC_EDITOR_MODE.selectedObjectIndices[0].Type)
            {
                case (OBJECT_TYPE.Physics):
                    {
                        furthestPos = ObjectsList[STATIC_EDITOR_MODE.selectedObjectIndices[0].Index].Position.Y + ObjectsList[STATIC_EDITOR_MODE.selectedObjectIndices[0].Index].Height / 2;
                    }
                    break;
                case (OBJECT_TYPE.Decal):
                    {
                        furthestPos = STATIC_EDITOR_MODE.levelInstance.DecalManager.DecalList[STATIC_EDITOR_MODE.selectedObjectIndices[0].Index].Position.Y + STATIC_EDITOR_MODE.levelInstance.DecalManager.DecalList[STATIC_EDITOR_MODE.selectedObjectIndices[0].Index].Height / 2;
                    }
                    break;
            }

            for (int i = 0; i < STATIC_EDITOR_MODE.selectedObjectIndices.Count; i++)
            {
                switch (STATIC_EDITOR_MODE.selectedObjectIndices[i].Type)
                {
                    case (OBJECT_TYPE.Physics):
                        {
                            if (furthestPos < ObjectsList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Position.Y + ObjectsList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Height / 2)
                                furthestPos = ObjectsList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Position.Y + ObjectsList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Height / 2;
                        }
                        break;
                    case (OBJECT_TYPE.Decal):
                        {
                            if (furthestPos > STATIC_EDITOR_MODE.levelInstance.DecalManager.DecalList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Position.Y + STATIC_EDITOR_MODE.levelInstance.DecalManager.DecalList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Height / 2)
                                furthestPos = STATIC_EDITOR_MODE.levelInstance.DecalManager.DecalList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Position.Y + STATIC_EDITOR_MODE.levelInstance.DecalManager.DecalList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Height / 2;
                        }
                        break;
                }
            }

            return furthestPos;
        }

        #endregion

        #endregion

        #endregion

        #region Texture List Methods

        #region Update TextureList to match changed object type
        private void listBox_Classes_SelectedIndexChanged(object sender, EventArgs e)
        {
            amountOfTexturesNeeded = 0;

            switch (listBox_Classes.Items[listBox_Classes.SelectedIndex].ToString())
            {
                case "Bounce Pad": 
                    break;
                case "Cushioned Platform":
                    amountOfTexturesNeeded = 1;
                    break;
                case "Decal":
                    amountOfTexturesNeeded = 1;
                    break;
                case "Door":
                    amountOfTexturesNeeded = 1;
                    break;
                case "Ladder":
                    amountOfTexturesNeeded = 1;
                    break;
                case "Moving Platform":
                    amountOfTexturesNeeded = 1;
                    break;
                case "Note":
                    amountOfTexturesNeeded = 1;
                    break;
                case "One-Sided Platform": 
                    break;
                case "Particle Emitter":
                    amountOfTexturesNeeded = 1;
                    break;
                case "Piston": 
                    amountOfTexturesNeeded = 2;
                    break;
                case "Physics Object":
                    amountOfTexturesNeeded = 1;
                    break;
                case "Pushing Platform":
                    amountOfTexturesNeeded = 1;
                    break;
                case "Rope":
                    amountOfTexturesNeeded = 2;
                    break;
                case "Rotate Room Button":
                    amountOfTexturesNeeded = 1;
                    break;
                case "Rotating Platform":
                    amountOfTexturesNeeded = 1;
                    break;
                case "Saw":
                    amountOfTexturesNeeded = 2;
                    break;
                case "Spikes":
                    amountOfTexturesNeeded = 1;
                    break;
                case "Sprite":
                    amountOfTexturesNeeded = 1;
                    break;
                case "Static Object":
                    amountOfTexturesNeeded = 1;
                    break;
                default:
                    amountOfTexturesNeeded = -1;
                    break;
            }


            if (amountOfTexturesNeeded == 1)
            {
                assetLocTextBox1.BackColor = System.Drawing.Color.FromArgb(255, 128, 128);
                assetLocTextBox2.BackColor = System.Drawing.Color.FromName("Control");
                assetLocTextBox3.BackColor = System.Drawing.Color.FromName("Control");
            }
            else if (amountOfTexturesNeeded == 2)
            {
                assetLocTextBox1.BackColor = System.Drawing.Color.FromArgb(255, 128, 128);
                assetLocTextBox2.BackColor = System.Drawing.Color.FromArgb(255, 128, 128);
                assetLocTextBox3.BackColor = System.Drawing.Color.FromName("Control");
            }
            else if (amountOfTexturesNeeded == 3)
            {
                assetLocTextBox1.BackColor = System.Drawing.Color.FromArgb(255, 128, 128);
                assetLocTextBox2.BackColor = System.Drawing.Color.FromArgb(255, 128, 128);
                assetLocTextBox3.BackColor = System.Drawing.Color.FromArgb(255, 128, 128);
            }
            else
            {
                assetLocTextBox1.BackColor = System.Drawing.Color.FromName("Control");
                assetLocTextBox2.BackColor = System.Drawing.Color.FromName("Control");
                assetLocTextBox3.BackColor = System.Drawing.Color.FromName("Control");
            }

            #region Selected-Class Texture Switch
            //switch (listBox_Classes.Items[listBox_Classes.SelectedIndex].ToString())
            //{
            //    case "Cushioned Platform":
            //        STATIC_CONTBUILDER.textureLoc = "Assets/Images/Textures/Environment/";
            //        break;
            //    case "Decal":
            //        STATIC_CONTBUILDER.textureLoc = "Assets/Images/Textures/Decal/";
            //        break;
            //    case "Door":
            //        STATIC_CONTBUILDER.textureLoc = "Assets/Images/Textures/Doors/";
            //        break;
            //    case "Ladder":
            //        STATIC_CONTBUILDER.textureLoc = "Assets/Images/Textures/Ladder/";
            //        break;
            //    case "Moving Platform":
            //        STATIC_CONTBUILDER.textureLoc = "Assets/Images/Textures/Environment/";
            //        break;
            //    case "One-Sided Platform":
            //        STATIC_CONTBUILDER.textureLoc = "Assets/Images/Textures/Environment/";
            //        break;
            //    case "Particle Emitter":
            //        STATIC_CONTBUILDER.textureLoc = "Assets/Images/Effects/";
            //        break;
            //    case "Piston":
            //        STATIC_CONTBUILDER.textureLoc = "Assets/Images/Textures/Piston/";
            //        break;
            //    case "Rope":
            //        STATIC_CONTBUILDER.textureLoc = "Assets/Images/Textures/Rope/";
            //        break;
            //    case "Rotate Room Button":
            //        STATIC_CONTBUILDER.textureLoc = "Assets/Images/Textures/Switch/";
            //        break;
            //    case "Rotating Platform":
            //        STATIC_CONTBUILDER.textureLoc = "Assets/Images/Textures/Environment/";
            //        break;
            //    case "Saw Blade":
            //        STATIC_CONTBUILDER.textureLoc = "Assets/Images/Textures/Saw/";
            //        break;
            //    case "Spikes":
            //        STATIC_CONTBUILDER.textureLoc = "Assets/Images/Textures/Spikes/";
            //        break;
            //    case "Static Object":
            //        STATIC_CONTBUILDER.textureLoc = "Assets/Images/Textures/Environment/";
            //        break;
            //    default:
            //        STATIC_CONTBUILDER.textureLoc = "Assets/Images/Textures/";
            //        break;
            //}


            //Refresh_XNB_Asset_List();
            #endregion
        }
        #endregion

        #endregion

        #region Camera Methods
        Point get_mouse_vpos()
        {
            Point topLeftPoint = xnA_RenderControl1.Camera.GetTopLeftCameraPoint(xnA_RenderControl1.GraphicsDevice);

            Point mousePoint =
                new Point(
                    (int)(Mouse.GetState().X / xnA_RenderControl1.Camera.Zoom + topLeftPoint.X),
                    (int)(Mouse.GetState().Y / xnA_RenderControl1.Camera.Zoom + topLeftPoint.Y));
            return (mousePoint);
        }

        #region Scrollbars
        void vScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
            xnA_RenderControl1.Camera.Pos = new Microsoft.Xna.Framework.Vector2(xnA_RenderControl1.Camera.Pos.X, vScrollBar1.Value);

            this.Refresh();
        }

        void hScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
            xnA_RenderControl1.Camera.Pos = new Microsoft.Xna.Framework.Vector2(hScrollBar1.Value, xnA_RenderControl1.Camera.Pos.Y);

            this.Refresh();
        }
        #endregion

        #endregion

        #region Toolstrip

        void SwitchToSelectMode()
        {
            if (!BUTTON_EDITOR_MODE_SELECT.Checked)
            {
                BUTTON_EDITOR_MODE_SELECT.Checked = true;

                STATIC_EDITOR_MODE.ED_MODE = EDITOR_MODE.SELECT;
            }

            BUTTON_EDITOR_MODE_MOVE.Checked = BUTTON_EDITOR_MODE_PLACE.Checked = BUTTON_EDIT_ROOM.Checked = false;
            Handle_Property_Grid_Items();
        }

        void SwitchToMoveMode()
        {
            if (!BUTTON_EDITOR_MODE_MOVE.Checked)
            {
                BUTTON_EDITOR_MODE_MOVE.Checked = true;

                STATIC_EDITOR_MODE.ED_MODE = EDITOR_MODE.MOVE;
            }

            BUTTON_EDITOR_MODE_SELECT.Checked = BUTTON_EDITOR_MODE_PLACE.Checked = BUTTON_EDIT_ROOM.Checked = false;
            Handle_Property_Grid_Items();
        }

        void SwitchToPlaceMode()
        {
            if (!BUTTON_EDITOR_MODE_PLACE.Checked)
            {
                BUTTON_EDITOR_MODE_PLACE.Checked = true;

                STATIC_EDITOR_MODE.ED_MODE = EDITOR_MODE.PLACE;
            }

            BUTTON_EDITOR_MODE_SELECT.Checked = BUTTON_EDITOR_MODE_MOVE.Checked = BUTTON_EDIT_ROOM.Checked = false;
            Handle_Property_Grid_Items();
        }

        void SwitchToRoomProperties()
        {
            BUTTON_EDIT_ROOM.Checked = true;

            STATIC_EDITOR_MODE.ED_MODE = EDITOR_MODE.EDIT_LEVEL;

            BUTTON_EDITOR_MODE_SELECT.Checked = BUTTON_EDITOR_MODE_MOVE.Checked = BUTTON_EDITOR_MODE_PLACE.Checked = false;
            Handle_Property_Grid_Items();
        }

        void DeleteSelectedObjects()
        {
            if (Is_Something_Selected())
            {
                //  Sort them by Index and work backwards.. left hassle.
                STATIC_EDITOR_MODE.selectedObjectIndices.Sort(delegate(ObjectIndex objA, ObjectIndex objB)
                {
                    return objA.Index.CompareTo(objB.Index);
                });

                for (int i = STATIC_EDITOR_MODE.selectedObjectIndices.Count; i > 0; i--)
                {
                    switch (STATIC_EDITOR_MODE.selectedObjectIndices[i - 1].Type)
                    {
                        case (OBJECT_TYPE.Physics):
                            {
                                STATIC_EDITOR_MODE.levelInstance.ObjectsList.RemoveAt(STATIC_EDITOR_MODE.selectedObjectIndices[i - 1].Index);
                            }
                            break;
                        case (OBJECT_TYPE.Decal):
                            {
                                STATIC_EDITOR_MODE.levelInstance.DecalManager.DecalList.RemoveAt(STATIC_EDITOR_MODE.selectedObjectIndices[i - 1].Index);
                            }
                            break;
                    }
                }

                STATIC_EDITOR_MODE.selectedObjectIndices.Clear();
                //STATIC_EDITOR_MODE.selectedObjectIndex = -1;
                propertyGrid1.SelectedObject = false;

                //Update_undoArray();
            }
        }

        #endregion

        #region User Input Methods

        void xnA_RenderControl1_MouseMove(object sender, MouseEventArgs e)
        {
            xnA_RenderControl1.UpdateCoOrds();

            Point MousePos = get_mouse_vpos();

            lst_ObjectsUnderCursor.Clear();

            switch (STATIC_EDITOR_MODE.ED_MODE)
            {
                case EDITOR_MODE.SELECT:
                    {
                        #region Handle Objects Under Cursor
                        if (STATIC_EDITOR_MODE.levelInstance.ObjectsList.Count > 0)
                        {
                            for (int i = 0; i < STATIC_EDITOR_MODE.levelInstance.ObjectsList.Count; i++)
                            {
                                Rectangle BoundingBox = new Rectangle(
                                    (int)(STATIC_EDITOR_MODE.levelInstance.ObjectsList[i].Position.X - STATIC_EDITOR_MODE.levelInstance.ObjectsList[i].Width * 0.5f),
                                    (int)(STATIC_EDITOR_MODE.levelInstance.ObjectsList[i].Position.Y - STATIC_EDITOR_MODE.levelInstance.ObjectsList[i].Height * 0.5f),
                                    (int)(STATIC_EDITOR_MODE.levelInstance.ObjectsList[i].Width),
                                    (int)(STATIC_EDITOR_MODE.levelInstance.ObjectsList[i].Height));

                                if (BoundingBox.Contains(MousePos))
                                {
                                    lst_ObjectsUnderCursor.Add(new ObjectIndex(OBJECT_TYPE.Physics, i, STATIC_EDITOR_MODE.levelInstance.ObjectsList[i].ZLayer));
                                }
                            }
                        }

                        if (STATIC_EDITOR_MODE.levelInstance.DecalManager.DecalList.Count > 0)
                        {
                            for (int i = 0; i < STATIC_EDITOR_MODE.levelInstance.DecalManager.DecalList.Count; i++)
                            {
                                float width = STATIC_EDITOR_MODE.levelInstance.DecalManager.DecalList[i].Width * STATIC_EDITOR_MODE.levelInstance.DecalManager.DecalList[i].Scale;
                                float height = STATIC_EDITOR_MODE.levelInstance.DecalManager.DecalList[i].Height * STATIC_EDITOR_MODE.levelInstance.DecalManager.DecalList[i].Scale;

                                Rectangle BoundingBox = new Rectangle(
                                    (int)(STATIC_EDITOR_MODE.levelInstance.DecalManager.DecalList[i].Position.X - width * 0.5f),
                                    (int)(STATIC_EDITOR_MODE.levelInstance.DecalManager.DecalList[i].Position.Y - height * 0.5f),
                                    (int)width,
                                    (int)height);

                                if (BoundingBox.Contains(MousePos))
                                {
                                    lst_ObjectsUnderCursor.Add(new ObjectIndex(OBJECT_TYPE.Decal, i, STATIC_EDITOR_MODE.levelInstance.DecalManager.DecalList[i].ZLayer));
                                }
                            }
                        }
                        #endregion

                        //  Sort them out by their Zlayer
                        lst_ObjectsUnderCursor.Sort(delegate(ObjectIndex objA, ObjectIndex objB)
                            {
                                return objA.ZLayer.CompareTo(objB.ZLayer);
                            });

                        }
                    break;
                case EDITOR_MODE.MOVE:
                    {
                        #region Handle Moving Objects
                        if (Is_Something_Selected())
                        {
                            for (int i = STATIC_EDITOR_MODE.selectedObjectIndices.Count - 1; i >= 0 && containsMouse == false; i--)
                            {
                                Rectangle newRect = new Rectangle();
                                switch (STATIC_EDITOR_MODE.selectedObjectIndices[i].Type)
                                {
                                    case (OBJECT_TYPE.Physics):
                                        {
                                            newRect = new Rectangle(
                                                    (int)STATIC_EDITOR_MODE.levelInstance.ObjectsList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Position.X - (int)(STATIC_EDITOR_MODE.levelInstance.ObjectsList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Width * 0.5f),
                                                    (int)STATIC_EDITOR_MODE.levelInstance.ObjectsList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Position.Y - (int)(STATIC_EDITOR_MODE.levelInstance.ObjectsList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Height * 0.5f),
                                                    (int)STATIC_EDITOR_MODE.levelInstance.ObjectsList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Width,
                                                    (int)STATIC_EDITOR_MODE.levelInstance.ObjectsList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Height);
                                        }
                                        break;
                                    case (OBJECT_TYPE.Decal):
                                        {
                                            newRect = new Rectangle(
                                                    (int)STATIC_EDITOR_MODE.levelInstance.DecalManager.DecalList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Position.X - (int)(STATIC_EDITOR_MODE.levelInstance.DecalManager.DecalList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Width * 0.5f),
                                                    (int)STATIC_EDITOR_MODE.levelInstance.DecalManager.DecalList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Position.Y - (int)(STATIC_EDITOR_MODE.levelInstance.DecalManager.DecalList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Height * 0.5f),
                                                    (int)STATIC_EDITOR_MODE.levelInstance.DecalManager.DecalList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Width,
                                                    (int)STATIC_EDITOR_MODE.levelInstance.DecalManager.DecalList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Height);
                                        }
                                        break;
                                }

                                if (newRect.Contains(MousePos))
                                {
                                    containsMouse = true;
                                }
                            }

                            if (containsMouse == true && Microsoft.Xna.Framework.Input.Mouse.GetState().LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
                            {
                                if (mouseDown != Vector2.Zero)
                                {
                                    Vector2 moveDis = new Vector2(get_mouse_vpos().X, get_mouse_vpos().Y);
                                    moveDis -= mouseDown;
                                    for (int i = STATIC_EDITOR_MODE.selectedObjectIndices.Count - 1; i >= 0; i--)
                                    {
                                        switch (STATIC_EDITOR_MODE.selectedObjectIndices[i].Type)
                                        {
                                            case (OBJECT_TYPE.Physics):
                                                {
                                                    STATIC_EDITOR_MODE.levelInstance.ObjectsList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Position += moveDis;
                                                    Type t = STATIC_EDITOR_MODE.levelInstance.ObjectsList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].GetType();

                                                    if (t.BaseType == typeof(DynamicObject))
                                                    {
                                                        DynamicObject dyOb = (DynamicObject)STATIC_EDITOR_MODE.levelInstance.ObjectsList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index];
                                                        dyOb.EndPosition += moveDis;
                                                        STATIC_EDITOR_MODE.levelInstance.ObjectsList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index] = dyOb;
                                                    }
                                                }
                                                break;
                                            case (OBJECT_TYPE.Decal):
                                                {
                                                    STATIC_EDITOR_MODE.levelInstance.DecalManager.DecalList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Position += moveDis;
                                                }
                                                break;
                                        }
                                    }
                                    mouseDown = Vector2.Zero;
                                }

                                if (mouseDown == Vector2.Zero)
                                {
                                    mouseDown = new Vector2(get_mouse_vpos().X, get_mouse_vpos().Y);
                                }

                                dragReleased = false;
                            }

                            if (Microsoft.Xna.Framework.Input.Mouse.GetState().LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Released)
                            {
                                mouseDown = Vector2.Zero;
                                containsMouse = false;

                                if (dragReleased == false)
                                {
                                    dragReleased = true;
                                    Update_undoArray();
                                    Handle_Property_Grid_Items();
                                }
                            }
                        }
                        #endregion
                    }
                    break;
            }
        }

        void xnA_RenderControl1_MouseDown(object sender, MouseEventArgs e)
        {
            Microsoft.Xna.Framework.Point MousePos = get_mouse_vpos();
            STATIC_EDITOR_MODE.keyboardCurrentState = Keyboard.GetState();

            switch (STATIC_EDITOR_MODE.ED_MODE)
            {
                case EDITOR_MODE.SELECT:
                    {
                        if (lst_ObjectsUnderCursor.Count > 0)
                        {
                            //carry out depth tests or other logic here if we are mousing over more than one object
                            if (STATIC_EDITOR_MODE.keyboardCurrentState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.LeftShift) || STATIC_EDITOR_MODE.keyboardCurrentState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.RightShift))
                            {
                                ShiftSelection();
                            }
                            else
                            {
                                STATIC_EDITOR_MODE.selectedObjectIndices.Clear();
                                STATIC_EDITOR_MODE.selectedObjectIndices.Add(lst_ObjectsUnderCursor[0]);
                            }
                        }
                        else
                        {
                            //nothing under cursor so deselect any selected objects
                            STATIC_EDITOR_MODE.selectedObjectIndices.Clear();
                        }
                    }
                    break;
                case EDITOR_MODE.PLACE:
                    {
                        CreateObject(listBox_Classes.Items[listBox_Classes.SelectedIndex].ToString(), new Vector2(MousePos.X, MousePos.Y));

                        Update_undoArray();
                    }
                    break;
            }

            Handle_Property_Grid_Items();
        }

        void XNA_RenderControl_MouseWheel(object sender, MouseEventArgs e)
        {
            int num = 0;
            if (e.Delta > 0)
                num = 1;
            else if (e.Delta < 0)
                num = -1;

            if (STATIC_EDITOR_MODE.keyboardCurrentState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.LeftAlt))
            {
                xnA_RenderControl1.Camera.Zoom += (0.03f * num);
            }
            else if (STATIC_EDITOR_MODE.keyboardCurrentState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.LeftControl))
            {
                if (num == 1)
                {
                    this.hScrollBar1.Value -= 5;
                    this.hScrollBar1_Scroll(null, null);
                }
                else
                {
                    this.hScrollBar1.Value += 5;
                    this.hScrollBar1_Scroll(null, null);
                }
            }
            else
            {
                if (num == 1)
                {
                    vScrollBar1.Value -= 5;
                    this.vScrollBar1_Scroll(null, null);
                }
                else
                {
                    vScrollBar1.Value += 5;
                    this.vScrollBar1_Scroll(null, null);
                }
            }
        }

        void CheckInput(GameTime gameTime)
        {
            STATIC_EDITOR_MODE.mouseCurrentState = Mouse.GetState();
            STATIC_EDITOR_MODE.keyboardCurrentState = Keyboard.GetState();

            #region Editor Toolstrip
            if (xnA_RenderControl1.Focused)
            {
                if (CheckNewKey(Microsoft.Xna.Framework.Input.Keys.Q))
                {
                    SwitchToSelectMode();
                }

                if (CheckNewKey(Microsoft.Xna.Framework.Input.Keys.W))
                {
                    SwitchToMoveMode();
                }

                if (CheckNewKey(Microsoft.Xna.Framework.Input.Keys.E))
                {
                    SwitchToPlaceMode();
                }

                if (CheckNewKey(Microsoft.Xna.Framework.Input.Keys.Delete))
                {
                    DeleteSelectedObjects();
                }

                if (CheckNewKey(Microsoft.Xna.Framework.Input.Keys.C) && STATIC_EDITOR_MODE.keyboardCurrentState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.LeftControl))
                {
                    CopyPaste();
                }

                if (CheckNewKey(Microsoft.Xna.Framework.Input.Keys.D) && STATIC_EDITOR_MODE.keyboardCurrentState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.LeftControl))
                {
                    STATIC_EDITOR_MODE.selectedObjectIndices.Clear();
                }
            }
            #endregion

            if (xnA_RenderControl1.Focused)
            {
                if (CheckNewKey(Microsoft.Xna.Framework.Input.Keys.S) && STATIC_EDITOR_MODE.keyboardCurrentState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.LeftControl))
                {
                    SaveFile();
                }

                #region Undo / Redo

                if (CheckNewKey(Microsoft.Xna.Framework.Input.Keys.Z) && STATIC_EDITOR_MODE.keyboardCurrentState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.LeftControl))
                {
                    Undo();
                }

                if (CheckNewKey(Microsoft.Xna.Framework.Input.Keys.Y) && STATIC_EDITOR_MODE.keyboardCurrentState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.LeftControl))
                {
                    Redo();
                }
            }
            #endregion

            #region Toggles

            if (CheckNewKey(Microsoft.Xna.Framework.Input.Keys.F1))
            {
                this.ViewMenuHideCoordinates_Click(null, null);
            }

            if (CheckNewKey(Microsoft.Xna.Framework.Input.Keys.F2))
            {
                this.ViewMenuHideGrid_Click(null, null);
            }

            if (CheckNewKey(Microsoft.Xna.Framework.Input.Keys.F3))
            {
                this.ViewMenuHideMovementPath_Click(null, null);
            }

            if (CheckNewKey(Microsoft.Xna.Framework.Input.Keys.F4))
            {
                this.ViewMenuHideOverlay_Click(null, null);
            }
            #endregion

            STATIC_EDITOR_MODE.keyboardOldState = STATIC_EDITOR_MODE.keyboardCurrentState;
            STATIC_EDITOR_MODE.mouseOldState = STATIC_EDITOR_MODE.mouseCurrentState;
        }

        bool CheckNewKey(Microsoft.Xna.Framework.Input.Keys key)
        {
            if (STATIC_EDITOR_MODE.keyboardCurrentState.IsKeyDown(key) && STATIC_EDITOR_MODE.keyboardOldState.IsKeyUp(key))
                return true;
            return false;
        }

        #endregion

        #endregion

        #region Texture Selections
        private void assetSelectDialog1_Click(object sender, EventArgs e)
        {
            openAssetFileDialog1.Title = "Select Texture Asset";
            openAssetFileDialog1.Filter = "PNG Files (*.png)|*.png|" +
                                     "JPEG Files (*.jpg)|*.jpg";


            if (openAssetFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string location = openAssetFileDialog1.FileName;
                string removeAfter = "ALL ASSETS\\";
                int startFrom = location.IndexOf(removeAfter);
                string newloc = String.Empty;

                for (int i = startFrom + removeAfter.Length; i < location.Length - 4; i++)
                {
                    char newchar = location[i];
                    if (newchar == '\\' && newloc[newloc.Length - 1] == '\\') 
                    {
                    }
                    else 
                        newloc += newchar;
                }
                assetLocTextBox1.Text = newloc;
                assetLocTextBox1.BackColor = System.Drawing.Color.FromArgb(128, 255, 128);
            }
        }

        private void assetSelectDialog2_Click(object sender, EventArgs e)
        {
            openAssetFileDialog1.Title = "Select Texture Asset";
            openAssetFileDialog1.Filter = "PNG Files (*.png)|*.png|" +
                                     "JPEG Files (*.jpg)|*.jpg";


            if (openAssetFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string location = openAssetFileDialog1.FileName;
                string removeAfter = "ALL ASSETS\\";
                int startFrom = location.IndexOf(removeAfter);
                string newloc = String.Empty;

                for (int i = startFrom + removeAfter.Length; i < location.Length - 4; i++)
                {
                    char newchar = location[i];
                    if (newchar == '\\' && newloc[newloc.Length - 1] == '\\')
                    {
                    }
                    else
                        newloc += newchar;
                }
                assetLocTextBox2.Text = newloc;
                assetLocTextBox2.BackColor = System.Drawing.Color.FromArgb(128, 255, 128);
            }
        }

        private void assetSelectDialog3_Click(object sender, EventArgs e)
        {
            openAssetFileDialog1.Title = "Select Texture Asset";
            openAssetFileDialog1.Filter = "PNG Files (*.png)|*.png|" +
                                     "JPEG Files (*.jpg)|*.jpg";


            if (openAssetFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string location = openAssetFileDialog1.FileName;
                string removeAfter = "ALL ASSETS\\";
                int startFrom = location.IndexOf(removeAfter);
                string newloc = String.Empty;

                for (int i = startFrom + removeAfter.Length; i < location.Length - 4; i++)
                {
                    char newchar = location[i];
                    if (newchar == '\\' && newloc[newloc.Length - 1] == '\\')
                    {
                    }
                    else
                        newloc += newchar;
                }
                assetLocTextBox3.Text = newloc;
                assetLocTextBox3.BackColor = System.Drawing.Color.FromArgb(128, 255, 128);
            }
        }
        #endregion
        
    }
}
