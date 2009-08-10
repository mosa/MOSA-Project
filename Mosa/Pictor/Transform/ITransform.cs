/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Simon Wollwage (<mailto:rootnode@mosa-project.org>)
 */

namespace Pictor.Transform
{
    ///<summary>
    ///</summary>
    public interface ITransform
    {
        ///<summary>
        ///</summary>
        ///<param name="x"></param>
        ///<param name="y"></param>
        void Transform(ref double x, ref double y);
    };
}