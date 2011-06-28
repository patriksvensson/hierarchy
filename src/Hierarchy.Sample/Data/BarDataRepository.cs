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

namespace Hierarchy.Sample
{
	public class BarDataRepository
	{
		private HashSet<BarData> _bars;
		private int _nextId;

		public BarDataRepository()
		{
			_bars = new HashSet<BarData>();
			_nextId = 1;

			this.AddBar("Bar One");
			this.AddBar("Bar Two");
			this.AddBar("Bar Three");
		}

		public IEnumerable<BarData> GetBars()
		{
			return _bars;
		}

		public void AddBar(string name)
		{
			_bars.Add(new BarData(_nextId, name));
			_nextId++;
		}

		public void DeleteBar(int id)
		{
			_bars.RemoveWhere(x => x.Id == id);
		}

		public void RenameBar(int id, string name)
		{
			var data = _bars.FirstOrDefault(x => x.Id == id);
			if (data != null)
			{
				data.Name = name;
			}
		}
	}
}
