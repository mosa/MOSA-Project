/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Simon Wollwage (<mailto:kintaro@mosa-project.org>)
 */

namespace Pictor.Objects
{
    /// <summary>
    /// 
    /// </summary>
    public enum PathCommand
    {
        /// <summary>
        /// 
        /// </summary>
        PathStop = 0,        //----path_cmd_stop    
        /// <summary>
        /// 
        /// </summary>
        PathMoveTo = 1,        //----path_cmd_move_to 
        /// <summary>
        /// 
        /// </summary>
        PathLineTo = 2,        //----path_cmd_line_to 
        /// <summary>
        /// 
        /// </summary>
        PathCurve3 = 3,        //----path_cmd_curve3  
        /// <summary>
        /// 
        /// </summary>
        PathCurve4 = 4,        //----path_cmd_curve4  
        /// <summary>
        /// 
        /// </summary>
        PathCurveN = 5,        //----path_cmd_curveN
        /// <summary>
        /// 
        /// </summary>
        PathCatrom = 6,        //----path_cmd_catrom
        /// <summary>
        /// 
        /// </summary>
        PathUbSpline = 7,        //----path_cmd_ubspline
        /// <summary>
        /// 
        /// </summary>
        PathEndPolygon = 0x0F,     //----path_cmd_end_poly
        /// <summary>
        /// 
        /// </summary>
        PathMask = 0x0F      //----path_cmd_mask    
    };
}