using BusinessEntities;

namespace BusinessServices
{
	public interface ITokenServices
	{
		TokenEntity GenerateToken(int userId);
		bool ValidateToken(string tokenId);
		bool Kill(string tokenId);
		bool DeleteByUserId(int userId);
	}
}
