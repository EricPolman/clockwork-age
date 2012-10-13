using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace Clockwork_Age_Editor
{
    class CameraMan
    {
        const float MOVEMENT_SPEED = 15;
        const float ROTATION_SPEED = 1f;

        Camera m_Camera;

        KeyboardState m_KeyboardState;
        MouseState m_CurrentMouseState, m_PastMouseState;

        public CameraMan(Camera camera)
        {
            m_Camera = camera;
            m_PastMouseState = Mouse.GetState();
        }

        public void update(float deltaTime)
        {
            m_KeyboardState = Keyboard.GetState();
            m_CurrentMouseState = Mouse.GetState();

            //movement shit hier!

            m_PastMouseState = m_CurrentMouseState;
        }
    }
}
