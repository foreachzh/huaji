using System;
using System.Drawing;
using System.Collections.Generic;
using System.Threading;

namespace huaji
{
    /// <summary>
    /// 此次训练目的：练习面向对象的思想做绘图、
    /// </summary>
    public class ball
    {
        /// <summary>
        /// 球的横左边
        /// </summary>
        internal double posX; 
        /// <summary>
        /// 球的纵坐标
        /// </summary>
        internal double posY; 
        internal List<Image> img; //球的贴图
        /// <summary>
        /// 球的半径
        /// </summary>
        internal double Radius; 
        internal double nowimg; //球的当前贴图代数
        public ball(double x, double y, List<Image> img, double radius, double nowimg)
        {
            this.posX = x;
            this.posY = y;
            this.img = img;
            this.Radius = radius;
            this.nowimg = nowimg;
        }
    }

    public class npcball : ball
    {
        /// <summary>
        /// 球的速度,每1毫秒的移动的距离
        /// </summary>
        internal double Speed; 
        /// <summary>
        /// 球的运动角度，从其坐标顺时针计算
        /// </summary>
        internal double Angle; 
        internal double av; //球的衰减加速度百分比
        internal double gv; //向下加速度
        internal double mg; //重力加速度
        /// <summary>
        /// 弹力
        /// </summary>
        internal double ef; 
        internal double gva; //重力加速度计数器
        internal static double fn = 1000000; //空气阻力常数，越大阻力越小
        internal static double kn = 1; //弹力常数
        internal static double G = 300; //重力常数，越小重力越大
        internal int r_l; //球的左右运动方向,左为-1,右为1，垂直纵向为0
        internal int d_u; //球的上下运动方向，上为-1，下为1,垂直横向为0
        internal static int room_width = Form2.width; //屏幕长
        internal static int room_height = Form2.height; //屏幕宽
        internal static int k = 10; //运动刷新率
        internal double g; //质量
        public npcball(int x, int y, List<Image> img, double radius, double Speedx, double angle, int r_l, int d_u)
            : base(x, y, img, radius, new Random(1).NextDouble())
        {
            Console.WriteLine("width=" + room_width + ";height=" + room_height);
            this.Speed = Speedx;
            this.Angle = angle;
            this.av = 1 - (radius / fn);
            this.r_l = r_l;
            this.d_u = d_u;
            gva = 1;
            g = (radius / G) * (radius / G);
            // (new Thread(this)).Start();
            Thread thread = new Thread(DrawOwnThread);
            thread.IsBackground = true;
            thread.Start();
        }

        public virtual void mmotioning() //k为刷新频率，为毫秒值
        {
            gv = (Speed * k / Math.Sin(Math.PI / 180 * 90) * Math.Sin(Math.PI / 180 * (Angle)));
            ef = ef < (g / 2) ? 0 : ef;
            mg = ((g > gv && posY > room_height - Radius / 2 - 35) ? gv : g) * gva;
            gva = mg >= g ? posY > room_height - Radius / 2 - 35 ? 1 : gva + 0.05 : 1;
            posY = posY + (d_u * (gv + ((d_u == 1 || ef != 0) ? mg : -mg)) - ef);
            posX = posX + (r_l * (Speed * k / Math.Sin(Math.PI / 180 * 90) * Math.Sin(Math.PI / 180 * (90 - Angle))));
            Speed *= av;
            r_l = posX < Radius / 2 ? 1 : posX > room_width - Radius / 2 - 15 ? -1 : r_l;
            d_u = posY < Radius / 2 ? 1 : posY > room_height - Radius / 2 - 35 ? -1 : d_u;
            ef = posY > room_height - Radius / 2 - 35 ? gv * kn : 0;

            nowimg = nowimg >= img.Count - 0.8 ? 0 : nowimg + (new Random(1).NextDouble() / 100);
        }

        public void DrawOwnThread()
        {
            try
            {
                while (true)
                {
                    this.mmotioning();
                    Thread.Sleep(k);
                    if (Speed == 0)
                    {
                        return;
                    }
                    Thread.Sleep(10);
                }
            }
            catch (Exception e)
            {
                // TODO 自动生成的 catch 块
                Console.WriteLine(e.ToString());
                Console.Write(e.StackTrace);
            }
            // TODO 自动生成的方法存根

        }
    }
}
