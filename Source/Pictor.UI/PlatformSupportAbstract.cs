//----------------------------------------------------------------------------
// Anti-Grain Geometry - Version 2.4
// Copyright (C) 2002-2005 Maxim Shemanarev (http://www.antigrain.com)
//
// C# Port port by: Lars Brubaker
//                  larsbrubaker@gmail.com
// Copyright (C) 2007
//
// Permission to copy, use, modify, sell and distribute this software 
// is granted provided this copyright notice appears in all copies. 
// This software is provided "as is" without express or implied
// warranty, and with no claim as to its suitability for any purpose.
//
//----------------------------------------------------------------------------
// Contact: mcseem@antigrain.com
//          mcseemagg@yahoo.com
//          http://www.antigrain.com
//----------------------------------------------------------------------------
//
// class platform_support
//
// It's not a part of the AGG library, it's just a helper class to create 
// interactive demo examples. Since the examples should not be too complex
// this class is provided to support some very basic interactive graphical
// funtionality, such as putting the rendered image to the window, simple 
// keyboard and mouse input, window resizing, setting the window title,
// and catching the "idle" events.
// 
// The idea is to have a single header file that does not depend on any 
// platform (I hate these endless #ifdef/#elif/#elif.../#endif) and a number
// of different implementations depending on the concrete platform. 
// The most popular platforms are:
//
// Windows-32 API
// X-Window API
// SDL library (see http://www.libsdl.org/)
// MacOS C/C++ API
// 
// This file does not include any system dependent .h files such as
// windows.h or X11.h, so, your demo applications do not depend on the
// platform. The only file that can #include system dependend headers
// is the implementation file agg_platform_support.cpp. Different
// implementations are placed in different directories, such as
// ~/agg/src/platform/win32
// ~/agg/src/platform/sdl
// ~/agg/src/platform/X11
// and so on.
//
//----------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using Pictor.Transform;

namespace Pictor.UI
{
	abstract public class PlatformSupportAbstract : UIWidget
	{
		public enum max_images_e { max_images = 16 };

		public enum ERenderOrigin { OriginTopLeft, OriginBottomLeft };

		//----------------------------------------------------------window_flag_e
		// These are flags used in method Init(). Not all of them are
		// applicable on different platforms, for example the win32_api
		// cannot use a hardware buffer (window_hw_buffer).
		// The implementation should simply ignore unsupported flags.
		public enum EWindowFlags
		{
			None = 0,
			Risizeable = 1,
			KeepAspectRatio = 4,
			UseOpenGL = 16,
		};

		//-----------------------------------------------------------e
		// Possible formats of the rendering buffer. Initially I thought that it's
		// reasonable to create the buffer and the rendering functions in 
		// accordance with the native pixel Format of the system because it 
		// would have no overhead for pixel Format conversion. 
		// But eventually I came to a conclusion that having a possibility to 
		// convert pixel formats on demand is a good idea. First, it was X11 where 
		// there lots of different formats and visuals and it would be great to 
		// render everything in, say, RGB-24 and display it automatically without
		// any additional efforts. The second reason is to have a possibility to 
		// debug renderers for different pixel formats and colorspaces having only 
		// one computer and one system.
		//
		// This stuff is not included into the basic AGG functionality because the 
		// number of supported pixel formats (and/or colorspaces) can be great and 
		// if one needs to add new Format it would be good only to add new 
		// rendering files without having to modify any existing ones (a general 
		// principle of incapsulation and isolation).
		//
		// Using a particular pixel Format doesn't obligatory mean the necessity
		// of software conversion. For example, win32 API can natively display 
		// gray8, 15-bit RGB, 24-bit BGR, and 32-bit BGRA formats. 
		// This list can be (and will be!) extended in future.
		public enum PixelFormats
		{
			Undefined = 0,  // By default. No conversions are applied 
			BlackWhite,             // 1 bit per Color B/W
			Gray8,          // Simple 256 level grayscale
			Gray16,         // Simple 65535 level grayscale
			Rgb555,         // 15 bit rgb. Depends on the byte ordering!
			Rgb565,         // 16 bit rgb. Depends on the byte ordering!
			RgbAAA,         // 30 bit rgb. Depends on the byte ordering!
			RgbBBA,         // 32 bit rgb. Depends on the byte ordering!
			BgrAAA,         // 30 bit bgr. Depends on the byte ordering!
			BgrABB,         // 32 bit bgr. Depends on the byte ordering!
			Rgb24,          // R-G-B, one byte per Color component
			Bgr24,          // B-G-R, native win32 BMP Format.
			Rgba32,         // R-G-B-A, one byte per Color component
			Argb32,         // A-R-G-B, native MAC Format
			Abgr32,         // A-B-G-R, one byte per Color component
			Bgra32,         // B-G-R-A, native win32 BMP Format
			Rgb48,          // R-G-B, 16 bits per Color component
			Bgr48,          // B-G-R, native win32 BMP Format.
			Rgba64,         // R-G-B-A, 16 bits byte per Color component
			Argb64,         // A-R-G-B, native MAC Format
			Abgr64,         // A-B-G-R, one byte per Color component
			Bgra64,         // B-G-R-A, native win32 BMP Format

			EndOfPixelFormats
		};

		public static uint GetBitDepthForPixelFormat(PixelFormats pixelFormat)
		{
			switch (pixelFormat)
			{
				case PixelFormats.Bgr24:
				case PixelFormats.Rgb24:
					return 24;

				case PixelFormats.Bgra32:
				case PixelFormats.Rgba32:
					return 32;

				default:
					throw new System.NotImplementedException();
			}
		}

		// Format - see enum e {};
		// FlipY - true if you want to have the Y-axis flipped vertically.
		public PlatformSupportAbstract(PixelFormats format, PlatformSupportAbstract.ERenderOrigin RenderOrigin)
		{
			m_format = format;
			m_bpp = GetBitDepthForPixelFormat(format);
			m_RenderOrigin = RenderOrigin;
			m_initial_width = 10;
			m_initial_height = 10;
			m_resize_mtx = Affine.NewIdentity();
			m_rbuf_img = new List<RasterBuffer>();
		}

		//~platform_support();

		// Setting the windows caption (title). Should be able
		// to be called at least before calling Init(). 
		// It's perfect if they can be called anytime.
		public abstract string Caption
		{
			get;
			set;
		}

		// These 3 methods handle working with images. The image
		// formats are the simplest ones, such as .BMP in Windows or 
		// .ppm in Linux. In the applications the names of the files
		// should not have any file extensions. Method LoadImage() can
		// be called before Init(), so, the application could be able 
		// to determine the initial size of the window depending on 
		// the size of the loaded image. 
		// The argument "idx" is the number of the image 0...max_images-1
		abstract public bool LoadImage(uint idx, String file);
		abstract public bool CreateImage(uint idx, uint width, uint height, uint BitsPerPixel);
		/*
		abstract public bool save_img(uint idx, String file);

		public bool CreateImage(uint idx)
		{
			return CreateImage(idx, 0, 0);
		}

		 */

		public virtual void OnInitialize() { }

		// Init() and Run(). See description before the class for details.
		// The necessity of calling Init() after creation is that it's 
		// impossible to call the overridden virtual function (OnInitialize()) 
		// from the constructor. On the other hand it's very useful to have
		// some OnInitialize() event handler when the window is created but 
		// not yet displayed. The RenderBufferWindow() method (see below) is 
		// accessible from OnInitialize().
		abstract public bool Init(uint width, uint height, uint flags);
		abstract public void Run();
		// The very same parameters that were used in the constructor
		public PixelFormats Format
		{
			get { return m_format; }
		}
		public PlatformSupportAbstract.ERenderOrigin RenderOrigin
		{
			get
			{
				return m_RenderOrigin;
			}
			set
			{
				m_RenderOrigin = value;
			}
		}
		public bool FlipY()
		{
			return RenderOrigin == ERenderOrigin.OriginBottomLeft;
		}
		public uint bpp() { return m_bpp; }

		// The following provides a very simple mechanism of doing someting
		// in background. It's not multithreading. When WaitMode is true
		// the class waits for the events and it does not ever call on_idle().
		// When it's false it calls on_idle() when the event queue is empty.
		// The mode can be changed anytime. This mechanism is satisfactory
		// to create very simple animations.
		public bool WaitMode
		{
			get { return m_wait_mode; }
			set { m_wait_mode = value; }
		}

		// These two functions control updating of the window. 
		// ForceRedraw() is an analog of the Win32 InvalidateRect() function.
		// Being called it sets a flag (or sends a Message) which results
		// in calling OnDraw() and updating the content of the window 
		// when the next event cycle comes.
		// UpdateWindow() results in just putting immediately the content 
		// of the currently rendered buffer to the window without calling
		// OnDraw().
		abstract public void ForceRedraw();
		abstract public void UpdateWindow();

		abstract public void Close();

		// So, finally, how to draw anything with AGG? Very simple.
		// RenderBufferWindow() returns a reference to the main rendering 
		// buffer which can be attached to any rendering class.
		// RenderBufferImage() returns a reference to the previously created
		// or loaded image buffer (see LoadImage()). The image buffers 
		// are not displayed directly, they should be copied to or 
		// combined somehow with the RenderBufferWindow(). RenderBufferWindow() is
		// the only buffer that can be actually displayed.
		public RasterBuffer RenderBufferWindow
		{
			get { return m_rbuf_window; }
		}
		public RasterBuffer RenderBufferImage(uint idx) { return m_rbuf_img[(int)idx]; }

		// Returns file extension used in the implementation for the particular
		// system.
		abstract public String ImageExtension();

		/*
		public void copy_img_to_window(uint idx)
		{
			if(idx < max_images && RenderBufferImage(idx).buf())
			{
				RenderBufferWindow().copy_from(RenderBufferImage(idx));
			}
		}
        
		public void copy_window_to_img(uint idx)
		{
			if(idx < max_images)
			{
				CreateImage(idx, RenderBufferWindow().Width(), RenderBufferWindow().Height());
				RenderBufferImage(idx).copy_from(RenderBufferWindow());
			}
		}
       
		 */

		public void CopyImageToImage(uint idx_to, uint idx_from)
		{
			unsafe
			{
				if (idx_from < (uint)max_images_e.max_images
					&& idx_to < (int)max_images_e.max_images
					&& RenderBufferImage(idx_from).GetBuffer() != null)
				{
					CreateImage(idx_to,
							   RenderBufferImage(idx_from).Width(),
							   RenderBufferImage(idx_from).Height(), RenderBufferImage(idx_from).BitsPerPixel);
					RenderBufferImage(idx_to).CopyFrom(RenderBufferImage(idx_from));
				}
			}
		}

		public abstract void OnIdle();
		abstract public void OnResize(int sx, int sy);
		abstract public void OnControlChanged();

		// Auxiliary functions. TransAffineResizing() modifier sets up the resizing 
		// matrix on the basis of the given Width and Height and the initial
		// Width and Height of the window. The implementation should simply 
		// call this function every time when it catches the resizing event
		// passing in the new values of Width and Height of the window.
		// Nothing prevents you from "cheating" the scaling matrix if you
		// call this function from somewhere with wrong arguments. 
		// TransAffineResizing() accessor simply returns current resizing matrix 
		// which can be used to apply additional scaling of any of your 
		// stuff when the window is being resized.
		// Width(), Height(), InitialWidth(), and InitialHeight() must be
		// clear to understand with no comments :-)
		public void TransAffineResizing(int width, int height)
		{
			if ((m_window_flags & (uint)EWindowFlags.KeepAspectRatio) != 0)
			{
				double sx = (double)(width) / (double)(m_initial_width);
				double sy = (double)(height) / (double)(m_initial_height);
				if (sy < sx) sx = sy;
				m_resize_mtx = Affine.NewScaling(sx, sx);
				Transform.Viewport vp = new Transform.Viewport();
				vp.PreserveAspectRatio(0.5, 0.5, Pictor.Transform.Viewport.EAspectRatio.Meet);
				vp.DeviceViewport(0, 0, width, height);
				vp.WorldViewport(0, 0, m_initial_width, m_initial_height);
				m_resize_mtx = vp.ToAffine();
			}
			else
			{
				m_resize_mtx = Affine.NewScaling(
					(double)(width) / (double)(m_initial_width),
					(double)(height) / (double)(m_initial_height));
			}
		}

		public Transform.Affine TransAffineResizing() { return m_resize_mtx; }

		new public double Width
		{
			get { return m_rbuf_window.Width(); }
		}
		new public double Height
		{
			get { return m_rbuf_window.Height(); }
		}
		public double InitialWidth
		{
			get { return m_initial_width; }
		}
		public double InitialHeight
		{
			get { return m_initial_height; }
		}
		public uint WindowFlags
		{
			get { return m_window_flags; }
		}

		// Get raw display handler depending on the system. 
		// For win32 its an HDC, for other systems it can be a pointer to some
		// structure. See the implementation files for detals.
		// It's provided "as is", so, first you should check if it's not null.
		// If it's null the raw_display_handler is not supported. Also, there's 
		// no guarantee that this function is implemented, so, in some 
		// implementations you may have simply an unresolved symbol when linking.
		//public void* raw_display_handler();


		// display Message box or print the Message to the console 
		// (depending on implementation)
		abstract public void Message(String msg);

		// Stopwatch functions. Function elapsed_time() returns time elapsed 
		// since the latest start_timer() invocation in millisecods. 
		// The resolutoin depends on the implementation. 
		// In Win32 it uses QueryPerformanceFrequency() / QueryPerformanceCounter().
		//abstract public void start_timer();
		//abstract public double elapsed_time();

		// Get the full file name. In most cases it simply returns
		// file_name. As it's appropriate in many systems if you open
		// a file by its name without specifying the path, it tries to 
		// open it in the current directory. The demos usually expect 
		// all the supplementary files to be placed in the current 
		// directory, that is usually coincides with the directory where
		// the the executable is. However, in some systems (BeOS) it's not so. 
		// For those kinds of systems full_file_name() can help access files 
		// preserving commonly used policy.
		// So, it's a good idea to use in the demos the following:
		// FILE* fd = fopen(full_file_name("some.file"), "r"); 
		// instead of
		// FILE* fd = fopen("some.file", "r"); 
		//abstract public String full_file_name(String file_name);

		// Sorry, I'm too tired to describe the private 
		// data members. See the implementations for different
		// platforms for details.
		//private platform_support(platform_support platform);
		//private platform_support operator = (platform_support platform);

		protected PixelFormats m_format;
		protected uint m_bpp;
		protected RasterBuffer m_rbuf_window;
		protected List<RasterBuffer> m_rbuf_img;
		protected uint m_window_flags;
		protected bool m_wait_mode;
		protected PlatformSupportAbstract.ERenderOrigin m_RenderOrigin;
		protected uint m_initial_width;
		protected uint m_initial_height;
		protected Transform.Affine m_resize_mtx;
	};
}
