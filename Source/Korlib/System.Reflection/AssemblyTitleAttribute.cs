/*
 * (c) 2014 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Stefan Andres Charsley (charsleysa) <charsleysa@gmail.com>
 */

namespace System.Reflection
{
    [AttributeUsage(AttributeTargets.Assembly, Inherited = false)]
    public sealed class AssemblyTitleAttribute : Attribute
    {
        private readonly string title;

        /// <summary>
        /// The assembly title.
        /// </summary>
        public string Title
        {
            get { return this.title; }
        }

        /// <summary>
        /// Initializes a new instance of the AssemblyTitleAttribute class.
        /// </summary>
        /// <param name="title">The assembly title.</param>
        public AssemblyTitleAttribute(string title)
        {
            this.title = title;
        }
    }
}
