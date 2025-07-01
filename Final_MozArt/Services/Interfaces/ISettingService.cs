using Final_MozArt.Models;
using Final_MozArt.ViewModels.Setting;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Final_MozArt.Services.Interfaces
{
    public interface ISettingService
    {
        Dictionary<string, string> GetSettings();
        Task<List<Setting>> GetAllAsync();
        Task<Setting> GetByIdAsync(int id);
        Task EditAsync(SettingEditVM setting);
        string GetSettingValue(string key);
        
        }
}
