using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

public class ScreenLib
{

    [DllImport("User32.dll")]
    static extern IntPtr GetDC(IntPtr hwnd);

    [DllImport("User32.dll")]
    static extern void ReleaseDC(IntPtr dc);

    /// <summary>
    /// 抓圖
    /// </summary>
    /// <param name="point">位置</param>
    /// <param name="size">大小</param>
    /// <returns></returns>
    static public Bitmap GetScreen(Point point, Size size)
    {
        Bitmap bitmap = new Bitmap(size.Width, size.Height);
        Graphics graphics = Graphics.FromImage(bitmap);
        graphics.CopyFromScreen(point, new Point(0, 0), size);
        return bitmap;
    }
    /// <summary>
    /// 掃描是否有符合的顏色
    /// </summary>
    /// <param name="bitmap">BMP</param>
    /// <param name="color">CLR</param>
    /// <returns>BOOLEAN</returns>
    static public bool Scan(Bitmap bitmap, Color color)
    {
        for (int i = 0; i < bitmap.Width; i++ )
        {
            for (int j = 0; j < bitmap.Height; j++)
            {
                if (bitmap.GetPixel(i, j).ToArgb() == color.ToArgb())
                    return true;
            }
        }

        return false;
    }

    /// <summary>
    /// 抓畫面顏色
    /// </summary>
    /// <param name="pos"></param>
    /// <returns></returns>
    static public Color GetScreenColor(Point pos)
    {
        Bitmap bitmap = new Bitmap(1, 1);
        Graphics graphics = Graphics.FromImage(bitmap);
        graphics.CopyFromScreen(pos, new Point(0, 0), new Size(1, 1));
        return bitmap.GetPixel(0, 0);
    }
}
