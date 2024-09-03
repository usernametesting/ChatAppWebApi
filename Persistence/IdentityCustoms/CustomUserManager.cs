using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.IdentityCustoms;

public class CustomUserManager<TUser> : UserManager<TUser> where TUser : class
{
    public CustomUserManager(IUserStore<TUser> store, IOptions<IdentityOptions> optionsAccessor, IPasswordHasher<TUser> passwordHasher, IEnumerable<IUserValidator<TUser>> userValidators, IEnumerable<IPasswordValidator<TUser>> passwordValidators, ILookupNormalizer keyNormalizer, IdentityErrorDescriber errors, IServiceProvider services, ILogger<UserManager<TUser>> logger)
        : base(store, optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, services, logger)
    {
    }

    private void EnsureSecurityStamp(TUser user)
    {
        var securityStampProperty = typeof(TUser).GetProperty("SecurityStamp");
        if (securityStampProperty != null && securityStampProperty.GetValue(user) == null)
        {
            var currentSecurityStamp = securityStampProperty.GetValue(user);
            if (currentSecurityStamp == null)
            {
                securityStampProperty.SetValue(user, Guid.NewGuid().ToString());
            }
        }
    }

    public override async Task<IdentityResult> CreateAsync(TUser user)
    {
        EnsureSecurityStamp(user);  
        return await base.CreateAsync(user);
    }

    public override async Task<IdentityResult> UpdateAsync(TUser user)
    {
        EnsureSecurityStamp(user); 
        return await base.UpdateAsync(user);
    }
}
