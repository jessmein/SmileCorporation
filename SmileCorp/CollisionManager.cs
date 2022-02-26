using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SmileCorp
{
    public class CollisionManager
    {

        // General AABB collision between two objects
        public bool CheckCollision(GameObject obj1, GameObject obj2, int offset)
        {
            Rectangle obj1Dims = new Rectangle((int)obj1.Position.X, (int)obj1.Position.Y, obj1.Width, obj1.Height);
            Rectangle obj2Dims = new Rectangle((int)obj2.Position.X, (int)obj2.Position.Y, obj2.Width, obj2.Height);

            if (obj1Dims.X > (obj2Dims.X) &&
                obj1Dims.X < (obj2Dims.X + obj2Dims.Width + offset) &&
                obj1Dims.Y > (obj2Dims.Y) &&
                obj1Dims.Y < (obj2Dims.Y + obj2Dims.Height + offset))
            {
                return true;
            }

            return false;
        }
    }
}
