using DataModel.GenericRepository;

namespace DataModel.UnitOfWork
{
	public interface IUnitOfWork
	{
		#region properties

		GenericRepository<Product> ProductRepository { get; }
		GenericRepository<User> UserRepository { get; }
		GenericRepository<Token> TokenRepository { get; }

		#endregion
		void Save();
	}
}
