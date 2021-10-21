// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;

namespace Mosa.Collections.Generic
{
    public class TClass: IComparable
    {
        public uint ID = 0;
        public uint Magic = 0;

        public TClass()
        {
            this.ID = 0;
            this.Magic = 0;
        }

        public TClass(uint ID, uint Magic)
        {
            this.ID = ID;
            this.Magic = Magic;
        }

        public int CompareTo(object obj)
        {
            if (obj != null)
            {
                if (this.GetType() == obj.GetType() && this is TClass && obj is TClass)
                {
                    int Result = 0;

                    TClass LHS = this;
                    TClass RHS = (TClass)obj;

                    if (LHS.Magic < RHS.Magic) Result = -1;
                    if (LHS.Magic == RHS.Magic) Result = 0;
                    if (LHS.Magic > RHS.Magic) Result = 1;

                    return Result;
                }
                else
                {
                    throw new ArgumentException("Object is not a 'TClass' type object!");
                }
            }
            else
            {
                throw new ArgumentNullException("obj", "Object cannot be NULL!");
            }
        }

        public static bool operator == (TClass lhs, TClass rhs)
        {
            return lhs.Equals(rhs);
        }

        public static bool operator != (TClass lhs, TClass rhs)
        {
            return !(lhs == rhs);
        }

        public override bool Equals(object obj)
        {
            bool Result = false;

            // In order to have "equality", we must have same type of objects
            if (this.GetType() == obj.GetType() && this is TClass && obj is TClass)
            {
                // Check whether we have same instance
                if (Object.ReferenceEquals(this, obj))
                {
                    Result = true;
                }

                // Check whether both objects are null
                if (Object.ReferenceEquals(this, null) && Object.ReferenceEquals(obj, null))
                {
                    Result = true;
                }

                // Check whether the "contents" are the same
                if (!Object.ReferenceEquals(this, null) && !Object.ReferenceEquals(obj, null))
                {
                    TClass LHS = this;
                    TClass RHS = (TClass)obj;

                    Result = (LHS.Magic == RHS.Magic);
                }
            }

            return Result;
        }

        public override int GetHashCode()
        {
            return (int)this.ID ^ (int)this.Magic;
        }

        public override string ToString()
        {
            return "[ID:" + this.ID + ", MAGIC:" + this.Magic + "]";
        }
    }

    public static class Tests
    {
        public static void BinaryTreeBase()
        {
            BinarySearchTree<uint> BTree = new BinarySearchTree<uint>();
            Test.ResetTestResult();

            BTree.Add(3);
            BTree.Add(3);
            BTree.Add(3);
            BTree.Add(4);
            BTree.Add(2);
            BTree.Add(2);
            BTree.Add(1);
            Test.IsEqual(BTree.GetTotalSize, 4, "BinaryTree<uint>.GetTotalSize should be 4");
            Test.IsEqual(BTree.GetActiveSize, 4, "BinaryTree<uint>.GetActiveSize should be 4");
            Test.IsEqual(BTree.Find(4).CollisionCount, 1, "BinaryTree<uint>.Find(4).Count should be 1");
            Test.IsEqual(BTree.Find(3).CollisionCount, 3, "BinaryTree<uint>.Find(3).Count should be 3");
            Test.IsEqual(BTree.Find(2).CollisionCount, 2, "BinaryTree<uint>.Find(2).Count should be 2");
            Test.IsEqual(BTree.Find(1).CollisionCount, 1, "BinaryTree<uint>.Find(1).Count should be 1");

            BTree.Remove(4);
            BTree.Remove(3);
            BTree.Remove(2);
            BTree.Remove(1);
            Test.IsEqual(BTree.GetTotalSize, 4, "BinaryTree<uint>.GetTotalSize should be 4");
            Test.IsEqual(BTree.GetActiveSize, 2, "BinaryTree<uint>.GetActiveSize should be 2");
            Test.IsEqual(BTree.Find(4).CollisionCount, 0, "BinaryTree<uint>.Find(4).Count should be 0");
            Test.IsEqual(BTree.Find(3).CollisionCount, 2, "BinaryTree<uint>.Find(3).Count should be 2");
            Test.IsEqual(BTree.Find(2).CollisionCount, 1, "BinaryTree<uint>.Find(2).Count should be 1");
            Test.IsEqual(BTree.Find(1).CollisionCount, 0, "BinaryTree<uint>.Find(1).Count should be 0");

            BTree.Add(1);
            Test.IsEqual(BTree.GetTotalSize, 4, "BinaryTree<uint>.GetTotalSize should be 4");
            Test.IsEqual(BTree.GetActiveSize, 3, "BinaryTree<uint>.GetActiveSize should be 3");
            Test.IsEqual(BTree.Find(4).CollisionCount, 0, "BinaryTree<uint>.Find(4).Count should be 0");
            Test.IsEqual(BTree.Find(3).CollisionCount, 2, "BinaryTree<uint>.Find(3).Count should be 2");
            Test.IsEqual(BTree.Find(2).CollisionCount, 1, "BinaryTree<uint>.Find(2).Count should be 1");
            Test.IsEqual(BTree.Find(1).CollisionCount, 1, "BinaryTree<uint>.Find(1).Count should be 1");

            BTree.RemoveAll(3);
            Test.IsEqual(BTree.GetTotalSize, 4, "BinaryTree<uint>.GetTotalSize should be 4");
            Test.IsEqual(BTree.GetActiveSize, 2, "BinaryTree<uint>.GetActiveSize should be 2");
            Test.IsEqual(BTree.Find(4).CollisionCount, 0, "BinaryTree<uint>.Find(4).Count should be 0");
            Test.IsEqual(BTree.Find(3).CollisionCount, 0, "BinaryTree<uint>.Find(3).Count should be 0");
            Test.IsEqual(BTree.Find(2).CollisionCount, 1, "BinaryTree<uint>.Find(2).Count should be 1");
            Test.IsEqual(BTree.Find(1).CollisionCount, 1, "BinaryTree<uint>.Find(1).Count should be 1");

            BTree.Add(5);
            Test.IsEqual(BTree.GetTotalSize, 5, "BinaryTree<uint>.GetTotalSize should be 5");
            Test.IsEqual(BTree.GetActiveSize, 3, "BinaryTree<uint>.GetActiveSize should be 3");
            Test.IsEqual(BTree.Find(5).CollisionCount, 1, "BinaryTree<uint>.Find(5).Count should be 1");
            Test.IsEqual(BTree.Find(4).CollisionCount, 0, "BinaryTree<uint>.Find(4).Count should be 0");
            Test.IsEqual(BTree.Find(3).CollisionCount, 0, "BinaryTree<uint>.Find(3).Count should be 0");
            Test.IsEqual(BTree.Find(2).CollisionCount, 1, "BinaryTree<uint>.Find(2).Count should be 1");
            Test.IsEqual(BTree.Find(1).CollisionCount, 1, "BinaryTree<uint>.Find(1).Count should be 1");
        }

        public static void BinaryTreeClass()
        {
            TClass TClass1 = new TClass(1, 0xcafebabe);
            TClass TClass2 = new TClass(2, 0xdeadbabe);
            TClass TClass3 = new TClass(3, 0xdeadbeef);

            BinarySearchTree<TClass> BTree = new BinarySearchTree<TClass>();
            Test.ResetTestResult();

            BTree.Add(TClass2);
            BTree.Add(TClass1);
            BTree.Add(TClass3);
            Test.IsEqual(BTree.GetTotalSize, 3, "BinaryTree<TClass>.GetTotalSize should be 3");
            Test.IsEqual(BTree.GetActiveSize, 3, "BinaryTree<TClass>.GetActiveSize should be 3");
            Test.IsEqual(BTree.Find(TClass1).CollisionCount, 1, "BinaryTree<TClass>.Find(TClass1).Count should be 1");
            Test.IsEqual(BTree.Find(TClass2).CollisionCount, 1, "BinaryTree<uint>.Find(TClass2).Count should be 1");
            Test.IsEqual(BTree.Find(TClass3).CollisionCount, 1, "BinaryTree<uint>.Find(TClass3).Count should be 1");

            BTree.Add(TClass1);
            BTree.Add(TClass3);
            Test.IsEqual(BTree.GetTotalSize, 3, "BinaryTree<TClass>.GetTotalSize should be 3");
            Test.IsEqual(BTree.GetActiveSize, 3, "BinaryTree<TClass>.GetActiveSize should be 3");
            Test.IsEqual(BTree.Find(TClass1).CollisionCount, 2, "BinaryTree<TClass>.Find(TClass1).Count should be 2");
            Test.IsEqual(BTree.Find(TClass2).CollisionCount, 1, "BinaryTree<uint>.Find(TClass2).Count should be 1");
            Test.IsEqual(BTree.Find(TClass3).CollisionCount, 2, "BinaryTree<uint>.Find(TClass3).Count should be 2");

            BTree.Remove(TClass2);
            Test.IsEqual(BTree.GetTotalSize, 3, "BinaryTree<TClass>.GetTotalSize should be 3");
            Test.IsEqual(BTree.GetActiveSize, 2, "BinaryTree<TClass>.GetActiveSize should be 2");
            Test.IsEqual(BTree.Find(TClass1).CollisionCount, 2, "BinaryTree<TClass>.Find(TClass1).Count should be 2");
            Test.IsEqual(BTree.Find(TClass2).CollisionCount, 0, "BinaryTree<uint>.Find(TClass2).Count should be 0");
            Test.IsEqual(BTree.Find(TClass3).CollisionCount, 2, "BinaryTree<uint>.Find(TClass3).Count should be 2");
        }

        public static void BitFlags()
        {
            BitFlags Options = new BitFlags(32, 0xCAFEBABE);

            Test.IsEqual(Options.GetFlags(), 0xCAFEBABE, "BitFlags.GetFlags should be 0xCAFEBABE");

            Test.IsEqual(Options.Test(0), false, "BitFlags.Test(0) should be false");
            Options.Reset(0);
            Test.IsEqual(Options.Test(0), false, "BitFlags.Test(0) should be false");
            Options.Set(0);
            Test.IsEqual(Options.Test(0), true, "BitFlags.Test(0) should be true");

            Test.IsEqual(Options.GetFlags(), 0xCAFEBABF, "BitFlags.GetFlags should be 0xCAFEBABF");
        }

        public static void DictionaryBase()
        {
            Dictionary<uint, uint> MagicWords = new Dictionary<uint, uint>();
            Test.ResetTestResult();

            MagicWords.Add(1, 0xCAFEBABE);
            MagicWords.Add(2, 0xDEADBEEF);
            MagicWords.Add(3, 0xDEADCAFE);
            Test.IsEqual(MagicWords.Count, 3, "Dictionary<uint,uint>.Count should be 3");
            Test.IsEqual(MagicWords[1], 0xCAFEBABE, "Dictionary<uint,uint>[1] should be 0xCAFEBABE");
            Test.IsEqual(MagicWords[2], 0xDEADBEEF, "Dictionary<uint,uint>[2] should be 0xDEADBEEF");
            Test.IsEqual(MagicWords[3], 0xDEADCAFE, "Dictionary<uint,uint>[3] should be 0xDEADCAFE");

            MagicWords = null;
        }

        public static void DictionaryClass()
        {
            Dictionary<uint, TClass> MagicWords = new Dictionary<uint, TClass>();
            Test.ResetTestResult();

            TClass TClass1 = new TClass(1, 0xCAFEBABE);
            TClass TClass2 = new TClass(2, 0xDEADBABE);
            TClass TClass3 = new TClass(3, 0xDEADBEEF);
            TClass TClass4 = new TClass(4, 0xCAFEBEEF);

            MagicWords.Add(1, TClass1);
            MagicWords.Add(2, TClass2);
            MagicWords.Add(3, TClass3);
            Test.IsEqual(MagicWords.Count, 3, "Dictionary<uint, TClass>.Count should be 3");
            Test.IsEqual(MagicWords[1].Magic, 0xCAFEBABE, "Dictionary<uint, TClass>[1].Magic should be 0xCAFEBABE");
            Test.IsEqual(MagicWords[2].Magic, 0xDEADBABE, "Dictionary<uint, TClass>[2].Magic should be 0xDEADBABE");
            Test.IsEqual(MagicWords[3].Magic, 0xDEADBEEF, "Dictionary<uint, TClass>[3].Magic should be 0xDEADBEEF");

            // Exception: Key not found
            try
            {
				TClass Dummy = MagicWords[4];
            }
            catch (CollectionsDataNotFoundException Except) { }

            // Exception: Key already exists
            try
            {
                MagicWords.Add(3, TClass4);
            }
            catch (CollectionsDataExistsException Except) { }

            MagicWords.Add(4, TClass4);
            Test.IsEqual(MagicWords[4].Magic, 0xCAFEBEEF, "Dictionary<uint, TClass>[4].Magic should be 0xCAFEBEEF");

            MagicWords = null;
        }

        public static void ListBase()
        {
            List<uint> Primes = new List<uint>();
            Test.ResetTestResult();

            Primes.AddLast(1);
            Primes.AddLast(1);
            Primes.AddLast(1);
            Primes.AddLast(2);
            Primes.AddLast(3);
            Primes.AddLast(3);
            Primes.AddLast(3);
            Primes.AddLast(5);
            Primes.AddLast(7);
            Primes.AddLast(11);
            Primes.AddLast(13);
            Primes.AddLast(17);
            Primes.AddLast(19);
            Test.IsEqual(Primes.Count, 13, "List<uint>.Count should be 13");

            Primes.RemoveAll(1);
            Test.IsEqual(Primes.Count, 10, "List<uint>.Count should be 10");

            Primes.RemoveFirst(3);
            Test.IsEqual(Primes.Count, 9, "List<uint>.Count should be 9");

            Primes.RemoveLast(3);
            Test.IsEqual(Primes.Count, 8, "List<uint>.Count should be 8");

            Primes.RemoveAll(3);
            Test.IsEqual(Primes.Count, 7, "List<uint>.Count should be 7");

            Primes.RemoveAll(7);
            Test.IsEqual(Primes.Count, 6, "List<uint>.Count should be 6");

            Primes.AddAfter(Primes.FindFirst(2), 3);
            Test.IsEqual(Primes.Count, 7, "List<uint>.Count should be 7");

            Primes.AddAfter(Primes.FindFirst(5), 7);
            Test.IsEqual(Primes.Count, 8, "List<uint>.Count should be 8");

            Primes.AddAfter(Primes.FindFirst(19), 23);
            Test.IsEqual(Primes.Count, 9, "List<uint>.Count should be 9");

            Primes.RemoveAll(2);
            Test.IsEqual(Primes.Count, 8, "List<uint>.Count should be 8");

            Primes.RemoveAll(19);
            Test.IsEqual(Primes.Count, 7, "List<uint>.Count should be 7");

            Primes.AddBefore(Primes.FindFirst(3), 2);
            Test.IsEqual(Primes.Count, 8, "List<uint>.Count should be 8");

            Primes.AddBefore(Primes.FindFirst(23), 19);
            Test.IsEqual(Primes.Count, 9, "List<uint>.Count should be 9");

            Primes.Clear();
            Test.IsEqual(Primes.Count, 0, "List<uint>.Count should be 0");

            Primes = null;
        }

        public static void ListClass()
        {
            List<TClass> Classes = new List<TClass>();
            Test.ResetTestResult();

            TClass Class1 = new TClass(1, 0xCAFEBABE);
            TClass Class2 = new TClass(2, 0xDEADBABE);
            TClass Class3 = new TClass(3, 0xDEADBEEF);

            Classes.AddLast(Class1);
            Classes.AddLast(Class2);
            Classes.AddLast(Class3);
            Classes.AddLast(Class1);
            Test.IsEqual(Classes.Count, 4, "List<TClass>.Count should be 4");

            Classes.RemoveAll(Class1);
            Test.IsEqual(Classes.Count, 2, "List<TClass>.Count should be 2");

            Classes.AddBefore(Classes.FindFirst(Class2), Class1);
            Test.IsEqual(Classes.Count, 3, "List<TClass>.Count should be 3");

            Classes.AddAfter(Classes.FindLast(Class3), Class1);
            Test.IsEqual(Classes.Count, 4, "List<TClass>.Count should be 4");

            Classes.Clear();
            Test.IsEqual(Classes.Count, 0, "List<TClass>.Count should be 0");

            Classes = null;
        }

        public static void SortedDictionaryBase()
        {
            Dictionary<uint, uint> MagicWords = new Dictionary<uint, uint>();
            Test.ResetTestResult();

            MagicWords.Add(3, 0xDEADCAFE);
            MagicWords.Add(2, 0xDEADBEEF);
            MagicWords.Add(1, 0xCAFEBABE);

            MagicWords.SortWithBinarySearchTree();

            Test.IsEqual(MagicWords.Count, 3, "SortedDictionary<uint, uint>.Count should be 3");
            Test.IsEqual(MagicWords.GetFirstNode.Value, 0xCAFEBABE, "SortedDictionary<uint, uint>.GetFirstNode.Value should be 0xCAFEBABE");
            Test.IsEqual(MagicWords.GetLastNode.Value, 0xDEADCAFE, "SortedDictionary<uint, uint>.GetLastNode.Value should be 0xDEADCAFE");

            MagicWords = null;
        }

        public static void SortedDictionaryClass()
        {
            Dictionary<uint, TClass> MagicWords = new Dictionary<uint, TClass>();
            Test.ResetTestResult();

            TClass Class1 = new TClass(1, 0xCAFEBABE);
            TClass Class2 = new TClass(2, 0xDEADBEEF);
            TClass Class3 = new TClass(3, 0xDEADCAFE);

            MagicWords.Add(3, Class3);
            MagicWords.Add(2, Class2);
            MagicWords.Add(1, Class1);

            MagicWords.SortWithBinarySearchTree();

            Test.IsEqual(MagicWords.Count, 3, "SortedDictionary<uint, TClass>.Count should be 3");
            Test.IsEqual(MagicWords.GetFirstNode.Value.Magic, 0xCAFEBABE, "SortedDictionary<uint, TClass>.GetFirstNode.Value should be 0xCAFEBABE");
            Test.IsEqual(MagicWords.GetLastNode.Value.Magic, 0xDEADCAFE, "SortedDictionary<uint, TClass>.GetLastNode.Value should be 0xDEADCAFE");

            MagicWords = null;
        }

        public static void SortedListBase()
        {
            List<uint> Primes = new List<uint>();
            Test.ResetTestResult();

            Primes.Add(23);
            Primes.Add(13);
            Primes.Add(29);
            Primes.Add(2);
            Primes.Add(19);
            Primes.Add(5);
            Primes.Add(37);
            Primes.Add(17);
            Primes.Add(3);
            Primes.Add(31);
            Primes.Add(7);
            Primes.Add(11);

            Primes.SortWithBinarySearchTree();

            Test.IsEqual(Primes.Count, 12, "SortedList<uint>.Count should be 12");
            Test.IsEqual(Primes.GetFirstNode.Data, 2, "SortedList<uint>.GetFirstNode.Data should be 2");
            Test.IsEqual(Primes.GetLastNode.Data, 37, "SortedList<uint>.GetLastNode.Data should be 37");

            Primes = null;
        }

        public static void SortedListClass()
        {
            List<TClass> Classes = new List<TClass>();
            Test.ResetTestResult();

            TClass Class1 = new TClass(1, 0xCAFEBABE);
            TClass Class2 = new TClass(2, 0xDEADBEEF);
            TClass Class3 = new TClass(3, 0xDEADCAFE);

            Classes.Add(Class3);
            Classes.Add(Class2);
            Classes.Add(Class1);

            Classes.SortWithBinarySearchTree();

            Test.IsEqual(Classes.Count, 3, "SortedList<TClass>.Count should be 3");
            Test.IsEqual(Classes.GetFirstNode.Data.Magic, 0xCAFEBABE, "SortedList<TClass>.GetFirstNode.Data.Magic should be 0xCAFEBABE");
            Test.IsEqual(Classes.GetLastNode.Data.Magic, 0xDEADCAFE, "SortedList<TClass>.GetLastNode.Data.Magic should be 0xDEADCAFE");

            Classes = null;
        }

        public static void StackBase()
        {
            Stack<uint> Calculator = new Stack<uint>();
            Test.ResetTestResult();

            Calculator.Push(1);
            Calculator.Push(2);
            Calculator.Push(3);
            Calculator.Push(5);
            Calculator.Push(7);
            Calculator.Push(11);
            Calculator.Push(13);
            Calculator.Push(17);
            Calculator.Push(19);
            Calculator.Push(23);
            Calculator.Push(29);
            Calculator.Push(31);
            Calculator.Push(37);

            Test.IsEqual(Calculator.GetSize, 13, "Stack<uint>.GetSize should be 13");
            Test.IsEqual(Calculator.Pop(), 37, "Stack<uint>.Pop should be 37");
            Test.IsEqual(Calculator.Pop(), 31, "Stack<uint>.Pop should be 31");
            Test.IsEqual(Calculator.Pop(), 29, "Stack<uint>.Pop should be 29");
            Test.IsEqual(Calculator.Peek(), 23, "Stack<uint>.Peek should be 23");

            Calculator = null;
        }
        
        public static void StackClass()
        {
            Stack<TClass> Calculator = new Stack<TClass>();
            Test.ResetTestResult();

            TClass Class1 = new TClass(1, 0xCAFEBABE);
            TClass Class2 = new TClass(2, 0xDEADBEEF);
            TClass Class3 = new TClass(3, 0xDEADCAFE);

            Calculator.Push(Class3);
            Calculator.Push(Class2);
            Calculator.Push(Class1);

            Test.IsEqual(Calculator.GetSize, 3, "Stack<TClass>.GetSize should be 3");
            Test.IsEqual(Calculator.Pop().Magic, 0xCAFEBABE, "Stack<TClass>.Pop().Magic should be 0xCAFEBABE");
            Test.IsEqual(Calculator.Pop().Magic, 0xDEADBEEF, "Stack<TClass>.Pop().Magic should be 0xDEADBEEF");
            Test.IsEqual(Calculator.Pop().Magic, 0xDEADCAFE, "Stack<TClass>.Pop().Magic should be 0xDEADCAFE");
            Test.IsEqual(Calculator.GetSize, 0, "Stack<TClass>.GetSize should be 0");

            // Exception: Data not found
            try
            {
                Calculator.Peek();
            }
            catch (CollectionsDataNotFoundException Except) { }

            Calculator = null;
        }

        public static void QueueBase()
        {
            Queue<uint> WaitingLine = new Queue<uint>();
            Test.ResetTestResult();

            WaitingLine.Enqueue(1);
            WaitingLine.Enqueue(2);
            WaitingLine.Enqueue(3);
            WaitingLine.Enqueue(5);
            WaitingLine.Enqueue(7);
            WaitingLine.Enqueue(11);
            WaitingLine.Enqueue(13);
            WaitingLine.Enqueue(17);
            WaitingLine.Enqueue(19);
            WaitingLine.Enqueue(23);
            WaitingLine.Enqueue(29);
            WaitingLine.Enqueue(31);
            WaitingLine.Enqueue(37);

            Test.IsEqual(WaitingLine.Count, 13, "Queue<uint>.Count should be 13");
            Test.IsEqual(WaitingLine.Dequeue(), 1, "Queue<uint>.Dequeue should be 1");
            Test.IsEqual(WaitingLine.Dequeue(), 2, "Queue<uint>.Dequeue should be 2");
            Test.IsEqual(WaitingLine.Dequeue(), 3, "Queue<uint>.Dequeue should be 3");
            Test.IsEqual(WaitingLine.Peek(), 5, "Queue<uint>.Peek should be 5");

            WaitingLine = null;
        }

        public static void QueueClass()
        {
            Queue<TClass> WaitingLine = new Queue<TClass>();

            TClass Class1 = new TClass(1, 0xCAFEBABE);
            TClass Class2 = new TClass(2, 0xDEADBEEF);
            TClass Class3 = new TClass(3, 0xDEADCAFE);

            WaitingLine.Enqueue(Class1);
            WaitingLine.Enqueue(Class2);
            WaitingLine.Enqueue(Class3);

            Test.IsEqual(WaitingLine.Count, 3, "Queue<TClass>.Count should be 3");
            Test.IsEqual(WaitingLine.Dequeue().Magic, 0xCAFEBABE, "Queue<TClass>.Dequeue should be 0xCAFEBABE");
            Test.IsEqual(WaitingLine.Dequeue().Magic, 0xDEADBEEF, "Queue<TClass>.Dequeue should be 0xDEADBEEF");
            Test.IsEqual(WaitingLine.Dequeue().Magic, 0xDEADCAFE, "Queue<TClass>.Dequeue should be 0xDEADCAFE");
            Test.IsEqual(WaitingLine.Count, 0, "Queue<TClass>.Count should be 0");

            // Exception: Data not found
            try
            {
                WaitingLine.Peek();
            }
            catch (CollectionsDataNotFoundException Except) { }

            WaitingLine = null;
        }
    }
}
