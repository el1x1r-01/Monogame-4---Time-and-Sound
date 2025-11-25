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

        SpriteFont timeFont, statusFont;

        float seconds, exitTimer;

        MouseState mouseState;

        SoundEffect explode, cheering;

        bool timerActive, explosionImage, exit, canClick;

        string statusText;

        Vector2 statusLocation;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = false;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            window = new Rectangle(0, 0, 800, 600);
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
            canClick = true;

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
            statusFont = Content.Load<SpriteFont>("StatusFont");

            explode = Content.Load<SoundEffect>("explosion");
            cheering = Content.Load<SoundEffect>("kids_cheering");
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            exitTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (timerActive)
            {
                seconds += (float)gameTime.ElapsedGameTime.TotalSeconds;

                statusLocation = new Vector2(50, 500);
                statusText = "Diffuse the bomb before it explodes!";
            }

            mouseState = Mouse.GetState();

            pliersRect = new Rectangle(mouseState.X, mouseState.Y, 300, 300);

            if (canClick)
            {
                if (mouseState.LeftButton == ButtonState.Pressed && redWireRect.Contains(mouseState.Position))
                {
                    statusLocation = new Vector2(50, 500);
                    statusText = "You cut the RED wire! The bomb explodes!";

                    explode.Play();
                    explosionImage = true;

                    seconds = 0f;
                    timerActive = false;

                    exitTimer = 0f;
                    exit = true;

                    canClick = false;
                }
            }

            if (canClick)
            {
                if (mouseState.LeftButton == ButtonState.Pressed && greenWireRect.Contains(mouseState.Position))
                {
                    statusLocation = new Vector2(35, 500);
                    statusText = "You cut the GREEN wire! You diffuse the bomb!";

                    timerActive = false;

                    cheering.Play();

                    exitTimer = 0f;
                    exit = true;

                    canClick = false;
                }
            }

            if (seconds >= 10f)
            {
                statusLocation = new Vector2(50, 500);
                statusText ="Time's up! The bomb explodes!";

                explode.Play();
                explosionImage = true;

                seconds = 0f;
                timerActive = false;

                exitTimer = 0f;
                exit = true;

                canClick = false;
            }

            if (exit)
            {
                canClick = false;

                if (exitTimer >= 10f)
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
           
            _spriteBatch.DrawString(timeFont, (10 - seconds).ToString("0.00"), new Vector2(270, 200), Color.Black);

            if (explosionImage)
            {
                 _spriteBatch.Draw(explosionTexture, new Rectangle(0, 0, 800, 500), Color.White);
            }

            _spriteBatch.Draw(pliersTexture, pliersRect, Color.White);

            _spriteBatch.DrawString(statusFont,statusText, statusLocation, Color.White);
            
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
