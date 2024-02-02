using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

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
            public Rectangle CubeCollision { get { return cubeCollision; } set { cubeCollision = value; } }
            public override bool CollisionDetected(Rectangle collision)
            {
                return IsActive && cubeCollision.Intersects(collision);
            }
            public bool IsActive { get; set; }

            public Cube(Rectangle rectangle)
            {
                CubeCollision = rectangle;
                IsActive = true;
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
            public Vector2 ballMovement = new Vector2(5, 5);
            Rectangle ballCollision;
            public int numberDetection = 0;
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
        private Texture2D blockTexture;
        private Cube[,] blocks;


        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }
        public void Restart()
        {
            ball.X = playerBar.X + playerBarTexture.Width / 2 - ballTexture.Width / 2;
            ball.Y = playerBar.Y - ball.BallCollision.Height;
            restart = true;
            lives--;
        }
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            
            _graphics.PreferredBackBufferHeight = 512;
            _graphics.PreferredBackBufferWidth = 356;
            _graphics.ApplyChanges();


            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            blockTexture = Content.Load<Texture2D>("prova2");
            InitializeBlocks();

            base.LoadContent();
            // TODO: use this.Content to load your game content here
            ballTexture = Content.Load<Texture2D>("ball_arkanoid_full");
            playerBarTexture = Content.Load<Texture2D>("bar_arkanoid_full");
            backgroundTexture = Content.Load<Texture2D>("background");
            heartTextureDead = Content.Load<Texture2D>("hear_arkanoid_dead");
            heartTextureAlive = Content.Load<Texture2D>("hear_arkanoid_alive");
            
            playerBar = new PlayerBar(new Rectangle(_graphics.PreferredBackBufferWidth / 2 - playerBarTexture.Width / 2, _graphics.PreferredBackBufferHeight / 20 * 19, playerBarTexture.Width, playerBarTexture.Height));
            ball = new Ball(new Rectangle(playerBar.X + playerBarTexture.Width / 2 - ballTexture.Width / 2, playerBar.Y - ballTexture.Height, ballTexture.Width, ballTexture.Height)); // da cambiare quando si avrà la barra
        }
        void CollisionDetection()
        {
            #region palla contro i bordi
            //parte laterale
            if (ball.X >= _graphics.PreferredBackBufferWidth - ball.BallCollision.Width - 20 || ball.X <= 0 + 20)
            {
                ball.ballMovement.X = -ball.ballMovement.X;
            }
            //parte inferiore e superiore
            if (ball.Y + ball.ballMovement.Y <= 0 + 90)
            {
                ball.ballMovement.Y = -ball.ballMovement.Y;
            }
            else if (ball.Y + ball.ballMovement.Y >= _graphics.PreferredBackBufferHeight - ball.BallCollision.Height)
            {
                Restart();
            }
            #endregion

            if (ball.CollisionDetected(playerBar.PlayerBarCollision))
            {
                if (playerBar.X <= ball.X + ball.BallCollision.Width && ball.X < playerBar.X + playerBar.PlayerBarCollision.Width / 3 * 1)
                {
                    if (ball.ballMovement.X < 0)
                        ball.ballMovement.Y = -ball.ballMovement.Y;
                    else
                    {
                        ball.ballMovement.Y = -ball.ballMovement.Y;
                        ball.ballMovement.X = -ball.ballMovement.X;
                    }
                }
                else if (playerBar.X + playerBar.PlayerBarCollision.Width / 3 * 1 <= ball.X + ball.BallCollision.Width && ball.X <= playerBar.X + playerBar.PlayerBarCollision.Width / 3 * 2)
                    ball.ballMovement.Y = -ball.ballMovement.Y;
                else if (playerBar.X + playerBar.PlayerBarCollision.Width / 3 * 2 < ball.X + ball.BallCollision.Width && ball.X <= playerBar.X + playerBar.PlayerBarCollision.Width)
                {
                    if (ball.ballMovement.X > 0)
                        ball.ballMovement.Y = -ball.ballMovement.Y;
                    else
                    {
                        ball.ballMovement.Y = -ball.ballMovement.Y;
                        ball.ballMovement.X = -ball.ballMovement.X;
                    }
                }
            }
        }
        void InitializeBlocks()
        {
            int rows = 5;
            int columns = 13;
            int blockWidth = blockTexture.Width;
            int blockHeight = blockTexture.Height;
            blocks = new Cube[rows, columns];

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    int x = j * (blockWidth + 2); 
                    int y = i * (blockHeight + 2);
                    blocks[i, j] = new Cube(new Rectangle(x, y, blockWidth, blockHeight));
                }
            }
        }

        void DrawBlocks()
        {
            foreach (var block in blocks)
            {
                if (block.IsActive)
                {
                    _spriteBatch.Draw(blockTexture, block.CubeCollision, Color.White);
                }
            }
        }

        void HandleBlockCollision()
        {
            foreach (var block in blocks)
            {
                if (block.CollisionDetected(ball.BallCollision))
                {
                    block.IsActive = false;
                    //controlla se tocca un lato sinistro o destro
                    if(block.CubeCollision.Bottom < ball.Y && ball.Y < block.CubeCollision.Top)
                    { 
                    }
                }
            }
        }


        void Movement(KeyboardState keyboardState)
        {
            CollisionDetection();
            // movimento a destra
            if (keyboardState.IsKeyDown(Keys.Right))
            {
                playerBar.X += playerBar.playerBarSpeed;
                if (restart)
                    ball.ballMovement.X = 5;
            }

            // movimento a sinistra
            if (keyboardState.IsKeyDown(Keys.Left))
            {
                playerBar.X -= playerBar.playerBarSpeed;
                if (restart)
                    ball.ballMovement.X = -5;
            }
            if (keyboardState.IsKeyDown(Keys.Up))
            {
                restart = false;
                ball.ballMovement.Y = -5;
            }
            //bordi
            if (playerBar.X > _graphics.PreferredBackBufferWidth - playerBarTexture.Width - 20)
            {
                playerBar.X = _graphics.PreferredBackBufferWidth - playerBarTexture.Width;
            }
            else if (playerBar.X < 0 + 20)
            {
                playerBar.X = 0;
            }
            if (restart)
            {
                ball.X = playerBar.X + playerBarTexture.Width / 2 - ballTexture.Width / 2;
                ball.Y = playerBar.Y - ball.BallCollision.Height;
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
            HandleBlockCollision();

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
            _spriteBatch.Draw(backgroundTexture, new Vector2(0, 0), Color.White);
            _spriteBatch.Draw(ballTexture, ball.BallCollision, Color.White);
            _spriteBatch.Draw(playerBarTexture, playerBar.PlayerBarCollision, Color.White);
            DrawBlocks();
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