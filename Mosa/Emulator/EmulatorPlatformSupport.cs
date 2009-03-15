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
using System.Diagnostics;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Drawing.Imaging;
using Pictor.PixelFormat;

namespace Pictor.UI.EmulatorPlatform
{

    //------------------------------------------------------------------------
    public class PlatformSpecificWindow : Form
    {
        public PlatformSupportAbstract.PixelFormats m_format;
        public PlatformSupportAbstract.PixelFormats m_sys_format;
        private PlatformSupportAbstract.ERenderOrigin m_RenderOrigin;
        //private HWND          m_hwnd;
        internal PixelMap m_pmap_window;
        internal PixelMap[] m_pmap_img = new PixelMap[(int)PlatformSupportAbstract.max_images_e.max_images];
        private uint[] m_keymap = new uint[256];
        private uint m_last_translated_key;
        //private int           m_cur_x;
        //private int           m_cur_y;
        //private uint      m_input_flags;
        public bool m_WindowContentNeedsRedraw;
        public bool fullscreen;
        internal System.Diagnostics.Stopwatch m_StopWatch;
        PlatformSupportAbstract m_app;

        public PlatformSpecificWindow(PlatformSupportAbstract app, PlatformSupportAbstract.PixelFormats format, PlatformSupportAbstract.ERenderOrigin RenderOrigin)
        {
            m_app = app;
            m_format = format;
            m_sys_format = PlatformSupportAbstract.PixelFormats.Undefined;
            m_RenderOrigin = RenderOrigin;
            m_WindowContentNeedsRedraw = true;
            m_StopWatch = new Stopwatch();

            switch (m_format)
            {
                case PlatformSupportAbstract.PixelFormats.BlackWhite:
                    m_sys_format = PlatformSupportAbstract.PixelFormats.BlackWhite;
                    break;

                case PlatformSupportAbstract.PixelFormats.Gray8:
                    m_sys_format = PlatformSupportAbstract.PixelFormats.Gray8;
                    break;

                case PlatformSupportAbstract.PixelFormats.Gray16:
                    m_sys_format = PlatformSupportAbstract.PixelFormats.Gray16;
                    break;

                case PlatformSupportAbstract.PixelFormats.Rgb565:
                case PlatformSupportAbstract.PixelFormats.Rgb555:
                    m_sys_format = PlatformSupportAbstract.PixelFormats.Rgb555;
                    break;

                case PlatformSupportAbstract.PixelFormats.RgbAAA:
                case PlatformSupportAbstract.PixelFormats.BgrAAA:
                case PlatformSupportAbstract.PixelFormats.RgbBBA:
                case PlatformSupportAbstract.PixelFormats.BgrABB:
                    m_sys_format = PlatformSupportAbstract.PixelFormats.Bgr24;
                    break;

                case PlatformSupportAbstract.PixelFormats.Rgb24:
                    m_sys_format = PlatformSupportAbstract.PixelFormats.Rgb24;
                    break;

                case PlatformSupportAbstract.PixelFormats.Bgr24:
                    m_sys_format = PlatformSupportAbstract.PixelFormats.Bgr24;
                    break;

                case PlatformSupportAbstract.PixelFormats.Rgb48:
                case PlatformSupportAbstract.PixelFormats.Bgr48:
                    m_sys_format = PlatformSupportAbstract.PixelFormats.Bgr24;
                    break;

                case PlatformSupportAbstract.PixelFormats.Bgra32:
                    m_sys_format = PlatformSupportAbstract.PixelFormats.Bgra32;
                    break;
                case PlatformSupportAbstract.PixelFormats.Abgr32:
                    m_sys_format = PlatformSupportAbstract.PixelFormats.Abgr32;
                    break;
                case PlatformSupportAbstract.PixelFormats.Argb32:
                    m_sys_format = PlatformSupportAbstract.PixelFormats.Argb32;
                    break;
                case PlatformSupportAbstract.PixelFormats.Rgba32:
                    m_sys_format = PlatformSupportAbstract.PixelFormats.Rgba32;
                    break;

                case PlatformSupportAbstract.PixelFormats.Bgra64:
                case PlatformSupportAbstract.PixelFormats.Abgr64:
                case PlatformSupportAbstract.PixelFormats.Argb64:
                case PlatformSupportAbstract.PixelFormats.Rgba64:
                    m_sys_format = PlatformSupportAbstract.PixelFormats.Bgra32;
                    break;
            }

            m_StopWatch.Reset();
            m_StopWatch.Start();
        }

        protected override void OnKeyUp(System.Windows.Forms.KeyEventArgs windowsKeyEvent)
        {
            Pictor.UI.KeyEventArgs PictorKeyEvent = new Pictor.UI.KeyEventArgs((Pictor.UI.Keys)windowsKeyEvent.KeyData);
            m_app.OnKeyUp(PictorKeyEvent);

            windowsKeyEvent.Handled = PictorKeyEvent.Handled;
            windowsKeyEvent.SuppressKeyPress = PictorKeyEvent.SuppressKeyPress;

            base.OnKeyUp(windowsKeyEvent);
        }

        protected override void OnKeyDown(System.Windows.Forms.KeyEventArgs windowsKeyEvent)
        {
            Pictor.UI.KeyEventArgs PictorKeyEvent = new Pictor.UI.KeyEventArgs((Pictor.UI.Keys)windowsKeyEvent.KeyData);
            m_app.OnKeyDown(PictorKeyEvent);

            if (PictorKeyEvent.Handled)
            {
                m_app.OnControlChanged();
                m_app.ForceRedraw();
            }

            windowsKeyEvent.Handled = PictorKeyEvent.Handled;
            windowsKeyEvent.SuppressKeyPress = PictorKeyEvent.SuppressKeyPress;

            base.OnKeyDown(windowsKeyEvent);
        }

        protected override void OnKeyPress(System.Windows.Forms.KeyPressEventArgs windowsKeyPressEvent)
        {
            Pictor.UI.KeyPressEventArgs PictorKeyPressEvent = new Pictor.UI.KeyPressEventArgs(windowsKeyPressEvent.KeyChar);
            m_app.OnKeyPress(PictorKeyPressEvent);
            windowsKeyPressEvent.Handled = PictorKeyPressEvent.Handled;

            base.OnKeyPress(windowsKeyPressEvent);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (m_WindowContentNeedsRedraw)
            {
                m_app.OnDraw();
                m_WindowContentNeedsRedraw = false;
            }

            display_pmap(e.Graphics, m_app.RenderBufferWindow);
        }

        protected override void OnMouseDown(System.Windows.Forms.MouseEventArgs mouseEvent)
        {
            base.OnMouseDown(mouseEvent);

            int Y = mouseEvent.Y;
            if (m_app.FlipY())
            {
                Y = (int)m_app.RenderBufferWindow.Height() - Y;
            }
            Pictor.UI.MouseEventArgs PictorMouseEvent = new Pictor.UI.MouseEventArgs((Pictor.UI.MouseButtons)mouseEvent.Button, mouseEvent.Clicks, mouseEvent.X, Y, mouseEvent.Delta);

            m_app.SetChildCurrent(PictorMouseEvent.X, PictorMouseEvent.Y);
            m_app.OnMouseDown(PictorMouseEvent);
            if (PictorMouseEvent.Handled)
            {
                m_app.OnControlChanged();
                m_app.ForceRedraw();
            }
            else
            {
                if (m_app.InRect(PictorMouseEvent.X, PictorMouseEvent.Y))
                {
                    if (m_app.SetChildCurrent(PictorMouseEvent.X, PictorMouseEvent.Y))
                    {
                        m_app.OnControlChanged();
                        m_app.ForceRedraw();
                    }
                }
                else
                {
                    m_app.OnMouseDown(PictorMouseEvent);
                }
            }
        }

        protected override void OnMouseMove(System.Windows.Forms.MouseEventArgs mouseEvent)
        {
            base.OnMouseMove(mouseEvent);

            int Y = mouseEvent.Y;
            if (m_app.FlipY())
            {
                Y = (int)m_app.RenderBufferWindow.Height() - Y;
            }
            Pictor.UI.MouseEventArgs PictorMouseEvent = new Pictor.UI.MouseEventArgs((Pictor.UI.MouseButtons)mouseEvent.Button, mouseEvent.Clicks, mouseEvent.X, Y, mouseEvent.Delta);

            m_app.OnMouseMove(PictorMouseEvent);
            if (PictorMouseEvent.Handled)
            {
                m_app.OnControlChanged();
                m_app.ForceRedraw();
            }
            else
            {
                if (!m_app.InRect(PictorMouseEvent.X, PictorMouseEvent.Y))
                {
                    m_app.OnMouseMove(PictorMouseEvent);
                }
            }
        }

        protected override void OnMouseUp(System.Windows.Forms.MouseEventArgs mouseEvent)
        {
            base.OnMouseUp(mouseEvent);

            int Y = mouseEvent.Y;
            if (m_app.FlipY())
            {
                Y = (int)m_app.RenderBufferWindow.Height() - Y;
            }
            Pictor.UI.MouseEventArgs PictorMouseEvent = new Pictor.UI.MouseEventArgs((Pictor.UI.MouseButtons)mouseEvent.Button, mouseEvent.Clicks, mouseEvent.X, Y, mouseEvent.Delta);

            m_app.OnMouseUp(PictorMouseEvent);
            if (PictorMouseEvent.Handled)
            {
                m_app.OnControlChanged();
                m_app.ForceRedraw();
            }
            m_app.OnMouseUp(PictorMouseEvent);
        }

        public unsafe void create_pmap(uint width, uint height, RasterBuffer wnd)
        {
            m_pmap_window = new PixelMap();
            m_pmap_window.Create(width, height, PlatformSupportAbstract.GetBitDepthForPixelFormat(m_format));
            wnd.Attach(m_pmap_window.buf(),
                m_pmap_window.Width(),
                m_pmap_window.Height(),
                m_RenderOrigin == PlatformSupportAbstract.ERenderOrigin.OriginBottomLeft ? -m_pmap_window.stride() : m_pmap_window.stride(),
                m_pmap_window.bpp());
        }

        static void convert_pmap(RasterBuffer dst,
                                 RasterBuffer src,
                                 PlatformSupportAbstract.PixelFormats format)
        {
            
        }

        public void display_pmap(Graphics displayGraphics, RasterBuffer src)
        {
            if (m_sys_format == m_format)
            {
                m_pmap_window.Draw(displayGraphics);
            }
            else
            {
                PixelMap pmap_tmp = new PixelMap();
                pmap_tmp.Create(m_pmap_window.Width(),
                    m_pmap_window.Height(), PlatformSupportAbstract.GetBitDepthForPixelFormat(m_sys_format));

                RasterBuffer rbuf_tmp = new RasterBuffer();
                unsafe
                {
                    rbuf_tmp.Attach(pmap_tmp.buf(),
                        pmap_tmp.Width(),
                        pmap_tmp.Height(),
                        m_app.FlipY() ?
                        pmap_tmp.stride() :
                        -pmap_tmp.stride(), pmap_tmp.bpp());
                }

                convert_pmap(rbuf_tmp, src, m_format);
                pmap_tmp.Draw(displayGraphics);

                throw new System.NotImplementedException();
            }
        }

        public bool load_pmap(string fn, uint idx, RasterBuffer dst)
        {
            PixelMap pmap_tmp = new PixelMap();
            if (!pmap_tmp.load_from_bmp(fn)) return false;

            RasterBuffer rbuf_tmp = new RasterBuffer();
            unsafe
            {
                rbuf_tmp.Attach(pmap_tmp.buf(),
                                pmap_tmp.Width(),
                                pmap_tmp.Height(),
                                m_app.FlipY() ?
                                  pmap_tmp.stride() :
                                 -pmap_tmp.stride(),
                                 pmap_tmp.bpp());

                m_pmap_img[idx] = new PixelMap();
                m_pmap_img[idx].Create(pmap_tmp.Width(), pmap_tmp.Height(),
                    PlatformSupportAbstract.GetBitDepthForPixelFormat(m_format),
                    0);

                dst.Attach(m_pmap_img[idx].buf(),
                    m_pmap_img[idx].Width(),
                    m_pmap_img[idx].Height(),
                    m_app.FlipY() ?
                    m_pmap_img[idx].stride() :
                    -m_pmap_img[idx].stride(), m_pmap_img[idx].bpp());

                switch (m_format)
                {
                   
                    case PlatformSupportAbstract.PixelFormats.Bgr24:
                        switch (pmap_tmp.bpp())
                        {
                            case 24:
                                unsafe
                                {
                                    for (uint y = 0; y < rbuf_tmp.Height(); y++)
                                    {
                                        byte* sourceBuffer = rbuf_tmp.GetPixelPointer((int)rbuf_tmp.Height() - (int)y - 1);
                                        byte* destBuffer = dst.GetPixelPointer((int)y);
                                        for (uint x = 0; x < rbuf_tmp.Width(); x++)
                                        {
                                            *destBuffer++ = sourceBuffer[0];
                                            *destBuffer++ = sourceBuffer[1];
                                            *destBuffer++ = sourceBuffer[2];
                                            sourceBuffer += 3;
                                        }
                                    }
                                }
                                break;

                            default:
                                throw new System.NotImplementedException();
                        }
                        break;

                    case PlatformSupportAbstract.PixelFormats.Rgb24:
                        switch (pmap_tmp.bpp())
                        {
                            //                 case 16: color_conv(dst, &rbuf_tmp, color_conv_rgb555_to_rgb24()); break;
                            case 24:
                                //color_conv(dst, &rbuf_tmp, color_conv_bgr24_to_rgba32()); 
                                unsafe
                                {
                                    for (uint y = 0; y < rbuf_tmp.Height(); y++)
                                    {
                                        byte* sourceBuffer = rbuf_tmp.GetPixelPointer((int)rbuf_tmp.Height() - (int)y - 1);
                                        byte* destBuffer = dst.GetPixelPointer((int)y);
                                        for (uint x = 0; x < rbuf_tmp.Width(); x++)
                                        {
                                            *destBuffer++ = sourceBuffer[2];
                                            *destBuffer++ = sourceBuffer[1];
                                            *destBuffer++ = sourceBuffer[0];
                                            sourceBuffer += 3;
                                        }
                                    }
                                }
                                break;
                            //                 case 32: color_conv(dst, &rbuf_tmp, color_conv_bgra32_to_rgb24()); break;
                            default:
                                throw new System.NotImplementedException();
                        }
                        break;
                    

                    case PlatformSupportAbstract.PixelFormats.Bgra32:
                        switch (pmap_tmp.bpp())
                        {
                            case 24:
                                unsafe
                                {
                                    for (uint y = 0; y < rbuf_tmp.Height(); y++)
                                    {
                                        byte* sourceBuffer = rbuf_tmp.GetPixelPointer((int)rbuf_tmp.Height() - (int)y - 1);
                                        byte* destBuffer = dst.GetPixelPointer((int)y);
                                        for (uint x = 0; x < rbuf_tmp.Width(); x++)
                                        {
                                            *destBuffer++ = sourceBuffer[0];
                                            *destBuffer++ = sourceBuffer[1];
                                            *destBuffer++ = sourceBuffer[2];
                                            *destBuffer++ = 255;
                                            sourceBuffer += 3;
                                        }
                                    }
                                }
                                break;
                        }
                        break;

                    case PlatformSupportAbstract.PixelFormats.Rgba32:
                        switch (pmap_tmp.bpp())
                        {
                            //                 case 16: color_conv(dst, &rbuf_tmp, color_conv_rgb555_to_rgba32()); break;
                            case 24:
                                //color_conv(dst, &rbuf_tmp, color_conv_bgr24_to_rgba32()); 
                                unsafe
                                {
                                    for (uint y = 0; y < rbuf_tmp.Height(); y++)
                                    {
                                        byte* sourceBuffer = rbuf_tmp.GetPixelPointer((int)rbuf_tmp.Height() - (int)y - 1);
                                        byte* destBuffer = dst.GetPixelPointer((int)y);
                                        for (uint x = 0; x < rbuf_tmp.Width(); x++)
                                        {
                                            *destBuffer++ = sourceBuffer[2];
                                            *destBuffer++ = sourceBuffer[1];
                                            *destBuffer++ = sourceBuffer[0];
                                            *destBuffer++ = 255;
                                            sourceBuffer += 3;
                                        }
                                    }
                                }
                                break;


                            //                 case 32: color_conv(dst, &rbuf_tmp, color_conv_bgra32_to_rgba32()); break;
                            default:
                                throw new System.NotImplementedException();
                        }
                        break;

                    

                    default:
                        throw new System.NotImplementedException();

                }
            }

            return true;
        }

        public bool save_pmap(string fn, uint idx, RasterBuffer src)
        {
            
            return true;
        }


        public uint translate(uint keycode)
        {
            return m_last_translated_key = (keycode > 255) ? 0 : m_keymap[keycode];
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            if (this.Visible)
            {
                if (m_app.RenderBufferWindow != null)
                {
                    create_pmap((uint)ClientSize.Width, (uint)ClientSize.Height, m_app.RenderBufferWindow);
                }

                m_app.TransAffineResizing(ClientSize.Width, ClientSize.Height);
                m_app.OnResize(ClientSize.Width, ClientSize.Height);
                m_app.ForceRedraw();
            }
        }

        protected override void OnClosed(EventArgs e)
        {
            // TODO: call on closing and check if we can close ("do you want to save might cancel." :).
            //OnClosing();
            m_app.OnClosed();

            base.OnClosed(e);
        }

        internal void ForceRedraw()
        {
            m_WindowContentNeedsRedraw = true;
            Invalidate();
        }
    };

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
