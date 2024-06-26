﻿using System.Collections.Generic;

namespace Coditech.Utilities.Filters
{
    public class FilterDataCollection : List<FilterDataTuple>
	{
		public void Add(string filterName, string filterOperator, string filterValue) => Add(new FilterDataTuple(filterName, filterOperator, filterValue));
	}
}
