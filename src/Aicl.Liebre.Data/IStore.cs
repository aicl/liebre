using System;
using System.Linq.Expressions;
using Aicl.Liebre.Model;
using MongoDB.Driver;

namespace Aicl.Liebre.Data
{
	public interface IStore
	{
		T Single<T> (IMongoQuery query) where T: class, new();
		bool Exists<T> (IMongoQuery query) where T: class;
	}
}

