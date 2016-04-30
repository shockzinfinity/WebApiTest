using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BusinessEntities;
using BusinessServices;
using WebApi.ActionFilters;
using WebApi.ErrorHelper;
using WebApi.Filters;

namespace WebApi.Controllers
{
	//[BasicAuthenticator] // Basic Authentication 에서 Token 으로 변경
	[AuthorizationRequired]
	[RoutePrefix("v1/Products/Product")]
	public class ProductController : ApiController
	{
		private readonly IProductServices _productServices;

		public ProductController(IProductServices productServices)
		{
			_productServices = productServices;
		}

		// GET: api/Product
		[HttpGet]
		[Route("allproducts")]
		[Route("all")]
		public HttpResponseMessage Get()
		{
			var products = _productServices.GetAllProducts();
			var productEntities = products as List<ProductEntity> ?? products.ToList();
			if (productEntities.Any())
				return Request.CreateResponse(HttpStatusCode.OK, productEntities);
			//return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Products not found.");
			throw new ApiDataException(1000, "Products not found.", HttpStatusCode.NotFound);
		}

		// GET: api/Product/5
		[HttpGet]
		[Route("productid/{id?}")]
		[Route("particularproduct/{id?}")]
		[Route("myproduct/{id:range(1, 3)}")]
		public HttpResponseMessage Get(int id)
		{
			if(id != null)
			{
				var product = _productServices.GetProductById(id);
				if (product != null)
					return Request.CreateResponse(HttpStatusCode.OK, product);

				throw new ApiDataException(1001, "No product found for this id.", HttpStatusCode.NotFound);
			}

			throw new ApiException() { ErrorCode = (int)HttpStatusCode.BadRequest, ErrorDescription = "Bad Request..." };
		}

		// POST: api/Product
		[HttpPost]
		[Route("Create")]
		[Route("Register")]
		public int Post([FromBody]ProductEntity productEntity)
		{
			return _productServices.CreateProduct(productEntity);
		}

		// PUT: api/Product/5
		[HttpPut]
		[Route("Update/productid/{id}")]
		[Route("Modify/productid/{id}")]
		public bool Put(int id, [FromBody]ProductEntity productEntity)
		{
			if (id > 0)
			{
				return _productServices.UpdateProduct(id, productEntity);
			}
			return false;
		}

		// DELETE: api/Product/5
		[HttpDelete]
		[Route("remove/productid/{id}")]
		[Route("clear/productid/{id}")]
		public bool Delete(int id)
		{
			if (id != null && id > 0)
			{
				var isSuccess = _productServices.DeleteProduct(id);
				if (isSuccess) return isSuccess;

				throw new ApiDataException(1002, "Product is already deleted or not exist in system.", HttpStatusCode.NoContent);
			}
			throw new ApiException() { ErrorCode = (int)HttpStatusCode.BadRequest, ErrorDescription = "Bad Request..." };
		}

		[HttpPut]
		[Route("delete/productid/{id}")]
		public bool PutDelete(int id)
		{
			if (id != null && id > 0)
			{
				var isSuccess = _productServices.DeleteProduct(id);
				if (isSuccess) return isSuccess;

				throw new ApiDataException(1002, "Product is already deleted or not exist in system.", HttpStatusCode.NoContent);
			}
			throw new ApiException() { ErrorCode = (int)HttpStatusCode.BadRequest, ErrorDescription = "Bad Request..." };
		}
	}
}
