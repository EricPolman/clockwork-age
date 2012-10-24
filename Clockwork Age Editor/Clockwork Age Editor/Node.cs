using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System.Windows.Forms;

namespace Clockwork_Age_Editor
{
    class Node : TreeNode
    {
        public Vector3 m_vPosition;

        public virtual void Update(float deltaTime)
        {
            
        }

        public void move(Vector3 movement)
        {
            m_vPosition += movement;
        }

        public void move(float x, float y, float z)
        {
            m_vPosition += new Vector3(x, y, z);
        }
    }
}
