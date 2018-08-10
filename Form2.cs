using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Threading;

namespace huaji
{
    public partial class Form2 : Form
    {
        public static int width = 1920;
        public static int height = 1080;
        private List<ball> ballList = new List<ball>();
        [DllImport("user32.dll")]
        public static extern UInt32 RegisterHotKey(IntPtr hWnd, UInt32 id, UInt32 fsModifiers, UInt32 vk); //API
        //重写消息循环
        protected override void WndProc(ref Message m)
        {
            const int WM_HOTKEY = 0x0312;

            if (m.Msg == WM_HOTKEY && m.WParam.ToInt32() == 247696411) //判断热键
            {
                Application.Exit();
            }

            base.WndProc(ref m);
        }
        public Form2()
        {
            InitializeComponent();
            Rectangle rect = Screen.GetWorkingArea(this);
            width = ClientSize.Width;// rect.Width;
            height = ClientSize.Height;// rect.Height;

            TransparencyKey = BackColor;//背景透明(鼠标穿透)
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);   //   禁止擦除背景.
            SetStyle(ControlStyles.DoubleBuffer, true);   //   双缓冲

            RegisterHotKey(this.Handle, 247696411, 0, (UInt32)Keys.Escape); //注册热键
        }

        private void Form2_Paint(object sender, PaintEventArgs e)
        {
            // Bitmap bmp = new Bitmap(ClientSize.Width, ClientSize.Height, e.Graphics);
            // Graphics g = e.Graphics;// Graphics.FromImage(bmp);
            BufferedGraphicsContext currentContext = BufferedGraphicsManager.Current;
            BufferedGraphics myBuffer = currentContext.Allocate(e.Graphics, e.ClipRectangle);
            Graphics g = myBuffer.Graphics;

            g.Clear(this.BackColor);
            //Rectangle r = new Rectangle(10, 10, 300, 200);//是创建画矩形的区域  
            //g.DrawRectangle(Pens.Red, r);//g对象提供了画图形的方法，我们只需调用即可

            int x = 0, y = 0, xr = 0, yr = 0;
            try
            {
                // base.paint(g);
                for (int i = 0; i < ballList.Count; i++)
                {
                    ball myball = ballList[i];
                    x = (int)(myball.posX - (myball.Radius / 2));
                    y = (int)(myball.posY - (myball.Radius / 2));
                    xr = (int)myball.Radius;
                    yr = (int)myball.Radius;
                    Rectangle destRect = new Rectangle(x, y, xr, yr);
                    g.DrawImage(myball.img[(int)myball.nowimg], destRect/*,x, y, xr, yr*/);
                }
                myBuffer.Render(e.Graphics);  //呈现图像至关联的Graphics
                myBuffer.Dispose();

            }
            catch (Exception ex)
            {
                // TODO 自动生成的 catch 块
                Console.WriteLine(ex.ToString());
                Console.Write(ex.StackTrace);
            }

            g.Dispose();
            // bmp.Dispose();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            Thread thread = new Thread(CreateBallThread);
            thread.IsBackground = true;
            thread.Start();
        }

        public void CreateBallThread()
        {
            Thread.Sleep(1000);
            string basedir = System.AppDomain.CurrentDomain.BaseDirectory;
            List<Image> ballimg = new List<Image>();
            ballimg.Add(Program.ReadImageFile(basedir + "image\\huaji2.png"));
            ballimg.Add(Program.ReadImageFile(basedir + "image\\huaji4.png"));
            ballimg.Add(Program.ReadImageFile(basedir + "image\\huaji3.png"));

            for (int i = 0; i < 1000 && i < 6; i++)
            {
                var r = new Random(Guid.NewGuid().GetHashCode());
                int ballsize = (int)(r.NextDouble() * (ClientSize.Height/3 - 50) + 50);

                var r2 = new Random(Guid.NewGuid().GetHashCode());
                var r3 = new Random(Guid.NewGuid().GetHashCode());
                var r4 = new Random(Guid.NewGuid().GetHashCode());
                var r5 = new Random(Guid.NewGuid().GetHashCode());
                var r6 = new Random(Guid.NewGuid().GetHashCode());
                var r7 = new Random(Guid.NewGuid().GetHashCode());
                npcball myball = new npcball(
                    (int)(r2.NextDouble() * (Form2.width - ballsize / 2 - 15 - ballsize / 2) + ballsize / 2),
                    (int)(r3.NextDouble() * (Form2.height - ballsize / 2 - 35 - ballsize / 2) + ballsize / 2),
                    ballimg, ballsize, 
                    (int)(r4.NextDouble() * (2 - 1) + 1),
                    r5.NextDouble() * (90), (r6.NextDouble() * 1000) % 2 == 0 ? 1 : -1,
                    (r7.NextDouble() * 1000) % 2 == 0 ? 1 : -1);

                ballList.Add(myball);
                try
                {
                    Thread.Sleep(1000);
                    // Threading..Sleep(1000);
                }
                catch (Exception e)
                {
                    // TODO 自动生成的 catch 块
                    Console.WriteLine(e.ToString());
                    Console.Write(e.StackTrace);
                }
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            this.Refresh();
        }
    }
}
