using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Threading;

namespace ArkanoidCopycat
{
    public class Game1 : Game
    {
        abstract class Collision
        { 

        }
        class Cube : Collision
        { }
        class PlayerBar : Collision
        { }
        class Ball : Collision
        { }


        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        Rectangle ballCollision;
        const float ballSpeed = 4f;
        int initialBallPosX;
        int initialBallPosY;
        Vector2 ballMovement;
        Texture2D ballTexture;
        int lives = 3;
        Texture2D barTexture;
        Rectangle barCollision;
        int barSpeed;
        bool check = false;
        public void Sleep()
        {
            
        }
        public void Restart()
        {
            barCollision.X = _graphics.PreferredBackBufferWidth / 2;
            ballCollision.X = barCollision.X + ballTexture.Width;
            ballCollision.Y = barCollision.Y;
            lives--;
            
          
           
        }
        SpriteFont spriteFont;
        public Game1()
        {

            _graphics = new GraphicsDeviceManager(this);
            ;
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {

            // TODO: Add your initialization logic here
            _graphics.PreferredBackBufferHeight = 500;
            _graphics.PreferredBackBufferWidth = 400;

            _graphics.ApplyChanges();
            ballMovement.Y = ballSpeed;

            initialBallPosX = barCollision.X;
            initialBallPosY = barCollision.Y - 5;
            // posizione iniziale barra

            // velocità barra
            barSpeed = 7;

            base.Initialize();

        }

        protected override void LoadContent()
        {
            spriteFont = Content.Load<SpriteFont>("File");
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            ballTexture = Content.Load<Texture2D>("ball_arkanoid_full");
            barTexture = Content.Load<Texture2D>("bar_arkanoid_full");


            spriteFont = Content.Load<SpriteFont>("File");

            barCollision = new Rectangle(_graphics.PreferredBackBufferWidth / 2, _graphics.PreferredBackBufferHeight - barTexture.Width, barTexture.Width, barTexture.Height);
            ballCollision = new Rectangle(barCollision.X, barCollision.Y - 5, ballTexture.Width, ballTexture.Height); // da cambiare quando si avrà la barra
            
        }

        protected override void Update(GameTime gameTime)
        {
            // commento per github

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            var keyboardState = Keyboard.GetState();


            if (ballCollision.X < _graphics.PreferredBackBufferWidth - ballCollision.Width && ballCollision.X + ballMovement.X > 0)
                ballCollision.X += (int)ballMovement.X;
            else
            {
                ballMovement.X = -ballMovement.X;
                ballCollision.X += (int)ballMovement.X;
            }
            if (ballCollision.Y < _graphics.PreferredBackBufferHeight - ballCollision.Height && ballCollision.Y + ballMovement.Y > 0)
                ballCollision.Y += (int)ballMovement.Y;
            else if (ballCollision.Y + ballMovement.Y <= 0)
            {
                ballMovement.Y = -ballMovement.Y;
                ballCollision.Y += (int)ballMovement.Y;
            }
            else
            {

                Restart();
            }
            if (barCollision.Intersects(ballCollision))
            {
                ballMovement.Y = -ballMovement.Y;
                ballCollision.Y += (int)ballMovement.Y;
            }

            // movimento a destra
            if (keyboardState.IsKeyDown(Keys.Right))
            {
                barCollision.X += barSpeed;
            }

            // movimento a sinistra
            if (keyboardState.IsKeyDown(Keys.Left))
            {
                barCollision.X -= barSpeed;
            }

            //bordi
            if (barCollision.X > _graphics.PreferredBackBufferWidth - 24)
            {
                barCollision.X = _graphics.PreferredBackBufferWidth - 24;
            }
            else if (barCollision.X < 0)
            {
                barCollision.X = 0;
            }


            //if(barCollision.X == 0 )

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            _spriteBatch.Begin();
            _spriteBatch.Draw(ballTexture, ballCollision, Color.White);
            _spriteBatch.DrawString(spriteFont, $"lives={lives} X={ballCollision.X} Y={ballCollision.Y}", new Vector2(0, 0), Color.White);
            _spriteBatch.Draw(barTexture, barCollision, Color.White);
            // _spriteBatch.DrawString(spriteFont, $"X={(int)barTexture.Width} Y={barTexture.Height},  XPOS = {barCollision.X} YPOS = {barCollision.Y}", new Vector2(0, 0), Color.White);
            _spriteBatch.End();

            // inserimento della barra

            //_spriteBatch.DrawString(spriteFont, $"X={(int)barTexture.Width} Y={barTexture.Height},  XPOS = {barCollision.X} YPOS = {barCollision.Y}", new Vector2(0, 0), Color.White);


            base.Draw(gameTime);
        }
    }
}