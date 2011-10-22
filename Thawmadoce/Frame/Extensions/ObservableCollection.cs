﻿using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Thawmadoce.Frame.Extensions
{
	public static class ObservableCollection
	{
		public static void AddRange<T>(this ObservableCollection<T> collection, IEnumerable<T> items)
		{
			foreach (var item in items)
			{
				collection.Add(item);
			}
		}
	}
}
