using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;

namespace GameLibrary.GameLogic.Objects
{
    public interface IGameObject
    {
        /// <summary>
        /// The name of the object for event targetting.
        /// </summary>
        string Name { get; }

#if EDITOR
#else
        void Start();
        void Stop();
        void Enable();
        void Disable();
        void Toggle();
        void Change(object sent);
#endif
    }
}
