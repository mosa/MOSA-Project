/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Simon Wollwage (<mailto:rootnode@mosa-project.org>)
 */

namespace Pictor.VertexSource
{
    //----------------------------------------------------------conv_transform
    public class conv_transform : IVertexSource
    {
        private IVertexSource m_VertexSource;
        private Transform.ITransform m_Transform;

        public conv_transform(IVertexSource VertexSource, Transform.ITransform InTransform)
        {
            m_VertexSource = VertexSource;
            m_Transform = InTransform;
        }

        public void attach(IVertexSource VertexSource) { m_VertexSource = VertexSource; }

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

        public void transformer(Transform.ITransform InTransform)
        {
            m_Transform = InTransform;
        }
    };
}