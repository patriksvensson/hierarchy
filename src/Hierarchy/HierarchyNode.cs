﻿//
// Copyright 2011 Patrik Svensson
//
// This file is part of Hierarchy.
//
// Hierarchy is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// Hierarchy is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU Lesser Public License for more details.
//
// You should have received a copy of the GNU Lesser Public License
// along with Hierarchy. If not, see <http://www.gnu.org/licenses/>.
//

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Hierarchy
{
	public abstract class HierarchyNode
	{
		private readonly IHierarchyProvider _provider;
		private readonly Guid _guid;
		private TreeNode _node;

		#region Properties

		public IHierarchyProvider Provider
		{
			get { return _provider; }
		}

		public TreeNode TreeNode
		{
			get { return _node; }
			internal set { _node = value; }
		}

		public virtual string ImageKey
		{
			get { return this.GetType().FullName; }
		}

		public virtual bool SupportsChildren
		{
			get { return true; }
		}

		public virtual bool LazyLoad
		{
			get { return false; }
		}

		public virtual bool AutoExpand
		{
			get { return true; }
		}

		public virtual Guid Key
		{
			get { return _guid; }
		}

		public virtual bool AllowRename
		{
			get { return false; }
		}

		public virtual bool AllowDelete
		{
			get { return false; }
		}

		public abstract string Text { get; }

		#endregion

		protected HierarchyNode(IHierarchyProvider provider)
		{
			_provider = provider;
			_guid = Guid.NewGuid();
		}

		public void Rename()
		{
			if (this.AllowRename)
			{
				IHierarchyTree tree = _node.TreeView as IHierarchyTree;
				if (tree != null)
				{
					tree.Rename(this);
				}
			}
		}

		public void Delete()
		{
			if (this.AllowDelete)
			{
				IHierarchyTree tree = _node.TreeView as IHierarchyTree;
				if (tree != null)
				{
					tree.Delete(this);
				}
			}
		}
	}
}
