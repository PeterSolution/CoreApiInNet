using CoreApiInNet.Users;
using Microsoft.AspNetCore.Identity;

namespace CoreApiInNet.Contracts
{
    public interface InterfaceAuthManager
    {
        Task<IEnumerable<IdentityError>> Register(ApiUserDto userDto);
    }
}
