using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Drawing;

public class RtfScrolledBottom : RichTextBox {
    public event EventHandler ScrolledToBottom;

    private const int WmVscroll = 0x115;
    private const int WmMousewheel = 0x20A;
    private const int WmUser = 0x400;
    private const int SbVert = 1;
    private const int EmSetscrollpos = WmUser + 222;
    private const int EmGetscrollpos = WmUser + 221;

    [DllImport("user32.dll")]
    private static extern bool GetScrollRange(IntPtr hWnd, int nBar, out int lpMinPos, out int lpMaxPos);

    [DllImport("user32.dll")]
    private static extern IntPtr SendMessage(IntPtr hWnd, Int32 wMsg, Int32 wParam, ref Point lParam);

    public bool IsAtMaxScroll() {
        int minScroll;
        int maxScroll;
        GetScrollRange(this.Handle, SbVert, out minScroll, out maxScroll);
        Point rtfPoint = Point.Empty;
        SendMessage(this.Handle, EmGetscrollpos, 0, ref rtfPoint);

        return (rtfPoint.Y + this.ClientSize.Height >= maxScroll);
    }

    protected virtual void OnScrolledToBottom(EventArgs e) {
        if (ScrolledToBottom != null)
            ScrolledToBottom(this, e);
    }

    protected override void OnKeyUp(KeyEventArgs e) {
        if (IsAtMaxScroll())
            OnScrolledToBottom(EventArgs.Empty);

        base.OnKeyUp(e);
    }

    protected override void WndProc(ref Message m) {
        if (m.Msg == WmVscroll || m.Msg == WmMousewheel) {
            if (IsAtMaxScroll())
                OnScrolledToBottom(EventArgs.Empty);
        }

        base.WndProc(ref m);
    }

}