
namespace Pictor.CairoLib
{
    /// <summary>
    /// 
    /// </summary>
    public class CairoT
    {
        /// <summary>
        /// 
        /// </summary>
        public uint ref_count;
        
        /// <summary>
        /// 
        /// </summary>
        public Pictor.Status status;
        //cairo_user_data_array_t user_data;
        
        /// <summary>
        /// 
        /// </summary>
        CairoGState gstate = null;
        
        /// <summary>
        /// 
        /// </summary>
        CairoGState[]  gstate_tail = new CairoGState[1];
        
        /// <summary>
        /// 
        /// </summary>
        CairoGState gstate_freelist = null;
        //cairo_path_fixed_t path[1];

        /// <summary>
        /// 
        /// </summary>
        public CairoT()
        {
            gstate = null;
            gstate_freelist = null;

            // dummy
            if (gstate != null && gstate_freelist != null)
                return;
        }
    }
}
