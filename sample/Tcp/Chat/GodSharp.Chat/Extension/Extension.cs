using System;
using System.Drawing;
using System.Windows.Forms;

namespace GodSharp.Chat
{
    public static class Extension
    {
        private static void Append(this RichTextBox ctrl, string text, Color? color = null, bool addNewLine = true)
        {
            if (addNewLine)
            {
                text += Environment.NewLine;
            }

            ctrl.SelectionStart = ctrl.TextLength;
            ctrl.SelectionLength = 0;
            if (color.HasValue)
            {
                ctrl.SelectionColor = color.Value;
            }
            ctrl.AppendText(text);

            ctrl.SelectionColor = ctrl.ForeColor;
        }

        public static void Append(this RichTextBox ctrl, string text, Color? color=null)
        {
            Append(ctrl, text, color, false);
        }

        public static void AppendLine(this RichTextBox ctrl, string text, Color? color = null)
        {
            Append(ctrl, text, color, true);
        }

        public static void ScrollToBottom(this RichTextBox ctrl)
        {
            ctrl.SelectionStart = ctrl.TextLength;
            ctrl.ScrollToCaret();
        }
    }
}