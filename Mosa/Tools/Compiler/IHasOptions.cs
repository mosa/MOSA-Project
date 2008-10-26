/*
 * Created by Kai P. Reisert using SharpDevelop
 * Date: 26.10.2008 
 */

using System;
using System.Collections.Generic;
using NDesk.Options;

namespace Mosa.Tools.Compiler
{
    /// <summary>
    /// Interface for stages, that can add options to the command line parsing.
    /// </summary>
    public interface IHasOptions
    {
        /// <summary>
        /// Gets the additional options for the parsing process.
        /// </summary>
        /// <returns>An array of options.</returns>
        IEnumerable<Option> GetOptions();
    }
}
