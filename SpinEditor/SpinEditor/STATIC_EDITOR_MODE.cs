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
        public static EDITOR_MODE ED_MODE;
        public static ALIGN_ANCHOR ALIGN_TO;

        public static Level levelInstance;

        public static World world = new World(Vector2.Zero);

        public static bool bIsSomethingSelected;

        public static List<ObjectIndex> selectedObjectIndices;
        
        public static NodeObject[][] undoObjArray;
        public static Decal[][] undoDecalArray;
        public static int arrayLength = 0;
        public static int arrayMax = 100;
        public static int arrayIndex = -1;

        public static KeyboardState keyboardCurrentState = Keyboard.GetState();
        public static KeyboardState keyboardOldState;

        public static MouseState mouseCurrentState = Mouse.GetState();
        public static MouseState mouseOldState;

        public static void Setup()
        {
            levelInstance = new Level();
            ALIGN_TO = ALIGN_ANCHOR.LAST;
            bIsSomethingSelected = false;
            selectedObjectIndices = new List<ObjectIndex>();
            undoObjArray = new NodeObject[arrayMax][];
            undoDecalArray = new Decal[arrayMax][];
        }

    }
}
