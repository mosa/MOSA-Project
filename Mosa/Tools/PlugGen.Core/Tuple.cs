// TODO: Find Mono licensing for this file. 99% sure it is BSD, but no header or authorship information was easily available

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PlugGen
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", Scope = "type")]
    public static class Tuple
    {
        public static Tuple<Ta, Tb> Create<Ta, Tb>(Ta a, Tb b) { return new Tuple<Ta, Tb>(a, b); }
        public static Tuple<Ta, Tb, Tc> Create<Ta, Tb, Tc>(Ta a, Tb b, Tc c) { return new Tuple<Ta, Tb, Tc>(a, b, c); }

        public static T Assign<T, Ta, Tb>(this Tuple<Ta, Tb> tuple, Func<Ta, Tb, T> f)
        {
            return f(tuple.A, tuple.B);
        }
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", Scope = "type")]
    public struct Tuple<Ta, Tb>
    {
        public Tuple(Ta a, Tb b)
        {
            this.a = a;
            this.b = b;
        }
        readonly Ta a;
        public Ta A { get { return this.a; } }
        readonly Tb b;
        public Tb B { get { return this.b; } }


        public static bool operator ==(Tuple<Ta, Tb> tup1, Tuple<Ta, Tb> tup2)
        {
            return Object.Equals(tup1.A, tup2.A) && Object.Equals(tup1.B, tup2.B);
        }
        public static bool operator !=(Tuple<Ta, Tb> tup1, Tuple<Ta, Tb> tup2)
        {
            return !(tup1 == tup2);
        }
        public override bool Equals(object obj)
        {
            if (obj is Tuple<Ta, Tb>) return this == (Tuple<Ta, Tb>)obj;
            else return false;
        }

        public override int GetHashCode()
        {
            return this.A.GetHashCode() ^ this.B.GetHashCode();
        }
    }

    public struct Tuple<Ta, Tb, Tc>
    {
        public Tuple(Ta a, Tb b, Tc c)
        {
            this.a = a;
            this.b = b;
            this.c = c;
        }
        readonly Ta a;
        public Ta A { get { return this.a; } }
        readonly Tb b;
        public Tb B { get { return this.b; } }
        readonly Tc c;
        public Tc C { get { return this.c; } }

        public static bool operator ==(Tuple<Ta, Tb, Tc> tup1, Tuple<Ta, Tb, Tc> tup2)
        {
            return Object.Equals(tup1.A, tup2.A) && Object.Equals(tup1.B, tup2.B) && Object.Equals(tup1.C, tup2.C);
        }
        public static bool operator !=(Tuple<Ta, Tb, Tc> tup1, Tuple<Ta, Tb, Tc> tup2)
        {
            return !(tup1 == tup2);
        }
        public override bool Equals(object obj)
        {
            if (obj is Tuple<Ta, Tb, Tc>) return this == (Tuple<Ta, Tb, Tc>)obj;
            else return false;
        }

        public override int GetHashCode()
        {
            return this.A.GetHashCode() ^ this.B.GetHashCode() ^ this.C.GetHashCode();
        }
    }
}
