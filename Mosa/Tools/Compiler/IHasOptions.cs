/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License
 * with restrictions to the license beneath, concering
 * the use of the CommandLine assembly.
 *
 * Authors:
 *  Kai P. Reisert (<mailto:kpreisert@googlemail.com>)
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
