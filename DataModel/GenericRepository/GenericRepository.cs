using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace DataModel.GenericRepository
{
	public class GenericRepository<TEntity> where TEntity:class
	{
		#region private members

		internal WebApiDbEntities Context;
		internal DbSet<TEntity> DbSet;

		#endregion

		public GenericRepository(WebApiDbEntities context)
		{
			this.Context = context;
			this.DbSet = context.Set<TEntity>();
		}

		#region public members

		public virtual IEnumerable<TEntity> Get()
		{
			IQueryable<TEntity> query = DbSet;
			return query.ToList();
		}

		public virtual TEntity GetById(object id)
		{
			return DbSet.Find(id);
		}

		public virtual void Insert(TEntity entity)
		{
			DbSet.Add(entity);
		}

		public virtual void Delete(object id)
		{
			TEntity entityToDelete = DbSet.Find(id);
			Delete(entityToDelete);
		}

		public virtual void Delete(TEntity entityToDelete)
		{
			if (Context.Entry(entityToDelete).State == EntityState.Detached)
				DbSet.Attach(entityToDelete);

			DbSet.Remove(entityToDelete);
		}

		public virtual void Update(TEntity entityToUpdate)
		{
			DbSet.Attach(entityToUpdate);
			Context.Entry(entityToUpdate).State = EntityState.Modified;
		}

		public virtual IEnumerable<TEntity> GetMany(Func<TEntity, bool> where)
		{
			return DbSet.Where(where).ToList();
		}

		public virtual IQueryable<TEntity> GetManyQueryable(Func<TEntity, bool> where)
		{
			return DbSet.Where(where).AsQueryable();
		}

		public TEntity Get(Func<TEntity, bool> where)
		{
			return DbSet.Where(where).FirstOrDefault<TEntity>();
		}

		public void Delete(Func<TEntity, bool> where)
		{
			IQueryable<TEntity> objects = DbSet.Where<TEntity>(where).AsQueryable();

			foreach (TEntity obj in objects)
			{
				DbSet.Remove(obj);
			}
		}

		public virtual IEnumerable<TEntity> GetAll()
		{
			return DbSet.ToList();
		}

		public IQueryable<TEntity> GetWithInclude(Expression<Func<TEntity, bool>> predicate, params string[] include)
		{
			IQueryable<TEntity> query = this.DbSet;
			query = include.Aggregate(query, (current, inc) => current.Include(inc));

			return query.Where(predicate);
		}

		public bool Exists(object primaryKey)
		{
			return DbSet.Find(primaryKey) != null;
		}

		public TEntity GetSingle(Func<TEntity, bool> predicate)
		{
			return DbSet.Single<TEntity>(predicate);
		}

		public TEntity GetFirst(Func<TEntity, bool> predicate)
		{
			return DbSet.First<TEntity>(predicate);
		}

		#endregion
	}
}
