using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Clockwork_Age_Editor
{
    class GameObject : Node
    {
        public Effect m_Effect;
        public Texture2D m_Texture;
        public Model m_Model;

        public string m_sName;

        GraphicsDevice GRAPHICS_DEVICE = AssetManager.Singleton.m_GraphicsDevice;
        Matrix worldRotation, worldScale;
        
        
        public GameObject(string name, Model model, Effect effect, Texture2D texture, Vector3 position)
        {
            m_vPosition = position;
            m_sName = name;
            m_Model = model;
            m_Texture = texture;
            m_Effect = effect;

            worldRotation = Matrix.Identity;
            worldScale = Matrix.CreateScale(1);
            
            SetEffect(m_Effect);
        }

        public void SetEffect(Effect effect)
        {
            foreach (ModelMesh mesh in m_Model.Meshes)
            {
                foreach (ModelMeshPart part in mesh.MeshParts)
                {
                    part.Effect = effect;
                }
            }
        }

        public override void Update(float deltaTime)
        {
            base.Update(deltaTime);
        }

        public void Draw()
        {
            Matrix[] tranforms = new Matrix[m_Model.Bones.Count];
            m_Model.CopyAbsoluteBoneTransformsTo(tranforms);

            foreach(ModelMesh mesh in m_Model.Meshes)
            {
                foreach (ModelMeshPart part in mesh.MeshParts)
                {
                    if (m_Texture == null)
                    {
                        m_Effect.CurrentTechnique = m_Effect.Techniques["DiffuseWithoutTexture"];
                    }
                    else
                    {
                        m_Effect.CurrentTechnique = m_Effect.Techniques["DiffuseWithTexture"];
                       
                        m_Effect.Parameters["ModelTexture"].SetValue(m_Texture);
                    }
                    m_Effect.Parameters["World"].SetValue(mesh.ParentBone.Transform * GetWorld());
                    m_Effect.Parameters["View"].SetValue(Camera.View);
                    m_Effect.Parameters["Projection"].SetValue(Camera.Projection);
                    m_Effect.Parameters["WorldInverseTranspose"].SetValue(Matrix.Transpose(Matrix.Invert(GetWorld() * mesh.ParentBone.Transform)));
                }

                mesh.Draw();
            }           
        }

        public virtual Matrix GetWorld()
        {
            return Matrix.CreateScale(0.01f) * worldScale * worldRotation * Matrix.CreateTranslation(m_vPosition);
        }

        public string[] Export()
        {
            string[] exportData = new string[4];
            exportData[0] = "Models/" + m_sName;
            exportData[1] = m_Effect.Name;


            return exportData;
        }
    }
}
