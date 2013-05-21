using image_filter_scale_e = Pictor.ImageFilterLookUpTable.EImageFilterScale;

/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Simon Wollwage (rootnode) <kintaro@think-in-co.de>
 */

using image_subpixel_scale_e = Pictor.ImageFilterLookUpTable.EImageSubpixelScale;

namespace Pictor
{
	//==============================================SpanImageFilterRGBANN
	public class SpanImageFilterRGBANN : SpanImageFilter
	{
		private int OrderR;
		private int OrderG;
		private int OrderB;
		private int OrderA;

		private const int base_shift = 8;
		private const uint base_scale = (uint)(1 << base_shift);
		private const uint base_mask = base_scale - 1;

		public SpanImageFilterRGBANN(IRasterBufferAccessor src, ISpanInterpolator inter)
			: base(src, inter, null)
		{
			OrderR = src.PixelFormat.Blender.OrderR;
			OrderG = src.PixelFormat.Blender.OrderG;
			OrderB = src.PixelFormat.Blender.OrderB;
			OrderA = src.PixelFormat.Blender.OrderA;
		}

		public unsafe override void Generate(RGBA_Bytes* span, int x, int y, uint len)
		{
			RasterBuffer pSourceRenderingBuffer = base.source().PixelFormat.RenderingBuffer;
			ISpanInterpolator spanInterpolator = base.interpolator();
			spanInterpolator.Begin(x + base.filter_dx_dbl(), y + base.filter_dy_dbl(), len);
			do
			{
				int x_hr;
				int y_hr;
				spanInterpolator.Coordinates(out x_hr, out y_hr);
				int x_lr = x_hr >> (int)image_subpixel_scale_e.Shift;
				int y_lr = y_hr >> (int)image_subpixel_scale_e.Shift;
				byte* fg_ptr = pSourceRenderingBuffer.GetPixelPointer(y_lr) + (x_lr << 2);

				//byte* fg_ptr = spanInterpolator.Span(x_lr, y_lr, 1);
				(*span).m_R = fg_ptr[OrderR];
				(*span).m_G = fg_ptr[OrderG];
				(*span).m_B = fg_ptr[OrderB];
				(*span).m_A = fg_ptr[OrderA];
				++span;
				spanInterpolator.Next();
			} while (--len != 0);
		}
	};

	//=========================================SpanImageFilterRGBABilinear
	public class SpanImageFilterRGBABilinear : SpanImageFilter
	{
		private int OrderR;
		private int OrderG;
		private int OrderB;
		private int OrderA;

		private const int base_shift = 8;
		private const uint base_scale = (uint)(1 << base_shift);
		private const uint base_mask = base_scale - 1;

		public SpanImageFilterRGBABilinear(IRasterBufferAccessor src, ISpanInterpolator inter)
			: base(src, inter, null)
		{
			OrderR = src.PixelFormat.Blender.OrderR;
			OrderG = src.PixelFormat.Blender.OrderG;
			OrderB = src.PixelFormat.Blender.OrderB;
			OrderA = src.PixelFormat.Blender.OrderA;
		}

#if use_timers
            static CNamedTimer Generate_Span = new CNamedTimer("Generate_Span rgba");
#endif

		public unsafe void Generate(out RGBA_Bytes destPixel, int x, int y)
		{
			base.interpolator().Begin(x + base.filter_dx_dbl(), y + base.filter_dy_dbl(), 1);

			uint* fg = stackalloc uint[4];

			byte* fg_ptr;

			RasterBuffer pSourceRenderingBuffer = base.source().PixelFormat.RenderingBuffer;
			int maxx = (int)pSourceRenderingBuffer.Width() - 1;
			int maxy = (int)pSourceRenderingBuffer.Height() - 1;
			ISpanInterpolator spanInterpolator = base.interpolator();

			unchecked
			{
				int x_hr;
				int y_hr;

				spanInterpolator.Coordinates(out x_hr, out y_hr);

				x_hr -= base.filter_dx_int();
				y_hr -= base.filter_dy_int();

				int x_lr = x_hr >> (int)image_subpixel_scale_e.Shift;
				int y_lr = y_hr >> (int)image_subpixel_scale_e.Shift;

				uint weight;

				fg[0] = fg[1] = fg[2] = fg[3] = (int)image_subpixel_scale_e.Scale * (int)image_subpixel_scale_e.Scale / 2;

				x_hr &= (int)image_subpixel_scale_e.Mask;
				y_hr &= (int)image_subpixel_scale_e.Mask;

				fg_ptr = pSourceRenderingBuffer.GetPixelPointer(y_lr) + (x_lr << 2);

				weight = (uint)(((int)image_subpixel_scale_e.Scale - x_hr) *
						 ((int)image_subpixel_scale_e.Scale - y_hr));
				fg[0] += weight * fg_ptr[0];
				fg[1] += weight * fg_ptr[1];
				fg[2] += weight * fg_ptr[2];
				fg[3] += weight * fg_ptr[3];

				weight = (uint)(x_hr * ((int)image_subpixel_scale_e.Scale - y_hr));
				fg[0] += weight * fg_ptr[4];
				fg[1] += weight * fg_ptr[5];
				fg[2] += weight * fg_ptr[6];
				fg[3] += weight * fg_ptr[7];

				++y_lr;
				fg_ptr = pSourceRenderingBuffer.GetPixelPointer(y_lr) + (x_lr << 2);

				weight = (uint)(((int)image_subpixel_scale_e.Scale - x_hr) * y_hr);
				fg[0] += weight * fg_ptr[0];
				fg[1] += weight * fg_ptr[1];
				fg[2] += weight * fg_ptr[2];
				fg[3] += weight * fg_ptr[3];

				weight = (uint)(x_hr * y_hr);
				fg[0] += weight * fg_ptr[4];
				fg[1] += weight * fg_ptr[5];
				fg[2] += weight * fg_ptr[6];
				fg[3] += weight * fg_ptr[7];

				fg[0] >>= (int)image_subpixel_scale_e.Shift * 2;
				fg[1] >>= (int)image_subpixel_scale_e.Shift * 2;
				fg[2] >>= (int)image_subpixel_scale_e.Shift * 2;
				fg[3] >>= (int)image_subpixel_scale_e.Shift * 2;

				destPixel.m_R = (byte)fg[OrderR];
				destPixel.m_G = (byte)fg[OrderG];
				destPixel.m_B = (byte)fg[OrderB];
				destPixel.m_A = (byte)fg[OrderA];
			}
		}

		public unsafe override void Generate(RGBA_Bytes* span, int x, int y, uint len)
		{
#if use_timers
                Generate_Span.Start();
#endif
			base.interpolator().Begin(x + base.filter_dx_dbl(), y + base.filter_dy_dbl(), len);

			uint* fg = stackalloc uint[4];

			byte* fg_ptr;

			RasterBuffer pSourceRenderingBuffer = base.source().PixelFormat.RenderingBuffer;
			int maxx = (int)pSourceRenderingBuffer.Width() - 1;
			int maxy = (int)pSourceRenderingBuffer.Height() - 1;
			ISpanInterpolator spanInterpolator = base.interpolator();

			unchecked
			{
				do
				{
					int x_hr;
					int y_hr;

					spanInterpolator.Coordinates(out x_hr, out y_hr);

					x_hr -= base.filter_dx_int();
					y_hr -= base.filter_dy_int();

					int x_lr = x_hr >> (int)image_subpixel_scale_e.Shift;
					int y_lr = y_hr >> (int)image_subpixel_scale_e.Shift;

					uint weight;

					fg[0] = fg[1] = fg[2] = fg[3] = (int)image_subpixel_scale_e.Scale * (int)image_subpixel_scale_e.Scale / 2;

					x_hr &= (int)image_subpixel_scale_e.Mask;
					y_hr &= (int)image_subpixel_scale_e.Mask;

					fg_ptr = pSourceRenderingBuffer.GetPixelPointer(y_lr) + (x_lr << 2);

					weight = (uint)(((int)image_subpixel_scale_e.Scale - x_hr) *
							 ((int)image_subpixel_scale_e.Scale - y_hr));
					fg[0] += weight * fg_ptr[0];
					fg[1] += weight * fg_ptr[1];
					fg[2] += weight * fg_ptr[2];
					fg[3] += weight * fg_ptr[3];

					weight = (uint)(x_hr * ((int)image_subpixel_scale_e.Scale - y_hr));
					fg[0] += weight * fg_ptr[4];
					fg[1] += weight * fg_ptr[5];
					fg[2] += weight * fg_ptr[6];
					fg[3] += weight * fg_ptr[7];

					++y_lr;
					fg_ptr = pSourceRenderingBuffer.GetPixelPointer(y_lr) + (x_lr << 2);

					weight = (uint)(((int)image_subpixel_scale_e.Scale - x_hr) * y_hr);
					fg[0] += weight * fg_ptr[0];
					fg[1] += weight * fg_ptr[1];
					fg[2] += weight * fg_ptr[2];
					fg[3] += weight * fg_ptr[3];

					weight = (uint)(x_hr * y_hr);
					fg[0] += weight * fg_ptr[4];
					fg[1] += weight * fg_ptr[5];
					fg[2] += weight * fg_ptr[6];
					fg[3] += weight * fg_ptr[7];

					fg[0] >>= (int)image_subpixel_scale_e.Shift * 2;
					fg[1] >>= (int)image_subpixel_scale_e.Shift * 2;
					fg[2] >>= (int)image_subpixel_scale_e.Shift * 2;
					fg[3] >>= (int)image_subpixel_scale_e.Shift * 2;

					(*span).m_R = (byte)fg[OrderR];
					(*span).m_G = (byte)fg[OrderG];
					(*span).m_B = (byte)fg[OrderB];
					(*span).m_A = (byte)fg[OrderA];
					++span;
					spanInterpolator.Next();
				} while (--len != 0);
			}
#if use_timers
                Generate_Span.Stop();
#endif
		}
	};

	//====================================SpanImageFilterRGBABilinearClip
	public class SpanImageFilterRGBABilinearClip : SpanImageFilter
	{
		private int OrderR;
		private int OrderG;
		private int OrderB;
		private int OrderA;
		private RGBA_Bytes m_back_color;

		private const int base_shift = 8;
		private const uint base_scale = (uint)(1 << base_shift);
		private const uint base_mask = base_scale - 1;

		public SpanImageFilterRGBABilinearClip(IRasterBufferAccessor src,
			IColorType back_color, ISpanInterpolator inter)
			: base(src, inter, null)
		{
			m_back_color = back_color.GetAsRGBA_Bytes();
			OrderR = src.PixelFormat.Blender.OrderR;
			OrderG = src.PixelFormat.Blender.OrderG;
			OrderB = src.PixelFormat.Blender.OrderB;
			OrderA = src.PixelFormat.Blender.OrderA;
		}

		public IColorType background_color()
		{
			return m_back_color;
		}

		public void background_color(IColorType v)
		{
			m_back_color = v.GetAsRGBA_Bytes();
		}

#if use_timers
            static CNamedTimer Generate_Span = new CNamedTimer("Generate_Span rgba clip");
#endif

		public unsafe override void Generate(RGBA_Bytes* span, int x, int y, uint len)
		{
#if use_timers
                Generate_Span.Start();
#endif
			base.interpolator().Begin(x + base.filter_dx_dbl(), y + base.filter_dy_dbl(), len);

			//uint[] fg = new uint[4];
			uint* fg = stackalloc uint[4];

			uint back_r = m_back_color.m_R;
			uint back_g = m_back_color.m_G;
			uint back_b = m_back_color.m_B;
			uint back_a = m_back_color.m_A;

			byte* fg_ptr;

			RasterBuffer pSourceRenderingBuffer = base.source().PixelFormat.RenderingBuffer;
			int maxx = (int)pSourceRenderingBuffer.Width() - 1;
			int maxy = (int)pSourceRenderingBuffer.Height() - 1;
			ISpanInterpolator spanInterpolator = base.interpolator();

			unchecked
			{
				do
				{
					int x_hr;
					int y_hr;

					spanInterpolator.Coordinates(out x_hr, out y_hr);

					x_hr -= base.filter_dx_int();
					y_hr -= base.filter_dy_int();

					int x_lr = x_hr >> (int)image_subpixel_scale_e.Shift;
					int y_lr = y_hr >> (int)image_subpixel_scale_e.Shift;

					uint weight;

					if (x_lr >= 0 && y_lr >= 0 &&
					   x_lr < maxx && y_lr < maxy)
					{
						fg[0] =
						fg[1] =
						fg[2] =
						fg[3] = (int)image_subpixel_scale_e.Scale * (int)image_subpixel_scale_e.Scale / 2;

						x_hr &= (int)image_subpixel_scale_e.Mask;
						y_hr &= (int)image_subpixel_scale_e.Mask;

						fg_ptr = pSourceRenderingBuffer.GetPixelPointer(y_lr) + (x_lr << 2);

						weight = (uint)(((int)image_subpixel_scale_e.Scale - x_hr) *
								 ((int)image_subpixel_scale_e.Scale - y_hr));
						fg[0] += weight * fg_ptr[0];
						fg[1] += weight * fg_ptr[1];
						fg[2] += weight * fg_ptr[2];
						fg[3] += weight * fg_ptr[3];

						weight = (uint)(x_hr * ((int)image_subpixel_scale_e.Scale - y_hr));
						fg[0] += weight * fg_ptr[4];
						fg[1] += weight * fg_ptr[5];
						fg[2] += weight * fg_ptr[6];
						fg[3] += weight * fg_ptr[7];

						++y_lr;
						fg_ptr = pSourceRenderingBuffer.GetPixelPointer(y_lr) + (x_lr << 2);

						weight = (uint)(((int)image_subpixel_scale_e.Scale - x_hr) * y_hr);
						fg[0] += weight * fg_ptr[0];
						fg[1] += weight * fg_ptr[1];
						fg[2] += weight * fg_ptr[2];
						fg[3] += weight * fg_ptr[3];

						weight = (uint)(x_hr * y_hr);
						fg[0] += weight * fg_ptr[4];
						fg[1] += weight * fg_ptr[5];
						fg[2] += weight * fg_ptr[6];
						fg[3] += weight * fg_ptr[7];

						fg[0] >>= (int)image_subpixel_scale_e.Shift * 2;
						fg[1] >>= (int)image_subpixel_scale_e.Shift * 2;
						fg[2] >>= (int)image_subpixel_scale_e.Shift * 2;
						fg[3] >>= (int)image_subpixel_scale_e.Shift * 2;
					}
					else
					{
						if (x_lr < -1 || y_lr < -1 ||
						   x_lr > maxx || y_lr > maxy)
						{
							fg[OrderR] = back_r;
							fg[OrderG] = back_g;
							fg[OrderB] = back_b;
							fg[OrderA] = back_a;
						}
						else
						{
							fg[0] =
							fg[1] =
							fg[2] =
							fg[3] = (int)image_subpixel_scale_e.Scale * (int)image_subpixel_scale_e.Scale / 2;

							x_hr &= (int)image_subpixel_scale_e.Mask;
							y_hr &= (int)image_subpixel_scale_e.Mask;

							weight = (uint)(((int)image_subpixel_scale_e.Scale - x_hr) *
									 ((int)image_subpixel_scale_e.Scale - y_hr));
							BlendInFilterPixel(fg, back_r, back_g, back_b, back_a, pSourceRenderingBuffer, maxx, maxy, x_lr, y_lr, weight);

							x_lr++;

							weight = (uint)(x_hr * ((int)image_subpixel_scale_e.Scale - y_hr));
							BlendInFilterPixel(fg, back_r, back_g, back_b, back_a, pSourceRenderingBuffer, maxx, maxy, x_lr, y_lr, weight);

							x_lr--;
							y_lr++;

							weight = (uint)(((int)image_subpixel_scale_e.Scale - x_hr) * y_hr);
							BlendInFilterPixel(fg, back_r, back_g, back_b, back_a, pSourceRenderingBuffer, maxx, maxy, x_lr, y_lr, weight);

							x_lr++;

							weight = (uint)(x_hr * y_hr);
							BlendInFilterPixel(fg, back_r, back_g, back_b, back_a, pSourceRenderingBuffer, maxx, maxy, x_lr, y_lr, weight);

							fg[0] >>= (int)image_subpixel_scale_e.Shift * 2;
							fg[1] >>= (int)image_subpixel_scale_e.Shift * 2;
							fg[2] >>= (int)image_subpixel_scale_e.Shift * 2;
							fg[3] >>= (int)image_subpixel_scale_e.Shift * 2;
						}
					}

					(*span).m_R = (byte)fg[0];
					(*span).m_G = (byte)fg[1];
					(*span).m_B = (byte)fg[2];
					(*span).m_A = (byte)fg[3];
					++span;
					spanInterpolator.Next();
				} while (--len != 0);
			}
#if use_timers
                Generate_Span.Stop();
#endif
		}

		unsafe private void BlendInFilterPixel(uint* fg, uint back_r, uint back_g, uint back_b, uint back_a, RasterBuffer pSourceRenderingBuffer, int maxx, int maxy, int x_lr, int y_lr, uint weight)
		{
			unchecked
			{
				byte* fg_ptr;
				if ((uint)x_lr <= (uint)maxx && (uint)y_lr <= (uint)maxy)
				{
					fg_ptr = pSourceRenderingBuffer.GetPixelPointer(y_lr) + (x_lr << 2);

					fg[0] += weight * fg_ptr[0];
					fg[1] += weight * fg_ptr[1];
					fg[2] += weight * fg_ptr[2];
					fg[3] += weight * fg_ptr[3];
				}
				else
				{
					fg[OrderR] += back_r * weight;
					fg[OrderG] += back_g * weight;
					fg[OrderB] += back_b * weight;
					fg[OrderA] += back_a * weight;
				}
			}
		}
	};

	/*

	//==============================================span_image_filter_rgba_2x2
	//template<class Source, class Interpolator>
	public class span_image_filter_rgba_2x2 : SpanImageFilter//<Source, Interpolator>
	{
		//typedef Source source_type;
		//typedef typename source_type::color_type color_type;
		//typedef typename source_type::order_type order_type;
		//typedef Interpolator interpolator_type;
		//typedef SpanImageFilter<source_type, interpolator_type> base_type;
		//typedef typename color_type::value_type value_type;
		//typedef typename color_type::calc_type calc_type;
		enum base_scale_e
		{
			BaseShift = 8, //color_type::BaseShift,
			BaseMask  = 255,//color_type::BaseMask
		};

		//--------------------------------------------------------------------
		public span_image_filter_rgba_2x2() {}
		public span_image_filter_rgba_2x2(pixfmt_alpha_blend_bgra32 src,
								   interpolator_type inter,
								   ImageFilterLookUpTable filter) :
			base(src, inter, filter)
		{}

		//--------------------------------------------------------------------
		public void Generate(color_type* Span, int x, int y, unsigned len)
		{
			base.Interpolator().Begin(x + base.filter_dx_dbl(),
											y + base.filter_dy_dbl(), len);

			calc_type fg[4];

			byte *fg_ptr;
			int16* weight_array = base.filter().weight_array() +
										((base.filter().diameter()/2 - 1) <<
										  Shift);

			do
			{
				int x_hr;
				int y_hr;

				base.Interpolator().Coordinates(&x_hr, &y_hr);

				x_hr -= base.filter_dx_int();
				y_hr -= base.filter_dy_int();

				int x_lr = x_hr >> Shift;
				int y_lr = y_hr >> Shift;

				unsigned weight;
				fg[0] = fg[1] = fg[2] = fg[3] = (int)EImageFilterScale.Scale / 2;

				x_hr &= Mask;
				y_hr &= Mask;

				fg_ptr = base.source().Span(x_lr, y_lr, 2);
				weight = (weight_array[x_hr + Scale] *
						  weight_array[y_hr + Scale] +
						  (int)EImageFilterScale.Scale / 2) >>
						  Shift;
				fg[0] += weight * *fg_ptr++;
				fg[1] += weight * *fg_ptr++;
				fg[2] += weight * *fg_ptr++;
				fg[3] += weight * *fg_ptr;

				fg_ptr = base.source().NextX();
				weight = (weight_array[x_hr] *
						  weight_array[y_hr + Scale] +
						  (int)EImageFilterScale.Scale / 2) >>
						  Shift;
				fg[0] += weight * *fg_ptr++;
				fg[1] += weight * *fg_ptr++;
				fg[2] += weight * *fg_ptr++;
				fg[3] += weight * *fg_ptr;

				fg_ptr = base.source().NextY();
				weight = (weight_array[x_hr + Scale] *
						  weight_array[y_hr] +
						  (int)EImageFilterScale.Scale / 2) >>
						  Shift;
				fg[0] += weight * *fg_ptr++;
				fg[1] += weight * *fg_ptr++;
				fg[2] += weight * *fg_ptr++;
				fg[3] += weight * *fg_ptr;

				fg_ptr = base.source().NextX();
				weight = (weight_array[x_hr] *
						  weight_array[y_hr] +
						  (int)EImageFilterScale.Scale / 2) >>
						  Shift;
				fg[0] += weight * *fg_ptr++;
				fg[1] += weight * *fg_ptr++;
				fg[2] += weight * *fg_ptr++;
				fg[3] += weight * *fg_ptr;

				fg[0] >>= Shift;
				fg[1] >>= Shift;
				fg[2] >>= Shift;
				fg[3] >>= Shift;

				if(fg[OrderA] > BaseMask)         fg[OrderA] = BaseMask;
				if(fg[OrderR] > fg[OrderA]) fg[OrderR] = fg[OrderA];
				if(fg[OrderG] > fg[OrderA]) fg[OrderG] = fg[OrderA];
				if(fg[OrderB] > fg[OrderA]) fg[OrderB] = fg[OrderA];

				Span->r = (byte)fg[OrderR];
				Span->g = (byte)fg[OrderG];
				Span->b = (byte)fg[OrderB];
				Span->a = (byte)fg[OrderA];
				++Span;
				++base.Interpolator();
			} while(--len);
		}
	};

	//==================================================span_image_filter_rgba
	//template<class Source, class Interpolator>
	public class span_image_filter_rgba : SpanImageFilter//<Source, Interpolator>
	{
		//typedef Source source_type;
		//typedef typename source_type::color_type color_type;
		//typedef typename source_type::order_type order_type;
		//typedef Interpolator interpolator_type;
		//typedef SpanImageFilter<source_type, interpolator_type> base_type;
		//typedef typename color_type::value_type value_type;
		//typedef typename color_type::calc_type calc_type;
		enum base_scale_e
		{
			BaseShift = 8, //color_type::BaseShift,
			BaseMask  = 255,//color_type::BaseMask
		};

		//--------------------------------------------------------------------
		public span_image_filter_rgba() {}
		public span_image_filter_rgba(pixfmt_alpha_blend_bgra32 src,
							   interpolator_type inter,
							   ImageFilterLookUpTable filter) :
			base(src, inter, &filter)
		{}

		//--------------------------------------------------------------------
		public void Generate(color_type* Span, int x, int y, unsigned len)
		{
			base.Interpolator().Begin(x + base.filter_dx_dbl(),
											y + base.filter_dy_dbl(), len);

			int fg[4];
			byte *fg_ptr;

			unsigned     diameter     = base.filter().diameter();
			int          Start        = base.filter().Start();
			int16* weight_array = base.filter().weight_array();

			int x_count;
			int weight_y;

			do
			{
				base.Interpolator().Coordinates(&x, &y);

				x -= base.filter_dx_int();
				y -= base.filter_dy_int();

				int x_hr = x;
				int y_hr = y;

				int x_lr = x_hr >> Shift;
				int y_lr = y_hr >> Shift;

				fg[0] = fg[1] = fg[2] = fg[3] = (int)EImageFilterScale.Scale / 2;

				int x_fract = x_hr & Mask;
				unsigned y_count = diameter;

				y_hr = Mask - (y_hr & Mask);
				fg_ptr = base.source().Span(x_lr + Start,
																	 y_lr + Start,
																	 diameter);
				for(;;)
				{
					x_count  = diameter;
					weight_y = weight_array[y_hr];
					x_hr = Mask - x_fract;
					for(;;)
					{
						int weight = (weight_y * weight_array[x_hr] +
									 (int)EImageFilterScale.Scale / 2) >>
									 Shift;

						fg[0] += weight * *fg_ptr++;
						fg[1] += weight * *fg_ptr++;
						fg[2] += weight * *fg_ptr++;
						fg[3] += weight * *fg_ptr;

						if(--x_count == 0) break;
						x_hr  += Scale;
						fg_ptr = base.source().NextX();
					}

					if(--y_count == 0) break;
					y_hr  += Scale;
					fg_ptr = base.source().NextY();
				}

				fg[0] >>= Shift;
				fg[1] >>= Shift;
				fg[2] >>= Shift;
				fg[3] >>= Shift;

				if(fg[0] < 0) fg[0] = 0;
				if(fg[1] < 0) fg[1] = 0;
				if(fg[2] < 0) fg[2] = 0;
				if(fg[3] < 0) fg[3] = 0;

				if(fg[OrderA] > BaseMask)         fg[OrderA] = BaseMask;
				if(fg[OrderR] > fg[OrderA]) fg[OrderR] = fg[OrderA];
				if(fg[OrderG] > fg[OrderA]) fg[OrderG] = fg[OrderA];
				if(fg[OrderB] > fg[OrderA]) fg[OrderB] = fg[OrderA];

				Span->r = (byte)fg[OrderR];
				Span->g = (byte)fg[OrderG];
				Span->b = (byte)fg[OrderB];
				Span->a = (byte)fg[OrderA];
				++Span;
				++base.Interpolator();
			} while(--len);
		}
	};

	//========================================span_image_resample_rgba_affine
	public class span_image_resample_rgba_affine : span_image_resample_affine
	{
		//typedef Source source_type;
		//typedef typename source_type::color_type color_type;
		//typedef typename source_type::order_type order_type;
		//typedef span_image_resample_affine<source_type> base_type;
		//typedef typename base.interpolator_type interpolator_type;
		//typedef typename color_type::value_type value_type;
		//typedef typename color_type::long_type long_type;
		enum base_scale_e
		{
			BaseShift      = 8, //color_type::BaseShift,
			BaseMask       = 255,//color_type::BaseMask,
			downscale_shift = Shift
		};

		//--------------------------------------------------------------------
		public span_image_resample_rgba_affine() {}
		public span_image_resample_rgba_affine(pixfmt_alpha_blend_bgra32 src,
										interpolator_type inter,
										ImageFilterLookUpTable filter) :
			base(src, inter, filter)
		{}

		//--------------------------------------------------------------------
		public void Generate(color_type* Span, int x, int y, unsigned len)
		{
			base.Interpolator().Begin(x + base.filter_dx_dbl(),
											y + base.filter_dy_dbl(), len);

			long_type fg[4];

			int diameter     = base.filter().diameter();
			int filter_scale = diameter << Shift;
			int radius_x     = (diameter * base.m_rx) >> 1;
			int radius_y     = (diameter * base.m_ry) >> 1;
			int len_x_lr     =
				(diameter * base.m_rx + Mask) >>
					Shift;

			int16* weight_array = base.filter().weight_array();

			do
			{
				base.Interpolator().Coordinates(&x, &y);

				x += base.filter_dx_int() - radius_x;
				y += base.filter_dy_int() - radius_y;

				fg[0] = fg[1] = fg[2] = fg[3] = (int)EImageFilterScale.Scale / 2;

				int y_lr = y >> Shift;
				int y_hr = ((Mask - (y & Mask)) *
								base.m_ry_inv) >>
									Shift;
				int total_weight = 0;
				int x_lr = x >> Shift;
				int x_hr = ((Mask - (x & Mask)) *
								base.m_rx_inv) >>
									Shift;

				int x_hr2 = x_hr;
				byte* fg_ptr = base.source().Span(x_lr, y_lr, len_x_lr);
				for(;;)
				{
					int weight_y = weight_array[y_hr];
					x_hr = x_hr2;
					for(;;)
					{
						int weight = (weight_y * weight_array[x_hr] +
									 (int)EImageFilterScale.Scale / 2) >>
									 downscale_shift;

						fg[0] += *fg_ptr++ * weight;
						fg[1] += *fg_ptr++ * weight;
						fg[2] += *fg_ptr++ * weight;
						fg[3] += *fg_ptr++ * weight;
						total_weight += weight;
						x_hr  += base.m_rx_inv;
						if(x_hr >= filter_scale) break;
						fg_ptr = base.source().NextX();
					}
					y_hr += base.m_ry_inv;
					if(y_hr >= filter_scale) break;
					fg_ptr = base.source().NextY();
				}

				fg[0] /= total_weight;
				fg[1] /= total_weight;
				fg[2] /= total_weight;
				fg[3] /= total_weight;

				if(fg[0] < 0) fg[0] = 0;
				if(fg[1] < 0) fg[1] = 0;
				if(fg[2] < 0) fg[2] = 0;
				if(fg[3] < 0) fg[3] = 0;

				if(fg[OrderA] > BaseMask)         fg[OrderA] = BaseMask;
				if(fg[OrderR] > fg[OrderA]) fg[OrderR] = fg[OrderA];
				if(fg[OrderG] > fg[OrderA]) fg[OrderG] = fg[OrderA];
				if(fg[OrderB] > fg[OrderA]) fg[OrderB] = fg[OrderA];

				Span->r = (byte)fg[OrderR];
				Span->g = (byte)fg[OrderG];
				Span->b = (byte)fg[OrderB];
				Span->a = (byte)fg[OrderA];

				++Span;
				++base.Interpolator();
			} while(--len);
		}
	};
	 */

	//==============================================SpanImageResampleRGBA
	public class SpanImageResampleRGBA
		: SpanImageResample
	{
		private int OrderR;
		private int OrderG;
		private int OrderB;
		private int OrderA;
		private const int base_mask = 255;
		private const int downscale_shift = (int)ImageFilterLookUpTable.EImageFilterScale.Shift;

		//--------------------------------------------------------------------
		public SpanImageResampleRGBA(IRasterBufferAccessor src,
							ISpanInterpolator inter,
							ImageFilterLookUpTable filter) :
			base(src, inter, filter)
		{
			if (src.PixelFormat.Blender.NumPixelBits != 32)
			{
				throw new System.FormatException("You have to use a rgba blender with SpanImageResampleRGBA");
			}
			OrderR = src.PixelFormat.Blender.OrderR;
			OrderG = src.PixelFormat.Blender.OrderG;
			OrderB = src.PixelFormat.Blender.OrderB;
			OrderA = src.PixelFormat.Blender.OrderA;
		}

		//--------------------------------------------------------------------
		public unsafe override void Generate(RGBA_Bytes* span, int x, int y, uint len)
		{
			ISpanInterpolator spanInterpolator = base.interpolator();
			spanInterpolator.Begin(x + base.filter_dx_dbl(), y + base.filter_dy_dbl(), len);

			int* fg = stackalloc int[4];

			byte* fg_ptr;
			fixed (short* pWeightArray = filter().weight_array())
			{
				int diameter = (int)base.filter().diameter();
				int filter_scale = diameter << (int)image_subpixel_scale_e.Shift;

				short* weight_array = pWeightArray;

				do
				{
					int rx;
					int ry;
					int rx_inv = (int)image_subpixel_scale_e.Scale;
					int ry_inv = (int)image_subpixel_scale_e.Scale;
					spanInterpolator.Coordinates(out x, out y);
					spanInterpolator.LocalScale(out rx, out ry);
					base.AdjustScale(ref rx, ref ry);

					rx_inv = (int)image_subpixel_scale_e.Scale * (int)image_subpixel_scale_e.Scale / rx;
					ry_inv = (int)image_subpixel_scale_e.Scale * (int)image_subpixel_scale_e.Scale / ry;

					int radius_x = (diameter * rx) >> 1;
					int radius_y = (diameter * ry) >> 1;
					int len_x_lr =
						(diameter * rx + (int)image_subpixel_scale_e.Mask) >>
							(int)(int)image_subpixel_scale_e.Shift;

					x += base.filter_dx_int() - radius_x;
					y += base.filter_dy_int() - radius_y;

					fg[0] = fg[1] = fg[2] = fg[3] = (int)image_filter_scale_e.Scale / 2;

					int y_lr = y >> (int)(int)image_subpixel_scale_e.Shift;
					int y_hr = (((int)image_subpixel_scale_e.Mask - (y & (int)image_subpixel_scale_e.Mask)) *
								   ry_inv) >>
									   (int)(int)image_subpixel_scale_e.Shift;
					int total_weight = 0;
					int x_lr = x >> (int)(int)image_subpixel_scale_e.Shift;
					int x_hr = (((int)image_subpixel_scale_e.Mask - (x & (int)image_subpixel_scale_e.Mask)) *
								   rx_inv) >>
									   (int)(int)image_subpixel_scale_e.Shift;
					int x_hr2 = x_hr;
					fg_ptr = base.source().Span(x_lr, y_lr, (uint)len_x_lr);

					for (; ; )
					{
						int weight_y = weight_array[y_hr];
						x_hr = x_hr2;
						for (; ; )
						{
							int weight = (weight_y * weight_array[x_hr] +
										 (int)image_filter_scale_e.Scale / 2) >>
										 downscale_shift;
							fg[0] += *fg_ptr++ * weight;
							fg[1] += *fg_ptr++ * weight;
							fg[2] += *fg_ptr++ * weight;
							fg[3] += *fg_ptr++ * weight;
							total_weight += weight;
							x_hr += rx_inv;
							if (x_hr >= filter_scale) break;
							fg_ptr = base.source().NextX();
						}
						y_hr += ry_inv;
						if (y_hr >= filter_scale)
						{
							break;
						}

						fg_ptr = base.source().NextY();
					}

					fg[0] /= total_weight;
					fg[1] /= total_weight;
					fg[2] /= total_weight;
					fg[3] /= total_weight;

					if (fg[0] < 0) fg[0] = 0;
					if (fg[1] < 0) fg[1] = 0;
					if (fg[2] < 0) fg[2] = 0;
					if (fg[3] < 0) fg[3] = 0;

					if (fg[0] > fg[0]) fg[0] = fg[0];
					if (fg[1] > fg[1]) fg[1] = fg[1];
					if (fg[2] > fg[2]) fg[2] = fg[2];
					if (fg[3] > base_mask) fg[3] = base_mask;

					span->R_Byte = (byte)fg[OrderR];
					span->G_Byte = (byte)fg[OrderG];
					span->B_Byte = (byte)fg[OrderB];
					span->A_Byte = (byte)fg[OrderA];

					++span;
					interpolator().Next();
				} while (--len != 0);
			}
		}
	};
}

//#endif