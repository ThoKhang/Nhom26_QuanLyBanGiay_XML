using System;
using System.Windows.Forms;

namespace QuanLyBanGiay
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();

            // Cách 1: dùng fully qualified name
            Application.Run(new GUI.DangNhap());

            // N?u b?n mu?n g?n h?n thì:
            // using QuanLyBanGiay.GUI; ? trên cùng
            // r?i vi?t: Application.Run(new DangNhap());
        }
    }
}
