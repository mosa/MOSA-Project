using System;
using System.Collections.Generic;
using System.Text;

namespace Pictor.UI.Dialogs
{
    /// <summary>
    /// 
    /// </summary>
    public class Dialog : Window
    {
        /// <summary>
        /// 
        /// </summary>
        bool running = true;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="label"></param>
        public Dialog(double x, double y, double width, double height, string label)
            : base(x, y, width, height, label)
        {
        }

        /// <summary>
        /// True if the dialog is still being displayed
        /// </summary>
        public bool Running
        {
            get { return running; }
            set { running = value; }
        }

        /// <summary>
        /// Displays the dialog and adds it to the parent window's
        /// child list
        /// </summary>
        /// <param name="parent">The parent window</param>
        public void Show(UIWidget parent)
        {
            parent.AddChild(this);
        }
    }
}
