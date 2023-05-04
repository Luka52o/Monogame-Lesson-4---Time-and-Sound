using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Threading;
using System.Transactions;

namespace Monogame_Lesson_4___Time_and_Sound
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        SpriteFont timeFont;
        SoundEffect explodeSFX, cheerSFX;
        SoundEffectInstance explodeSFXInstance, cheerSFXInstance;
        Rectangle bombRect, explosionRect, bombScreenRect, bombWiresRect, confettiRect;
        Texture2D bombTexture, explosionTexture, rectangleTexture, confettiTexture;
        MouseState mouseState;
        float seconds, startTime, defuseTime;
        bool exploded = false, defused = false;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            _graphics.PreferredBackBufferWidth = 800;
            _graphics.PreferredBackBufferHeight = 500;
            Window.Title = "Bombs Away";
            bombRect = new Rectangle(50, 50, 700, 400);
            explosionRect = new Rectangle(-200, -200, 1600, 1000);
            bombScreenRect = new Rectangle(225, 140, 260, 200);
            bombWiresRect = new Rectangle(650, 100, 260, 200);
            confettiRect = new Rectangle(0, 0, 800, 500);

            _graphics.ApplyChanges();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            timeFont = Content.Load<SpriteFont>("Font");
            bombTexture = Content.Load<Texture2D>("bomb");
            explosionTexture = Content.Load<Texture2D>("explosionIMG");
            explodeSFX = Content.Load<SoundEffect>("explosion");
            cheerSFX = Content.Load<SoundEffect>("cheer");
            rectangleTexture = Content.Load<Texture2D>("rectangle");
            confettiTexture = Content.Load<Texture2D>("confetti");
            explodeSFXInstance = explodeSFX.CreateInstance();
            cheerSFXInstance = cheerSFX.CreateInstance();
            _graphics.ApplyChanges();
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            mouseState = Mouse.GetState();
            if (mouseState.LeftButton == ButtonState.Pressed && bombWiresRect.Contains(mouseState.X, mouseState.Y))
            {
                defuseTime = (10 - seconds);
                defused = true;
                cheerSFXInstance.Play();
            }
            else if (mouseState.LeftButton == ButtonState.Pressed && bombScreenRect.Contains(mouseState.X, mouseState.Y))
            {
                startTime = (float)gameTime.TotalGameTime.TotalSeconds;
            }
           

            seconds = (float)gameTime.TotalGameTime.TotalSeconds - startTime;
            if (seconds > 10)
                startTime = (float)gameTime.TotalGameTime.TotalSeconds;

            if (seconds >= 10)
            {
                explodeSFXInstance.Play();
                exploded = true;
            }
            if (exploded && explodeSFXInstance.State == SoundState.Stopped)
                Exit();
            if (defused && cheerSFXInstance.State == SoundState.Stopped)
                Exit();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.RosyBrown);
            _spriteBatch.Begin();

            _spriteBatch.Draw(bombTexture, bombRect, Color.White);
            if (defused != true)
            {
                _spriteBatch.DrawString(timeFont, (10.0 - seconds).ToString("00.0"), new Vector2(270, 200), Color.Black);
            }
            else
            {
                _spriteBatch.DrawString(timeFont, (defuseTime).ToString("00.0"), new Vector2(270, 200), Color.Black);
                _spriteBatch.Draw(confettiTexture, confettiRect, Color.White);
            }

            if (exploded == true)
            {
                _spriteBatch.Draw(explosionTexture, explosionRect, Color.White);
            }

            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}