using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel.UnitOfWork
{
	public interface IUnitOfWork
	{
		void Save();
	}
}
