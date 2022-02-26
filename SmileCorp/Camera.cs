using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SmileCorp
{
    public class Camera
    {
        public Matrix Transform { get; private set; }

        public void Follow(GameObject target, int windowHeight, int windowWidth)
        {
            var position = Matrix.CreateTranslation(
                -target.Position.X - (target.Width / 2),
                -target.Position.Y - (target.Height / 2),
                0);

            var offset = Matrix.CreateTranslation(
                windowWidth / 2,
                windowHeight / 2,
                0);

            Transform = position * offset;
        }
    }
}
