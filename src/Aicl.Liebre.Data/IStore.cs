using System;
using System.Linq.Expressions;
using Aicl.Liebre.Model;
using MongoDB.Driver;

namespace Aicl.Liebre.Data
{
	public interface IStore
	{
		T Single<T> (Expression<Func<T, bool>> predicate) where T:class, IDocument, new();
		bool Exists<T> (IMongoQuery query) where T: class;
	}
}

