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
	public abstract class HierarchyProvider<TContext> : IHierarchyProvider
	{
		private HierarchyBuilder<TContext> _builder;

		public IHierarchyBuilder Builder
		{
			get { return _builder; }
		}

		public abstract IEnumerable<HierarchyNode> GetChildren(TContext context, HierarchyNode node);
		public abstract bool HasChildren(TContext context, HierarchyNode node);

		public virtual IEnumerable<KeyValuePair<string, Image>> GetImages()
		{
			yield break;
		}

		internal void SetBuilder(HierarchyBuilder<TContext> builder)
		{
			_builder = builder;
		}

		protected internal virtual IEnumerable<HierarchyTask<TContext>> GetTasks(TContext context, HierarchyNode node)
		{
			yield break;
		}

		protected internal virtual bool HandleRename(TContext context, HierarchyNode node, string newName)
		{
			return false;
		}

		protected internal virtual bool HandleDelete(TContext context, HierarchyNode node)
		{
			return false;
		}
	}
}
