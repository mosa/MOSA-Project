namespace Mosa.DeviceDrivers.PCI.VideoCard
{
    public unsafe partial class IntelHD
    {
        // ------------------------------------------------------------------------------------------------
// Shader Types

static uint SHADER_VS                       = 0;
static uint SHADER_HS                       = 1;
static uint SHADER_DS                       = 2;
static uint SHADER_GS                       = 3;
static uint SHADER_PS                       = 4;
static uint SHADER_COUNT                    = 5;

// ------------------------------------------------------------------------------------------------
// Common macros

static uint MI_INSTR(uint opcode, uint flags)
{
    return (((opcode) << 23) | (flags));
}
static uint GFX_INSTR(uint subType, uint opcode, uint subOpcode, uint flags)
{
    return ((0x3 << 29) | (subType << 27) | ((opcode) << 24) | ((subOpcode) << 16) | (flags));
}

static uint MASKED_ENABLE(uint x)
{
    return (((x) << 16) | (x));
}
static uint MASKED_DISABLE(uint x)
{
    return ((x) << 16);
}

// ------------------------------------------------------------------------------------------------
// Vol 1. Part 1. Graphics Core
// ------------------------------------------------------------------------------------------------

// ------------------------------------------------------------------------------------------------
// 3.5.1 STATE_BASE_ADDRESS

static uint STATE_BASE_ADDRESS = GFX_INSTR(0, 0x1, 0x1, 8);

static uint BASE_ADDRESS_MODIFY             = (1 << 0);

// DWORD 1 - General State Base Address
// DWORD 2 - Surface State Base Address
// DWORD 3 - Dynamic State Base Address
// DWORD 4 - Indirect State Base Address
// DWORD 5 - Instruction Base Address
// DWORD 6 - General State Access Upper Bound
// DWORD 7 - Dynamic State Access Upper Bound
// DWORD 8 - Indirect State Access Upper Bound
// DWORD 9 - Instruction Access Upper Bound

// ------------------------------------------------------------------------------------------------
// 3.8.1 PIPELINE_SELECT

static uint PIPELINE_SELECT(uint x)
{
    return GFX_INSTR(1, 0x1, 0x4, x);
}

static uint PIPELINE_3D                     = 0x0;
static uint PIPELINE_MEDIA                  = 0x1;
static uint PIPELINE_GPGPU                  = 0x2;

// ------------------------------------------------------------------------------------------------
// Vol 1. Part 2. MMIO, Media Registers, and Programming Environment
// ------------------------------------------------------------------------------------------------

//typedef u64 GfxAddress;    // Address in Gfx Virtual space

// ------------------------------------------------------------------------------------------------
// 2.1.2.1 GTT Page Table Entries

static uint GTT_PAGE_SHIFT                  = 12;
static uint GTT_PAGE_SIZE                   = (uint)(1 << (int)GTT_PAGE_SHIFT);

static uint GTT_ENTRY_VALID                 = (1 << 0);
static uint GTT_ENTRY_L3_CACHE_CONTROL      = (1 << 1);
static uint GTT_ENTRY_LLC_CACHE_CONTROL     = (1 << 2);
static uint GTT_ENTRY_GFX_DATA_TYPE         = (1 << 3);
static uint GTT_ENTRY_ADDR(uint x)
{
    return ((x) | ((x >> 28) & 0xff0));
}

// ------------------------------------------------------------------------------------------------
// 3. GFX MMIO - MCHBAR Aperture

static uint GFX_MCHBAR                      = 0x140000;

// ------------------------------------------------------------------------------------------------
// Vol 1. Part 3. Memory Interface and Commands for the Render Engine
// ------------------------------------------------------------------------------------------------

// ------------------------------------------------------------------------------------------------
// 1.1.1.1 ARB_MODE – Arbiter Mode Control register

static uint ARB_MODE                        = 0x04030;     // R/W

static uint ARB_MODE_GGTAGDR                = (1 << 0);    // GTT Accesses GDR
static uint ARB_MODE_CCGDREN                = (1 << 1);    // Color Cache GDR Enable Bit
static uint ARB_MODE_DCGDREN                = (1 << 2);    // Depth Cache GDR Enable Bit
static uint ARB_MODE_TCGDREN                = (1 << 3);    // Texture Cache GDR Enable Bit
static uint ARB_MODE_VMC_GDR_EN             = (1 << 4);    // VMC GDR Enable
static uint ARB_MODE_AS4TS                  = (1 << 5);    // Address Swizzling for Tiled Surfaces
static uint ARB_MODE_CDPS                   = (1 << 8);    // Color/Depth Port Share Bit
static uint ARB_MODE_GAMPD_GDR              = (1 << 9);    // GAM PD GDR
static uint ARB_MODE_BLB_GDR                = (1 << 10);   // BLB GDR
static uint ARB_MODE_STC_GDR                = (1 << 11);   // STC GDR
static uint ARB_MODE_HIZ_GDR                = (1 << 12);   // HIZ GDR
static uint ARB_MODE_DC_GDR                 = (1 << 13);   // DC GDR
static uint ARB_MODE_GAM2BGTTT              = (1 << 14);   // GAM to Bypass GTT Translation

// ------------------------------------------------------------------------------------------------
// 1.1.5.1 Hardware Status Page Address

static uint RCS_HWS_PGA                     = 0x04080;     // R/W

// ------------------------------------------------------------------------------------------------
// 1.1.9 Instruction Parser Mode

static uint INSTPM                          = 0x020c0;     // R/W

// ------------------------------------------------------------------------------------------------
// 1.1.10.2 Render Mode Register for Software Interface

static uint MI_MODE                         = 0x0209c;     // R/W

// ------------------------------------------------------------------------------------------------
// 1.1.10.13 Render/Video Semaphore Sync

static uint RVSYNC                          = 0x02040;     // R/W

// ------------------------------------------------------------------------------------------------
// 1.1.10.14 Render/Blitter Semaphore Sync

static uint RBSYNC                          = 0x02044;     // R/W

// ------------------------------------------------------------------------------------------------
// 1.1.11.1 Ring Buffer Tail

static uint RCS_RING_BUFFER_TAIL            = 0x02030;     // R/W
static uint VCS_RING_BUFFER_TAIL            = 0x12030;     // R/W
static uint BCS_RING_BUFFER_TAIL            = 0x22030;     // R/W

// ------------------------------------------------------------------------------------------------
// 1.1.11.2 Ring Buffer Head

static uint RCS_RING_BUFFER_HEAD            = 0x02034;     // R/W
static uint VCS_RING_BUFFER_HEAD            = 0x12034;     // R/W
static uint BCS_RING_BUFFER_HEAD            = 0x22034;     // R/W

// ------------------------------------------------------------------------------------------------
// 1.1.11.3 Ring Buffer Start

static uint RCS_RING_BUFFER_START           = 0x02038;     // R/W
static uint VCS_RING_BUFFER_START           = 0x12038;     // R/W
static uint BCS_RING_BUFFER_START           = 0x22038;     // R/W

// ------------------------------------------------------------------------------------------------
// 1.1.11.4 Ring Buffer Control

static uint RCS_RING_BUFFER_CTL             = 0x0203c;     // R/W
static uint VCS_RING_BUFFER_CTL             = 0x1203c;     // R/W
static uint BCS_RING_BUFFER_CTL             = 0x2203c;     // R/W

// ------------------------------------------------------------------------------------------------
// 1.1.15 Pipeline Statistics Counters

static uint IA_VERTICES_COUNT               = 0x02310;     // R/W - 3DSTATE_VF_STATISTICS
static uint IA_PRIMITIVES_COUNT             = 0x02318;     // R/W - 3DSTATE_VF_STATISTICS
static uint VS_INVOCATION_COUNT             = 0x02320;     // R/W - 3DSTATE_VS
static uint HS_INVOCATION_COUNT             = 0x02300;     // R/W - 3DSTATE_HS
static uint DS_INVOCATION_COUNT             = 0x02308;     // R/W - 3DSTATE_DS
static uint GS_INVOCATION_COUNT             = 0x02328;     // R/W - 3DSTATE_GS
static uint GS_PRIMITIVES_COUNT             = 0x02330;     // R/W - 3DSTATE_GS
static uint CL_INVOCATION_COUNT             = 0x02338;     // R/W - 3DSTATE_CLIP
static uint CL_PRIMITIVES_COUNT             = 0x02340;     // R/W - 3DSTATE_CLIP
static uint PS_INVOCATION_COUNT             = 0x02348;     // R/W - 3DSTATE_WM
static uint PS_DEPTH_COUNT                  = 0x02350;     // R/W - 3DSTATE_WM
static uint TIMESTAMP                       = 0x02358;     // R/W
static uint SO_NUM_PRIMS_WRITTEN0           = 0x05200;     // R/W - 3DSTATE_STREAMOUT
static uint SO_NUM_PRIMS_WRITTEN1           = 0x05208;     // R/W - 3DSTATE_STREAMOUT
static uint SO_NUM_PRIMS_WRITTEN2           = 0x05210;     // R/W - 3DSTATE_STREAMOUT
static uint SO_NUM_PRIMS_WRITTEN3           = 0x05218;     // R/W - 3DSTATE_STREAMOUT
static uint SO_PRIM_STORAGE_NEEDED0         = 0x05240;     // R/W
static uint SO_PRIM_STORAGE_NEEDED1         = 0x05248;     // R/W
static uint SO_PRIM_STORAGE_NEEDED2         = 0x05250;     // R/W
static uint SO_PRIM_STORAGE_NEEDED3         = 0x05258;     // R/W
static uint SO_WRITE_OFFSET0                = 0x05280;     // R/W
static uint SO_WRITE_OFFSET1                = 0x05288;     // R/W
static uint SO_WRITE_OFFSET2                = 0x05290;     // R/W
static uint SO_WRITE_OFFSET3                = 0x05298;     // R/W

// ------------------------------------------------------------------------------------------------
// 1.2.5 MI_BATCH_BUFFER_END

static uint MI_BATCH_BUFFER_END             = MI_INSTR(0x0a, 0);

// ------------------------------------------------------------------------------------------------
// 1.2.7 MI_BATCH_BUFFER_START

static uint MI_BATCH_BUFFER_START           = MI_INSTR(0x31, 0);

// DWORD1 = batch buffer start address

// ------------------------------------------------------------------------------------------------
// 1.2.12 MI_NOOP

static uint MI_NOOP                         = MI_INSTR(0x00, 0);

// ------------------------------------------------------------------------------------------------
// 1.2.16 MI_SET_CONTEXT

static uint MI_SET_CONTEXT                  = MI_INSTR(0x18, 0);

// DWORD 1 = logical context address (4KB aligned)
static uint MI_GTT_ADDR                     = (1 << 8);
static uint MI_EXT_STATE_SAVE               = (1 << 3);
static uint MI_EXT_STATE_RESTORE            = (1 << 2);
static uint MI_FORCE_RESTORE                = (1 << 1);
static uint MI_RESTORE_INHIBIT              = (1 << 0);

// ------------------------------------------------------------------------------------------------
// 1.2.18 MI_STORE_DATA_INDEX

static uint MI_STORE_DATA_INDEX             = MI_INSTR(0x21, 1);

// DWORD 1 = offset
// DWORD 2 = data 0
// DWORD 3 = data 1

// ------------------------------------------------------------------------------------------------
// Vol 1. Part 4. Blitter Engine
// ------------------------------------------------------------------------------------------------

// ------------------------------------------------------------------------------------------------
// 1.8.1 COLOR_BLT (Fill)

static uint COLOR_BLT                       = ((2 << 29) | (0x40 << 22) | 3);

// ------------------------------------------------------------------------------------------------
// 1.10.1 BR00 - BLT Opcode and Control

static uint WRITE_ALPHA                     = (1 << 21);
static uint WRITE_RGB                       = (1 << 20);

// ------------------------------------------------------------------------------------------------
// 1.10.6 BR09 - Destination Address

// ------------------------------------------------------------------------------------------------
// 1.10.9 BR13 - BLT Raster OP, Control, and Destination Pitch

static uint COLOR_DEPTH_8                   = (0 << 24);
static uint COLOR_DEPTH_16                  = (1 << 24);
static uint COLOR_DEPTH_32                  = (3 << 24);

static uint ROP_SHIFT                       = 16;

// ------------------------------------------------------------------------------------------------
// 1.10.10 BR14 - Destination Width and Height

static uint HEIGHT_SHIFT                    = 16;

// ------------------------------------------------------------------------------------------------
// 1.10.12 BR16 - Pattern Expansion Background and Solid Pattern Color


// ------------------------------------------------------------------------------------------------
// 2.1.6.1 BCS Hardware Status Page Address

static uint BCS_HWS_PGA                     = 0x04280;     // R/W

// ------------------------------------------------------------------------------------------------
// 2.1.8.1 Blitter/Render Semaphore Sync

static uint BRSYNC                          = 0x22040;     // R/W

// ------------------------------------------------------------------------------------------------
// 2.1.8.2 Blitter/Video Semaphore Sync

static uint BVSYNC                          = 0x22044;     // R/W

// ------------------------------------------------------------------------------------------------
// Vol 1. Part 5. Video Codec Engine Command Streamer
// ------------------------------------------------------------------------------------------------

// ------------------------------------------------------------------------------------------------
// 1.1.2.2.1 VCS Hardware Status Page Address

static uint VCS_HWS_PGA                     = 0x04180;     // R/W

// ------------------------------------------------------------------------------------------------
// Vol 2. Part 1. 3D Pipeline
// ------------------------------------------------------------------------------------------------

// ------------------------------------------------------------------------------------------------
// 1.4.2 3DSTATE_CC_STATE_POINTERS

static uint COLOR_CALC_TABLE_ALIGN          = 64;

static uint _3DSTATE_CC_STATE_POINTERS      = GFX_INSTR(0x3, 0x0, 0xe, 0);

// DWORD 1 - Pointer to ColorCalcState (relative to Dynamic State Base Address)

// ------------------------------------------------------------------------------------------------
// 1.4.3 3DSTATE_BLEND_STATE_POINTERS

static uint BLEND_TABLE_ALIGN               = 64;

static uint _3DSTATE_BLEND_STATE_POINTERS   = GFX_INSTR(0x3, 0x0, 0x24, 0);

// DWORD 1 - Pointer to BlendState (relative to Dynamic State Base Address)

// ------------------------------------------------------------------------------------------------
// 1.4.4 3DSTATE_DEPTH_STENCIL_STATE_POINTERS

static uint DEPTH_STENCIL_TABLE_ALIGN                   = 64;

static uint _3DSTATE_DEPTH_STENCIL_STATE_POINTERS       = GFX_INSTR(0x3, 0x0, 0x24, 0);

// DWORD 1 - Pointer to DepthStencilState (relative to Dynamic State Base Address)

// ------------------------------------------------------------------------------------------------
// 1.4.5 3DSTATE_BINDING_TABLE_POINTERS

static uint BINDING_TABLE_ALIGN                         = 32;
static uint BINDING_TABLE_SIZE                          = 256;

static uint _3DSTATE_BINDING_TABLE_POINTERS_VS          = GFX_INSTR(0x3, 0x0, 0x26, 0);
static uint _3DSTATE_BINDING_TABLE_POINTERS_HS          = GFX_INSTR(0x3, 0x0, 0x27, 0);
static uint _3DSTATE_BINDING_TABLE_POINTERS_DS          = GFX_INSTR(0x3, 0x0, 0x28, 0);
static uint _3DSTATE_BINDING_TABLE_POINTERS_GS          = GFX_INSTR(0x3, 0x0, 0x29, 0);
static uint _3DSTATE_BINDING_TABLE_POINTERS_PS          = GFX_INSTR(0x3, 0x0, 0x2a, 0);

// DWORD 1 - Pointer to BindingTableState (relative to Surface State Base Address)

// ------------------------------------------------------------------------------------------------
// 1.5 3DSTATE_SAMPLER_STATE_POINTERS

static uint SAMPLER_TABLE_ALIGN                         = 32;
static uint SAMPLER_TABLE_SIZE                          = 16;

static uint _3DSTATE_SAMPLER_STATE_POINTERS_VS          = GFX_INSTR(0x3, 0x0, 0x2b, 0);
static uint _3DSTATE_SAMPLER_STATE_POINTERS_HS          = GFX_INSTR(0x3, 0x0, 0x2c, 0);
static uint _3DSTATE_SAMPLER_STATE_POINTERS_DS          = GFX_INSTR(0x3, 0x0, 0x2d, 0);
static uint _3DSTATE_SAMPLER_STATE_POINTERS_GS          = GFX_INSTR(0x3, 0x0, 0x2e, 0);
static uint _3DSTATE_SAMPLER_STATE_POINTERS_PS          = GFX_INSTR(0x3, 0x0, 0x2f, 0);

// DWORD 1 - Pointer to SamplerState table (relative to Dynamic State Base Address)

// ------------------------------------------------------------------------------------------------
// 1.6.1 3DSTATE_VIEWPORT_STATE_POINTERS_CC

static uint CC_VIEWPORT_TABLE_ALIGN                     = 32;
static uint CC_VIEWPORT_TABLE_SIZE                      = 16;

static uint _3DSTATE_VIEWPORT_STATE_POINTERS_CC         = GFX_INSTR(0x3, 0x0, 0x23, 0);

// DWORD 1 - Pointer to CCViewport table (relative to Dynamic State Base Address)

// ------------------------------------------------------------------------------------------------
// 1.6.2 3DSTATE_VIEWPORT_STATE_POINTERS_SF_CLIP

static uint SF_CLIP_VIEWPORT_TABLE_ALIGN                = 64;
static uint SF_CLIP_VIEWPORT_TABLE_SIZE                 = 16;

static uint _3DSTATE_VIEWPORT_STATE_POINTERS_SF_CLIP    = GFX_INSTR(0x3, 0x0, 0x21, 0);

// DWORD 1 - Pointer to SFClipViewport table (relative to Dynamic State Base Address)

// ------------------------------------------------------------------------------------------------
// 1.6.3 3DSTATE_SCISSOR_STATE_POINTERS

static uint _3DSTATE_SCISSOR_STATE_POINTERS             = GFX_INSTR(0x3, 0x0, 0x0f, 0);

// DWORD 1 - Pointer to ScissorRect table (relative to Dynamic State Base Address)

// ------------------------------------------------------------------------------------------------
// 1.7.1 3DSTATE_URB_*S

static uint _3DSTATE_URB_VS                             = GFX_INSTR(0x3, 0x0, 0x30, 0);
static uint _3DSTATE_URB_HS                             = GFX_INSTR(0x3, 0x0, 0x31, 0);
static uint _3DSTATE_URB_DS                             = GFX_INSTR(0x3, 0x0, 0x32, 0);
static uint _3DSTATE_URB_GS                             = GFX_INSTR(0x3, 0x0, 0x33, 0);

// DWORD 1
static uint URB_START_ADDR_SHIFT                        = 25;     // multiples of 8KB
static uint URB_START_ADDR_MASK                         = 0x1f;
static uint URB_ENTRY_ALLOC_SIZE_SHIFT                  = 16;     // multiples of 64B - 1
static uint URB_ENTRY_ALLOC_SIZE_MASK                   = 0x1ff;
static uint URB_ENTRY_COUNT_SHIFT                       = 0;
static uint URB_ENTRY_COUNT_MASK                        = 0xffff;

// ------------------------------------------------------------------------------------------------
// 1.10.4 PIPE_CONTROL Command

static uint PIPE_CONTROL(uint n)
{
    return GFX_INSTR(0x3, 0x2, 0x0, (n)-2);
}

// DWORD 1 - flags
static uint PIPE_CONTROL_DEPTH_CACHE_FLUSH              = (1 << 0);
static uint PIPE_CONTROL_SCOREBOARD_STALL               = (1 << 1);
static uint PIPE_CONTROL_STATE_CACHE_INVALIDATE         = (1 << 2);
static uint PIPE_CONTROL_CONST_CACHE_INVALIDATE         = (1 << 3);
static uint PIPE_CONTROL_VF_CACHE_INVALIDATE            = (1 << 4);
static uint PIPE_CONTROL_DC_FLUSH                       = (1 << 5);
static uint PIPE_CONTROL_PIPE_CONTROL_FLUSH             = (1 << 7);
static uint PIPE_CONTROL_NOTIFY                         = (1 << 8);
static uint PIPE_CONTROL_INDIRECT_STATE_DISABLE         = (1 << 9);
static uint PIPE_CONTROL_TEXTURE_CACHE_INVALIDATE       = (1 << 10);
static uint PIPE_CONTROL_INSTR_CACHE_INVALIDATE         = (1 << 11);
static uint PIPE_CONTROL_RENDER_TARGET_CACHE_FLUSH      = (1 << 12);
static uint PIPE_CONTROL_DEPTH_STALL                    = (1 << 13);
static uint PIPE_CONTROL_WRITE_IMM                      = (1 << 14);
static uint PIPE_CONTROL_WRITE_PS_DEPTH_COUNT           = (2 << 14);
static uint PIPE_CONTROL_WRITE_TIMESTAMP                = (3 << 14);
static uint PIPE_CONTROL_GENERIC_MEDIA_STATE_CLEAR      = (1 << 16);
static uint PIPE_CONTROL_TLB_INVALIDATE                 = (1 << 18);
static uint PIPE_CONTROL_GLOBAL_SNAPSHOT_COUNT_RESET    = (1 << 19);
static uint PIPE_CONTROL_CS_STALL                       = (1 << 20);
static uint PIPE_CONTROL_STORE_DATA_INDEX               = (1 << 21);
static uint PIPE_CONTROL_MMIO_WRITE_IMM                 = (1 << 23);
static uint PIPE_CONTROL_USE_GGTT                       = (1 << 24);

// DWORD 2 - address
// DWORD 3 - immediate data (low)
// DWORD 4 - immediate data (high)

// ------------------------------------------------------------------------------------------------
// 2.2.1 3DSTATE_INDEX_BUFFER

static uint _3DSTATE_INDEX_BUFFER           = GFX_INSTR(0x3, 0x0, 0x0a, 1);

// INDEX_FMT
static uint INDEX_FMT_BYTE                  = 0x0;
static uint INDEX_FMT_WORD                  = 0x1;
static uint INDEX_FMT_DWORD                 = 0x2;
static uint INDEX_FMT_MASK                  = 0x3;

// DWORD 0
static uint IB_OBJ_CONTROL_STATE_SHIFT      = 12;     // MEMORY_OBJECT_CONTROL_STATE
static uint IB_CUT_INDEX                    = (1 << 10);
static uint IB_FORMAT_SHIFT                 = 8;     // INDEX_FMT

// DWORD 1 - Buffer Starting Address (must be in linear memory)
// DWORD 2 - Buffer Ending Address

// ------------------------------------------------------------------------------------------------
// 2.3.1 3DSTATE_VERTEX_BUFFERS

static uint _3DSTATE_VERTEX_BUFFERS(uint n)
{
    return GFX_INSTR(0x3, 0x0, 0x08, 4*(n) - 1);
}

// DWORD 1..n - VERTEX_BUFFER_STATE

// ------------------------------------------------------------------------------------------------
// 2.3.2 VERTEX_BUFFER_STATE

// DWORD 0
static uint VB_INDEX_SHIFT                  = 26;
static uint VB_INDEX_MASK                   = 0x3f;
static uint VB_INSTANCE_DATA                = (1 << 20);
static uint VB_OBJ_CONTROL_STATE_SHIFT      = 16;     // MEMORY_OBJECT_CONTROL_STATE
static uint VB_ADDRESS_MODIFY               = (1 << 14);
static uint VB_NULL                         = (1 << 13);
static uint VB_FETCH_INVALIDATE             = (1 << 12);
static uint VB_PITCH_SHIFT                  = 0;
static uint VB_PITCH_MASK                   = 0xfff;

// DWORD 1 - Start Address
// DWORD 2 - End Address (inclusive)
// DWORD 3 - Instance Step Rate

// ------------------------------------------------------------------------------------------------
// 2.4.1 3DSTATE_VERTEX_ELEMENTS

static uint _3DSTATE_VERTEX_ELEMENTS(uint n)
{
    return GFX_INSTR(0x3, 0x0, 0x09, 2*(n) - 1);
}

// DWORD 1..n - VERTEX_ELEMENT_STATE

// ------------------------------------------------------------------------------------------------
// 2.4.2 VERTEX_ELEMENT_STATE

// VFCOMP_CONTROL
static uint VFCOMP_NOSTORE                  = 0x0;
static uint VFCOMP_STORE_SRC                = 0x1;
static uint VFCOMP_STORE_0                  = 0x2;
static uint VFCOMP_STORE_1_FP               = 0x3;
static uint VFCOMP_STORE_1_INT              = 0x4;
static uint VFCOMP_STORE_VID                = 0x5;
static uint VFCOMP_STORE_IID                = 0x6;
static uint VFCOMP_MASK                     = 0x7;

// DWORD 0
static uint VE_INDEX_SHIFT                  = 26;
static uint VE_INDEX_MASK                   = 0x3f;
static uint VE_VALID                        = (1 << 25);
static uint VE_FORMAT_SHIFT                 = 16;     // SURFACE_FORMAT
static uint VE_EDGE_FLAG                    = (1 << 15);
static uint VE_OFFSET_SHIFT                 = 0;
static uint VE_OFFSET_MASK                  = 0x7ff;

// DWORD 1
static uint VE_COMP0_SHIFT                  = 28;     // VFCOMP_CONTROL
static uint VE_COMP1_SHIFT                  = 24;     // VFCOMP_CONTROL
static uint VE_COMP2_SHIFT                  = 20;     // VFCOMP_CONTROL
static uint VE_COMP3_SHIFT                  = 16;     // VFCOMP_CONTROL

// ------------------------------------------------------------------------------------------------
// 2.5.1 3DPRIMITIVE

static uint _3DPRIMITIVE                    = GFX_INSTR(0x3, 0x3, 0x00, 5);

// 3DPRIM_TOPOLOGY (not defined in documentation, cut index table in 2.2.1 seems to correspond to ordering)
static uint _3DPRIM_POINTLIST               = 0x01;
static uint _3DPRIM_LINELIST                = 0x02;
static uint _3DPRIM_LINESTRIP               = 0x03;
static uint _3DPRIM_TRILIST                 = 0x04;
static uint _3DPRIM_TRISTRIP                = 0x05;
static uint _3DPRIM_TRIFAN                  = 0x06;
static uint _3DPRIM_QUADLIST                = 0x07;
static uint _3DPRIM_QUADSTRIP               = 0x08;
static uint _3DPRIM_LINELIST_ADJ            = 0x09;
static uint _3DPRIM_LINESTRIP_ADJ           = 0x0a;
static uint _3DPRIM_TRILIST_ADJ             = 0x0b;
static uint _3DPRIM_TRISTRIP_ADJ            = 0x0c;
static uint _3DPRIM_TRISTRIP_REVERSE        = 0x0d;
static uint _3DPRIM_POLYGON                 = 0x0e;
static uint _3DPRIM_RECTLIST                = 0x0f;
static uint _3DPRIM_LINELOOP                = 0x10;
static uint _3DPRIM_POINTLIST_BF            = 0x11;
static uint _3DPRIM_LINESTRIP_CONT          = 0x12;
static uint _3DPRIM_LINESTRIP_BF            = 0x13;
static uint _3DPRIM_LINESTRIP_CONT_BF       = 0x14;
static uint _3DPRIM_TRIFAN_NOSTIPPLE        = 0x15;
static uint _3DPRIM_PATCHLIST_n             ;// ???
static uint _3DPRIM_MASK                    = 0x3f;

// DWORD 0
static uint PRIM_INDIRECT_PARAMETER         = (1 << 10);
static uint PRIM_PREDICATE                  = (1 << 8);

// DWORD 1
static uint PRIM_END_OFFSET                 = (1 << 9);
static uint PRIM_RANDOM                     = (1 << 8);
static uint PRIM_TOPOLOGY_SHIFT             = 0;     // 3DPRIM_TOPOLOGY

// DWORD 2 - Vertex Count Per Instance
// DWORD 3 - Start Vertex Location
// DWORD 4 - Instance Count
// DWORD 5 - Start Instance Location
// DWORD 6 - Base Vertex Location

// ------------------------------------------------------------------------------------------------
// 2.7.1 3DSTATE_VF_STATISTICS

static uint _3DSTATE_VF_STATISTICS          = GFX_INSTR(0x1, 0x0, 0x0b, 0);

// DWORD 0
static uint VF_STATISTICS                   = (1 << 0);

// ------------------------------------------------------------------------------------------------
// Shader Stages

// SAMPLER_USAGE
static uint SAMPLER_USAGE_NONE              = 0x0;
static uint SAMPLER_USAGE_1_4               = 0x1;
static uint SAMPLER_USAGE_5_8               = 0x2;
static uint SAMPLER_USAGE_9_12              = 0x3;
static uint SAMPLER_USAGE_13_16             = 0x4;
static uint SAMPLER_USAGE_MASK              = 0x7;

// 3DSTATE_CONSTANT(Body)
struct ConstantBufferBody
{
    ushort[] bufLen = new ushort[4];
    uint[] buffers = new uint[4];
}

// CONST_ALLOC
static uint CONST_ALLOC_OFFSET_SHIFT        = 16;
static uint CONST_ALLOC_OFFSET_MASK         = 0xf;
static uint CONST_ALLOC_SIZE_SHIFT          = 0;
static uint CONST_ALLOC_SIZE_MASK           = 0x3f;

// ------------------------------------------------------------------------------------------------
// 3DSTATE_CONSTANT_*S

static uint _3DSTATE_CONSTANT_VS            = GFX_INSTR(0x3, 0x0, 0x15, 5);
static uint _3DSTATE_CONSTANT_HS            = GFX_INSTR(0x3, 0x0, 0x19, 5);
static uint _3DSTATE_CONSTANT_DS            = GFX_INSTR(0x3, 0x0, 0x1a, 5);
static uint _3DSTATE_CONSTANT_GS            = GFX_INSTR(0x3, 0x0, 0x16, 5);
static uint _3DSTATE_CONSTANT_PS            = GFX_INSTR(0x3, 0x0, 0x17, 5);

// DWORD 1..6 - ConstantBufferBody

// ------------------------------------------------------------------------------------------------
// 3.2.1.4 3DSTATE_PUSH_CONSTANT_ALLOC_*S

static uint _3DSTATE_PUSH_CONSTANT_ALLOC_VS = GFX_INSTR(0x3, 0x1, 0x12, 0);
static uint _3DSTATE_PUSH_CONSTANT_ALLOC_HS = GFX_INSTR(0x3, 0x1, 0x13, 0);
static uint _3DSTATE_PUSH_CONSTANT_ALLOC_DS = GFX_INSTR(0x3, 0x1, 0x14, 0);
static uint _3DSTATE_PUSH_CONSTANT_ALLOC_GS = GFX_INSTR(0x3, 0x1, 0x15, 0);
static uint _3DSTATE_PUSH_CONSTANT_ALLOC_PS = GFX_INSTR(0x3, 0x1, 0x16, 0);

// DWORD 1 - CONST_ALLOC

// ------------------------------------------------------------------------------------------------
// 3.2.1.2 3DSTATE_VS

static uint _3DSTATE_VS                     = GFX_INSTR(0x03, 0x0, 0x10, 4);

// DWORD 1 - Kernel Start Pointer (relative to Instruction Base Address)

// DWORD 2
static uint VS_SINGLE_VERTEX_DISPATCH       = (uint)(1 << 31);
static uint VS_VECTOR_MASK                  = (1 << 30);
static uint VS_SAMPLER_COUNT_SHIFT          = 27;     // SAMPLER_USAGE
static uint VS_BINDING_COUNT_SHIFT          = 18;
static uint VS_BINDING_COUNT_MASK           = 0xff;
static uint VS_FLOATING_POINT_ALT           = (1 << 16);
static uint VS_ILLEGAL_OPCODE_EX            = (1 << 13);
static uint VS_SOFTWARE_EX                  = (1 << 7);

// DWORD 3
static uint VS_SCRATCH_SPACE_BASE_SHIFT     = (1 << 10);
static uint VS_SCRATCH_SPACE_BASE_MASK      = 0x3fffff;
static uint VS_PER_THREAD_SPACE_SHIFT       = 0;     // Power of 2 bytes + 1KB
static uint VS_PER_THREAD_SPACE_MASK        = 0xf;

// DWORD 4
static uint VS_DISPATCH_GRF_SHIFT           = 20;
static uint VS_DISPATCH_GRF_MASK            = 0x1f;
static uint VS_URB_READ_LENGTH_SHIFT        = 11;
static uint VS_URB_READ_LENGTH_MASK         = 0x3f;
static uint VS_URB_READ_OFFSET_SHIFT        = 4;
static uint VS_URB_READ_OFFSET_MASK         = 0x3f;

// DWORD 5
static uint VS_MAX_THREAD_SHIFT             = 25;
static uint VS_MAX_THREAD_MASK              = 0x7f;
static uint VS_STATISTICS                   = (1 << 10);
static uint VS_CACHE_DISABLE                = (1 << 1);
static uint VS_ENABLE                       = (1 << 0);

// ------------------------------------------------------------------------------------------------
// 4.4 3DSTATE_HS

static uint _3DSTATE_HS                     = GFX_INSTR(0x03, 0x0, 0x1b, 5);

// DWORD 1
static uint HS_SAMPLER_COUNT_SHIFT          = 27;     // SAMPLER_USAGE
static uint HS_BINDING_COUNT_SHIFT          = 18;
static uint HS_BINDING_COUNT_MASK           = 0xff;
static uint HS_FLOATING_POINT_ALT           = (1 << 16);
static uint HS_FLOATING_POINT_ALT           = (1 << 16);
static uint HS_ILLEGAL_OPCODE_EX            = (1 << 13);
static uint HS_SOFTWARE_EX                  = (1 << 7);
static uint HS_MAX_THREAD_SHIFT             = 0;
static uint HS_MAX_THREAD_MASK              = 0x7f;

// DWORD 2
static uint HS_ENABLE                       = (uint)(1 << 31);
static uint HS_STATISTICS                   = (1 << 29);
static uint HS_INSTANCE_COUNT_SHIFT         = 0;
static uint HS_INSTANCE_COUNT_MASK          = 0xf;

// DWORD 3 - Kernel Start Pointer (relative to Instruction Base Address)

// DWORD 4
static uint HS_SCRATCH_SPACE_BASE_SHIFT     = (1 << 10);
static uint HS_SCRATCH_SPACE_BASE_MASK      = 0x3fffff;
static uint HS_PER_THREAD_SPACE_SHIFT       = 0;     // Power of 2 bytes + 1KB
static uint HS_PER_THREAD_SPACE_MASK        = 0xf;

// DWORD 5
static uint HS_SINGLE_PROGRAM_FLOW          = (1 << 27);
static uint HS_VECTOR_MASK                  = (1 << 26);
static uint HS_INCLUDE_VERTEX_HANDLES       = (1 << 24);
static uint HS_DISPATCH_GRF_SHIFT           = 19;
static uint HS_DISPATCH_GRF_MASK            = 0x1f;
static uint HS_URB_READ_LENGTH_SHIFT        = 11;
static uint HS_URB_READ_LENGTH_MASK         = 0x3f;
static uint HS_URB_READ_OFFSET_SHIFT        = 4;
static uint HS_URB_READ_OFFSET_MASK         = 0x3f;

// DWORD 6 - Semaphore Handle

// ------------------------------------------------------------------------------------------------
// 5.1 3DSTATE_TE

static uint _3DSTATE_TE                     = GFX_INSTR(0x3, 0x0, 0x1c, 2);

// TE_PARTITIONING
static uint TE_PARTITIONING_INT             = 0x0;
static uint TE_PARTITIONING_ODD_FRAC        = 0x1;
static uint TE_PARTITIONING_EVEN_FRAC       = 0x2;
static uint TE_PARTITIONING_MASK            = 0x3;

// TE_OUTPUT
static uint TE_OUTPUT_POINT                 = 0x0;
static uint TE_OUTPUT_LINE                  = 0x1;
static uint TE_OUTPUT_TRI_CW                = 0x2;
static uint TE_OUTPUT_TRI_CCW               = 0x3;
        
// TE_DOMAIN
static uint TE_DOMAIN_QUAD                  = 0x0;
static uint TE_DOMAIN_TRI                   = 0x1;
static uint TE_DOMAIN_ISOLINE               = 0x2;
static uint TE_DOMAIN_MASK                  = 0x3;

// DWORD 1
static uint TE_PARTITIONING_SHIFT           = 12;     // TE_PARTITIONING
static uint TE_OUTPUT_SHIFT                 = 8;      // TE_OUTPUT
static uint TE_DOMAIN_SHIFT                 = 4;
static uint TE_SW_TESS                      = (1 << 1);
static uint TE_ENABLE                       = (1 << 0);

// DWORD 2 - Max TessFactor Odd (float)
// DWORD 3 - Max TessFactor Not Odd (float)

// ------------------------------------------------------------------------------------------------
// 6.1 3DSTATE_DS

static uint _3DSTATE_DS                     = GFX_INSTR(0x3, 0x0, 0x1d, 4);

// DWORD 1 - Kernel Start Pointer (relative teo Instruction Base Address)

// DWORD 2
static uint DS_SINGLE_POINT_DISPATCH        = (uint)(1 << 31);
static uint DS_VECTOR_MASK                  = (1 << 30);
static uint DS_SAMPLER_COUNT_SHIFT          = 27;     // SAMPLER_USAGE
static uint DS_BINDING_COUNT_SHIFT          = 18;
static uint DS_BINDING_COUNT_MASK           = 0xff;
static uint DS_FLOATING_POINT_ALT           = (1 << 16);
static uint DS_ILLEGAL_OPCODE_EX            = (1 << 13);
static uint DS_SOFTWARE_EX                  = (1 << 7);

// DWORD 3
static uint DS_SCRATCH_SPACE_BASE_SHIFT     = (1 << 10);
static uint DS_SCRATCH_SPACE_BASE_MASK      = 0x3fffff;
static uint DS_PER_THREAD_SPACE_SHIFT       = 0;     // Power of 2 bytes + 1KB
static uint DS_PER_THREAD_SPACE_MASK        = 0xf;

// DWORD 4
static uint DS_DISPATCH_GRF_SHIFT           = 20;
static uint DS_DISPATCH_GRF_MASK            = 0x1f;
static uint DS_URB_READ_LENGTH_SHIFT        = 11;
static uint DS_URB_READ_LENGTH_MASK         = 0x3f;
static uint DS_URB_READ_OFFSET_SHIFT        = 4;
static uint DS_URB_READ_OFFSET_MASK         = 0x3f;

// DWORD 5
static uint DS_MAX_THREAD_SHIFT             = 25;
static uint DS_MAX_THREAD_MASK              = 0x7f;
static uint DS_STATISTICS                   = (1 << 10);
static uint DS_COMPUTE_W                    = (1 << 2);
static uint DS_CACHE_DISABLE                = (1 << 1);
static uint DS_ENABLE                       = (1 << 0);

// ------------------------------------------------------------------------------------------------
// 7.2.1.1 3DSTATE_GS

static uint _3DSTATE_GS                     = GFX_INSTR(0x3, 0x0, 0x11, 5);

// DWORD 1 - Kernel Start Pointer (relative teo Instruction Base Address)

// DWORD 2
static uint GS_SINGLE_PROGRAM_FLOW          = (uint)(1 << 31);
static uint GS_VECTOR_MASK                  = (1 << 30);
static uint GS_SAMPLER_COUNT_SHIFT          = 27;     // SAMPLER_USAGE
static uint GS_BINDING_COUNT_SHIFT          = 18;
static uint GS_BINDING_COUNT_MASK           = 0xff;
static uint GS_THREAD_HIGH_PRIORITY         = (1 << 17);
static uint GS_FLOATING_POINT_ALT           = (1 << 16);
static uint GS_ILLEGAL_OPCODE_EX            = (1 << 13);
static uint GS_MASK_STACK_EX                = (1 << 11);
static uint GS_SOFTWARE_EX                  = (1 << 7);

// DWORD 3
static uint GS_SCRATCH_SPACE_BASE_SHIFT     = (1 << 10);
static uint GS_SCRATCH_SPACE_BASE_MASK      = 0x3fffff;
static uint GS_PER_THREAD_SPACE_SHIFT       = 0;     // Power of 2 bytes + 1KB
static uint GS_PER_THREAD_SPACE_MASK        = 0xf;

// DWORD 4
static uint GS_OUTPUT_VTX_SIZE_SHIFT        = 23;     // multiples of 32B - 1
static uint GS_OUTPUT_VTX_SIZE_MASK         = 0x3f;
static uint GS_OUTPUT_TOPOLOGY_SHIFT        = 17;     // 3DPRIM_TOPOLOGY
static uint GS_URB_READ_LENGTH_SHIFT        = 11;
static uint GS_URB_READ_LENGTH_MASK         = 0x3f;
static uint GS_URB_INCLUDE_VTX_HANDLES      = (1 << 10);
static uint GS_URB_READ_OFFSET_SHIFT        = 4;
static uint GS_URB_READ_OFFSET_MASK         = 0x3f;
static uint GS_DISPATCH_GRF_SHIFT           = 0;
static uint GS_DISPATCH_GRF_MASK            = 0xf;

// DWORD 5
static uint GS_MAX_THREAD_SHIFT             = 25;
static uint GS_MAX_THREAD_MASK              = 0x7f;
static uint GS_CONTROL_STREAM_ID            = (1 << 24);
static uint GS_CONTROL_HEADER_SIZE_SHIFT    = 20;
static uint GS_CONTROL_HEADER_SIZE_MASK     = 0xf;
static uint GS_INSTANCE_CONTROL_SHIFT       = 15;
static uint GS_INSTANCE_CONTROL_MASK        = 0x1f;
static uint GS_DEFAULT_STREAM_ID_SHIFT      = 13;
static uint GS_DEFAULT_STREAM_ID_MASK       = 0x3;
static uint GS_DISPATCH_DUAL_OBJECT         = (1 << 12);
static uint GS_DISPATCH_DUAL_INSTANCE       = (1 << 11);
static uint GS_STATISTICS                   = (1 << 10);
static uint GS_INVOCATIONS_INC_SHIFT        = 5;
static uint GS_INVOCATIONS_INC_MASK         = 0x1f;
static uint GS_INCLUDE_PID                  = (1 << 4);
static uint GS_HINT                         = (1 << 3);
static uint GS_REORDE                       = (1 << 2);
static uint GS_DISCARD_ADJACENCY            = (1 << 1);
static uint GS_ENABLE                       = (1 << 0);

// DWORD 6 - Semaphore Handle

// ------------------------------------------------------------------------------------------------
// 8.4 3DSTATE_STREAMOUT

static uint _3DSTATE_STREAMOUT              = GFX_INSTR(0x3, 0x0, 0x1e, 1);

// DWORD 1
static uint SO_ENABLE                       = (uint)(1 << 31);
static uint SO_RENDERING_DISABLE            = (1 << 30);
static uint SO_RENDER_STREAM_SHIFT          = 27;
static uint SO_RENDER_STREAM_MASK           = 0x3;
static uint SO_REORDER_TRAILING             = (1 << 26);
static uint SO_STATISTICS                   = (1 << 25);
static uint SO_BUFFER3                      = (1 << 11);
static uint SO_BUFFER2                      = (1 << 10);
static uint SO_BUFFER1                      = (1 << 9);
static uint SO_BUFFER0                      = (1 << 8);

// DWORD 2
static uint SO_STREAM_READ_LEN_MASK         = 0x1f;
static uint SO_STREAM3_READ_OFFSET          = (1 << 29);
static uint SO_STREAM3_READ_LEN_SHIFT       = 24;
static uint SO_STREAM2_READ_OFFSET          = (1 << 21);
static uint SO_STREAM2_READ_LEN_SHIFT       = 16;
static uint SO_STREAM1_READ_OFFSET          = (1 << 13);
static uint SO_STREAM1_READ_LEN_SHIFT       = 8;
static uint SO_STREAM0_READ_OFFSET          = (1 << 5);
static uint SO_STREAM0_READ_LEN_SHIFT       = 0;

// ------------------------------------------------------------------------------------------------
// 8.5 3DSTATE_SO_DECL_LIST

static uint _3DSTATE_SO_DECL_LIST(uint n)
{
    return GFX_INSTR(0x3, 0x1, 0x17, (n)*2 + 1);
}

// DWORD 1
static uint SO_DECLS_STREAM_OFFSETS_MASK    = 0xf;
static uint SO_DECLS_STREAM3_OFFSETS_SHIFT  = 12;
static uint SO_DECLS_STREAM2_OFFSETS_SHIFT  = 8;
static uint SO_DECLS_STREAM1_OFFSETS_SHIFT  = 4;
static uint SO_DECLS_STREAM0_OFFSETS_SHIFT  = 0;

// DWORD 2
static uint SO_DECLS_STREAM_ENTRIES_MASK    = 0xff;
static uint SO_DECLS_STREAM3_ENTRIES_SHIFT  = 24;
static uint SO_DECLS_STREAM2_ENTRIES_SHIFT  = 16;
static uint SO_DECLS_STREAM1_ENTRIES_SHIFT  = 8;
static uint SO_DECLS_STREAM0_ENTRIES_SHIFT  = 0;

// ------------------------------------------------------------------------------------------------
// 8.5.1 SO_DECL

// WORD
static uint SO_DECL_OUTPUT_SLOT_SHIFT       = 12;
static uint SO_DECL_OUTPUT_SLOT_MASK        = 0x3;
static uint SO_DECL_HOLE_FLAG               = (1 << 11);
static uint SO_DECL_REG_INDEX_SHIFT         = 4;
static uint SO_DECL_REG_INDEX_MASK          = 0x3f;
static uint SO_DECL_COMP_MASK_SHIFT         = 0;     // xyzw bitfield

// ------------------------------------------------------------------------------------------------
// 8.6 3DSTATE_SO_BUFFER

static uint _3DSTATE_SO_BUFFER              = GFX_INSTR(0x3, 0x1, 0x18, 2);

// DWORD 1
static uint SO_BUF_INDEX_SHIFT              = 29;
static uint SO_BUF_INDEX_MASK               = 0x3;
static uint SO_BUF_OBJ_CONTROL_STATE_SHIFT  = 25;     // MEMORY_OBJECT_CONTROL_STATE
static uint SO_BUF_PITCH_SHIFT              = 0;
static uint SO_BUF_PITCH_MASK               = 0xfff;

// DWORD 2 - Surface Base Address
// DWORD 3 - End Base Address

// ------------------------------------------------------------------------------------------------
// 9.3.1.1 3DSTATE_CLIP

static uint _3DSTATE_CLIP                   = GFX_INSTR(0x3, 0x0, 0x12, 2);

// CULL_MODE
static uint CULL_BOTH                       = 0x0;
static uint CULL_NONE                       = 0x1;
static uint CULL_FRONT                      = 0x2;
static uint CULL_BACK                       = 0x3;
static uint CULL_MASK                       = 0x3;

// CLIP_MODE
static uint CLIP_NORMAL                     = 0x0;
static uint CLIP_REJECT_ALL                 = 0x3;
static uint CLIP_ACCEPT_ALL                 = 0x4;
static uint CLIP_MASK                       = 0x7;

// DWORD 1
static uint CLIP_FRONT_CCW                  = (1 << 20);
static uint CLIP_SUBPIXEL_4_BITS            = (1 << 19);
static uint CLIP_EARLY_CULL                 = (1 << 18);
static uint CLIP_CULL_SHIFT                 = 16;     // CULL_MODE
static uint CLIP_STATISTICS                 = (1 << 10);
static uint CLIP_USER_CULL_SHIFT            = 0;
static uint CLIP_USER_CULL_MASK             = 0xff;

// DWORD 2
static uint CLIP_ENABLE                     = (uint)(1 << 31);
static uint CLIP_API_DX                     = (1 << 30);   // Value not documented, 0 is OGL
static uint CLIP_VIEWPORT_XY                = (1 << 28);
static uint CLIP_VIEWPORT_Z                 = (1 << 27);
static uint CLIP_GUARDBAND                  = (1 << 26);
static uint CLIP_USER_CLIP_SHIFT            = 16;
static uint CLIP_USER_CLIP_MASK             = 0xff;
static uint CLIP_MODE_SHIFT                 = 13;     // CLIP_MODE
static uint CLIP_PERSP_DIVIDE               = (1 << 9);
static uint CLIP_NON_PERSP_BARY             = (1 << 8);
static uint CLIP_TRI_STRIP_LIST_PVTX_SHIFT  = 4;
static uint CLIP_TRI_STRIP_LIST_PVTX_MASK   = 0x3;
static uint CLIP_LINE_STRIP_LIST_PVTX_SHIFT = 2;
static uint CLIP_LINE_STRIP_LIST_PVTX_MASK  = 0x3;
static uint CLIP_TRI_FAN_PVTX_SHIFT         = 0;
static uint CLIP_TRI_FAN_PVTX_MASK          = 0x3;

// DWORD 3
static uint CLIP_MIN_POINT_WIDTH_SHIFT      = 17;     // Unsigned 8.3
static uint CLIP_MIN_POINT_WIDTH_MASK       = 0x7ff;
static uint CLIP_MAX_POINT_WIDTH_SHIFT      = 6;     // Unsigned 8.3
static uint CLIP_MAX_POINT_WIDTH_MASK       = 0x7ff;
static uint CLIP_FORCE_ZERO_RTA_INDEX       = (1 << 5);
static uint CLIP_MAX_VP_INDEX_SHIFT         = 0;
static uint CLIP_MAX_VP_INDEX_MASK          = 0xf;

// ------------------------------------------------------------------------------------------------
// 10.3.5.1 3DSTATE_DRAWING_RECTANGLE

static uint _3DSTATE_DRAWING_RECTANGLE      = GFX_INSTR(0x3, 0x1, 0x00, 2);

// DWORD 1
static uint DRAWING_RECT_Y_MIN_SHIFT        = 16;
static uint DRAWING_RECT_Y_MIN_MASK         = 0xffff;
static uint DRAWING_RECT_X_MIN_SHIFT        = 0;
static uint DRAWING_RECT_X_MIN_MASK         = 0xffff;

// DWORD 2
static uint DRAWING_RECT_Y_MAX_SHIFT        = 16;
static uint DRAWING_RECT_Y_MAX_MASK         = 0xffff;
static uint DRAWING_RECT_X_MAX_SHIFT        = 0;
static uint DRAWING_RECT_X_MAX_MASK         = 0xffff;

// DWORD 3
static uint DRAWING_RECT_Y_ORIGIN_SHIFT     = 16;
static uint DRAWING_RECT_Y_ORIGIN_MASK      = 0xffff;
static uint DRAWING_RECT_X_ORIGIN_SHIFT     = 0;
static uint DRAWING_RECT_X_ORIGIN_MASK      = 0xffff;

// ------------------------------------------------------------------------------------------------
// 10.3.13 3DSTATE_SF

static uint _3DSTATE_SF                     = GFX_INSTR(0x3, 0x0, 0x13, 5)

// DEPTH_FORMAT
static uint DFMT_D32_FLOAT_S8X24_UINT       0x0
static uint DFMT_D32_FLOAT                  = 0x1
static uint DFMT_D24_UNORM_S8_UINT          = 0x2
static uint DFMT_D24_UNORM_X8_UINT          = 0x3
static uint DFMT_D16_UNORM                  = 0x5
static uint DFMT_MASK                       = 0x7

// FILL_MODE
static uint FILL_SOLID                      = 0x0
static uint FILL_WIREFRAME                  = 0x1
static uint FILL_POINT                      = 0x2
static uint FILL_MASK                       = 0x3

// DWORD 1
static uint SF_FORMAT_SHIFT                 12     ;     // DEPTH_FORMAT
static uint SF_LEGACY_GLOBAL_DEPTH_BIAS     = (1 << 11)
static uint SF_STATISTICS                   = (1 << 10)
static uint SF_GLOBAL_DEPTH_OFFSET_SOLID    = (1 << 9)
static uint SF_GLOBAL_DEPTH_OFFSET_WF       = (1 << 8)
static uint SF_GLOBAL_DEPTH_OFFSET_POINT    = (1 << 7)
static uint SF_FRONT_FACE_FILL_SHIFT        5      ;     // FILL_MODE
static uint SF_BACK_FACE_FILL_SHIFT         3      ;     // FILL_MODE
static uint SF_VIEW_TRANSFORM               = (1 << 1)
static uint SF_FRONT_WINDING                = (1 << 0)

// DWORD 2
static uint SF_ANTI_ALIAS                   = (1 << 31)
static uint SF_CULL_SHIFT                   29     ;     // CULL_MODE
static uint SF_LINE_WIDTH_SHIFT             18     ;     // Unsigned 3.7
static uint SF_LINE_WIDTH_MASK              = 0x3ff
static uint SF_LINE_END_AA_WIDTH_SHIFT      16
static uint SF_SCISSOR                      = (1 << 11)
static uint SF_MULTISAMPLE_SHIFT            = (1 << 8)

// DWORD 3
static uint SF_LAST_PIXEL                   = (1 << 31)
static uint SF_TRI_STRIP_LIST_PVTX_SHIFT    29
static uint SF_TRI_STRIP_LIST_PVTX_MASK     0x3
static uint SF_LINE_STRIP_LIST_PVTX_SHIFT   27
static uint SF_LINE_STRIP_LIST_PVTX_MASK    0x3
static uint SF_TRI_FAN_PVTX_SHIFT           25
static uint SF_TRI_FAN_PVTX_MASK            = 0x3
static uint SS_AA_LINE_DISTANCE             = (1 << 14)
static uint SF_SUBPIXEL_4_BITS              = (1 << 12)
static uint SF_USE_POINT_WIDTH_STATE        = (1 << 11)
static uint SF_POINT_WIDTH_SHIFT            0      ;     // Unsigned 8.3
static uint SF_POINT_WIDTH_MASK             = 0x7ff

// DWORD 4 - Global Depth Offset Constant (float)
// DWORD 5 - Global Depth Offset Scale (float)
// DWORD 6 - Global Depth Offset Clamp (float)

// ------------------------------------------------------------------------------------------------
// 10.3.14 3DSTATE_SBE

static uint _3DSTATE_SBE                    = GFX_INSTR(0x3, 0x0, 0x1f, 0xc)

// ATTR_CONST
static uint ATTR_CONST_0000                 = 0x0
static uint ATTR_CONST_0001_FLOAT           = 0x1
static uint ATTR_CONST_1111_FLOAT           = 0x2
static uint ATTR_PRIM_ID                    = 0x3
static uint ATTR_CONST_MASK                 = 0x3

// ATTR_INPUT
static uint ATTR_INPUT                      = 0x0
static uint ATTR_INPUT_FACING               = 0x1
static uint ATTR_INPUT_W                    = 0x2
static uint ATTR_INPUT_FACING_W             = 0x3
static uint ATTR_INPUT_MASK                 = 0x3

// SBE_ATTR
static uint SBE_ATTR_OVERRIDE_W             = (1 << 15)
static uint SBE_ATTR_OVERRIDE_Z             = (1 << 14)
static uint SBE_ATTR_OVERRIDE_Y             = (1 << 13)
static uint SBE_ATTR_OVERRIDE_X             = (1 << 12)
static uint SBE_ATTR_CONST_SOUCE_SHIFT      9      ;     // ATTR_CONST
static uint SBE_ATTR_INPUT_SHIFT            6      ;     // ATTR_INPUT
static uint SBE_ATTR_SOURCE_INDEX_SHIFT     0
static uint SBE_ATTR_SOURCE_INDEX_MASK      0x1f

// DWORD 1
static uint SBE_ATTR_SWIZZLE_HIGH_BANK      = (1 << 28)
static uint SBE_SF_OUTPUT_COUNT_SHIFT       22
static uint SBE_SF_OUTPUT_COUNT_MASK        0x3f
static uint SBE_ATTR_SWIZZLE                = (1 << 21)
static uint SBE_POINT_SPRITE_ORIGIN_LL      = (1 << 20)
static uint SBE_URB_READ_LENGTH_SHIFT       11
static uint SBE_URB_READ_LENGTH_MASK        0x3f
static uint SBE_URB_READ_OFFSET_SHIFT       4
static uint SBE_URB_READ_OFFSET_MASK        0x3f

// DWORD 2-9 - 16 SBE_ATTR elements
// DWORD 10 - Point sprite Texture Coordinate Enable
// DWORD 11 - Constant Interpolation Enable
// DWORD 12 - WrapShortest Enables Attributes 0-7
// DWORD 13 - WrapShortest Enables Attributes 8-15

// ------------------------------------------------------------------------------------------------
// 10.3.15 SF_CLIP_VIEWPORT

typedef struct SFClipViewport
{
    float scaleX;
    float scaleY;
    float scaleZ;
    float transX;
    float transY;
    float transZ;
    float pad0[2];
    float guardbandMinX;       ;     // NDC space
    float guardbandMaxX;       ;     // NDC space
    float guardbandMinY;       ;     // NDC space
    float guardbandMaxY;       ;     // NDC space
    float pad1[4];
} SFClipViewport;

// ------------------------------------------------------------------------------------------------
// 10.3.16 SCISSOR_RECT

typedef struct ScissorRect
{
    u16 minX;
    u16 minY;
    u16 maxX;
    u16 maxY;
} ScissorRect;

// ------------------------------------------------------------------------------------------------
// 11.2.1 3DSTATE_WM

static uint _3DSTATE_WM                     = GFX_INSTR(0x3, 0x0, 0x14, 1)

// PSCDEPTH
static uint PSCDEPTH_OFF                    = 0x0
static uint PSCDEPTH_ON                     = 0x1
static uint PSCDEPTH_ON_GE                  = 0x2
static uint PSCDEPTH_ON_LE                  = 0x3
static uint PSCDEPTH_MASK                   = 0x3

// EDSC_MODE
static uint EDSC_NORMAL                     = 0x0
static uint EDSC_PSEXEC                     = 0x1
static uint EDSC_PREPS                      = 0x2
static uint EDSC_MASK                       = 0x3

// INTERP_MODE
static uint INTERP_PIXEL                    = 0x0
static uint INTERP_CENTROID                 = 0x2
static uint INTERP_SAMPLE                   = 0x3
static uint INTERP_MASK                     = 0x3

// AA_REGION
static uint AA_HALF_PIXEL                   = 0x0
static uint AA_ONE_PIXEL                    = 0x1
static uint AA_TWO_PIXELS                   = 0x2
static uint AA_FOUR_PIXELS                  = 0x3
static uint AA_MASK                         = 0x3

// DWORD 1
static uint WM_STATISTICS                   = (1 << 31)
static uint WM_DEPTH_BUFFER_CLEAR           = (1 << 30)
static uint WM_THREAD_DISPATCH              = (1 << 29)
static uint WM_DEPTH_BUFFER_RESOLVE         = (1 << 28)
static uint WM_HIER_DEPTH_BUFFER_RESOLVE    = (1 << 27)
static uint WM_LEGACY_DIAMOND_LINE          = (1 << 26)
static uint WM_PS_KILL_PIXEL                = (1 << 25)
static uint WM_PS_COMPUTED_DEPTH_SHIFT      23     ;     // PSCDEPTH
static uint WM_EARLY_DS_CONTROL_SHIFT       21     ;     // EDSC_MODE
static uint WM_PS_USES_SOURCE_DEPTH         = (1 << 20)
static uint WM_PS_USES_SOURCE_W             = (1 << 19)
static uint WM_POS_ZW_INTERP_MODE_SHIFT     17
static uint WM_PS_USES_NON_PERSP_SAMPLE     = (1 << 16)
static uint WM_PS_USES_NON_PERSP_CENTROID   (1 << 15)
static uint WM_PS_USES_NON_PERSP_LOC        = (1 << 14)
static uint WM_PS_USES_PERSP_SAMPLE         = (1 << 13)
static uint WM_PS_USES_PERSP_CENTROID       = (1 << 12)
static uint WM_PS_USES_PERSP_LOC            = (1 << 11)
static uint WM_PS_USES_COVERAGE_MASK        = (1 << 10)
static uint WM_LINE_END_CAP_AA_WIDTH_SHIFT  8      ;     // AA_REGION
static uint WM_LINE_AA_WIDTH_SHIFT          6      ;     // AA_REGION
static uint WM_POLYGON_STIPPLE              = (1 << 4)
static uint WM_LINE_STIPPLE                 = (1 << 3)
static uint WM_RASTRULE_UPPER_RIGHT         = (1 << 2)
static uint WM_MS_RASTERIZER                = (1 << 1)
static uint WM_MS_PATTERN                   = (1 << 0)

// DWORD 2
static uint WM_MS_PER_PIXEL                 = (1 << 31)

// ------------------------------------------------------------------------------------------------
// 11.2.2 3DSTATE_PS

static uint _3DSTATE_PS                     = GFX_INSTR(0x3, 0x0, 0x20, 6)

// ROUND_MODE
static uint ROUND_EVEN                      = 0x0
static uint ROUND_UP                        = 0x1
static uint ROUND_DOWN                      = 0x2
static uint ROUND_ZERO                      = 0x3
static uint ROUND_MASK                      = 0x3

// POSOFFSET_MODE
static uint POSOFFSET_NONE                  = 0x0
static uint POSOFFSET_CENTROID              = 0x2
static uint POSOFFSET_SAMPLE                = 0x3

// DWORD 1 - Kernel Start Pointer 0 (relative to Instruction Base Address)

// DWORD 2
static uint PS_SINGLE_PROGRAM_FLOW          = (1 << 31)
static uint PS_VECTOR_MASK                  = (1 << 30)
static uint PS_SAMPLER_COUNT_SHIFT          27     ;     // SAMPLER_USAGE
static uint PS_DENORMAL_RETAIN              = (1 << 26)
static uint PS_BINDING_COUNT_SHIFT          18
static uint PS_BINDING_COUNT_MASK           = 0xff
static uint PS_FLOATING_POINT_ALT           = (1 << 16)
static uint PS_ROUND_MODE_SHIFT             14
static uint PS_ILLEGAL_OPCODE_EX            = (1 << 13)
static uint PS_MASK_STACK_EX                = (1 << 11)
static uint PS_SOFTWARE_EX                  = (1 << 7)

// DWORD 3
static uint PS_SCRATCH_SPACE_BASE_SHIFT     = (1 << 10)
static uint PS_SCRATCH_SPACE_BASE_MASK      0x3fffff
static uint PS_PER_THREAD_SPACE_SHIFT       0      ;     // Power of 2 bytes + 1KB
static uint PS_PER_THREAD_SPACE_MASK        0xf

// DWORD 4
static uint PS_MAX_THREAD_SHIFT             24
static uint PS_MAX_THREAD_MASK              = 0xff
static uint PS_PUSH_CONSTANTS               = (1 << 11)
static uint PS_ATTRIBUTES                   = (1 << 10)
static uint PS_OUTPUT_MASK                  = (1 << 9)
static uint PS_RENDER_TARGET_FAST_CLEAR     = (1 << 8)
static uint PS_DUAL_SOUCE_BLEND             = (1 << 7)
static uint PS_RENDER_TARGET_RESOLVE        = (1 << 6)
static uint PS_POS_XY_OFFSET_SHIFT          3
static uint PS_DISPATCH32                   = (1 << 2)
static uint PS_DISPATCH16                   = (1 << 1)
static uint PS_DISPATCH8                    = (1 << 0)

// DWORD 5
static uint PS_DISPATCH_GRF_MASK            = 0x7f
static uint PS_DISPATCH0_GRF_SHIFT          16
static uint PS_DISPATCH1_GRF_SHIFT          8
static uint PS_DISPATCH2_GRF_SHIFT          0

// DWORD 6 - Kernel Start Pointer 1 (relative to Instruction Base Address)

// DWORD 7 - Kernel Start Pointer 2 (relative to Instruction Base Address)

// ------------------------------------------------------------------------------------------------
// 11.2.5 3DSTATE_SAMPLE_MASK

static uint _3DSTATE_SAMPLE_MASK            = GFX_INSTR(0x3, 0x0, 0x18, 0)

// DWORD 1 - Sample Mask (low-byte)

// ------------------------------------------------------------------------------------------------
// 11.3.2.2 3DSTATE_AA_LINE_PARAMS

static uint _3DSTATE_AA_LINE_PARAMS             = GFX_INSTR(0x3, 0x1, 0x0a, 1)

// DWORD 1
static uint AA_LINE_COVERAGE_BIAS_SHIFT         16
static uint AA_LINE_COVERAGE_BIAS_MASK          = 0xff
static uint AA_LINE_COVERAGE_SLOPE_SHIFT        0
static uint AA_LINE_COVERAGE_SLOPE_MASK         = 0xff

// DWORD 2
static uint AA_LINE_COVERAGE_ENDCAP_BIAS_SHIFT  16
static uint AA_LINE_COVERAGE_ENDCAP_BIAS_MASK   0xff
static uint AA_LINE_COVERAGE_ENDCAP_SLOPE_SHIFT 0
static uint AA_LINE_COVERAGE_ENDCAP_SLOPE_MASK  0xff

// ------------------------------------------------------------------------------------------------
// 11.3.2.4 3DSTATE_LINE_STIPPLE

static uint _3DSTATE_LINE_STIPPLE               = GFX_INSTR(0x3, 0x1, 0x08, 1)

// DWORD 1
static uint LINE_STIPPLE_MODIFY                 = (1 << 31)
static uint LINE_STIPPLE_REPEAT_COUNTER_SHIFT   21
static uint LINE_STIPPLE_REPEAT_COUNTER_MASK    0x1ff
static uint LINE_STIPPLE_CURRENT_INDEX_SHIFT    16
static uint LINE_STIPPLE_CURRENT_INDEX_MASK     0xf
static uint LINE_STIPPLE_PATTERN_SHIFT          0
static uint LINE_STIPPLE_PATTERN_MASK           = 0xffff

// DWORD 2
static uint LINE_STIPPLE_INV_REPEAT_COUNT_SHIFT 15
static uint LINE_STIPPLE_INV_REPEAT_COUNT_MASK  0x1ffff
static uint LINE_STIPPLE_REPEAT_COUNT_SHIFT     0
static uint LINE_STIPPLE_REPEAT_COUNT_MASK      0x1ff

// ------------------------------------------------------------------------------------------------
// 11.3.3.2 3DSTATE_POLY_STIPPLE_OFFSET

static uint _3DSTATE_POLY_STIPPLE_OFFSET    GFX_INSTR(0x3, 0x1, 0x06, 0)

// DWORD 1
static uint POLY_STIPPLE_X_OFFSET_SHIFT     8
static uint POLY_STIPPLE_X_OFFSET_MASK      0x1f
static uint POLY_STIPPLE_Y_OFFSET_SHIFT     0
static uint POLY_STIPPLE_Y_OFFSET_MASK      0x1f

// ------------------------------------------------------------------------------------------------
// 11.3.3.3 3DSTATE_POLY_STIPPLE_PATTERN

static uint _3DSTATE_POLY_STIPPLE_PATTERN(n)    GFX_INSTR(0x3, 0x1, 0x07, (n)-1)

// DWORD 1 - Stipple Pattern Row 1
// DWORD 2..32 - Stipple Pattern Rown 2-32

// ------------------------------------------------------------------------------------------------
// 11.4.2 3DSTATE_MULTISAMPLE

static uint _3DSTATE_MULTISAMPLE            = GFX_INSTR(0x3, 0x1, 0x0d, 2)

// MS_COUNT
static uint MS_COUNT_1                      = 0x0
static uint MS_COUNT_4                      = 0x2
static uint MS_COUNT_8                      = 0x3
static uint MS_COUNT_MASK                   = 0x7

// DWORD 1
static uint MS_PIXEL_UPPER_LEFT             = (1 << 4)
static uint MS_COUNT_SHIFT                  1

// DWORD 2
static uint MS_SAMPLE_OFFSET_MASK           = 0xf
static uint MS_SAMPLE3_X_OFFSET_SHIFT       28
static uint MS_SAMPLE3_Y_OFFSET_SHIFT       24
static uint MS_SAMPLE2_X_OFFSET_SHIFT       20
static uint MS_SAMPLE2_Y_OFFSET_SHIFT       16
static uint MS_SAMPLE1_X_OFFSET_SHIFT       12
static uint MS_SAMPLE1_Y_OFFSET_SHIFT       8
static uint MS_SAMPLE0_X_OFFSET_SHIFT       4
static uint MS_SAMPLE0_Y_OFFSET_SHIFT       0

// DWORD 3
static uint MS_SAMPLE7_X_OFFSET_SHIFT       28
static uint MS_SAMPLE7_Y_OFFSET_SHIFT       24
static uint MS_SAMPLE6_X_OFFSET_SHIFT       20
static uint MS_SAMPLE6_Y_OFFSET_SHIFT       16
static uint MS_SAMPLE5_X_OFFSET_SHIFT       12
static uint MS_SAMPLE5_Y_OFFSET_SHIFT       8
static uint MS_SAMPLE4_X_OFFSET_SHIFT       4
static uint MS_SAMPLE4_Y_OFFSET_SHIFT       0

// ------------------------------------------------------------------------------------------------
// 11.5.5.1 3DSTATE_DEPTH_BUFFER

static uint _3DSTATE_DEPTH_BUFFER               = GFX_INSTR(0x3, 0x0, 0x05, 0)

// DWORD 1
static uint DEPTH_BUF_SURFACE_TYPE_SHIFT        29     ;     // SURFTYPE
static uint DEPTH_BUF_DEPTH_WRITE               = (1 << 28)
static uint DEPTH_BUF_STENCIL_WRITE             = (1 << 27)
static uint DEPTH_BUF_HIER_BUFFER               = (1 << 22)
static uint DEPTH_BUF_SURFACE_FMT_SHIFT         18     ;     // DEPTH_FORMAT
static uint DEPTH_BUF_SURFACE_PITCH_SHIFT       0
static uint DEPTH_BUF_SURFACE_PITCH_MASK        0x3ffff

// DWORD 2 - Surface Base Address

// DWORD 3
static uint DEPTH_BUF_HEIGHT_SHIFT              18
static uint DEPTH_BUF_HEIGHT_MASK               = 0x3fff
static uint DEPTH_BUF_WIDTH_SHIFT               4
static uint DEPTH_BUF_WIDTH_MASK                = 0x3fff
static uint DEPTH_BUF_LOD_SHIFT                 0
static uint DEPTH_BUF_LOD_MASK                  = 0xf

// DWORD 4
static uint DEPTH_BUF_DEPTH_SHIFT               21
static uint DEPTH_BUF_DEPTH_MASK                = 0x7ff
static uint DEPTH_BUF_MIN_ELEMENT_SHIFT         10
static uint DEPTH_BUF_MIN_ELEMENT_MASK          = 0x7ff
static uint DEPTH_BUF_OBJ_CONTROL_STATE_SHIFT   0x0    ;     // MEMORY_OBJECT_CONTROL_STATE

// DWORD 5
static uint DEPTH_BUF_OFFSET_Y_SHIFT            16
static uint DEPTH_BUF_OFFSET_Y_MASK             = 0xffff
static uint DEPTH_BUF_OFFSET_X_SHIFT            0
static uint DEPTH_BUF_OFFSET_X_MASK             = 0xffff

// DWORD 6
static uint DEPTH_BUF_RT_VIEW_EXTENT_SHIFT      21
static uint DEPTH_BUF_RT_VIEW_EXTENT_MASK       0x7ff

// ------------------------------------------------------------------------------------------------
// 11.5.5.2 3DSTATE_STENCIL_BUFFER

static uint _3DSTATE_STENCIL_BUFFER             = GFX_INSTR(0x3, 0x0, 0x06, 1)

// DWORD 1
static uint STENCIL_BUF_OBJ_CONTROL_STATE_SHIFT 25     ;     // MEMORY_OBJECT_CONTROL_STATE
static uint STENCIL_BUF_SURFACE_PITCH_SHIFT     0
static uint STENCIL_BUF_SURFACE_PITCH_MASK      0xffff

// DWORD 2 - Surface Base Address

// ------------------------------------------------------------------------------------------------
// 11.5.5.3 3DSTATE_HIER_DEPTH_BUFFER

static uint _3DSTATE_HIER_DEPTH_BUFFER          = GFX_INSTR(0x3, 0x0, 0x07, 1)

// DWORD 1
static uint HDEPTH_BUF_OBJ_CONTRL_STATE_SHIFT   25     ;     // MEMORY_OBJECT_CONTROL_STATE
static uint HDEPTH_BUF_SURFACE_PITCH_SHIFT      0
static uint HDEPTH_BUF_SURFACE_PITCH_MASK       0xffff

// DWORD 2 - Surface Base Address

// ------------------------------------------------------------------------------------------------
// 11.5.5.4 3DSTATE_CLEAR_PARAMS

static uint _3DSTATE_CLEAR_PARAMS           = GFX_INSTR(0x3, 0x0, 0x04, 1)

// DWORD 1 - Depth Clear Value

// DWORD 2
static uint CLEAR_DEPTH_VALUE               = (1 << 0)

// ------------------------------------------------------------------------------------------------
// 12.2 Pixel Pipeline State

// COMPARE_FUNC
static uint COMPARE_FUNC_ALWAYS             = 0x0
static uint COMPARE_FUNC_NEVER              = 0x1
static uint COMPARE_FUNC_LT                 = 0x2
static uint COMPARE_FUNC_EQ                 = 0x3
static uint COMPARE_FUNC_LE                 = 0x4
static uint COMPARE_FUNC_GT                 = 0x5
static uint COMPARE_FUNC_NE                 = 0x6
static uint COMPARE_FUNC_GE                 = 0x7

// ------------------------------------------------------------------------------------------------
// 12.2.1 COLOR_CALC_STATE

// Flags
static uint CC_FRONT_FACE_STENCIL_REF_SHIFT 24
static uint CC_BACK_FACE_STENCIL_REF_SHIFT  16
static uint CC_STENCIL_REF_MASK             = 0xff
static uint CC_ROUND_DISABLE                = (1 << 15)
static uint CC_ALPHA_REF_FLOAT              = (1 << 0)

typedef struct ColorCalcState
{
    u32 flags;
    union ColorCalcState_AlphaRef
    {
        float floatVal;
        u32 intVal;    ;     // ref value stored in high byte
    } alphaRef;
    float constR;
    float constG;
    float constB;
    float constA;
} ColorCalcState;

// ------------------------------------------------------------------------------------------------
// 12.2.2 DEPTH_STENCIL_STATE

// STENCIL_OP
static uint STENCIL_OP_KEEP                     = 0x0
static uint STENCIL_OP_ZERO                     = 0x1
static uint STENCIL_OP_REPLACE                  = 0x2
static uint STENCIL_OP_INC_SAT                  = 0x3
static uint STENCIL_OP_DEC_SAT                  = 0x4
static uint STENCIL_OP_INC                      = 0x5
static uint STENCIL_OP_DEC                      = 0x6
static uint STENCIL_OP_INV                      = 0x7

// Stencil Flags
static uint STENCIL_ENABLE                      = (1 << 31)
static uint STENCIL_FUNC_SHIFT                  28     ;     // COMPARE_FUNC
static uint STENCIL_FAIL_OP_SHIFT               25     ;     // STENCIL_OP
static uint STENCIL_DEPTH_FAIL_OP_SHIFT         22     ;     // STENCIL_OP
static uint STENCIL_PASS_OP_SHIFT               19     ;     // STENCIL_OP
static uint STENCIL_BUFFER_WRITE                = (1 << 18)
static uint STENCIL_DOUBLE_SIDED                = (1 << 15)
static uint STENCIL_BACK_FUNC_SHIFT             12     ;     // COMPARE_FUNC
static uint STENCIL_BACK_FAIL_OP_SHIFT          9      ;     // STENCIL_OP
static uint STENCIL_BACK_DEPTH_FAIL_OP_SHIFT    6      ;     // STENCIL_OP
static uint STENCIL_BACK_PASS_OP_SHIFT          3      ;     // STENCIL_OP

// Stencil Masks
static uint STENCIL_TEST_MASK_SHIFT             24
static uint STENCIL_WRITE_MASK_SHIFT            16
static uint STENCIL_BACK_TEST_MASK_SHIFT        8
static uint STENCIL_BACK_WRITE_MASK_SHIFT       0

// Depth Flags
static uint DEPTH_TEST_ENABLE                   = (1 << 31)
static uint DEPTH_FUNC_SHIFT                    27     ;     // COMPARE_FUNC
static uint DEPTH_WRITE_ENABLE                  = (1 << 26)

typedef struct DepthStencilState
{
    u32 stencilFlags;
    u32 stencilMasks;
    u32 depthFlags;
} DepthStencilState;

// ------------------------------------------------------------------------------------------------
// 12.2.3 BLEND_STATE

// BLEND_FUNC
static uint BLEND_FUNC_ADD                  0
static uint BLEND_FUNC_SUB                  1
static uint BLEND_FUNC_REV_SUB              2
static uint BLEND_FUNC_MIN                  3
static uint BLEND_FUNC_MAX                  4
static uint BLEND_FUNC_MASK                 = 0x7

// BLEND_FACTOR
static uint BLEND_FACTOR_ONE                = 0x01
static uint BLEND_FACTOR_SRC_COLOR          = 0x02
static uint BLEND_FACTOR_SRC_ALPHA          = 0x03
static uint BLEND_FACTOR_DST_ALPHA          = 0x04
static uint BLEND_FACTOR_DST_COLOR          = 0x05
static uint BLEND_FACTOR_SRC_ALPHA_SAT      0x06
static uint BLEND_FACTOR_CONST_COLOR        0x07
static uint BLEND_FACTOR_CONST_ALPHA        0x08
static uint BLEND_FACTOR_SRC1_COLOR         = 0x09
static uint BLEND_FACTOR_SRC1_ALPHA         = 0x0a
static uint BLEND_FACTOR_ZERO               = 0x11
static uint BLEND_FACTOR_INV_SRC_COLOR      0x12
static uint BLEND_FACTOR_INV_SRC_ALPHA      0x13
static uint BLEND_FACTOR_INV_DST_ALPHA      0x14
static uint BLEND_FACTOR_INV_DST_COLOR      0x15
static uint BLEND_FACTOR_INV_CONST_COLOR    0x17
static uint BLEND_FACTOR_INV_CONST_ALPHA    0x18
static uint BLEND_FACTOR_INV_SRC1_COLOR     0x19
static uint BLEND_FACTOR_INV_SRC1_ALPHA     0x1a
static uint BLEND_FACTOR_MASK               = 0x1f

// LOGIC_OP
static uint LOGIC_OP_CLEAR                  = 0x0
static uint LOGIC_OP_NOR                    = 0x1
static uint LOGIC_OP_AND_INV                = 0x2
static uint LOGIC_OP_COPY_INV               = 0x3
static uint LOGIC_OP_AND_REV                = 0x4
static uint LOGIC_OP_INV                    = 0x5
static uint LOGIC_OP_XOR                    = 0x6
static uint LOGIC_OP_NAND                   = 0x7
static uint LOGIC_OP_AND                    = 0x8
static uint LOGIC_OP_EQUIV                  = 0x9
static uint LOGIC_OP_NOOP                   = 0xa
static uint LOGIC_OP_OR_INV                 = 0xb
static uint LOGIC_OP_COPY                   = 0xc
static uint LOGIC_OP_OR_REV                 = 0xd
static uint LOGIC_OP_OR                     = 0xe
static uint LOGIC_OP_SET                    = 0xf
static uint LOGIC_OP_MASK                   = 0xf

// COLOR_CLAMP
static uint COLOR_CLAMP_UNORM               = 0x0
static uint COLOR_CLAMP_SNORM               = 0x1
static uint COLOR_CLAMP_RTFORMAT            = 0x2
static uint COLOR_CLAMP_MASK                = 0x3

// Flags 0
static uint BLEND_COLOR                     = (1 << 31)   // Only BLEND_COLOR or BLEND_LOGIC can be enabled
static uint BLEND_INDEPENDENT_ALPHA         = (1 << 30)
static uint BLEND_FUNC_ALPHA_SHIFT          26     ;     // BLEND_FUNC
static uint BLEND_SRC_ALPHA_SHIFT           20     ;     // BLEND_FACTOR
static uint BLEND_DST_ALPHA_SHIFT           15     ;     // BLEND_FACTOR
static uint BLEND_FUNC_COLOR_SHIFT          11     ;     // BLEND_FUNC
static uint BLEND_SRC_COLOR_SHIFT           5      ;     // BLEND_FACTOR
static uint BLEND_DST_COLOR_SHIFT           0      ;     // BLEND_FACTOR

// Flags 1
static uint BLEND_ALPHA_TO_COVERAGE         = (1 << 31)
static uint BLEND_ALPHA_TO_ONE              = (1 << 30)   // Errata - must be disabled
static uint BLEND_ALPHA_TO_COVERAGE_DITHER  (1 << 29)
static uint BLEND_DISABLE_ALPHA             = (1 << 27)   // Errata - must be set to 0 if not present in the render target
static uint BLEND_DISABLE_RED               = (1 << 26)   // Errata - must be set to 0 if not present in the render target
static uint BLEND_DISABLE_GREEN             = (1 << 25)   // Errata - must be set to 0 if not present in the render target
static uint BLEND_DISABLE_BLUE              = (1 << 24)   // Errata - must be set to 0 if not present in the render target
static uint BLEND_LOGIC                     = (1 << 22)   // Only BLEND_COLOR or BLEND_LOGIC can be enabled
static uint BLEND_LOGIC_OP_SHIFT            18     ;     // LOGIC_OP
static uint BLEND_ALPHA_TEST                = (1 << 16)
static uint BLEND_ALPHA_FUNC                = (1 << 13)   // COMPARE_FUNC
static uint BLEND_COLOR_DITHER              = (1 << 12)
static uint BLEND_DITHER_X_SHIFT            10
static uint BLEND_DITHER_Y_SHIFT            8
static uint BLEND_COLOR_CLAMP_RANGE_SHIFT   2
static uint BLEND_PRE_COLOR_CLAMP           = (1 << 1)
static uint BLEND_POST_COLOR_CLAMP          = (1 << 0)

typedef struct BlendState
{
    u32 flags0;
    u32 flags1;
} BlendState;

// ------------------------------------------------------------------------------------------------
// 12.2.4 CC_VIEWPORT

typedef struct CCViewport
{
    float minDepth;
    float maxDepth;
} CCViewport;

// ------------------------------------------------------------------------------------------------
// Vol 3. Part 1. VGA and Extended VGA Registers
// ------------------------------------------------------------------------------------------------

// ------------------------------------------------------------------------------------------------
// 1.2.1 Sequencer Index

static uint SR_INDEX                        = 0x3c4
static uint SR_DATA                         = 0x3c5

// ------------------------------------------------------------------------------------------------
// 1.2.3 Clocking Mode

static uint SEQ_CLOCKING                    = 0x01
static uint SCREEN_OFF                      = (1 << 5)

// ------------------------------------------------------------------------------------------------
// Vol 3. Part 2. PCI Registers
// ------------------------------------------------------------------------------------------------

// ------------------------------------------------------------------------------------------------
// 1.25 MGGC0 - Mirror of GMCH Graphics Control Register

static uint MGGC0                           = 0x50   ;     // In PCI Config Space

static uint GGC_LOCK                        = (1 << 0)
static uint GGC_IVD                         = (1 << 1)    // IGD VGA Disable
static uint GGC_GMS_SHIFT                   3      ;     // Graphics Mode Select
static uint GGC_GMS_MASK                    = 0x1f
static uint GGC_GGMS_SHIFT                  8      ;     // GTT Graphics Memory Size
static uint GGC_GGMS_MASK                   = 0x3
static uint GGC_VAMEN                       = (1 << 14)   // Versatile Acceleration Mode Enable

// This matches the IVB graphics documentation, not the IVB CPU documentation
static uint GMS_32MB                        = 0x05
static uint GMS_48MB                        = 0x06
static uint GMS_64MB                        = 0x07
static uint GMS_128MB                       = 0x08
static uint GMS_256MB                       = 0x09
static uint GMS_96MB                        = 0x0A
static uint GMS_160MB                       = 0x0B
static uint GMS_224MB                       = 0x0C
static uint GMS_352MB                       = 0x0D
static uint GMS_0MB                         = 0x00
static uint GMS_32MB_1                      = 0x01
static uint GMS_64MB_1                      = 0x02
static uint GMS_96MB_1                      = 0x03
static uint GMS_128MB_1                     = 0x04
static uint GMS_448MB                       = 0x0E
static uint GMS_480MB                       = 0x0F
static uint GMS_512MB                       = 0x10

static uint GGMS_None                       = 0x00
static uint GGMS_1MB                        = 0x01
static uint GGMS_2MB                        = 0x02

// ------------------------------------------------------------------------------------------------
// 1.27 BDSM - Base Data of Stolen Memory

static uint BDSM                            = 0x5C // In PCI Config Space

static uint BDSM_LOCK                       = (1 << 0)
static uint BDSM_ADDR_MASK                  (0xfff << 20)

// ------------------------------------------------------------------------------------------------
// 1.45 ASLS - ASL Storage
// Software scratch register (BIOS sets the opregion address in here)

static uint ASLS                            = 0xFC // In PCI Config Space

// ------------------------------------------------------------------------------------------------
// Vol 3. Part 3. North Display Engine Registers
// ------------------------------------------------------------------------------------------------

// ------------------------------------------------------------------------------------------------
// 3.1.1 VGA Control

static uint VGA_CONTROL                     = 0x41000;     // R/W

static uint VGA_DISABLE                     = (1 << 31)

// ------------------------------------------------------------------------------------------------
// 3.7.1 ARB_CTL-Display Arbitration Control 1

static uint ARB_CTL                         = 0x45000;     // R/W

static uint ARB_CTL_HP_DATA_REQUEST_LIMIT_MASK          = 0x7f
static uint ARB_CTL_HP_PAGE_BREAK_LIMIT_SHIFT           8
static uint ARB_CTL_HP_PAGE_BREAK_LIMIT_MASK            = 0x1f
static uint ARB_CTL_TILED_ADDRESS_SWIZZLING             = (1 << 13)
static uint ARB_CTL_TLB_REQUEST_IN_FLIGHT_LIMIT_SHIFT   16
static uint ARB_CTL_TLB_REQUEST_IN_FLIGHT_LIMIT_MASK    0x7
static uint ARB_CTL_TLB_REQUEST_LIMIT_SHIFT             20
static uint ARB_CTL_TLB_REQUEST_LIMIT_MASK              = 0x7
static uint ARB_CTL_LP_WRITE_REQUEST_LIMIT_SHIFT        24
static uint ARB_CTL_LP_WRITE_REQUEST_LIMIT_MASK         = 0x3
static uint ARB_CTL_HP_QUEUE_WATERMARK_SHIFT            26
static uint ARB_CTL_HP_QUEUE_WATERMARK_MASK             = 0x7

// ------------------------------------------------------------------------------------------------
// 4.1.1 Horizontal Total

static uint PIPE_HTOTAL_A                   = 0x60000;     // R/W
static uint PIPE_HTOTAL_B                   = 0x61000;     // R/W
static uint PIPE_HTOTAL_C                   = 0x62000;     // R/W

// ------------------------------------------------------------------------------------------------
// 4.1.2 Horizontal Blank

static uint PIPE_HBLANK_A                   = 0x60004;     // R/W
static uint PIPE_HBLANK_B                   = 0x61004;     // R/W
static uint PIPE_HBLANK_C                   = 0x62004;     // R/W

// ------------------------------------------------------------------------------------------------
// 4.1.3 Horizontal Sync

static uint PIPE_HSYNC_A                    = 0x60008;     // R/W
static uint PIPE_HSYNC_B                    = 0x61008;     // R/W
static uint PIPE_HSYNC_C                    = 0x62008;     // R/W

// ------------------------------------------------------------------------------------------------
// 4.1.4 Vertical Total

static uint PIPE_VTOTAL_A                   = 0x6000c;     // R/W
static uint PIPE_VTOTAL_B                   = 0x6100c;     // R/W
static uint PIPE_VTOTAL_C                   = 0x6200c;     // R/W

// ------------------------------------------------------------------------------------------------
// 4.1.5 Vertical Blank

static uint PIPE_VBLANK_A                   = 0x60010;     // R/W
static uint PIPE_VBLANK_B                   = 0x61010;     // R/W
static uint PIPE_VBLANK_C                   = 0x62010;     // R/W

// ------------------------------------------------------------------------------------------------
// 4.1.6 Vertical Sync

static uint PIPE_VSYNC_A                    = 0x60014;     // R/W
static uint PIPE_VSYNC_B                    = 0x61014;     // R/W
static uint PIPE_VSYNC_C                    = 0x62014;     // R/W

// ------------------------------------------------------------------------------------------------
// 4.1.7 Source Image Size

static uint PIPE_SRCSZ_A                    = 0x6001c;     // R/W
static uint PIPE_SRCSZ_B                    = 0x6101c;     // R/W
static uint PIPE_SRCSZ_C                    = 0x6201c;     // R/W

// ------------------------------------------------------------------------------------------------
// 5.1.3 Pipe Configuration

static uint PIPE_CONF_A                     = 0x70008;     // R/W
static uint PIPE_CONF_B                     = 0x71008;     // R/W
static uint PIPE_CONF_C                     = 0x72008;     // R/W

// ------------------------------------------------------------------------------------------------
// 5.2.1 Cursor Control

static uint CUR_CTL_A                       = 0x70080;     // R/W
static uint CUR_CTL_B                       = 0x71080;     // R/W
static uint CUR_CTL_C                       = 0x72080;     // R/W

static uint CUR_GAMMA_ENABLE                = (1 << 26)   // Gamma Enable
static uint CUR_MODE_ARGB                   = (1 << 5)    // 32bpp ARGB
static uint CUR_MODE_64_32BPP               (7 << 0)    // 64 x 64 32bpp

// ------------------------------------------------------------------------------------------------
// 5.2.2 Cursor Base

static uint CUR_BASE_A                      = 0x70084;     // R/W
static uint CUR_BASE_B                      = 0x71084;     // R/W
static uint CUR_BASE_C                      = 0x72084;     // R/W

// ------------------------------------------------------------------------------------------------
// 5.2.3 Cursor Position

static uint CUR_POS_A                       = 0x70088;     // R/W
static uint CUR_POS_B                       = 0x71088;     // R/W
static uint CUR_POS_C                       = 0x72088;     // R/W

// ------------------------------------------------------------------------------------------------
// 5.3.1 Primary Control

static uint PRI_CTL_A                       = 0x70180;     // R/W
static uint PRI_CTL_B                       = 0x71180;     // R/W
static uint PRI_CTL_C                       = 0x72180;     // R/W

static uint PRI_PLANE_ENABLE                = (1 << 31)
static uint PRI_PLANE_32BPP                 (6 << 26)

// ------------------------------------------------------------------------------------------------
// 5.3.2 Primary Linear Offset

static uint PRI_LINOFF_A                    = 0x70184;     // R/W
static uint PRI_LINOFF_B                    = 0x71184;     // R/W
static uint PRI_LINOFF_C                    = 0x72184;     // R/W

// ------------------------------------------------------------------------------------------------
// 5.3.3 Primary Stride

static uint PRI_STRIDE_A                    = 0x70188;     // R/W
static uint PRI_STRIDE_B                    = 0x71188;     // R/W
static uint PRI_STRIDE_C                    = 0x72188;     // R/W

// ------------------------------------------------------------------------------------------------
// 5.3.4 Primary Surface Base Address

static uint PRI_SURF_A                      = 0x7019c;     // R/W
static uint PRI_SURF_B                      = 0x7119c;     // R/W
static uint PRI_SURF_C                      = 0x7219c;     // R/W

// ------------------------------------------------------------------------------------------------
// 5.3.5 Primary Tiled Offset

static uint PRI_TILEOFF_A                   = 0x701a4;     // R/W
static uint PRI_TILEOFF_B                   = 0x711a4;     // R/W
static uint PRI_TILEOFF_C                   = 0x721a4;     // R/W

// ------------------------------------------------------------------------------------------------
// Vol 3. Part 4. South Display Engine Registers
// ------------------------------------------------------------------------------------------------

// ------------------------------------------------------------------------------------------------
// 3.6.1 HDMI Port Control

static uint HDMI_CTL_B                      = 0xe1140;     // R/W
static uint HDMI_CTL_C                      = 0xe1150;     // R/W
static uint HDMI_CTL_D                      = 0xe1160;     // R/W

static uint PORT_DETECTED                   = (1 << 2)    // RO

// ------------------------------------------------------------------------------------------------
// Vol 4. Part 1. Subsystems ad Cores - Shared Functions
// ------------------------------------------------------------------------------------------------

// ------------------------------------------------------------------------------------------------
// 2.12.2 Surface State

// SURFTYPE
static uint SURFTYPE_1D                     = 0x0
static uint SURFTYPE_2D                     = 0x1
static uint SURFTYPE_3D                     = 0x2
static uint SURFTYPE_CUBE                   = 0x3
static uint SURFTYPE_BUFFER                 = 0x4
static uint SURFTYPE_STRBUF                 = 0x5
static uint SURFTYPE_NULL                   = 0x7
static uint SURFTYPE_MASK                   = 0x7

// FMT (SURFACE_FORMAT)
// VF = can be used by VF Unit
static uint FMT_R32G32B32A32_FLOAT          = 0x000  ;     // VF
static uint FMT_R32G32B32A32_SINT           = 0x001  ;     // VF
static uint FMT_R32G32B32A32_UINT           = 0x002  ;     // VF
static uint FMT_R32G32B32A32_UNORM          = 0x003  ;     // VF
static uint FMT_R32G32B32A32_SNORM          = 0x004  ;     // VF
static uint FMT_R64G64_FLOAT                = 0x005  ;     // VF
static uint FMT_R32G32B32X32_FLOAT          = 0x006
static uint FMT_R32G32B32A32_SSCALED        0x007  ;     // VF
static uint FMT_R32G32B32A32_USCALED        0x008  ;     // VF
static uint FMT_R32G32B32A32_SFIXED         = 0x020
static uint FMT_R64G64_PASSTHRU             = 0x021
static uint FMT_R32G32B32_FLOAT             = 0x040  ;     // VF
static uint FMT_R32G32B32_SINT              = 0x041  ;     // VF
static uint FMT_R32G32B32_UINT              = 0x042  ;     // VF
static uint FMT_R32G32B32_UNORM             = 0x043  ;     // VF
static uint FMT_R32G32B32_SNORM             = 0x044  ;     // VF
static uint FMT_R32G32B32_SSCALED           = 0x045  ;     // VF
static uint FMT_R32G32B32_USCALED           = 0x046  ;     // VF
static uint FMT_R32G32B32_SFIXED            = 0x050
static uint FMT_R16G16B16A16_UNORM          = 0x080  ;     // VF
static uint FMT_R16G16B16A16_SNORM          = 0x081  ;     // VF
static uint FMT_R16G16B16A16_SINT           = 0x082  ;     // VF
static uint FMT_R16G16B16A16_UINT           = 0x083  ;     // VF
static uint FMT_R16G16B16A16_FLOAT          = 0x084  ;     // VF
static uint FMT_R32G32_FLOAT                = 0x085  ;     // VF
static uint FMT_R32G32_SINT                 = 0x086  ;     // VF
static uint FMT_R32G32_UINT                 = 0x087  ;     // VF
static uint FMT_R32_FLOAT_X8X24_TYPELESS    0x088
static uint FMT_X32_TYPELESS_G8X24_UINT     0x089
static uint FMT_L32A32_FLOAT                = 0x08A
static uint FMT_R32G32_UNORM                = 0x08B  ;     // VF
static uint FMT_R32G32_SNORM                = 0x08C  ;     // VF
static uint FMT_R64_FLOAT                   = 0x08D  ;     // VF
static uint FMT_R16G16B16X16_UNORM          = 0x08E
static uint FMT_R16G16B16X16_FLOAT          = 0x08F
static uint FMT_A32X32_FLOAT                = 0x090
static uint FMT_L32X32_FLOAT                = 0x091
static uint FMT_I32X32_FLOAT                = 0x092
static uint FMT_R16G16B16A16_SSCALED        0x093  ;     // VF
static uint FMT_R16G16B16A16_USCALED        0x094  ;     // VF
static uint FMT_R32G32_SSCALED              = 0x095  ;     // VF
static uint FMT_R32G32_USCALED              = 0x096  ;     // VF
static uint FMT_R32G32_SFIXED               = 0x0A0
static uint FMT_R64_PASSTHRU                = 0x0A1
static uint FMT_B8G8R8A8_UNORM              = 0x0C0  ;     // VF
static uint FMT_B8G8R8A8_UNORM_SRGB         = 0x0C1
static uint FMT_R10G10B10A2_UNORM           = 0x0C2  ;     // VF
static uint FMT_R10G10B10A2_UNORM_SRGB      0x0C3
static uint FMT_R10G10B10A2_UINT            = 0x0C4  ;     // VF
static uint FMT_R10G10B10_SNORM_A2_UNORM    0x0C5  ;     // VF
static uint FMT_R8G8B8A8_UNORM              = 0x0C7  ;     // VF
static uint FMT_R8G8B8A8_UNORM_SRGB         = 0x0C8
static uint FMT_R8G8B8A8_SNORM              = 0x0C9  ;     // VF
static uint FMT_R8G8B8A8_SINT               = 0x0CA  ;     // VF
static uint FMT_R8G8B8A8_UINT               = 0x0CB  ;     // VF
static uint FMT_R16G16_UNORM                = 0x0CC  ;     // VF
static uint FMT_R16G16_SNORM                = 0x0CD  ;     // VF
static uint FMT_R16G16_SINT                 = 0x0CE  ;     // VF
static uint FMT_R16G16_UINT                 = 0x0CF  ;     // VF
static uint FMT_R16G16_FLOAT                = 0x0D0  ;     // VF
static uint FMT_B10G10R10A2_UNORM           = 0x0D1
static uint FMT_B10G10R10A2_UNORM_SRGB      0x0D2
static uint FMT_R11G11B10_FLOAT             = 0x0D3  ;     // VF
static uint FMT_R32_SINT                    = 0x0D6  ;     // VF
static uint FMT_R32_UINT                    = 0x0D7  ;     // VF
static uint FMT_R32_FLOAT                   = 0x0D8  ;     // VF
static uint FMT_R24_UNORM_X8_TYPELESS       0x0D9
static uint FMT_X24_TYPELESS_G8_UINT        0x0DA
static uint FMT_L32_UNORM                   = 0x0DD
static uint FMT_A32_UNORM                   = 0x0DE
static uint FMT_L16A16_UNORM                = 0x0DF
static uint FMT_I24X8_UNORM                 = 0x0E0
static uint FMT_L24X8_UNORM                 = 0x0E1
static uint FMT_A24X8_UNORM                 = 0x0E2
static uint FMT_I32_FLOAT                   = 0x0E3
static uint FMT_L32_FLOAT                   = 0x0E4
static uint FMT_A32_FLOAT                   = 0x0E5
static uint FMT_X8B8_UNORM_G8R8_SNORM       0x0E6
static uint FMT_A8X8_UNORM_G8R8_SNORM       0x0E7
static uint FMT_B8X8_UNORM_G8R8_SNORM       0x0E8
static uint FMT_B8G8R8X8_UNORM              = 0x0E9
static uint FMT_B8G8R8X8_UNORM_SRGB         = 0x0EA
static uint FMT_R8G8B8X8_UNORM              = 0x0EB
static uint FMT_R8G8B8X8_UNORM_SRGB         = 0x0EC
static uint FMT_R9G9B9E5_SHAREDEXP          = 0x0ED
static uint FMT_B10G10R10X2_UNORM           = 0x0EE
static uint FMT_L16A16_FLOAT                = 0x0F0
static uint FMT_R32_UNORM                   = 0x0F1  ;     // VF
static uint FMT_R32_SNORM                   = 0x0F2  ;     // VF
static uint FMT_R10G10B10X2_USCALED         = 0x0F3  ;     // VF
static uint FMT_R8G8B8A8_SSCALED            = 0x0F4  ;     // VF
static uint FMT_R8G8B8A8_USCALED            = 0x0F5  ;     // VF
static uint FMT_R16G16_SSCALED              = 0x0F6  ;     // VF
static uint FMT_R16G16_USCALED              = 0x0F7  ;     // VF
static uint FMT_R32_SSCALED                 = 0x0F8  ;     // VF
static uint FMT_R32_USCALED                 = 0x0F9  ;     // VF
static uint FMT_B5G6R5_UNORM                = 0x100
static uint FMT_B5G6R5_UNORM_SRGB           = 0x101
static uint FMT_B5G5R5A1_UNORM              = 0x102
static uint FMT_B5G5R5A1_UNORM_SRGB         = 0x103
static uint FMT_B4G4R4A4_UNORM              = 0x104
static uint FMT_B4G4R4A4_UNORM_SRGB         = 0x105
static uint FMT_R8G8_UNORM                  = 0x106  ;     // VF
static uint FMT_R8G8_SNORM                  = 0x107  ;     // VF
static uint FMT_R8G8_SINT                   = 0x108  ;     // VF
static uint FMT_R8G8_UINT                   = 0x109  ;     // VF
static uint FMT_R16_UNORM                   = 0x10A  ;     // VF
static uint FMT_R16_SNORM                   = 0x10B  ;     // VF
static uint FMT_R16_SINT                    = 0x10C  ;     // VF
static uint FMT_R16_UINT                    = 0x10D  ;     // VF
static uint FMT_R16_FLOAT                   = 0x10E  ;     // VF
static uint FMT_A8P8_UNORM_PALETTE0         = 0x10F
static uint FMT_A8P8_UNORM_PALETTE1         = 0x110
static uint FMT_I16_UNORM                   = 0x111
static uint FMT_L16_UNORM                   = 0x112
static uint FMT_A16_UNORM                   = 0x113
static uint FMT_L8A8_UNORM                  = 0x114
static uint FMT_I16_FLOAT                   = 0x115
static uint FMT_L16_FLOAT                   = 0x116
static uint FMT_A16_FLOAT                   = 0x117
static uint FMT_L8A8_UNORM_SRGB             = 0x118
static uint FMT_R5G5_SNORM_B6_UNORM         = 0x119
static uint FMT_B5G5R5X1_UNORM              = 0x11A
static uint FMT_B5G5R5X1_UNORM_SRGB         = 0x11B
static uint FMT_R8G8_SSCALED                = 0x11C  ;     // VF
static uint FMT_R8G8_USCALED                = 0x11D  ;     // VF
static uint FMT_R16_SSCALED                 = 0x11E  ;     // VF
static uint FMT_R16_USCALED                 = 0x11F  ;     // VF
static uint FMT_P8A8_UNORM_PALETTE0         = 0x122
static uint FMT_P8A8_UNORM_PALETTE1         = 0x123
static uint FMT_A1B5G5R5_UNORM              = 0x124
static uint FMT_A4B4G4R4_UNORM              = 0x125
static uint FMT_L8A8_UINT                   = 0x126
static uint FMT_L8A8_SINT                   = 0x127
static uint FMT_R8_UNORM                    = 0x140  ;     // VF
static uint FMT_R8_SNORM                    = 0x141  ;     // VF
static uint FMT_R8_SINT                     = 0x142
static uint FMT_R8_UINT                     = 0x143
static uint FMT_A8_UNORM                    = 0x144
static uint FMT_I8_UNORM                    = 0x145
static uint FMT_L8_UNORM                    = 0x146
static uint FMT_P4A4_UNORM_PALETTE0         = 0x147
static uint FMT_A4P4_UNORM_PALETTE0         = 0x148
static uint FMT_R8_SSCALED                  = 0x149  ;     // VF
static uint FMT_R8_USCALED                  = 0x14A  ;     // VF
static uint FMT_P8_UNORM_PALETTE0           = 0x14B
static uint FMT_L8_UNORM_SRGB               = 0x14C
static uint FMT_P8_UNORM_PALETTE1           = 0x14D
static uint FMT_P4A4_UNORM_PALETTE1         = 0x14E
static uint FMT_A4P4_UNORM_PALETTE1         = 0x14F
static uint FMT_Y8_UNORM                    = 0x150
static uint FMT_L8_UINT                     = 0x152
static uint FMT_L8_SINT                     = 0x153
static uint FMT_I8_UINT                     = 0x154
static uint FMT_I8_SINT                     = 0x155
static uint FMT_DXT1_RGB_SRGB               = 0x180
static uint FMT_R1_UNORM                    = 0x181
static uint FMT_YCRCB_NORMAL                = 0x182
static uint FMT_YCRCB_SWAPUVY               = 0x183
static uint FMT_P2_UNORM_PALETTE0           = 0x184
static uint FMT_P2_UNORM_PALETTE1           = 0x185
static uint FMT_BC1_UNORM                   = 0x186
static uint FMT_BC2_UNORM                   = 0x187
static uint FMT_BC3_UNORM                   = 0x188
static uint FMT_BC4_UNORM                   = 0x189
static uint FMT_BC5_UNORM                   = 0x18A
static uint FMT_BC1_UNORM_SRGB              = 0x18B
static uint FMT_BC2_UNORM_SRGB              = 0x18C
static uint FMT_BC3_UNORM_SRGB              = 0x18D
static uint FMT_MONO8                       = 0x18E
static uint FMT_YCRCB_SWAPUV                = 0x18F
static uint FMT_YCRCB_SWAPY                 = 0x190
static uint FMT_DXT1_RGB                    = 0x191
static uint FMT_FXT1                        = 0x192
static uint FMT_R8G8B8_UNORM                = 0x193  ;     // VF
static uint FMT_R8G8B8_SNORM                = 0x194  ;     // VF
static uint FMT_R8G8B8_SSCALED              = 0x195  ;     // VF
static uint FMT_R8G8B8_USCALED              = 0x196  ;     // VF
static uint FMT_R64G64B64A64_FLOAT          = 0x197  ;     // VF
static uint FMT_R64G64B64_FLOAT             = 0x198  ;     // VF
static uint FMT_BC4_SNORM                   = 0x199
static uint FMT_BC5_SNORM                   = 0x19A
static uint FMT_R16G16B16_FLOAT             = 0x19B  ;     // VF
static uint FMT_R16G16B16_UNORM             = 0x19C  ;     // VF
static uint FMT_R16G16B16_SNORM             = 0x19D  ;     // VF
static uint FMT_R16G16B16_SSCALED           = 0x19E  ;     // VF
static uint FMT_R16G16B16_USCALED           = 0x19F  ;     // VF
static uint FMT_BC6H_SF16                   = 0x1A1
static uint FMT_BC7_UNORM                   = 0x1A2
static uint FMT_BC7_UNORM_SRGB              = 0x1A3
static uint FMT_BC6H_UF16                   = 0x1A4
static uint FMT_PLANAR_420_8                = 0x1A5
static uint FMT_R8G8B8_UNORM_SRGB           = 0x1A8
static uint FMT_ETC1_RGB8                   = 0x1A9
static uint FMT_ETC2_RGB8                   = 0x1AA
static uint FMT_EAC_R11                     = 0x1AB
static uint FMT_EAC_RG11                    = 0x1AC
static uint FMT_EAC_SIGNED_R11              = 0x1AD
static uint FMT_EAC_SIGNED_RG11             = 0x1AE
static uint FMT_ETC2_SRGB8                  = 0x1AF
static uint FMT_R16G16B16_UINT              = 0x1B0
static uint FMT_R16G16B16_SINT              = 0x1B1
static uint FMT_R32_SFIXED                  = 0x1B2
static uint FMT_R10G10B10A2_SNORM           = 0x1B3
static uint FMT_R10G10B10A2_USCALED         = 0x1B4
static uint FMT_R10G10B10A2_SSCALED         = 0x1B5
static uint FMT_R10G10B10A2_SINT            = 0x1B6
static uint FMT_B10G10R10A2_SNORM           = 0x1B7
static uint FMT_B10G10R10A2_USCALED         = 0x1B8
static uint FMT_B10G10R10A2_SSCALED         = 0x1B9
static uint FMT_B10G10R10A2_UINT            = 0x1BA
static uint FMT_B10G10R10A2_SINT            = 0x1BB
static uint FMT_R64G64B64A64_PASSTHRU       0x1BC
static uint FMT_R64G64B64_PASSTHRU          = 0x1BD
static uint FMT_ETC2_RGB8_PTA               = 0x1C0
static uint FMT_ETC2_SRGB8_PTA              = 0x1C1
static uint FMT_ETC2_EAC_RGBA8              = 0x1C2
static uint FMT_ETC2_EAC_SRGB8_A8           = 0x1C3
static uint FMT_R8G8B8_UINT                 = 0x1C8
static uint FMT_R8G8B8_SINT                 = 0x1C9
static uint FMT_RAW                         = 0x1FF

// MEDIA_BOUNDARY
static uint MEDIA_BOUNDARY_NORMAL           = 0x0
static uint MEDIA_BOUNDARY_PROGRESSIVE      0x2
static uint MEDIA_BOUNDARY_INTERLACED       0x3
static uint MEDIA_BOUNDARY_MASK             = 0x3

// RTROTATE
static uint RTROTATE_0                      = 0x0
static uint RTROTATE_90                     = 0x1
static uint RTROTATE_270                    = 0x3
static uint RTROTATE_MASK                   = 0x3

// MULTISAMPLECOUNT
static uint MULTISAMPLECOUNT_1              = 0x0
static uint MULTISAMPLECOUNT_4              = 0x2
static uint MULTISAMPLECOUNT_8              = 0x3
static uint MULTISAMPLECOUNT_MASK           = 0x7

// Flags 0
static uint SURFACE_TYPE_SHIFT              29     ;     // SURFTYPE
static uint SURFACE_ARRAY                   = (1 << 28)
static uint SURFACE_FORMAT_SHIFT            18     ;     // SURFACE_FORMAT
static uint SURFACE_VERT_ALIGN_SHIFT        16     ;     // Values not doucmented
static uint SURFACE_HALIGN_8                = (1 << 15)
static uint SURFACE_TILED                   = (1 << 14)
static uint SURFACE_TILE_YMAJOR             = (1 << 13)
static uint SURFACE_VERT_LINE_STRIDE        = (1 << 12)
static uint SURFACE_VERT_LINE_STRIDE_OFFSET (1 << 11)
static uint SURFACE_ARRAY_LOD0              = (1 << 10)
static uint SURFACE_RENDER_CACHE_RW         = (1 << 8)
static uint SURFACE_MEDIA_BOUNDARY_SHIFT    6      ;     // MEDIA_BOUNDARY
static uint SURFACE_CUBE_NEG_X              = (1 << 5)
static uint SURFACE_CUBE_POS_X              = (1 << 4)
static uint SURFACE_CUBE_NEG_Y              = (1 << 3)
static uint SURFACE_CUBE_POS_Y              = (1 << 2)
static uint SURFACE_CUBE_NEG_Z              = (1 << 1)
static uint SURFACE_CUBE_POS_Z              = (1 << 0)

// Pitch/Depth
static uint SURFACE_DEPTH_SHIFT             21
static uint SURFACE_DEPTH_MASK              = 0x3ff
static uint SURFACE_PITCH_SHIFT             0
static uint SURFACE_PITCH_MASK              = 0x3ffff

// Flags 1 - minimum array element for SURFTYPE_STRBUF
static uint SURFACE_ROTATION_SHIFT          29     ;     // RTROTATE
static uint SURFACE_MIN_ELEMENT_SHIFT       18
static uint SURFACE_MIN_ELEMENT_MASK        0x7ff
static uint SURFACE_RT_VIEW_EXTENT_SHIFT    7
static uint SURFACE_RT_VIEW_EXTENT_MASK     0x7ff
static uint SURFACE_MS_DEPTH                = (1 << 6)
static uint SURFACE_MS_COUNT_SHIFT          3      ;     // MULTISAMPLECOUNT
static uint SURFACE_MS_PALETTE_INDEX_SHIFT  0
static uint SURFACE_MS_PALETTE_INDEX_MASK   0x7

// Flags 2
static uint SURFACE_X_OFFSET_SHIFT          25
static uint SURFACE_X_OFFSET_MASK           = 0x7f
static uint SURFACE_Y_OFFSET_SHIFT          20
static uint SURFACE_Y_OFFSET_MASK           = 0xf
static uint SURFACE_OBJ_CONTROL_STATE_SHIFT 16     ;     // MEMORY_OBJECT_CONTROL_STATE
static uint SURFACE_MIN_LOD_SHIFT           4
static uint SURFACE_MIN_LOD_MASK            = 0xf
static uint SURFACE_MIP_COUNT_SHIFT         0
static uint SURFACE_MIP_COUNT_MASK          = 0xf

// Flags 3 - Surface Format == PLANAR
static uint SURFACE_X_OFFSET_UV_SHIFT       16
static uint SURFACE_X_OFFSET_UV_MASK        0x3fff
static uint SURFACE_Y_OFFSET_UV_SHIFT       0
static uint SURFACE_Y_OFFSET_UV_MASK        0x3fff

// Flags 3 - Surface Format != PLANAR && SURFACE_MCS
// MCS Base Address (4KB aligned)
static uint SURFACE_MCS_PITCH_SHIFT         3
static uint SUFFACE_MCS_PITCH_MASK          = 0x1ff
static uint SURFACE_MCS                     = (1 << 0)

// Flags 3 - Surface Format != PLANAR && !SURFACE_MCS
// Surface Append Counter Address
static uint SURFACE_APPEND_COUNTER          = (1 << 1)

// Flags 4
static uint SURFACE_RED_CLEAR_COLOR         = (1 << 31)
static uint SURFACE_GREEN_CLEAR_COLOR       = (1 << 30)
static uint SURFACE_BLUE_CLEAR_COLOR        = (1 << 29)
static uint SURFACE_ALPHA_CLEAR_COLOR       = (1 << 28)
static uint SURFACE_RSRC_MIN_LOD_SHIFT      0
static uint SURFACE_RSRC_MIN_LOD_MASK       0xfff

typedef struct SurfaceState
{
    u32 flags0;
    u32 baseAddr;
    u16 height;
    u16 width;
    u32 pitchDepth;
    u32 flags1;
    u32 flags2;
    u32 flags3;
    u32 flags4;
} SurfaceState;

// ------------------------------------------------------------------------------------------------
// 2.12.3 Sampler State

// MIP_FILTER
static uint MIP_FILTER_NONE                 = 0x0
static uint MIP_FILTER_NEAREST              = 0x1
static uint MIP_FILTER_LINEAR               = 0x3
static uint MIP_FILTER_MASK                 = 0x3

// MAP_FILTER
static uint MAP_FILTER_NONE                 = 0x0
static uint MAP_FILTER_LINEAR               = 0x1
static uint MAP_FILTER_ANISO                = 0x2
static uint MAP_FILTER_MONO                 = 0x6
static uint MAP_FILTER_MASK                 = 0x7

// ANISO_RATIO
static uint ANISO_RATIO_2                   = 0x0
static uint ANISO_RATIO_4                   = 0x1
static uint ANISO_RATIO_6                   = 0x2
static uint ANISO_RATIO_8                   = 0x3
static uint ANISO_RATIO_10                  = 0x4
static uint ANISO_RATIO_12                  = 0x5
static uint ANISO_RATIO_14                  = 0x6
static uint ANISO_RATIO_16                  = 0x7
static uint ANISO_RATIO_MASK                = 0x7

// TRIQUAL
static uint TRIQUAL_FULL                    = 0x0
static uint TRIQUAL_MED                     = 0x2
static uint TRIQUAL_LOW                     = 0x3
static uint TRIQUAL_MASK                    = 0x3

// TEXTURE_ADDRESS
static uint TEXTURE_ADDRESS_WRAP            = 0x0
static uint TEXTURE_ADDRESS_MIRROR          = 0x1
static uint TEXTURE_ADDRESS_CLAMP           = 0x2
static uint TEXTURE_ADDRESS_CUBE            = 0x3
static uint TEXTURE_ADDRESS_CLAMP_BORDER    0x4
static uint TEXTURE_ADDRESS_MIRROR_ONCE     0x5
static uint TEXTURE_ADDRESS_MASK            = 0x7

// Flags 0
static uint SAMPLER_DISABLE                 = (1 << 31)
static uint SAMPLER_BORDER_COLOR_DX9        = (1 << 29)   // Must be set for P4A4_UNORM or A4P4_UNORM
static uint SAMPLER_LOD_PRECLAMP_OGL        = (1 << 28)
static uint SAMPLER_BASE_MIP_SHIFT          22     ;     // Unsigned 4.1 [0.0, 14.0]
static uint SAMPLER_MIP_FILTER_SHIFT        20     ;     // MIP_FILTER
static uint SAMPLER_MAG_FILTER_SHIFT        17     ;     // MAP_FILTER
static uint SAMPLER_MIN_FILTER_SHIFT        14     ;     // MAP_FILTER
static uint SAMPLER_LOD_BIAS_SHIFT          1      ;     // Signed 4.8 [-16.0, 16.0)
static uint SAMPLER_ANISO_EWA               = (1 << 0)

// Flags 1
static uint SAMPLER_MIN_LOD_SHIFT           20     ;     // Unsigned 4.8 [0.0, 14.0]
static uint SAMPLER_MAX_LOD_SHIFT           8      ;     // Unsigned 4.8 [0.0, 14.0]
static uint SAMPLER_COMPARISON_FUNC_SHIFT   1      ;     // COMPARE_FUNC
static uint SAMPLER_CUBE_MODE_OVERRIDE      = (1 << 0)    // Must not be set

// Flags 2
static uint SAMPLER_CHROMA_KEY              = (1 << 25)
static uint SAMPLER_CHROMA_KEY_INDEX_SHIFT  23
static uint SAMPLER_CHROMA_KEY_MODE         = (1 << 22)
static uint SAMPLER_MAX_ANISO_SHIFT         19     ;     // ANISO_RATIO
static uint SAMPLER_ADDRESS_ROUND_SHIFT     13
static uint SAMPLER_TRILINEAR_SHIFT         11     ;     // TRIQUAL
static uint SAMPLER_NON_NORMALIZED          = (1 << 10)
static uint SAMPLER_ADDRESS_U_SHIFT         6
static uint SAMPLER_ADDRESS_V_SHIFT         3
static uint SAMPLER_ADDRESS_W_SHIFT         0

typedef struct SamplerState
{
    u32 flags0;
    u32 flags1;
    u32 borderColorAddr;           ;     // SAMPLER_BORDER_COLOR_STATE relative to Dynamic State Base Address
    u32 flags2;
} SamplerState;

// ------------------------------------------------------------------------------------------------
// IGD OpRegion Specification
// ------------------------------------------------------------------------------------------------

// ------------------------------------------------------------------------------------------------
// 2.2.1 OpRegion Header

typedef struct OpRegionHeader
{
    char sign[0x10];
    u32  size;
    u32  over;
    char sver[0x20];
    char vver[0x10];
    char gver[0x10];
    union OpRegionHeader_MBox
    {
        struct OpRegionHeader_MBox_Bits
        {
            u32 acpi  :  1;
            u32 swsci :  1;
            u32 asle  :  1;
            u32 rsvd  : 29;
        } bits;
        u32 dword;
    } mbox;
    u32  dmod;
    u8   rsv1[0xA0];
} OpRegionHeader;

// ------------------------------------------------------------------------------------------------
// 3.1 Mailbox #1: Public ACPI Methods Mailbox

static uint OPREGION_MAILBOX1_OFFSET 0x0100

typedef struct OpRegionMailbox1ACPI
{
    u32 drdy;       ;     // Driver Ready
    u32 csts;       ;     // STATUS
    u32 cevt;       ;     // Current Event
    u8  rsv2[0x14];
    u32 didl[8];    ;     // Supported Display Devices ID List (_DOD)
    u32 cpdl[8];    ;     // Current Attached (or Present) Display Devices List
    u32 cadl[8];    ;     // Current Active Display DevicesLists (_DCS)
    u32 nadl[8];    ;     // Next Active Devices List (_DGS use)
    u32 aslp;       ;     // ASL Sleep Time Out
    u32 tidx;       ;     // Toggle Table Index
    u32 chpd;       ;     // Current Hotplug Enable Indicator
    u32 clid;       ;     // Current Lid State Indicator
    u32 cdck;       ;     // Current Docking State Indicator
    u32 sxsw;       ;     // Request ASL to issue Display Switch notification on Sx State resume
    u32 evts;       ;     // Events Supported by ASL
    u32 cnot;       ;     // Current OS Notification
    u32 nrdy;       ;     // Driver Status
    u8  rsv3[0x40];
} OpRegionMailbox1ACPI;

// ------------------------------------------------------------------------------------------------
// 2.2.1 OpRegion Header

static uint OPREGION_MAILBOX2_OFFSET 0x0200

typedef struct OpRegionMailbox2SWSCI
{
    u32 scic;        ;     // SWSCI Command/Status/Data
    u32 parm;        ;     // Parameters
    u32 dslp;        ;     // Driver Sleep Time Out
    u8  rsv4[0xF4];
} OpRegionMailbox2SWSCI;

// ------------------------------------------------------------------------------------------------
// 2.2.1 OpRegion Header

static uint OPREGION_MAILBOX3_OFFSET 0x0300
typedef struct OpRegionMailbox3ASLE
{
    u32 ardy;        ;     // Driver Readiness
    u32 aslc;        ;     // ASLE Interrupt Command/Status
    u32 tche;        ;     // Technology Enable Indicator
    u32 alsi;        ;     // Current ALS Luminance Reading (in Lux)
    u32 bclp;        ;     // Requested Backlight Brightness
    u32 pfit;        ;     // Panel Fitting State or Request
    u32 cblv;        ;     // Current Brightness Level
    u16 bclm[20];    ;     // Backlight Brightness Levels Duty Cycle Mapping Table
    u32 cpfm;        ;     // Current Panel Fitting Mode
    u32 epfm;        ;     // Enabled Panel Fitting Modes
    struct PLUT
    {
        u8 lutHeader;;     // MWDD FIX: Prob need to break down to bits
        struct PanelId
        {
            u16 manufacturingId;
            u16 productId;
            u32 serialNumbers;
            u8  weekOfManufacture;
            u8  yearOfManufacture;
        } panelId;
        u8 lutTable[7][9]; // MWDD FIX: Do I have rows/cols backwards?
    } plut;          ;     // Panel LUT & Identifer
    u32 pfmb;        ;     // PWM Frequency and Minimum Brightness
    u32 ccdv;        ;     // Color Correction Default Values
    u8  rsv4[0xF4];
} OpRegionMailbox3ASLE;

// ------------------------------------------------------------------------------------------------
// 2.2.1 OpRegion Header

static uint OPREGION_VBT_OFFSET 0x0500

// ------------------------------------------------------------------------------------------------
// Desktop 3rd Generation Intel® Core™ Processor Family and Desktop Intel® Pentium® Processor Family
// Datasheet - Volume 2
// ------------------------------------------------------------------------------------------------

// ------------------------------------------------------------------------------------------------
// 2.16.2-3 Address Decode Channel Registers

static uint MAD_DIMM_CH0			        0x5004
static uint MAD_DIMM_CH1			        0x5008

static uint MAD_DIMM_A_SIZE_SHIFT           0
static uint MAD_DIMM_A_SIZE_MASK            = 0xff
static uint MAD_DIMM_B_SIZE_SHIFT           8
static uint MAD_DIMM_B_SIZE_MASK            = 0xff
static uint MAD_DIMM_AB_SIZE_MASK           = 0xffff
static uint MAD_DIMM_A_SELECT               = (1 << 16)
static uint MAD_DIMM_A_DUAL_RANK            = (1 << 17)
static uint MAD_DIMM_B_DUAL_RANK            = (1 << 18)
static uint MAD_DIMM_A_X16                  = (1 << 19)
static uint MAD_DIMM_B_X16                  = (1 << 20)
static uint MAD_DIMM_RANK_INTERLEAVE        = (1 << 21)
static uint MAD_DIMM_ENH_INTERLEAVE         = (1 << 22)
static uint MAD_DIMM_ECC_MODE               = (3 << 24) 

// ------------------------------------------------------------------------------------------------
// Registers not in the Spec (Found in Linux Driver)
// ------------------------------------------------------------------------------------------------

// ------------------------------------------------------------------------------------------------
// Force Wake
// Bring the card out of D6 state

static uint ECOBUS                          = 0xA180
static uint FORCE_WAKE_MT                   = 0xA188 
static uint FORCE_WAKE                      = 0xA18C 
static uint FORCE_WAKE_MT_ACK               = 0x130040 
static uint FORCE_WAKE_ACK                  = 0x130090 

// ------------------------------------------------------------------------------------------------
// Fence registers.  Mentioned lots of times
// and the base address is in Vol2 Part3: MFX, but the definition is not

static uint FENCE_BASE                      = 0x100000
static uint FENCE_COUNT                     16

/*
typedef union RegFence
{
    struct RegFence_Bits
    {
        u64 valid              : 1;
        u64 ytile              : 1;
        u64 rsvd               : 10;
        u64 startPage          : 20;
        u64 pitch              : 12;
        u64 endPage            : 20;  // inclusive
    } bits;
    u64 qword;
} RegFence;
*/

// ------------------------------------------------------------------------------------------------
// Tile Ctrl - control register for cpu gtt access

static uint TILE_CTL                         = 0x101000;     // R/W

static uint TILE_CTL_SWIZZLE                = (1 << 0)
static uint TILE_CTL_TLB_PREFETCH_DISABLE   (1 << 2)
static uint TILE_CTL_BACKSNOOP_DISABLE      = (1 << 3)

    }
}
