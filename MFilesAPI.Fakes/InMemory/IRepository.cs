using System;
using System.Collections.Concurrent;
using System.Linq;

namespace MFilesAPI.Fakes.InMemory
{
	/// <summary>
	/// Represents a collection of vault structural content.
	/// </summary>
	/// <typeparam name="TKey">The type of key used to identify an item in the collection.</typeparam>
	/// <typeparam name="TAdminType">The type of the "admin" version of the object (e.g. "ObjTypeAdmin").</typeparam>
	/// <typeparam name="TStandardType">The type of the non-admin version of the object (e.g. "ObjType").</typeparam>
	public interface IRepository<TKey, TAdminType, TStandardType>
	{
		/// <summary>
		/// Retrieves a specific administrative-type vault structural element from the collection.
		/// </summary>
		/// <param name="key">The key for the item to return.</param>
		/// <returns>The item.</returns>
		TAdminType GetAdminInstance(TKey key);

		/// <summary>
		/// Retrieves a specific non-administrative-type vault structural element from the collection.
		/// </summary>
		/// <param name="key">The key for the item to return.</param>
		/// <returns>The item.</returns>
		TStandardType GetStandardInstance(TKey key);
	}

	/// <summary>
	/// Implementation of <see cref="IRepository{TKey, TAdminType, TStandardType}"/>, using an in-memory
	/// dictionary as the data source.
	/// </summary>
	/// <typeparam name="TKey">The type of key used to identify an item in the collection.</typeparam>
	/// <typeparam name="TAdminType">The type of the "admin" version of the object (e.g. "ObjTypeAdmin").</typeparam>
	/// <typeparam name="TStandardType">The type of the non-admin version of the object (e.g. "ObjType").</typeparam>
	public abstract class RepositoryBase<TKey, TAdminType, TStandardType>
		: ConcurrentDictionary<TKey, TAdminType>, IRepository<TKey, TAdminType, TStandardType>
	{
		/// <summary>
		/// The method used to convert an admin-type to a non-admin-type.
		/// </summary>
		protected Func<TAdminType, TStandardType> Convert { get; private set; }

		/// <summary>
		/// Creates the repository base.
		/// </summary>
		/// <param name="convert"></param>
		protected RepositoryBase(Func<TAdminType, TStandardType> convert)
		{
			this.Convert = convert ?? throw new ArgumentNullException(nameof(convert));
		}

		/// <inheritdoc />
		public TAdminType GetAdminInstance(TKey key)
		{
			return this.ContainsKey(key)
				? this[key]
				: default;
		}

		/// <inheritdoc />
		public TStandardType GetStandardInstance(TKey key)
		{
			var adminInstance = this.GetAdminInstance(key);
			return null != adminInstance
				? this.Convert(adminInstance)
				: default;
		}
	}

	/// <summary>
	/// An implementation of <see cref="RepositoryBase{TKey, TAdminType, TStandardType}"/>,
	/// using <see cref="int"/> as the key type.
	/// </summary>
	/// <typeparam name="TAdminType">The type of the "admin" version of the object (e.g. "ObjTypeAdmin").</typeparam>
	/// <typeparam name="TStandardType">The type of the non-admin version of the object (e.g. "ObjType").</typeparam>
	public abstract class RepositoryBase<TAdminType, TStandardType>
		: RepositoryBase<int, TAdminType, TStandardType>
	{
		protected int ItemCounter { get; set; } = 0;
		protected RepositoryBase(Func<TAdminType, TStandardType> convert) : base(convert)
		{
		}

		/// <summary>
		/// Adds <paramref name="item"/> to the collection.
		/// Assigns a new ID.
		/// </summary>
		/// <param name="item">The item to add.</param>
		/// <returns>The ID of the item that was added.</returns>
		public int Add(TAdminType item)
		{
			var id = ++this.ItemCounter;
			this.Add(id, item);
			return id;
		}

		/// <summary>
		/// Adds <paramref name="item"/> to the collection with key <paramref name="id"/>.
		/// </summary>
		/// <param name="id">The ID/key for the item.</param>
		/// <param name="item">The item to add.</param>
		/// <returns>The ID of the item that was added.</returns>
		/// <remarks>Replaces an item if it's found with the same key.</remarks>
		public int Add(int id, TAdminType item)
		{
			if (id > this.ItemCounter)
				this.ItemCounter = id;
			this.AddOrUpdate(id, (i) =>
			{
				return item;
			}, (i, a) =>
			{
				return item;
			});
			return id;
		}

		/// <summary>
		/// Updates an item with key <paramref name="id"/>.
		/// </summary>
		/// <param name="id">The key of the item</param>
		/// <param name="item">The item to add.</param>
		/// <remarks>Adds the item if it is not found.</remarks>
		public  void Update(int id, TAdminType item)
		{
			if (id > this.ItemCounter)
				this.ItemCounter = id;
			this.AddOrUpdate(id, (i) =>
			{
				return item;
			}, (i, a) =>
			{
				return item;
			});
		}
	}
}
