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
using GameLibrary.Graphics.UI;

namespace GameLibrary.GameLogic.Screens
{
    public class MessageOverlay : GameScreen
    {
        #region Fields

        private int _ID;
        private Texture2D _texture;
        private Vector2 _position = Vector2.Zero;
        private SpriteFont _font;
        private TextString _closeText;

        private InputAction _moveUp;
        private InputAction _moveDown;
        private InputAction _inputContinue;

        private float _elapsed = 0.0f;
        private bool _playingSound = false;
        private string _noteString = null;
        private Vector2 _offset = new Vector2(120, 80);
        private Cue _voiceoverSound;

        #endregion

        public MessageOverlay(int ID)
            : base()
        {
            this._ID = ID;
            this._font = FontManager.Instance.GetFont(FontList.Notes);

            this._inputContinue = new InputAction(
                new Buttons[] { Buttons.A, Buttons.B, Buttons.Back, Buttons.Start },
                new Keys[] { Keys.Space, Keys.Enter, Keys.Escape, Keys.Tab, Keys.E }, true);
            this._moveUp = new InputAction(new Buttons[] { Buttons.RightThumbstickUp, Buttons.LeftThumbstickUp },
                new Keys[] { Keys.Up, Keys.W, Keys.PageUp }, false);
            this._moveDown = new InputAction(new Buttons[] { Buttons.RightThumbstickDown, Buttons.LeftThumbstickDown },
                new Keys[] { Keys.Down, Keys.S, Keys.PageDown }, false);
        }

        public override void Activate()
        {
            ContentManager content = new ContentManager(this.ScreenManager.Game.Services, "Content");
            PresentationParameters pp = this.ScreenManager.GraphicsDevice.PresentationParameters;

            AudioManager.Instance.PauseSounds();

            this._closeText = new TextString(" to close.");
            this._closeText.Load(content);
            this._closeText.TextAlignment = TextAlignment.Left;
            this._closeText.ButtonType = (ButtonIcon)1;
            this._closeText.Position = new Vector2(this.ScreenManager.GraphicsDevice.Viewport.Height * 0.03f, this.ScreenManager.GraphicsDevice.Viewport.Height * 0.96f);

            this._texture = content.Load<Texture2D>(Defines.NOTE_DIRECTORY + "Note_Empty");
            this._font = FontManager.Instance.GetFont(FontList.Notes);
            string path = "./Content/Assets/Other/Notes/" + _ID + ".txt";

            this._noteString = File.ReadAllText(path);

            string[] noteWords = _noteString.Split(' ');
            string lineString = String.Empty;
            StringBuilder builder = new StringBuilder();
            int strPos = 0;
            float width = _texture.Width - _offset.X;

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


            this._position = new Vector2(pp.BackBufferWidth * 0.5f, pp.BackBufferHeight * 0.2f);
            
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

            AudioManager.Instance.ResumeSounds();
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
            float speed = 5;

            if (_inputContinue.Evaluate(input, ControllingPlayer, out playerIndex))
            {
                OnCancel(playerIndex);
            }

            if (_moveUp.Evaluate(input, ControllingPlayer, out playerIndex))
            {
                if (_position.Y < this.ScreenManager.GraphicsDevice.Viewport.Height * 0.2)
                {
                    this._position.Y += speed;
                }
            }

            if (_moveDown.Evaluate(input, ControllingPlayer, out playerIndex))
            {
                if (_position.Y + _texture.Height >= this.ScreenManager.GraphicsDevice.Viewport.Height * 0.8)
                {
                    this._position.Y -= speed;
                }
            }
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch spriteBatch = this.ScreenManager.SpriteBatch;
            
            Vector2 noteTopLeft = _position - new Vector2(_texture.Width * 0.5f, 0);

            spriteBatch.Begin();

            //  Note
            spriteBatch.Draw(_texture, _position, null, Color.White, 0.0f, new Vector2(_texture.Width * 0.5f, 0), 1.0f, SpriteEffects.None, 1.0f);

            //  Text
            spriteBatch.DrawString(_font, _noteString, noteTopLeft + new Vector2(_offset.X * 0.5f, _offset.Y), Color.Black, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0.5f);

            _closeText.Draw(spriteBatch);

            spriteBatch.End();
        }

        protected virtual void OnCancel(PlayerIndex playerIndex)
        {
            ExitScreen();
        }
    }
}
