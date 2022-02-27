using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SmileCorp
{
    enum NPC_States
    {
        WalkLeft,
        WalkRight,
        WalkNorth,
        WalkSouth,
        Still
    }

    public class Npc : GameObject
    {
        #region fields

        string name;
        Texture2D spriteSheet;
        int speed;
        int frame;
        double timeCounter;
        double fps;
        double timePerFrame;

        NPC_States currState;

        public string Name
        {
            get { return name; }
            set { this.name = value; }
        }
        #endregion

        // Public NPC Constructor
        public Npc(int width, int height, Vector2 position, Texture2D texture, String name) : base(760, 300, position, texture)
        {
            this.spriteSheet = texture;
            this.name = name;
            this.position = position;

            currState = NPC_States.Still;
        }

        public void Update(GameTime gameTime)
        {
            UpdateAnimation(gameTime);
        }

        // Player Animation
        public void AnimateSprite(Rectangle sheetRect, SpriteEffects flipSprite, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(
                spriteSheet,
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

            switch (currState)
            {
                case NPC_States.Still:
                    AnimateSprite(new Rectangle(128, 0, Width, Height), SpriteEffects.None, sb);
                    break;
                case NPC_States.WalkNorth:
                    AnimateSprite(new Rectangle(frame * Width, 384, Width, Height), SpriteEffects.None, sb);
                    break;
                case NPC_States.WalkSouth:
                    AnimateSprite(new Rectangle(frame * Width, 0, Width, Height), SpriteEffects.None, sb);
                    break;
                case NPC_States.WalkRight:
                    AnimateSprite(new Rectangle(frame * Width, 256, Width, Height), SpriteEffects.None, sb);
                    break;
                case NPC_States.WalkLeft:
                    AnimateSprite(new Rectangle(frame * Width, 128, Width, Height), SpriteEffects.None, sb);
                    break;
                default:
                    break;
            }
        }
    }
}
