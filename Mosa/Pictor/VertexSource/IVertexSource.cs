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
    public interface IVertexSource
    {
        void Rewind(uint path_id);
        uint Vertex(out double x, out double y);
    };
}
