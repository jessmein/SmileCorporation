using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;

namespace SmileCorp
{

    class Dialogue
    {
        private Rectangle portraitDims;
        private Rectangle textDims;
        public Dictionary<string, string> currentConvo;
        Texture2D rec;
        public Dialogue(Texture2D rec)
        {
            this.rec = rec;
        }
        public new void Draw(GameTime gameTime, SpriteBatch sb)
        {
            // Option One (if you have integer size and coordinates)
            sb.Draw(rec, new Rectangle(10, 20, 80, 30),
                    Color.Chocolate);

            // Option Two (if you have floating-point coordinates)
            sb.Draw(rec, new Vector2(10f, 20f), null,
                    Color.Chocolate, 0f, Vector2.Zero, new Vector2(80f, 30f),
                    SpriteEffects.None, 0f);
        }
        // Function takes a file and returns a dictionary containing the entire conversation
        public Dictionary<string, string> OpenFile(string file)
        {
            StreamReader input = null;
            Dictionary<string, string> convo = new Dictionary<string, string>();

            try
            {
                input = new StreamReader(file);
                string line = null;

                while (input.ReadLine() != null)
                {
                    line = input.ReadLine();
                    string[] convoVars = line.Split('-');
                    convo.Add(convoVars[0], convoVars[1]);
                }
                return convo;
            }
            catch (System.Exception)
            {
                System.Diagnostics.Debug.WriteLine("Files made a whoopsy");
            }

            return null;
        }


    }
}