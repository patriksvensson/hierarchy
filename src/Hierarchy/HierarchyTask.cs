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
using System.Drawing;

namespace Hierarchy
{
	public class HierarchyTask<TContext>
	{
		private readonly HierarchyNode m_owner;
		private readonly Action<TContext, HierarchyNode> m_action;

		#region Properties

		public HierarchyNode Owner
		{
			get { return m_owner; }
		}

		public Action<TContext, HierarchyNode> Action
		{
			get { return m_action; }
		}

		public string Title { get; set; }
		public bool Enabled { get; set; }
		public bool Visible { get; set; }
		public Image Image { get; set; }
		public int SortOrder { get; set; }
		public int Group { get; set; }
		internal bool IsSeparator { get; set; }

		#endregion

		public HierarchyTask(string title, HierarchyNode owner, Action<TContext, HierarchyNode> action)
		{
			m_owner = owner;
			m_action = action;

			this.Title = title;
			this.Enabled = true;
			this.Visible = true;
			this.Image = null;
			this.Group = 0;
			this.SortOrder = 0;
		}
	}
}
