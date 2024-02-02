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
        private Block[,] blocks;

        class Block : Collision
        {
            Rectangle blockCollision;
            public Rectangle BlockCollision { get { return blockCollision; } set { blockCollision = value; } }
            public bool IsActive { get; set; }

            public Block(Rectangle rectangle)
            {
                blockCollision = rectangle;
                IsActive = true;
            }

            public override bool CollisionDetected(Rectangle collision)
            {
                return IsActive && blockCollision.Intersects(collision);
            }
        }
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
            #endregion

            if (ball.CollisionDetected(playerBar.PlayerBarCollision) && ball.numberDetection == 0)
            {
                if (playerBar.X <= ball.X + ball.BallCollision.Width && ball.X < playerBar.X + playerBar.PlayerBarCollision.Width / 3 * 1)
                {
                    ball.ballMovement.Y = -ball.ballMovement.Y;
                    ball.ballMovement.X = -ball.ballMovement.X;
                }
                else if (playerBar.X + playerBar.PlayerBarCollision.Width / 3 * 1 <= ball.X + ball.BallCollision.Width && ball.X <= playerBar.X + playerBar.PlayerBarCollision.Width / 3 * 2)
                    ball.ballMovement.Y = -ball.ballMovement.Y;
                else if (playerBar.X + playerBar.PlayerBarCollision.Width / 3 * 2 < ball.X + ball.BallCollision.Width && ball.X <= playerBar.X + playerBar.PlayerBarCollision.Width)
                { 
                    ball.ballMovement.Y = -ball.ballMovement.Y;
                    ball.ballMovement.X = - ball.ballMovement.X;
                }
                ball.numberDetection++;
            }
            else
            {
                ball.numberDetection = 0;
            }
        }
        void InitializeBlocks()
        {
            int rows = 5;
            int columns = 13;
            int blockWidth = blockTexture.Width;
            int blockHeight = blockTexture.Height;
            blocks = new Block[rows, columns];

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    int x = j * (blockWidth + 2); 
                    int y = i * (blockHeight + 2);
                    blocks[i, j] = new Block(new Rectangle(x, y, blockWidth, blockHeight));
                }
            }
        }

        void DrawBlocks()
        {
            foreach (var block in blocks)
            {
                if (block.IsActive)
                {
                    _spriteBatch.Draw(blockTexture, block.BlockCollision, Color.White);
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
                    ball.ballMovement.Y = -ball.ballMovement.Y; // Inverti la direzione della pallina
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
            }

            // movimento a sinistra
            if (keyboardState.IsKeyDown(Keys.Left))
            {
                playerBar.X -= playerBar.playerBarSpeed;
            }
            if (keyboardState.IsKeyDown(Keys.Up))
            {
                restart = false;
            }
            //bordi
            if (playerBar.X > _graphics.PreferredBackBufferWidth - playerBarTexture.Width)
            {
                playerBar.X = _graphics.PreferredBackBufferWidth - playerBarTexture.Width;
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