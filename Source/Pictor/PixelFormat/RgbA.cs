/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Simon Wollwage (rootnode) <kintaro@think-in-co.de>
 */
#define USE_BLENDER

namespace Pictor.PixelFormat
{
	/*

		//=========================================================multiplier_rgba
		template<class ColorT, class Order> struct multiplier_rgba
		{
			typedef typename ColorT::value_type value_type;
			typedef typename ColorT::calc_type calc_type;

			//--------------------------------------------------------------------
			static void PreMultiply(value_type* p)
			{
				calc_type a = p[Order::A];
				if(a < ColorT::BaseMask)
				{
					if(a == 0)
					{
						p[Order::R] = p[Order::G] = p[Order::B] = 0;
						return;
					}
					p[Order::R] = value_type((p[Order::R] * a + ColorT::BaseMask) >> ColorT::BaseShift);
					p[Order::G] = value_type((p[Order::G] * a + ColorT::BaseMask) >> ColorT::BaseShift);
					p[Order::B] = value_type((p[Order::B] * a + ColorT::BaseMask) >> ColorT::BaseShift);
				}
			}

			//--------------------------------------------------------------------
			static void DeMultiply(value_type* p)
			{
				calc_type a = p[Order::A];
				if(a < ColorT::BaseMask)
				{
					if(a == 0)
					{
						p[Order::R] = p[Order::G] = p[Order::B] = 0;
						return;
					}
					calc_type r = (calc_type(p[Order::R]) * ColorT::BaseMask) / a;
					calc_type g = (calc_type(p[Order::G]) * ColorT::BaseMask) / a;
					calc_type b = (calc_type(p[Order::B]) * ColorT::BaseMask) / a;
					p[Order::R] = value_type((r > ColorT::BaseMask) ? ColorT::BaseMask : r);
					p[Order::G] = value_type((g > ColorT::BaseMask) ? ColorT::BaseMask : g);
					p[Order::B] = value_type((b > ColorT::BaseMask) ? ColorT::BaseMask : b);
				}
			}
		};

		//=====================================================apply_gamma_dir_rgba
		template<class ColorT, class Order, class GammaLut> class apply_gamma_dir_rgba
		{
		public:
			typedef typename ColorT::value_type value_type;

			apply_gamma_dir_rgba(const GammaLut& Gamma) : m_gamma(Gamma) {}

			void operator () (value_type* p)
			{
				p[Order::R] = m_gamma.Dir(p[Order::R]);
				p[Order::G] = m_gamma.Dir(p[Order::G]);
				p[Order::B] = m_gamma.Dir(p[Order::B]);
			}

		private:
			const GammaLut& m_gamma;
		};

		//=====================================================apply_gamma_inv_rgba
		template<class ColorT, class Order, class GammaLut> class apply_gamma_inv_rgba
		{
		public:
			typedef typename ColorT::value_type value_type;

			apply_gamma_inv_rgba(const GammaLut& Gamma) : m_gamma(Gamma) {}

			void operator () (value_type* p)
			{
				p[Order::R] = m_gamma.Inv(p[Order::R]);
				p[Order::G] = m_gamma.Inv(p[Order::G]);
				p[Order::B] = m_gamma.Inv(p[Order::B]);
			}

		private:
			const GammaLut& m_gamma;
		};

		//=============================================================blender_rgba
		template<class ColorT, class Order> struct blender_rgba
		{
			typedef ColorT color_type;
			typedef Order order_type;
			typedef typename color_type::value_type value_type;
			typedef typename color_type::calc_type calc_type;
			enum base_scale_e
			{
				BaseShift = color_type::BaseShift,
				BaseMask  = color_type::BaseMask
			};

			//--------------------------------------------------------------------
			static void BlendPixel(value_type* p,
											 uint cr, uint cg, uint cb,
											 uint alpha,
											 uint cover=0)
			{
				calc_type r = p[Order::R];
				calc_type g = p[Order::G];
				calc_type b = p[Order::B];
				calc_type a = p[Order::A];
				p[Order::R] = (value_type)(((cr - r) * alpha + (r << BaseShift)) >> BaseShift);
				p[Order::G] = (value_type)(((cg - g) * alpha + (g << BaseShift)) >> BaseShift);
				p[Order::B] = (value_type)(((cb - b) * alpha + (b << BaseShift)) >> BaseShift);
				p[Order::A] = (value_type)((alpha + a) - ((alpha * a + BaseMask) >> BaseShift));
			}
		};
	 */

	public interface IBlender
	{
		uint NumPixelBits { get; }

		int OrderR { get; }

		int OrderG { get; }

		int OrderB { get; }

		int OrderA { get; }

		unsafe void BlendPixel(byte* p, uint cr, uint cg, uint cb, uint alpha);

		unsafe void BlendPixel(byte* p, uint cr, uint cg, uint cb, uint alpha, uint cover);
	}

	public class BlenderBaseBGRA
	{
		public uint NumPixelBits { get { return 32; } }

		public enum Order { R = 2, G = 1, B = 0, A = 3 };

		public int OrderR { get { return (int)Order.R; } }

		public int OrderG { get { return (int)Order.G; } }

		public int OrderB { get { return (int)Order.B; } }

		public int OrderA { get { return (int)Order.A; } }

		public const byte base_mask = 255;
	};

	public sealed class BlenderBGRA : BlenderBaseBGRA, IBlender
	{
		unsafe public void BlendPixel(byte* p, uint cr, uint cg, uint cb, uint alpha, uint cover)
		{
			BlendPixel(p, cr, cg, cb, alpha);
		}

		unsafe public void BlendPixel(byte* p, uint cr, uint cg, uint cb, uint alpha)
		{
			unchecked
			{
				uint r = p[(int)Order.R];
				uint g = p[(int)Order.G];
				uint b = p[(int)Order.B];
				uint a = p[(int)Order.A];
				p[(int)Order.R] = (byte)(((uint)(cr - r) * alpha + (r << (int)RGBA_Bytes.BaseShift)) >> (int)RGBA_Bytes.BaseShift);
				p[(int)Order.G] = (byte)(((cg - g) * alpha + (g << (int)RGBA_Bytes.BaseShift)) >> (int)RGBA_Bytes.BaseShift);
				p[(int)Order.B] = (byte)(((cb - b) * alpha + (b << (int)RGBA_Bytes.BaseShift)) >> (int)RGBA_Bytes.BaseShift);
				p[(int)Order.A] = (byte)((alpha + a) - ((alpha * a + base_mask) >> (int)RGBA_Bytes.BaseShift));
			}
		}
	};

	public sealed class BlenderGammaBGRA : BlenderBaseBGRA, IBlender
	{
		private GammaLookupTable m_gamma;

		public BlenderGammaBGRA()
		{
			m_gamma = new GammaLookupTable();
		}

		public BlenderGammaBGRA(GammaLookupTable g)
		{
			m_gamma = g;
		}

		public GammaLookupTable Gamma
		{
			set
			{
				m_gamma = value;
			}
		}

		unsafe public void BlendPixel(byte* p, uint cr, uint cg, uint cb, uint alpha, uint cover)
		{
			BlendPixel(p, cr, cg, cb, alpha);
		}

		unsafe public void BlendPixel(byte* p, uint cr, uint cg, uint cb, uint alpha)
		{
			unchecked
			{
				uint r = p[(int)Order.R];
				uint g = p[(int)Order.G];
				uint b = p[(int)Order.B];
				uint a = p[(int)Order.A];
				p[(int)Order.R] = m_gamma.Inv((byte)(((uint)(cr - r) * alpha + (r << (int)RGBA_Bytes.BaseShift)) >> (int)RGBA_Bytes.BaseShift));
				p[(int)Order.G] = m_gamma.Inv((byte)(((cg - g) * alpha + (g << (int)RGBA_Bytes.BaseShift)) >> (int)RGBA_Bytes.BaseShift));
				p[(int)Order.B] = m_gamma.Inv((byte)(((cb - b) * alpha + (b << (int)RGBA_Bytes.BaseShift)) >> (int)RGBA_Bytes.BaseShift));
				p[(int)Order.A] = (byte)((alpha + a) - ((alpha * a + base_mask) >> (int)RGBA_Bytes.BaseShift));
			}
		}
	};

	public sealed class BlenderPreMultBGRA : BlenderBaseBGRA, IBlender
	{
		unsafe public void BlendPixel(byte* p, uint cr, uint cg, uint cb, uint alpha, uint cover)
		{
			uint OneOverAlpha = base_mask - alpha;
			cover = (cover + 1) << ((int)RGBA_Bytes.BaseShift - 8);
			unchecked
			{
				p[(int)Order.R] = (byte)((p[(int)Order.R] * OneOverAlpha + cr * cover) >> (int)RGBA_Bytes.BaseShift);
				p[(int)Order.G] = (byte)((p[(int)Order.G] * OneOverAlpha + cg * cover) >> (int)RGBA_Bytes.BaseShift);
				p[(int)Order.B] = (byte)((p[(int)Order.B] * OneOverAlpha + cb * cover) >> (int)RGBA_Bytes.BaseShift);
				p[(int)Order.A] = (byte)(base_mask - ((OneOverAlpha * (base_mask - p[(int)Order.A])) >> (int)RGBA_Bytes.BaseShift));
			}
		}

		//--------------------------------------------------------------------
		unsafe public void BlendPixel(byte* p, uint cr, uint cg, uint cb, uint alpha)
		{
			uint OneOverAlpha = base_mask - alpha;
			unchecked
			{
				p[(int)Order.R] = (byte)(((p[(int)Order.R] * OneOverAlpha) >> (int)RGBA_Bytes.BaseShift) + cr);
				p[(int)Order.G] = (byte)(((p[(int)Order.G] * OneOverAlpha) >> (int)RGBA_Bytes.BaseShift) + cg);
				p[(int)Order.B] = (byte)(((p[(int)Order.B] * OneOverAlpha) >> (int)RGBA_Bytes.BaseShift) + cb);
				p[(int)Order.A] = (byte)(base_mask - ((OneOverAlpha * (base_mask - p[(int)Order.A])) >> (int)RGBA_Bytes.BaseShift));
			}
		}
	};

	public sealed class BlenderPreMultClampedBGRA : BlenderBaseBGRA, IBlender
	{
		unsafe public void BlendPixel(byte* p, uint cr, uint cg, uint cb, uint alpha, uint cover)
		{
			uint OneOverAlpha = base_mask - alpha;
			cover = (cover + 1) << ((int)RGBA_Bytes.BaseShift - 8);
			p[(int)Order.R] = (byte)System.Math.Min(((p[(int)Order.R] * OneOverAlpha + cr * cover) >> (int)RGBA_Bytes.BaseShift), base_mask);
			p[(int)Order.G] = (byte)System.Math.Min(((p[(int)Order.G] * OneOverAlpha + cg * cover) >> (int)RGBA_Bytes.BaseShift), base_mask);
			p[(int)Order.B] = (byte)System.Math.Min(((p[(int)Order.B] * OneOverAlpha + cb * cover) >> (int)RGBA_Bytes.BaseShift), base_mask);
			p[(int)Order.A] = (byte)System.Math.Min((base_mask - ((OneOverAlpha * (base_mask - p[(int)Order.A])) >> (int)RGBA_Bytes.BaseShift)), base_mask);
		}

		//--------------------------------------------------------------------
		unsafe public void BlendPixel(byte* p, uint cr, uint cg, uint cb, uint alpha)
		{
			uint OneOverAlpha = base_mask - alpha;
			p[(int)Order.R] = (byte)System.Math.Min((((p[(int)Order.R] * OneOverAlpha) >> (int)RGBA_Bytes.BaseShift) + cr), base_mask);
			p[(int)Order.G] = (byte)System.Math.Min((((p[(int)Order.G] * OneOverAlpha) >> (int)RGBA_Bytes.BaseShift) + cr), base_mask);
			p[(int)Order.B] = (byte)System.Math.Min((((p[(int)Order.B] * OneOverAlpha) >> (int)RGBA_Bytes.BaseShift) + cr), base_mask);
			p[(int)Order.A] = (byte)System.Math.Min((base_mask - ((OneOverAlpha * (base_mask - p[(int)Order.A])) >> (int)RGBA_Bytes.BaseShift)), base_mask);
		}
	};

	/*

//======================================================blender_rgba_plain
template<class ColorT, class Order> struct blender_rgba_plain
{
	typedef ColorT color_type;
	typedef Order order_type;
	typedef typename color_type::value_type value_type;
	typedef typename color_type::calc_type calc_type;
	enum base_scale_e { BaseShift = color_type::BaseShift };

	//--------------------------------------------------------------------
	static void BlendPixel(value_type* p,
									 uint cr, uint cg, uint cb,
									 uint alpha,
									 uint cover=0)
	{
		if(alpha == 0) return;
		calc_type a = p[Order::A];
		calc_type r = p[Order::R] * a;
		calc_type g = p[Order::G] * a;
		calc_type b = p[Order::B] * a;
		a = ((alpha + a) << BaseShift) - alpha * a;
		p[Order::A] = (value_type)(a >> BaseShift);
		p[Order::R] = (value_type)((((cr << BaseShift) - r) * alpha + (r << BaseShift)) / a);
		p[Order::G] = (value_type)((((cg << BaseShift) - g) * alpha + (g << BaseShift)) / a);
		p[Order::B] = (value_type)((((cb << BaseShift) - b) * alpha + (b << BaseShift)) / a);
	}
};

//=========================================================comp_op_rgba_clear
template<class ColorT, class Order> struct comp_op_rgba_clear
{
	typedef ColorT color_type;
	typedef Order order_type;
	typedef typename color_type::value_type value_type;
	enum base_scale_e
	{
		BaseShift = color_type::BaseShift,
		BaseMask  = color_type::BaseMask
	};

	static void BlendPixel(value_type* p,
									 uint, uint, uint, uint,
									 uint cover)
	{
		if(cover < 255)
		{
			cover = 255 - cover;
			p[Order::R] = (value_type)((p[Order::R] * cover + 255) >> 8);
			p[Order::G] = (value_type)((p[Order::G] * cover + 255) >> 8);
			p[Order::B] = (value_type)((p[Order::B] * cover + 255) >> 8);
			p[Order::A] = (value_type)((p[Order::A] * cover + 255) >> 8);
		}
		else
		{
			p[0] = p[1] = p[2] = p[3] = 0;
		}
	}
};

//===========================================================comp_op_rgba_src
template<class ColorT, class Order> struct comp_op_rgba_src
{
	typedef ColorT color_type;
	typedef Order order_type;
	typedef typename color_type::value_type value_type;

	static void BlendPixel(value_type* p,
									 uint sr, uint sg, uint sb,
									 uint sa, uint cover)
	{
		if(cover < 255)
		{
			uint alpha = 255 - cover;
			p[Order::R] = (value_type)(((p[Order::R] * alpha + 255) >> 8) + ((sr * cover + 255) >> 8));
			p[Order::G] = (value_type)(((p[Order::G] * alpha + 255) >> 8) + ((sg * cover + 255) >> 8));
			p[Order::B] = (value_type)(((p[Order::B] * alpha + 255) >> 8) + ((sb * cover + 255) >> 8));
			p[Order::A] = (value_type)(((p[Order::A] * alpha + 255) >> 8) + ((sa * cover + 255) >> 8));
		}
		else
		{
			p[Order::R] = sr;
			p[Order::G] = sg;
			p[Order::B] = sb;
			p[Order::A] = sa;
		}
	}
};

//===========================================================comp_op_rgba_dst
template<class ColorT, class Order> struct comp_op_rgba_dst
{
	typedef ColorT color_type;
	typedef Order order_type;
	typedef typename color_type::value_type value_type;

	static void BlendPixel(value_type*,
									 uint, uint, uint,
									 uint, uint)
	{
	}
};

//======================================================comp_op_rgba_src_over
template<class ColorT, class Order> struct comp_op_rgba_src_over
{
	typedef ColorT color_type;
	typedef Order order_type;
	typedef typename color_type::value_type value_type;
	typedef typename color_type::calc_type calc_type;
	enum base_scale_e
	{
		BaseShift = color_type::BaseShift,
		BaseMask  = color_type::BaseMask
	};

	//   Dca' = Sca + Dca.(1 - Sa)
	//   Da'  = Sa + Da - Sa.Da
	static void BlendPixel(value_type* p,
									 uint sr, uint sg, uint sb,
									 uint sa, uint cover)
	{
		if(cover < 255)
		{
			sr = (sr * cover + 255) >> 8;
			sg = (sg * cover + 255) >> 8;
			sb = (sb * cover + 255) >> 8;
			sa = (sa * cover + 255) >> 8;
		}
		calc_type s1a = BaseMask - sa;
		p[Order::R] = (value_type)(sr + ((p[Order::R] * s1a + BaseMask) >> BaseShift));
		p[Order::G] = (value_type)(sg + ((p[Order::G] * s1a + BaseMask) >> BaseShift));
		p[Order::B] = (value_type)(sb + ((p[Order::B] * s1a + BaseMask) >> BaseShift));
		p[Order::A] = (value_type)(sa + p[Order::A] - ((sa * p[Order::A] + BaseMask) >> BaseShift));
	}
};

//======================================================comp_op_rgba_dst_over
template<class ColorT, class Order> struct comp_op_rgba_dst_over
{
	typedef ColorT color_type;
	typedef Order order_type;
	typedef typename color_type::value_type value_type;
	typedef typename color_type::calc_type calc_type;
	enum base_scale_e
	{
		BaseShift = color_type::BaseShift,
		BaseMask  = color_type::BaseMask
	};

	// Dca' = Dca + Sca.(1 - Da)
	// Da'  = Sa + Da - Sa.Da
	static void BlendPixel(value_type* p,
									 uint sr, uint sg, uint sb,
									 uint sa, uint cover)
	{
		if(cover < 255)
		{
			sr = (sr * cover + 255) >> 8;
			sg = (sg * cover + 255) >> 8;
			sb = (sb * cover + 255) >> 8;
			sa = (sa * cover + 255) >> 8;
		}
		calc_type d1a = BaseMask - p[Order::A];
		p[Order::R] = (value_type)(p[Order::R] + ((sr * d1a + BaseMask) >> BaseShift));
		p[Order::G] = (value_type)(p[Order::G] + ((sg * d1a + BaseMask) >> BaseShift));
		p[Order::B] = (value_type)(p[Order::B] + ((sb * d1a + BaseMask) >> BaseShift));
		p[Order::A] = (value_type)(sa + p[Order::A] - ((sa * p[Order::A] + BaseMask) >> BaseShift));
	}
};

//======================================================comp_op_rgba_src_in
template<class ColorT, class Order> struct comp_op_rgba_src_in
{
	typedef ColorT color_type;
	typedef Order order_type;
	typedef typename color_type::value_type value_type;
	typedef typename color_type::calc_type calc_type;
	enum base_scale_e
	{
		BaseShift = color_type::BaseShift,
		BaseMask  = color_type::BaseMask
	};

	// Dca' = Sca.Da
	// Da'  = Sa.Da
	static void BlendPixel(value_type* p,
									 uint sr, uint sg, uint sb,
									 uint sa, uint cover)
	{
		calc_type da = p[Order::A];
		if(cover < 255)
		{
			uint alpha = 255 - cover;
			p[Order::R] = (value_type)(((p[Order::R] * alpha + 255) >> 8) + ((((sr * da + BaseMask) >> BaseShift) * cover + 255) >> 8));
			p[Order::G] = (value_type)(((p[Order::G] * alpha + 255) >> 8) + ((((sg * da + BaseMask) >> BaseShift) * cover + 255) >> 8));
			p[Order::B] = (value_type)(((p[Order::B] * alpha + 255) >> 8) + ((((sb * da + BaseMask) >> BaseShift) * cover + 255) >> 8));
			p[Order::A] = (value_type)(((p[Order::A] * alpha + 255) >> 8) + ((((sa * da + BaseMask) >> BaseShift) * cover + 255) >> 8));
		}
		else
		{
			p[Order::R] = (value_type)((sr * da + BaseMask) >> BaseShift);
			p[Order::G] = (value_type)((sg * da + BaseMask) >> BaseShift);
			p[Order::B] = (value_type)((sb * da + BaseMask) >> BaseShift);
			p[Order::A] = (value_type)((sa * da + BaseMask) >> BaseShift);
		}
	}
};

//======================================================comp_op_rgba_dst_in
template<class ColorT, class Order> struct comp_op_rgba_dst_in
{
	typedef ColorT color_type;
	typedef Order order_type;
	typedef typename color_type::value_type value_type;
	typedef typename color_type::calc_type calc_type;
	enum base_scale_e
	{
		BaseShift = color_type::BaseShift,
		BaseMask  = color_type::BaseMask
	};

	// Dca' = Dca.Sa
	// Da'  = Sa.Da
	static void BlendPixel(value_type* p,
									 uint, uint, uint,
									 uint sa, uint cover)
	{
		if(cover < 255)
		{
			sa = BaseMask - ((cover * (BaseMask - sa) + 255) >> 8);
		}
		p[Order::R] = (value_type)((p[Order::R] * sa + BaseMask) >> BaseShift);
		p[Order::G] = (value_type)((p[Order::G] * sa + BaseMask) >> BaseShift);
		p[Order::B] = (value_type)((p[Order::B] * sa + BaseMask) >> BaseShift);
		p[Order::A] = (value_type)((p[Order::A] * sa + BaseMask) >> BaseShift);
	}
};

//======================================================comp_op_rgba_src_out
template<class ColorT, class Order> struct comp_op_rgba_src_out
{
	typedef ColorT color_type;
	typedef Order order_type;
	typedef typename color_type::value_type value_type;
	typedef typename color_type::calc_type calc_type;
	enum base_scale_e
	{
		BaseShift = color_type::BaseShift,
		BaseMask  = color_type::BaseMask
	};

	// Dca' = Sca.(1 - Da)
	// Da'  = Sa.(1 - Da)
	static void BlendPixel(value_type* p,
									 uint sr, uint sg, uint sb,
									 uint sa, uint cover)
	{
		calc_type da = BaseMask - p[Order::A];
		if(cover < 255)
		{
			uint alpha = 255 - cover;
			p[Order::R] = (value_type)(((p[Order::R] * alpha + 255) >> 8) + ((((sr * da + BaseMask) >> BaseShift) * cover + 255) >> 8));
			p[Order::G] = (value_type)(((p[Order::G] * alpha + 255) >> 8) + ((((sg * da + BaseMask) >> BaseShift) * cover + 255) >> 8));
			p[Order::B] = (value_type)(((p[Order::B] * alpha + 255) >> 8) + ((((sb * da + BaseMask) >> BaseShift) * cover + 255) >> 8));
			p[Order::A] = (value_type)(((p[Order::A] * alpha + 255) >> 8) + ((((sa * da + BaseMask) >> BaseShift) * cover + 255) >> 8));
		}
		else
		{
			p[Order::R] = (value_type)((sr * da + BaseMask) >> BaseShift);
			p[Order::G] = (value_type)((sg * da + BaseMask) >> BaseShift);
			p[Order::B] = (value_type)((sb * da + BaseMask) >> BaseShift);
			p[Order::A] = (value_type)((sa * da + BaseMask) >> BaseShift);
		}
	}
};

//======================================================comp_op_rgba_dst_out
template<class ColorT, class Order> struct comp_op_rgba_dst_out
{
	typedef ColorT color_type;
	typedef Order order_type;
	typedef typename color_type::value_type value_type;
	typedef typename color_type::calc_type calc_type;
	enum base_scale_e
	{
		BaseShift = color_type::BaseShift,
		BaseMask  = color_type::BaseMask
	};

	// Dca' = Dca.(1 - Sa)
	// Da'  = Da.(1 - Sa)
	static void BlendPixel(value_type* p,
									 uint, uint, uint,
									 uint sa, uint cover)
	{
		if(cover < 255)
		{
			sa = (sa * cover + 255) >> 8;
		}
		sa = BaseMask - sa;
		p[Order::R] = (value_type)((p[Order::R] * sa + BaseShift) >> BaseShift);
		p[Order::G] = (value_type)((p[Order::G] * sa + BaseShift) >> BaseShift);
		p[Order::B] = (value_type)((p[Order::B] * sa + BaseShift) >> BaseShift);
		p[Order::A] = (value_type)((p[Order::A] * sa + BaseShift) >> BaseShift);
	}
};

//=====================================================comp_op_rgba_src_atop
template<class ColorT, class Order> struct comp_op_rgba_src_atop
{
	typedef ColorT color_type;
	typedef Order order_type;
	typedef typename color_type::value_type value_type;
	typedef typename color_type::calc_type calc_type;
	enum base_scale_e
	{
		BaseShift = color_type::BaseShift,
		BaseMask  = color_type::BaseMask
	};

	// Dca' = Sca.Da + Dca.(1 - Sa)
	// Da'  = Da
	static void BlendPixel(value_type* p,
									 uint sr, uint sg, uint sb,
									 uint sa, uint cover)
	{
		if(cover < 255)
		{
			sr = (sr * cover + 255) >> 8;
			sg = (sg * cover + 255) >> 8;
			sb = (sb * cover + 255) >> 8;
			sa = (sa * cover + 255) >> 8;
		}
		calc_type da = p[Order::A];
		sa = BaseMask - sa;
		p[Order::R] = (value_type)((sr * da + p[Order::R] * sa + BaseMask) >> BaseShift);
		p[Order::G] = (value_type)((sg * da + p[Order::G] * sa + BaseMask) >> BaseShift);
		p[Order::B] = (value_type)((sb * da + p[Order::B] * sa + BaseMask) >> BaseShift);
	}
};

//=====================================================comp_op_rgba_dst_atop
template<class ColorT, class Order> struct comp_op_rgba_dst_atop
{
	typedef ColorT color_type;
	typedef Order order_type;
	typedef typename color_type::value_type value_type;
	typedef typename color_type::calc_type calc_type;
	enum base_scale_e
	{
		BaseShift = color_type::BaseShift,
		BaseMask  = color_type::BaseMask
	};

	// Dca' = Dca.Sa + Sca.(1 - Da)
	// Da'  = Sa
	static void BlendPixel(value_type* p,
									 uint sr, uint sg, uint sb,
									 uint sa, uint cover)
	{
		calc_type da = BaseMask - p[Order::A];
		if(cover < 255)
		{
			uint alpha = 255 - cover;
			sr = (p[Order::R] * sa + sr * da + BaseMask) >> BaseShift;
			sg = (p[Order::G] * sa + sg * da + BaseMask) >> BaseShift;
			sb = (p[Order::B] * sa + sb * da + BaseMask) >> BaseShift;
			p[Order::R] = (value_type)(((p[Order::R] * alpha + 255) >> 8) + ((sr * cover + 255) >> 8));
			p[Order::G] = (value_type)(((p[Order::G] * alpha + 255) >> 8) + ((sg * cover + 255) >> 8));
			p[Order::B] = (value_type)(((p[Order::B] * alpha + 255) >> 8) + ((sb * cover + 255) >> 8));
			p[Order::A] = (value_type)(((p[Order::A] * alpha + 255) >> 8) + ((sa * cover + 255) >> 8));
		}
		else
		{
			p[Order::R] = (value_type)((p[Order::R] * sa + sr * da + BaseMask) >> BaseShift);
			p[Order::G] = (value_type)((p[Order::G] * sa + sg * da + BaseMask) >> BaseShift);
			p[Order::B] = (value_type)((p[Order::B] * sa + sb * da + BaseMask) >> BaseShift);
			p[Order::A] = (value_type)sa;
		}
	}
};

//=========================================================comp_op_rgba_xor
template<class ColorT, class Order> struct comp_op_rgba_xor
{
	typedef ColorT color_type;
	typedef Order order_type;
	typedef typename color_type::value_type value_type;
	typedef typename color_type::calc_type calc_type;
	enum base_scale_e
	{
		BaseShift = color_type::BaseShift,
		BaseMask  = color_type::BaseMask
	};

	// Dca' = Sca.(1 - Da) + Dca.(1 - Sa)
	// Da'  = Sa + Da - 2.Sa.Da
	static void BlendPixel(value_type* p,
									 uint sr, uint sg, uint sb,
									 uint sa, uint cover)
	{
		if(cover < 255)
		{
			sr = (sr * cover + 255) >> 8;
			sg = (sg * cover + 255) >> 8;
			sb = (sb * cover + 255) >> 8;
			sa = (sa * cover + 255) >> 8;
		}
		if(sa)
		{
			calc_type s1a = BaseMask - sa;
			calc_type d1a = BaseMask - p[Order::A];
			p[Order::R] = (value_type)((p[Order::R] * s1a + sr * d1a + BaseMask) >> BaseShift);
			p[Order::G] = (value_type)((p[Order::G] * s1a + sg * d1a + BaseMask) >> BaseShift);
			p[Order::B] = (value_type)((p[Order::B] * s1a + sb * d1a + BaseMask) >> BaseShift);
			p[Order::A] = (value_type)(sa + p[Order::A] - ((sa * p[Order::A] + BaseMask/2) >> (BaseShift - 1)));
		}
	}
};

//=========================================================comp_op_rgba_plus
template<class ColorT, class Order> struct comp_op_rgba_plus
{
	typedef ColorT color_type;
	typedef Order order_type;
	typedef typename color_type::value_type value_type;
	typedef typename color_type::calc_type calc_type;
	enum base_scale_e
	{
		BaseShift = color_type::BaseShift,
		BaseMask  = color_type::BaseMask
	};

	// Dca' = Sca + Dca
	// Da'  = Sa + Da
	static void BlendPixel(value_type* p,
									 uint sr, uint sg, uint sb,
									 uint sa, uint cover)
	{
		if(cover < 255)
		{
			sr = (sr * cover + 255) >> 8;
			sg = (sg * cover + 255) >> 8;
			sb = (sb * cover + 255) >> 8;
			sa = (sa * cover + 255) >> 8;
		}
		if(sa)
		{
			calc_type dr = p[Order::R] + sr;
			calc_type dg = p[Order::G] + sg;
			calc_type db = p[Order::B] + sb;
			calc_type da = p[Order::A] + sa;
			p[Order::R] = (dr > BaseMask) ? (value_type)BaseMask : dr;
			p[Order::G] = (dg > BaseMask) ? (value_type)BaseMask : dg;
			p[Order::B] = (db > BaseMask) ? (value_type)BaseMask : db;
			p[Order::A] = (da > BaseMask) ? (value_type)BaseMask : da;
		}
	}
};

//========================================================comp_op_rgba_minus
template<class ColorT, class Order> struct comp_op_rgba_minus
{
	typedef ColorT color_type;
	typedef Order order_type;
	typedef typename color_type::value_type value_type;
	typedef typename color_type::calc_type calc_type;
	enum base_scale_e
	{
		BaseShift = color_type::BaseShift,
		BaseMask  = color_type::BaseMask
	};

	// Dca' = Dca - Sca
	// Da' = 1 - (1 - Sa).(1 - Da)
	static void BlendPixel(value_type* p,
									 uint sr, uint sg, uint sb,
									 uint sa, uint cover)
	{
		if(cover < 255)
		{
			sr = (sr * cover + 255) >> 8;
			sg = (sg * cover + 255) >> 8;
			sb = (sb * cover + 255) >> 8;
			sa = (sa * cover + 255) >> 8;
		}
		if(sa)
		{
			calc_type dr = p[Order::R] - sr;
			calc_type dg = p[Order::G] - sg;
			calc_type db = p[Order::B] - sb;
			p[Order::R] = (dr > BaseMask) ? 0 : dr;
			p[Order::G] = (dg > BaseMask) ? 0 : dg;
			p[Order::B] = (db > BaseMask) ? 0 : db;
			p[Order::A] = (value_type)(sa + p[Order::A] - ((sa * p[Order::A] + BaseMask) >> BaseShift));

			//p[Order::A] = (value_type)(BaseMask - (((BaseMask - sa) * (BaseMask - p[Order::A]) + BaseMask) >> BaseShift));
		}
	}
};

//=====================================================comp_op_rgba_multiply
template<class ColorT, class Order> struct comp_op_rgba_multiply
{
	typedef ColorT color_type;
	typedef Order order_type;
	typedef typename color_type::value_type value_type;
	typedef typename color_type::calc_type calc_type;
	enum base_scale_e
	{
		BaseShift = color_type::BaseShift,
		BaseMask  = color_type::BaseMask
	};

	// Dca' = Sca.Dca + Sca.(1 - Da) + Dca.(1 - Sa)
	// Da'  = Sa + Da - Sa.Da
	static void BlendPixel(value_type* p,
									 uint sr, uint sg, uint sb,
									 uint sa, uint cover)
	{
		if(cover < 255)
		{
			sr = (sr * cover + 255) >> 8;
			sg = (sg * cover + 255) >> 8;
			sb = (sb * cover + 255) >> 8;
			sa = (sa * cover + 255) >> 8;
		}
		if(sa)
		{
			calc_type s1a = BaseMask - sa;
			calc_type d1a = BaseMask - p[Order::A];
			calc_type dr = p[Order::R];
			calc_type dg = p[Order::G];
			calc_type db = p[Order::B];
			p[Order::R] = (value_type)((sr * dr + sr * d1a + dr * s1a + BaseMask) >> BaseShift);
			p[Order::G] = (value_type)((sg * dg + sg * d1a + dg * s1a + BaseMask) >> BaseShift);
			p[Order::B] = (value_type)((sb * db + sb * d1a + db * s1a + BaseMask) >> BaseShift);
			p[Order::A] = (value_type)(sa + p[Order::A] - ((sa * p[Order::A] + BaseMask) >> BaseShift));
		}
	}
};

//=====================================================comp_op_rgba_screen
template<class ColorT, class Order> struct comp_op_rgba_screen
{
	typedef ColorT color_type;
	typedef Order order_type;
	typedef typename color_type::value_type value_type;
	typedef typename color_type::calc_type calc_type;
	enum base_scale_e
	{
		BaseShift = color_type::BaseShift,
		BaseMask  = color_type::BaseMask
	};

	// Dca' = Sca + Dca - Sca.Dca
	// Da'  = Sa + Da - Sa.Da
	static void BlendPixel(value_type* p,
									 uint sr, uint sg, uint sb,
									 uint sa, uint cover)
	{
		if(cover < 255)
		{
			sr = (sr * cover + 255) >> 8;
			sg = (sg * cover + 255) >> 8;
			sb = (sb * cover + 255) >> 8;
			sa = (sa * cover + 255) >> 8;
		}
		if(sa)
		{
			calc_type dr = p[Order::R];
			calc_type dg = p[Order::G];
			calc_type db = p[Order::B];
			calc_type da = p[Order::A];
			p[Order::R] = (value_type)(sr + dr - ((sr * dr + BaseMask) >> BaseShift));
			p[Order::G] = (value_type)(sg + dg - ((sg * dg + BaseMask) >> BaseShift));
			p[Order::B] = (value_type)(sb + db - ((sb * db + BaseMask) >> BaseShift));
			p[Order::A] = (value_type)(sa + da - ((sa * da + BaseMask) >> BaseShift));
		}
	}
};

//=====================================================comp_op_rgba_overlay
template<class ColorT, class Order> struct comp_op_rgba_overlay
{
	typedef ColorT color_type;
	typedef Order order_type;
	typedef typename color_type::value_type value_type;
	typedef typename color_type::calc_type calc_type;
	enum base_scale_e
	{
		BaseShift = color_type::BaseShift,
		BaseMask  = color_type::BaseMask
	};

	// if 2.Dca < Da
	//   Dca' = 2.Sca.Dca + Sca.(1 - Da) + Dca.(1 - Sa)
	// otherwise
	//   Dca' = Sa.Da - 2.(Da - Dca).(Sa - Sca) + Sca.(1 - Da) + Dca.(1 - Sa)
	//
	// Da' = Sa + Da - Sa.Da
	static void BlendPixel(value_type* p,
									 uint sr, uint sg, uint sb,
									 uint sa, uint cover)
	{
		if(cover < 255)
		{
			sr = (sr * cover + 255) >> 8;
			sg = (sg * cover + 255) >> 8;
			sb = (sb * cover + 255) >> 8;
			sa = (sa * cover + 255) >> 8;
		}
		if(sa)
		{
			calc_type d1a  = BaseMask - p[Order::A];
			calc_type s1a  = BaseMask - sa;
			calc_type dr   = p[Order::R];
			calc_type dg   = p[Order::G];
			calc_type db   = p[Order::B];
			calc_type da   = p[Order::A];
			calc_type sada = sa * p[Order::A];

			p[Order::R] = (value_type)(((2*dr < da) ?
				2*sr*dr + sr*d1a + dr*s1a :
				sada - 2*(da - dr)*(sa - sr) + sr*d1a + dr*s1a + BaseMask) >> BaseShift);

			p[Order::G] = (value_type)(((2*dg < da) ?
				2*sg*dg + sg*d1a + dg*s1a :
				sada - 2*(da - dg)*(sa - sg) + sg*d1a + dg*s1a + BaseMask) >> BaseShift);

			p[Order::B] = (value_type)(((2*db < da) ?
				2*sb*db + sb*d1a + db*s1a :
				sada - 2*(da - db)*(sa - sb) + sb*d1a + db*s1a + BaseMask) >> BaseShift);

			p[Order::A] = (value_type)(sa + da - ((sa * da + BaseMask) >> BaseShift));
		}
	}
};

template<class T> inline T sd_min(T a, T b) { return (a < b) ? a : b; }
template<class T> inline T sd_max(T a, T b) { return (a > b) ? a : b; }

//=====================================================comp_op_rgba_darken
template<class ColorT, class Order> struct comp_op_rgba_darken
{
	typedef ColorT color_type;
	typedef Order order_type;
	typedef typename color_type::value_type value_type;
	typedef typename color_type::calc_type calc_type;
	enum base_scale_e
	{
		BaseShift = color_type::BaseShift,
		BaseMask  = color_type::BaseMask
	};

	// Dca' = min(Sca.Da, Dca.Sa) + Sca.(1 - Da) + Dca.(1 - Sa)
	// Da'  = Sa + Da - Sa.Da
	static void BlendPixel(value_type* p,
									 uint sr, uint sg, uint sb,
									 uint sa, uint cover)
	{
		if(cover < 255)
		{
			sr = (sr * cover + 255) >> 8;
			sg = (sg * cover + 255) >> 8;
			sb = (sb * cover + 255) >> 8;
			sa = (sa * cover + 255) >> 8;
		}
		if(sa)
		{
			calc_type d1a = BaseMask - p[Order::A];
			calc_type s1a = BaseMask - sa;
			calc_type dr  = p[Order::R];
			calc_type dg  = p[Order::G];
			calc_type db  = p[Order::B];
			calc_type da  = p[Order::A];

			p[Order::R] = (value_type)((sd_min(sr * da, dr * sa) + sr * d1a + dr * s1a + BaseMask) >> BaseShift);
			p[Order::G] = (value_type)((sd_min(sg * da, dg * sa) + sg * d1a + dg * s1a + BaseMask) >> BaseShift);
			p[Order::B] = (value_type)((sd_min(sb * da, db * sa) + sb * d1a + db * s1a + BaseMask) >> BaseShift);
			p[Order::A] = (value_type)(sa + da - ((sa * da + BaseMask) >> BaseShift));
		}
	}
};

//=====================================================comp_op_rgba_lighten
template<class ColorT, class Order> struct comp_op_rgba_lighten
{
	typedef ColorT color_type;
	typedef Order order_type;
	typedef typename color_type::value_type value_type;
	typedef typename color_type::calc_type calc_type;
	enum base_scale_e
	{
		BaseShift = color_type::BaseShift,
		BaseMask  = color_type::BaseMask
	};

	// Dca' = max(Sca.Da, Dca.Sa) + Sca.(1 - Da) + Dca.(1 - Sa)
	// Da'  = Sa + Da - Sa.Da
	static void BlendPixel(value_type* p,
									 uint sr, uint sg, uint sb,
									 uint sa, uint cover)
	{
		if(cover < 255)
		{
			sr = (sr * cover + 255) >> 8;
			sg = (sg * cover + 255) >> 8;
			sb = (sb * cover + 255) >> 8;
			sa = (sa * cover + 255) >> 8;
		}
		if(sa)
		{
			calc_type d1a = BaseMask - p[Order::A];
			calc_type s1a = BaseMask - sa;
			calc_type dr  = p[Order::R];
			calc_type dg  = p[Order::G];
			calc_type db  = p[Order::B];
			calc_type da  = p[Order::A];

			p[Order::R] = (value_type)((sd_max(sr * da, dr * sa) + sr * d1a + dr * s1a + BaseMask) >> BaseShift);
			p[Order::G] = (value_type)((sd_max(sg * da, dg * sa) + sg * d1a + dg * s1a + BaseMask) >> BaseShift);
			p[Order::B] = (value_type)((sd_max(sb * da, db * sa) + sb * d1a + db * s1a + BaseMask) >> BaseShift);
			p[Order::A] = (value_type)(sa + da - ((sa * da + BaseMask) >> BaseShift));
		}
	}
};

//=====================================================comp_op_rgba_color_dodge
template<class ColorT, class Order> struct comp_op_rgba_color_dodge
{
	typedef ColorT color_type;
	typedef Order order_type;
	typedef typename color_type::value_type value_type;
	typedef typename color_type::calc_type calc_type;
	typedef typename color_type::long_type long_type;
	enum base_scale_e
	{
		BaseShift = color_type::BaseShift,
		BaseMask  = color_type::BaseMask
	};

	// if Sca.Da + Dca.Sa >= Sa.Da
	//   Dca' = Sa.Da + Sca.(1 - Da) + Dca.(1 - Sa)
	// otherwise
	//   Dca' = Dca.Sa/(1-Sca/Sa) + Sca.(1 - Da) + Dca.(1 - Sa)
	//
	// Da'  = Sa + Da - Sa.Da
	static void BlendPixel(value_type* p,
									 uint sr, uint sg, uint sb,
									 uint sa, uint cover)
	{
		if(cover < 255)
		{
			sr = (sr * cover + 255) >> 8;
			sg = (sg * cover + 255) >> 8;
			sb = (sb * cover + 255) >> 8;
			sa = (sa * cover + 255) >> 8;
		}
		if(sa)
		{
			calc_type d1a  = BaseMask - p[Order::A];
			calc_type s1a  = BaseMask - sa;
			calc_type dr   = p[Order::R];
			calc_type dg   = p[Order::G];
			calc_type db   = p[Order::B];
			calc_type da   = p[Order::A];
			long_type drsa = dr * sa;
			long_type dgsa = dg * sa;
			long_type dbsa = db * sa;
			long_type srda = sr * da;
			long_type sgda = sg * da;
			long_type sbda = sb * da;
			long_type sada = sa * da;

			p[Order::R] = (value_type)((srda + drsa >= sada) ?
				(sada + sr * d1a + dr * s1a + BaseMask) >> BaseShift :
				drsa / (BaseMask - (sr << BaseShift) / sa) + ((sr * d1a + dr * s1a + BaseMask) >> BaseShift));

			p[Order::G] = (value_type)((sgda + dgsa >= sada) ?
				(sada + sg * d1a + dg * s1a + BaseMask) >> BaseShift :
				dgsa / (BaseMask - (sg << BaseShift) / sa) + ((sg * d1a + dg * s1a + BaseMask) >> BaseShift));

			p[Order::B] = (value_type)((sbda + dbsa >= sada) ?
				(sada + sb * d1a + db * s1a + BaseMask) >> BaseShift :
				dbsa / (BaseMask - (sb << BaseShift) / sa) + ((sb * d1a + db * s1a + BaseMask) >> BaseShift));

			p[Order::A] = (value_type)(sa + da - ((sa * da + BaseMask) >> BaseShift));
		}
	}
};

//=====================================================comp_op_rgba_color_burn
template<class ColorT, class Order> struct comp_op_rgba_color_burn
{
	typedef ColorT color_type;
	typedef Order order_type;
	typedef typename color_type::value_type value_type;
	typedef typename color_type::calc_type calc_type;
	typedef typename color_type::long_type long_type;
	enum base_scale_e
	{
		BaseShift = color_type::BaseShift,
		BaseMask  = color_type::BaseMask
	};

	// if Sca.Da + Dca.Sa <= Sa.Da
	//   Dca' = Sca.(1 - Da) + Dca.(1 - Sa)
	// otherwise
	//   Dca' = Sa.(Sca.Da + Dca.Sa - Sa.Da)/Sca + Sca.(1 - Da) + Dca.(1 - Sa)
	//
	// Da'  = Sa + Da - Sa.Da
	static void BlendPixel(value_type* p,
									 uint sr, uint sg, uint sb,
									 uint sa, uint cover)
	{
		if(cover < 255)
		{
			sr = (sr * cover + 255) >> 8;
			sg = (sg * cover + 255) >> 8;
			sb = (sb * cover + 255) >> 8;
			sa = (sa * cover + 255) >> 8;
		}
		if(sa)
		{
			calc_type d1a  = BaseMask - p[Order::A];
			calc_type s1a  = BaseMask - sa;
			calc_type dr   = p[Order::R];
			calc_type dg   = p[Order::G];
			calc_type db   = p[Order::B];
			calc_type da   = p[Order::A];
			long_type drsa = dr * sa;
			long_type dgsa = dg * sa;
			long_type dbsa = db * sa;
			long_type srda = sr * da;
			long_type sgda = sg * da;
			long_type sbda = sb * da;
			long_type sada = sa * da;

			p[Order::R] = (value_type)(((srda + drsa <= sada) ?
				sr * d1a + dr * s1a :
				sa * (srda + drsa - sada) / sr + sr * d1a + dr * s1a + BaseMask) >> BaseShift);

			p[Order::G] = (value_type)(((sgda + dgsa <= sada) ?
				sg * d1a + dg * s1a :
				sa * (sgda + dgsa - sada) / sg + sg * d1a + dg * s1a + BaseMask) >> BaseShift);

			p[Order::B] = (value_type)(((sbda + dbsa <= sada) ?
				sb * d1a + db * s1a :
				sa * (sbda + dbsa - sada) / sb + sb * d1a + db * s1a + BaseMask) >> BaseShift);

			p[Order::A] = (value_type)(sa + da - ((sa * da + BaseMask) >> BaseShift));
		}
	}
};

//=====================================================comp_op_rgba_hard_light
template<class ColorT, class Order> struct comp_op_rgba_hard_light
{
	typedef ColorT color_type;
	typedef Order order_type;
	typedef typename color_type::value_type value_type;
	typedef typename color_type::calc_type calc_type;
	typedef typename color_type::long_type long_type;
	enum base_scale_e
	{
		BaseShift = color_type::BaseShift,
		BaseMask  = color_type::BaseMask
	};

	// if 2.Sca < Sa
	//    Dca' = 2.Sca.Dca + Sca.(1 - Da) + Dca.(1 - Sa)
	// otherwise
	//    Dca' = Sa.Da - 2.(Da - Dca).(Sa - Sca) + Sca.(1 - Da) + Dca.(1 - Sa)
	//
	// Da'  = Sa + Da - Sa.Da
	static void BlendPixel(value_type* p,
									 uint sr, uint sg, uint sb,
									 uint sa, uint cover)
	{
		if(cover < 255)
		{
			sr = (sr * cover + 255) >> 8;
			sg = (sg * cover + 255) >> 8;
			sb = (sb * cover + 255) >> 8;
			sa = (sa * cover + 255) >> 8;
		}
		if(sa)
		{
			calc_type d1a  = BaseMask - p[Order::A];
			calc_type s1a  = BaseMask - sa;
			calc_type dr   = p[Order::R];
			calc_type dg   = p[Order::G];
			calc_type db   = p[Order::B];
			calc_type da   = p[Order::A];
			calc_type sada = sa * da;

			p[Order::R] = (value_type)(((2*sr < sa) ?
				2*sr*dr + sr*d1a + dr*s1a :
				sada - 2*(da - dr)*(sa - sr) + sr*d1a + dr*s1a + BaseMask) >> BaseShift);

			p[Order::G] = (value_type)(((2*sg < sa) ?
				2*sg*dg + sg*d1a + dg*s1a :
				sada - 2*(da - dg)*(sa - sg) + sg*d1a + dg*s1a + BaseMask) >> BaseShift);

			p[Order::B] = (value_type)(((2*sb < sa) ?
				2*sb*db + sb*d1a + db*s1a :
				sada - 2*(da - db)*(sa - sb) + sb*d1a + db*s1a + BaseMask) >> BaseShift);

			p[Order::A] = (value_type)(sa + da - ((sa * da + BaseMask) >> BaseShift));
		}
	}
};

//=====================================================comp_op_rgba_soft_light
template<class ColorT, class Order> struct comp_op_rgba_soft_light
{
	typedef ColorT color_type;
	typedef Order order_type;
	typedef typename color_type::value_type value_type;
	typedef typename color_type::calc_type calc_type;
	typedef typename color_type::long_type long_type;
	enum base_scale_e
	{
		BaseShift = color_type::BaseShift,
		BaseMask  = color_type::BaseMask
	};

	// if 2.Sca < Sa
	//   Dca' = Dca.(Sa + (1 - Dca/Da).(2.Sca - Sa)) + Sca.(1 - Da) + Dca.(1 - Sa)
	// otherwise if 8.Dca <= Da
	//   Dca' = Dca.(Sa + (1 - Dca/Da).(2.Sca - Sa).(3 - 8.Dca/Da)) + Sca.(1 - Da) + Dca.(1 - Sa)
	// otherwise
	//   Dca' = (Dca.Sa + ((Dca/Da)^(0.5).Da - Dca).(2.Sca - Sa)) + Sca.(1 - Da) + Dca.(1 - Sa)
	//
	// Da'  = Sa + Da - Sa.Da

	static void BlendPixel(value_type* p,
									 uint r, uint g, uint b,
									 uint a, uint cover)
	{
		double sr = double(r * cover) / (BaseMask * 255);
		double sg = double(g * cover) / (BaseMask * 255);
		double sb = double(b * cover) / (BaseMask * 255);
		double sa = double(a * cover) / (BaseMask * 255);
		if(sa > 0)
		{
			double dr = double(p[Order::R]) / BaseMask;
			double dg = double(p[Order::G]) / BaseMask;
			double db = double(p[Order::B]) / BaseMask;
			double da = double(p[Order::A] ? p[Order::A] : 1) / BaseMask;
			if(cover < 255)
			{
				a = (a * cover + 255) >> 8;
			}

			if(2*sr < sa)       dr = dr*(sa + (1 - dr/da)*(2*sr - sa)) + sr*(1 - da) + dr*(1 - sa);
			else if(8*dr <= da) dr = dr*(sa + (1 - dr/da)*(2*sr - sa)*(3 - 8*dr/da)) + sr*(1 - da) + dr*(1 - sa);
			else                dr = (dr*sa + (sqrt(dr/da)*da - dr)*(2*sr - sa)) + sr*(1 - da) + dr*(1 - sa);

			if(2*sg < sa)       dg = dg*(sa + (1 - dg/da)*(2*sg - sa)) + sg*(1 - da) + dg*(1 - sa);
			else if(8*dg <= da) dg = dg*(sa + (1 - dg/da)*(2*sg - sa)*(3 - 8*dg/da)) + sg*(1 - da) + dg*(1 - sa);
			else                dg = (dg*sa + (sqrt(dg/da)*da - dg)*(2*sg - sa)) + sg*(1 - da) + dg*(1 - sa);

			if(2*sb < sa)       db = db*(sa + (1 - db/da)*(2*sb - sa)) + sb*(1 - da) + db*(1 - sa);
			else if(8*db <= da) db = db*(sa + (1 - db/da)*(2*sb - sa)*(3 - 8*db/da)) + sb*(1 - da) + db*(1 - sa);
			else                db = (db*sa + (sqrt(db/da)*da - db)*(2*sb - sa)) + sb*(1 - da) + db*(1 - sa);

			p[Order::R] = (value_type)UnsignedRound(dr * BaseMask);
			p[Order::G] = (value_type)UnsignedRound(dg * BaseMask);
			p[Order::B] = (value_type)UnsignedRound(db * BaseMask);
			p[Order::A] = (value_type)(a + p[Order::A] - ((a * p[Order::A] + BaseMask) >> BaseShift));
		}
	}
};

//=====================================================comp_op_rgba_difference
template<class ColorT, class Order> struct comp_op_rgba_difference
{
	typedef ColorT color_type;
	typedef Order order_type;
	typedef typename color_type::value_type value_type;
	typedef typename color_type::calc_type calc_type;
	typedef typename color_type::long_type long_type;
	enum base_scale_e
	{
		BaseShift = color_type::BaseShift,
		base_scale = color_type::base_scale,
		BaseMask  = color_type::BaseMask
	};

	// Dca' = Sca + Dca - 2.min(Sca.Da, Dca.Sa)
	// Da'  = Sa + Da - Sa.Da
	static void BlendPixel(value_type* p,
									 uint sr, uint sg, uint sb,
									 uint sa, uint cover)
	{
		if(cover < 255)
		{
			sr = (sr * cover + 255) >> 8;
			sg = (sg * cover + 255) >> 8;
			sb = (sb * cover + 255) >> 8;
			sa = (sa * cover + 255) >> 8;
		}
		if(sa)
		{
			calc_type dr = p[Order::R];
			calc_type dg = p[Order::G];
			calc_type db = p[Order::B];
			calc_type da = p[Order::A];
			p[Order::R] = (value_type)(sr + dr - ((2 * sd_min(sr*da, dr*sa) + BaseMask) >> BaseShift));
			p[Order::G] = (value_type)(sg + dg - ((2 * sd_min(sg*da, dg*sa) + BaseMask) >> BaseShift));
			p[Order::B] = (value_type)(sb + db - ((2 * sd_min(sb*da, db*sa) + BaseMask) >> BaseShift));
			p[Order::A] = (value_type)(sa + da - ((sa * da + BaseMask) >> BaseShift));
		}
	}
};

//=====================================================comp_op_rgba_exclusion
template<class ColorT, class Order> struct comp_op_rgba_exclusion
{
	typedef ColorT color_type;
	typedef Order order_type;
	typedef typename color_type::value_type value_type;
	typedef typename color_type::calc_type calc_type;
	typedef typename color_type::long_type long_type;
	enum base_scale_e
	{
		BaseShift = color_type::BaseShift,
		BaseMask  = color_type::BaseMask
	};

	// Dca' = (Sca.Da + Dca.Sa - 2.Sca.Dca) + Sca.(1 - Da) + Dca.(1 - Sa)
	// Da'  = Sa + Da - Sa.Da
	static void BlendPixel(value_type* p,
									 uint sr, uint sg, uint sb,
									 uint sa, uint cover)
	{
		if(cover < 255)
		{
			sr = (sr * cover + 255) >> 8;
			sg = (sg * cover + 255) >> 8;
			sb = (sb * cover + 255) >> 8;
			sa = (sa * cover + 255) >> 8;
		}
		if(sa)
		{
			calc_type d1a = BaseMask - p[Order::A];
			calc_type s1a = BaseMask - sa;
			calc_type dr = p[Order::R];
			calc_type dg = p[Order::G];
			calc_type db = p[Order::B];
			calc_type da = p[Order::A];
			p[Order::R] = (value_type)((sr*da + dr*sa - 2*sr*dr + sr*d1a + dr*s1a + BaseMask) >> BaseShift);
			p[Order::G] = (value_type)((sg*da + dg*sa - 2*sg*dg + sg*d1a + dg*s1a + BaseMask) >> BaseShift);
			p[Order::B] = (value_type)((sb*da + db*sa - 2*sb*db + sb*d1a + db*s1a + BaseMask) >> BaseShift);
			p[Order::A] = (value_type)(sa + da - ((sa * da + BaseMask) >> BaseShift));
		}
	}
};

//=====================================================comp_op_rgba_contrast
template<class ColorT, class Order> struct comp_op_rgba_contrast
{
	typedef ColorT color_type;
	typedef Order order_type;
	typedef typename color_type::value_type value_type;
	typedef typename color_type::calc_type calc_type;
	typedef typename color_type::long_type long_type;
	enum base_scale_e
	{
		BaseShift = color_type::BaseShift,
		BaseMask  = color_type::BaseMask
	};

	static void BlendPixel(value_type* p,
									 uint sr, uint sg, uint sb,
									 uint sa, uint cover)
	{
		if(cover < 255)
		{
			sr = (sr * cover + 255) >> 8;
			sg = (sg * cover + 255) >> 8;
			sb = (sb * cover + 255) >> 8;
			sa = (sa * cover + 255) >> 8;
		}
		long_type dr = p[Order::R];
		long_type dg = p[Order::G];
		long_type db = p[Order::B];
		int       da = p[Order::A];
		long_type d2a = da >> 1;
		uint s2a = sa >> 1;

		int r = (int)((((dr - d2a) * int((sr - s2a)*2 + BaseMask)) >> BaseShift) + d2a);
		int g = (int)((((dg - d2a) * int((sg - s2a)*2 + BaseMask)) >> BaseShift) + d2a);
		int b = (int)((((db - d2a) * int((sb - s2a)*2 + BaseMask)) >> BaseShift) + d2a);

		r = (r < 0) ? 0 : r;
		g = (g < 0) ? 0 : g;
		b = (b < 0) ? 0 : b;

		p[Order::R] = (value_type)((r > da) ? da : r);
		p[Order::G] = (value_type)((g > da) ? da : g);
		p[Order::B] = (value_type)((b > da) ? da : b);
	}
};

//=====================================================comp_op_rgba_invert
template<class ColorT, class Order> struct comp_op_rgba_invert
{
	typedef ColorT color_type;
	typedef Order order_type;
	typedef typename color_type::value_type value_type;
	typedef typename color_type::calc_type calc_type;
	typedef typename color_type::long_type long_type;
	enum base_scale_e
	{
		BaseShift = color_type::BaseShift,
		BaseMask  = color_type::BaseMask
	};

	// Dca' = (Da - Dca) * Sa + Dca.(1 - Sa)
	// Da'  = Sa + Da - Sa.Da
	static void BlendPixel(value_type* p,
									 uint sr, uint sg, uint sb,
									 uint sa, uint cover)
	{
		sa = (sa * cover + 255) >> 8;
		if(sa)
		{
			calc_type da = p[Order::A];
			calc_type dr = ((da - p[Order::R]) * sa + BaseMask) >> BaseShift;
			calc_type dg = ((da - p[Order::G]) * sa + BaseMask) >> BaseShift;
			calc_type db = ((da - p[Order::B]) * sa + BaseMask) >> BaseShift;
			calc_type s1a = BaseMask - sa;
			p[Order::R] = (value_type)(dr + ((p[Order::R] * s1a + BaseMask) >> BaseShift));
			p[Order::G] = (value_type)(dg + ((p[Order::G] * s1a + BaseMask) >> BaseShift));
			p[Order::B] = (value_type)(db + ((p[Order::B] * s1a + BaseMask) >> BaseShift));
			p[Order::A] = (value_type)(sa + da - ((sa * da + BaseMask) >> BaseShift));
		}
	}
};

//=================================================comp_op_rgba_invert_rgb
template<class ColorT, class Order> struct comp_op_rgba_invert_rgb
{
	typedef ColorT color_type;
	typedef Order order_type;
	typedef typename color_type::value_type value_type;
	typedef typename color_type::calc_type calc_type;
	typedef typename color_type::long_type long_type;
	enum base_scale_e
	{
		BaseShift = color_type::BaseShift,
		BaseMask  = color_type::BaseMask
	};

	// Dca' = (Da - Dca) * Sca + Dca.(1 - Sa)
	// Da'  = Sa + Da - Sa.Da
	static void BlendPixel(value_type* p,
									 uint sr, uint sg, uint sb,
									 uint sa, uint cover)
	{
		if(cover < 255)
		{
			sr = (sr * cover + 255) >> 8;
			sg = (sg * cover + 255) >> 8;
			sb = (sb * cover + 255) >> 8;
			sa = (sa * cover + 255) >> 8;
		}
		if(sa)
		{
			calc_type da = p[Order::A];
			calc_type dr = ((da - p[Order::R]) * sr + BaseMask) >> BaseShift;
			calc_type dg = ((da - p[Order::G]) * sg + BaseMask) >> BaseShift;
			calc_type db = ((da - p[Order::B]) * sb + BaseMask) >> BaseShift;
			calc_type s1a = BaseMask - sa;
			p[Order::R] = (value_type)(dr + ((p[Order::R] * s1a + BaseMask) >> BaseShift));
			p[Order::G] = (value_type)(dg + ((p[Order::G] * s1a + BaseMask) >> BaseShift));
			p[Order::B] = (value_type)(db + ((p[Order::B] * s1a + BaseMask) >> BaseShift));
			p[Order::A] = (value_type)(sa + da - ((sa * da + BaseMask) >> BaseShift));
		}
	}
};

//======================================================comp_op_table_rgba
template<class ColorT, class Order> struct comp_op_table_rgba
{
	typedef typename ColorT::value_type value_type;
	typedef void (*comp_op_func_type)(value_type* p,
									  uint cr,
									  uint cg,
									  uint cb,
									  uint ca,
									  uint cover);
	static comp_op_func_type g_comp_op_func[];
};

//==========================================================g_comp_op_func
template<class ColorT, class Order>
typename comp_op_table_rgba<ColorT, Order>::comp_op_func_type
comp_op_table_rgba<ColorT, Order>::g_comp_op_func[] =
{
	comp_op_rgba_clear      <ColorT,Order>::BlendPixel,
	comp_op_rgba_src        <ColorT,Order>::BlendPixel,
	comp_op_rgba_dst        <ColorT,Order>::BlendPixel,
	comp_op_rgba_src_over   <ColorT,Order>::BlendPixel,
	comp_op_rgba_dst_over   <ColorT,Order>::BlendPixel,
	comp_op_rgba_src_in     <ColorT,Order>::BlendPixel,
	comp_op_rgba_dst_in     <ColorT,Order>::BlendPixel,
	comp_op_rgba_src_out    <ColorT,Order>::BlendPixel,
	comp_op_rgba_dst_out    <ColorT,Order>::BlendPixel,
	comp_op_rgba_src_atop   <ColorT,Order>::BlendPixel,
	comp_op_rgba_dst_atop   <ColorT,Order>::BlendPixel,
	comp_op_rgba_xor        <ColorT,Order>::BlendPixel,
	comp_op_rgba_plus       <ColorT,Order>::BlendPixel,
	comp_op_rgba_minus      <ColorT,Order>::BlendPixel,
	comp_op_rgba_multiply   <ColorT,Order>::BlendPixel,
	comp_op_rgba_screen     <ColorT,Order>::BlendPixel,
	comp_op_rgba_overlay    <ColorT,Order>::BlendPixel,
	comp_op_rgba_darken     <ColorT,Order>::BlendPixel,
	comp_op_rgba_lighten    <ColorT,Order>::BlendPixel,
	comp_op_rgba_color_dodge<ColorT,Order>::BlendPixel,
	comp_op_rgba_color_burn <ColorT,Order>::BlendPixel,
	comp_op_rgba_hard_light <ColorT,Order>::BlendPixel,
	comp_op_rgba_soft_light <ColorT,Order>::BlendPixel,
	comp_op_rgba_difference <ColorT,Order>::BlendPixel,
	comp_op_rgba_exclusion  <ColorT,Order>::BlendPixel,
	comp_op_rgba_contrast   <ColorT,Order>::BlendPixel,
	comp_op_rgba_invert     <ColorT,Order>::BlendPixel,
	comp_op_rgba_invert_rgb <ColorT,Order>::BlendPixel,
	0
};

//==============================================================comp_op_e
enum comp_op_e
{
	comp_op_clear,         //----comp_op_clear
	comp_op_src,           //----comp_op_src
	comp_op_dst,           //----comp_op_dst
	comp_op_src_over,      //----comp_op_src_over
	comp_op_dst_over,      //----comp_op_dst_over
	comp_op_src_in,        //----comp_op_src_in
	comp_op_dst_in,        //----comp_op_dst_in
	comp_op_src_out,       //----comp_op_src_out
	comp_op_dst_out,       //----comp_op_dst_out
	comp_op_src_atop,      //----comp_op_src_atop
	comp_op_dst_atop,      //----comp_op_dst_atop
	comp_op_xor,           //----comp_op_xor
	comp_op_plus,          //----comp_op_plus
	comp_op_minus,         //----comp_op_minus
	comp_op_multiply,      //----comp_op_multiply
	comp_op_screen,        //----comp_op_screen
	comp_op_overlay,       //----comp_op_overlay
	comp_op_darken,        //----comp_op_darken
	comp_op_lighten,       //----comp_op_lighten
	comp_op_color_dodge,   //----comp_op_color_dodge
	comp_op_color_burn,    //----comp_op_color_burn
	comp_op_hard_light,    //----comp_op_hard_light
	comp_op_soft_light,    //----comp_op_soft_light
	comp_op_difference,    //----comp_op_difference
	comp_op_exclusion,     //----comp_op_exclusion
	comp_op_contrast,      //----comp_op_contrast
	comp_op_invert,        //----comp_op_invert
	comp_op_invert_rgb,    //----comp_op_invert_rgb

	end_of_comp_op_e
};

//====================================================comp_op_adaptor_rgba
template<class ColorT, class Order> struct comp_op_adaptor_rgba
{
	typedef Order  order_type;
	typedef ColorT color_type;
	typedef typename color_type::value_type value_type;
	enum base_scale_e
	{
		BaseShift = color_type::BaseShift,
		BaseMask  = color_type::BaseMask
	};

	static void BlendPixel(uint op, value_type* p,
									 uint cr, uint cg, uint cb,
									 uint ca,
									 uint cover)
	{
		comp_op_table_rgba<ColorT, Order>::g_comp_op_func[op]
			(p, (cr * ca + BaseMask) >> BaseShift,
				(cg * ca + BaseMask) >> BaseShift,
				(cb * ca + BaseMask) >> BaseShift,
				 ca, cover);
	}
};

//=========================================comp_op_adaptor_clip_to_dst_rgba
template<class ColorT, class Order> struct comp_op_adaptor_clip_to_dst_rgba
{
	typedef Order  order_type;
	typedef ColorT color_type;
	typedef typename color_type::value_type value_type;
	enum base_scale_e
	{
		BaseShift = color_type::BaseShift,
		BaseMask  = color_type::BaseMask
	};

	static void BlendPixel(uint op, value_type* p,
									 uint cr, uint cg, uint cb,
									 uint ca,
									 uint cover)
	{
		cr = (cr * ca + BaseMask) >> BaseShift;
		cg = (cg * ca + BaseMask) >> BaseShift;
		cb = (cb * ca + BaseMask) >> BaseShift;
		uint da = p[Order::A];
		comp_op_table_rgba<ColorT, Order>::g_comp_op_func[op]
			(p, (cr * da + BaseMask) >> BaseShift,
				(cg * da + BaseMask) >> BaseShift,
				(cb * da + BaseMask) >> BaseShift,
				(ca * da + BaseMask) >> BaseShift,
				cover);
	}
};

//================================================comp_op_adaptor_rgba_pre
template<class ColorT, class Order> struct comp_op_adaptor_rgba_pre
{
	typedef Order  order_type;
	typedef ColorT color_type;
	typedef typename color_type::value_type value_type;
	enum base_scale_e
	{
		BaseShift = color_type::BaseShift,
		BaseMask  = color_type::BaseMask
	};

	static void BlendPixel(uint op, value_type* p,
									 uint cr, uint cg, uint cb,
									 uint ca,
									 uint cover)
	{
		comp_op_table_rgba<ColorT, Order>::g_comp_op_func[op](p, cr, cg, cb, ca, cover);
	}
};

//=====================================comp_op_adaptor_clip_to_dst_rgba_pre
template<class ColorT, class Order> struct comp_op_adaptor_clip_to_dst_rgba_pre
{
	typedef Order  order_type;
	typedef ColorT color_type;
	typedef typename color_type::value_type value_type;
	enum base_scale_e
	{
		BaseShift = color_type::BaseShift,
		BaseMask  = color_type::BaseMask
	};

	static void BlendPixel(uint op, value_type* p,
									 uint cr, uint cg, uint cb,
									 uint ca,
									 uint cover)
	{
		uint da = p[Order::A];
		comp_op_table_rgba<ColorT, Order>::g_comp_op_func[op]
			(p, (cr * da + BaseMask) >> BaseShift,
				(cg * da + BaseMask) >> BaseShift,
				(cb * da + BaseMask) >> BaseShift,
				(ca * da + BaseMask) >> BaseShift,
				cover);
	}
};

//=======================================================comp_adaptor_rgba
template<class BlenderPre> struct comp_adaptor_rgba
{
	typedef typename BlenderPre::order_type order_type;
	typedef typename BlenderPre::color_type color_type;
	typedef typename color_type::value_type value_type;
	enum base_scale_e
	{
		BaseShift = color_type::BaseShift,
		BaseMask  = color_type::BaseMask
	};

	static void BlendPixel(uint op, value_type* p,
									 uint cr, uint cg, uint cb,
									 uint ca,
									 uint cover)
	{
		BlenderPre::BlendPixel(p,
							  (cr * ca + BaseMask) >> BaseShift,
							  (cg * ca + BaseMask) >> BaseShift,
							  (cb * ca + BaseMask) >> BaseShift,
							  ca, cover);
	}
};

//==========================================comp_adaptor_clip_to_dst_rgba
template<class BlenderPre> struct comp_adaptor_clip_to_dst_rgba
{
	typedef typename BlenderPre::order_type order_type;
	typedef typename BlenderPre::color_type color_type;
	typedef typename color_type::value_type value_type;
	enum base_scale_e
	{
		BaseShift = color_type::BaseShift,
		BaseMask  = color_type::BaseMask
	};

	static void BlendPixel(uint op, value_type* p,
									 uint cr, uint cg, uint cb,
									 uint ca,
									 uint cover)
	{
		cr = (cr * ca + BaseMask) >> BaseShift;
		cg = (cg * ca + BaseMask) >> BaseShift;
		cb = (cb * ca + BaseMask) >> BaseShift;
		uint da = p[OrderA];
		BlenderPre::BlendPixel(p,
							  (cr * da + BaseMask) >> BaseShift,
							  (cg * da + BaseMask) >> BaseShift,
							  (cb * da + BaseMask) >> BaseShift,
							  (ca * da + BaseMask) >> BaseShift,
							  cover);
	}
};

//======================================comp_adaptor_clip_to_dst_rgba_pre
template<class BlenderPre> struct comp_adaptor_clip_to_dst_rgba_pre
{
	typedef typename BlenderPre::order_type order_type;
	typedef typename BlenderPre::color_type color_type;
	typedef typename color_type::value_type value_type;
	enum base_scale_e
	{
		BaseShift = color_type::BaseShift,
		BaseMask  = color_type::BaseMask
	};

	static void BlendPixel(uint op, value_type* p,
									 uint cr, uint cg, uint cb,
									 uint ca,
									 uint cover)
	{
		uint da = p[OrderA];
		BlenderPre::BlendPixel(p,
							  (cr * da + BaseMask) >> BaseShift,
							  (cg * da + BaseMask) >> BaseShift,
							  (cb * da + BaseMask) >> BaseShift,
							  (ca * da + BaseMask) >> BaseShift,
							  cover);
	}
};

*/

	//===============================================copy_or_blend_rgba_wrapper
	//template<class Blender>
	public static class CopyOrBlendBGRAWrapper
	{
		//private IBlender m_Blender;
		//typedef typename Blender::color_type color_type;
		//typedef typename Blender::order_type order_type;
		//typedef typename color_type::value_type value_type;
		//typedef typename color_type::calc_type calc_type;

		private enum Order
		{
			R = 2,
			G = 1,
			B = 0,
			A = 3
		};

		private const byte base_mask = 255;

		//--------------------------------------------------------------------
		public unsafe static void CopyOrBlendPixel(IBlender Blender, byte* p, uint cr, uint cg, uint cb, uint alpha)
		{
			if (alpha != 0)
			{
				if (alpha == base_mask)
				{
					p[(int)Order.R] = (byte)cr;
					p[(int)Order.G] = (byte)cg;
					p[(int)Order.B] = (byte)cb;
					p[(int)Order.A] = (byte)base_mask;
				}
				else
				{
					Blender.BlendPixel(p, cr, cg, cb, alpha);
				}
			}
		}

		//--------------------------------------------------------------------
		public unsafe static void CopyOrBlendPixel(IBlender Blender, byte* p, uint cr, uint cg, uint cb, uint alpha, uint cover)
		{
			if (cover == 255)
			{
				CopyOrBlendPixel(Blender, p, cr, cg, cb, alpha);
			}
			else
			{
				if (alpha != 0)
				{
					alpha = (alpha * (cover + 1)) >> 8;
					if (alpha == base_mask)
					{
						p[(int)Order.R] = (byte)cr;
						p[(int)Order.G] = (byte)cg;
						p[(int)Order.B] = (byte)cb;
						p[(int)Order.A] = (byte)base_mask;
					}
					else
					{
						Blender.BlendPixel(p, cr, cg, cb, alpha, cover);
					}
				}
			}
		}
	};

	//=================================================pixfmt_alpha_blend_rgba
	//template<class Blender, class RenBuf, class PixelT = int32u>
	public sealed class FormatRGBA : IPixelFormat
	{
		private RasterBuffer m_rbuf;
		private IBlender m_Blender;
		private int OrderR;
		private int OrderG;
		private int OrderB;
		private int OrderA;

		private const byte base_mask = 255;

		public uint PixelWidthInBytes { get { return 4; } }

		//--------------------------------------------------------------------
		public FormatRGBA(RasterBuffer rb, IBlender blender, GammaLookupTable gammaTable)
			: this(rb, blender)
		{
			throw new System.NotImplementedException("Not using the Gamma table yet.");
		}

		public FormatRGBA(RasterBuffer rb, IBlender blender)
		{
			m_rbuf = rb;
			Blender = blender;
		}

		public IBlender Blender
		{
			get
			{
				return m_Blender;
			}

			set
			{
				if (value.NumPixelBits != 32)
				{
					throw new System.NotSupportedException("pixfmt_alpha_blend_rgba32 requires your Blender to be 32 bit. Change the Blender or the Pixel Format");
				}
				m_Blender = value;
				OrderR = m_Blender.OrderR;
				OrderG = m_Blender.OrderG;
				OrderB = m_Blender.OrderB;
				OrderA = m_Blender.OrderA;
			}
		}

		public void attach(RasterBuffer rb)
		{
			m_rbuf = rb;
		}

		//--------------------------------------------------------------------
		/*

		//template<class PixFmt>
		public bool Attach(PixFmt& pixf, int x1, int y1, int x2, int y2)
		{
			RectI r(x1, y1, x2, y2);
			if(r.Clip(RectI(0, 0, pixf.Width()-1, pixf.Height()-1)))
			{
				int Stride = pixf.Stride();
				m_rbuf->Attach(pixf.PixelPointer(r.x1, Stride < 0 ? r.y2 : r.y1),
							   (r.x2 - r.x1) + 1,
							   (r.y2 - r.y1) + 1,
							   Stride);
				return true;
			}
			return false;
		}
		 */

		//--------------------------------------------------------------------
		public uint Width
		{
			get { return m_rbuf.Width(); }
		}

		public uint Height
		{
			get { return m_rbuf.Height(); }
		}

		public int Stride
		{
			get { return m_rbuf.StrideInBytes(); }
		}

		//--------------------------------------------------------------------
		public RasterBuffer RenderingBuffer
		{
			get { return m_rbuf; }
		}

		unsafe public byte* RowPointer(int y)
		{
			return m_rbuf.GetPixelPointer(y);
		}

		//unsafe public const_row_info row(int y) { return m_rbuf.row(y); }

		//--------------------------------------------------------------------
		unsafe public byte* PixelPointer(int x, int y)
		{
			return m_rbuf.GetPixelPointer(y) + x * (int)PixelWidthInBytes;
		}

		//--------------------------------------------------------------------
		unsafe public void MakePixel(byte* p, IColorType c)
		{
			p[OrderR] = (byte)c.R_Byte;
			p[OrderG] = (byte)c.G_Byte;
			p[OrderB] = (byte)c.B_Byte;
			p[OrderA] = (byte)c.A_Byte;
		}

		public RGBA_Bytes Pixel(int x, int y)
		{
			unsafe
			{
				byte* p = m_rbuf.GetPixelPointer(y);
				if (p != null)
				{
					p += x << 2;
					return new RGBA_Bytes(p[OrderR], p[OrderG], p[OrderB], p[OrderA]);
				}
				return new RGBA_Bytes();
			}
		}

		/*

		//--------------------------------------------------------------------
		public IColorType Pixel(int x, int y)
		{
			value_type* p = (value_type*)m_rbuf->RowPointer(y);
			if(p)
			{
				p += x << 2;
				return p;

				//return color_type(p[OrderR],
				  //                p[OrderG],
					//              p[OrderB],
					  //            p[OrderA]);
				m_rbuf.ren();

				//ren()
			}
			return color_type::no_color();
		}
		 */

		//--------------------------------------------------------------------
		unsafe public void CopyPixel(int x, int y, byte* c)
		{
			byte* p = m_rbuf.GetPixelPointer(y) + (x << 2);
			((int*)p)[0] = ((int*)c)[0];

			//p[OrderR] = c.r;
			//p[OrderG] = c.g;
			//p[OrderB] = c.b;
			//p[OrderA] = c.a;
		}

		//--------------------------------------------------------------------
		public void BlendPixel(int x, int y, RGBA_Bytes c, byte cover)
		{
			/*
			cob_type::CopyOrBlendPixel(
				(value_type*)m_rbuf->RowPointer(x, y, 1) + (x << 2),
				c.r, c.g, c.b, c.a,
				cover);*/
		}

		//--------------------------------------------------------------------
		public void CopyHorizontalLine(int x, int y, uint len, RGBA_Bytes c)
		{
			unsafe
			{
				byte* p = (byte*)m_rbuf.GetPixelPointer(y) + (x << 2);
				uint v;
				((byte*)&v)[OrderR] = (byte)c.R_Byte;
				((byte*)&v)[OrderG] = (byte)c.G_Byte;
				((byte*)&v)[OrderB] = (byte)c.B_Byte;
				((byte*)&v)[OrderA] = (byte)c.A_Byte;
				do
				{
					*(uint*)p = v;
					p += 4;
				}
				while (--len != 0);
			}
		}

		//--------------------------------------------------------------------
		public void CopyVerticalLine(int x, int y, uint len, RGBA_Bytes c)
		{
			int ScanWidth = m_rbuf.StrideInBytes();
			unsafe
			{
				byte* p = (byte*)m_rbuf.GetPixelPointer(y) + (x << 2);
				uint v;
				((byte*)&v)[OrderR] = (byte)c.R_Byte;
				((byte*)&v)[OrderG] = (byte)c.G_Byte;
				((byte*)&v)[OrderB] = (byte)c.B_Byte;
				((byte*)&v)[OrderA] = (byte)c.A_Byte;
				do
				{
					*(uint*)p = v;
					p = &p[ScanWidth];
				}
				while (--len != 0);
			}
		}

		//--------------------------------------------------------------------
		public void BlendHorizontalLine(int x1, int y, int x2, RGBA_Bytes c, byte cover)
		{
			if (c.m_A != 0)
			{
				int len = x2 - x1 + 1;
				unsafe
				{
					byte* p = (byte*)m_rbuf.GetPixelPointer(y) + (x1 << 2);
					uint alpha = (uint)(((int)(c.m_A) * (cover + 1)) >> 8);
					if (alpha == base_mask)
					{
						uint v;
						((byte*)&v)[OrderR] = (byte)c.m_R;
						((byte*)&v)[OrderG] = (byte)c.m_G;
						((byte*)&v)[OrderB] = (byte)c.m_B;
						((byte*)&v)[OrderA] = (byte)c.m_A;
						do
						{
							*(uint*)p = v;
							p += 4;
						}
						while (--len != 0);
					}
					else
					{
						if (cover == 255)
						{
							do
							{
#if USE_BLENDER
								m_Blender.BlendPixel(p, c.m_R, c.m_G, c.m_B, alpha);
#else
								unchecked
								{
									uint b = p[0];
									uint g = p[1];
									uint r = p[2];
									uint a = p[3];
									p[0] = (byte)(((c.m_B - b) * alpha + (b << (int)rgba8.base_shift)) >> (int)rgba8.base_shift);
									p[1] = (byte)(((c.m_G - g) * alpha + (g << (int)rgba8.base_shift)) >> (int)rgba8.base_shift);
									p[2] = (byte)(((uint)(c.m_R - r) * alpha + (r << (int)rgba8.base_shift)) >> (int)rgba8.base_shift);
									p[3] = (byte)((alpha + a) - ((alpha * a + base_mask) >> (int)rgba8.base_shift));
								}
#endif
								p += 4;
							}
							while (--len != 0);
						}
						else
						{
							do
							{
#if USE_BLENDER
								m_Blender.BlendPixel(p, c.m_R, c.m_G, c.m_B, alpha);
#else
								unchecked
								{
									uint b = p[0];
									uint g = p[1];
									uint r = p[2];
									uint a = p[3];
									p[0] = (byte)(((c.m_B - b) * alpha + (b << (int)rgba8.base_shift)) >> (int)rgba8.base_shift);
									p[1] = (byte)(((c.m_G - g) * alpha + (g << (int)rgba8.base_shift)) >> (int)rgba8.base_shift);
									p[2] = (byte)(((uint)(c.m_R - r) * alpha + (r << (int)rgba8.base_shift)) >> (int)rgba8.base_shift);
									p[3] = (byte)((alpha + a) - ((alpha * a + base_mask) >> (int)rgba8.base_shift));
								}
#endif
								p += 4;
							}
							while (--len != 0);
						}
					}
				}
			}
		}

		//--------------------------------------------------------------------
		public void BlendVerticalLine(int x, int y1, int y2, RGBA_Bytes c, byte cover)
		{
			if (c.m_A != 0)
			{
				int ScanWidth = m_rbuf.StrideInBytes();
				unsafe
				{
					int len = y2 - y1 + 1;
					byte* p = (byte*)m_rbuf.GetPixelPointer(y1) + (x << 2);
					uint alpha = (uint)(((int)(c.m_A) * (cover + 1)) >> 8);
					if (alpha == base_mask)
					{
						uint v;
						((byte*)&v)[OrderR] = (byte)c.m_R;
						((byte*)&v)[OrderG] = (byte)c.m_G;
						((byte*)&v)[OrderB] = (byte)c.m_B;
						((byte*)&v)[OrderA] = (byte)c.m_A;
						do
						{
							*(uint*)p = v;
							p = &p[ScanWidth];
						}
						while (--len != 0);
					}
					else
					{
						if (cover == 255)
						{
							do
							{
								m_Blender.BlendPixel(p, c.m_R, c.m_G, c.m_B, alpha);
								p = &p[ScanWidth];
							}
							while (--len != 0);
						}
						else
						{
							do
							{
								m_Blender.BlendPixel(p, c.m_R, c.m_G, c.m_B, alpha);
								p = &p[ScanWidth];
							}
							while (--len != 0);
						}
					}
				}
			}
		}

		//--------------------------------------------------------------------
		unsafe public void BlendSolidHorizontalSpan(int x, int y, uint len, RGBA_Bytes c, byte* covers)
		{
			if (c.m_A != 0)
			{
				unchecked
				{
					byte* p = (byte*)m_rbuf.GetPixelPointer(y) + (x << 2);
					do
					{
						uint alpha = ((uint)(c.m_A) * ((uint)(*covers) + 1)) >> 8;
						if (alpha == base_mask)
						{
							p[OrderR] = c.m_R;
							p[OrderG] = c.m_G;
							p[OrderB] = c.m_B;
							p[OrderA] = base_mask;
						}
						else
						{
#if USE_BLENDER
							m_Blender.BlendPixel(p, c.m_R, c.m_G, c.m_B, alpha);
#else
							 uint b = p[0];
							 uint g = p[1];
							 uint r = p[2];
							 uint a = p[3];
							 p[0] = (byte)(((c.m_B - b) * alpha + (b << (int)rgba8.base_shift)) >> (int)rgba8.base_shift);
							 p[1] = (byte)(((c.m_G - g) * alpha + (g << (int)rgba8.base_shift)) >> (int)rgba8.base_shift);
							 p[2] = (byte)(((uint)(c.m_R - r) * alpha + (r << (int)rgba8.base_shift)) >> (int)rgba8.base_shift);
							 p[3] = (byte)((alpha + a) - ((alpha * a + base_mask) >> (int)rgba8.base_shift));
#endif
						}
						p += 4;
						++covers;
					}
					while (--len != 0);
				}
			}
		}

		//--------------------------------------------------------------------
		public unsafe void BlendSolidVerticalSpan(int x, int y, uint len, RGBA_Bytes c, byte* covers)
		{
			if (c.m_A != 0)
			{
				int ScanWidth = m_rbuf.StrideInBytes();
				unchecked
				{
					byte* p = (byte*)m_rbuf.GetPixelPointer(y) + (x << 2);
					do
					{
						uint alpha = ((uint)(c.m_A) * ((uint)(*covers) + 1)) >> 8;
						if (alpha == base_mask)
						{
							p[OrderR] = c.m_R;
							p[OrderG] = c.m_G;
							p[OrderB] = c.m_B;
							p[OrderA] = base_mask;
						}
						else
						{
							m_Blender.BlendPixel(p, c.m_R, c.m_G, c.m_B, alpha);
						}
						p = &p[ScanWidth];
						++covers;
					}
					while (--len != 0);
				}
			}
		}

		//--------------------------------------------------------------------
		public unsafe void CopyHorizontalColorSpan(int x, int y, uint len, RGBA_Bytes* colors)
		{
			byte* p = m_rbuf.GetPixelPointer(y) + (x << 2);
			do
			{
				p[OrderR] = colors[0].m_R;
				p[OrderG] = colors[0].m_G;
				p[OrderB] = colors[0].m_B;
				p[OrderA] = colors[0].m_A;
				++colors;
				p += 4;
			}
			while (--len != 0);
		}

		//--------------------------------------------------------------------
		public unsafe void CopyVerticalColorSpan(int x, int y, uint len, RGBA_Bytes* colors)
		{
			byte* p = m_rbuf.GetPixelPointer(y) + (x << 2);
			int ScanWidth = m_rbuf.StrideInBytes();
			do
			{
				p[OrderR] = colors[0].m_R;
				p[OrderG] = colors[0].m_G;
				p[OrderB] = colors[0].m_B;
				p[OrderA] = colors[0].m_A;
				p = &p[ScanWidth];
				++colors;
			}
			while (--len != 0);
		}

		//--------------------------------------------------------------------
		unsafe public void BlendHorizontalColorSpan(int x, int y, uint len, RGBA_Bytes* colors, byte* covers, byte cover)
		{
			byte* p = m_rbuf.GetPixelPointer(y) + (x << 2);
			if (covers != null)
			{
				do
				{
					CopyOrBlendBGRAWrapper.CopyOrBlendPixel(m_Blender, p, colors->m_R, colors->m_G, colors->m_B, colors->m_A, *covers++);
					p += 4;
					++colors;
				}
				while (--len != 0);
			}
			else
			{
				if (cover == 255)
				{
					do
					{
						CopyOrBlendBGRAWrapper.CopyOrBlendPixel(m_Blender, p, colors->R_Byte, colors->G_Byte, colors->B_Byte, colors->A_Byte);
						p += 4;
						++colors;
					}
					while (--len != 0);
				}
				else
				{
					do
					{
						CopyOrBlendBGRAWrapper.CopyOrBlendPixel(m_Blender, p, colors->R_Byte, colors->G_Byte, colors->B_Byte, colors->A_Byte, cover);
						p += 4;
						++colors;
					}
					while (--len != 0);
				}
			}
		}

		//--------------------------------------------------------------------
		public unsafe void BlendVerticalColorSpan(int x, int y, uint len, RGBA_Bytes* colors, byte* covers, byte cover)
		{
			byte* p = m_rbuf.GetPixelPointer(y) + (x << 2);
			int ScanWidth = m_rbuf.StrideInBytes();
			if (covers != null)
			{
				do
				{
					CopyOrBlendBGRAWrapper.CopyOrBlendPixel(m_Blender, p, colors->m_R, colors->m_G, colors->m_B, colors->m_A, *covers++);
					p = &p[ScanWidth];
					++colors;
				}
				while (--len != 0);
			}
			else
			{
				if (cover == 255)
				{
					do
					{
						CopyOrBlendBGRAWrapper.CopyOrBlendPixel(m_Blender, p, colors->R_Byte, colors->G_Byte, colors->B_Byte, colors->A_Byte);
						p = &p[ScanWidth];
						++colors;
					}
					while (--len != 0);
				}
				else
				{
					do
					{
						CopyOrBlendBGRAWrapper.CopyOrBlendPixel(m_Blender, p, colors->R_Byte, colors->G_Byte, colors->B_Byte, colors->A_Byte, cover);
						p = &p[ScanWidth];
						++colors;
					}
					while (--len != 0);
				}
			}
		}

		public void CopyFrom(RasterBuffer sourceBuffer, int xdst, int ydst, int xsrc, int ysrc, uint len)
		{
			unsafe
			{
				Basics.memmove(&m_rbuf.GetPixelPointer(ydst)[xdst * 4],
						&sourceBuffer.GetPixelPointer(ysrc)[xsrc * 4],
						(int)len * 4);
			}
		}

		/*

		//--------------------------------------------------------------------
		//template<class Function>
		public void for_each_pixel(Function f)
		{
			uint y;
			for(y = 0; y < Height(); ++y)
			{
				row_data r = m_rbuf->row(y);
				if(r.ptr)
				{
					uint len = r.x2 - r.x1 + 1;
					value_type* p =
						(value_type*)m_rbuf->RowPointer(r.x1, y, len) + (r.x1 << 2);
					do
					{
						f(p);
						p += 4;
					}
					while(--len);
				}
			}
		}

		//--------------------------------------------------------------------
		public void PreMultiply()
		{
			for_each_pixel(multiplier_rgba<color_type, order_type>::PreMultiply);
		}

		//--------------------------------------------------------------------
		public void DeMultiply()
		{
			for_each_pixel(multiplier_rgba<color_type, order_type>::DeMultiply);
		}

		//--------------------------------------------------------------------
		//template<class GammaLut>
		public void apply_gamma_dir(GammaLut g)
		{
			for_each_pixel(apply_gamma_dir_rgba<color_type, order_type, GammaLut>(g));
		}

		//--------------------------------------------------------------------
		//template<class GammaLut>
		 */

		public void apply_gamma_inv(GammaLookupTable g)
		{
			throw new System.NotImplementedException();

			//for_each_pixel(apply_gamma_inv_rgba<color_type, order_type, GammaLut>(g));
		}

		/*

		//--------------------------------------------------------------------
		//template<class RenBuf2>

		//--------------------------------------------------------------------
		//template<class SrcPixelFormatRenderer>
		public void blend_from(SrcPixelFormatRenderer from,
						int xdst, int ydst,
						int xsrc, int ysrc,
						uint len,
						byte cover)
		{
			typedef typename SrcPixelFormatRenderer::order_type src_order;
			value_type* psrc = (value_type*)from.RowPointer(ysrc);
			if(psrc)
			{
				psrc += xsrc << 2;
				value_type* pdst =
					(value_type*)m_rbuf->RowPointer(xdst, ydst, len) + (xdst << 2);
				int incp = 4;
				if(xdst > xsrc)
				{
					psrc += (len-1) << 2;
					pdst += (len-1) << 2;
					incp = -4;
				}

				if(cover == 255)
				{
					do
					{
						cob_type::CopyOrBlendPixel(pdst,
													psrc[src_order::R],
													psrc[src_order::G],
													psrc[src_order::B],
													psrc[src_order::A]);
						psrc += incp;
						pdst += incp;
					}
					while(--len);
				}
				else
				{
					do
					{
						cob_type::CopyOrBlendPixel(pdst,
													psrc[src_order::R],
													psrc[src_order::G],
													psrc[src_order::B],
													psrc[src_order::A],
													cover);
						psrc += incp;
						pdst += incp;
					}
					while(--len);
				}
			}
		}

		//--------------------------------------------------------------------
		//template<class SrcPixelFormatRenderer>
		public void blend_from_color(SrcPixelFormatRenderer from,
							  color_type Color,
							  int xdst, int ydst,
							  int xsrc, int ysrc,
							  uint len,
							  byte cover)
		{
			typedef typename SrcPixelFormatRenderer::value_type src_value_type;
			src_value_type* psrc = (src_value_type*)from.RowPointer(ysrc);
			if(psrc)
			{
				value_type* pdst =
					(value_type*)m_rbuf->RowPointer(xdst, ydst, len) + (xdst << 2);
				do
				{
					cob_type::CopyOrBlendPixel(pdst,
												Color.r, Color.g, Color.b, Color.a,
												(*psrc * cover + BaseMask) >> BaseShift);
					++psrc;
					pdst += 4;
				}
				while(--len);
			}
		}

		//--------------------------------------------------------------------
		//template<class SrcPixelFormatRenderer>
		public void blend_from_lut(SrcPixelFormatRenderer from,
							color_type* color_lut,
							int xdst, int ydst,
							int xsrc, int ysrc,
							uint len,
							byte cover)
		{
			typedef typename SrcPixelFormatRenderer::value_type src_value_type;
			src_value_type* psrc = (src_value_type*)from.RowPointer(ysrc);
			if(psrc)
			{
				value_type* pdst =
					(value_type*)m_rbuf->RowPointer(xdst, ydst, len) + (xdst << 2);

				if(cover == 255)
				{
					do
					{
						color_type& Color = color_lut[*psrc];
						cob_type::CopyOrBlendPixel(pdst,
													Color.r, Color.g, Color.b, Color.a);
						++psrc;
						pdst += 4;
					}
					while(--len);
				}
				else
				{
					do
					{
						color_type& Color = color_lut[*psrc];
						cob_type::CopyOrBlendPixel(pdst,
													Color.r, Color.g, Color.b, Color.a,
													cover);
						++psrc;
						pdst += 4;
					}
					while(--len);
				}
			}
		}
		 */
	};

	/*

		//================================================pixfmt_custom_blend_rgba
		template<class Blender, class RenBuf> class pixfmt_custom_blend_rgba
		{
		public:

			//typedef RenBuf   rbuf_type;
			//typedef typename rbuf_type::row_data row_data;
			typedef Blender  blender_type;
			typedef typename blender_type::color_type color_type;
			typedef typename blender_type::order_type order_type;
			typedef typename color_type::value_type value_type;
			typedef typename color_type::calc_type calc_type;
			enum base_scale_e
			{
				BaseShift = color_type::BaseShift,
				base_scale = color_type::base_scale,
				BaseMask  = color_type::BaseMask,
				pix_width  = sizeof(value_type) * 4
			};

			//--------------------------------------------------------------------
			pixfmt_custom_blend_rgba() : m_rbuf(0), m_comp_op(3) {}
			explicit pixfmt_custom_blend_rgba(rendering_buffer rb, uint comp_op=3) :
				m_rbuf(&rb),
				m_comp_op(comp_op)
			{}
			void Attach(rendering_buffer rb) { m_rbuf = &rb; }

			//--------------------------------------------------------------------
			template<class PixFmt>
			bool Attach(PixFmt& pixf, int x1, int y1, int x2, int y2)
			{
				RectI r(x1, y1, x2, y2);
				if(r.Clip(RectI(0, 0, pixf.Width()-1, pixf.Height()-1)))
				{
					int Stride = pixf.Stride();
					m_rbuf->Attach(pixf.PixelPointer(r.x1, Stride < 0 ? r.y2 : r.y1),
								   (r.x2 - r.x1) + 1,
								   (r.y2 - r.y1) + 1,
								   Stride);
					return true;
				}
				return false;
			}

			//--------------------------------------------------------------------
			uint Width()  const { return m_rbuf->Width();  }
			uint Height() const { return m_rbuf->Height(); }
			int      Stride() const { return m_rbuf->Stride(); }

			//--------------------------------------------------------------------
				  byte* RowPointer(int y)       { return m_rbuf->RowPointer(y); }
			const byte* RowPointer(int y) const { return m_rbuf->RowPointer(y); }
			row_data     row(int y)     const { return m_rbuf->row(y); }

			//--------------------------------------------------------------------
			byte* PixelPointer(int x, int y)
			{
				return m_rbuf->RowPointer(y) + x * pix_width;
			}

			const byte* PixelPointer(int x, int y) const
			{
				return m_rbuf->RowPointer(y) + x * pix_width;
			}

			//--------------------------------------------------------------------
			void comp_op(uint op) { m_comp_op = op; }
			uint comp_op() const  { return m_comp_op; }

			//--------------------------------------------------------------------
			static void MakePixel(byte* p, const color_type& c)
			{
				((value_type*)p)[OrderR] = c.r;
				((value_type*)p)[OrderG] = c.g;
				((value_type*)p)[OrderB] = c.b;
				((value_type*)p)[OrderA] = c.a;
			}

			//--------------------------------------------------------------------
			color_type Pixel(int x, int y) const
			{
				const value_type* p = (value_type*)m_rbuf->RowPointer(y) + (x << 2);
				return color_type(p[OrderR],
								  p[OrderG],
								  p[OrderB],
								  p[OrderA]);
			}

			//--------------------------------------------------------------------
			unsafe void CopyPixel(int x, int y, byte* c)
			{
				blender_type::BlendPixel(
					m_comp_op,
					(value_type*)m_rbuf->RowPointer(x, y, 1) + (x << 2),
					c.r, c.g, c.b, c.a, 255);
			}

			//--------------------------------------------------------------------
			void BlendPixel(int x, int y, const color_type& c, byte cover)
			{
				blender_type::BlendPixel(
					m_comp_op,
					(value_type*)m_rbuf->RowPointer(x, y, 1) + (x << 2),
					c.r, c.g, c.b, c.a,
					cover);
			}

			//--------------------------------------------------------------------
			void CopyHorizontalLine(int x, int y, uint len, const color_type& c)
			{
				value_type* p = (value_type*)m_rbuf->RowPointer(x, y, len) + (x << 2);;
				do
				{
					blender_type::BlendPixel(m_comp_op, p, c.r, c.g, c.b, c.a, 255);
					p += 4;
				}
				while(--len);
			}

			//--------------------------------------------------------------------
			void CopyVerticalLine(int x, int y, uint len, const color_type& c)
			{
				do
				{
					blender_type::BlendPixel(
						m_comp_op,
						(value_type*)m_rbuf->RowPointer(x, y++, 1) + (x << 2),
						c.r, c.g, c.b, c.a, 255);
				}
				while(--len);
			}

			//--------------------------------------------------------------------
			void BlendHorizontalLine(int x, int y, uint len,
							 const color_type& c, byte cover)
			{
				value_type* p = (value_type*)m_rbuf->RowPointer(x, y, len) + (x << 2);
				do
				{
					blender_type::BlendPixel(m_comp_op, p, c.r, c.g, c.b, c.a, cover);
					p += 4;
				}
				while(--len);
			}

			//--------------------------------------------------------------------
			void BlendVerticalLine(int x, int y, uint len,
							 const color_type& c, byte cover)
			{
				do
				{
					blender_type::BlendPixel(
						m_comp_op,
						(value_type*)m_rbuf->RowPointer(x, y++, 1) + (x << 2),
						c.r, c.g, c.b, c.a,
						cover);
				}
				while(--len);
			}

			//--------------------------------------------------------------------
			void BlendSolidHorizontalSpan(int x, int y, uint len,
								   const color_type& c, const byte* covers)
			{
				value_type* p = (value_type*)m_rbuf->RowPointer(x, y, len) + (x << 2);
				do
				{
					blender_type::BlendPixel(m_comp_op,
											p, c.r, c.g, c.b, c.a,
											*covers++);
					p += 4;
				}
				while(--len);
			}

			//--------------------------------------------------------------------
			void BlendSolidVerticalSpan(int x, int y, uint len,
								   const color_type& c, const byte* covers)
			{
				do
				{
					blender_type::BlendPixel(
						m_comp_op,
						(value_type*)m_rbuf->RowPointer(x, y++, 1) + (x << 2),
						c.r, c.g, c.b, c.a,
						*covers++);
				}
				while(--len);
			}

			//--------------------------------------------------------------------
			void CopyHorizontalColorSpan(int x, int y,
								  uint len,
								  const color_type* Colors)
			{
				value_type* p = (value_type*)m_rbuf->RowPointer(x, y, len) + (x << 2);
				do
				{
					p[OrderR] = Colors->r;
					p[OrderG] = Colors->g;
					p[OrderB] = Colors->b;
					p[OrderA] = Colors->a;
					++Colors;
					p += 4;
				}
				while(--len);
			}

			//--------------------------------------------------------------------
			void CopyVerticalColorSpan(int x, int y,
								  uint len,
								  const color_type* Colors)
			{
				do
				{
					value_type* p = (value_type*)m_rbuf->RowPointer(x, y++, 1) + (x << 2);
					p[OrderR] = Colors->r;
					p[OrderG] = Colors->g;
					p[OrderB] = Colors->b;
					p[OrderA] = Colors->a;
					++Colors;
				}
				while(--len);
			}

			//--------------------------------------------------------------------
			void BlendHorizontalColorSpan(int x, int y, uint len,
								   const color_type* Colors,
								   const byte* covers,
								   byte cover)
			{
				value_type* p = (value_type*)m_rbuf->RowPointer(x, y, len) + (x << 2);
				do
				{
					blender_type::BlendPixel(m_comp_op,
											p,
											Colors->r,
											Colors->g,
											Colors->b,
											Colors->a,
											covers ? *covers++ : cover);
					p += 4;
					++Colors;
				}
				while(--len);
			}

			//--------------------------------------------------------------------
			void BlendVerticalColorSpan(int x, int y, uint len,
								   const color_type* Colors,
								   const byte* covers,
								   byte cover)
			{
				do
				{
					blender_type::BlendPixel(
						m_comp_op,
						(value_type*)m_rbuf->RowPointer(x, y++, 1) + (x << 2),
						Colors->r,
						Colors->g,
						Colors->b,
						Colors->a,
						covers ? *covers++ : cover);
					++Colors;
				}
				while(--len);
			}

			//--------------------------------------------------------------------
			template<class Function> void for_each_pixel(Function f)
			{
				uint y;
				for(y = 0; y < Height(); ++y)
				{
					row_data r = m_rbuf->row(y);
					if(r.ptr)
					{
						uint len = r.x2 - r.x1 + 1;
						value_type* p =
							(value_type*)m_rbuf->RowPointer(r.x1, y, len) + (r.x1 << 2);
						do
						{
							f(p);
							p += 4;
						}
						while(--len);
					}
				}
			}

			//--------------------------------------------------------------------
			void PreMultiply()
			{
				for_each_pixel(multiplier_rgba<color_type, order_type>::PreMultiply);
			}

			//--------------------------------------------------------------------
			void DeMultiply()
			{
				for_each_pixel(multiplier_rgba<color_type, order_type>::DeMultiply);
			}

			//--------------------------------------------------------------------
			template<class GammaLut> void apply_gamma_dir(const GammaLut& g)
			{
				for_each_pixel(apply_gamma_dir_rgba<color_type, order_type, GammaLut>(g));
			}

			//--------------------------------------------------------------------
			template<class GammaLut> void apply_gamma_inv(const GammaLut& g)
			{
				for_each_pixel(apply_gamma_inv_rgba<color_type, order_type, GammaLut>(g));
			}

			//--------------------------------------------------------------------
			template<class RenBuf2> void CopyFrom(const RenBuf2& from,
												   int xdst, int ydst,
												   int xsrc, int ysrc,
												   uint len)
			{
				const byte* p = from.RowPointer(ysrc);
				if(p)
				{
					memmove(m_rbuf->RowPointer(xdst, ydst, len) + xdst * pix_width,
							p + xsrc * pix_width,
							len * pix_width);
				}
			}

			//--------------------------------------------------------------------
			template<class SrcPixelFormatRenderer>
			void blend_from(const SrcPixelFormatRenderer& from,
							int xdst, int ydst,
							int xsrc, int ysrc,
							uint len,
							byte cover)
			{
				typedef typename SrcPixelFormatRenderer::order_type src_order;
				const value_type* psrc = (const value_type*)from.RowPointer(ysrc);
				if(psrc)
				{
					psrc += xsrc << 2;
					value_type* pdst =
						(value_type*)m_rbuf->RowPointer(xdst, ydst, len) + (xdst << 2);

					int incp = 4;
					if(xdst > xsrc)
					{
						psrc += (len-1) << 2;
						pdst += (len-1) << 2;
						incp = -4;
					}

					do
					{
						blender_type::BlendPixel(m_comp_op,
												pdst,
												psrc[src_order::R],
												psrc[src_order::G],
												psrc[src_order::B],
												psrc[src_order::A],
												cover);
						psrc += incp;
						pdst += incp;
					}
					while(--len);
				}
			}

			//--------------------------------------------------------------------
			template<class SrcPixelFormatRenderer>
			void blend_from_color(const SrcPixelFormatRenderer& from,
								  const color_type& Color,
								  int xdst, int ydst,
								  int xsrc, int ysrc,
								  uint len,
								  byte cover)
			{
				typedef typename SrcPixelFormatRenderer::value_type src_value_type;
				const src_value_type* psrc = (src_value_type*)from.RowPointer(ysrc);
				if(psrc)
				{
					value_type* pdst =
						(value_type*)m_rbuf->RowPointer(xdst, ydst, len) + (xdst << 2);
					do
					{
						blender_type::BlendPixel(m_comp_op,
												pdst,
												Color.r, Color.g, Color.b, Color.a,
												(*psrc * cover + BaseMask) >> BaseShift);
						++psrc;
						pdst += 4;
					}
					while(--len);
				}
			}

			//--------------------------------------------------------------------
			template<class SrcPixelFormatRenderer>
			void blend_from_lut(const SrcPixelFormatRenderer& from,
								const color_type* color_lut,
								int xdst, int ydst,
								int xsrc, int ysrc,
								uint len,
								byte cover)
			{
				typedef typename SrcPixelFormatRenderer::value_type src_value_type;
				const src_value_type* psrc = (src_value_type*)from.RowPointer(ysrc);
				if(psrc)
				{
					value_type* pdst =
						(value_type*)m_rbuf->RowPointer(xdst, ydst, len) + (xdst << 2);
					do
					{
						const color_type& Color = color_lut[*psrc];
						blender_type::BlendPixel(m_comp_op,
												pdst,
												Color.r, Color.g, Color.b, Color.a,
												cover);
						++psrc;
						pdst += 4;
					}
					while(--len);
				}
			}

		private:
			rendering_buffer m_rbuf;
			uint m_comp_op;
		};

		//-----------------------------------------------------------------------
		typedef blender_rgba<rgba8, OrderRGBA> blender_rgba32; //----blender_rgba32
		typedef blender_rgba<rgba8, OrderARGB> blender_argb32; //----blender_argb32
		typedef blender_rgba<rgba8, OrderABGR> blender_abgr32; //----blender_abgr32
		typedef blender_rgba<rgba8, OrderBGRA> blender_bgra32; //----blender_bgra32

		typedef blender_rgba_pre<rgba8, OrderRGBA> blender_rgba32_pre; //----blender_rgba32_pre
		typedef blender_rgba_pre<rgba8, OrderARGB> blender_argb32_pre; //----blender_argb32_pre
		typedef blender_rgba_pre<rgba8, OrderABGR> blender_abgr32_pre; //----blender_abgr32_pre
		typedef blender_rgba_pre<rgba8, OrderBGRA> blender_bgra32_pre; //----blender_bgra32_pre

		typedef blender_rgba_plain<rgba8, OrderRGBA> blender_rgba32_plain; //----blender_rgba32_plain
		typedef blender_rgba_plain<rgba8, OrderARGB> blender_argb32_plain; //----blender_argb32_plain
		typedef blender_rgba_plain<rgba8, OrderABGR> blender_abgr32_plain; //----blender_abgr32_plain
		typedef blender_rgba_plain<rgba8, OrderBGRA> blender_bgra32_plain; //----blender_bgra32_plain

		typedef blender_rgba<rgba16, OrderRGBA> blender_rgba64; //----blender_rgba64
		typedef blender_rgba<rgba16, OrderARGB> blender_argb64; //----blender_argb64
		typedef blender_rgba<rgba16, OrderABGR> blender_abgr64; //----blender_abgr64
		typedef blender_rgba<rgba16, OrderBGRA> blender_bgra64; //----blender_bgra64

		typedef blender_rgba_pre<rgba16, OrderRGBA> blender_rgba64_pre; //----blender_rgba64_pre
		typedef blender_rgba_pre<rgba16, OrderARGB> blender_argb64_pre; //----blender_argb64_pre
		typedef blender_rgba_pre<rgba16, OrderABGR> blender_abgr64_pre; //----blender_abgr64_pre
		typedef blender_rgba_pre<rgba16, OrderBGRA> blender_bgra64_pre; //----blender_bgra64_pre

		//-----------------------------------------------------------------------
		typedef int32u pixel32_type;
		typedef pixfmt_alpha_blend_rgba<blender_rgba32, rendering_buffer, pixel32_type> pixfmt_rgba32; //----pixfmt_rgba32
		typedef pixfmt_alpha_blend_rgba<blender_argb32, rendering_buffer, pixel32_type> pixfmt_argb32; //----pixfmt_argb32
		typedef pixfmt_alpha_blend_rgba<blender_abgr32, rendering_buffer, pixel32_type> pixfmt_abgr32; //----pixfmt_abgr32
		typedef pixfmt_alpha_blend_rgba<blender_bgra32, rendering_buffer, pixel32_type> pixfmt_bgra32; //----pixfmt_bgra32

		typedef pixfmt_alpha_blend_rgba<blender_rgba32_pre, rendering_buffer, pixel32_type> pixfmt_rgba32_pre; //----pixfmt_rgba32_pre
		typedef pixfmt_alpha_blend_rgba<blender_argb32_pre, rendering_buffer, pixel32_type> pixfmt_argb32_pre; //----pixfmt_argb32_pre
		typedef pixfmt_alpha_blend_rgba<blender_abgr32_pre, rendering_buffer, pixel32_type> pixfmt_abgr32_pre; //----pixfmt_abgr32_pre
		typedef pixfmt_alpha_blend_rgba<blender_bgra32_pre, rendering_buffer, pixel32_type> pixfmt_bgra32_pre; //----pixfmt_bgra32_pre

		typedef pixfmt_alpha_blend_rgba<blender_rgba32_plain, rendering_buffer, pixel32_type> pixfmt_rgba32_plain; //----pixfmt_rgba32_plain
		typedef pixfmt_alpha_blend_rgba<blender_argb32_plain, rendering_buffer, pixel32_type> pixfmt_argb32_plain; //----pixfmt_argb32_plain
		typedef pixfmt_alpha_blend_rgba<blender_abgr32_plain, rendering_buffer, pixel32_type> pixfmt_abgr32_plain; //----pixfmt_abgr32_plain
		typedef pixfmt_alpha_blend_rgba<blender_bgra32_plain, rendering_buffer, pixel32_type> pixfmt_bgra32_plain; //----pixfmt_bgra32_plain

		struct  pixel64_type { int16u c[4]; };
		typedef pixfmt_alpha_blend_rgba<blender_rgba64, rendering_buffer, pixel64_type> pixfmt_rgba64; //----pixfmt_rgba64
		typedef pixfmt_alpha_blend_rgba<blender_argb64, rendering_buffer, pixel64_type> pixfmt_argb64; //----pixfmt_argb64
		typedef pixfmt_alpha_blend_rgba<blender_abgr64, rendering_buffer, pixel64_type> pixfmt_abgr64; //----pixfmt_abgr64
		typedef pixfmt_alpha_blend_rgba<blender_bgra64, rendering_buffer, pixel64_type> pixfmt_bgra64; //----pixfmt_bgra64

		typedef pixfmt_alpha_blend_rgba<blender_rgba64_pre, rendering_buffer, pixel64_type> pixfmt_rgba64_pre; //----pixfmt_rgba64_pre
		typedef pixfmt_alpha_blend_rgba<blender_argb64_pre, rendering_buffer, pixel64_type> pixfmt_argb64_pre; //----pixfmt_argb64_pre
		typedef pixfmt_alpha_blend_rgba<blender_abgr64_pre, rendering_buffer, pixel64_type> pixfmt_abgr64_pre; //----pixfmt_abgr64_pre
		typedef pixfmt_alpha_blend_rgba<blender_bgra64_pre, rendering_buffer, pixel64_type> pixfmt_bgra64_pre; //----pixfmt_bgra64_pre
	*/
}

//#endif