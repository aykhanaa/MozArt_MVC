namespace Final_MozArt.Services.Interfaces
{
    public interface IRoleService
    {
        Task<string> AddRoleAsync(string username, string roleName);
        Task<string> RemoveRoleAsync(string username, string roleName);
        Task<List<string>> GetRolesAsync();
        Task<List<string>> GetUsernamesAsync();
    }
}
