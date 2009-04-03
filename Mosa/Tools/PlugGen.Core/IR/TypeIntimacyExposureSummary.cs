/*
 * (c) 2009 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Bruce Markham       < illuminus86@gmail.com >
 */


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PlugGen.IR
{
    public class TypeIntimacyExposureSummary
    {
        public bool IsInternalType;
        public bool HasInternalInstanceMembers;
        public bool HasInternalStaticMembers;
        public bool HasNestedTypes;
        public bool IsNestedType;
    }
}
