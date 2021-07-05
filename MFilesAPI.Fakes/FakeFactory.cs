using System;
using System.Collections.Generic;

namespace MFilesAPI.Fakes
{
	public class FakeFactory
	{
		protected Dictionary<Type, Func<object>> TypeDictionary { get; } = new Dictionary<Type, Func<object>>();

		public void Register<TInterface, TConcrete>()
			where TConcrete : TInterface, new()
		{
			this.TypeDictionary.Add(typeof(TInterface), () => new TConcrete());
		}
		public void Register<TInterface, TConcrete>(Func<TConcrete> instantiation)
		{
			this.TypeDictionary.Add(typeof(TInterface), () => instantiation);
		}
		public TInterface Instantiate<TInterface>()
		{
			var o = this.TypeDictionary[typeof(TInterface)]();
			if (o == null)
				return default;
			if (false == (o is TInterface))
				throw new InvalidOperationException($"Cannot instantiate type {typeof(TInterface).FullName} as the factory method returned a {o.GetType().FullName}.");
			return (TInterface)o;
		}
		public bool HasRegistration<TInterface>() => this.TypeDictionary.ContainsKey(typeof(TInterface));

		/// <summary>
		/// Creates a default in-memory factory.
		/// </summary>
		/// <returns>The factory.</returns>
		public static FakeFactory CreateInMemoryFactory()
		{
			var factory = new FakeFactory();
			factory.Register<MFilesAPI.VaultObjectTypeOperations, VaultObjectTypeOperations>();
			factory.Register<MFilesAPI.VaultClassOperations, VaultClassOperations>();
			factory.Register<MFilesAPI.VaultPropertyDefOperations, VaultPropertyDefOperations>();
			factory.Register<MFilesAPI.SessionInfo, SessionInfo>(() => SessionInfo.CreateDefault());
			return factory;
		}
	}
}
