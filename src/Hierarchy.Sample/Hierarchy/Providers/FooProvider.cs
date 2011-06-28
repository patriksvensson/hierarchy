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
using Hierarchy.Sample.Properties;
using System.Drawing;

namespace Hierarchy.Sample
{
	public class FooProvider : HierarchyProvider<BarDataRepository>
	{
		public override IEnumerable<HierarchyNode> GetChildren(BarDataRepository context, HierarchyNode node)
		{
			if (node == null)
			{
				yield return new FooNode(this);
			}
		}

		public override bool HasChildren(BarDataRepository context, HierarchyNode node)
		{
			return node == null;
		}

		public override IEnumerable<KeyValuePair<string, Image>> GetImages()
		{
			yield return new KeyValuePair<string, Image>(typeof(FooNode).FullName, Resources.Red);
		}
	}
}
