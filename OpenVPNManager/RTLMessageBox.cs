using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;
using System.Windows.Forms;

namespace OpenVPNManager
{
    class RTLMessageBox
    {
        public static DialogResult Show(string text)
        {
            return Show(null, text, "OpenVPN Manager", MessageBoxButtons.OK,
                MessageBoxIcon.None, MessageBoxDefaultButton.Button1,
                (MessageBoxOptions) 0);
        }

        public static DialogResult Show(string text,
            MessageBoxButtons buttons, MessageBoxDefaultButton defaultButton)
        {
            return Show(null, text, "OpenVPN Manager", buttons, MessageBoxIcon.None, 
                defaultButton, (MessageBoxOptions) 0);
        }

        public static DialogResult Show(IWin32Window owner, string text,
            MessageBoxButtons buttons, MessageBoxDefaultButton defaultButton)
        {
            return Show(owner, text, "OpenVPN Manager", buttons, MessageBoxIcon.None, 
                defaultButton, (MessageBoxOptions) 0);
        }

        public static DialogResult Show(string text,
            MessageBoxButtons buttons, MessageBoxDefaultButton defaultButton,
            MessageBoxIcon icon)
        {
            return Show(null, text, "OpenVPN Manager", buttons, icon,
                defaultButton, (MessageBoxOptions) 0);
        }

        public static DialogResult Show(IWin32Window owner, string text,
            MessageBoxButtons buttons, MessageBoxDefaultButton defaultButton,
            MessageBoxIcon icon)
        {
            return Show(owner, text, "OpenVPN Manager", buttons, icon,
                defaultButton, (MessageBoxOptions) 0);
        }

        public static DialogResult Show(string text, MessageBoxIcon icon)
        {
            return Show(null, text, "OpenVPN Manager", MessageBoxButtons.OK, icon,
                MessageBoxDefaultButton.Button1, (MessageBoxOptions)0);
        }

        public static DialogResult Show(IWin32Window owner, string text,
            MessageBoxIcon icon)
        {
            return Show(owner, text, "OpenVPN Manager", MessageBoxButtons.OK, icon, 
                MessageBoxDefaultButton.Button1, (MessageBoxOptions) 0);
        }


        public static DialogResult Show(IWin32Window owner, string text)
        {
            return Show(owner, text, "OpenVPN Manager", MessageBoxButtons.OK,
                MessageBoxIcon.None, MessageBoxDefaultButton.Button1,
                (MessageBoxOptions) 0);
        }



        public static DialogResult Show(IWin32Window owner, string text, 
            string caption, MessageBoxButtons buttons, MessageBoxIcon icon, 
            MessageBoxDefaultButton defaultButton, MessageBoxOptions options)
        {
            
            return MessageBox.Show(owner, text, caption,
                buttons, icon, defaultButton, getDefaultOptions(owner) | options);
        }

        private static MessageBoxOptions getDefaultOptions(IWin32Window owner)
        {
            if (IsRightToLeft(owner))
            {
                return MessageBoxOptions.RtlReading |
                    MessageBoxOptions.RightAlign;
            } 
            else
            {
                return (MessageBoxOptions) 0;
            }
        }

        private static bool IsRightToLeft(IWin32Window owner)
        {
            Control control = owner as Control;

            if (control != null)
            {
                return control.RightToLeft == RightToLeft.Yes;
            } else
                return CultureInfo.CurrentUICulture.TextInfo.IsRightToLeft;
        }
    }
}
