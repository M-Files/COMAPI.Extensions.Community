using System;
using System.Collections.Generic;

namespace MFilesAPI.Fakes
{
	public class FakeFactory
	{
		protected Dictionary<Type, Func<object>> TypeDictionary { get; } = new Dictionary<Type, Func<object>>();

		public FakeFactory Register<TClass>()
			where TClass : new()
		{
			return this.Register(() => new TClass());
		}
		public FakeFactory Register<TClass>(Func<TClass> instantiation)
		{
			this.TypeDictionary.Add(typeof(TClass), () => instantiation());
			foreach (var i in typeof(TClass).GetInterfaces())
			{
				if (i.FullName.StartsWith("System"))
					continue;
				this.TypeDictionary.Add(i, () => instantiation());
			}
			return this;
		}

		public FakeFactory Register<TInterface, TConcrete>()
			where TConcrete : TInterface, new()
		{
			return this.Register<TInterface, TConcrete>(() => new TConcrete());
		}
		public FakeFactory Register<TInterface, TConcrete>(Func<TConcrete> instantiation)
		{
			this.TypeDictionary.Add(typeof(TInterface), () => instantiation());
			return this;
		}
		public TInterface Instantiate<TInterface>(Vault vault)
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

			if (o is IRequiresVaultInstance)
			{
				(o as IRequiresVaultInstance).Vault = vault;
			}
			return (TInterface)o;
		}
		public bool HasRegistration<TInterface>() => this.TypeDictionary.ContainsKey(typeof(TInterface));
		public bool TryInstantiate<TInterface>(out TInterface item)
		{
			if (this.HasRegistration<TInterface>())
			{
				item = this.Instantiate<TInterface>(vault: null);
				return true;
			}
			item = default;
			return false;
		}

		
		/// <summary>
		/// A default factory using in-memory collections.
		/// </summary>
		public static readonly FakeFactory Default = new FakeFactory()
			.Register<Vault>()
			.Register<VaultObjectTypeOperations>()
			.Register<VaultClassOperations>()
			.Register<VaultPropertyDefOperations>()
			.Register<VaultObjectOperations>()
			.Register(() => SessionInfo.CreateDefault());
	}
}
