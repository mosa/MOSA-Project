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
//          mcseemPictor@yahoo.com
//          http://www.antigrain.com
//----------------------------------------------------------------------------
//
// class platform_support
// 
//----------------------------------------------------------------------------
//#define USE_OPENGL

using System;
using System.Windows.Forms;
using System.Drawing;
using Pictor.PixelFormat;

namespace Pictor.UI.EmulatorPlatform
{
    
    public class PlatformSupport : PlatformSupportAbstract
    {
        private Renderer m_ScreenRenderer;
        private PlatformSpecificWindow m_specific;

        //------------------------------------------------------------------------
        public PlatformSupport(PixelFormats format, PlatformSupportAbstract.ERenderOrigin RenderOrigin)
            : base(format, RenderOrigin)
        {
            m_specific = new PlatformSpecificWindow(this, format, RenderOrigin);
            m_format = format;
            //m_bpp(m_specific->m_bpp),
            //m_window_flags(0),
            m_wait_mode = true;
            m_RenderOrigin = RenderOrigin;
            m_initial_width = 10;
            m_initial_height = 10;

            Caption = "Anti-Grain Geometry Application";

            Focus();
        }

        //------------------------------------------------------------------------
        public override string Caption
        {
            get
            {
                return m_specific.Text;
            }
            set
            {
                m_specific.Text = value;
            }
        }

        public override void OnIdle() { }
        public override void OnResize(int sx, int sy)
        {
            m_Bounds.Bottom = 0;
            m_Bounds.Left = 0;
            m_Bounds.Top = sy;
            m_Bounds.Right = sx;
            if (m_ScreenRenderer != null)
            {
                m_ScreenRenderer.Rasterizer.SetVectorClipBox(0, 0, Width, Height);
            }
        }

        public override Renderer GetRenderer()
        {
            return m_ScreenRenderer;
        }
        public override void OnControlChanged() { }

        public override void Invalidate()
        {
            m_specific.Invalidate();
            m_specific.m_WindowContentNeedsRedraw = true;
        }

        public override void Invalidate(RectD rectToInvalidate)
        {
            m_specific.Invalidate(new Rectangle(
                (int)System.Math.Floor(rectToInvalidate.Left),
                (int)System.Math.Floor(Height - rectToInvalidate.Top),
                (int)System.Math.Ceiling(rectToInvalidate.Width),
                (int)System.Math.Ceiling(rectToInvalidate.Height)));
            m_specific.m_WindowContentNeedsRedraw = true;
        }


        public override void Close()
        {
            m_specific.Close();
        }

        //------------------------------------------------------------------------
        public override void ForceRedraw()
        {
            m_specific.ForceRedraw();
        }

        //------------------------------------------------------------------------
        public override void UpdateWindow()
        {
            if (!m_specific.IsDisposed)
            {
                Graphics displayGraphics = m_specific.CreateGraphics();
                m_specific.display_pmap(displayGraphics, m_rbuf_window);
            }
        }

        //------------------------------------------------------------------------
        public override bool Init(uint width, uint height, uint flags)
        {
            if (m_specific.m_sys_format == PlatformSupportAbstract.PixelFormats.Undefined)
            {
                return false;
            }

            m_window_flags = flags;

            System.Drawing.Size clientSize = new System.Drawing.Size();
            clientSize.Width = (int)width;
            clientSize.Height = (int)height;
            m_specific.ClientSize = clientSize;

            if ((m_window_flags & (uint)EWindowFlags.Risizeable) == 0)
            {
                m_specific.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
                m_specific.MaximizeBox = false;
            }

            m_rbuf_window = new RasterBuffer();
            m_specific.create_pmap(width, height, m_rbuf_window);
            m_initial_width = width;
            m_initial_height = height;

            m_specific.Show();

            m_Bounds.Bottom = 0;
            m_Bounds.Left = 0;
            m_Bounds.Top = height;
            m_Bounds.Right = width;

            OnInitialize();
            m_specific.m_WindowContentNeedsRedraw = true;

            return true;
        }

        public override void OnInitialize()
        {
            IPixelFormat screenPixelFormat;
            FormatClippingProxy screenClippingProxy;

            AntiAliasedScanlineRasterizer rasterizer = new AntiAliasedScanlineRasterizer();
            Scanline scanlinePacked = new Scanline();

            if (RenderBufferWindow.BitsPerPixel == 24)
            {
                screenPixelFormat = new FormatRGB(RenderBufferWindow, new BlenderBGR());
            }
            else
            {
                screenPixelFormat = new FormatRGBA(RenderBufferWindow, new BlenderBGRA());
            }
            screenClippingProxy = new FormatClippingProxy(screenPixelFormat);

            m_ScreenRenderer = new Renderer(screenClippingProxy, rasterizer, scanlinePacked);
            m_ScreenRenderer.Rasterizer.SetVectorClipBox(0, 0, Width, Height);

            base.OnInitialize();
        }

        //------------------------------------------------------------------------
        public override void Run()
        {
            while (m_specific.Created)
            {
                Application.DoEvents();
                if (m_wait_mode)
                {
                    System.Threading.Thread.Sleep(1);
                }
                else
                {
                    OnIdle();
                }
            }
        }

        //------------------------------------------------------------------------
        public override bool LoadImage(uint idx, string file)
        {
            if (idx < (uint)max_images_e.max_images)
            {
                int len = file.Length;
                if (len < 4 || !file.EndsWith(".BMP"))
                {
                    file += ".bmp";
                }

                RasterBuffer temp = new RasterBuffer();
                bool goodLoad = m_specific.load_pmap(file, idx, temp);
                m_rbuf_img.Add(temp);
                return goodLoad;
            }
            return true;
        }

        //------------------------------------------------------------------------
        public override string ImageExtension() { return ".bmp"; }

        //------------------------------------------------------------------------
        public void start_timer()
        {
            m_specific.m_StopWatch.Reset();
            m_specific.m_StopWatch.Start();
        }

        //------------------------------------------------------------------------
        public double elapsed_time()
        {
            m_specific.m_StopWatch.Stop();
            return m_specific.m_StopWatch.Elapsed.TotalMilliseconds;
        }

        public override bool CreateImage(uint idx, uint width, uint height, uint BitsPerPixel)
        {
            if (idx < (uint)max_images_e.max_images)
            {
                if (width == 0) width = m_specific.m_pmap_window.Width();
                if (height == 0) height = m_specific.m_pmap_window.Height();
                if (m_specific.m_pmap_img[idx] == null)
                {
                    m_specific.m_pmap_img[idx] = new PixelMap();
                    m_rbuf_img.Add(new RasterBuffer());
                }
                m_specific.m_pmap_img[idx].Create(width, height, BitsPerPixel);
                unsafe
                {
                    m_rbuf_img[(int)idx].Attach(m_specific.m_pmap_img[idx].buf(),
                                           m_specific.m_pmap_img[idx].Width(),
                                           m_specific.m_pmap_img[idx].Height(),
                                            m_specific.m_pmap_img[idx].stride(),
                                            m_specific.m_pmap_img[idx].bpp());
                }
                return true;
            }
            return false;
        }

        public override void Message(String msg)
        {
            System.Windows.Forms.MessageBox.Show(msg, "PictorSharp message");
        }
    };
}
