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

namespace Hierarchy
{
	public class HierarchyImageListMapper
	{
		private readonly Dictionary<string, int> m_dictionary;
		private readonly ImageList m_imageList;
		private string m_defaultKey;

		public ImageList ImageList
		{
			get { return m_imageList; }
		}

		public HierarchyImageListMapper()
		{
			m_dictionary = new Dictionary<string, int>();
			m_imageList = new ImageList();
			m_imageList.ColorDepth = ColorDepth.Depth24Bit;
			m_defaultKey = string.Empty;
		}

		public void SetDefaultKey(string key, Image image)
		{
			this.AddImage(key, image);
			m_defaultKey = key;
		}

		public void AddImage(string key, Image image)
		{
			m_imageList.Images.Add(key, image);
			int index = m_imageList.Images.IndexOfKey(key);
			if (m_dictionary.ContainsKey(key))
			{
				m_dictionary.Remove(key);
			}
			m_dictionary.Add(key, index);
		}

		public void RemoveImage(string key)
		{
			if (!m_dictionary.ContainsKey(key))
			{
				return;
			}
			int index = m_dictionary[key];
			m_imageList.Images.RemoveAt(index);
		}

		public int GetImageIndex(string key)
		{
			if (!m_dictionary.ContainsKey(key))
			{
				if (string.IsNullOrEmpty(m_defaultKey))
				{
					return -1;
				}
				if (!m_dictionary.ContainsKey(m_defaultKey))
				{
					return -1;
				}
				key = m_defaultKey;
			}
			int index = m_dictionary[key];
			return index;
		}
	}
}
