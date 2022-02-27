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
        public Npc(int width, int height, Vector2 position, Texture2D texture, String name) : base(760, 300, position, texture)
        {
            this.spriteSheet = texture;
            this.name = name;
            this.position = position;
        }

        public void Update(GameTime gameTime)
        {
        }

        public new void Draw(SpriteBatch sb)
        {
            sb.Draw(spriteSheet, 
                new Vector2(position.X, position.Y), 
                null, 
                Color.White, 
                0, 
                Vector2.Zero, 
                1.0f, 
                SpriteEffects.None, 
                0f);
        }
    }
}
