// ================================================================================================
// Copyright (c) MOSA Project. Licensed under the New BSD License.
// ================================================================================================
// AUTHOR       : TAYLAN INAN
// E-MAIL       : taylaninan@yahoo.com
// GITHUB       : www.github.com/taylaninan/
// ================================================================================================

using System;

namespace Mosa.Collections.Generic
{
    public enum TestResultCode: byte
    {
        FAILED  = 0x00,
        PASSED  = 0x01,
        SKIPPED = 0x02
    }

    public static class Test
    {
        private static TestResultCode TestResult = TestResultCode.PASSED;

        public static void ResetTestResult()
        {
            TestResult = TestResultCode.PASSED;
        }

        public static TestResultCode GetTestResult()
        {
            return TestResult;
        }

        private static TestResultCode CombineResults (TestResultCode OldResult, TestResultCode NewResult)
        {
            if (OldResult == TestResultCode.FAILED)
            {
                return TestResultCode.FAILED;
            }

            if (OldResult == TestResultCode.PASSED || OldResult == TestResultCode.SKIPPED)
            {
                switch (NewResult)
                {
                    case TestResultCode.FAILED : return TestResultCode.FAILED;
                    case TestResultCode.PASSED : return TestResultCode.PASSED;
                    case TestResultCode.SKIPPED: return TestResultCode.PASSED;
                }
            }

            // I don't know how this can happen, but anyway...
            return TestResultCode.FAILED;
        }

        private static bool IsEqual<AnyType>(AnyType Actual, AnyType Expected, string Message) where AnyType: IComparable
        {
            if (Actual.CompareTo(Expected) == 0)
            {
                TestResult = CombineResults(TestResult, TestResultCode.PASSED);

                return true;
            }
            else
            {
                TestResult = CombineResults(TestResult, TestResultCode.FAILED);

                Console.WriteLine(Message);

                return false;
            }
        }

        private static bool IsNotEqual<AnyType>(AnyType Actual, AnyType Expected, string Message) where AnyType: IComparable
        {
            if (Actual.CompareTo(Expected) != 0)
            {
                TestResult = CombineResults(TestResult, TestResultCode.PASSED);

                return true;
            }
            else
            {
                TestResult = CombineResults(TestResult, TestResultCode.FAILED);

                Console.WriteLine(Message);

                return false;
            }
        }

        public static bool IsEqual(bool Actual, bool Expected, string Message)
        {
            return IsEqual<bool>(Actual, Expected, Message);
        }

        public static bool IsEqual(sbyte Actual, sbyte Expected, string Message)
        {
            return IsEqual<sbyte>(Actual, Expected, Message);
        }

        public static bool IsEqual(byte Actual, byte Expected, string Message)
        {
            return IsEqual<byte>(Actual, Expected, Message);
        }

        public static bool IsEqual(short Actual, short Expected, string Message)
        {
            return IsEqual<short>(Actual, Expected, Message);
        }

        public static bool IsEqual(ushort Actual, ushort Expected, string Message)
        {
            return IsEqual<ushort>(Actual, Expected, Message);
        }

        public static bool IsEqual(int Actual, int Expected, string Message)
        {
            return IsEqual<int>(Actual, Expected, Message);
        }

        public static bool IsEqual(uint Actual, uint Expected, string Message)
        {
            return IsEqual<uint>(Actual, Expected, Message);
        }

        public static bool IsEqual(long Actual, long Expected, string Message)
        {
            return IsEqual<long>(Actual, Expected, Message);
        }

        public static bool IsEqual(ulong Actual, ulong Expected, string Message)
        {
            return IsEqual<ulong>(Actual, Expected, Message);
        }

        public static bool IsEqual(char Actual, char Expected, string Message)
        {
            return IsEqual<char>(Actual, Expected, Message);
        }

        public static bool IsEqual(string Actual, string Expected, string Message)
        {
            return IsEqual<string>(Actual, Expected, Message);
        }

        public static bool IsNotEqual(bool Actual, bool Expected, string Message)
        {
            return IsNotEqual<bool>(Actual, Expected, Message);
        }

        public static bool IsNotEqual(sbyte Actual, sbyte Expected, string Message)
        {
            return IsNotEqual<sbyte>(Actual, Expected, Message);
        }

        public static bool IsNotEqual(byte Actual, byte Expected, string Message)
        {
            return IsNotEqual<byte>(Actual, Expected, Message);
        }

        public static bool IsNotEqual(short Actual, short Expected, string Message)
        {
            return IsNotEqual<short>(Actual, Expected, Message);
        }

        public static bool IsNotEqual(ushort Actual, ushort Expected, string Message)
        {
            return IsNotEqual<ushort>(Actual, Expected, Message);
        }

        public static bool IsNotEqual(int Actual, int Expected, string Message)
        {
            return IsNotEqual<int>(Actual, Expected, Message);
        }

        public static bool IsNotEqual(uint Actual, uint Expected, string Message)
        {
            return IsNotEqual<uint>(Actual, Expected, Message);
        }

        public static bool IsNotEqual(long Actual, long Expected, string Message)
        {
            return IsNotEqual<long>(Actual, Expected, Message);
        }

        public static bool IsNotEqual(ulong Actual, ulong Expected, string Message)
        {
            return IsNotEqual<ulong>(Actual, Expected, Message);
        }

        public static bool IsNotEqual(char Actual, char Expected, string Message)
        {
            return IsNotEqual<char>(Actual, Expected, Message);
        }

        public static bool IsNotEqual(string Actual, string Expected, string Message)
        {
            return IsNotEqual<string>(Actual, Expected, Message);
        }
    }
}
