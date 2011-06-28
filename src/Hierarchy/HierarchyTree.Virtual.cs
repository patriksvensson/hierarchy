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
	public abstract partial class HierarchyTree<TContext>
	{
		private const string VirtualNodeKey = "{75A21630-0CF5-4A7D-BD49-34129FE72CF8}";

		public bool HasVirtualNode(HierarchyNode root)
		{
			if (root != null)
			{
				foreach (TreeNode node in root.TreeNode.Nodes)
				{
					if (node.Text == HierarchyTree<TContext>.VirtualNodeKey)
					{
						return true;
					}
				}
			}
			return false;
		}

		public void AddVirtualNode(HierarchyNode root, bool check)
		{
			if (check)
			{
				if (this.HasVirtualNode(root))
				{
					return;
				}
			}
			root.TreeNode.Nodes.Add(HierarchyTree<TContext>.VirtualNodeKey);
		}

		public void RemoveVirtualNode(HierarchyNode root)
		{
			foreach (TreeNode node in root.TreeNode.Nodes)
			{
				if (node.Text.Equals(HierarchyTree<TContext>.VirtualNodeKey))
				{
					node.Remove();
					return;
				}
			}
		}
	}
}
