/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Simon Wollwage (rootnode) <kintaro@think-in-co.de>
 */

using System;
using System.Collections.Generic;
using System.Text;

namespace Pictor.UI.Dialogs
{
    /// <summary>
    /// A simple Yes-No-Dialog
    /// </summary>
    public class YesNoDialog : Dialog
    {
        bool result = false;
        ButtonWidget yesButton;
        ButtonWidget noButton;

        /// <summary>
        /// Returns the dialog's result.
        /// True if "Yes" and false otherwise
        /// </summary>
        public bool Result
        {
            get 
            {
                if (!Running && null != Parent)
                    Parent.RemoveChild(this);
                return result;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="label"></param>
        public YesNoDialog(double x, double y, string label)
            : base(x, y, 120, 50, label)
        {
            yesButton = new ButtonWidget(5, 5, "Yes", 10, 1, 1, 2);
            noButton = new ButtonWidget(55, 5, "No", 10, 1, 1, 2);

            yesButton.ButtonClick += Yes;
            noButton.ButtonClick += No;

            AddChild(yesButton);
            AddChild(noButton);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="button"></param>
        private void Yes(ButtonWidget button)
        {
            Running = false;
            result = true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="button"></param>
        private void No(ButtonWidget button)
        {
            Running = false;
            result = false;
        }
    }
}
