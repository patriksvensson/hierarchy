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
	public abstract partial class HierarchyTree<TContext> : TreeView
	{
		protected override void OnMouseDown(MouseEventArgs e)
		{
			this.ContextMenuStrip = null;
			if (e.Button == MouseButtons.Right)
			{
				var hitTestInfo = this.HitTest(e.X, e.Y);
				if (hitTestInfo.Node != null)
				{
					this.SelectedNode = hitTestInfo.Node;
					HierarchyNode node = hitTestInfo.Node.Tag as HierarchyNode;
					if (node != null)
					{
						ContextMenuStrip menu = null;
						TContext context = this.CreateContext();

						try
						{
							menu = this.CreateTaskContextMenuStrip(context, node);
						}
						finally
						{
							this.ReleaseContext(context);
						}

						this.ContextMenuStrip = menu;
						this.SelectedNode = hitTestInfo.Node;
					}
				}
			}
		}

		private void HierarchyTask_Click(object sender, EventArgs e)
		{
			var node = sender as ToolStripItem;
			if (node != null)
			{
				HierarchyTask<TContext> task = node.Tag as HierarchyTask<TContext>;
				if (task != null && task.Owner != null && task.Action != null)
				{
					// Create the context.
					TContext context = this.CreateContext();

					try
					{
						task.Action(context, task.Owner);
					}
					finally
					{
						// Release the context.
						this.ReleaseContext(context);
					}
				}
			}
		}

		private ContextMenuStrip CreateTaskContextMenuStrip(TContext context, HierarchyNode node)
		{
			ContextMenuStrip menu = new ContextMenuStrip();
			List<HierarchyTask<TContext>> tasks = new List<HierarchyTask<TContext>>();
			foreach (HierarchyProvider<TContext> provider in _builder.Providers)
			{
				// Get all provider's tasks.
				var providerTasks = provider.GetTasks(context, node).ToList();
				tasks.AddRange(providerTasks);
			}
			this.AddTasksToContextMenuStrip(menu, tasks);
			return menu;
		}

		private void AddTasksToContextMenuStrip(ContextMenuStrip menu, IEnumerable<HierarchyTask<TContext>> tasks)
		{
			// Group the tasks by their task group.
			var grouped = from task in tasks
						  group task by task.Group into taskGroup
						  select new { Group = taskGroup.Key, Tasks = taskGroup };

			int groupIndex = 0;
			foreach (var group in grouped)
			{
				// Iterate through all tasks in the group.
				foreach (var task in group.Tasks.OrderBy(x => x.SortOrder))
				{
					if (!task.IsSeparator)
					{
						menu.Items.Add(this.CreateContextMenuItem(task));
					}
				}

				// Add the separator (if this is not the last one).
				if (groupIndex != grouped.Count() - 1)
				{
					menu.Items.Add(new ToolStripSeparator());
				}

				groupIndex++;
			}
		}

		private ToolStripItem CreateContextMenuItem(HierarchyTask<TContext> task)
		{
			if (task.IsSeparator)
			{
				return new ToolStripSeparator();
			}
			else
			{
				ToolStripItem item = new ToolStripMenuItem();
				item.Text = task.Title;
				item.Image = task.Image;
				item.Enabled = task.Enabled;
				item.Tag = task;
				item.Click += new EventHandler(HierarchyTask_Click);
				return item;
			}
		}
	}
}
