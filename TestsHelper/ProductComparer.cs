using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataModel;

namespace TestsHelper
{
	public class ProductComparer : IComparer, IComparer<Product>
	{
		#region IComparer implementation

		public int Compare(Product expected, Product actual)
		{
			int temp;
			return (temp = expected.ProductId.CompareTo(actual.ProductId)) != 0 ? temp : expected.ProductName.CompareTo(actual.ProductName);
		}

		#endregion

		#region IComparer<T> implementation

		public int Compare(object expected, object actual)
		{
			var lhs = expected as Product;
			var rhs = actual as Product;
			if (lhs == null || rhs == null) throw new InvalidOperationException();
			return Compare(lhs, rhs);
		}

		#endregion
	}
}
