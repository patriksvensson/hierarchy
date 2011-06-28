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
using System.Drawing;

namespace Hierarchy
{
	internal partial class HierarchyBuilder<TContext> : IHierarchyBuilder
	{
		private void InitializeImageMapper()
		{
			_tree.ImageList = _tree.ShowNodeImages ? _imageMapper.ImageList : null;
			foreach (HierarchyProvider<TContext> provider in _providers)
			{
				provider.SetBuilder(this);
				foreach (KeyValuePair<string, Image> imageMapping in provider.GetImages())
				{
					_imageMapper.AddImage(imageMapping.Key, imageMapping.Value);
				}
			}
		}

		internal void SetDefaultNodeImage(Image image)
		{
			if (image != null)
			{
				_imageMapper.SetDefaultKey(typeof(HierarchyNode).FullName, image);
			}
		}

		private void UpdateNodeAppearance(HierarchyNode root)
		{
			if (root != null && root.TreeNode != null)
			{
				root.TreeNode.Text = root.Text;
				root.TreeNode.ImageIndex = _imageMapper.GetImageIndex(root.ImageKey);
				root.TreeNode.SelectedImageIndex = root.TreeNode.ImageIndex;
			}
		}

		private void RemoveMissingNodes(HierarchyProvider<TContext> provider, HierarchyNode root, IEnumerable<HierarchyNode> children)
		{
			TreeNodeCollection treeNodes = this.GetTreeNodeCollection(root);
			if (treeNodes == null && root != null)
			{
				if (root.TreeNode == null)
				{
					root.TreeNode = this.FindTreeNode(null, root);
				}
				treeNodes = root.TreeNode.Nodes;
			}
			for (int index = treeNodes.Count - 1; index >= 0; index--)
			{
				TreeNode treeNode = treeNodes[index];
				if (treeNode.Tag == null)
				{
					continue;
				}
				if (((HierarchyNode)treeNode.Tag).Provider == provider)
				{
					bool found = false;
					foreach (var childNode in children)
					{
						// TODO: Implement equality check here.
						if (childNode.Key == ((HierarchyNode)treeNode.Tag).Key)
						{
							found = true;
							break;
						}
					}
					if (!found)
					{
						((HierarchyNode)treeNode.Tag).TreeNode.Remove();
					}
				}
			}
		}

		private TreeNode FindTreeNode(HierarchyNode root, HierarchyNode child)
		{
			TreeNodeCollection treeNodes = this.GetTreeNodeCollection(root);
			foreach (TreeNode treeNode in treeNodes)
			{
				HierarchyNode node = treeNode.Tag as HierarchyNode;
				if (node == null)
				{
					continue;
				}
				if (node.Key == child.Key)
				{
					return treeNode;
				}
			}
			return null;
		}

		private TreeNode CreateTreeNode(HierarchyNode child)
		{
			child.TreeNode = new TreeNode { Tag = child };
			return child.TreeNode;
		}

		private bool HasChildren(TContext context, HierarchyNode child)
		{
			foreach (HierarchyProvider<TContext> provider in _providers)
			{
				if (provider.HasChildren(context, child))
				{
					return true;
				}
			}
			return false;
		}

		protected virtual TreeNodeCollection GetTreeNodeCollection(HierarchyNode root)
		{
			TreeNodeCollection treeNodes = (root == null) ? _tree.Nodes : null;
			if (treeNodes == null && root != null)
			{
				if (root.TreeNode == null)
				{
					root.TreeNode = this.FindTreeNode(null, root);
					if (root.TreeNode == null)
					{
						throw new InvalidOperationException("Could not retrieve tree node.");
					}
				}
				treeNodes = root.TreeNode.Nodes;
			}
			return treeNodes;
		}
	}
}
