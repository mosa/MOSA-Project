// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.FileSystem.VFS
{
	/// <summary>
	///
	/// </summary>
	public sealed class DirectoryEntry
	{
		#region Static data members

		/// <summary>
		/// Holds the current directory of the current thread.
		/// </summary>
		[System.ThreadStatic]
		private static DirectoryEntry currentDirectory = null;

		#endregion Static data members

		#region Data members

		/// <summary>
		/// References the inode that belongs to this name.
		/// </summary>
		private IVfsNode inode;

		/// <summary>
		/// The name of this directory entry.
		/// </summary>
		private string name;

		/// <summary>
		/// Ptr to the parent directory entry.
		/// </summary>
		/// <remarks>
		/// If _parent == this, we're at the root directory entry.
		/// </remarks>
		private DirectoryEntry parent;

		/// <summary>
		/// Sorted list of child directory entries of this name.
		/// </summary>
		private DirectoryEntry child, next;

		#endregion Data members

		#region Construction

		/// <summary>
		///
		/// </summary>
		public DirectoryEntry()
		{
		}

		#endregion Construction

		#region Properties

		/// <summary>
		///
		/// </summary>
		public string Name
		{
			get
			{
				return this.name;
			}
		}

		/// <summary>
		///
		/// </summary>
		public IVfsNode Node
		{
			get
			{
				return inode;
			}
		}

		/// <summary>
		///
		/// </summary>
		public DirectoryEntry Parent
		{
			get
			{
				return parent;
			}
		}

		#endregion Properties

		#region Methods

		/// <summary>
		///
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public DirectoryEntry Lookup(string name)
		{
			/*
				DirectoryEntry entry = null;
				int idx = 0, rmin = 0, rmax = _children.Count - 1;

				// Check for common names
				if (name.Equals("."))
					return this;
				if (name.Equals(".."))
					return _parent;

				// Iterative binary lookup into the _children list
				while (rmin <= rmax)
				{
					idx = (rmax + rmin) / 2;
					entry = _children[idx];
					if (name == entry.Name)
					{
						return entry;
					}

					if (0 < entry.Name.CompareTo(name))
					{
						rmax = idx - 1;
					}
					else
					{
						rmin = idx + 1;
					}
				}

				// FIXME: Maybe we don't have everything from the inode we're naming. Get all subentries from the inode.
			 */

			DirectoryEntry e = child;
			while ((e != null) && (e.name != name))
				e = e.next;

			return e;
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="parent"></param>
		/// <param name="name"></param>
		/// <param name="node"></param>
		private void Setup(DirectoryEntry parent, string name, IVfsNode node)
		{
			if (!System.Object.ReferenceEquals(this, parent))
				parent.InsertChild(this);

			this.parent = parent;
			this.name = name;
			inode = node;
		}

		/// <summary>
		/// Releases the DirectoryEntry from the parent DirectoryEntry. This is *not* a delete operation.
		/// </summary>
		/// <remarks>
		/// This function is used to remove a DirectoryEntry from the cache. Some (networked) file systems may want to use
		/// this function to remove "known" directories from the lookup cache when they loose server connection.
		/// </remarks>
		public void Release()
		{
			// FIXME: Remove the entry from the parent and release it to the
			// entry cache in the vfs service.
			if (!System.Object.ReferenceEquals(this, Parent))
				parent.RemoveChild(this);

			inode = null;
			name = null;
			parent = null;
		}

		#endregion Methods

		#region Child list functions

		/// <summary>
		///
		/// </summary>
		/// <param name="child"></param>
		private void InsertChild(DirectoryEntry child)
		{
			/*
						DirectoryEntry entry = null;
						int idx = 0, rmin = 0, rmax = _children.Count - 1;

						// Iterative binary lookup into the _children list
						while (rmin <= rmax)
						{
							idx = (rmax + rmin) / 2;
							entry = _children[idx];
							if (child.Name.Equals(entry.Name))
							{
			#if VFS_NO_EXCEPTIONS
								throw new InvalidOperationException("Duplicate name.");
			#endif // #if VFS_NO_EXCEPTIONS
							}

							if (0 < entry.Name.CompareTo(child.Name))
							{
								rmax = idx - 1;
							}
							else
							{
								rmin = idx + 1;
							}
						}

						_children.Insert(rmin, child);
			 */

			// FIXME: Thread safety
			child.next = this.child;
			this.child = child;
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="child"></param>
		private void RemoveChild(DirectoryEntry child)
		{
			// FIXME: Thread safety
			if (System.Object.ReferenceEquals(this.child, child))
			{
				this.child = child.next;
			}
			else
			{
				DirectoryEntry e = this.child;
				while (!System.Object.ReferenceEquals(e.next, child))
					e = e.next;
				e.next = child.next;
			}

			child.next = null;

			//			_children.Remove(child);
		}

		#endregion Child list functions

		#region Static methods

		/// <summary>
		///
		/// </summary>
		public static DirectoryEntry CurrentDirectoryEntry
		{
			get
			{
				if (currentDirectory == null)
				{
					// FIXME: Use the process root instead of this in order to put processes in a jail.
					currentDirectory = VirtualFileSystem.RootDirectoryEntry;
				}

				return currentDirectory;
			}

			set
			{
				if (value == null)
					throw new System.ArgumentNullException("value");

				currentDirectory = value;
			}
		}

		/// <summary>
		/// Allocates a new DirectoryEntry object for the given settings.
		/// </summary>
		/// <param name="parent">The parent directory entry.</param>
		/// <param name="name">The name of the entry to create.</param>
		/// <param name="node">The vfs node referenced by the directory entry.</param>
		/// <returns>The allocated directory entry.</returns>
		/// <exception cref="System.ArgumentNullException">If any one of the parameters is null.</exception>
		/// <exception cref="System.ArgumentException">If the name is zero-length.</exception>
		/// <remarks>
		/// This method is used to control the DirectoryEntry allocation and maintain a cache of them. Also used to
		/// prevent infinite name allocations.
		/// </remarks>
		public static DirectoryEntry Allocate(DirectoryEntry parent, string name, IVfsNode node)
		{
			//#if VFS_NO_EXCEPTIONS
			if (parent == null)
				throw new System.ArgumentNullException(@"parent");

			if (name == null)
				throw new System.ArgumentNullException(@"name");

			if (node == null)
				throw new System.ArgumentNullException(@"node");

			if (name.Length == 0)
				throw new System.ArgumentException(@"Invalid directory entry name."); // , @"name"

			// FIXME: Add precondition check for invalid characters
			// FIXME: Localize exception messages
			//#endif // #if VFS_NO_EXCEPTIONS

			DirectoryEntry directory = new DirectoryEntry();

			directory.Setup(parent, name, node);

			return directory;
		}

		/// <summary>
		/// Allocates a vfs root directory entry.
		/// </summary>
		/// <param name="node">The vfs node, which corresponds to the root directory.</param>
		/// <returns>The created directory entry.</returns>
		/// <exception cref="System.ArgumentNullException">The specified node is invalid.</exception>
		/// <remarks>
		/// This method creates a directory entry, which has some special properties. The first one is, that
		/// its parent is itself. This provides for the ability to cd .. on the root to stay on the root.
		/// <para/>
		/// The next ability is to create specialized root directories to isolate processes from the remainder
		/// of the filesystem. Setting a root directory created using this method effectively limits the process
		/// to access inside of the newly created namespace.
		/// </remarks>
		public static DirectoryEntry AllocateRoot(IVfsNode node)
		{
#if VFS_NO_EXCEPTIONS
			if (node == null)
				throw new ArgumentNullException(@"node");
#endif // #if VFS_NO_EXCEPTIONS
			DirectoryEntry result = new DirectoryEntry();
			result.Setup(result, System.String.Empty, node);
			return result;
		}

		#endregion Static methods
	}
}
