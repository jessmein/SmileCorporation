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

        public GameObject(int width, int height, Vector2 position)
        {
            this.height = height;
            this.width = width;
            this.position = position;
        }


    }
}
