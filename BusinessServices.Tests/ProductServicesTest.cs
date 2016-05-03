using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using AutoMapper;
using BusinessEntities;
using DataModel;
using DataModel.GenericRepository;
using DataModel.UnitOfWork;
using Moq;
using NUnit.Framework;
using TestsHelper;

namespace BusinessServices.Tests
{
	[TestFixture]
	public class ProductServicesTest
	{
		#region variables

		private IProductServices _productServices;
		private IUnitOfWork _unitOfWork;
		private List<Product> _products;
		private GenericRepository<Product> _productRepository;
		private WebApiDbEntities _dbEntities;

		#endregion

		[TestFixtureSetUp]
		public void Setup()
		{
			_products = SetUpProducts();
		}

		private static List<Product> SetUpProducts()
		{
			var productId = new int();
			var products = DataInitializer.GetAllProducts();
			foreach (Product product in products)
			{
				product.ProductId = ++productId;
			}

			return products;
		}

		[SetUp]
		public void ReInitializeTest()
		{
			_products = SetUpProducts();
			_dbEntities = new Mock<WebApiDbEntities>().Object;
			_productRepository = SetUpProductRepository();
			var unitOfWork = new Mock<IUnitOfWork>();
			unitOfWork.SetupGet(s => s.ProductRepository).Returns(_productRepository);
			_unitOfWork = unitOfWork.Object;
			_productServices = new ProductServices(_unitOfWork);
		}

		private GenericRepository<Product> SetUpProductRepository()
		{
			// initialize repository
			var mockRepo = new Mock<GenericRepository<Product>>(MockBehavior.Default, _dbEntities);

			// setup moking behavior
			mockRepo.Setup(p => p.GetAll()).Returns(_products);

			mockRepo.Setup(p => p.GetById(It.IsAny<int>()))
				.Returns(new Func<int, Product>(
					id => _products.Find(p => p.ProductId.Equals(id))));

			mockRepo.Setup(p => p.Insert(It.IsAny<Product>()))
				.Callback(new Action<Product>(newProduct =>
														{
															dynamic maxProductId = _products.Last().ProductId;
															dynamic nextProductId = maxProductId + 1;
															newProduct.ProductId = nextProductId;
															_products.Add(newProduct);
														}));

			mockRepo.Setup(p => p.Update(It.IsAny<Product>()))
				.Callback(new Action<Product>(prod =>
												{
													var oldProduct = _products.Find(a => a.ProductId == prod.ProductId);
													oldProduct = prod;
												}));

			mockRepo.Setup(p => p.Delete(It.IsAny<Product>()))
				.Callback(new Action<Product>(prod =>
												{
													var productToRemove =
														_products.Find(a => a.ProductId == prod.ProductId);

													if (productToRemove != null)
														_products.Remove(productToRemove);
												}));

			// return mock implementation object
			return mockRepo.Object;
		}

		[Test]
		public void GetAllProductsTest()
		{
			var products = _productServices.GetAllProducts();
			var productList = products.Select(productEntity => new Product { ProductId = productEntity.ProductId, ProductName = productEntity.ProductName }).ToList();
			var comparer = new ProductComparer();
			CollectionAssert.AreEqual(productList.OrderBy(product => product, comparer), _products.OrderBy(product => product, comparer), comparer);
		}

		[Test]
		public void GetAllProductsTestForNull()
		{
			_products.Clear();
			var products = _productServices.GetAllProducts();
			Assert.Null(products);
			SetUpProducts();
		}

		[Test]
		public void GetProductByRightIdTest()
		{
			var mobileProduct = _productServices.GetProductById(2);
			if (mobileProduct != null)
			{
				Mapper.CreateMap<ProductEntity, Product>();
				var productModel = Mapper.Map<ProductEntity, Product>(mobileProduct);
				AssertObjects.PropertyValuesAreEquals(productModel, _products.Find(a => a.ProductName.Contains("Mobile")));
			}
		}

		[Test]
		public void GetProductByWrongIdTest()
		{
			var product = _productServices.GetProductById(0);
			Assert.Null(product);
		}

		[Test]
		public void AddNewProductTest()
		{
			var newProduct = new ProductEntity()
			{
				ProductName = "Android Phone"
			};

			var maxProductIdBeforeAdd = _products.Max(x => x.ProductId);
			newProduct.ProductId = maxProductIdBeforeAdd + 1;
			_productServices.CreateProduct(newProduct);
			var addedProduct = new Product() { ProductName = newProduct.ProductName, ProductId = newProduct.ProductId };
			AssertObjects.PropertyValuesAreEquals(addedProduct, _products.Last());
			Assert.That(maxProductIdBeforeAdd + 1, Is.EqualTo(_products.Last().ProductId));
		}

		[Test]
		public void UpdateProductTest()
		{
			var firstProduct = _products.First();
			firstProduct.ProductName = "Laptop updated";
			var updateProduct = new ProductEntity()
			{
				ProductName = firstProduct.ProductName,
				ProductId = firstProduct.ProductId
			};
			_productServices.UpdateProduct(firstProduct.ProductId, updateProduct);
			Assert.That(firstProduct.ProductId, Is.EqualTo(1)); // hasn't changed.
			Assert.That(firstProduct.ProductName, Is.EqualTo("Laptop updated")); // product name changed.
		}

		[Test]
		public void DeleteProductTest()
		{
			int maxId = _products.Max(a => a.ProductId); // before removal
			var lastProduct = _products.Last();

			// remove last product
			_productServices.DeleteProduct(lastProduct.ProductId);
			Assert.That(maxId, Is.GreaterThan(_products.Max(a => a.ProductId))); // max id reduced by 1
		}

		[TearDown]
		public void DisposeTest()
		{
			_productServices = null;
			_unitOfWork = null;
			_productRepository = null;
			if (_dbEntities != null) _dbEntities.Dispose();
			_products = null;
		}

		[TestFixtureTearDown]
		public void DisposeAllObjects()
		{
			_products = null;
		}
	}
}
