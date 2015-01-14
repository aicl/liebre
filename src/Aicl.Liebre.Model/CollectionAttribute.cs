using System;

namespace Aicl.Liebre.Model
{
	public class CollectionAttribute:Attribute
	{
		public Type CollectionType { get; set;}

		public CollectionAttribute (Type collection)
		{
			CollectionType = collection;
		}
	}
}

