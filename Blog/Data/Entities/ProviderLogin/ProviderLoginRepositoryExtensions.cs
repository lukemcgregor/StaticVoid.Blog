using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StaticVoid.Blog.Data
{
	public static class ProviderLoginRepositoryExtensions
	{
		public static ProviderLogin GetByProviderAndKey(this IQueryable<ProviderLogin> providerLogins, string provider, string providerKey)
		{
			var type = provider.ParseProviderType();

			return providerLogins.SingleOrDefault(p => p.ProviderKey == providerKey && p.Provider == type);
		}

		public static ProviderType ParseProviderType(this string providerType)
		{
			ProviderType type;
			if (!Enum.TryParse<ProviderType>(providerType, out type))
			{
				throw new ArgumentException("Invalid provider type", "provider");
			}
			return type;
		}
	}
}
