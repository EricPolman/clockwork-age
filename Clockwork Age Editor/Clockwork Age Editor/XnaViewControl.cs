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
        public Scene m_Scene;
        float deltaTime = 0;
        DateTime startTime, prev;
        public TreeView m_SceneView;
        
        public static int g_LocationX, g_LocationY;

        protected override void Initialize()
        {
            Application.Idle += delegate { Refresh(); };
            Application.Idle += delegate { Invalidate(); };

            BoundingSphereRenderer.InitializeGraphics(GraphicsDevice, 49);
            m_SceneView = Form1.g_SceneView;
        }

        public override void Refresh()
        {
            TimeSpan ts = System.DateTime.Now - startTime;
            deltaTime = (float)ts.TotalSeconds;
            startTime = System.DateTime.Now;

            Selector.Singleton.update(deltaTime);
            UpdateScene();
            
            base.Refresh();

            g_LocationX = Location.X;
            g_LocationY = Location.Y;
            
        }

        public void LoadContent(ContentManager Content)
        {
            AssetManager.Singleton.m_GraphicsDevice = GraphicsDevice;

            Selector.Singleton.LoadContent(GraphicsDevice);
            LoadScene(Content, new Scene());
        }
        
        public void LoadScene(ContentManager Content, Scene scene)
        {
            m_Scene = scene;
            m_Scene.Dimensions = new Microsoft.Xna.Framework.Vector2(Width, Height);
            m_Scene.LoadContent(Content, GraphicsDevice);
        }

        public void UpdateScene()
        {
            if (m_Scene != null)
                m_Scene.Update(deltaTime);
        }

        protected override void Draw()
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            if(m_Scene != null)
                m_Scene.Draw();
            
        }
    }
}
