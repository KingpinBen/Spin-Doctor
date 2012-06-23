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
using GameLibrary.Assists;
using GameLibrary.Drawing;
using GameLibrary.Objects;
using FarseerPhysics.Dynamics;
using GameLibrary.Objects.Triggers;
#endregion

namespace SpinEditor
{
    public partial class Form1 : Form
    {
        #region Properties
        public static List<ObjectIndex> ListThing
        {
            get { return lst_ObjectsUnderCursor; }
        }
        #endregion

        #region Fields

        static List<ObjectIndex> lst_ObjectsUnderCursor;
        Vector2 mouseDown = Vector2.Zero;
        Vector2 mouseUp= Vector2.Zero;
        bool containsMouse = false;
        bool dragReleased = true;
        #endregion

        #region Constructor and Load

        public Form1()
        {
            InitializeComponent();

            STATIC_EDITOR_MODE.world = new World(new Vector2(0, 0));
            
            //STATIC_EDITOR_MODE.levelInstance = new GameLibrary.Drawing.Level();
            STATIC_EDITOR_MODE.undoPhysObjArray = new PhysicsObject[STATIC_EDITOR_MODE.arrayMax][];
            this.MaximizeBox = false;
            lst_ObjectsUnderCursor = new List<ObjectIndex>();

            hScrollBar1.Minimum = -2000;
            vScrollBar1.Minimum = -2000;
            hScrollBar1.Maximum = 2000;
            vScrollBar1.Maximum = 2000;

            Handle_Property_Grid_Items();

            //  Allows access to public methods from xna_renderControl, such as Update().
            xnA_RenderControl1.form1 = this;

            //  Hook some events,
            this.SizeChanged += xnA_RenderControl1.WindowChangedSize;
            this.ClientSizeChanged += xnA_RenderControl1.WindowChangedSize;
            this.MouseWheel += new MouseEventHandler(XNA_RenderControl_MouseWheel);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            ZOOM.Value = (decimal)(xnA_RenderControl1.Camera.Zoom * 100);

            //Refresh_XNB_Asset_List();

            listBox_Classes.SelectedIndex = 0;
            listBox_Assets0.SelectedItem = "icon";
            listBox_Assets1.SelectedItem = "icon";
            listBox_Assets2.SelectedItem = "icon";
            Align_Relative.SelectedItem = "Last Selected";
            Rotate_Relative.SelectedItem = "As Group";

            NewFile();

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
            if ((listBox_Classes.SelectedItem != null) && (listBox_Assets0.SelectedItem != null))
            {
                //  All textures are loaded inside each object.
                string texloc0 = STATIC_CONTBUILDER.textureLoc + listBox_Assets0.Items[listBox_Assets0.SelectedIndex].ToString();
                string texloc1 = STATIC_CONTBUILDER.textureLoc + listBox_Assets1.Items[listBox_Assets0.SelectedIndex].ToString();
                string texloc2 = STATIC_CONTBUILDER.textureLoc + listBox_Assets2.Items[listBox_Assets0.SelectedIndex].ToString();
                string texloc3 = STATIC_CONTBUILDER.textureLoc + listBox_Assets3.Items[listBox_Assets0.SelectedIndex].ToString();

                #region Object Types
                switch (Type)
                {
                    //case "Bounce Pad":
                    //    {
                    //        BouncePad bouncePad = new BouncePad();
                    //        bouncePad.Init(Position, texloc0);
                    //        bouncePad.Load(xnA_RenderControl1.contentMan, STATIC_EDITOR_MODE.world);
                    //        STATIC_EDITOR_MODE.levelInstance.PhysicsObjectsList.Add(bouncePad);
                    //    }
                    //    break;
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
                            STATIC_EDITOR_MODE.levelInstance.PhysicsObjectsList.Add(door);
                        }
                        break;
                    case "Ladder":
                        {
                            Ladder ladder = new Ladder();
                            ladder.Init(Position, texloc0);
                            ladder.Load(xnA_RenderControl1.contentMan, STATIC_EDITOR_MODE.world);
                            STATIC_EDITOR_MODE.levelInstance.PhysicsObjectsList.Add(ladder);
                        }
                        break;
                    case "Moving Platform":
                        {
                            MovingPlatform movPlat = new MovingPlatform();
                            movPlat.Init(Position, texloc0);
                            movPlat.Load(xnA_RenderControl1.contentMan, STATIC_EDITOR_MODE.world);
                            STATIC_EDITOR_MODE.levelInstance.PhysicsObjectsList.Add(movPlat);
                        }
                        break;
                    //case "Note":
                    //    {
                    //        Note shiny = new Note();
                    //        shiny.Init(Position, texloc0);
                    //        shiny.Load(xnA_RenderControl1.contentMan, STATIC_EDITOR_MODE.world);
                    //        STATIC_EDITOR_MODE.levelInstance.PhysicsObjectsList.Add(shiny);
                    //    }
                    //    break;
                    //case "One-Sided Platform":
                    //    {
                    //        OneSidedPlatform oneSidePlat = new OneSidedPlatform();
                    //        oneSidePlat.Init(Position, tex.Width, tex.Height, texloc0);
                    //        oneSidePlat.Load(xnA_RenderControl1.contentMan, STATIC_EDITOR_MODE.world);
                    //        STATIC_EDITOR_MODE.levelInstance.PhysicsObjectsList.Add(oneSidePlat);
                    //    }
                    //    break;
                    case "Piston":
                        {
                            Piston piston = new Piston();
                            piston.Init(Position, texloc0, texloc1);
                            piston.Load(xnA_RenderControl1.contentMan, STATIC_EDITOR_MODE.world);
                            STATIC_EDITOR_MODE.levelInstance.PhysicsObjectsList.Add(piston);
                        }
                        break;
                    //case "Pullable Object":
                    //    {
                    //        PullableObject pullObj = new PullableObject();
                    //        pullObj.Init(Position, tex.Width, tex.Height, texloc0);
                    //        pullObj.Load(xnA_RenderControl1.contentMan, STATIC_EDITOR_MODE.world);
                    //        STATIC_EDITOR_MODE.levelInstance.PhysicsObjectsList.Add(pullObj);
                    //    }
                    //    break;
                    case "Pushing Platform":
                        {
                            PushingPlatform pushPlat = new PushingPlatform();
                            pushPlat.Init(Position, 100, 20, texloc0, "Assets/Sprites/Effects/explosion");
                            pushPlat.Load(xnA_RenderControl1.contentMan, STATIC_EDITOR_MODE.world);
                            STATIC_EDITOR_MODE.levelInstance.PhysicsObjectsList.Add(pushPlat);
                        }
                        break;
                    case "Rope":
                        {
                            Rope rope = new Rope();
                            rope.Init(Position, texloc0);
                            rope.Load(xnA_RenderControl1.contentMan, STATIC_EDITOR_MODE.world);
                            STATIC_EDITOR_MODE.levelInstance.PhysicsObjectsList.Add(rope);
                        }
                        break;
                    case "Rotate Room Button":
                        {
                            RotateRoomButton rotRoomButton = new RotateRoomButton();
                            rotRoomButton.Init(Position, texloc0);
                            rotRoomButton.Load(xnA_RenderControl1.contentMan, STATIC_EDITOR_MODE.world);
                            STATIC_EDITOR_MODE.levelInstance.PhysicsObjectsList.Add(rotRoomButton);
                        }
                        break;
                    case "Rotating Platform":
                        {
                            RotatingPlatform rotPlat = new RotatingPlatform();
                            rotPlat.Init(Position, texloc0);
                            rotPlat.Load(xnA_RenderControl1.contentMan, STATIC_EDITOR_MODE.world);
                            STATIC_EDITOR_MODE.levelInstance.PhysicsObjectsList.Add(rotPlat);
                        }
                        break;
                    case "Saw Blade":
                        {
                            if (listBox_Assets1.SelectedItem == null) return;
                            Saw sawBlade = new Saw();
                            sawBlade.Init(Position, texloc0, texloc1);
                            sawBlade.Load(xnA_RenderControl1.contentMan, STATIC_EDITOR_MODE.world);
                            STATIC_EDITOR_MODE.levelInstance.PhysicsObjectsList.Add(sawBlade);
                        }
                        break;
                    case "Spikes":
                        {
                            Spike spike = new Spike();
                            spike.Init(Position, texloc0);
                            spike.Load(xnA_RenderControl1.contentMan, STATIC_EDITOR_MODE.world);
                            STATIC_EDITOR_MODE.levelInstance.PhysicsObjectsList.Add(spike);
                        }
                        break;
                    case "Static Object":
                        {
                            StaticObject staticObj = new StaticObject();
                            staticObj.Init(Position, texloc0);
                            staticObj.Load(xnA_RenderControl1.contentMan, STATIC_EDITOR_MODE.world);
                            STATIC_EDITOR_MODE.levelInstance.PhysicsObjectsList.Add(staticObj);
                        }
                        break;
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
                            STATIC_EDITOR_MODE.selectedObjectIndices.Add(new ObjectIndex(OBJECT_TYPE.Decal, STATIC_EDITOR_MODE.levelInstance.DecalManager.DecalList.Count - 1));
                        }
                        break;
                    case "Physics":
                        {
                            if (!STATIC_EDITOR_MODE.keyboardCurrentState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.LeftShift) && !STATIC_EDITOR_MODE.keyboardCurrentState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.RightShift))
                            {
                                STATIC_EDITOR_MODE.selectedObjectIndices.Clear();
                            }
                            STATIC_EDITOR_MODE.selectedObjectIndices.Add(new ObjectIndex(OBJECT_TYPE.Physics, STATIC_EDITOR_MODE.levelInstance.PhysicsObjectsList.Count - 1));
                        }
                        break;
                }
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
                    STATIC_EDITOR_MODE.undoPhysObjArray[i] = null;
                    STATIC_EDITOR_MODE.arrayLength--;
                }
            }
            if (STATIC_EDITOR_MODE.arrayLength == STATIC_EDITOR_MODE.arrayMax)
            {
                for (int i = 0; i < STATIC_EDITOR_MODE.arrayMax - 1; i++)
                {
                    STATIC_EDITOR_MODE.undoPhysObjArray[i] = STATIC_EDITOR_MODE.undoPhysObjArray[i + 1];
                }
                STATIC_EDITOR_MODE.undoPhysObjArray[STATIC_EDITOR_MODE.arrayLength - 1] = null;
                STATIC_EDITOR_MODE.arrayLength--;
            }
            //STATIC_EDITOR_MODE.undoPhysObjArray[STATIC_EDITOR_MODE.arrayLength] = STATIC_EDITOR_MODE.levelInstance.PhysicsObjectsList.ToArray();
            
            STATIC_EDITOR_MODE.undoPhysObjArray[STATIC_EDITOR_MODE.arrayLength] = new PhysicsObject[STATIC_EDITOR_MODE.levelInstance.PhysicsObjectsList.Count];
            for (int i = 0; i < STATIC_EDITOR_MODE.levelInstance.PhysicsObjectsList.Count; i++)
            {
                STATIC_EDITOR_MODE.undoPhysObjArray[STATIC_EDITOR_MODE.arrayLength][i] = STATIC_EDITOR_MODE.levelInstance.PhysicsObjectsList[i];
            }

            STATIC_EDITOR_MODE.arrayLength++;
            STATIC_EDITOR_MODE.arrayIndex = STATIC_EDITOR_MODE.arrayLength - 1;
        }

        void Undo()
        {
            if (STATIC_EDITOR_MODE.arrayIndex > 0)
            {
                STATIC_EDITOR_MODE.arrayIndex--;
                STATIC_EDITOR_MODE.levelInstance.PhysicsObjectsList = null;
                STATIC_EDITOR_MODE.levelInstance.PhysicsObjectsList = STATIC_EDITOR_MODE.undoPhysObjArray[STATIC_EDITOR_MODE.arrayIndex].ToList();
                STATIC_EDITOR_MODE.selectedObjectIndices.Clear();
                Handle_Property_Grid_Items();
            }
        }

        void Redo()
        {
            if (STATIC_EDITOR_MODE.arrayLength - 1 > STATIC_EDITOR_MODE.arrayIndex)
            {
                STATIC_EDITOR_MODE.arrayIndex++;
                STATIC_EDITOR_MODE.levelInstance.PhysicsObjectsList = STATIC_EDITOR_MODE.undoPhysObjArray[STATIC_EDITOR_MODE.arrayIndex].ToList();
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

        #region Editting Mode Select

        #region Change to Select Mode
        private void BUTTON_EDITOR_MODE_SELECT_Click(object sender, EventArgs e)
        {
            SwitchToSelectMode();
        }
        #endregion

        #region Change to Move Mode
        private void BUTTON_EDITOR_MODE_MOVE_Click(object sender, EventArgs e)
        {
            SwitchToMoveMode();
        }
        #endregion

        #region Change to Place Mode
        private void BUTTON_EDITOR_MODE_PLACE_Click(object sender, EventArgs e)
        {
            SwitchToPlaceMode();
        }
        #endregion

        #region Show Room Properties
        private void BUTTON_EDIT_ROOM_Click(object sender, EventArgs e)
        {
            SwitchToRoomProperties();
        }
        #endregion

        #region Delete Selected Item(s)
        void BUTTON_DELETE_Click(object sender, EventArgs e)
        {
            DeleteSelectedObjects();
        }
        #endregion

        #region SubMethods

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
                for (int i = STATIC_EDITOR_MODE.selectedObjectIndices.Count; i > 0; i--)
                {
                    switch (STATIC_EDITOR_MODE.selectedObjectIndices[i - 1].Type)
                    {
                        case (OBJECT_TYPE.Physics):
                            {
                                STATIC_EDITOR_MODE.levelInstance.PhysicsObjectsList.RemoveAt(STATIC_EDITOR_MODE.selectedObjectIndices[i - 1].Index);
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

                Update_undoArray();
            }
        }

        #endregion

        #endregion

        #endregion

        #region Private Methods

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
                        STATIC_EDITOR_MODE.levelInstance = IntermediateSerializer.Deserialize<Level>(input, null);

                        xnA_RenderControl1.levelBackground = xnA_RenderControl1.contentMan.Load<Texture2D>(STATIC_EDITOR_MODE.levelInstance.BackgroundFile);
                        xnA_RenderControl1.levelDimensions = STATIC_EDITOR_MODE.levelInstance.RoomDimensions;

                        for (int i = 0; i < STATIC_EDITOR_MODE.levelInstance.PhysicsObjectsList.Count; i++)
                        {
                            STATIC_EDITOR_MODE.levelInstance.PhysicsObjectsList[i].Load(xnA_RenderControl1.contentMan, STATIC_EDITOR_MODE.world);
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

        bool SaveFile()
        {
            saveFileDialog1.Filter = "Xml files|*.xml";
            saveFileDialog1.AddExtension = true;

            if (saveFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
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

        void NewFile()
        {
            if (CheckIfOpenLevel())
            {
                STATIC_EDITOR_MODE.levelInstance = new Level();
                STATIC_EDITOR_MODE.selectedObjectIndices = new List<ObjectIndex>();
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
                                    STATIC_EDITOR_MODE.levelInstance.BackgroundFile = RoomForm.rearWall;
                                    xnA_RenderControl1.levelBackground = xnA_RenderControl1.contentMan.Load<Texture2D>(RoomForm.rearWall);

                                    Update_undoArray();
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
                                    theStuff.Add(STATIC_EDITOR_MODE.levelInstance.PhysicsObjectsList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index]);
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
        private void BUTTON_IMPORT_ASSET_Click(object sender, EventArgs e)
        {
            // Default to the directory which contains our content files
            string assemblyLocation = System.Reflection.Assembly.GetExecutingAssembly().Location;
            string relativePath = Path.Combine(assemblyLocation, "../../../../Content", STATIC_CONTBUILDER.textureLoc);
            string contentPath = Path.GetFullPath(relativePath);

            openFileDialog1.InitialDirectory = contentPath;

            openFileDialog1.Title = "Load Asset";

            openFileDialog1.Filter = "PNG Files (*.png)|*.png|" +
                                     "DDS Files (*.dds)|*.dds|" +
                                     "BMP Files (*.bmp)|*.bmp|" +
                                     "All Files (*.*)|*.*";

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string outFileName = STATIC_CONTBUILDER.BuildXNBFromFile((openFileDialog1.FileName));

                // copy the asset from the temporary build directory to our assets directory, for the purposes of this example, we will just use the root of the content folder
                File.Copy(
                    Path.Combine(STATIC_CONTBUILDER.contentBuilder.OutputDirectory, outFileName),
                    Path.Combine(STATIC_CONTBUILDER.pathToContent(), outFileName),
                    true);
            }

            Refresh_XNB_Asset_List();
        }

        #region ALIGN/ROTATE

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
        // Horizontal Alignment Functions
        #region Align Right Side to Left of Anchor
        private void BUTTON_ALIGN_1_Click(object sender, EventArgs e)
        {
            switch (Align_Relative.SelectedItem.ToString())
            {
                case "First Selected":
                    {
                        float firstObjXPos = get_first_xpos();
                        float firstObjTexWidth = get_first_texwidth();

                        for (int i = STATIC_EDITOR_MODE.selectedObjectIndices.Count - 1; i > 0; i--)
                        {
                            switch (STATIC_EDITOR_MODE.selectedObjectIndices[i].Type)
                            {
                                case (OBJECT_TYPE.Physics):
                                    {
                                        STATIC_EDITOR_MODE.levelInstance.PhysicsObjectsList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Position = new Vector2(
                                            firstObjXPos
                                            - (STATIC_EDITOR_MODE.levelInstance.PhysicsObjectsList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Texture.Width / 2)
                                            - (firstObjTexWidth / 2),
                                            STATIC_EDITOR_MODE.levelInstance.PhysicsObjectsList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Position.Y);
                                    }
                                    break;
                                case (OBJECT_TYPE.Decal):
                                    {
                                        STATIC_EDITOR_MODE.levelInstance.DecalManager.DecalList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Position = new Vector2(
                                            firstObjXPos
                                            - (STATIC_EDITOR_MODE.levelInstance.DecalManager.DecalList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Width / 2)
                                            - (firstObjTexWidth / 2),
                                            STATIC_EDITOR_MODE.levelInstance.DecalManager.DecalList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Position.Y);
                                    }
                                    break;
                            }
                        }
                    }
                    break;
                case "Last Selected":
                    {
                        float lastObjXPos = get_last_xpos();
                        float lastObjTexWidth = get_last_texwidth();

                        for (int i = 0; i < STATIC_EDITOR_MODE.selectedObjectIndices.Count - 1; i++)
                        {
                            switch (STATIC_EDITOR_MODE.selectedObjectIndices[i].Type)
                            {
                                case (OBJECT_TYPE.Physics):
                                    {
                                        STATIC_EDITOR_MODE.levelInstance.PhysicsObjectsList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Position = new Vector2(
                                            lastObjXPos
                                            - (STATIC_EDITOR_MODE.levelInstance.PhysicsObjectsList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Texture.Width / 2)
                                            - (lastObjTexWidth / 2),
                                            STATIC_EDITOR_MODE.levelInstance.PhysicsObjectsList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Position.Y);
                                    }
                                    break;
                                case (OBJECT_TYPE.Decal):
                                    {
                                        STATIC_EDITOR_MODE.levelInstance.DecalManager.DecalList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Position = new Vector2(
                                            lastObjXPos
                                            - (STATIC_EDITOR_MODE.levelInstance.DecalManager.DecalList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Width / 2)
                                            - (lastObjTexWidth / 2),
                                            STATIC_EDITOR_MODE.levelInstance.DecalManager.DecalList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Position.Y);
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
                                        STATIC_EDITOR_MODE.levelInstance.PhysicsObjectsList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Position = new Vector2(
                                            furthestPos - STATIC_EDITOR_MODE.levelInstance.PhysicsObjectsList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Texture.Width / 2,
                                            STATIC_EDITOR_MODE.levelInstance.PhysicsObjectsList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Position.Y);
                                    }
                                    break;
                                case (OBJECT_TYPE.Decal):
                                    {
                                        STATIC_EDITOR_MODE.levelInstance.DecalManager.DecalList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Position = new Vector2(
                                            furthestPos - STATIC_EDITOR_MODE.levelInstance.DecalManager.DecalList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Width / 2,
                                            STATIC_EDITOR_MODE.levelInstance.DecalManager.DecalList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Position.Y);
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

        #region Align Left Sides
        private void BUTTON_ALIGN_2_Click(object sender, EventArgs e)
        {
            switch (Align_Relative.SelectedItem.ToString())
            {
                case "First Selected":
                    {
                        float firstObjXPos = get_first_xpos();
                        float firstObjTexWidth = get_first_texwidth();

                        for (int i = STATIC_EDITOR_MODE.selectedObjectIndices.Count - 1; i > 0; i--)
                        {
                            switch (STATIC_EDITOR_MODE.selectedObjectIndices[i].Type)
                            {
                                case (OBJECT_TYPE.Physics):
                                    {
                                        STATIC_EDITOR_MODE.levelInstance.PhysicsObjectsList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Position = new Vector2(
                                            firstObjXPos
                                            + (STATIC_EDITOR_MODE.levelInstance.PhysicsObjectsList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Texture.Width / 2)
                                            - (firstObjTexWidth / 2),
                                            STATIC_EDITOR_MODE.levelInstance.PhysicsObjectsList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Position.Y);
                                    }
                                    break;
                                case (OBJECT_TYPE.Decal):
                                    {
                                        STATIC_EDITOR_MODE.levelInstance.DecalManager.DecalList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Position = new Vector2(
                                            firstObjXPos
                                            + (STATIC_EDITOR_MODE.levelInstance.DecalManager.DecalList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Width / 2)
                                            - (firstObjTexWidth / 2),
                                            STATIC_EDITOR_MODE.levelInstance.DecalManager.DecalList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Position.Y);
                                    }
                                    break;
                            }
                        }
                    }
                    break;
                case "Last Selected":
                    {
                        float lastObjXPos = get_last_xpos();
                        float lastObjTexWidth = get_last_texwidth();

                        for (int i = 0; i < STATIC_EDITOR_MODE.selectedObjectIndices.Count - 1; i++)
                        {
                            switch (STATIC_EDITOR_MODE.selectedObjectIndices[i].Type)
                            {
                                case (OBJECT_TYPE.Physics):
                                    {
                                        STATIC_EDITOR_MODE.levelInstance.PhysicsObjectsList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Position = new Vector2(
                                            lastObjXPos
                                            + (STATIC_EDITOR_MODE.levelInstance.PhysicsObjectsList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Texture.Width / 2)
                                            - (lastObjTexWidth / 2),
                                            STATIC_EDITOR_MODE.levelInstance.PhysicsObjectsList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Position.Y);
                                    }
                                    break;
                                case (OBJECT_TYPE.Decal):
                                    {
                                        STATIC_EDITOR_MODE.levelInstance.DecalManager.DecalList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Position = new Vector2(
                                            lastObjXPos
                                            + (STATIC_EDITOR_MODE.levelInstance.DecalManager.DecalList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Width / 2)
                                            - (lastObjTexWidth / 2),
                                            STATIC_EDITOR_MODE.levelInstance.DecalManager.DecalList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Position.Y);
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
                                        STATIC_EDITOR_MODE.levelInstance.PhysicsObjectsList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Position = new Vector2(
                                            furthestPos + STATIC_EDITOR_MODE.levelInstance.PhysicsObjectsList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Texture.Width / 2,
                                            STATIC_EDITOR_MODE.levelInstance.PhysicsObjectsList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Position.Y);
                                    }
                                    break;
                                case (OBJECT_TYPE.Decal):
                                    {
                                        STATIC_EDITOR_MODE.levelInstance.DecalManager.DecalList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Position = new Vector2(
                                            furthestPos + STATIC_EDITOR_MODE.levelInstance.DecalManager.DecalList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Width / 2,
                                            STATIC_EDITOR_MODE.levelInstance.DecalManager.DecalList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Position.Y);
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
        private void BUTTON_ALIGN_3_Click(object sender, EventArgs e)
        {
            switch (Align_Relative.SelectedItem.ToString())
            {
                case "First Selected":
                    {
                        float firstObjXPos = get_first_xpos();

                        for (int i = STATIC_EDITOR_MODE.selectedObjectIndices.Count - 1; i > 0; i--)
                        {
                            switch (STATIC_EDITOR_MODE.selectedObjectIndices[i].Type)
                            {
                                case (OBJECT_TYPE.Physics):
                                    {
                                        STATIC_EDITOR_MODE.levelInstance.PhysicsObjectsList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Position = new Vector2(
                                            firstObjXPos,
                                            STATIC_EDITOR_MODE.levelInstance.PhysicsObjectsList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Position.Y);
                                    }
                                    break;
                                case (OBJECT_TYPE.Decal):
                                    {
                                        STATIC_EDITOR_MODE.levelInstance.DecalManager.DecalList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Position = new Vector2(
                                            firstObjXPos,
                                            STATIC_EDITOR_MODE.levelInstance.DecalManager.DecalList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Position.Y);
                                    }
                                    break;
                            }
                        }
                    }
                    break;
                case "Last Selected":
                    {
                        float lastObjXPos = get_last_xpos();

                        for (int i = 0; i < STATIC_EDITOR_MODE.selectedObjectIndices.Count - 1; i++)
                        {
                            switch (STATIC_EDITOR_MODE.selectedObjectIndices[i].Type)
                            {
                                case (OBJECT_TYPE.Physics):
                                    {
                                        STATIC_EDITOR_MODE.levelInstance.PhysicsObjectsList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Position = new Vector2(
                                            lastObjXPos,
                                            STATIC_EDITOR_MODE.levelInstance.PhysicsObjectsList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Position.Y);
                                    }
                                    break;
                                case (OBJECT_TYPE.Decal):
                                    {
                                        STATIC_EDITOR_MODE.levelInstance.DecalManager.DecalList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Position = new Vector2(
                                            lastObjXPos,
                                            STATIC_EDITOR_MODE.levelInstance.DecalManager.DecalList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Position.Y);
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
                                        STATIC_EDITOR_MODE.levelInstance.PhysicsObjectsList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Position = new Vector2(
                                            avgXPos, STATIC_EDITOR_MODE.levelInstance.PhysicsObjectsList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Position.Y);
                                    }
                                    break;
                                case (OBJECT_TYPE.Decal):
                                    {
                                        STATIC_EDITOR_MODE.levelInstance.DecalManager.DecalList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Position = new Vector2(
                                            avgXPos, STATIC_EDITOR_MODE.levelInstance.DecalManager.DecalList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Position.Y);
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

        #region Align Right Sides
        private void BUTTON_ALIGN_4_Click(object sender, EventArgs e)
        {
            switch (Align_Relative.SelectedItem.ToString())
            {
                case "First Selected":
                    {
                        float firstObjXPos = get_first_xpos();
                        float firstObjTexWidth = get_first_texwidth();

                        for (int i = STATIC_EDITOR_MODE.selectedObjectIndices.Count - 1; i > 0; i--)
                        {
                            switch (STATIC_EDITOR_MODE.selectedObjectIndices[i].Type)
                            {
                                case (OBJECT_TYPE.Physics):
                                    {
                                        STATIC_EDITOR_MODE.levelInstance.PhysicsObjectsList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Position = new Vector2(
                                            firstObjXPos
                                            - (STATIC_EDITOR_MODE.levelInstance.PhysicsObjectsList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Texture.Width / 2)
                                            + (firstObjTexWidth / 2),
                                            STATIC_EDITOR_MODE.levelInstance.PhysicsObjectsList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Position.Y);
                                    }
                                    break;
                                case (OBJECT_TYPE.Decal):
                                    {
                                        STATIC_EDITOR_MODE.levelInstance.DecalManager.DecalList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Position = new Vector2(
                                            firstObjXPos
                                            - (STATIC_EDITOR_MODE.levelInstance.DecalManager.DecalList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Width / 2)
                                            + (firstObjTexWidth / 2),
                                            STATIC_EDITOR_MODE.levelInstance.DecalManager.DecalList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Position.Y);
                                    }
                                    break;
                            }
                        }
                    }
                    break;
                case "Last Selected":
                    {
                        float lastObjXPos = get_last_xpos();
                        float lastObjTexWidth = get_last_texwidth();

                        for (int i = 0; i < STATIC_EDITOR_MODE.selectedObjectIndices.Count - 1; i++)
                        {
                            switch (STATIC_EDITOR_MODE.selectedObjectIndices[i].Type)
                            {
                                case (OBJECT_TYPE.Physics):
                                    {
                                        STATIC_EDITOR_MODE.levelInstance.PhysicsObjectsList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Position = new Vector2(
                                            lastObjXPos
                                            - (STATIC_EDITOR_MODE.levelInstance.PhysicsObjectsList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Texture.Width / 2)
                                            + (lastObjTexWidth / 2),
                                            STATIC_EDITOR_MODE.levelInstance.PhysicsObjectsList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Position.Y);
                                    }
                                    break;
                                case (OBJECT_TYPE.Decal):
                                    {
                                        STATIC_EDITOR_MODE.levelInstance.DecalManager.DecalList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Position = new Vector2(
                                            lastObjXPos
                                            - (STATIC_EDITOR_MODE.levelInstance.DecalManager.DecalList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Width / 2)
                                            + (lastObjTexWidth / 2),
                                            STATIC_EDITOR_MODE.levelInstance.DecalManager.DecalList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Position.Y);
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
                                        STATIC_EDITOR_MODE.levelInstance.PhysicsObjectsList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Position = new Vector2(
                                            furthestPos - STATIC_EDITOR_MODE.levelInstance.PhysicsObjectsList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Texture.Width / 2,
                                            STATIC_EDITOR_MODE.levelInstance.PhysicsObjectsList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Position.Y);
                                    }
                                    break;
                                case (OBJECT_TYPE.Decal):
                                    {
                                        STATIC_EDITOR_MODE.levelInstance.DecalManager.DecalList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Position = new Vector2(
                                            furthestPos - STATIC_EDITOR_MODE.levelInstance.DecalManager.DecalList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Width / 2,
                                            STATIC_EDITOR_MODE.levelInstance.DecalManager.DecalList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Position.Y);
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
        private void BUTTON_ALIGN_5_Click(object sender, EventArgs e)
        {
            switch (Align_Relative.SelectedItem.ToString())
            {
                case "First Selected":
                    {
                        float firstObjXPos = get_first_xpos();
                        float firstObjTexWidth = get_first_texwidth();

                        for (int i = STATIC_EDITOR_MODE.selectedObjectIndices.Count - 1; i > 0; i--)
                        {
                            switch (STATIC_EDITOR_MODE.selectedObjectIndices[i].Type)
                            {
                                case (OBJECT_TYPE.Physics):
                                    {
                                        STATIC_EDITOR_MODE.levelInstance.PhysicsObjectsList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Position = new Vector2(
                                            firstObjXPos
                                            + (STATIC_EDITOR_MODE.levelInstance.PhysicsObjectsList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Texture.Width / 2)
                                            + (firstObjTexWidth / 2),
                                            STATIC_EDITOR_MODE.levelInstance.PhysicsObjectsList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Position.Y);
                                    }
                                    break;
                                case (OBJECT_TYPE.Decal):
                                    {
                                        STATIC_EDITOR_MODE.levelInstance.DecalManager.DecalList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Position = new Vector2(
                                            firstObjXPos
                                            + (STATIC_EDITOR_MODE.levelInstance.DecalManager.DecalList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Width / 2)
                                            + (firstObjTexWidth / 2),
                                            STATIC_EDITOR_MODE.levelInstance.DecalManager.DecalList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Position.Y);
                                    }
                                    break;
                            }
                        }
                    }
                    break;
                case "Last Selected":
                    {
                        float lastObjXPos = get_last_xpos();
                        float lastObjTexWidth = get_last_texwidth();

                        for (int i = 0; i < STATIC_EDITOR_MODE.selectedObjectIndices.Count - 1; i++)
                        {
                            switch (STATIC_EDITOR_MODE.selectedObjectIndices[i].Type)
                            {
                                case (OBJECT_TYPE.Physics):
                                    {
                                        STATIC_EDITOR_MODE.levelInstance.PhysicsObjectsList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Position = new Vector2(
                                            lastObjXPos
                                            + (STATIC_EDITOR_MODE.levelInstance.PhysicsObjectsList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Texture.Width / 2)
                                            + (lastObjTexWidth / 2),
                                            STATIC_EDITOR_MODE.levelInstance.PhysicsObjectsList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Position.Y);
                                    }
                                    break;
                                case (OBJECT_TYPE.Decal):
                                    {
                                        STATIC_EDITOR_MODE.levelInstance.DecalManager.DecalList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Position = new Vector2(
                                            lastObjXPos
                                            + (STATIC_EDITOR_MODE.levelInstance.DecalManager.DecalList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Width / 2)
                                            + (lastObjTexWidth / 2),
                                            STATIC_EDITOR_MODE.levelInstance.DecalManager.DecalList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Position.Y);
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
                                        STATIC_EDITOR_MODE.levelInstance.PhysicsObjectsList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Position = new Vector2(
                                            furthestPos + STATIC_EDITOR_MODE.levelInstance.PhysicsObjectsList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Texture.Width / 2,
                                            STATIC_EDITOR_MODE.levelInstance.PhysicsObjectsList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Position.Y);
                                    }
                                    break;
                                case (OBJECT_TYPE.Decal):
                                    {
                                        STATIC_EDITOR_MODE.levelInstance.DecalManager.DecalList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Position = new Vector2(
                                            furthestPos + STATIC_EDITOR_MODE.levelInstance.DecalManager.DecalList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Width / 2,
                                            STATIC_EDITOR_MODE.levelInstance.DecalManager.DecalList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Position.Y);
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

        // Vertical Alignment Functions
        #region Align Bottoms to Top of Anchor
        private void BUTTON_ALIGN_6_Click(object sender, EventArgs e)
        {
            switch (Align_Relative.SelectedItem.ToString())
            {
                case "First Selected":
                    {
                        float firstObjYPos = get_first_ypos();
                        float firstObjTexHeight = get_first_texheight();

                        for (int i = STATIC_EDITOR_MODE.selectedObjectIndices.Count - 1; i > 0; i--)
                        {
                            switch (STATIC_EDITOR_MODE.selectedObjectIndices[i].Type)
                            {
                                case (OBJECT_TYPE.Physics):
                                    {
                                        STATIC_EDITOR_MODE.levelInstance.PhysicsObjectsList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Position = new Vector2(
                                            STATIC_EDITOR_MODE.levelInstance.PhysicsObjectsList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Position.X,
                                            firstObjYPos
                                            - (STATIC_EDITOR_MODE.levelInstance.PhysicsObjectsList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Texture.Height / 2)
                                            - (firstObjTexHeight / 2));
                                    }
                                    break;
                                case (OBJECT_TYPE.Decal):
                                    {
                                        STATIC_EDITOR_MODE.levelInstance.DecalManager.DecalList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Position = new Vector2(
                                            STATIC_EDITOR_MODE.levelInstance.DecalManager.DecalList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Position.X,
                                            firstObjYPos
                                            - (STATIC_EDITOR_MODE.levelInstance.DecalManager.DecalList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Height / 2)
                                            - (firstObjTexHeight / 2));
                                    }
                                    break;
                            }
                        }
                    }
                    break;
                case "Last Selected":
                    {
                        float lastObjYPos = get_last_ypos();
                        float lastObjTexHeight = get_last_texheight();

                        for (int i = 0; i < STATIC_EDITOR_MODE.selectedObjectIndices.Count - 1; i++)
                        {
                            switch (STATIC_EDITOR_MODE.selectedObjectIndices[i].Type)
                            {
                                case (OBJECT_TYPE.Physics):
                                    {
                                        STATIC_EDITOR_MODE.levelInstance.PhysicsObjectsList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Position = new Vector2(
                                            STATIC_EDITOR_MODE.levelInstance.PhysicsObjectsList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Position.X,
                                            lastObjYPos
                                            - (STATIC_EDITOR_MODE.levelInstance.PhysicsObjectsList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Texture.Height / 2)
                                            - (lastObjTexHeight / 2));
                                    }
                                    break;
                                case (OBJECT_TYPE.Decal):
                                    {
                                        STATIC_EDITOR_MODE.levelInstance.DecalManager.DecalList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Position = new Vector2(
                                            STATIC_EDITOR_MODE.levelInstance.DecalManager.DecalList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Position.X,
                                            lastObjYPos
                                            - (STATIC_EDITOR_MODE.levelInstance.DecalManager.DecalList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Height / 2)
                                            - (lastObjTexHeight / 2));
                                    }
                                    break;
                            }
                        }
                    }
                    break;
                case "Selection":
                    {
                        float furthestPos = get_furthest_upos();

                        for (int i = 0; i < STATIC_EDITOR_MODE.selectedObjectIndices.Count; i++)
                        {
                            switch (STATIC_EDITOR_MODE.selectedObjectIndices[i].Type)
                            {
                                case (OBJECT_TYPE.Physics):
                                    {
                                        STATIC_EDITOR_MODE.levelInstance.PhysicsObjectsList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Position = new Vector2(
                                            STATIC_EDITOR_MODE.levelInstance.PhysicsObjectsList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Position.X,
                                            furthestPos - STATIC_EDITOR_MODE.levelInstance.PhysicsObjectsList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Texture.Height / 2);
                                    }
                                    break;
                                case (OBJECT_TYPE.Decal):
                                    {
                                        STATIC_EDITOR_MODE.levelInstance.DecalManager.DecalList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Position = new Vector2(
                                            STATIC_EDITOR_MODE.levelInstance.DecalManager.DecalList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Position.X,
                                            furthestPos - STATIC_EDITOR_MODE.levelInstance.DecalManager.DecalList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Height / 2);
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
        private void BUTTON_ALIGN_7_Click(object sender, EventArgs e)
        {
            switch (Align_Relative.SelectedItem.ToString())
            {
                case "First Selected":
                    {
                        float firstObjYPos = get_first_ypos();
                        float firstObjTexHeight = get_first_texheight();

                        for (int i = STATIC_EDITOR_MODE.selectedObjectIndices.Count - 1; i > 0; i--)
                        {
                            switch (STATIC_EDITOR_MODE.selectedObjectIndices[i].Type)
                            {
                                case (OBJECT_TYPE.Physics):
                                    {
                                        STATIC_EDITOR_MODE.levelInstance.PhysicsObjectsList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Position = new Vector2(
                                            STATIC_EDITOR_MODE.levelInstance.PhysicsObjectsList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Position.X,
                                            firstObjYPos
                                            + (STATIC_EDITOR_MODE.levelInstance.PhysicsObjectsList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Texture.Height / 2)
                                            - (firstObjTexHeight / 2));
                                    }
                                    break;
                                case (OBJECT_TYPE.Decal):
                                    {
                                        STATIC_EDITOR_MODE.levelInstance.DecalManager.DecalList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Position = new Vector2(
                                            STATIC_EDITOR_MODE.levelInstance.DecalManager.DecalList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Position.X,
                                            firstObjYPos
                                            + (STATIC_EDITOR_MODE.levelInstance.DecalManager.DecalList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Height / 2)
                                            - (firstObjTexHeight / 2));
                                    }
                                    break;
                            }
                        }
                    }
                    break;
                case "Last Selected":
                    {
                        float lastObjYPos = get_last_ypos();
                        float lastObjTexHeight = get_last_texheight();

                        for (int i = 0; i < STATIC_EDITOR_MODE.selectedObjectIndices.Count - 1; i++)
                        {
                            switch (STATIC_EDITOR_MODE.selectedObjectIndices[i].Type)
                            {
                                case (OBJECT_TYPE.Physics):
                                    {
                                        STATIC_EDITOR_MODE.levelInstance.PhysicsObjectsList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Position = new Vector2(
                                            STATIC_EDITOR_MODE.levelInstance.PhysicsObjectsList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Position.X,
                                            lastObjYPos
                                            + (STATIC_EDITOR_MODE.levelInstance.PhysicsObjectsList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Texture.Height / 2)
                                            - (lastObjTexHeight / 2));
                                    }
                                    break;
                                case (OBJECT_TYPE.Decal):
                                    {
                                        STATIC_EDITOR_MODE.levelInstance.DecalManager.DecalList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Position = new Vector2(
                                            STATIC_EDITOR_MODE.levelInstance.DecalManager.DecalList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Position.X,
                                            lastObjYPos
                                            + (STATIC_EDITOR_MODE.levelInstance.DecalManager.DecalList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Height / 2)
                                            - (lastObjTexHeight / 2));
                                    }
                                    break;
                            }
                        }
                    }
                    break;
                case "Selection":
                    {
                        float furthestPos = get_furthest_upos();

                        for (int i = 0; i < STATIC_EDITOR_MODE.selectedObjectIndices.Count; i++)
                        {
                            switch (STATIC_EDITOR_MODE.selectedObjectIndices[i].Type)
                            {
                                case (OBJECT_TYPE.Physics):
                                    {
                                        STATIC_EDITOR_MODE.levelInstance.PhysicsObjectsList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Position = new Vector2(
                                            STATIC_EDITOR_MODE.levelInstance.PhysicsObjectsList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Position.X,
                                            furthestPos + STATIC_EDITOR_MODE.levelInstance.PhysicsObjectsList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Texture.Height / 2);
                                    }
                                    break;
                                case (OBJECT_TYPE.Decal):
                                    {
                                        STATIC_EDITOR_MODE.levelInstance.DecalManager.DecalList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Position = new Vector2(
                                            STATIC_EDITOR_MODE.levelInstance.DecalManager.DecalList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Position.X,
                                            furthestPos + STATIC_EDITOR_MODE.levelInstance.DecalManager.DecalList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Height / 2);
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
        private void BUTTON_ALIGN_8_Click(object sender, EventArgs e)
        {
            switch (Align_Relative.SelectedItem.ToString())
            {
                case "First Selected":
                    {
                        float firstObjYPos = get_first_ypos();

                        for (int i = STATIC_EDITOR_MODE.selectedObjectIndices.Count - 1; i > 0; i--)
                        {
                            switch (STATIC_EDITOR_MODE.selectedObjectIndices[i].Type)
                            {
                                case (OBJECT_TYPE.Physics):
                                    {
                                        STATIC_EDITOR_MODE.levelInstance.PhysicsObjectsList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Position = new Vector2(
                                            STATIC_EDITOR_MODE.levelInstance.PhysicsObjectsList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Position.X,
                                            firstObjYPos);
                                    }
                                    break;
                                case (OBJECT_TYPE.Decal):
                                    {
                                        STATIC_EDITOR_MODE.levelInstance.DecalManager.DecalList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Position = new Vector2(
                                            STATIC_EDITOR_MODE.levelInstance.DecalManager.DecalList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Position.X,
                                            firstObjYPos);
                                    }
                                    break;
                            }
                        }
                    }
                    break;
                case "Last Selected":
                    {
                        float lastObjYPos = get_last_ypos();

                        for (int i = 0; i < STATIC_EDITOR_MODE.selectedObjectIndices.Count - 1; i++)
                        {
                            switch (STATIC_EDITOR_MODE.selectedObjectIndices[i].Type)
                            {
                                case (OBJECT_TYPE.Physics):
                                    {
                                        STATIC_EDITOR_MODE.levelInstance.PhysicsObjectsList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Position = new Vector2(
                                            STATIC_EDITOR_MODE.levelInstance.PhysicsObjectsList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Position.X,
                                            lastObjYPos);
                                    }
                                    break;
                                case (OBJECT_TYPE.Decal):
                                    {
                                        STATIC_EDITOR_MODE.levelInstance.DecalManager.DecalList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Position = new Vector2(
                                            STATIC_EDITOR_MODE.levelInstance.DecalManager.DecalList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Position.X,
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

                        for (int i = 0; i < STATIC_EDITOR_MODE.selectedObjectIndices.Count; i++)
                        {
                            switch (STATIC_EDITOR_MODE.selectedObjectIndices[i].Type)
                            {
                                case (OBJECT_TYPE.Physics):
                                    {
                                        STATIC_EDITOR_MODE.levelInstance.PhysicsObjectsList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Position =
                                            new Vector2(STATIC_EDITOR_MODE.levelInstance.PhysicsObjectsList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Position.X, avgYPos);
                                    }
                                    break;
                                case (OBJECT_TYPE.Decal):
                                    {
                                        STATIC_EDITOR_MODE.levelInstance.DecalManager.DecalList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Position =
                                            new Vector2(STATIC_EDITOR_MODE.levelInstance.DecalManager.DecalList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Position.X, avgYPos);
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
        private void BUTTON_ALIGN_9_Click(object sender, EventArgs e)
        {
            switch (Align_Relative.SelectedItem.ToString())
            {
                case "First Selected":
                    {
                        float firstObjYPos = get_first_ypos();
                        float firstObjTexHeight = get_first_texheight();

                        for (int i = STATIC_EDITOR_MODE.selectedObjectIndices.Count - 1; i > 0; i--)
                        {
                            switch (STATIC_EDITOR_MODE.selectedObjectIndices[i].Type)
                            {
                                case (OBJECT_TYPE.Physics):
                                    {
                                        STATIC_EDITOR_MODE.levelInstance.PhysicsObjectsList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Position = new Vector2(
                                            STATIC_EDITOR_MODE.levelInstance.PhysicsObjectsList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Position.X,
                                            firstObjYPos
                                            - (STATIC_EDITOR_MODE.levelInstance.PhysicsObjectsList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Texture.Height / 2)
                                            + (firstObjTexHeight / 2));
                                    }
                                    break;
                                case (OBJECT_TYPE.Decal):
                                    {
                                        STATIC_EDITOR_MODE.levelInstance.DecalManager.DecalList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Position = new Vector2(
                                            STATIC_EDITOR_MODE.levelInstance.DecalManager.DecalList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Position.X,
                                            firstObjYPos
                                            - (STATIC_EDITOR_MODE.levelInstance.DecalManager.DecalList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Height / 2)
                                            + (firstObjTexHeight / 2));
                                    }
                                    break;
                            }
                        }
                    }
                    break;
                case "Last Selected":
                    {
                        float lastObjYPos = get_last_ypos();
                        float lastObjTexHeight = get_last_texheight();

                        for (int i = 0; i < STATIC_EDITOR_MODE.selectedObjectIndices.Count - 1; i++)
                        {
                            switch (STATIC_EDITOR_MODE.selectedObjectIndices[i].Type)
                            {
                                case (OBJECT_TYPE.Physics):
                                    {
                                        STATIC_EDITOR_MODE.levelInstance.PhysicsObjectsList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Position = new Vector2(
                                            STATIC_EDITOR_MODE.levelInstance.PhysicsObjectsList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Position.X,
                                            lastObjYPos
                                            - (STATIC_EDITOR_MODE.levelInstance.PhysicsObjectsList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Texture.Height / 2)
                                            + (lastObjTexHeight / 2));
                                    }
                                    break;
                                case (OBJECT_TYPE.Decal):
                                    {
                                        STATIC_EDITOR_MODE.levelInstance.DecalManager.DecalList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Position = new Vector2(
                                            STATIC_EDITOR_MODE.levelInstance.DecalManager.DecalList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Position.X,
                                            lastObjYPos
                                            - (STATIC_EDITOR_MODE.levelInstance.DecalManager.DecalList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Height / 2)
                                            + (lastObjTexHeight / 2));
                                    }
                                    break;
                            }
                        }
                    }
                    break;
                case "Selection":
                    {
                        float furthestPos = get_furthest_dpos();

                        for (int i = 0; i < STATIC_EDITOR_MODE.selectedObjectIndices.Count; i++)
                        {
                            switch (STATIC_EDITOR_MODE.selectedObjectIndices[i].Type)
                            {
                                case (OBJECT_TYPE.Physics):
                                    {
                                        STATIC_EDITOR_MODE.levelInstance.PhysicsObjectsList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Position = new Vector2(
                                            STATIC_EDITOR_MODE.levelInstance.PhysicsObjectsList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Position.X,
                                            furthestPos - STATIC_EDITOR_MODE.levelInstance.PhysicsObjectsList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Texture.Height / 2);
                                    }
                                    break;
                                case (OBJECT_TYPE.Decal):
                                    {
                                        STATIC_EDITOR_MODE.levelInstance.DecalManager.DecalList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Position = new Vector2(
                                            STATIC_EDITOR_MODE.levelInstance.DecalManager.DecalList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Position.X,
                                            furthestPos - STATIC_EDITOR_MODE.levelInstance.DecalManager.DecalList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Height / 2);
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
        private void BUTTON_ALIGN_10_Click(object sender, EventArgs e)
        {
            switch (Align_Relative.SelectedItem.ToString())
            {
                case "First Selected":
                    {
                        float firstObjYPos = get_first_ypos();
                        float firstObjTexHeight = get_first_texheight();

                        for (int i = STATIC_EDITOR_MODE.selectedObjectIndices.Count - 1; i > 0; i--)
                        {
                            switch (STATIC_EDITOR_MODE.selectedObjectIndices[i].Type)
                            {
                                case (OBJECT_TYPE.Physics):
                                    {
                                        STATIC_EDITOR_MODE.levelInstance.PhysicsObjectsList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Position = new Vector2(
                                            STATIC_EDITOR_MODE.levelInstance.PhysicsObjectsList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Position.X,
                                            firstObjYPos
                                            + (STATIC_EDITOR_MODE.levelInstance.PhysicsObjectsList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Texture.Height / 2)
                                            + (firstObjTexHeight / 2));
                                    }
                                    break;
                                case (OBJECT_TYPE.Decal):
                                    {
                                        STATIC_EDITOR_MODE.levelInstance.DecalManager.DecalList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Position = new Vector2(
                                            STATIC_EDITOR_MODE.levelInstance.DecalManager.DecalList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Position.X,
                                            firstObjYPos
                                            + (STATIC_EDITOR_MODE.levelInstance.DecalManager.DecalList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Height / 2)
                                            + (firstObjTexHeight / 2));
                                    }
                                    break;
                            }
                        }
                    }
                    break;
                case "Last Selected":
                    {
                        float lastObjYPos = get_last_ypos();
                        float lastObjTexHeight = get_last_texheight();

                        for (int i = 0; i < STATIC_EDITOR_MODE.selectedObjectIndices.Count - 1; i++)
                        {
                            switch (STATIC_EDITOR_MODE.selectedObjectIndices[i].Type)
                            {
                                case (OBJECT_TYPE.Physics):
                                    {
                                        STATIC_EDITOR_MODE.levelInstance.PhysicsObjectsList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Position = new Vector2(
                                            STATIC_EDITOR_MODE.levelInstance.PhysicsObjectsList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Position.X,
                                            lastObjYPos
                                            + (STATIC_EDITOR_MODE.levelInstance.PhysicsObjectsList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Texture.Height / 2)
                                            + (lastObjTexHeight / 2));
                                    }
                                    break;
                                case (OBJECT_TYPE.Decal):
                                    {
                                        STATIC_EDITOR_MODE.levelInstance.DecalManager.DecalList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Position = new Vector2(
                                            STATIC_EDITOR_MODE.levelInstance.DecalManager.DecalList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Position.X,
                                            lastObjYPos
                                            + (STATIC_EDITOR_MODE.levelInstance.DecalManager.DecalList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Height / 2)
                                            + (lastObjTexHeight / 2));
                                    }
                                    break;
                            }
                        }
                    }
                    break;
                case "Selection":
                    {
                        float furthestPos = get_furthest_dpos();

                        for (int i = 0; i < STATIC_EDITOR_MODE.selectedObjectIndices.Count; i++)
                        {
                            switch (STATIC_EDITOR_MODE.selectedObjectIndices[i].Type)
                            {
                                case (OBJECT_TYPE.Physics):
                                    {
                                        STATIC_EDITOR_MODE.levelInstance.PhysicsObjectsList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Position = new Vector2(
                                            STATIC_EDITOR_MODE.levelInstance.PhysicsObjectsList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Position.X,
                                            furthestPos + STATIC_EDITOR_MODE.levelInstance.PhysicsObjectsList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Texture.Height / 2);
                                    }
                                    break;
                                case (OBJECT_TYPE.Decal):
                                    {
                                        STATIC_EDITOR_MODE.levelInstance.DecalManager.DecalList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Position = new Vector2(
                                            STATIC_EDITOR_MODE.levelInstance.DecalManager.DecalList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Position.X,
                                            furthestPos + STATIC_EDITOR_MODE.levelInstance.DecalManager.DecalList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Height / 2);
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

        #region Get Positions To Align By
        private float get_avg_xpos()
        {
            float furthestLeftPos = 0;
            float furthestRightPos = 0;

            switch (STATIC_EDITOR_MODE.selectedObjectIndices[0].Type)
            {
                case (OBJECT_TYPE.Physics):
                    {
                        furthestLeftPos = STATIC_EDITOR_MODE.levelInstance.PhysicsObjectsList[STATIC_EDITOR_MODE.selectedObjectIndices[0].Index].Position.X - STATIC_EDITOR_MODE.levelInstance.PhysicsObjectsList[STATIC_EDITOR_MODE.selectedObjectIndices[0].Index].Width / 2;
                        furthestRightPos = STATIC_EDITOR_MODE.levelInstance.PhysicsObjectsList[STATIC_EDITOR_MODE.selectedObjectIndices[0].Index].Position.X + STATIC_EDITOR_MODE.levelInstance.PhysicsObjectsList[STATIC_EDITOR_MODE.selectedObjectIndices[0].Index].Width / 2;
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
                            if (furthestLeftPos > STATIC_EDITOR_MODE.levelInstance.PhysicsObjectsList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Position.X - STATIC_EDITOR_MODE.levelInstance.PhysicsObjectsList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Width / 2)
                                furthestLeftPos = STATIC_EDITOR_MODE.levelInstance.PhysicsObjectsList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Position.X - STATIC_EDITOR_MODE.levelInstance.PhysicsObjectsList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Width / 2;
                            if (furthestRightPos < STATIC_EDITOR_MODE.levelInstance.PhysicsObjectsList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Position.X + STATIC_EDITOR_MODE.levelInstance.PhysicsObjectsList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Width / 2)
                                furthestRightPos = STATIC_EDITOR_MODE.levelInstance.PhysicsObjectsList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Position.X + STATIC_EDITOR_MODE.levelInstance.PhysicsObjectsList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Width / 2;
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

            switch (STATIC_EDITOR_MODE.selectedObjectIndices[0].Type)
            {
                case (OBJECT_TYPE.Physics):
                    {
                        furthestUpPos = STATIC_EDITOR_MODE.levelInstance.PhysicsObjectsList[STATIC_EDITOR_MODE.selectedObjectIndices[0].Index].Position.Y - STATIC_EDITOR_MODE.levelInstance.PhysicsObjectsList[STATIC_EDITOR_MODE.selectedObjectIndices[0].Index].Height / 2;
                        furthestDownPos = STATIC_EDITOR_MODE.levelInstance.PhysicsObjectsList[STATIC_EDITOR_MODE.selectedObjectIndices[0].Index].Position.Y + STATIC_EDITOR_MODE.levelInstance.PhysicsObjectsList[STATIC_EDITOR_MODE.selectedObjectIndices[0].Index].Height / 2;
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
                            if (furthestUpPos > STATIC_EDITOR_MODE.levelInstance.PhysicsObjectsList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Position.Y - STATIC_EDITOR_MODE.levelInstance.PhysicsObjectsList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Height / 2)
                                furthestUpPos = STATIC_EDITOR_MODE.levelInstance.PhysicsObjectsList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Position.Y - STATIC_EDITOR_MODE.levelInstance.PhysicsObjectsList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Height / 2;
                            if (furthestDownPos < STATIC_EDITOR_MODE.levelInstance.PhysicsObjectsList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Position.Y + STATIC_EDITOR_MODE.levelInstance.PhysicsObjectsList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Height / 2)
                                furthestDownPos = STATIC_EDITOR_MODE.levelInstance.PhysicsObjectsList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Position.Y + STATIC_EDITOR_MODE.levelInstance.PhysicsObjectsList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Height / 2;
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
        private float get_first_xpos()
        {
            float firstObjXPos = 0;

            switch (STATIC_EDITOR_MODE.selectedObjectIndices[0].Type)
            {
                case (OBJECT_TYPE.Physics):
                    {
                        firstObjXPos = STATIC_EDITOR_MODE.levelInstance.PhysicsObjectsList[STATIC_EDITOR_MODE.selectedObjectIndices[0].Index].Position.X;
                    }
                    break;
                case (OBJECT_TYPE.Decal):
                    {
                        firstObjXPos = STATIC_EDITOR_MODE.levelInstance.DecalManager.DecalList[STATIC_EDITOR_MODE.selectedObjectIndices[0].Index].Position.X;
                    }
                    break;
            }

            return firstObjXPos;
        }
        private float get_first_ypos()
        {
            float firstObjYPos = 0;

            switch (STATIC_EDITOR_MODE.selectedObjectIndices[0].Type)
            {
                case (OBJECT_TYPE.Physics):
                    {
                        firstObjYPos = STATIC_EDITOR_MODE.levelInstance.PhysicsObjectsList[STATIC_EDITOR_MODE.selectedObjectIndices[0].Index].Position.Y;
                    }
                    break;
                case (OBJECT_TYPE.Decal):
                    {
                        firstObjYPos = STATIC_EDITOR_MODE.levelInstance.DecalManager.DecalList[STATIC_EDITOR_MODE.selectedObjectIndices[0].Index].Position.Y;
                    }
                    break;
            }

            return firstObjYPos;
        }
        private float get_last_xpos()
        {
            float lastObjXPos = 0;

            switch (STATIC_EDITOR_MODE.selectedObjectIndices[STATIC_EDITOR_MODE.selectedObjectIndices.Count - 1].Type)
            {
                case (OBJECT_TYPE.Physics):
                    {
                        lastObjXPos = STATIC_EDITOR_MODE.levelInstance.PhysicsObjectsList[STATIC_EDITOR_MODE.selectedObjectIndices[STATIC_EDITOR_MODE.selectedObjectIndices.Count - 1].Index].Position.X;
                    }
                    break;
                case (OBJECT_TYPE.Decal):
                    {
                        lastObjXPos = STATIC_EDITOR_MODE.levelInstance.DecalManager.DecalList[STATIC_EDITOR_MODE.selectedObjectIndices[STATIC_EDITOR_MODE.selectedObjectIndices.Count - 1].Index].Position.X;
                    }
                    break;
            }

            return lastObjXPos;
        }
        private float get_last_ypos()
        {
            float lastObjYPos = 0;

            switch (STATIC_EDITOR_MODE.selectedObjectIndices[STATIC_EDITOR_MODE.selectedObjectIndices.Count - 1].Type)
            {
                case (OBJECT_TYPE.Physics):
                    {
                        lastObjYPos = STATIC_EDITOR_MODE.levelInstance.PhysicsObjectsList[STATIC_EDITOR_MODE.selectedObjectIndices[STATIC_EDITOR_MODE.selectedObjectIndices.Count - 1].Index].Position.Y;
                    }
                    break;
                case (OBJECT_TYPE.Decal):
                    {
                        lastObjYPos = STATIC_EDITOR_MODE.levelInstance.DecalManager.DecalList[STATIC_EDITOR_MODE.selectedObjectIndices[STATIC_EDITOR_MODE.selectedObjectIndices.Count - 1].Index].Position.Y;
                    }
                    break;
            }

            return lastObjYPos;
        }
        private float get_first_texwidth()
        {
            float firstObjTexWidth = 0;

            switch (STATIC_EDITOR_MODE.selectedObjectIndices[0].Type)
            {
                case (OBJECT_TYPE.Physics):
                    {
                        firstObjTexWidth = STATIC_EDITOR_MODE.levelInstance.PhysicsObjectsList[STATIC_EDITOR_MODE.selectedObjectIndices[0].Index].Texture.Width;
                    }
                    break;
                case (OBJECT_TYPE.Decal):
                    {
                        firstObjTexWidth = STATIC_EDITOR_MODE.levelInstance.DecalManager.DecalList[STATIC_EDITOR_MODE.selectedObjectIndices[0].Index].Width;
                    }
                    break;
            }

            return firstObjTexWidth;
        }
        private float get_first_texheight()
        {
            float firstObjTexHeight = 0;

            switch (STATIC_EDITOR_MODE.selectedObjectIndices[0].Type)
            {
                case (OBJECT_TYPE.Physics):
                    {
                        firstObjTexHeight = STATIC_EDITOR_MODE.levelInstance.PhysicsObjectsList[STATIC_EDITOR_MODE.selectedObjectIndices[0].Index].Texture.Height;
                    }
                    break;
                case (OBJECT_TYPE.Decal):
                    {
                        firstObjTexHeight = STATIC_EDITOR_MODE.levelInstance.DecalManager.DecalList[STATIC_EDITOR_MODE.selectedObjectIndices[0].Index].Height;
                    }
                    break;
            }

            return firstObjTexHeight;
        }
        private float get_last_texwidth()
        {
            float lastObjTexWidth = 0;

            switch (STATIC_EDITOR_MODE.selectedObjectIndices[STATIC_EDITOR_MODE.selectedObjectIndices.Count - 1].Type)
            {
                case (OBJECT_TYPE.Physics):
                    {
                        lastObjTexWidth = STATIC_EDITOR_MODE.levelInstance.PhysicsObjectsList[STATIC_EDITOR_MODE.selectedObjectIndices[STATIC_EDITOR_MODE.selectedObjectIndices.Count - 1].Index].Texture.Width;
                    }
                    break;
                case (OBJECT_TYPE.Decal):
                    {
                        lastObjTexWidth = STATIC_EDITOR_MODE.levelInstance.DecalManager.DecalList[STATIC_EDITOR_MODE.selectedObjectIndices[STATIC_EDITOR_MODE.selectedObjectIndices.Count - 1].Index].Width;
                    }
                    break;
            }

            return lastObjTexWidth;
        }
        private float get_last_texheight()
        {
            float lastObjTexHeight = 0;

            switch (STATIC_EDITOR_MODE.selectedObjectIndices[STATIC_EDITOR_MODE.selectedObjectIndices.Count - 1].Type)
            {
                case (OBJECT_TYPE.Physics):
                    {
                        lastObjTexHeight = STATIC_EDITOR_MODE.levelInstance.PhysicsObjectsList[STATIC_EDITOR_MODE.selectedObjectIndices[STATIC_EDITOR_MODE.selectedObjectIndices.Count - 1].Index].Texture.Height;
                    }
                    break;
                case (OBJECT_TYPE.Decal):
                    {
                        lastObjTexHeight = STATIC_EDITOR_MODE.levelInstance.DecalManager.DecalList[STATIC_EDITOR_MODE.selectedObjectIndices[STATIC_EDITOR_MODE.selectedObjectIndices.Count - 1].Index].Height;
                    }
                    break;
            }

            return lastObjTexHeight;
        }
        private float get_furthest_lpos()
        {
            float furthestPos = 0;
            switch (STATIC_EDITOR_MODE.selectedObjectIndices[0].Type)
            {
                case (OBJECT_TYPE.Physics):
                    {
                        furthestPos = STATIC_EDITOR_MODE.levelInstance.PhysicsObjectsList[STATIC_EDITOR_MODE.selectedObjectIndices[0].Index].Position.X - STATIC_EDITOR_MODE.levelInstance.PhysicsObjectsList[STATIC_EDITOR_MODE.selectedObjectIndices[0].Index].Width / 2;
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
                            if (furthestPos > STATIC_EDITOR_MODE.levelInstance.PhysicsObjectsList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Position.X - STATIC_EDITOR_MODE.levelInstance.PhysicsObjectsList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Width / 2)
                                furthestPos = STATIC_EDITOR_MODE.levelInstance.PhysicsObjectsList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Position.X - STATIC_EDITOR_MODE.levelInstance.PhysicsObjectsList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Width / 2;
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
            switch (STATIC_EDITOR_MODE.selectedObjectIndices[0].Type)
            {
                case (OBJECT_TYPE.Physics):
                    {
                        furthestPos = STATIC_EDITOR_MODE.levelInstance.PhysicsObjectsList[STATIC_EDITOR_MODE.selectedObjectIndices[0].Index].Position.X + STATIC_EDITOR_MODE.levelInstance.PhysicsObjectsList[STATIC_EDITOR_MODE.selectedObjectIndices[0].Index].Width / 2;
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
                            if (furthestPos < STATIC_EDITOR_MODE.levelInstance.PhysicsObjectsList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Position.X + STATIC_EDITOR_MODE.levelInstance.PhysicsObjectsList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Width / 2)
                                furthestPos = STATIC_EDITOR_MODE.levelInstance.PhysicsObjectsList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Position.X + STATIC_EDITOR_MODE.levelInstance.PhysicsObjectsList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Width / 2;
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
            switch (STATIC_EDITOR_MODE.selectedObjectIndices[0].Type)
            {
                case (OBJECT_TYPE.Physics):
                    {
                        furthestPos = STATIC_EDITOR_MODE.levelInstance.PhysicsObjectsList[STATIC_EDITOR_MODE.selectedObjectIndices[0].Index].Position.Y - STATIC_EDITOR_MODE.levelInstance.PhysicsObjectsList[STATIC_EDITOR_MODE.selectedObjectIndices[0].Index].Height / 2;
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
                            if (furthestPos > STATIC_EDITOR_MODE.levelInstance.PhysicsObjectsList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Position.Y - STATIC_EDITOR_MODE.levelInstance.PhysicsObjectsList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Height / 2)
                                furthestPos = STATIC_EDITOR_MODE.levelInstance.PhysicsObjectsList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Position.Y - STATIC_EDITOR_MODE.levelInstance.PhysicsObjectsList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Height / 2;
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
            switch (STATIC_EDITOR_MODE.selectedObjectIndices[0].Type)
            {
                case (OBJECT_TYPE.Physics):
                    {
                        furthestPos = STATIC_EDITOR_MODE.levelInstance.PhysicsObjectsList[STATIC_EDITOR_MODE.selectedObjectIndices[0].Index].Position.Y + STATIC_EDITOR_MODE.levelInstance.PhysicsObjectsList[STATIC_EDITOR_MODE.selectedObjectIndices[0].Index].Height / 2;
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
                            if (furthestPos < STATIC_EDITOR_MODE.levelInstance.PhysicsObjectsList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Position.Y + STATIC_EDITOR_MODE.levelInstance.PhysicsObjectsList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Height / 2)
                                furthestPos = STATIC_EDITOR_MODE.levelInstance.PhysicsObjectsList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Position.Y + STATIC_EDITOR_MODE.levelInstance.PhysicsObjectsList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Height / 2;
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

        #region Rotate Selection
        private void BUTTON_ROTATE_SELECTION_CLOCKWISE_Click(object sender, EventArgs e)
        {
            switch (Rotate_Relative.SelectedItem.ToString())
            {
                case "As Group":
                    Vector2 avgPos = new Vector2(get_avg_xpos(), get_avg_ypos());

                    for (int i = STATIC_EDITOR_MODE.selectedObjectIndices.Count - 1; i >= 0; i--)
                    {
                        switch (STATIC_EDITOR_MODE.selectedObjectIndices[i].Type)
                        {
                            case (OBJECT_TYPE.Physics):
                                {
                                    Vector2 curPos = STATIC_EDITOR_MODE.levelInstance.PhysicsObjectsList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Position;
                                    STATIC_EDITOR_MODE.levelInstance.PhysicsObjectsList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Position =
                                        new Vector2((float)(Math.Cos(MathHelper.ToRadians(90)) * (curPos.X - avgPos.X) - Math.Sin(MathHelper.ToRadians(90)) * (curPos.Y - avgPos.Y) + avgPos.X),
                                                    (float)(Math.Sin(MathHelper.ToRadians(90)) * (curPos.X - avgPos.X) + Math.Cos(MathHelper.ToRadians(90)) * (curPos.Y - avgPos.Y) + avgPos.Y));
                                    Type t = STATIC_EDITOR_MODE.levelInstance.PhysicsObjectsList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].GetType();
                                    if (t.BaseType == typeof(DynamicObject))
                                    {
                                        DynamicObject dyOb = (DynamicObject)STATIC_EDITOR_MODE.levelInstance.PhysicsObjectsList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index];
                                        Vector2 curEndPos = dyOb.EndPosition;
                                        dyOb.EndPosition =
                                            new Vector2((float)(Math.Cos(MathHelper.ToRadians(90)) * (curEndPos.X - avgPos.X) - Math.Sin(MathHelper.ToRadians(90)) * (curEndPos.Y - avgPos.Y) + avgPos.X),
                                                        (float)(Math.Sin(MathHelper.ToRadians(90)) * (curEndPos.X - avgPos.X) + Math.Cos(MathHelper.ToRadians(90)) * (curEndPos.Y - avgPos.Y) + avgPos.Y));
                                        STATIC_EDITOR_MODE.levelInstance.PhysicsObjectsList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index] = dyOb;
                                    }
                                    STATIC_EDITOR_MODE.levelInstance.PhysicsObjectsList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].TextureRotation =
                                        MathHelper.ToDegrees(STATIC_EDITOR_MODE.levelInstance.PhysicsObjectsList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].TextureRotation) + 90;
                                }
                                break;
                            case (OBJECT_TYPE.Decal):
                                {
                                    Vector2 curPos = STATIC_EDITOR_MODE.levelInstance.DecalManager.DecalList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Position;
                                    STATIC_EDITOR_MODE.levelInstance.DecalManager.DecalList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Position =
                                        new Vector2((float)(Math.Cos(MathHelper.ToRadians(90)) * (curPos.X - avgPos.X) - Math.Sin(MathHelper.ToRadians(90)) * (curPos.Y - avgPos.Y) + avgPos.X),
                                                    (float)(Math.Sin(MathHelper.ToRadians(90)) * (curPos.X - avgPos.X) + Math.Cos(MathHelper.ToRadians(90)) * (curPos.Y - avgPos.Y) + avgPos.Y));
                                    STATIC_EDITOR_MODE.levelInstance.DecalManager.DecalList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Rotation =
                                        MathHelper.ToDegrees(STATIC_EDITOR_MODE.levelInstance.DecalManager.DecalList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Rotation) + 90;
                                }
                                break;
                        }
                    }
                    break;
                case "Per Object":
                    for (int i = STATIC_EDITOR_MODE.selectedObjectIndices.Count - 1; i >= 0; i--)
                    {
                        switch (STATIC_EDITOR_MODE.selectedObjectIndices[i].Type)
                        {
                            case (OBJECT_TYPE.Physics):
                                {
                                    STATIC_EDITOR_MODE.levelInstance.PhysicsObjectsList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].TextureRotation =
                                        MathHelper.ToDegrees(STATIC_EDITOR_MODE.levelInstance.PhysicsObjectsList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].TextureRotation)
                                        + 90;
                                }
                                break;
                            case (OBJECT_TYPE.Decal):
                                {
                                    STATIC_EDITOR_MODE.levelInstance.DecalManager.DecalList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Rotation =
                                        MathHelper.ToDegrees(STATIC_EDITOR_MODE.levelInstance.DecalManager.DecalList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Rotation)
                                        + 90;
                                }
                                break;
                        }
                    }
                    break;
            }
            Handle_Property_Grid_Items();
            Update_undoArray();
        }

        private void BUTTON_ROTATE_SELECTION_ANTICLOCKWISE_Click(object sender, EventArgs e)
        {
            switch (Rotate_Relative.SelectedItem.ToString())
            {
                case "As Group":
                    Vector2 avgPos = new Vector2(get_avg_xpos(), get_avg_ypos());

                    for (int i = STATIC_EDITOR_MODE.selectedObjectIndices.Count - 1; i >= 0; i--)
                    {
                        switch (STATIC_EDITOR_MODE.selectedObjectIndices[i].Type)
                        {
                            case (OBJECT_TYPE.Physics):
                                {
                                    Vector2 curPos = STATIC_EDITOR_MODE.levelInstance.PhysicsObjectsList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Position;
                                    STATIC_EDITOR_MODE.levelInstance.PhysicsObjectsList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Position =
                                        new Vector2((float)(Math.Cos(MathHelper.ToRadians(-90)) * (curPos.X - avgPos.X) - Math.Sin(MathHelper.ToRadians(-90)) * (curPos.Y - avgPos.Y) + avgPos.X),
                                                    (float)(Math.Sin(MathHelper.ToRadians(-90)) * (curPos.X - avgPos.X) + Math.Cos(MathHelper.ToRadians(-90)) * (curPos.Y - avgPos.Y) + avgPos.Y));
                                    Type t = STATIC_EDITOR_MODE.levelInstance.PhysicsObjectsList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].GetType();
                                    if (t.BaseType == typeof(DynamicObject))
                                    {
                                        DynamicObject dyOb = (DynamicObject)STATIC_EDITOR_MODE.levelInstance.PhysicsObjectsList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index];
                                        Vector2 curEndPos = dyOb.EndPosition;
                                        dyOb.EndPosition =
                                            new Vector2((float)(Math.Cos(MathHelper.ToRadians(-90)) * (curEndPos.X - avgPos.X) - Math.Sin(MathHelper.ToRadians(-90)) * (curEndPos.Y - avgPos.Y) + avgPos.X),
                                                        (float)(Math.Sin(MathHelper.ToRadians(-90)) * (curEndPos.X - avgPos.X) + Math.Cos(MathHelper.ToRadians(-90)) * (curEndPos.Y - avgPos.Y) + avgPos.Y));
                                        STATIC_EDITOR_MODE.levelInstance.PhysicsObjectsList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index] = dyOb;
                                    }
                                    STATIC_EDITOR_MODE.levelInstance.PhysicsObjectsList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].TextureRotation =
                                        MathHelper.ToDegrees(STATIC_EDITOR_MODE.levelInstance.PhysicsObjectsList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].TextureRotation) - 90;
                                }
                                break;
                            case (OBJECT_TYPE.Decal):
                                {
                                    Vector2 curPos = STATIC_EDITOR_MODE.levelInstance.DecalManager.DecalList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Position;
                                    STATIC_EDITOR_MODE.levelInstance.DecalManager.DecalList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Position =
                                        new Vector2((float)(Math.Cos(MathHelper.ToRadians(-90)) * (curPos.X - avgPos.X) - Math.Sin(MathHelper.ToRadians(-90)) * (curPos.Y - avgPos.Y) + avgPos.X),
                                                    (float)(Math.Sin(MathHelper.ToRadians(-90)) * (curPos.X - avgPos.X) + Math.Cos(MathHelper.ToRadians(-90)) * (curPos.Y - avgPos.Y) + avgPos.Y));
                                    STATIC_EDITOR_MODE.levelInstance.DecalManager.DecalList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Rotation =
                                        MathHelper.ToDegrees(STATIC_EDITOR_MODE.levelInstance.DecalManager.DecalList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Rotation) - 90;
                                }
                                break;
                        }
                    }
                    break;
                case "Per Object":
                    for (int i = STATIC_EDITOR_MODE.selectedObjectIndices.Count - 1; i >= 0; i--)
                    {
                        switch (STATIC_EDITOR_MODE.selectedObjectIndices[i].Type)
                        {
                            case (OBJECT_TYPE.Physics):
                                {
                                    STATIC_EDITOR_MODE.levelInstance.PhysicsObjectsList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].TextureRotation =
                                        MathHelper.ToDegrees(STATIC_EDITOR_MODE.levelInstance.PhysicsObjectsList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].TextureRotation)
                                        - 90;
                                }
                                break;
                            case (OBJECT_TYPE.Decal):
                                {
                                    STATIC_EDITOR_MODE.levelInstance.DecalManager.DecalList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Rotation =
                                        MathHelper.ToDegrees(STATIC_EDITOR_MODE.levelInstance.DecalManager.DecalList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Rotation)
                                        - 90;
                                }
                                break;
                        }
                    }
                    break;
            }
            Handle_Property_Grid_Items();
            Update_undoArray();
        }
        #endregion
        #endregion

        #endregion

        #region Texture List Methods

        private void Refresh_XNB_Asset_List()
        {
            listBox_Assets0.Items.Clear();
            listBox_Assets1.Items.Clear();
            listBox_Assets2.Items.Clear();
            listBox_Assets3.Items.Clear();

            string[] lst_Files = Directory.GetFiles(STATIC_CONTBUILDER.pathToContent(), "*.xnb", SearchOption.TopDirectoryOnly);

            for (int i = 0; i < lst_Files.Length; i++)
            {
                //  If the file name contains "i_" anywhere,
                //  it will be ignored as a choice. For use
                //  with system only files.
                if (lst_Files[i].Contains("i_"))
                    continue;

                listBox_Assets0.Items.Add(Path.GetFileNameWithoutExtension(lst_Files[i]));
                listBox_Assets1.Items.Add(Path.GetFileNameWithoutExtension(lst_Files[i]));
                listBox_Assets2.Items.Add(Path.GetFileNameWithoutExtension(lst_Files[i]));
                listBox_Assets3.Items.Add(Path.GetFileNameWithoutExtension(lst_Files[i]));
            }
        }

        #region Update TextureList to match changed object type
        private void listBox_Classes_SelectedIndexChanged(object sender, EventArgs e)
        {
            #region Selected-Class Texture Switch
            switch (listBox_Classes.Items[listBox_Classes.SelectedIndex].ToString())
            {
                case "Decal":
                    STATIC_CONTBUILDER.textureLoc = "Assets/Images/Textures/Decal/";
                    break;
                case "Door":
                    STATIC_CONTBUILDER.textureLoc = "Assets/Images/Textures/Doors/";
                    break;
                case "Ladder":
                    STATIC_CONTBUILDER.textureLoc = "Assets/Images/Textures/Ladder/";
                    break;
                case "Moving Platform":
                    STATIC_CONTBUILDER.textureLoc = "Assets/Images/Textures/Environment/";
                    break;
                case "One-Sided Platform":
                    STATIC_CONTBUILDER.textureLoc = "Assets/Images/Textures/Environment/";
                    break;
                case "Piston":
                    STATIC_CONTBUILDER.textureLoc = "Assets/Images/Textures/Piston/";
                    break;
                case "Rope":
                    STATIC_CONTBUILDER.textureLoc = "Assets/Images/Textures/Rope/";
                    break;
                case "Rotate Room Button":
                    STATIC_CONTBUILDER.textureLoc = "Assets/Images/Textures/Switch/";
                    break;
                case "Rotating Platform":
                    STATIC_CONTBUILDER.textureLoc = "Assets/Images/Textures/Environment/";
                    break;
                case "Saw Blade":
                    STATIC_CONTBUILDER.textureLoc = "Assets/Images/Textures/Saw/";
                    break;
                case "Spikes":
                    STATIC_CONTBUILDER.textureLoc = "Assets/Images/Textures/Spikes/";
                    break;
                case "Static Object":
                    STATIC_CONTBUILDER.textureLoc = "Assets/Images/Textures/Environment/";
                    break;
                default:
                    STATIC_CONTBUILDER.textureLoc = "Assets/Images/Textures/";
                    break;
            }
            #endregion

            Refresh_XNB_Asset_List();

            if (listBox_Classes.Items[listBox_Classes.SelectedIndex].ToString() == "Piston")
            {
                listBox_Assets0.SelectedItem = "PistonBase";
                listBox_Assets1.SelectedItem = "Piston2";
                listBox_Assets2.SelectedItem = "Piston1";
                listBox_Assets3.SelectedItem = "PistonEnd";
            }
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

        private void ZOOM_ValueChanged(object sender, EventArgs e)
        {
            xnA_RenderControl1.Camera.Zoom = (float)ZOOM.Value / 100;
        }
        #endregion

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
                        for (int i = 0; i < STATIC_EDITOR_MODE.levelInstance.PhysicsObjectsList.Count; i++)
                        {
                            if (STATIC_EDITOR_MODE.levelInstance.PhysicsObjectsList[i] != null)
                            {
                                Rectangle BoundingBox = new Rectangle(
                                    (int)(STATIC_EDITOR_MODE.levelInstance.PhysicsObjectsList[i].Position.X - STATIC_EDITOR_MODE.levelInstance.PhysicsObjectsList[i].Width / 2),
                                    (int)(STATIC_EDITOR_MODE.levelInstance.PhysicsObjectsList[i].Position.Y - STATIC_EDITOR_MODE.levelInstance.PhysicsObjectsList[i].Height / 2),
                                    (int)STATIC_EDITOR_MODE.levelInstance.PhysicsObjectsList[i].Width,
                                    (int)STATIC_EDITOR_MODE.levelInstance.PhysicsObjectsList[i].Height);

                                if (BoundingBox.Contains(MousePos))
                                {
                                    lst_ObjectsUnderCursor.Add(new ObjectIndex(OBJECT_TYPE.Physics, i));
                                }
                            }
                        }

                        for (int i = 0; i < STATIC_EDITOR_MODE.levelInstance.DecalManager.DecalList.Count; i++)
                        {
                            if (STATIC_EDITOR_MODE.levelInstance.DecalManager.DecalList[i].AssetLocation != null)
                            {
                                Rectangle BoundingBox = new Rectangle(
                                    (int)(STATIC_EDITOR_MODE.levelInstance.DecalManager.DecalList[i].Position.X - STATIC_EDITOR_MODE.levelInstance.DecalManager.DecalList[i].Width / 2),
                                    (int)(STATIC_EDITOR_MODE.levelInstance.DecalManager.DecalList[i].Position.Y - STATIC_EDITOR_MODE.levelInstance.DecalManager.DecalList[i].Height / 2),
                                    (int)STATIC_EDITOR_MODE.levelInstance.DecalManager.DecalList[i].Width,
                                    (int)STATIC_EDITOR_MODE.levelInstance.DecalManager.DecalList[i].Height);

                                if (BoundingBox.Contains(MousePos))
                                {
                                    lst_ObjectsUnderCursor.Add(new ObjectIndex(OBJECT_TYPE.Decal, i));
                                }
                            }
                        }

                        //int topIndexZ = 0;
                        //for (int j = 0; j < lst_ObjectsUnderCursor.Count; j++)
                        //{
                        //    if (STATIC_EDITOR_MODE.levelInstance.PhysicsObjectsList[lst_ObjectsUnderCursor[j]].zLayer > STATIC_EDITOR_MODE.levelInstance.PhysicsObjectsList[lst_ObjectsUnderCursor[topIndexZ]].zLayer)
                        //    {
                        //        topIndexZ = j;
                        //    }
                        //}
                        #endregion
                    }
                    break;
                case EDITOR_MODE.MOVE:
                    {
                        #region Handle Moving Objects
                        if (Is_Something_Selected())
                        {
                            for (int i = STATIC_EDITOR_MODE.selectedObjectIndices.Count - 1; i >= 0 && containsMouse == false; i--)
                            {
                                Microsoft.Xna.Framework.Rectangle newRect = new Microsoft.Xna.Framework.Rectangle();
                                switch (STATIC_EDITOR_MODE.selectedObjectIndices[i].Type)
                                {
                                    case (OBJECT_TYPE.Physics):
                                        {
                                            newRect = new Microsoft.Xna.Framework.Rectangle(
                                                    (int)STATIC_EDITOR_MODE.levelInstance.PhysicsObjectsList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Position.X - (int)STATIC_EDITOR_MODE.levelInstance.PhysicsObjectsList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Width / 2,
                                                    (int)STATIC_EDITOR_MODE.levelInstance.PhysicsObjectsList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Position.Y - (int)STATIC_EDITOR_MODE.levelInstance.PhysicsObjectsList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Height / 2,
                                                    (int)STATIC_EDITOR_MODE.levelInstance.PhysicsObjectsList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Width,
                                                    (int)STATIC_EDITOR_MODE.levelInstance.PhysicsObjectsList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Height);
                                        }
                                        break;
                                    case (OBJECT_TYPE.Decal):
                                        {
                                            newRect = new Microsoft.Xna.Framework.Rectangle(
                                                    (int)STATIC_EDITOR_MODE.levelInstance.DecalManager.DecalList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Position.X - (int)STATIC_EDITOR_MODE.levelInstance.DecalManager.DecalList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Width / 2,
                                                    (int)STATIC_EDITOR_MODE.levelInstance.DecalManager.DecalList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Position.Y - (int)STATIC_EDITOR_MODE.levelInstance.DecalManager.DecalList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Height / 2,
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
                                                    STATIC_EDITOR_MODE.levelInstance.PhysicsObjectsList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].Position += moveDis;
                                                    Type t = STATIC_EDITOR_MODE.levelInstance.PhysicsObjectsList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index].GetType();
                                                    if (t.BaseType == typeof(DynamicObject))
                                                    {
                                                        DynamicObject dyOb = (DynamicObject)STATIC_EDITOR_MODE.levelInstance.PhysicsObjectsList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index];
                                                        dyOb.EndPosition += moveDis;
                                                        STATIC_EDITOR_MODE.levelInstance.PhysicsObjectsList[STATIC_EDITOR_MODE.selectedObjectIndices[i].Index] = dyOb;
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
            #endregion

            if (CheckNewKey(Microsoft.Xna.Framework.Input.Keys.S) && STATIC_EDITOR_MODE.keyboardCurrentState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.LeftControl))
            {
                SaveFile();
            }

            if (CheckNewKey(Microsoft.Xna.Framework.Input.Keys.D) && STATIC_EDITOR_MODE.keyboardCurrentState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.LeftControl))
            {
                STATIC_EDITOR_MODE.selectedObjectIndices.Clear();
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
            #endregion

            #region Toggles

            if (CheckNewKey(Microsoft.Xna.Framework.Input.Keys.F1))
            {
                xnA_RenderControl1.HideCoordinates = !xnA_RenderControl1.HideCoordinates;
            }

            if (CheckNewKey(Microsoft.Xna.Framework.Input.Keys.F2))
            {
                xnA_RenderControl1.HideGrid = !xnA_RenderControl1.HideGrid;
            }

            if (CheckNewKey(Microsoft.Xna.Framework.Input.Keys.F3))
            {
                xnA_RenderControl1.HideMovementPath = !xnA_RenderControl1.HideMovementPath;
            }

            if (CheckNewKey(Microsoft.Xna.Framework.Input.Keys.F4))
            {
                xnA_RenderControl1.HideOverlay = !xnA_RenderControl1.HideOverlay;
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
    }
}
