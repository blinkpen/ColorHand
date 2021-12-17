using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace ColorHand
{
    public partial class colorLense : UserControl
    {
        public bool GRIDTOGGLE = true;
        private Color MARK = Color.OrangeRed;
        private Color GRID = Color.Black;
        public Bitmap bmpNew = null;
        public Color selectedColor;
        public string hex;
        public int R;
        public int G;
        public int B;
        public int A;
        public Timer clock;
        private bool captureHover = true;


        public colorLense()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            clock = timer1;
        }

        private struct MSLLHOOKSTRUCT
        {
            public Point pt;
            public Int32 mouseData;
            public Int32 flags;
            public Int32 time;
            public IntPtr extra;
        }

        private const long GWL_WNDPROC = (-4);
        private IntPtr mHook = IntPtr.Zero;
        private const Int32 WH_MOUSE_LL = 0xE;
        private const Int32 WM_RBUTTONDOWN = 0x204;
        private const Int32 WM_LBUTTONDOWN = 0x201;
        private const Int32 WM_MOUSEHOVER = 0x2A1;
        private const long TME_CANCEL = 0x80000000;
        private const long TME_HOVER = 0x1;
        private const long TME_LEAVE = 0x2;
        private const long TME_NONCLIENT = 0x10;
        private const long TME_QUERY = 0x40000000;
        private const long WM_MOUSELEAVE = 0x2A3;
        private const long WM_MOUSEMOVE = 0x200;
        [MarshalAs(UnmanagedType.FunctionPtr)]
        private MouseHookDelegate mProc;
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern IntPtr SetWindowsHookExW(Int32 idHook, MouseHookDelegate HookProc, IntPtr hInstance, Int32 wParam);
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern bool UnhookWindowsHookEx(IntPtr hook);
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern Int32 CallNextHookEx(Int32 idHook, Int32 nCode, IntPtr wParam, ref MSLLHOOKSTRUCT lParam);
        [System.Runtime.InteropServices.DllImport("kernel32.dll")]
        private static extern IntPtr GetModuleHandleW(IntPtr fakezero);
        private delegate Int32 MouseHookDelegate(Int32 nCode, IntPtr wParam, ref MSLLHOOKSTRUCT lParam);

        public bool SetHookMouse()
        {
            if (mHook == IntPtr.Zero)
            {
                mProc = new MouseHookDelegate(MouseHookProc);
                mHook = SetWindowsHookExW(WH_MOUSE_LL, mProc, GetModuleHandleW(IntPtr.Zero), 0);
            }
            return mHook != IntPtr.Zero;
        }

        public void UnHookMouse()
        {
            if (mHook == IntPtr.Zero)
                return;
            UnhookWindowsHookEx(mHook);
            mHook = IntPtr.Zero;
        }

        private Int32 MouseHookProc(Int32 nCode, IntPtr wParam, ref MSLLHOOKSTRUCT lParam)
        {            
            if (wParam.ToInt32() == WM_LBUTTONDOWN)
            {
                UnHookMouse();
                timer1.Stop();                
                return 1;
            }

            if (wParam.ToInt32() == WM_MOUSEHOVER)
            {
                return 1;                
            }
            
            return CallNextHookEx(WH_MOUSE_LL, nCode, wParam, ref lParam);
        }

        private void colorLense_Paint(object sender, PaintEventArgs e)
        {
            if (GRIDTOGGLE == true)
            {
                this.BorderStyle = BorderStyle.None;
                Graphics g = e.Graphics;
                Pen pn = new Pen(GRID); // ~~~ color of the lines
                int x;
                int y;
                int intSpacing = 8; // ~~~ spacing between adjacent lines

                // ~~~ Draw the horizontal lines
                x = this.Width;
                for (y = 0; y <= this.Height; y += intSpacing)
                    g.DrawLine(pn, new Point(0, y), new Point(x, y));

                // ~~~ Draw the vertical lines
                y = this.Height;
                for (x = 0; x <= this.Width; x += intSpacing)
                    g.DrawLine(pn, new Point(x, 0), new Point(x, y));


                Graphics gr = e.Graphics;
                Pen pen = new Pen(MARK);     // ~~~ color Of the lines

                int intSpacing2 = 8; // ~~~ spacing between adjacent lines
                                     // ~~~ Draw the horizontal lines
                x = 128;
                for (y = 128; y <= 136; y += intSpacing2)
                    gr.DrawLine(pen, new Point(136, y), new Point(x, y));

                // ~~~ Draw the vertical lines
                y = 128;
                for (x = 128; x <= 136; x += intSpacing2)
                    gr.DrawLine(pen, new Point(x, 136), new Point(x, y));
        
            }
            else
            {
                //this.BorderStyle = BorderStyle.FixedSingle;

                //Graphics g = e.Graphics;
                //Pen pn = new Pen(GRID); // ~~~ color of the lines

                //int x;
                //int y;

                //int intSpacing = 8; // ~~~ spacing between adjacent lines

                //Graphics gr = e.Graphics;
                //Pen pen = new Pen(MARK);     // ~~~ color Of the lines

                //int intSpacing2 = 8; // ~~~ spacing between adjacent lines
                //                     // ~~~ Draw the horizontal lines
                //x = 126;
                //for (y = 126; y <= 132; y += intSpacing2)
                //    gr.DrawLine(pen, new Point(132, y), new Point(x, y));

                //// ~~~ Draw the vertical lines
                //y = 126;
                //for (x = 126; x <= 132; x += intSpacing2)
                //    gr.DrawLine(pen, new Point(x, 132), new Point(x, y));
            }
        }

        private void colorUpdater()
        {
            hex = ColorTranslator.ToHtml(selectedColor);
            R = selectedColor.R;
            G = selectedColor.G;
            B = selectedColor.B;
            A = selectedColor.A;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {           
            SetHookMouse();
            using (Bitmap pic = new Bitmap(this.Width, this.Height))
            {
                bmpNew = new Bitmap(pic, 257, 257);
                Graphics gfx = Graphics.FromImage(pic);
                gfx.CopyFromScreen(Cursor.Position - this.Size / (2 * 8), Point.Empty, pic.Size);
                using (Graphics g = Graphics.FromImage(bmpNew))
                {
                    g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
                    g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;
                    g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.Half;                    
                    g.DrawImage(pic, 0, 0, 257 * 8, 257 * 8);
                    this.BackgroundImage = bmpNew;
                }
            }
            selectedColor = bmpNew.GetPixel(bmpNew.Width / 2, bmpNew.Height / 2);
            colorUpdater();     

        }
    }
}
