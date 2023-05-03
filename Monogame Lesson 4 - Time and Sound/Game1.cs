using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Threading;

namespace Monogame_Lesson_4___Time_and_Sound
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        SpriteFont timeFont;
        SoundEffect explodeSFX;
        SoundEffectInstance explodeSFXInstance;
        Rectangle bombRect, explosionRect, bombScreenRect, bombWiresRect;
        Texture2D bombTexture, explosionTexture;
        MouseState mouseState;
        float seconds, startTime;
        bool exploded = false;

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
            explosionRect = new Rectangle(0, 0, 900, 700);
            bombScreenRect = new Rectangle(50, 50, 400, 250);  // complete guess

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
            explodeSFXInstance = explodeSFX.CreateInstance();
            _graphics.ApplyChanges();
        }

        protected override void Update(GameTime gameTime)
        {
            mouseState = Mouse.GetState();
            if (mouseState.LeftButton == ButtonState.Pressed)
            {
                startTime = (float)gameTime.TotalGameTime.TotalSeconds;
            }
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            seconds = (float)gameTime.TotalGameTime.TotalSeconds - startTime;
            if (seconds > 10)
                startTime = (float)gameTime.TotalGameTime.TotalSeconds;

            if (seconds >= 60)
            {
                explodeSFXInstance.Play();
                exploded = true;
            }
            if (exploded && explodeSFXInstance.State == SoundState.Stopped)
                Exit();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            _spriteBatch.Begin();

            _spriteBatch.Draw(bombTexture, bombRect, Color.White);
            _spriteBatch.DrawString(timeFont, (60.0 - seconds).ToString("00.0"), new Vector2(270, 200), Color.Black);
            if (exploded == true)
            {
                _spriteBatch.Draw(explosionTexture, explosionRect, Color.White);
            }

            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}