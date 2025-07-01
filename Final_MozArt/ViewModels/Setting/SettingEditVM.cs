using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Final_MozArt.ViewModels.Setting
{
    public class SettingEditVM
    {
        public int Id { get; set; }

        public string? Key { get; set; }

        public string? Value { get; set; }
    }
}
