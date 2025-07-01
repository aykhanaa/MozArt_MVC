using Final_MozArt.Data;
using Final_MozArt.Helpers.Enums;
using Final_MozArt.Models;
using Final_MozArt.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Final_MozArt.Services
{
  public class RoleService : IRoleService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly AppDbContext _context;

        public RoleService(UserManager<AppUser> userManager,
                           RoleManager<IdentityRole> roleManager,
                           IHttpContextAccessor httpContextAccessor,
                           SignInManager<AppUser> signInManager,
                           AppDbContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _httpContextAccessor = httpContextAccessor;
            _signInManager = signInManager;
            _context = context;
        }

        public async Task<string> AddRoleAsync(string username, string roleName)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user == null) return "User does not exist.";

            if (!await _roleManager.RoleExistsAsync(roleName))
                return "Role does not exist.";

            var result = await _userManager.AddToRoleAsync(user, roleName);
            if (!result.Succeeded)
            {
                return $"Failed to add role: {string.Join(", ", result.Errors.Select(e => e.Description))}";
            }

            await _userManager.UpdateSecurityStampAsync(user);

            return $"Role '{roleName}' added to user '{username}' successfully.";
        }

        public async Task<string> RemoveRoleAsync(string username, string roleName)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user == null) return "User does not exist.";

            if (!await _roleManager.RoleExistsAsync(roleName))
                return "Role does not exist.";

            if (roleName.Equals(Roles.SuperAdmin.ToString(), StringComparison.OrdinalIgnoreCase))
            {
                return "The 'SuperAdmin' role cannot be removed.";
            }

            var currentUser = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);
            if (await _userManager.IsInRoleAsync(currentUser, Roles.SuperAdmin.ToString()))
            {
                if (roleName.Equals(Roles.Member.ToString(), StringComparison.OrdinalIgnoreCase))
                {
                    return "SuperAdmin cannot remove the 'Member' role.";
                }
            }

            var result = await _userManager.RemoveFromRoleAsync(user, roleName);
            if (!result.Succeeded)
            {
                return $"Failed to remove role: {string.Join(", ", result.Errors.Select(e => e.Description))}";
            }

            await _userManager.UpdateSecurityStampAsync(user);

            return $"Role '{roleName}' removed from user '{username}' successfully.";
        }
        public async Task<List<string>> GetRolesAsync()
        {
            return await Task.FromResult(_roleManager.Roles.Select(r => r.Name).ToList());
        }

        public async Task<List<string>> GetUsernamesAsync()
        {
            return await _context.Users
                .Select(u => u.UserName)
                .ToListAsync();
        }
    }
}

