using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using BusinessServices;
using DataModel;
using DataModel.GenericRepository;
using DataModel.UnitOfWork;
using Moq;
using NUnit.Framework;
using TestsHelper;

namespace ApiController.Tests
{
	[TestFixture]
	public class ProductControllerTest
	{
		#region private variables

		private IProductServices _productServices;
		private ITokenServices _tokenServices;
		private IUnitOfWork _unitOfWork;
		private List<Product> _products;
		private List<Token> _tokens;
		private GenericRepository<Product> _productRepository;
		private GenericRepository<Token> _tokenRepository;
		private WebApiDbEntities _dbEntities;
		private HttpClient _client;

		private HttpResponseMessage _response;
		private string _token;
		private const string ServiceBaseUrl = "http://localhost:25866";

		#endregion

		[TestFixtureSetUp]
		public void Setup()
		{
			_products = SetUpProducts();
			_tokens = SetUpTokens();
			_dbEntities = new Mock<WebApiDbEntities>().Object;
			_tokenRepository = SetUpTokenRepository();
			_productRepository = SetUpProductRepository();
			var unitOfWork = new Mock<IUnitOfWork>();
			unitOfWork.SetupGet(s => s.ProductRepository).Returns(_productRepository);
			unitOfWork.Setup(s => s.TokenRepository).Returns(_tokenRepository);
			_unitOfWork = unitOfWork.Object;
			_productServices = new ProductServices(_unitOfWork);
			_tokenServices = new TokenServices(_unitOfWork);
			_client = new HttpClient { BaseAddress = new Uri(ServiceBaseUrl) };
			var tokenEntity = _tokenServices.GenerateToken(1);
			_token = tokenEntity.AuthToken;
			_client.DefaultRequestHeaders.Add("Token", _token);
		}

		private GenericRepository<Product> SetUpProductRepository()
		{
			throw new NotImplementedException();
		}

		private GenericRepository<Token> SetUpTokenRepository()
		{
			throw new NotImplementedException();
		}

		private static List<Token> SetUpTokens()
		{
			var tokenId = new int();
			var tokens = DataInitializer.GetAllTokens();
			foreach (Token item in tokens)
			{
				item.TokenId = ++tokenId;
			}

			return tokens;
		}

		private static List<Product> SetUpProducts()
		{
			var productId = new int();
			var products = DataInitializer.GetAllProducts();
			foreach (Product item in products)
			{
				item.ProductId = ++productId;
			}

			return products;
		}

		[SetUp]
		public void ReInitializeTest()
		{
			_client = new HttpClient { BaseAddress = new Uri(ServiceBaseUrl) };
			_client.DefaultRequestHeaders.Add("Token", _token);
		}

		[TestFixtureTearDown]
		public void DisposeAllObjects()
		{
			_tokenServices = null;
			_productServices = null;
			_unitOfWork = null;
			_tokenRepository = null;
			_productRepository = null;
			_tokens = null;
			_products = null;
			if (_response != null) _response.Dispose();
			if (_client != null) _client.Dispose();
		}
	}
}
