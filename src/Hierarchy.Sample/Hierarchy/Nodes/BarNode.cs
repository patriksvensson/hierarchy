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

namespace Hierarchy.Sample
{
	public class BarNode : HierarchyNode
	{
		private BarData _data;

		public int Id
		{
			get { return _data.Id; }
		}

		public override bool AllowRename
		{
			get { return true; }
		}

		public override bool AllowDelete
		{
			get { return true; }
		}

		public override string Text
		{
			get { return _data.Name; }
		}

		public BarNode(IHierarchyProvider provider, BarData data)
			: base(provider)
		{
			_data = data;
		}
	}
}
