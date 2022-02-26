using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System;
using System.Text;
using System.IO;
using System.Diagnostics;

namespace SmileCorp
{
    enum GameStates
    {
        Title,
        Game,
        Pause,
        GameOver
    }

    public class Game1 : Game
    {
        #region fields
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private int windowWidth;
        private int windowHeight;

        KeyboardState kbState;
        KeyboardState prevKBState;

        //Fields for gamestates
        private GameStates currentState;
        private GameStates previousState;

        private Texture2D playerImg;
        private Texture2D testMap;
        private Player player;
        private Npc npc;
        private CollisionManager collisionManager;
        private Camera camera;

        //private List<Npc> npcs;

        #endregion

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            windowWidth = _graphics.PreferredBackBufferWidth = 1000;
            windowHeight = _graphics.PreferredBackBufferHeight = 1000;

            _graphics.ApplyChanges();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            playerImg = Content.Load<Texture2D>("angelicaSpriteSheet");
            testMap = Content.Load<Texture2D>("testMap");

            player = new Player(128, 128, new Vector2(0, 0), playerImg);
            npc = new Npc(128, 128, new Vector2(30, 30), playerImg, "TestNpc");
            collisionManager = new CollisionManager();
            camera = new Camera();

            currentState = GameStates.Game;
        }

        protected override void Update(GameTime gameTime)
        {

            // Game state loop
            switch (currentState)
            {
                //Title Screen
                case GameStates.Title:

                    previousState = currentState;

                    break;

                //Game Screen --> Where the game is played
                case GameStates.Game:
                    player.Update(gameTime);
                    npc.Update(gameTime);
                    camera.Follow(player, windowHeight, windowWidth);

                    kbState = Keyboard.GetState();

                    if (collisionManager.CheckCollision(player, npc, 7) && SingleKeyPress(Keys.E, kbState))
                    {
                        System.Diagnostics.Debug.WriteLine("Collision Detected");
                    }

                    prevKBState = kbState;
                    break;

                //Pause Screen
                case GameStates.Pause:
                    previousState = currentState;

                    break;

                //GameOver Screen
                case GameStates.GameOver:

                    break;
            }

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            _spriteBatch.Begin(transformMatrix: camera.Transform); //Starts drawing

            _spriteBatch.Draw(testMap, new Vector2(0, 0), Color.White);

            npc.Draw(_spriteBatch);
            player.Draw(_spriteBatch);

            _spriteBatch.End(); //Ends drawing

            base.Draw(gameTime);
        }

        //Resets the game when on the title screen
        private void GameReset()
        {
            //Resets the players position
        }

        // Checking for single input
        public bool SingleKeyPress(Keys key, KeyboardState kbState)
        {
            return kbState.IsKeyDown(key) && !prevKBState.IsKeyDown(key);
        }
    }
}