using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using GameLibrary.Graphics;
using System.IO;
using GameLibrary.Helpers;

namespace GameLibrary.GameLogic.Screens
{
    internal enum State
    {
        FadeIn, Show, FadeOut, Hidden
    }

    public class CreditRollScreen : GameScreen
    {
        private static bool _creditsRolling = false;

        private Vector2 _scrollPosition;
        private string[] _creditsText;
        private float _alpha = 0.0f;
        private float _elapsed;
        private float _sectionShowingTime;
        private float _sectionSeparationTime;
        private float _speedMultiplier;
        private State _state = State.Hidden;
        private int _creditSection = 0;
        private SpriteFont _font;
        private Texture2D _gameLogo;

        private CreditRollScreen()
        {
        }

        public override void Activate()
        {
            ContentManager content = new ContentManager(this.ScreenManager.Game.Services, "Content");

            //  Set the position.
            Viewport vp = this.ScreenManager.Game.GraphicsDevice.Viewport;
            this._scrollPosition = new Vector2(vp.Width * 0.1f, vp.Height * 0.25f);

            //  We want it to be a non-interactable popup.
            this.AcceptInput = false;
            this.IsPopup = true;

            //  Get the font.
            this._font = FontManager.Instance.GetFont(FontList.Credits);

            this._gameLogo = content.Load<Texture2D>("Assets/Other/Game/TitleLogo");

            //  Sort out the credits text.
            string path = "./Content/Assets/Other/Game/Credits.txt";
            string fullCredits = File.ReadAllText(path);
            //  Make each '#' a section break.
            this._creditsText = fullCredits.Split('#');

            this._sectionSeparationTime = Defines.SYSTEM_CREDITS_SECT_SEPARATION;
            this._speedMultiplier = Defines.SYSTEM_CREDITS_FADE_MULTIPLIER;
            this._sectionShowingTime = Defines.SYSTEM_CREDITS_TIME_ON_SCREEN;
        }

        public static void Load(ScreenManager screenManager, 
                                PlayerIndex? controllingPlayer)
        {
            //  We only want one instance of the credits screen.
            if (!_creditsRolling)
            {
                CreditRollScreen screen = new CreditRollScreen();
                screenManager.AddScreen(screen, controllingPlayer);
                _creditsRolling = true;
            }
        }

        public override void Update(float delta, bool otherScreenHasFocus, 
                                    bool coveredByOtherScreen)
        {
            base.Update(delta, true, false);

            this._elapsed += delta;

            switch (_state)
            {
                case State.FadeIn:
                    {
                        this._alpha = Math.Min(_alpha + (_elapsed * _speedMultiplier), 1.0f);

                        if (_alpha >= 1)
                        {
                            this._state = State.Show;
                            this._elapsed = 0.0f;
                        }

                        break;
                    }
                case State.FadeOut:
                    {
                        this._alpha = Math.Max(_alpha - (_elapsed * _speedMultiplier), 0.0f);

                        if (_alpha <= 0.0f)
                        {
                            this._state = State.Hidden;
                            this._elapsed = 0.0f;

                            //  We want to go one above the credtext length 
                            //  so we can show the logo
                             if (_creditSection >= _creditsText.Length)
                            {
                                _creditsRolling = false;
                                this.ExitScreen();
                            }
                            else
                            {
                                this._creditSection++;
                            }
                        }

                        break;
                    }
                case State.Show:
                    {
                        if (_elapsed >= _sectionShowingTime)
                        {
                            this._state = State.FadeOut;
                            this._elapsed = 0.0f;
                        }

                        break;
                    }
                case State.Hidden:
                    {
                        if (_elapsed > _sectionSeparationTime)
                        {
                            this._state = State.FadeIn;
                            this._elapsed = 0.0f;
                        }

                        break;
                    }
            }
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch sb = this.ScreenManager.SpriteBatch;
            GraphicsDevice graphics = this.ScreenManager.GraphicsDevice;

            sb.Begin();

            if (_creditSection < _creditsText.Length)
            {
                sb.DrawString(_font, _creditsText[_creditSection], _scrollPosition, 
                    Color.DarkGoldenrod * _alpha, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0.0f);
            }
            else
            {
                sb.Draw(_gameLogo, new Vector2(graphics.Viewport.Width, graphics.Viewport.Height) * 0.5f, null, Color.White * _alpha, 0.0f,
                    new Vector2(_gameLogo.Width, _gameLogo.Height) * 0.5f, 0.5f, SpriteEffects.None, 0.0f);
            }


            sb.End();
        }
    }
}
