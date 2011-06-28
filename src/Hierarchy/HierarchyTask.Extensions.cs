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
	public static class HierarchyTaskExtensions
	{
		public static HierarchyTask<TContext> SetGroup<TContext>(this HierarchyTask<TContext> task, int group)
		{
			task.Group = group;
			return task;
		}

		public static HierarchyTask<TContext> SetSortOrder<TContext>(this HierarchyTask<TContext> task, int sortOrder)
		{
			task.SortOrder = sortOrder;
			return task;
		}

		public static HierarchyTask<TContext> SetImage<TContext>(this HierarchyTask<TContext> task, Image image)
		{
			task.Image = image;
			return task;
		}

		public static HierarchyTask<TContext> IsEnabled<TContext>(this HierarchyTask<TContext> task, bool enabled)
		{
			task.Enabled = enabled;
			return task;
		}

		public static HierarchyTask<TContext> IsVisible<TContext>(this HierarchyTask<TContext> task, bool visible)
		{
			task.Visible = visible;
			return task;
		}
	}
}
