using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataModel.UnitOfWork;

namespace BusinessServices
{
	public class UserServices : IUserServices
	{
		private readonly UnitOfWork _unitOfWork;

		public UserServices(UnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		#region IUserServices implementation

		public int Authentication(string userName, string password)
		{
			var user = _unitOfWork.UserRepository.Get(u => u.UserName == userName && u.Password == password);

			if (user != null && user.UserId > 0)
			{
				return user.UserId;
			}

			return 0;
		}

		#endregion
	}
}
