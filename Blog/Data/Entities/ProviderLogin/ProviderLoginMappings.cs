using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StaticVoid.Blog.Data
{
	public class ProviderLoginMappings : EntityTypeConfiguration<ProviderLogin>
	{
		public ProviderLoginMappings()
		{
			base.HasKey(p => new { p.ProviderKey, p.Provider });
			base.HasRequired(p => p.User).WithMany(u => u.Logins).HasForeignKey(p => p.UserId);
		}
	}
}
