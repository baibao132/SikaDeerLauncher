using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Text;

namespace windows
{
    public enum ShowEnum
    {
        SW_Close = 0,
        SW_NORMAL = 1,
        SW_MINIMIZE = 2,
        SW_MAXIMIZE = 3,
        SW_SHOWNOACTIVATE = 4,
        SW_SHOW = 5,
        SW_RESTORE = 9,//还原
        SW_SHOWDEFAULT = 10
    }

    public class WinAPI
    {
        #region WinodwsAPI

        [DllImport("user32.dll")]
        public static extern bool EnumChildWindows(IntPtr hWndParent, ChildWindowsProc lpEnumFunc, int lParam);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int GetClassName(IntPtr hWnd, StringBuilder lpClassName, int nMaxCount);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int GetClientRect(IntPtr hwnd, ref Rectangle rc);

        [DllImport("user32.dll", EntryPoint = "GetCursorPos")]
        public static extern bool GetCursorPos(out Point pt);

        [DllImport("user32.dll", EntryPoint = "GetParent")]
        public static extern IntPtr GetParent(IntPtr hWnd);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int GetWindowText(IntPtr hWnd, [Out, MarshalAs(UnmanagedType.LPTStr)] StringBuilder lpString, int nMaxCount);

        /// <summary>
        /// 取进程ID
        /// </summary>
        /// <param name="hWndParent">窗口句柄
        /// <param name="intPtr"> 进程id
        /// <returns>拥有窗口的线程的标识符</returns>
        [DllImport("user32.dll")]
        public static extern int GetWindowThreadProcessId(IntPtr hWndParent, ref IntPtr lpdwProcessId);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern bool MoveWindow(IntPtr hwnd, int x, int y, int nWidth, int nHeight, bool bRepaint);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true, ExactSpelling = true)]
        public static extern int ScreenToClient(IntPtr hWnd, ref Rectangle rect);

        [DllImport("user32.dll ", EntryPoint = "SendMessage")]
        public static extern int SendMessage(IntPtr hwnd, uint wMsg, int wParam, int lParam);

        [DllImport("user32.dll ", EntryPoint = "SendMessageA")]
        public static extern int SendMessageA(IntPtr hwnd, uint wMsg, int wParam, string lParam);

        /// <summary>
        ///  该函数将创建指定窗口的线程设置到前台，并且激活该窗口。键盘输入转向该窗口，并为用户改各种可视的记号。
        ///  系统给创建前台窗口的线程分配的权限稍高于其他线程。
        /// </summary>
        /// <param name="hWnd">将被激活并被调入前台的窗口句柄
        /// <returns>如果窗口设入了前台，返回值为非零；如果窗口未被设入前台，返回值为零</returns>
        [DllImport("User32.dll")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

        /// <summary>
        /// 窗口置顶 或设置大小
        /// </summary>
        /// <param name="hWnd">
        /// <param name="hWndInsertAfter">
        /// <param name="X">
        /// <param name="Y">
        /// <param name="cx">
        /// <param name="cy">
        /// <param name="uFlags">
        /// <returns></returns>
        [DllImport("user32.dll")]
        public static extern int SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int X, int Y, int cx, int cy, int uFlags);

        ///<summary>
        /// 该函数设置由不同线程产生的窗口的显示状态
        /// </summary>
        /// <param name="hWnd">窗口句柄
        /// <param name="cmdShow">s
        /// <returns>如果函数原来可见，返回值为非零；如果函数原来被隐藏，返回值为零</returns>
        [DllImport("User32.dll")]
        public static extern bool ShowWindowAsync(IntPtr hWnd, ShowEnum cmdShow);

        // ==============================
        /// <summary>
        /// 激活窗口
        /// </summary>
        /// <param name="hWnd">
        /// <param name="fAltTab">
        [DllImport("user32.dll")]
        public static extern void SwitchToThisWindow(IntPtr hWnd, bool fAltTab);

        [DllImport("user32.dll", EntryPoint = "WindowFromPoint", CharSet = CharSet.Auto, ExactSpelling = true)]
        public static extern IntPtr WindowFromPoint(Point pt);

        [DllImport("user32.dll")]
        static extern bool EnumWindows(WNDENUMPROC lpEnumFunc, int lParam);

        [DllImport("user32.dll", EntryPoint = "FindWindow")]
        static extern IntPtr FindWindow(string IpClassName, string IpWindowName);

        [DllImport("user32.dll", EntryPoint = "FindWindowEx")]
        static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass, string lpszWindow);

        [DllImport("user32.dll")]
        static extern int GetClassNameW(IntPtr hWnd, [MarshalAs(UnmanagedType.LPWStr)] StringBuilder lpString, int nMaxCount);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        static extern int GetWindowRect(IntPtr hwnd, ref Rectangle rc);

        [DllImport("user32.dll")]
        static extern int GetWindowTextW(IntPtr hWnd, [MarshalAs(UnmanagedType.LPWStr)] StringBuilder lpString, int nMaxCount);
        #endregion WinodwsAPI

        #region 封装API方法

        public delegate bool ChildWindowsProc(IntPtr hwnd, int lParam);

        private delegate bool WNDENUMPROC(IntPtr hWnd, int lParam);

        /// <summary>
        /// 查找子窗口句柄
        /// </summary>
        /// <param name="hwndParent">父窗口句柄
        /// <param name="hwndChildAfter">前一个同目录级同名窗口句柄
        /// <param name="lpszClass">类名
        /// <returns></returns>
        public static IntPtr FindWindowE(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass)
        {
            return FindWindowEx(hwndParent, hwndChildAfter, lpszClass, null);
        }

        /// <summary>
        /// 查找全部子窗口句柄
        /// </summary>
        /// <param name="hwndParent">父窗口句柄
        /// <param name="className">类名
        /// <returns></returns>
        public static List<IntPtr> FindWindowExList(IntPtr hwndParent, string className)
        {
            List<IntPtr> resultList = new List<IntPtr>();

            for (IntPtr hwndClient = FindWindowE(hwndParent, IntPtr.Zero, className); hwndClient != IntPtr.Zero; hwndClient = FindWindowE(hwndParent, hwndClient, className))
                resultList.Add(hwndClient);
            

            return resultList;
        }

        /// <summary>
        /// 得到窗口上一级窗口的句柄
        /// </summary>
        /// <param name="ChildHwnd">子窗口句柄
        /// <returns> 返回 窗口句柄 找不到返回 0</returns>
        public static int GetChild_Host(int ChildHwnd) => (int)GetParent((IntPtr)ChildHwnd);

        ///
        /// 得到指定窗口类名
        /// <summary>
        /// <param name="hWnd">句柄
        /// <returns>找不到返回""</returns>
        public static string GetClassName(int hWnd) => GetClassName((IntPtr)hWnd);

        /// </summary>
        /// 得到指定窗口类名
        /// <summary>
        /// <param name="hWnd">句柄
        /// <returns>找不到返回""</returns>
        public static string GetClassName(IntPtr hWnd)
        {
            var lpClassName = new StringBuilder(128);

            if (GetClassName(hWnd, lpClassName, lpClassName.Capacity) == 0)
            {
                return "";
            }

            return lpClassName.ToString();
        }

        /// <summary>
        /// 窗口大小
        /// </summary>
        /// <param name="hWnd">
        /// <returns></returns>
        public static Rectangle GetClientRect(IntPtr hWnd)
        {
            Rectangle result = default(Rectangle);
            GetClientRect(hWnd, ref result);
            return result;
        }

        /// <summary>
        /// 获取鼠标位置的坐标
        /// </summary>
        /// <returns></returns>
        public static Point GetCursorPosPoint()
        {
            Point p = new Point();
            if (GetCursorPos(out p)) return p;
            return default(Point);
        }

        /// <summary>
        /// 获得顶层窗口
        /// </summary>
        /// <returns>返回 窗口句柄</returns>
        public static int GetForeGroundWindow() => (int)GetForegroundwindow();

        /// <summary>
        /// 查找句柄
        /// </summary>
        /// <param name="IpClassName">类名
        /// <returns></returns>
        public static IntPtr GetHandle(string IpClassName) => FindWindow(IpClassName, null);

        /// <summary>
        /// 找到句柄
        /// </summary>
        /// <param name="p">坐标
        /// <returns></returns>
        public static IntPtr GetHandle(Point p) => WindowFromPoint(p);

        /// <summary>
        /// 得到鼠标指向的窗口句柄
        /// </summary>
        /// <returns>找不到则返回-1</returns>
        public static int GetMoustPointWindwsHwnd()
        {
            try
            {
                return (int)GetHandle(GetCursorPosPoint());
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return -1;
        }

        /// <summary>
        /// 窗口在屏幕位置
        /// </summary>
        /// <param name="hWnd">句柄
        /// <returns></returns>
        public static Rectangle GetWindowRect(IntPtr hWnd)
        {
            Rectangle result = default(Rectangle);
            GetWindowRect(hWnd, ref result);
            result.Width = result.Width - result.X;
            result.Height = result.Height - result.Y;

            return result;
        }

        /// <summary>
        /// 得到窗口状态
        /// </summary>
        /// <param name="Hwnd">窗口句柄
        /// <param name="flag">
        /// 操作方式
        /// 1：判断窗口是否最小化
        /// 2：判断窗口是否最大化
        /// 3：判断窗口是否激活
        ///
        /// <returns>满足条件返回 true</returns>
        public static bool GetWindowState(int Hwnd, int flag)
        {
            switch (flag)
            {
                case 1:
                    return IsIconic((IntPtr)Hwnd);

                case 2:
                    return IsZoomed((IntPtr)Hwnd);

                case 3:
                    if (Hwnd != GetForeGroundWindow()) return false;
                    break;
            }

            return true;
        }

        /// <summary>
        /// 获得窗口内容或标题
        /// </summary>
        /// <param name="hWnd">句柄
        /// <returns></returns>
        public static string GetWindowText(IntPtr hWnd)
        {
            var result = new StringBuilder(128);
            GetWindowText(hWnd, result, result.Capacity);
            return result.ToString();
        }

        public static void InputStr(IntPtr myIntPtr, string Input)
        {
            byte[] ch = (ASCIIEncoding.ASCII.GetBytes(Input));

            for (int i = 0; i < ch.Length; i++)
                SendMessageA(myIntPtr, (uint)0X102, int.Parse(ch[i].ToString()), "0");
            
        }

        /// <summary>
        /// 判断窗口是否存在
        /// </summary>
        /// <param name="Hwnd">窗口句柄
        /// <returns>存在返回 true 不存在返回 false</returns>
        public static bool IsWindow(int Hwnd)
        {
            if (IsWindow((IntPtr)Hwnd)) return true;
            return false;
        }

        /// <summary>
        /// 鼠标点击
        /// </summary>
        /// <param name="hWnd">句柄
        /// <param name="lParam">要发送的内容
        public static void MouseClick(IntPtr hWnd, int _x, int _y)
        {
            SendMessage(hWnd, WindowsMessage.WM_LBUTTONUP, 0, 0);
        }

        /// <summary>
        /// 窗口相对屏幕位置转换成父窗口位置
        /// </summary>
        /// <param name="hWnd">
        /// <param name="rect">
        /// <returns></returns>
        public static Rectangle ScreenToClient(IntPtr hWnd, Rectangle rect)
        {
            Rectangle result = rect;
            ScreenToClient(hWnd, ref result);
            return result;
        }

        /// <summary>
        /// 给窗口发送文本内容
        /// </summary>
        /// <param name="hWnd">句柄
        /// <param name="lParam">要发送的内容
        public static void SendMessageA(IntPtr hWnd, string lParam)
        {
            SendMessageA(hWnd, WindowsMessage.WM_SETTEXT, 0, lParam);
        }

        /// <summary>
        /// 激活窗口 并显示到最前面
        /// </summary>
        /// <param name="hwnd">
        public static void SetWindosActiv(IntPtr hwnd)
        {
            ShowWindowAsync(hwnd, ShowEnum.SW_NORMAL);//显示
            SetForegroundWindow(hwnd);//当到最前端
        }

        /// <summary>
        /// 窗口置顶
        /// </summary>
        /// <param name="hWnd">窗口句柄
        /// <param name="is_activ">是否置顶 为false 取消置顶
        /// <returns></returns>
        public static int SetWindowsTop(IntPtr hWnd, bool is_activ)
        {
            int is_top = -1;
            if (!is_activ) is_top = -2;

            return SetWindowPos(hWnd, is_top, 0, 0, 0, 0, 1 | 2);
        }

        // 修改指定窗口标题
        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        public static extern int SetWindowText(IntPtr hWnd, string text);

        /// <summary>
        /// 设置窗口标题
        /// </summary>
        /// <param name="Hwnd">窗口句柄
        /// <param name="newtext">新标题
        public static void SetWindowText(int Hwnd, string newtext)
        {
            SetWindowText((IntPtr)Hwnd, newtext);
        }

        /// <summary>
        /// 根据标题查找窗体句柄
        /// </summary>
        /// <param name="title">标题内容
        /// <returns></returns>
        public IntPtr FindWindow(string title)
        {
            Process[] ps = Process.GetProcesses();

            foreach (Process p in ps)
            {
                if (p.MainWindowTitle.IndexOf(title) != -1)
                {
                    return p.MainWindowHandle;
                }
            }

            return IntPtr.Zero;
        }

        /// <summary>
        /// 枚举窗口 返回窗口句柄 数组
        /// </summary>
        /// <param name="phwnd">
        /// <returns></returns>
        public IntPtr[] GetAllChildControlsHandle(IntPtr phwnd)
        {
            List<IntPtr> child = new List<IntPtr>();

            EnumChildWindows(phwnd, delegate (IntPtr hWnd, int lParam)
            {
                child.Add(hWnd);
                return true;
            }, 0);

            return child.ToArray();
        }

        // 获得顶层窗口
        [DllImport("user32", EntryPoint = "GetForegroundWindow")]
        static extern IntPtr GetForegroundwindow();

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool IsIconic(IntPtr hWnd);

        ///=====================================================================
        // 判断窗口是否存在
        [DllImport("user32", EntryPoint = "IsWindow")]
        static extern bool IsWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        static extern bool IsZoomed(IntPtr hWnd);

        #endregion 封装API方法

        /// <summary>
        /// 得到指定坐标的窗口句柄
        /// </summary>
        /// <param name="x">X坐标
        /// <param name="y">Y坐标
        /// <returns>找不到返回 0</returns>
        public static int FindPointWindow(int x, int y)
        {
            Point p = new Point(x, y);
            IntPtr formHandle = WindowFromPoint(p);//得到窗口句柄
            return (int)formHandle;
        }

        /// </summary>
        /// 根据标题和类名找句柄
        /// <summary>
        /// <param name="IpClassName">窗口类名 如果为"" 则只根据标题查找
        /// <param name="IpClassName">窗口标题 如果为"" 则只根据类名查找
        /// <returns>找不到则返回0</returns>
        public static int FindWindowHwnd(string IpClassName, string IpTitleName)
        {
            if (IpTitleName == "" && IpClassName != "")
            {
                return (int)FindWindow(IpClassName, null);
            }
            else if (IpClassName == "" && IpTitleName != "")
            {
                return (int)FindWindow(null, IpTitleName);
            }
            else if (IpClassName != "" && IpTitleName != "")
            {
                return (int)FindWindow(IpClassName, IpTitleName);
            }

            return 0;
        }

        /// <summary>
        /// 塞选方法 - 返回满足条件的句柄集合
        /// </summary>
        /// <param name="textName">窗口标题 - 为空不匹配标题( 模糊匹配，不区分大小写)
        /// <param name="textClass">窗口类名 - 为空不匹配类名( 模糊匹配，不区分大小写)
        /// <returns></returns>
        public List<int> EnumWindow(string textName, string textClass)
        {
            var Compare = CultureInfo.InvariantCulture.CompareInfo;
            List<int> gethwnd = new List<int>();
            // 先取得所有句柄
            List<WindowInfo> Listwindows = GetAllDesktopWindows();
            //
            foreach (WindowInfo wdf in Listwindows)
            {
                if (textName != "" && textClass != "")
                {
                    if (Compare.IndexOf(wdf.szClassName.ToUpper(), textClass.ToUpper()) > -1 && Compare.IndexOf(wdf.szWindowName.ToUpper(), textName.ToUpper()) > -1)
                    {
                        gethwnd.Add((int)wdf.hWnd);
                    }
                }
                else if (textName == "" && textClass == "")
                {
                    gethwnd.Add((int)wdf.hWnd);
                    // 都返回
                }
                else if (textName == "")
                { //只匹配类名
                    if (Compare.IndexOf(wdf.szClassName.ToUpper(), textClass.ToUpper()) > -1)
                    {
                        gethwnd.Add((int)wdf.hWnd);
                    }
                }
                else if (textClass == "")
                { //只匹配标题
                    if (Compare.IndexOf(wdf.szWindowName.ToUpper(), textName.ToUpper()) > -1)
                    {
                        gethwnd.Add((int)wdf.hWnd);
                    }
                }
            }

            return gethwnd;
        }

        /// <summary>
        /// 根据窗口标题模糊查找窗口句柄
        /// </summary>
        /// <param name="title">窗口标题
        /// <returns>返回 窗口句柄 找不到返回 0</returns>
        public int FindWindow_ByTitle(string title)
        { //按照窗口标题来寻找窗口句柄
            Process[] ps = Process.GetProcesses();
            var WindowHwnd = "";

            foreach (Process p in ps)
            {
                if (p.MainWindowTitle.IndexOf(title) != -1)
                {
                    WindowHwnd = p.MainWindowHandle.ToString();
                    return int.Parse(WindowHwnd);
                }
            }

            return 0;
        }

        /// <summary>
        /// 根据窗口标题模糊查找符合条件的所有窗口句柄
        /// </summary>
        /// <param name="title">窗口标题关键字
        /// <returns>返回 窗口句柄 多个句柄以"|" 隔开，找不到返回""</returns>
        public string FindWindowEx_ByTitle(string title)
        { //按照窗口标题来寻找窗口句柄
            Process[] ps = Process.GetProcesses();
            var WindowHwnd = "";

            foreach (Process p in ps)
            {
                if (p.MainWindowTitle.IndexOf(title) != -1)
                {
                    if (WindowHwnd == "")
                    {
                        WindowHwnd = p.MainWindowHandle.ToString();
                    }
                    else
                    {
                        WindowHwnd = WindowHwnd + "|" + p.MainWindowHandle.ToString();
                    }
                }
            }

            if (WindowHwnd == "") return "";
            return WindowHwnd;
        }

        /// <summary>
        /// 遍历方法 - 返回一个List<windowinfo> 集合
        /// </windowinfo></summary>
        /// <returns></returns>
        public List<WindowInfo> GetAllDesktopWindows()
        {
            List<WindowInfo> wndList = new List<WindowInfo>();

            // enum all desktop windows
            EnumWindows(delegate (IntPtr hWnd, int lParam)
            {
                WindowInfo wnd = new WindowInfo();
                var sb = new StringBuilder(256);
                wnd.hWnd = hWnd;
                GetWindowTextW(hWnd, sb, sb.Capacity);
                wnd.szWindowName = sb.ToString();
                GetClassNameW(hWnd, sb, sb.Capacity);
                wnd.szClassName = sb.ToString();
                wndList.Add(wnd);
                return true;
            }, 0);

            return wndList;
        }

        /// <summary>
        /// 不改变尺寸移动窗口到指定位置
        /// </summary>
        /// <param name="Hwnd">窗口句柄
        /// <param name="X">目的地左上角X
        /// <param name="Y">目的地左上角Y
        /// <returns>移动成功返回 true</returns>
        public bool MoveWindow(int Hwnd, int X, int Y)
        {
            Rectangle rect = new Rectangle();
            GetWindowRect((IntPtr)Hwnd, ref rect);
            return MoveWindow((IntPtr)Hwnd, rect.Left, rect.Top, rect.Right, rect.Bottom, true);
        }

        /// <summary>
        /// 改变尺寸移动窗口到指定位置
        /// </summary>
        /// <param name="Hwnd">窗口句柄
        /// <param name="X">目的地左上角X
        /// <param name="Y">目的地左上角Y
        /// <param name="Width">新宽度
        /// <param name="Height">新高度
        /// <returns>移动成功返回 true</returns>
        public bool MoveWindow(int Hwnd, int X, int Y, int Width, int Height)
        {
            return MoveWindow((IntPtr)Hwnd, X, Y, Width, Height, true);
        }

        /// <summary>
        /// 根据进程名获得窗口句柄 - 不需要带上进程后缀
        /// </summary>
        /// <param name="ProssName">进程名
        /// <returns>窗口句柄 找不到返回 0</returns>
        int FindWindow_ByProcessName(string ProssName)
        {
            Process[] pp = Process.GetProcessesByName(ProssName);

            for (int i = 0; i < pp.Length; i++)
            {
                if (pp[i].ProcessName == ProssName)
                {
                    return (int)pp[i].MainWindowHandle;
                }
            }

            return 0;
        }

        /// <summary>
        /// 根据进程名获得窗口句柄 - 不需要带上进程后缀
        /// </summary>
        /// <param name="ProssName">进程名
        /// <returns>窗口句柄 多个用"|"隔开 找不到返回 ""</returns>
        string FindWindowEx_ByProcessName(string ProssName)
        {
            var Hwnd = "";
            Process[] pp = Process.GetProcessesByName(ProssName);

            for (int i = 0; i < pp.Length; i++)
            {
                if (pp[i].ProcessName == ProssName)
                {
                    if (Hwnd == "")
                    {
                        Hwnd = pp[i].MainWindowHandle.ToString();
                    }
                    else
                    {
                        Hwnd = Hwnd + "|" + pp[i].MainWindowHandle.ToString();
                    }
                }
            }

            return Hwnd;
        }

        public struct WindowInfo
        {

            public IntPtr hWnd;
            public string szClassName;
            public string szWindowName;
        }
    }//end class
} 