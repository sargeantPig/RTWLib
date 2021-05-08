using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RTWLib.Extensions
{
    public static class ExtControl
    {
		public static void AppendText(this RichTextBox box, string text, Color color)
		{
			box.SelectionStart = box.TextLength;
			box.SelectionLength = 0;

			box.SelectionColor = color;
			box.AppendText(text);
			box.SelectionColor = box.ForeColor;
		}

		public static List<Control> ToList(this Control.ControlCollection coll)
		{
			List<Control> controls = new List<Control>();
			foreach (Control control in coll)
			{

				controls.Add(control);
			}
			return controls;
		}
	}
}
