﻿using System;
using System.Collections.Generic;
using System.Linq;
using DataModel;
using DataModel.GenericRepository;
using DataModel.UnitOfWork;
using Moq;
using NUnit.Framework;
using TestsHelper;

namespace BusinessServices.Tests
{
	public class TokenServicesTest
	{
		#region variables

		private ITokenServices _tokenServices;
		private IUnitOfWork _unitOfWork;
		private List<Token> _tokens;
		private GenericRepository<Token> _tokenRepository;
		private WebApiDbEntities _dbEntities;
		private const string SampleAuthToken = "9f907bdf-f6de-425d-be5b-b4852eb77761";

		#endregion

		[TestFixtureSetUp]
		public void Setup()
		{
			_tokens = SetUpTokens();
		}

		[SetUp]
		public void ReInitializeTest()
		{
			_dbEntities = new Mock<WebApiDbEntities>().Object;
			_tokenRepository = SetUpTokenRepository();
			var unitOfWork = new Mock<IUnitOfWork>();
			unitOfWork.SetupGet(s => s.TokenRepository).Returns(_tokenRepository);
			_unitOfWork = unitOfWork.Object;
			_tokenServices = new TokenServices(_unitOfWork);
		}

		private GenericRepository<Token> SetUpTokenRepository()
		{
			// Initialise repository
			var mockRepo = new Mock<GenericRepository<Token>>(MockBehavior.Default, _dbEntities);

			// Setup mocking behavior
			mockRepo.Setup(p => p.GetAll()).Returns(_tokens);

			mockRepo.Setup(p => p.GetById(It.IsAny<int>()))
				.Returns(new Func<int, Token>(
							 id => _tokens.Find(p => p.TokenId.Equals(id))));

			mockRepo.Setup(p => p.GetById(It.IsAny<string>()))
			   .Returns(new Func<string, Token>(
							authToken => _tokens.Find(p => p.AuthToken.Equals(authToken))));

			mockRepo.Setup(p => p.Insert((It.IsAny<Token>())))
				.Callback(new Action<Token>(newToken =>
				{
					dynamic maxTokenID = _tokens.Last().TokenId;
					dynamic nextTokenID = maxTokenID + 1;
					newToken.TokenId = nextTokenID;
					_tokens.Add(newToken);
				}));

			mockRepo.Setup(p => p.Update(It.IsAny<Token>()))
				.Callback(new Action<Token>(token =>
				{
					var oldToken = _tokens.Find(a => a.TokenId == token.TokenId);
					oldToken = token;
				}));

			mockRepo.Setup(p => p.Delete(It.IsAny<Token>()))
				.Callback(new Action<Token>(prod =>
				{
					var tokenToRemove =
						_tokens.Find(a => a.TokenId == prod.TokenId);

					if (tokenToRemove != null)
						_tokens.Remove(tokenToRemove);
				}));
			//Create setup for other methods too. note non virtauls methods can not be set up

			// Return mock implementation object
			return mockRepo.Object;
		}

		private List<Token> SetUpTokens()
		{
			var tokId = new int();
			var tokens = DataInitializer.GetAllTokens();
			foreach (Token tok in tokens)
				tok.TokenId = ++tokId;
			return tokens;
		}

		[Test]
		public void GenerateTokenByUserIdTest()
		{
			const int userId = 1;
			var maxTokenIdBeforeAdd = _tokens.Max(a => a.TokenId);
			var tokenEntity = _tokenServices.GenerateToken(userId);
			var newTokenDataModel = new Token()
			{
				AuthToken = tokenEntity.AuthToken,
				TokenId = maxTokenIdBeforeAdd + 1,
				ExpiresOn = tokenEntity.ExpiresOn,
				IssuedOn = tokenEntity.IssuedOn,
				UserId = tokenEntity.UserId
			};
			AssertObjects.PropertyValuesAreEquals(newTokenDataModel, _tokens.Last());
		}

		[Test]
		public void ValidateTokenWithRightAuthToken()
		{
			var authToken = Convert.ToString(SampleAuthToken);
			var validationResult = _tokenServices.ValidateToken(authToken);
			Assert.That(validationResult, Is.EqualTo(true));
		}

		[Test]
		public void ValidateTokenWithWrongAuthToken()
		{
			var authToken = Convert.ToString("xyz");
			var validationResult = _tokenServices.ValidateToken(authToken);
			Assert.That(validationResult, Is.EqualTo(false));
		}

		[TearDown]
		public void DisposeTest()
		{
			_tokenServices = null;
			_unitOfWork = null;
			_tokenRepository = null;
			if (_dbEntities != null)
				_dbEntities.Dispose();
		}

		[TestFixtureTearDown]
		public void DisposeAllObjects()
		{
			_tokens = null;
		}
	}
}
