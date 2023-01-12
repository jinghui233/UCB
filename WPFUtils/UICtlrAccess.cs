using System;
using System.Drawing;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace WPFUtils.Extension
{
    public static class UICtlrAccess
    {
        public static void AppendText(this DispatcherObject page, RichTextBox richTextBox, string txt, bool scrollToEnd = true, bool limitBlocks = false, int maxBlocks = 100)
        {
            if (!page.Dispatcher.CheckAccess())
            {
                page.Dispatcher.Invoke(new Action(() =>
                {
                    richTextBox.AppendText(txt);
                    while (limitBlocks&& richTextBox.Document.Blocks.Count > maxBlocks)
                    {
                        richTextBox.Document.Blocks.Remove(richTextBox.Document.Blocks.FirstBlock);
                    }
                    if (scrollToEnd) richTextBox.ScrollToEnd();
                }));
            }
            else
            {
                richTextBox.AppendText(txt);
                while (limitBlocks && richTextBox.Document.Blocks.Count > maxBlocks)
                {
                    richTextBox.Document.Blocks.Remove(richTextBox.Document.Blocks.FirstBlock);
                }
                if (scrollToEnd) richTextBox.ScrollToEnd();
            }
        }
        public static string GetVal(this DispatcherObject page, TextBox textBox)
        {
            string txt = "";
            if (!page.Dispatcher.CheckAccess())
            {
                page.Dispatcher.Invoke(new Action(() =>
                {
                    txt = textBox.Text;
                }));
                return txt;
            }
            else
            {
                return textBox.Text;
            }
        }
        public static void SetImage(this DispatcherObject page, Bitmap bitmap, System.Windows.Controls.Image imageControl)
        {
            if (bitmap == null) return;
            BitmapImage bitmapImage = ImageConverter.BitmapToBitmapImage(bitmap);
            if (!page.Dispatcher.CheckAccess())
            {
                page.Dispatcher.Invoke(new Action(() =>
                {
                    imageControl.Source = bitmapImage;
                }));
            }
            else
            {
                imageControl.Source = bitmapImage;
            }
        }
    }
}
