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
        #region Fields
        const float MOVEMENT_SPEED = 30;
        const float ROTATION_SPEED = 1f;

        Camera m_Camera;

        KeyboardState m_KeyboardState, m_PastKeyboardState;
        MouseState m_CurrentMouseState, m_PastMouseState;

        Vector2 m_MouseMovement;
        #endregion

        public CameraMan(Camera camera)
        {
            m_Camera = camera;
            m_PastMouseState = Mouse.GetState();
            m_PastKeyboardState = Keyboard.GetState();
        }

        public void update(float deltaTime)
        {
            m_KeyboardState = Keyboard.GetState();
            m_CurrentMouseState = Mouse.GetState();
            m_MouseMovement = new Vector2(-(m_CurrentMouseState.X - m_PastMouseState.X), m_CurrentMouseState.Y - m_PastMouseState.Y);
            float relativeScrollWheelValue = m_CurrentMouseState.ScrollWheelValue - m_PastMouseState.ScrollWheelValue;

            #region Mouse Control
            if (m_KeyboardState.IsKeyDown(Keys.LeftAlt))
            {
                if(m_CurrentMouseState.LeftButton == ButtonState.Pressed)
                {
                    //Orbiting
                    m_Camera.m_vRotation.Y += m_MouseMovement.X / 150;
                    m_Camera.m_vRotation.X -= m_MouseMovement.Y / 150;

                    m_Camera.m_mRotation = Matrix.CreateFromYawPitchRoll(m_Camera.m_vRotation.Y, m_Camera.m_vRotation.X, 0);

                    float dist = Vector3.Distance(m_Camera.m_vPosition, m_Camera.m_vTarget);
                    
                    m_Camera.m_vPosition = Vector3.Transform(Vector3.Backward, m_Camera.m_mRotation);
                    m_Camera.m_vPosition = m_Camera.m_vPosition * dist;

                    m_Camera.m_vPosition += m_Camera.m_vTarget;
                }
                else if (m_CurrentMouseState.MiddleButton == ButtonState.Pressed)
                {
                    //Panning
                    m_Camera.m_vPosition += Vector3.Transform(Vector3.Right * m_MouseMovement.X / 50, m_Camera.m_mRotation) + Vector3.Transform(Vector3.Up * m_MouseMovement.Y / 50, m_Camera.m_mRotation);
                    m_Camera.m_vTarget += Vector3.Transform(Vector3.Right * m_MouseMovement.X / 50, m_Camera.m_mRotation) + Vector3.Transform(Vector3.Up * m_MouseMovement.Y / 50, m_Camera.m_mRotation);

                }
                else if (m_CurrentMouseState.RightButton == ButtonState.Pressed)
                {
                    //Zooming
                    if(m_MouseMovement.Y > 0) 
                        m_Camera.m_vPosition = Vector3.Lerp(m_Camera.m_vPosition, m_Camera.m_vTarget, 0.02f);
                    else if(m_MouseMovement.Y < 0)
                        m_Camera.m_vPosition = Vector3.Lerp(m_Camera.m_vPosition, m_Camera.m_vTarget, -0.02f);
                }
            }

            //Scrolling
            if (relativeScrollWheelValue > 0)
            {
                m_Camera.m_vPosition = Vector3.Lerp(m_Camera.m_vPosition, m_Camera.m_vTarget, 0.02f);
                
            }
            else if (relativeScrollWheelValue < 0)
            {
                m_Camera.m_vPosition = Vector3.Lerp(m_Camera.m_vPosition, m_Camera.m_vTarget, -0.02f);
            }
            #endregion

            #region Keyboard Control
            if (m_KeyboardState.IsKeyDown(Keys.Up))
            {
                Vector3 forward = Vector3.Transform(Vector3.Forward, m_Camera.m_mRotation) * deltaTime * MOVEMENT_SPEED * 1.6f;
                forward.Y = 0;
                m_Camera.m_vPosition += forward;
                m_Camera.m_vTarget += forward;
            }
            if (m_KeyboardState.IsKeyDown(Keys.Right))
            {
                Vector3 right = Vector3.Transform(Vector3.Right, m_Camera.m_mRotation) * deltaTime * MOVEMENT_SPEED;
                right.Y = 0;
                m_Camera.m_vPosition += right;
                m_Camera.m_vTarget += right;
            }
            if (m_KeyboardState.IsKeyDown(Keys.Left))
            {
                Vector3 left = Vector3.Transform(Vector3.Left, m_Camera.m_mRotation) * deltaTime * MOVEMENT_SPEED;
                left.Y = 0;
                m_Camera.m_vPosition += left;
                m_Camera.m_vTarget += left;
            }
            if (m_KeyboardState.IsKeyDown(Keys.Down))
            {
                Vector3 back = Vector3.Transform(Vector3.Backward, m_Camera.m_mRotation) * deltaTime * MOVEMENT_SPEED * 1.6f;
                back.Y = 0;
                m_Camera.m_vPosition += back;
                m_Camera.m_vTarget += back;
            }
            if (m_KeyboardState.IsKeyDown(Keys.F))
            {
                Vector3 addition = Selector.Singleton.selection.m_BoundingSphere.Center - m_Camera.m_vTarget;
                m_Camera.m_vTarget = Selector.Singleton.selection.m_BoundingSphere.Center;
                m_Camera.m_vPosition += addition;
            }
            #endregion

            m_Camera.m_vUp = Vector3.Transform(Vector3.Up, m_Camera.m_mRotation);

            m_PastMouseState = m_CurrentMouseState;
            m_PastKeyboardState = m_KeyboardState;
        }
    }
}
