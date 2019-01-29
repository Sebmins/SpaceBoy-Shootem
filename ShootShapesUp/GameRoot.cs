using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using ShootShapesUp.MainMenu;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ShootShapesUp
{
    public class GameRoot : Game
    {
        // some helpful static properties
        public static GameRoot Instance { get; private set; }
        public static Viewport Viewport { get { return Instance.GraphicsDevice.Viewport; } }
        public static Vector2 ScreenSize { get { return new Vector2(Viewport.Width, Viewport.Height); } }
        public static GameTime GameTime { get; private set; }

        public static Texture2D Player { get; private set; }
        public static Texture2D Seeker { get; private set; }
        public static Texture2D Bullet { get; private set; }
        public static Texture2D Pointer { get; private set; }
        public static Texture2D Ammo { get; private set; }
        public static Texture2D Speed { get; private set; }
        public static Texture2D Boss { get; private set; }

        public static SpriteFont Font { get; private set; }
        public static SpriteFont Font2 { get; private set; }
        public static SpriteFont Font3 { get; private set; }

        public static Song Music { get; private set; }

        private static readonly Random rand = new Random();

        private static SoundEffect[] explosions;
        // return a random explosion sound
        public static SoundEffect Explosion { get { return explosions[rand.Next(explosions.Length)]; } }

        private static SoundEffect[] shots;
        public static SoundEffect Shot { get { return shots[rand.Next(shots.Length)]; } }

        private static SoundEffect[] spawns;
        public static SoundEffect Spawn { get { return spawns[rand.Next(spawns.Length)]; } }

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        enum GameStates
        {
            MainMenu,
            Level1,
            Level2,
            Level3,
            RestartScreen,
            EndScreen,

        }
        private Color _backgroundColour = Color.CornflowerBlue;
        private List<Component> _gameComponents;

        GameStates gameState;
        public static bool state23;
        public static bool state3;
        KeyboardState kbd;
        Color background;
        String gameStateTitle;
        Rectangle screen;
        Texture2D screenTexture;
        GameTime deltaTime;
        GameStates currentState;
        Component restartBtn;

        public double timePassed;
        int timePassed2;
        public int windowWidth = 1200;
        public int windowHeight = 600;

        public GameRoot()
        {
            Instance = this;
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = @"Content";

            graphics.PreferredBackBufferWidth = windowWidth;
            graphics.PreferredBackBufferHeight = windowHeight;
        }

        protected override void Initialize()
        {
            IsMouseVisible = true;

            

            screen = new Rectangle(50, 50, 700, 380);
            gameState = GameStates.MainMenu;
            gameStateTitle = "State 1";
            background = Color.CornflowerBlue;

            base.Initialize();

            EntityManager.Add(PlayerShip.Instance);

//            MediaPlayer.IsRepeating = true;
//            MediaPlayer.Play(GameRoot.Music);
        }

        protected override void LoadContent()
        {;

            spriteBatch = new SpriteBatch(GraphicsDevice);
            Player = Content.Load<Texture2D>("Art2/player");
            Seeker = Content.Load<Texture2D>("Art2/seeker");
            Bullet = Content.Load<Texture2D>("Art2/Bullet");
            Pointer = Content.Load<Texture2D>("Art/Pointer");
            Ammo = Content.Load<Texture2D>("Art2/Ammo");
            Speed = Content.Load<Texture2D>("Art2/Speed");
            Boss = Content.Load<Texture2D>("Art2/Boss");
            Font = Content.Load<SpriteFont>("Font");
            Font2 = Content.Load<SpriteFont>("Font2");
            Font3 = Content.Load<SpriteFont>("Font3");

            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            var startButton = new Button(Content.Load<Texture2D>("Button/Button"), Content.Load<SpriteFont>("Font"))
            {
                Position = new Vector2(windowWidth/2 - 50, windowHeight/2 -50),
                Text = "Start",
            };

            startButton.Click += StartButton_Click;



            var quitButton = new Button(Content.Load<Texture2D>("Button/Button"), Content.Load<SpriteFont>("Font"))
            {
                Position = new Vector2(windowWidth / 2 - 50, windowHeight / 2 + 50),
                Text = "Quit",
            };

            quitButton.Click += QuitButton_Click;

            var restartButton = new Button(Content.Load<Texture2D>("Button/Button"), Content.Load<SpriteFont>("Font"))
            {
                Position = new Vector2(windowWidth / 2 - 50, windowHeight / 2),
                Text = "Restart",
            };

            restartButton.Click += RestartButton_Click;

            _gameComponents = new List<Component>()
            {
            startButton,
            quitButton,
            };

            restartBtn = restartButton;

            // Music = Content.Load<Song>("Sound/Music");

            // These linq expressions are just a fancy way loading all sounds of each category into an array.
            explosions = Enumerable.Range(1, 8).Select(x => Content.Load<SoundEffect>("Sound/explosion-0" + x)).ToArray();
            shots = Enumerable.Range(1, 3).Select(x => Content.Load<SoundEffect>("Sound/shoot-0" + x)).ToArray();
            spawns = Enumerable.Range(1, 8).Select(x => Content.Load<SoundEffect>("Sound/spawn-0" + x)).ToArray();
        }

        private void QuitButton_Click(object sender, System.EventArgs e)
        {
            Exit();
        }

        private void StartButton_Click(object sender, System.EventArgs e)
        {

            var random = new Random();

            gameState = GameStates.Level1;
            background = new Color(0, 0, 0, 0);
            gameStateTitle = "State 2";

           // _backgroundColour = new Color(random.Next(0, 255), random.Next(0, 255), random.Next(0, 255));

        }

        private void RestartButton_Click(object sender, System.EventArgs e)
        {
            
            gameState = currentState;
        }
        protected override void Update(GameTime gameTime)
        {

            GameTime = gameTime;
            
            timePassed = gameTime.ElapsedGameTime.TotalSeconds;

            //Console.WriteLine(timePassed);
            Input.Update();

            // Allows the game to exit
            if (Input.WasButtonPressed(Buttons.Back) || Input.WasKeyPressed(Keys.Escape))
                this.Exit();
                        

            base.Update(gameTime);         

            switch (gameState)
            {
                case GameStates.MainMenu:
                    UpdateMainMenu(deltaTime);
                    break;
                case GameStates.Level1:
                    UpdateLevel1(deltaTime);
                    break;
                case GameStates.RestartScreen:
                    UpdateRestartScreen(deltaTime);
                    break;
                case GameStates.Level2:
                    UpdateLevel2(deltaTime);
                    break;
                case GameStates.Level3:
                    UpdateLevel3(deltaTime);
                    break;
                case GameStates.EndScreen:
                    UpdateEndScreen(deltaTime);
                    break;
            }
        }

        void UpdateMainMenu(GameTime deltaTime)
        {

            IsMouseVisible = true;

            foreach (var component in _gameComponents)
                component.Update(deltaTime);

            kbd = Keyboard.GetState();

            if (kbd.IsKeyDown(Keys.Enter))
            {
                gameState = GameStates.Level3;
                background = Color.Pink;
            }


        }
        public static void speedBooost(int timepassed)
        {
            while(timepassed<timepassed+500)
            PlayerShip.speed = 10;
        }
        void UpdateLevel1(GameTime deltaTime)
        {
            currentState = GameStates.Level1;
            state3 = false;
            state23 = false;
            
            timePassed2 = timePassed2 + 1;
       


            if (PlayerShip.Instance.IsDead == true && timePassed2 > 200)
            {
                reset();
                gameState = GameStates.RestartScreen;
            }

            IsMouseVisible = false;

            EntityManager.Update();
            EnemySpawner.Update();

            if(Enemy.killCount == 0)
            {
                reset();

                gameState = GameStates.Level2;
            }
            
        }

        void UpdateLevel2(GameTime deltaTime)
        {
            currentState = GameStates.Level2;
            state23 = true;
            state3 = false;
            timePassed2 = timePassed2 + 1;
            if (timePassed2 > 75)
            {
                if (PlayerShip.Instance.IsDead == true && timePassed2 > 200)
                {
                    reset();
                    gameState = GameStates.RestartScreen;
                }

                IsMouseVisible = false;

                EntityManager.Update();
                EnemySpawner.Update();
            }

            if (Enemy.killCount == 0)
            {
                reset();

                gameState = GameStates.Level3;
            }
        }

        void UpdateLevel3(GameTime deltaTime)
        {
            currentState = GameStates.Level3;
            state23 = true;
            state3 = true;

            timePassed2 = timePassed2 + 1;
            if (timePassed2 > 75)
            {
                if (PlayerShip.Instance.IsDead == true && timePassed2 > 200)
                {
                    reset();
                    gameState = GameStates.RestartScreen;
                }

                IsMouseVisible = false;

                EntityManager.Update();
                EnemySpawner.Update();
            }

            if (Bosses.hpCount == 0)
                gameState = GameStates.EndScreen;

        }

        void reset()
        {
            timePassed2 = 0;
            Enemy.killCount = 50;
            PlayerShip.ammoCounter = 100;
            EnemySpawner.Count = 0;
            Bosses.hpCount = 100;
            EntityManager.clear();
            EntityManager.removeDecals();

        }
        void UpdateRestartScreen(GameTime deltaTime)
        {
            restartBtn.Update(deltaTime);

            reset();
        }

        void UpdateEndScreen(GameTime deltaTime)
        { }

        protected override void Draw(GameTime gameTime)
        {

            GraphicsDevice.Clear(_backgroundColour);
            GraphicsDevice.Clear(background);
        
            // Draw mouse cursor
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive);
            spriteBatch.End();            

            base.Draw(gameTime);

            switch (gameState)
            {
            case GameStates.MainMenu:
                DrawMainMenu(deltaTime);
                break;
            case GameStates.Level1:
                DrawLevel1(deltaTime);
                break;
            case GameStates.Level2:
                DrawLevel2(deltaTime);
                break;
            case GameStates.RestartScreen:
                DrawRestartScreen(deltaTime);
                break;
            case GameStates.Level3:
                DrawLevel3(deltaTime);
                break;
            case GameStates.EndScreen:
                DrawEndScreen(deltaTime);
                break;
            }
        }
        
        void DrawMainMenu(GameTime deltaTime)
        {
            spriteBatch.Begin();
            spriteBatch.DrawString(Font3, "Space Boy Shoot'em", new Vector2(windowWidth/2-200, 50), Color.White);
            spriteBatch.DrawString(Font, "Main Menu", Vector2.UnitY, Color.White);
            foreach (var component in _gameComponents)
                component.Draw(deltaTime, spriteBatch);
            
            spriteBatch.End(); 

            //Draws buttons

        }
        void DrawLevel1(GameTime deltaTime)
        {
            if (timePassed2 > 75)
            {
                // Draw entities. Sort by texture for better batching.dd
                spriteBatch.Begin(SpriteSortMode.Texture, BlendState.Additive);
                EntityManager.Draw(spriteBatch);
                // Draw Cursor
                spriteBatch.DrawString(Font, "Level 1", Vector2.Zero, Color.White);
                spriteBatch.DrawString(Font2, "Ammo: " + PlayerShip.ammoCounter.ToString(), new Vector2(windowWidth - windowWidth / 6 - 50, windowHeight - windowHeight / 15), Color.White);
                spriteBatch.DrawString(Font2, Enemy.killCount.ToString(), new Vector2(windowWidth / 2, 50), Color.White);
                spriteBatch.Draw(GameRoot.Pointer, Input.MousePosition, Color.White);

                spriteBatch.End();
            }
            else
            {
                spriteBatch.Begin();
                spriteBatch.DrawString(Font3, "Level 1", new Vector2(windowWidth / 2 - 100, windowHeight / 2), Color.White);
                spriteBatch.End();
            }
        }

        void DrawLevel2(GameTime deltaTime)
        {

            timePassed2 = timePassed2 + 1;
            if (timePassed2 > 75)
            {
                // Draw entities. Sort by texture for better batching.dd
                spriteBatch.Begin(SpriteSortMode.Texture, BlendState.Additive);
                EntityManager.Draw(spriteBatch);
                // Draw Cursor
                spriteBatch.DrawString(Font, "Level 2", Vector2.Zero, Color.White);
                spriteBatch.DrawString(Font2, "Ammo: " + PlayerShip.ammoCounter.ToString(), new Vector2(windowWidth - windowWidth / 6 - 50, windowHeight - windowHeight / 15), Color.White);
                spriteBatch.DrawString(Font2, Enemy.killCount.ToString(), new Vector2(windowWidth / 2, 50), Color.White);
                spriteBatch.Draw(GameRoot.Pointer, Input.MousePosition, Color.White);
                //spriteBatch.Draw(Wanderer, new Vector2(100, 100), Color.White);
                spriteBatch.End();
            }
            else
            {
                spriteBatch.Begin();
                spriteBatch.DrawString(Font3, "Level 2", new Vector2(windowWidth / 2 - 100, windowHeight / 2), Color.White);
                spriteBatch.End();
            }
            
        }

        void DrawLevel3(GameTime deltaTime)
        {

            timePassed2 = timePassed2 + 1;
            if (timePassed2 > 75)
            {
                // Draw entities. Sort by texture for better batching.dd
                spriteBatch.Begin(SpriteSortMode.Texture, BlendState.Additive);
                EntityManager.Draw(spriteBatch);
                // Draw Cursor
                spriteBatch.DrawString(Font, "Level 3", Vector2.Zero, Color.White);
                spriteBatch.DrawString(Font2, "Ammo: " + PlayerShip.ammoCounter.ToString(), new Vector2(windowWidth - windowWidth / 6 - 50, windowHeight - windowHeight / 15), Color.White);
                spriteBatch.DrawString(Font2, "Boss HP = " + Bosses.hpCount.ToString(), new Vector2(windowWidth / 2 -75, 50), Color.White);
                spriteBatch.Draw(GameRoot.Pointer, Input.MousePosition, Color.White);
                //spriteBatch.Draw(Wanderer, new Vector2(100, 100), Color.White);
                spriteBatch.End();
            }
            else
            {
                spriteBatch.Begin();
                spriteBatch.DrawString(Font3, "Level 3", new Vector2(windowWidth / 2 - 100, windowHeight / 2), Color.White);
                spriteBatch.End();
            }

        }

        void DrawRestartScreen(GameTime deltaTime)
        {
            IsMouseVisible = true;
            // Draw entities. Sort by texture for better batching.
            spriteBatch.Begin();
            restartBtn.Draw(deltaTime, spriteBatch);

            // Draw Cursor
            spriteBatch.DrawString(Font, "RestartScreen", Vector2.Zero, Color.White);
            spriteBatch.DrawString(Font2, "game over", new Vector2(windowWidth / 2 - 50, 50), Color.White);
            spriteBatch.End();
        }

        void DrawEndScreen(GameTime deltaTime)
        {
            IsMouseVisible = true;
            // Draw entities. Sort by texture for better batching.
            spriteBatch.Begin();

            // Draw Cursor
            spriteBatch.DrawString(Font, "EndScreen", Vector2.Zero, Color.White);
            spriteBatch.DrawString(Font2, "GG WP", new Vector2(windowWidth / 2 - 50, 50), Color.White);
            spriteBatch.DrawString(Font2, "Thanks for playing", new Vector2(windowWidth / 2 - 100, 150), Color.White);
            spriteBatch.End();
        }

        private void DrawRightAlignedString(string text, float y)
        {
            var textWidth = GameRoot.Font.MeasureString(text).X;
            spriteBatch.DrawString(GameRoot.Font, text, new Vector2(ScreenSize.X - textWidth - 5, y), Color.White);
        }
    }
}
