using System;

namespace MFilesAPI.Extensions
{
	/// <summary>
	/// A base class for objects implementing the <see cref="IDisposable"/> pattern.
	/// Override <see cref="DisposeManagedObjects"/> and/or <see cref="DisposeUnmanagedObjects"/> as appropriate.
	/// <remarks>see: https://docs.microsoft.com/en-us/dotnet/standard/garbage-collection/implementing-dispose?redirectedfrom=MSDN#implementing-the-dispose-pattern-for-a-base-class </remarks>
	/// </summary>
	public abstract class DisposableBase
		: IDisposable
	{
		// Flag: Has Dispose already been called?
		private bool disposed = false;
   
		/// <inheritdoc />
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);           
		}

		/// <summary>
		/// Disposes of any managed (i.e. .NET) objects that this class uses.
		/// </summary>
		protected virtual void DisposeManagedObjects()
		{
		}

		/// <summary>
		/// Disposes of any unmanaged objects that this class uses.
		/// </summary>
		protected virtual void DisposeUnmanagedObjects()
		{
		}

		/// <summary>
		/// Implements the dispose pattern.
		/// </summary>
		/// <param name="disposing">If false, called from a destructor.  Otherwise called from <see cref="Dispose"/>.</param>
		protected virtual void Dispose(bool disposing)
		{
			if (this.disposed)
				return; 
      
			if (disposing)
			{
				this.DisposeManagedObjects();
			}

			this.DisposeUnmanagedObjects();
			this.disposed = true;
		}

		~DisposableBase()
		{
			this.Dispose(false);
		}
	}
}