using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using AutoMapper;
using BusinessEntities;
using DataModel;
using DataModel.UnitOfWork;

namespace BusinessServices
{
	public class ProductServices : IProductServices
	{
		private readonly IUnitOfWork _unitOfWork;

		public ProductServices(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		#region IProductServices implementation

		public int CreateProduct(ProductEntity productEntity)
		{
			using (var scope = new TransactionScope())
			{
				var product = new Product
				{
					ProductName = productEntity.ProductName
				};
				_unitOfWork.ProductRepository.Insert(product);
				_unitOfWork.Save();
				scope.Complete();
				return product.ProductId;
			}
		}

		public bool DeleteProduct(int productId)
		{
			var success = false;
			if (productId > 0)
			{
				using (var scope = new TransactionScope())
				{
					var product = _unitOfWork.ProductRepository.GetById(productId);
					if (product != null)
					{
						_unitOfWork.ProductRepository.Delete(productId);
						_unitOfWork.Save();
						scope.Complete();
						success = true;
					}
				}
			}

			return success;
		}

		public IEnumerable<ProductEntity> GetAllProducts()
		{
			var products = _unitOfWork.ProductRepository.GetAll().ToList();
			if (products.Any())
			{
				Mapper.CreateMap<Product, ProductEntity>();
				var productsModel = Mapper.Map<List<Product>, List<ProductEntity>>(products);
				return productsModel;
			}
			return null;
		}

		public ProductEntity GetProductById(int productId)
		{
			var product = _unitOfWork.ProductRepository.GetById(productId);
			if (product != null)
			{
				Mapper.CreateMap<Product, ProductEntity>();
				var productModel = Mapper.Map<Product, ProductEntity>(product);
				return productModel;
			}
			return null;
		}

		public bool UpdateProduct(int productId, ProductEntity productEntity)
		{
			var success = false;
			if (productEntity != null)
			{
				using (var scope = new TransactionScope())
				{
					var product = _unitOfWork.ProductRepository.GetById(productId);
					if (product != null)
					{
						product.ProductName = productEntity.ProductName;
						_unitOfWork.ProductRepository.Update(product);
						_unitOfWork.Save();
						scope.Complete();
						success = true;
					}
				}
			}

			return success;
		}

		#endregion
	}
}
