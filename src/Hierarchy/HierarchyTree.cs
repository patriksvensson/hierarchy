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
using System.ComponentModel;

namespace Hierarchy
{
	public abstract partial class HierarchyTree<TContext> : TreeView, IHierarchyTree
	{
		private HierarchyBuilder<TContext> _builder;
		private bool _showNodeImages;

		[Browsable(true)]
		[DefaultValue(true)]
		public bool ShowNodeImages 
		{
			get { return _showNodeImages; }
			set { _showNodeImages = value; }
		}

		[Browsable(true)]
		public Image DefaultNodeImage { get; set; }

		public HierarchyTree()
		{
			_showNodeImages = true;
		}

		public void Initialize(params HierarchyProvider<TContext>[] providers)
		{
			if (_builder == null)
			{
				// Create the hierarchy builder.
				_builder = new HierarchyBuilder<TContext>(this, providers);

				if (this.DefaultNodeImage != null)
				{
					// Set the default node image for the tree.
					_builder.SetDefaultNodeImage(this.DefaultNodeImage);
				}
			}
		}

		public abstract TContext CreateContext();
		public abstract void ReleaseContext(TContext context);

		public void Build()
		{
			if (_builder != null)
			{
				_builder.Build();
			}
		}

		public void Build(HierarchyNode node)
		{
			if (_builder != null)
			{
				_builder.Build(node);
			}
		}

		protected override void OnKeyUp(KeyEventArgs e)
		{
			if (this.SelectedNode == null)
			{
				return;
			}

			HierarchyNode node = this.SelectedNode.Tag as HierarchyNode;
			if (node == null) 
			{
				return;
			}

			if (e.KeyData == Keys.F2)
			{
				if (node.AllowRename)
				{
					node.Rename();
				}
			}
			if (e.KeyData == Keys.Delete)
			{
				if (node.AllowDelete)
				{
					node.Delete();
				}
			}
		}

		protected override void OnAfterLabelEdit(NodeLabelEditEventArgs e)
		{
			this.LabelEdit = false;

			if (e.Label != null)
			{
				TreeNode treeNode = e.Node;
				if (treeNode != null)
				{
					HierarchyNode node = treeNode.Tag as HierarchyNode;
					if (node != null)
					{
						HierarchyProvider<TContext> provider = node.Provider as HierarchyProvider<TContext>;
						if (provider != null)
						{
							TContext context = this.CreateContext();
							try
							{
								if (provider.HandleRename(context, node, e.Label))
								{
									node.Refresh();
									return;
								}
							}
							finally
							{
								this.ReleaseContext(context);
							}
						}
					}
				}
			}
			e.CancelEdit = true;
		}

		public void Rename(HierarchyNode node)
		{		
			if (node != null && node.TreeNode != null)
			{
				if (node.AllowRename)
				{
					this.LabelEdit = true;
					node.TreeNode.BeginEdit();
				}
			}
		}

		public void Delete(HierarchyNode node)
		{
			if (node != null && node.TreeNode != null)
			{
				if (node.AllowDelete)
				{
					HierarchyProvider<TContext> provider = node.Provider as HierarchyProvider<TContext>;
					if (provider != null)
					{
						TContext context = this.CreateContext();
						try
						{
							if (provider.HandleDelete(context, node))
							{
								// Refresh the parent.
								node.RefreshParent();
							}
						}
						finally
						{
							this.ReleaseContext(context);
						}
					}
				}
			}
		}
	}
}
