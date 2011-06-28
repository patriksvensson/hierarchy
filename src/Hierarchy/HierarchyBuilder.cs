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
	internal partial class HierarchyBuilder<TContext> : IHierarchyBuilder
	{
		private readonly HierarchyTree<TContext> _tree;
		private readonly HierarchyImageListMapper _imageMapper;
		private readonly List<HierarchyProvider<TContext>> _providers;

		public HierarchyImageListMapper ImageMapper
		{
			get { return _imageMapper; }
		}

		public List<HierarchyProvider<TContext>> Providers
		{
			get { return _providers; }
		}

		public IHierarchyTree Tree
		{
			get { return _tree; }
		}

		public HierarchyBuilder(HierarchyTree<TContext> tree, IEnumerable<HierarchyProvider<TContext>> providers)
		{
			_tree = tree;
			_providers = new List<HierarchyProvider<TContext>>(providers);
			_imageMapper = new HierarchyImageListMapper();

			this.InitializeImageMapper();
		}

		public HierarchyNode Build()
		{
			return this.Build(null);
		}

		public HierarchyNode Build(HierarchyNode node)
		{
			// Create the context.
			TContext context = _tree.CreateContext();

			try
			{
				return this.Build(context, node);
			}
			finally
			{
			}
		}

		private HierarchyNode Build(TContext context, HierarchyNode root)
		{
			// Begin the tree update.
			_tree.BeginUpdate();

			// Iterate through all the providers.
			foreach (HierarchyProvider<TContext> provider in _providers)
			{
				// Update the root node.
				this.UpdateNodeAppearance(root);

				// Get all the provider's children for the current node.
				var children = provider.GetChildren(context, root).ToList();
				if (children == null || children.Count() == 0)
				{
					// Remove missing nodes.
					this.RemoveMissingNodes(provider, root, children);
					continue;
				}

				// Remove nodes that exists under the root node but wasn't retrieved by the provider.
				this.RemoveMissingNodes(provider, root, children);

				// Iterate all child nodes.
				foreach (HierarchyNode child in children)
				{
					// Try to find the child tree node.
					TreeNode childTreeNode = this.FindTreeNode(root, child);
					bool foundChildTreeNode = childTreeNode != null;
					if (childTreeNode == null)
					{
						// Create the node tree node.
						childTreeNode = this.CreateTreeNode(child);
					}

					// Update the tree node's tag.
					childTreeNode.Tag = child;
					child.TreeNode = childTreeNode;

					// Update the child tree node's physical parameters.
					this.UpdateNodeAppearance(child);

					// Does the node support children?
					if (child.SupportsChildren)
					{
						// Root have placeholder node?
						if (_tree.HasVirtualNode(root))
						{
							// Remove the placeholder node.
							_tree.RemoveVirtualNode(root);

							// Child have children of it's own?
							if (this.HasChildren(context, child))
							{
								// Add placeholder node.
								_tree.AddVirtualNode(child, false);
							}
						}
						else
						{
							// Does the child tree node have children?
							if (childTreeNode.Nodes.Count > 0)
							{
								// Is the tree node expanded?
								if (childTreeNode.IsExpanded)
								{
									// Refresh the hierarchy child nodes recursivly.
									this.Build(context, child);
								}
							}
							else
							{
								// Child have children of it's own?
								if (this.HasChildren(context, child))
								{
									// Add placeholder node.
									_tree.AddVirtualNode(child, false);
								}
							}
						}
					}

					// Did we not find the tree node before?
					if (!foundChildTreeNode)
					{
						// Add the child tree node to the root tree node collection. 
						this.GetTreeNodeCollection(root).Add(childTreeNode);
					}

					// Should the child node auto expand?
					if (child.AutoExpand)
					{
						// Is the child tree node not expanded?
						if (!child.TreeNode.IsExpanded)
						{
							// Refresh the child node.
							this.Build(context, child);

							// Expand the tree node.
							child.TreeNode.Expand();
						}
					}
					else
					{
						if (!child.LazyLoad)
						{
							// Refresh the child node.
							this.Build(context, child);
						}
					}
				}
			}

			// End the tree update.
			_tree.EndUpdate();

			// Return the node.
			return root;
		}
	}
}
