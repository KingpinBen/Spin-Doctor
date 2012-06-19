using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using GameLibrary.Drawing;
using FarseerPhysics;
using Microsoft.Xna.Framework.Input;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using GameLibrary.Objects;

namespace SpinEditor
{
    public enum EDITOR_MODE
    {
        SELECT,
        MOVE,
        PLACE,
        EDIT_LEVEL
    }

    public enum ALIGN_ANCHOR
    {
        LAST,
        FIRST,
        SELECTION
    }

    public static class STATIC_EDITOR_MODE
    {
        public static EDITOR_MODE ED_MODE = EDITOR_MODE.EDIT_LEVEL;
        public static ALIGN_ANCHOR ALIGN_TO = ALIGN_ANCHOR.LAST;

        public static ContentManager contentMan;

        public static GameLibrary.Drawing.Level levelInstance;

        public static World world = new World(Vector2.Zero);

        public static bool bIsSomethingSelected = false;

        public static List<ObjectIndex> selectedObjectIndices = new List<ObjectIndex>();
        
        public static List<List<PhysicsObject>> listList = new List<List<PhysicsObject>>();
        public static PhysicsObject[][] undoPhysObjArray;
        public static int arrayLength = 0;
        public static int arrayMax = 100;
        public static int arrayIndex = -1;

        public static KeyboardState keyState = Keyboard.GetState();
        public static KeyboardState oldState;

    }
}
