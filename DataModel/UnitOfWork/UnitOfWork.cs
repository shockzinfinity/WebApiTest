using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataModel.GenericRepository;

namespace DataModel.UnitOfWork
{
	public class UnitOfWork : IDisposable
	{
		#region private members

		private WebApiDbEntities _context = null;
		private GenericRepository<User> _userRepository;
		private GenericRepository<Product> _productRepository;
		private GenericRepository<Token> _tokenRepository;

		#endregion

		#region public properties

		public GenericRepository<User> UserRepository
		{
			get
			{
				if (_userRepository == null)
					_userRepository = new GenericRepository<User>(_context);

				return _userRepository;
			}
		}

		public GenericRepository<Product> ProductRepository
		{
			get
			{
				if (_productRepository == null)
					_productRepository = new GenericRepository<Product>(_context);

				return _productRepository;
			}
		}

		public GenericRepository<Token> TokenRepository
		{
			get
			{
				if (_tokenRepository == null)
					_tokenRepository = new GenericRepository<Token>(_context);

				return _tokenRepository;
			}
		}

		#endregion

		public UnitOfWork()
		{
			_context = new WebApiDbEntities();
		}

		#region public methods

		public void Save()
		{
			try
			{
				_context.SaveChanges();
			}
			catch (DbEntityValidationException e)
			{
				var outputLines = new List<string>();
				foreach (var eve in e.EntityValidationErrors)
				{
					outputLines.Add(string.Format("{0}: Entity of type \"{1}\" in state \"{2}\" has the following validation errors:", DateTime.Now, eve.Entry.Entity.GetType().Name, eve.Entry.State));

					foreach (var ve in eve.ValidationErrors)
					{
						outputLines.Add(string.Format("- Property: \"{0}\", Error: \"{1}\"", ve.PropertyName, ve.ErrorMessage));
					}
				}

				File.AppendAllLines(Environment.CurrentDirectory + @"errors.txt", outputLines);

				throw e;
			}
		}

		#endregion

		#region IDisposable implementation

		private bool _disposed = false;

		protected virtual void Dispose(bool disposing)
		{
			if (!this._disposed)
			{
				if (disposing)
				{
					Debug.WriteLine("UnitOfWork is being disposed.");
					_context.Dispose();
				}
			}

			this._disposed = true;
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		#endregion
	}
}
