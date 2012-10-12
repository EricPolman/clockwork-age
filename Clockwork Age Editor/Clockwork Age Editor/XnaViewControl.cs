#region File Description
//-----------------------------------------------------------------------------
// SpinningTriangleControl.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System.Diagnostics;
using System.Windows.Forms;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System;
#endregion

namespace Clockwork_Age_Editor
{
    class XnaViewControl : GraphicsDeviceControl
    {
        public Clockwork_Age.Scene m_Scene;
        float deltaTime = 0;
        DateTime startTime, prev;
        
        public static int g_Width, g_Height;
        protected override void Initialize()
        {
            Application.Idle += delegate { Refresh(); };
            Application.Idle += delegate { Invalidate(); };
            g_Width = Width;
            g_Height = Height;
            
        }

        public override void Refresh()
        {
            TimeSpan ts = System.DateTime.Now - startTime;
            deltaTime = (float)ts.TotalSeconds;
            startTime = System.DateTime.Now;
            UpdateScene();
            base.Refresh();
            
        }

        public void LoadContent(ContentManager Content)
        {
            Clockwork_Age.ModelManager.Singleton.graphicsDevice = GraphicsDevice;
            Clockwork_Age.ModelManager.Singleton.LoadContent(Content);
            LoadScene(Content, new Clockwork_Age.Scene());
        }

        public void LoadScene(ContentManager Content, Clockwork_Age.Scene scene)
        {
            m_Scene = scene;
            m_Scene.Dimensions = new Microsoft.Xna.Framework.Vector2(Width, Height);
            m_Scene.LoadContent(Content, GraphicsDevice);
        }

        public void UpdateScene()
        {
            if (m_Scene != null)
                m_Scene.update(deltaTime);
        }

        protected override void Draw()
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            if(m_Scene != null)
                m_Scene.draw();
            // Models here and shit.
            
        }
    }
}
