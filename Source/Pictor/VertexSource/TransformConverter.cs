/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Simon Wollwage (rootnode) <kintaro@think-in-co.de>
 */

namespace Pictor.VertexSource
{
    //----------------------------------------------------------TransformationConverter
    public class TransformationConverter : IVertexSource
    {
        private IVertexSource m_VertexSource;
        private Transform.ITransform m_Transform;

        public TransformationConverter(IVertexSource VertexSource, Transform.ITransform InTransform)
        {
            m_VertexSource = VertexSource;
            m_Transform = InTransform;
        }

        public void Attach(IVertexSource VertexSource) { m_VertexSource = VertexSource; }

        public void Rewind(uint path_id) 
        { 
            m_VertexSource.Rewind(path_id); 
        }

        public uint Vertex(out double x, out double y)
        {
            uint cmd = m_VertexSource.Vertex(out x, out y);
            if(Path.IsVertex(cmd))
            {
                m_Transform.Transform(ref x, ref y);
            }
            return cmd;
        }

        public void Transformer(Transform.ITransform InTransform)
        {
            m_Transform = InTransform;
        }
    };
}