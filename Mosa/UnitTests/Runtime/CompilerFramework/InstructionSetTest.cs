using NUnit.Framework;
using Mosa.Runtime.CompilerFramework;

namespace Mosa.Runtime.CompilerFramework.UnitTests
{
	[TestFixture]
	public class InstructionSetTest
	{
		[Test]
		public void Constructor()
		{
			System.Random rnd = new System.Random();
			int size = rnd.Next() % 1000;
			InstructionSet set = new InstructionSet(size);
			
			Assert.AreEqual(size, set.Size);
			Assert.AreEqual(size, set.Data.Length);
		}
		
		[Test]
		public void Clear()
		{
			System.Random rnd = new System.Random();
			int size = rnd.Next() % 1000;
			InstructionSet set = new InstructionSet(size);
			set.Clear();
			
			Assert.AreEqual(0, set.Used);
			Assert.AreEqual(size, set.Size);
			Assert.AreEqual(-1, set.NextArray[size - 1]);
			Assert.AreEqual(-1, set.PrevArray[0]);
			
			for (int i = 0; i < size - 1; ++i)
			{
				Assert.AreEqual(i + 1, set.NextArray[i]);
				Assert.AreEqual(i - 1, set.PrevArray[i]);
			}
		}
		
		[Test]
		public void Resize()
		{
			System.Random rnd = new System.Random();
			int size = rnd.Next() % 1000;
			InstructionSet set = new InstructionSet(size);
			
			int[] next = new int[size * 2];
			int[] prev = new int[size * 2];
			set.NextArray.CopyTo(next, 0);
			set.PrevArray.CopyTo(prev, 0);
			set.Resize(size * 2);
			
			for (int i = 0; i < size; ++i)
			{
				Assert.AreEqual(next[i], set.NextArray[i]);
				Assert.AreEqual(prev[i], set.PrevArray[i]);
			}
		}
	
		[Test]
		public void InsertAfter()
		{
			InstructionSet set = new InstructionSet(10);
			int index = set.CreateRoot();
			
			InstructionData first = new InstructionData();
			first.Offset = 42;
			InstructionData second = new InstructionData();
			second.Offset = 17;
			
			set.Data[index] = first;
			//set.instructions[set.InsertAfter(index)] = second;
			
			Assert.AreEqual(42, set.Data[index].Offset);
			//Assert.AreEqual(17, set.instructions[1].Offset);
		}
		
		[Test]
		public void GetFree()
		{
			InstructionSet set = new InstructionSet(10);
			
			Assert.AreEqual(0, set.GetFree());
			int size = set.Size;
			for (int i = 0; i < size - 1; ++i)
				Assert.AreEqual(set.NextArray[i], set.GetFree());
			Assert.AreEqual(set.NextArray[set.Size - 1], -1);
		}

		[Test]
		public void CreateRoot()
		{
			InstructionSet set = new InstructionSet(10);
			int root = set.CreateRoot();
			
			Assert.AreEqual(0, root);
			Assert.AreEqual(-1, set.NextArray[0]);
			Assert.AreEqual(-1, set.PrevArray[0]);
		}
		
		[Test]
		public void SetOffset()
		{
			System.Random rnd = new System.Random();
			InstructionSet set = new InstructionSet(10);
			int index = set.CreateRoot();
			
			int offset = rnd.Next();
			set.SetOffset(index, offset);
			
			Assert.AreEqual(offset, set.Data[index].Offset);
		}

		[Test]
		public void AutomaticResize()
		{
			InstructionSet set = new InstructionSet(10);
			
			for (int i = 0; i < 10; ++i)
				set.GetFree();
			
			Assert.AreEqual(20, set.Size);
		}
	}
}