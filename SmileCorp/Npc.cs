using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SmileCorp
{
    public class Npc : GameObject
    {
        #region fields

        string name;
        Texture2D spriteSheet;

        public string Name
        {
            get { return name; }
            set { this.name = value; }
        }
        #endregion

        // Public NPC Constructor
        public Npc(int width, int height, Vector2 position, Texture2D texture, String name) : base(width, height, position)
        {
            this.spriteSheet = texture;
            this.name = name;
            this.position = position;
        }

        public void Update(GameTime gameTime)
        {

        }

        // Draws the NPC
        public void Draw(SpriteBatch sb)
        {
            sb.Draw(spriteSheet, position, Color.White);
        }

    }
}
