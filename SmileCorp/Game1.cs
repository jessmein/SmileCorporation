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
        Credits,
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

        // Map variables 
        private Texture2D testMap;
        private int mapWidth;
        private int mapHeight;

        private Texture2D playerImg;
        private Texture2D deskImg;
        private Player player;
        private Vector2 prevPlayerPos;
        private GameObject tempCamTarget;
        private Npc npc;
        private CollisionManager collisionManager;
        private Camera camera;

        // Testing Dialogue UI
        private string textText;

        // GameObjects
        private List<GameObject> objects;
        private Texture2D sofaLeft;
        private Texture2D sofaRight;

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
            windowWidth = _graphics.PreferredBackBufferWidth = 800;
            windowHeight = _graphics.PreferredBackBufferHeight = 800;

            mapHeight = 2000;
            mapWidth = 1500;

            _graphics.ApplyChanges();
            base.Initialize();
        }
         
        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            playerImg = Content.Load<Texture2D>("angelicaSpriteSheet");
            deskImg = Content.Load<Texture2D>("ReceptionDesk");
            testMap = Content.Load<Texture2D>("ReceptionA");
            sofaLeft = Content.Load<Texture2D>("sofaLeft");
            sofaRight = Content.Load<Texture2D>("sofaRight");

            player = new Player(128, 128, new Vector2(700, 700), playerImg);
            prevPlayerPos = player.Position;
            tempCamTarget = new GameObject(128, 128, new Vector2(0, 0), null);
            collisionManager = new CollisionManager();
            camera = new Camera();

            objects = new List<GameObject>();
            objects.Add(new GameObject(188, 338, new Vector2(60, 1450), sofaLeft));
            objects.Add(new GameObject(188, 338, new Vector2(1250, 1450), sofaRight));
            objects.Add(new GameObject(380, 150, new Vector2(550, 1350), deskImg));

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
                    //npc.Update(gameTime);          
                    
                    kbState = Keyboard.GetState();

                    player.borderRestriction(mapWidth, mapHeight);
                    checkObjectCollision();

                    // checks to see if the player interacts with any NPCs
                    if (collisionManager.CheckCollision(player, objects[2], 50) && SingleKeyPress(Keys.E, kbState))
                    {
                        System.Diagnostics.Debug.WriteLine("Collision Detected");
                    }

                    // Checks to see if the player is nearing the edge of the map
                    if (CheckCameraBounds(player.Position))
                    {
                        camera.Follow(tempCamTarget, windowHeight, windowWidth);
                    }
                    else
                    {
                        camera.Follow(player, windowHeight, windowWidth);
                    }

                    prevKBState = kbState;
                    prevPlayerPos = player.Position;
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

            player.Draw(_spriteBatch);

            foreach(GameObject obj in objects)
            {
                obj.Draw(_spriteBatch);
            }

            _spriteBatch.End(); //Ends drawing

            base.Draw(gameTime);
        }

        //Resets the game when on the title screen
        private void GameReset()
        {
            //Resets the players position
            player.Position = new Vector2(700, 700);

        }

        private bool CheckCameraBounds(Vector2 playerPos)
        {
            bool isBorder = false;
            Vector2 newPos = playerPos;

            // If the player is at the x bottom of the map
            if (playerPos.X < windowWidth / 2)
            {
                newPos.X = windowWidth / 2;
                isBorder = true;
            }
            // If the player is at the x top of the map
            if (playerPos.X + player.Width > mapWidth - (windowWidth / 2))
            {
                newPos.X = mapWidth - (windowWidth / 2) - player.Width;
                isBorder = true;
            }
            // If the player is at the y bottom of the map
            if (playerPos.Y < windowHeight / 2)
            {
                newPos.Y = windowHeight / 2;
                isBorder = true;
            }
            // If the player is at the y top of the map
            if (playerPos.Y + player.Height > mapHeight - (windowHeight / 2))
            {
                newPos.Y = mapHeight - (windowHeight / 2) - player.Height;
                isBorder = true;
            }

            if (isBorder)
            {
                tempCamTarget.Position = newPos;
                return true;
            }
            return false;
        }

        // Function checks to see if player hits any static objects, preventing them from walking on 'nullspace'
        private void checkObjectCollision()
        {
            foreach(GameObject obj in objects)
            {
                if (collisionManager.CheckCollision(player, obj, 0))
                {
                    player.Position = prevPlayerPos;
                }
            } 
        }

        // Checking for single input
        public bool SingleKeyPress(Keys key, KeyboardState kbState)
        {
            return kbState.IsKeyDown(key) && !prevKBState.IsKeyDown(key);
        }
    }
}