using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

// Code by HattyDoge
namespace ArkanoidCopycat
{
    public class Game1 : Game
    {
        abstract class Collision
        {
            public abstract bool CollisionDetected(Rectangle collision);
        }
        class Cube : Collision
        {
            Rectangle cubeCollision;
            public Rectangle CubeCollision { get { return CubeCollision; } set { CubeCollision = value; } }
            public override bool CollisionDetected(Rectangle collision)
            {
                return cubeCollision.Intersects(collision);
            }
        }
        class PlayerBar : Collision
        {
            public int playerBarSpeed { get { return 4; } }
            Rectangle playerBarCollision;
            public Rectangle PlayerBarCollision { get { return playerBarCollision; } set { playerBarCollision = value; } }
            public PlayerBar() { }
            public PlayerBar(Rectangle rectangle) { playerBarCollision = rectangle; }
            public int X { get { return playerBarCollision.X; } set { playerBarCollision.X = value; } }
            public int Y { get { return playerBarCollision.Y; } set { playerBarCollision.Y = value; } }
            public override bool CollisionDetected(Rectangle collision)
            {
                return playerBarCollision.Intersects(collision);
            }
        }
        class Ball : Collision
        {
            public Vector2 ballMovement = new Vector2(2, 2);
            Rectangle ballCollision;
            public Rectangle BallCollision { get { return ballCollision; } set { ballCollision = value; } }
            public Ball() { }
            public Ball(Rectangle rectangle) { ballCollision = rectangle; }
            public int X { get { return ballCollision.X; } set { ballCollision.X = value; } }
            public int Y { get { return ballCollision.Y; } set { ballCollision.Y = value; } }
            public override bool CollisionDetected(Rectangle collision)
            {
                return ballCollision.Intersects(collision);
            }
            public void BallMovementApply()
            {
                ballCollision.X += (int)ballMovement.X;
                ballCollision.Y += (int)ballMovement.Y;
            }
        }

        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        int lives = 3;
        Texture2D playerBarTexture;
        Texture2D ballTexture;
        Texture2D backgroundTexture;
        Texture2D heartTextureAlive;
        Texture2D heartTextureDead;
        PlayerBar playerBar;
        bool restart = false;
        Ball ball;

        
        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }
        public void Restart()
        {
            ball.X = playerBar.X + playerBarTexture.Width / 2 - ballTexture.Width / 2;
            ball.Y = playerBar.Y - playerBarTexture.Height / 2 - ballTexture.Height / 2;
            restart = true;
            lives--;
        }
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            
            _graphics.PreferredBackBufferHeight = 569;
            _graphics.PreferredBackBufferWidth = 400;
            _graphics.ApplyChanges();


            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            ballTexture = Content.Load<Texture2D>("ball_arkanoid_full");
            playerBarTexture = Content.Load<Texture2D>("bar_arkanoid_full");
            backgroundTexture = Content.Load<Texture2D>("background");
            heartTextureDead = Content.Load<Texture2D>("hear_arkanoid_dead");
            heartTextureAlive = Content.Load<Texture2D>("hear_arkanoid_alive");

            playerBar = new PlayerBar(new Rectangle(_graphics.PreferredBackBufferWidth / 2, _graphics.PreferredBackBufferHeight / 6 * 5, playerBarTexture.Width, playerBarTexture.Height));
            ball = new Ball(new Rectangle(playerBar.X + playerBarTexture.Width / 2 - ballTexture.Width / 2, playerBar.Y - ballTexture.Height, ballTexture.Width, ballTexture.Height)); // da cambiare quando si avrà la barra

        }
        void CollisionDetection()
        {

            if (ball.X >= _graphics.PreferredBackBufferWidth - ball.BallCollision.Width || ball.X <= 0)
            {
                ball.ballMovement.X = -ball.ballMovement.X;
            }

            if (ball.Y + ball.ballMovement.Y <= 0)
            {
                ball.ballMovement.Y = -ball.ballMovement.Y;
            }
            else if (ball.Y + ball.ballMovement.Y >= _graphics.PreferredBackBufferHeight - ball.BallCollision.Height)
            {
                Restart();
            }

            if (ball.CollisionDetected(playerBar.PlayerBarCollision))
            {
                ball.ballMovement.Y = -ball.ballMovement.Y;
            }
        }
        void Movement(KeyboardState keyboardState)
        {
            CollisionDetection();
            // movimento a destra
            if (keyboardState.IsKeyDown(Keys.Right))
            {
                playerBar.X += playerBar.playerBarSpeed;
            }

            // movimento a sinistra
            if (keyboardState.IsKeyDown(Keys.Left))
            {
                playerBar.X -= playerBar.playerBarSpeed;
            }
            if (keyboardState.IsKeyDown(Keys.Space))
            {
                restart = false;
            }
            //bordi
            if (playerBar.X > _graphics.PreferredBackBufferWidth - 24)
            {
                playerBar.X = _graphics.PreferredBackBufferWidth - 24;
            }
            else if (playerBar.X < 0)
            {
                playerBar.X = 0;
            }
            if (restart)
            {
                ball.X = playerBar.X + playerBarTexture.Width / 2 - ballTexture.Width / 2;
                ball.Y = playerBar.Y - playerBarTexture.Height / 2 - ballTexture.Height / 2;
            }
            else
            {
                ball.BallMovementApply();
            }
        }
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            var keyboardState = Keyboard.GetState();
            // TODO: Add your update logic here
            Movement(keyboardState);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            _spriteBatch.Begin();
            _spriteBatch.Draw(backgroundTexture, new Vector2(0, 0), Color.White);
            _spriteBatch.Draw(ballTexture, ball.BallCollision, Color.White);
            _spriteBatch.Draw(playerBarTexture, playerBar.PlayerBarCollision, Color.White);
            
            // TODO: Add your drawing code here
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}

/*
    if (keyboardState.IsKeyDown(Keys.A))
{
    _graphics.PreferredBackBufferWidth += 2;
    _graphics.PreferredBackBufferHeight += 2;
    _graphics.ApplyChanges();
}
    if (keyboardState.IsKeyDown(Keys.D))
{
    _graphics.PreferredBackBufferWidth -= 2;
    _graphics.PreferredBackBufferHeight -= 2;
    _graphics.ApplyChanges();
}
*/