using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameLibrary.GameLogic.Screens
{
    class CreditHandler : GameScreen
    {
        private int _currentLevelBackup;

        public CreditHandler()
            : base()
        {
            GameSettings gameSettings = GameSettings.Instance;

            this._currentLevelBackup = gameSettings.CurrentLevel;
            gameSettings.CurrentLevel = 42;
        }

        public override void Deactivate()
        {
            GameSettings.Instance.CurrentLevel = _currentLevelBackup;
        }
    }
}
