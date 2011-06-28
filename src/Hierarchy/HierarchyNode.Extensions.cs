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
	public static class HierarchyNodeExtensions
	{
		public static HierarchyNode GetParent(this HierarchyNode node)
		{
			if (node != null || node.TreeNode != null)
			{
				TreeNode parentTreeNode = node.TreeNode.Parent;
				if (parentTreeNode != null)
				{
					return parentTreeNode.Tag as HierarchyNode;
				}
			}
			return null;
		}

		public static HierarchyNode Refresh(this HierarchyNode node)
		{
			if (node != null)
			{
				IHierarchyBuilder builder = node.Provider.Builder;
				if (builder != null)
				{
					return builder.Build(node);
				}				
			}
			return null;
		}

		public static HierarchyNode RefreshParent(this HierarchyNode node)
		{
			if (node != null)
			{
				HierarchyNode parentNode = node.GetParent();
				if (parentNode != null)
				{
					return parentNode.Refresh();
				}
			}
			return null;
		}
	}
}
