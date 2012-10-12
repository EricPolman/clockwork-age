using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Clockwork_Age
{
    class Node
    {
        protected Vector3 position;

        public virtual void update(float deltaTime)
        {
            
        }

        public void move(Vector3 movement)
        {
            position += movement;
        }

        public void move(float x, float y, float z)
        {
            position += new Vector3(x, y, z);
        }
    }
}
