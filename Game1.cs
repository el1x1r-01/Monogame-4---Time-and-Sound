using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Monogame_4___Time_and_Sound
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        Rectangle window, bombRect, pliersRect, redWireRect, greenWireRect;

        Texture2D bombTexture, pliersTexture, explosionTexture;

        SpriteFont timeFont;

        float seconds;

        MouseState mouseState;

        SoundEffect explode, cheering;

        bool timerActive, explosionImage, exit;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = false;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            window = new Rectangle(0, 0, 800, 500);
            _graphics.PreferredBackBufferWidth = window.Width;
            _graphics.PreferredBackBufferHeight = window.Height;
            _graphics.ApplyChanges();

            bombRect = new Rectangle(50, 50, 700, 400);

            redWireRect = new Rectangle(715, 165, 50, 100);
            greenWireRect = new Rectangle(490, 140, 175, 40);

            seconds = 0;

            timerActive = true;
            explosionImage = false;
            exit = false;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);           

            // TODO: use this.Content to load your game content here

            bombTexture = Content.Load<Texture2D>("bomb");
            explosionTexture = Content.Load<Texture2D>("explosion imgae");
            pliersTexture = Content.Load<Texture2D>("pliers");

            timeFont = Content.Load<SpriteFont>("TimeFont");

            explode = Content.Load<SoundEffect>("explosion");
            cheering = Content.Load<SoundEffect>("kids_cheering");
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            if (timerActive)
            {
                seconds += (float)gameTime.ElapsedGameTime.TotalSeconds;
            }

            mouseState = Mouse.GetState();

            pliersRect = new Rectangle(mouseState.X, mouseState.Y, 300, 300);

            if (mouseState.LeftButton == ButtonState.Pressed && redWireRect.Contains(mouseState.Position))
            {
                explode.Play();
                explosionImage = true;

                seconds = 0f;

                exit = true;
            }

            if (mouseState.LeftButton == ButtonState.Pressed && greenWireRect.Contains(mouseState.Position))
            {
                timerActive = false;

                cheering.Play();
            }

            if (seconds >= 60f)
            {
                explode.Play();
                explosionImage = true;

                seconds = 0f;

                exit = true;
            }

            if (exit)
            {
                if (seconds >= 10f)
                {
                    Exit();
                }
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here

            _spriteBatch.Begin();

            _spriteBatch.Draw(bombTexture, bombRect, Color.White);
           
            _spriteBatch.DrawString(timeFont, (60 - seconds).ToString("00.0"), new Vector2(270, 200), Color.Black);

            if (explosionImage)
            {
                 _spriteBatch.Draw(explosionTexture, new Rectangle(0, 0, 800, 500), Color.White);
            }

            _spriteBatch.Draw(pliersTexture, pliersRect, Color.White);

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
