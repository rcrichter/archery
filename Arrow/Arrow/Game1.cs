using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

using ContentLibrary;

namespace Archery
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        //Built-in XNA Stuff
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Matrix SpriteScale;
        float screenscale;
        static public int SCREEN_WIDTH = 800;
        static public int SCREEN_HEIGHT = 480;
        static public float SCREEN_ASPECT = (float)SCREEN_WIDTH / (float)SCREEN_HEIGHT;
        Vector2 baseScreenSize = new Vector2(SCREEN_WIDTH, SCREEN_HEIGHT);

        //Our Hero, and general info about him
        Player player;

        //Controls
        KeyboardState currentKeyboardState;
        KeyboardState previousKeyboardState;
        GamePadState currentGamePadState;
        GamePadState previousGamePadState;

        //Art assets
        Texture2D titleScreen;        
        Texture2D projectileTexture;
        Texture2D demonTexture;
        Texture2D birdManTexture;
        Texture2D ammoTexture;
        Texture2D background;
        SpriteFont gameFont;
        
        //Enemy housekeeping
        List<Enemy> Enemies;
        List<List<Enemy>> StageEnemies;
        List<Enemy> newEnemyList;
        
        //General game information
        double elapsedTime;
        enum GameState { TitleScreen, GameOver, Playing, BetweenLevels, WinGame };
        GameState gameState;        
        int gameStage;
        int numberOfStages;
        int enemyCount;
        public int Score;
        

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.IsFullScreen = false;
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            Enemies = new List<Enemy>();
            gameState = GameState.TitleScreen;
            numberOfStages = 2;
            gameStage = 0;
            StageEnemies = new List<List<Enemy>>();
            enemyCount = 0;

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            titleScreen = Content.Load<Texture2D>("archerytitle");
            background = Content.Load<Texture2D>("background");
            gameFont = Content.Load<SpriteFont>("GameFont");
            projectileTexture = Content.Load<Texture2D>("arrow");
            demonTexture = Content.Load<Texture2D>("demon");
            birdManTexture = Content.Load<Texture2D>("BirdMan");
            ammoTexture = Content.Load<Texture2D>("ammo");
            player = new Player(Content.Load<Texture2D>("archer_elf"), new Vector2(0, SCREEN_HEIGHT / 2 - Content.Load<Texture2D>("archer_elf").Height), 8.0f, projectileTexture, ammoTexture);

            for (int i = 0; i < numberOfStages; i++)
            {
                StageEnemies.Add(new List<Enemy>());
            }



        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // TODO: Add your update logic here
            currentGamePadState = GamePad.GetState(PlayerIndex.One);
            currentKeyboardState = Keyboard.GetState();

            if (currentGamePadState.Buttons.X == ButtonState.Pressed)
            {
                graphics.PreferredBackBufferHeight = 768;
                graphics.PreferredBackBufferWidth = 1024;
                graphics.ApplyChanges();
            }
            if (currentGamePadState.Buttons.B == ButtonState.Pressed)
            {
                graphics.PreferredBackBufferHeight = 768;
                graphics.PreferredBackBufferWidth = 1280;
                graphics.ApplyChanges();
            }
            if (currentGamePadState.Buttons.Y == ButtonState.Pressed)
            {
                graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
                graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
                graphics.IsFullScreen = true;
                graphics.ApplyChanges();
            }

            switch (gameState)
            {
                case GameState.TitleScreen:
                    if( currentGamePadState.Buttons.Start == ButtonState.Pressed && previousGamePadState.Buttons.Start == ButtonState.Released)
                    {
                        NextLevel();
                    }
                    break;
                case GameState.BetweenLevels:
                    elapsedTime += gameTime.ElapsedGameTime.TotalSeconds;

                    if (elapsedTime > 3)
                    {
                        gameState = GameState.Playing;
                        elapsedTime = 0;
                    }

                    break;
                case GameState.GameOver:
                    elapsedTime += gameTime.ElapsedGameTime.TotalSeconds;

                    if (elapsedTime > 4)
                    {
                        gameState = GameState.TitleScreen;
                        elapsedTime = 0;
                    }

                    break;
                case GameState.WinGame:
                    elapsedTime += gameTime.ElapsedGameTime.TotalSeconds;

                    if (elapsedTime > 4)
                    {
                        gameState = GameState.TitleScreen;
                        elapsedTime = 0;
                    }

                    break;
                case GameState.Playing:
                    player.Update(currentGamePadState, previousGamePadState, currentKeyboardState, previousKeyboardState, this);


                    if (!GetEnemies(gameTime))
                    {
                        if (Enemies.Count == 0)
                        {
                            NextLevel();
                        }
                    }

                    if (player.Ammo <= 0 && player.arrows.Count <= 0)
                    {
                        elapsedTime = 0;
                        GameOver();
                    }

                    newEnemyList = new List<Enemy>();

                    foreach (Enemy enemy in Enemies)
                    {
                        enemy.Update(this);
                        if (CheckEnemyArrowCollision(enemy.getPosition(), enemy.getTexture()))
                        {
                            enemy.Die = true;
                        }
                        if (enemy.getActive())
                        {
                            newEnemyList.Add(enemy);
                        }

                    }

                    Enemies = newEnemyList;

                    break;
            }

            previousGamePadState = currentGamePadState;
            previousKeyboardState = currentKeyboardState;
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Viewport = new Viewport(0, 0, GraphicsDevice.PresentationParameters.BackBufferWidth, GraphicsDevice.PresentationParameters.BackBufferHeight);
            //GraphicsDevice.Clear(Color.Black);

            // figure out the largest area that fits in this resolution at the desired aspect ratio     
            int width = GraphicsDevice.PresentationParameters.BackBufferWidth;
            int height = (int)(width / SCREEN_ASPECT + .5f);
            if (height > GraphicsDevice.PresentationParameters.BackBufferHeight)
            {
                height = GraphicsDevice.PresentationParameters.BackBufferHeight;
                width = (int)(height * SCREEN_ASPECT + .5f);
            }
            GraphicsDevice.Viewport = new Viewport
            {
                X = GraphicsDevice.PresentationParameters.BackBufferWidth / 2 - width / 2,
                Y = GraphicsDevice.PresentationParameters.BackBufferHeight / 2 - height / 2,
                Width = width,
                Height = height,
                MinDepth = 0,
                MaxDepth = 1
            };

            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here
            screenscale = (float)graphics.GraphicsDevice.Viewport.TitleSafeArea.Width / (float)SCREEN_WIDTH;
            SpriteScale = Matrix.CreateScale(screenscale, screenscale, 1);
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, SpriteScale);
            //spriteBatch.Begin();
            switch (gameState)
            {
                case GameState.Playing:
                    spriteBatch.Draw(background, new Vector2(0, 0), Color.White);
                    player.Draw(spriteBatch);
                    DrawScore(spriteBatch, gameFont);
                    foreach (Enemy enemy in Enemies)
                    {
                        enemy.Draw(spriteBatch);
                    }
                    break;
                case GameState.TitleScreen:
                    spriteBatch.Draw(titleScreen, new Vector2(0, 0), Color.White);
                    break;
                case GameState.BetweenLevels:
                    DrawBetweenLevelScreen();
                    break;
                case GameState.GameOver:
                    DrawGameOverScreen();
                    break;
                case GameState.WinGame:
                    DrawWinGameScreen();
                    break;
            }
            spriteBatch.End();
            base.Draw(gameTime);
        }

        private void NextLevel()
        {
            enemyCount = 0;
            elapsedTime = 0;
            Enemies = new List<Enemy>();
            player.ClearArrow();
            

            if (gameStage < numberOfStages)
            {
                gameStage++;
                gameState = GameState.BetweenLevels;
                LoadLevel(gameStage - 1, Content.Load<List<EnemyInfo>>("Levels/level" + gameStage));
            }
            else
            {
                WinGame();
            }

            //Determine ammo for the next stage
            switch (gameStage)
            {
                case 1:
                    player.Ammo = 5;
                    Score = 0;
                    break;
                case 2:
                    player.Ammo += 23;
                    break;
            }
        }

        private void GameOver()
        {
            if (gameState != GameState.WinGame)
            {
                gameState = GameState.GameOver;
            }
            enemyCount = 0;
            gameStage = 0;
            for (int i = 0; i < numberOfStages; i++)
            {
                StageEnemies[i] = new List<Enemy>();
            }
        }

        private void WinGame()
        {
            gameState = GameState.WinGame;
            GameOver();
        }

        private bool GetEnemies(GameTime gameTime)
        {
            elapsedTime += gameTime.ElapsedGameTime.TotalSeconds;

            foreach (Enemy enemy in StageEnemies[gameStage - 1])
            {
                if (elapsedTime >= enemy.SpawnTime && enemy.Spawned == false)
                {
                    Enemies.Add(enemy);
                    enemy.Spawned = true;
                    enemyCount++;
                }
            }

            if (StageEnemies[gameStage - 1].Count <= enemyCount)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private bool CheckEnemyArrowCollision(Vector2 enemyPosition, Texture2D enemyTexture)
        {
            Rectangle rectangle1 = new Rectangle((int)(enemyPosition.X + 0.5f), (int)(enemyPosition.Y + 0.5f), enemyTexture.Width, enemyTexture.Height);
            Rectangle rectangle2;

            foreach (Missile arrow in player.arrows)
            {
                rectangle2 = new Rectangle((int)(arrow.getPosition().X + 0.5f), (int)(arrow.getPosition().Y + 0.5f), arrow.getTexture().Width, arrow.getTexture().Height);

                if (rectangle1.Intersects(rectangle2))
                {
                    return true;
                }
            }

            return false;
        }

        private void LoadLevel(int levelNumber, List<EnemyInfo> enemies)
        {
            foreach (EnemyInfo enemyInfo in enemies)
            {
                switch (enemyInfo.EnemyType)
                {
                    case "Demon":
                        Demon demon = new Demon(demonTexture, new Vector2(enemyInfo.XStart, enemyInfo.YStart), enemyInfo.Speed, enemyInfo.StartTime);
                        StageEnemies[levelNumber].Add(demon);
                        break;
                    case "BirdMan":
                        BirdMan birdMan = new BirdMan(birdManTexture, new Vector2(enemyInfo.XStart, enemyInfo.YStart), enemyInfo.Speed, enemyInfo.StartTime);
                        StageEnemies[levelNumber].Add(birdMan);
                        break;
                }
            }
        }

        private void DrawBetweenLevelScreen()
        {
            spriteBatch.DrawString(gameFont, "Get ready to archery on level " + gameStage.ToString(), new Vector2(220, 200), Color.White);
        }

        private void DrawGameOverScreen()
        {
            spriteBatch.DrawString(gameFont, "Sorry, you can't archery without arrows. Game Over. :(", new Vector2(120, 200), Color.White);
            spriteBatch.DrawString(gameFont, "Final Score: " + Score.ToString(), new Vector2(320, 250), Color.White);
        }

        public void DrawScore(SpriteBatch spriteBatch, SpriteFont gameFont)
        {
            spriteBatch.DrawString(gameFont, Score.ToString(), new Vector2(750, 20), Color.White);
        }

        private void DrawWinGameScreen()
        {
            spriteBatch.DrawString(gameFont, "You won the game! You are the greatest archery! \\o/", new Vector2(120, 200), Color.White);
            spriteBatch.DrawString(gameFont, "Final Score: " + Score.ToString(), new Vector2(320, 250), Color.White);
        }
    }
}
