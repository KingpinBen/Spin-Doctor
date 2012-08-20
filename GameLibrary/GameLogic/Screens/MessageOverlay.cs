using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using GameLibrary.GameLogic.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using GameLibrary.Graphics;
using GameLibrary.Helpers;
using System.IO;
using GameLibrary.Audio;
using Microsoft.Xna.Framework.Audio;

namespace GameLibrary.GameLogic.Screens
{
    public class MessageOverlay : GameScreen
    {
        #region Fields

        private int _ID;
        private Texture2D _texture;
        private Vector2 _position = Vector2.Zero;
        private SpriteFont _font;

        private InputAction moveUp;
        private InputAction moveDown;
        private InputAction inputContinue;

        private float _elapsed = 0.0f;
        private bool _playingSound = false;
        private string _noteString = null;
        private Vector2 offset = new Vector2(120, 80);
        private Cue _voiceoverSound;

        #endregion

        public MessageOverlay(int ID)
            : base()
        {
            this._ID = ID;
            this._font = FontManager.Instance.GetFont(FontList.Notes);

            this.inputContinue = new InputAction(
                new Buttons[] { Buttons.A, Buttons.B, Buttons.Back, Buttons.Start },
                new Keys[] { Keys.Space, Keys.Enter, Keys.Escape, Keys.Tab, Keys.E }, true);
            this.moveUp = new InputAction(new Buttons[] { Buttons.RightThumbstickUp, Buttons.LeftThumbstickUp },
                new Keys[] { Keys.Up, Keys.W, Keys.PageUp }, false);
            this.moveDown = new InputAction(new Buttons[] { Buttons.RightThumbstickDown, Buttons.LeftThumbstickDown },
                new Keys[] { Keys.Down, Keys.S, Keys.PageDown }, false);
        }

        public override void Activate()
        {
            ContentManager content = new ContentManager(this.ScreenManager.Game.Services, "Content");
            PresentationParameters pp = this.ScreenManager.GraphicsDevice.PresentationParameters;


            this._texture = content.Load<Texture2D>(Defines.NOTE_DIRECTORY + "Note_Empty");
            this._font = FontManager.Instance.GetFont(FontList.Notes);
            string path = "./Content/Assets/Other/Notes/" + _ID + ".txt";

            _noteString = File.ReadAllText(path);

            string[] noteWords = _noteString.Split(' ');
            string lineString = String.Empty;
            StringBuilder builder = new StringBuilder();
            int strPos = 0;
            float width = _texture.Width - offset.X;

            for (int i = 0; i < noteWords.Length; i++)
            {
                string word = noteWords[i];
                string[] removeWord = new string[] { "\n", "\r" };
                string[] words;

                if (word.Contains("\n"))
                {
                    words = word.Split(removeWord, StringSplitOptions.None);
                    bool newLineComplete = false;
                    foreach (string str in words)
                    {
                        if (str == "" && !newLineComplete)
                        {
                            builder.AppendFormat("{0}", Environment.NewLine);
                            builder.AppendFormat("{0}", Environment.NewLine);
                            newLineComplete = true;
                        }
                        else
                        {
                            builder.AppendFormat(" {0}", str);
                            strPos = str.Length + 1;
                            lineString = str;
                        }
                    }

                    continue;
                }

                float size = _font.MeasureString(lineString).X + _font.MeasureString(word).X;

                if (size < width)
                {
                    builder.AppendFormat(" {0}", word);
                    strPos += word.Length + 1;
                    lineString += " " + word;
                }
                else
                {
                    builder.AppendFormat("{0}{1}", Environment.NewLine, word);
                    strPos = word.Length;
                    lineString = word;
                }
            }

            _noteString = builder.ToString();


            this._position = new Vector2(pp.BackBufferWidth * 0.5f, pp.BackBufferHeight);
            
            base.Activate();
        }

        public override void Deactivate()
        {
            //  Check the cue has been played before trying to remove.
            if (_voiceoverSound != null)
            {
                if (!_voiceoverSound.IsStopped)
                {
                    this._voiceoverSound.Stop(AudioStopOptions.AsAuthored);
                }
            }
        }

        public override void Update(float delta, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(delta, otherScreenHasFocus, coveredByOtherScreen);

            if (!_playingSound)
            {
                if (_elapsed < 0.5f)
                {
                    this._elapsed += delta;
                }
                else
                {
                    this._playingSound = true;
                    this._elapsed = 0.0f;
                    this._voiceoverSound = AudioManager.Instance.PlayCue("Dialog_Reeves_Note" + _ID, true);
                }
            }
        }

        public override void HandleInput(float delta, InputState input)
        {
            PlayerIndex playerIndex;

            if (inputContinue.Evaluate(input, ControllingPlayer, out playerIndex))
            {
                OnCancel(playerIndex);
            }
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch spriteBatch = this.ScreenManager.SpriteBatch;
            
            Vector2 noteTopLeft = _position - new Vector2(_texture.Width * 0.5f, _texture.Height);

            spriteBatch.Begin();
            //  Note
            spriteBatch.Draw(_texture, _position, null, Color.White, 0.0f, new Vector2(_texture.Width * 0.5f, _texture.Height), 1.0f, SpriteEffects.None, 1.0f);

            //  Text
            spriteBatch.DrawString(_font, _noteString, noteTopLeft + new Vector2(offset.X * 0.5f, offset.Y), Color.Black, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0.5f);

            spriteBatch.End();
        }

        protected virtual void OnCancel(PlayerIndex playerIndex)
        {
            ExitScreen();
        }
    }
}
