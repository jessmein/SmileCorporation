﻿using Microsoft.Xna.Framework;
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
        private CollisionManager collisionManager;
        private Camera camera;

        private Texture2D titleMenu;
        private Texture2D npcImg;
        private Texture2D security;

        // Testing Dialogue UI
        private string textText;

        Dialogue dialogue;

        // GameObjects
        private List<GameObject> objects;
        private Texture2D sofaLeft;
        private Texture2D sofaRight;

        //Buttons
        private List<Button> buttons = new List<Button>();
        private Texture2D startButton;
        private Texture2D creditsButton;
        private Texture2D backButton;

        private List<Npc> npcs;
        private SpriteFont font;
        private Texture2D smile;

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
            windowWidth = _graphics.PreferredBackBufferWidth = 960;
            windowHeight = _graphics.PreferredBackBufferHeight = 540;

            mapHeight = 2000;
            mapWidth = 1500;

            _graphics.ApplyChanges();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            playerImg = this.Content.Load<Texture2D>("angelicaSpriteSheet");
            deskImg = this.Content.Load<Texture2D>("ReceptionDesk");
            testMap = this.Content.Load<Texture2D>("ReceptionA");
            sofaLeft = this.Content.Load<Texture2D>("sofaLeft");
            sofaRight = this.Content.Load<Texture2D>("sofaRight");

            titleMenu = this.Content.Load<Texture2D>("SmileCorporation_Title");
            npcImg = this.Content.Load<Texture2D>("recpSpritesheet");
            security = this.Content.Load<Texture2D>("securitySpriteSheet");
            smile = this.Content.Load<Texture2D>("smileY");

            font = this.Content.Load<SpriteFont>("Credit");

            player = new Player(128, 128, new Vector2(700, 700), playerImg);
            prevPlayerPos = player.Position;
            tempCamTarget = new GameObject(128, 128, new Vector2(0, 0), null);
            collisionManager = new CollisionManager();
            camera = new Camera();

            objects = new List<GameObject>();
            objects.Add(new GameObject(188, 338, new Vector2(60, 1450), sofaLeft));
            objects.Add(new GameObject(188, 338, new Vector2(1250, 1450), sofaRight));
            objects.Add(new GameObject(380, 150, new Vector2(550, 1350), deskImg));

            startButton = Content.Load<Texture2D>("SmileCorporation_Title-play");
            creditsButton = Content.Load<Texture2D>("SmileCorporation_Title-credits");
            backButton = Content.Load<Texture2D>("SmileCorporation_Title-back");

            // Add buttons
            // Title
            buttons.Add(new Button(
                _graphics.GraphicsDevice,
                new Rectangle(windowWidth / 2 - 390, windowHeight / 2 - 35, 160, 75),
                startButton,
                startButton
                ));

            // credits
            buttons.Add(new Button(
                _graphics.GraphicsDevice,
                new Rectangle(windowWidth / 2 - 430, windowHeight / 2 + 40, 300, 65),
                creditsButton,
                creditsButton
                ));

            //Back 
            buttons.Add(new Button(
               _graphics.GraphicsDevice,
               new Rectangle(windowWidth / 2 - 400, windowHeight / 2 + 200, 300, 65),
               backButton,
               backButton
               ));

            // Assign methods to the buttons' event 
            buttons[0].OnLeftButtonClick += PlayButton;
            buttons[1].OnLeftButtonClick += CreditsButton;
            buttons[2].OnLeftButtonClick += BackButton;

            currentState = GameStates.Title;

            // add curr level npcs to the list
            npcs = new List<Npc>();
            npcs.Add(new Npc(128, 128, new Vector2(500, 1220), npcImg, "Receptionist"));
            npcs.Add(new Npc(128, 128, new Vector2(100, 100), security, "Guard"));

            //dialogue
            dialogue = new Dialogue(new Texture2D(GraphicsDevice, 100, 100));


        }
        
        protected override void Update(GameTime gameTime)
        {

            // Game state loop
            switch (currentState)
            {
                //Title Screen
                case GameStates.Title:

                    previousState = currentState;

                    //Updates the buttons
                    buttons[0].Update();
                    buttons[1].Update();


                    IsMouseVisible = true;

                    break;

                case GameStates.Credits:

                    //update buttons
                    buttons[2].Update();

                    break;

                //Game Screen --> Where the game is played
                case GameStates.Game:
                    player.Update(gameTime);
                    foreach(Npc n in npcs)
                    {
                        n.Update(gameTime);
                    }          
                    
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

                    foreach (Npc n in npcs)
                    {
                        CheckInteraction(n);
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
            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here
            _spriteBatch.Begin(); //Starts drawing

            switch(currentState)
            {
                case GameStates.Title:
                    _spriteBatch.Begin();
                    _spriteBatch.Draw(titleMenu, new Rectangle(0, 0, windowWidth, windowHeight), Color.White);
                    _spriteBatch.End();
                    break;

                case GameStates.Credits:
                    _spriteBatch.Begin();
                    _spriteBatch.DrawString(font, "Credits", new Vector2(windowWidth / 2, 10), Color.White);
                    _spriteBatch.DrawString(font, "Programmers: Anna Piccione, Gia Lopez, Jessica Niem, Karin Sannomiya", new Vector2(10, 70), Color.White);
                    _spriteBatch.DrawString(font, "Artists: Gia Lopez, Jessica Niem, Karin Sannomiya", new Vector2(10, 110), Color.White);
                    _spriteBatch.Draw(smile, new Rectangle(380, 175, smile.Width / 2, smile.Height / 2), Color.White);

                    buttons[2].Draw(_spriteBatch); // back

                    _spriteBatch.End();
                    break;

                case GameStates.Game:
                    _spriteBatch.Begin(transformMatrix: camera.Transform); //Starts drawing
                    _spriteBatch.Draw(testMap, new Vector2(0, 0), Color.White);

                    player.Draw(_spriteBatch);

                    foreach (GameObject obj in objects)
                    {
                        obj.Draw(_spriteBatch);
                    }

                    foreach (Npc n in npcs)
                    {
                        n.Draw(_spriteBatch);
                    }
                    _spriteBatch.End();

                    dialogue.Draw(gameTime, _spriteBatch);

                    break;
            }

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

        private void CheckInteraction(Npc npc)
        {
            if (collisionManager.CheckCollision(player, npc, 7) && kbState.IsKeyDown(Keys.E) && prevKBState.IsKeyUp(Keys.E))
            {

            }
        }

        // Checking for single input
        public bool SingleKeyPress(Keys key, KeyboardState kbState)
        {
            return kbState.IsKeyDown(key) && !prevKBState.IsKeyDown(key);
        }

        public void PlayButton ()
        {
            currentState = GameStates.Game;
        }

        public void CreditsButton ()
        {
            currentState = GameStates.Credits;
        }

        public void BackButton ()
        {
            currentState = GameStates.Title;
        }
    }
}