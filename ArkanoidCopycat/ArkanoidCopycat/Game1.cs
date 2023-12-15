using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace Arkanoid
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        // texture barra e pallina
        Texture2D barTexture;
        Texture2D ballTexture;

        // inizializzazione posizione e velocità di barra e pallina
        Vector2 barPosition;
        float barSpeed;
        Vector2 ballPosition;
        float ballSpeed;
        
        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            _graphics.PreferredBackBufferHeight = 800;
            _graphics.PreferredBackBufferWidth = 1024;
            _graphics.ApplyChanges();

            // velocità barra e palla
            barSpeed = 160f;
            ballSpeed = 200f;
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // caricamento texture barra e pallina
            barTexture = Content.Load<Texture2D>("arkanoid_bar");
            ballTexture = Content.Load<Texture2D>("arkanoid_ball");

            // posizione iniziale barra e pallina
            barPosition = new Vector2(_graphics.PreferredBackBufferWidth / 2 - barTexture.Width / 2, _graphics.PreferredBackBufferHeight - barTexture.Height);
            ballPosition = new Vector2(_graphics.PreferredBackBufferWidth / 2 - ballTexture.Width / 2, barPosition.Y);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            var kstate = Keyboard.GetState();

            // movimento barra a destra
            if (kstate.IsKeyDown(Keys.Right))
            {
                barPosition.X += barSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }

            // movimento barra a sinistra
            if (kstate.IsKeyDown(Keys.Left))
            {
                barPosition.X -= barSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }

            // bordi barra
            if (barPosition.X > _graphics.PreferredBackBufferWidth - barTexture.Width)
            {
                barPosition.X = _graphics.PreferredBackBufferWidth - barTexture.Width;
            }
            else if (barPosition.X < barTexture.Width / 100)
            {
                barPosition.X = barTexture.Width / 100;
            }

            if (barPosition.Y > _graphics.PreferredBackBufferHeight - barTexture.Height)
            {
                barPosition.Y = _graphics.PreferredBackBufferHeight - barTexture.Height;
            }
            else if (barPosition.Y < barTexture.Height / 100)
            {
                barPosition.Y = barTexture.Height / 100;
            }

            // movimento pallina
            if (kstate.IsKeyDown(Keys.Space))
            {
                
            }

            // bordi pallina
            if (ballPosition.X > _graphics.PreferredBackBufferWidth - ballTexture.Width)
            {
                ballPosition.X = _graphics.PreferredBackBufferWidth - ballTexture.Width;
            }
            else if (ballPosition.X < ballTexture.Width / 100)
            {
                ballPosition.X = ballTexture.Width / 100;
            }

            if (ballPosition.Y > _graphics.PreferredBackBufferHeight - ballTexture.Height)
            {
                ballPosition.Y = _graphics.PreferredBackBufferHeight - ballTexture.Height;
            }
            else if (ballPosition.Y < ballTexture.Height / 100)
            {
                ballPosition.Y = ballTexture.Height / 100;
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // inserimento barra e pallina
            _spriteBatch.Begin();
            _spriteBatch.Draw(barTexture, barPosition, Color.White);
            _spriteBatch.Draw(ballTexture, ballPosition, Color.White);
            _spriteBatch.End();

            base.Draw(gameTime);

        }
    }
}