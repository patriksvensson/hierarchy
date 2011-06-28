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
using Hierarchy;
using System.Windows.Forms;
using System.Drawing;
using Hierarchy.Sample.Properties;

namespace Hierarchy.Sample
{
	public class BarProvider : HierarchyProvider<BarDataRepository>
	{
		public override IEnumerable<HierarchyNode> GetChildren(BarDataRepository context, HierarchyNode node)
		{
			if (node is FooNode)
			{
				foreach (BarData bar in context.GetBars())
				{
					yield return new BarNode(this, bar);
				}
			}
		}

		public override bool HasChildren(BarDataRepository context, HierarchyNode node)
		{
			if (node is FooNode)
			{
				return context.GetBars().Any();
			}
			return false;
		}

		public override IEnumerable<KeyValuePair<string, Image>> GetImages()
		{
			yield return new KeyValuePair<string, Image>(typeof(BarNode).FullName, Resources.Blue);
		}

		protected override IEnumerable<HierarchyTask<BarDataRepository>> GetTasks(BarDataRepository context, HierarchyNode node)
		{
			if (node is FooNode)
			{
				yield return new HierarchyTask<BarDataRepository>("Create Bar...", node, (c, n) =>  
				{
					context.AddBar("New Bar");
					node.Refresh();
				});
			}
			else if (node is BarNode)
			{
				yield return new HierarchyTask<BarDataRepository>("Delete...", node, (c, n) => { n.Delete(); });
				yield return new HierarchyTask<BarDataRepository>("Rename...", node, (c, n) => { n.Rename(); });
			}
		}

		protected override bool HandleRename(BarDataRepository context, HierarchyNode node, string newName)
		{
			if (node is BarNode)
			{
				context.RenameBar(((BarNode)node).Id, newName);
				return true;
			}
			return false;
		}

		protected override bool HandleDelete(BarDataRepository context, HierarchyNode node)
		{
			if (node is BarNode)
			{
				string message = string.Format("Do you want to delete the node '{0}'?", node.Text);
				var result = MessageBox.Show(message, "Delete?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
				if (result == DialogResult.Yes)
				{
					context.DeleteBar(((BarNode)node).Id);
					return true;
				}
			}
			return false;
		}
	}
}
