using System;
using System.Collections.Generic;

namespace MFilesAPI.Fakes
{
	public class FakeFactory
	{
		protected Dictionary<Type, Func<object>> TypeDictionary { get; } = new Dictionary<Type, Func<object>>();

		public FakeFactory Register<TInterface, TConcrete>()
			where TConcrete : TInterface, new()
		{
			this.TypeDictionary.Add(typeof(TInterface), () => new TConcrete());
			return this;
		}
		public FakeFactory Register<TInterface, TConcrete>(Func<TConcrete> instantiation)
		{
			this.TypeDictionary.Add(typeof(TInterface), () => instantiation());
			return this;
		}
		public TInterface Instantiate<TInterface>()
		{
			// If they asked for a Vault then give them an IVaultEx.
			var requestedType = typeof(TInterface);
			if (requestedType == typeof(MFilesAPI.Vault))
				requestedType = typeof(IVaultEx);

			if (false == this.TypeDictionary.ContainsKey(requestedType))
				throw new InvalidOperationException($"Type {requestedType.FullName} not registered with fake factory.");

			var o = this.TypeDictionary[requestedType]();
			if (o == null)
				return default;
			if (false == (o is TInterface))
				throw new InvalidOperationException($"Cannot instantiate type {requestedType.FullName} as the factory method returned a {o.GetType().FullName}.");
			return (TInterface)o;
		}
		public bool HasRegistration<TInterface>() => this.TypeDictionary.ContainsKey(typeof(TInterface));
		public bool TryInstantiate<TInterface>(out TInterface item)
		{
			if (this.HasRegistration<TInterface>())
			{
				item = this.Instantiate<TInterface>();
				return true;
			}
			item = default;
			return false;
		}

		
		/// <summary>
		/// A default factory using in-memory collections.
		/// </summary>
		public static readonly FakeFactory Default = new FakeFactory()
			.Register<IVaultEx, Vault>()
			.Register<MFilesAPI.VaultObjectTypeOperations, VaultObjectTypeOperations>()
			.Register<MFilesAPI.VaultClassOperations, VaultClassOperations>()
			.Register<MFilesAPI.VaultPropertyDefOperations, VaultPropertyDefOperations>()
			.Register<MFilesAPI.SessionInfo, SessionInfo>(() => SessionInfo.CreateDefault());
	}
}
