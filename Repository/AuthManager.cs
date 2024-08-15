using AutoMapper;
using CoreApiInNet.Contracts;
using CoreApiInNet.Data;
using CoreApiInNet.Users;
using Microsoft.AspNetCore.Identity;

namespace CoreApiInNet.Repository
{
    public class AuthManager : InterfaceAuthManager
    {
        private readonly IMapper mapper;
        private readonly UserManager<IdentityUser> usermanager;

        public AuthManager(IMapper mapper,UserManager<IdentityUser> usermanager) 
        {
            this.mapper = mapper;
            this.usermanager = usermanager;
        }

        public async Task<IEnumerable<IdentityError>> Register(ApiUserDto userDto)
        {
            var user=mapper.Map<IdentityUser>(userDto);
            user.UserName = userDto.nick;


            var execute = await usermanager.CreateAsync(user, userDto.password);

            if(execute.Succeeded)
            {
                await usermanager.AddToRoleAsync(user, "User");
            }

            return execute.Errors; // it can return null
        }
    }
}
