using AutoMapper;
using Final_MozArt.Data;
using Final_MozArt.Models;
using Final_MozArt.Services.Interfaces;
using Final_MozArt.ViewModels.Setting;
using Microsoft.EntityFrameworkCore;

namespace Final_MozArt.Services
{
    public class SettingService : ISettingService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public SettingService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public Dictionary<string, string> GetSettings()
        {
            return _context.Settings
                .AsNoTracking()
                .ToDictionary(m => m.Key, m => m.Value);
        }

        public string GetSettingValue(string key)
        {
            return _context.Settings
                .AsNoTracking()
                .Where(s => s.Key == key)
                .Select(s => s.Value)
                .FirstOrDefault();
        }

        public async Task<List<Setting>> GetAllAsync()
        {
            return await _context.Settings.AsNoTracking().ToListAsync();
        }

        public async Task<Setting> GetByIdAsync(int id)
        {
            return await _context.Settings.FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task EditAsync(SettingEditVM setting)
        {
            var dbSetting = await _context.Settings.FirstOrDefaultAsync(m => m.Id == setting.Id);
            if (dbSetting == null) throw new Exception("Setting not found");

            dbSetting.Key = setting.Key;
            dbSetting.Value = setting.Value;

            _context.Settings.Update(dbSetting);
            await _context.SaveChangesAsync();
        }
    }
}
