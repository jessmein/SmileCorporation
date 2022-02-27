using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace SmileCorp
{
    enum PlayerState
    {
        WalkLeft,
        WalkRight,
        WalkNorth,
        WalkSouth,
        Still
    }

    public class Player : GameObject
    {
        #region fields

        // Player animation
        Texture2D playerSheet;
        int speed;
        int frame;
        double timeCounter;
        double fps;
        double timePerFrame;
        int health;

        // Player Input
        KeyboardState prevKeyState;
        PlayerState currentState;

        #endregion

        // Player constructor
        public Player(int width, int height, Vector2 position, Texture2D texture) : base(width, height, position, texture)
        {
            this.speed = 5;
            this.health = 3;
            playerSheet = texture;

            fps = 3.0;
            timePerFrame = 1.0 / fps;
        }

        // Updating Player
        public void Update(GameTime gameTime)
        {
            KeyboardState newKeyState = Keyboard.GetState();
            PlayerInput(newKeyState);
            prevKeyState = newKeyState;
            UpdateAnimation(gameTime);
        }

        // Player Animation
        public void AnimateSprite(Rectangle sheetRect, SpriteEffects flipSprite, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(
                playerSheet,
                new Vector2(Position.X, Position.Y),
                sheetRect,
                Color.White,
                0,              //Rotation
                Vector2.Zero,   //Origin inside the image (top left)
                1.0f,           //Scale
                flipSprite,
                0);             //Layer Depth
        }

        // Updating Player Animation
        public void UpdateAnimation(GameTime gameTime)
        {
            timeCounter += gameTime.ElapsedGameTime.TotalSeconds;

            if (timeCounter >= timePerFrame)
            {
                frame += 1;

                if (frame > 1)
                {
                    frame = 0;
                }

                timeCounter -= timePerFrame;
            }
        }

        // Drawing the player
        public new void Draw(SpriteBatch sb)
        {

            switch (currentState)
            {
                case PlayerState.Still:
                    AnimateSprite(new Rectangle(128, 0, Width, Height), SpriteEffects.None, sb);
                    break;
                case PlayerState.WalkNorth:
                    AnimateSprite(new Rectangle(frame * Width, 384, Width, Height), SpriteEffects.None, sb);
                    break;
                case PlayerState.WalkSouth:
                    AnimateSprite(new Rectangle(frame * Width, 0, Width, Height), SpriteEffects.None, sb);
                    break;
                case PlayerState.WalkRight:
                    AnimateSprite(new Rectangle(frame * Width, 256, Width, Height), SpriteEffects.None, sb);
                    break;
                case PlayerState.WalkLeft:
                    AnimateSprite(new Rectangle(frame * Width, 128, Width, Height), SpriteEffects.None, sb);
                    break;
                default:
                    break;
            }
        }

        // Taking player input
        public void PlayerInput(KeyboardState kbState)
        {
            // Sets default state to player still
            currentState = PlayerState.Still;

            // Takes player inpput based on WASD
            if (kbState.IsKeyDown(Keys.W))
            {
                position.Y -= speed;
                currentState = PlayerState.WalkNorth;
            }
            if (kbState.IsKeyDown(Keys.A))
            {
                position.X -= speed;
                currentState = PlayerState.WalkLeft;
            }
            if (kbState.IsKeyDown(Keys.S))
            {
                position.Y += speed;
                currentState = PlayerState.WalkSouth;
            }
            if (kbState.IsKeyDown(Keys.D))
            {
                position.X += speed;
                currentState = PlayerState.WalkRight;
            }
        }

    }
}
