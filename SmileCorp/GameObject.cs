using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SmileCorp
{
    public class GameObject
    {
        int height;
        int width;
        Texture2D asset;
        protected Vector2 position;

        public int Height
        {
            get { return height; }
        }

        public int Width
        {
            get { return width; }
        }

        public Vector2 Position
        {
            get { return position; }
            set { this.position = value; }
        }

        public GameObject(int width, int height, Vector2 position, Texture2D asset)
        {
            this.height = height;
            this.width = width;
            this.position = position;
            this.asset = asset;
        }

        // Prevents object from walking outside of the border
        public void borderRestriction(int mapWidth, int mapHeight)
        {
            // Prevent it if the player tries to move outside of the window boundaries
            if (position.X < 0 || position.X > mapWidth)
            {
                position.X = Math.Clamp(position.X, 0, mapWidth);
            }
            if (position.Y < 0 || position.Y > mapHeight)
            {
                position.Y = Math.Clamp(position.Y, 0, mapHeight);
            }
        }

        // Draws the GameObject
        public void Draw(SpriteBatch sb)
        {
            sb.Draw(asset, position, Color.White);
        }

    }
}
