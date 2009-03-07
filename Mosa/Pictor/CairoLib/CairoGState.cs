using System;
using System.Collections.Generic;
using System.Text;

namespace Pictor.CairoLib
{
    /// <summary>
    /// 
    /// </summary>
    public class CairoGState 
    {
        /// <summary>
        /// 
        /// </summary>
        public Operator op;

        /// <summary>
        /// 
        /// </summary>
        public double tolerance;
        
        /// <summary>
        /// 
        /// </summary>
        public Antialias antialias;

        //public cairo_stroke_style_t stroke_style;

        /// <summary>
        /// 
        /// </summary>
        public FillRule fill_rule;

        /// <summary>
        /// 
        /// </summary>
        public FontFace font_face;

        /// <summary>
        /// 
        /// </summary>
        public ScaledFont scaled_font;	/* Specific to the current CTM */
        
        /// <summary>
        /// 
        /// </summary>
        public Matrix font_matrix;
        
        /// <summary>
        /// 
        /// </summary>
        public FontOptions font_options;

        //public cairo_clip_t clip;
        /// <summary>
        /// 
        /// </summary>
        public Surface target;		/* The target to which all rendering is directed */
        
        /// <summary>
        /// 
        /// </summary>
        public Surface parent_target;	/* The previous target which was receiving rendering */
        
        /// <summary>
        /// 
        /// </summary>
        public Surface original_target;	/* The original target the initial gstate was created with */

        /// <summary>
        /// 
        /// </summary>
        public Matrix ctm;
        
        /// <summary>
        /// 
        /// </summary>
        public Matrix ctm_inverse;
        
        /// <summary>
        /// 
        /// </summary>
        public Matrix source_ctm_inverse; /* At the time ->source was set */

        /// <summary>
        /// 
        /// </summary>
        public Pattern source;

        /// <summary>
        /// 
        /// </summary>
        public CairoGState next;
};
}
