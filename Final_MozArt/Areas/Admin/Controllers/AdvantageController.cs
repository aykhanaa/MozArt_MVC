using AutoMapper;
using Final_MozArt.Services.Interfaces;
using Final_MozArt.ViewModels.Advantage;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Final_MozArt.Areas.Admin.Controllers
{
    public class AdvantageController : MainController
    {
        private readonly IAdvantageService _advantageService;
        private readonly IMapper _mapper;

        public AdvantageController(IAdvantageService advantageService, IMapper mapper)
        {
            _advantageService = advantageService;
            _mapper = mapper;
        }

        [HttpGet]
        [Authorize(Roles = "Admin,SuperAdmin")]
        public async Task<IActionResult> Index()
        {
            var advantages = await _advantageService.GetAllAsync();
            return View(advantages);
        }
        [HttpGet]
        [Authorize(Roles = "Admin,SuperAdmin")]
        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null)
                return RedirectToAction("Index", "Error");

            var advantage = await _advantageService.GetByIdAsync(id.Value);
            if (advantage == null)
                return RedirectToAction("Index", "Error");

            return View(advantage);
        }

        [HttpGet]
        [Authorize(Roles = "Admin,SuperAdmin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return RedirectToAction("Index", "Error");

            var advantage = await _advantageService.GetEntityByIdAsync(id.Value);
            if (advantage == null)
                return NotFound();

            var model = new AdvantageEditVM
            {
                Id = advantage.Id,
                Title = advantage.Title,
                Description = advantage.Description,

            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,SuperAdmin")]
        public async Task<IActionResult> Edit(int id, AdvantageEditVM request)
        {
            if (id != request.Id) return BadRequest();

            if (!ModelState.IsValid) return View(request);

            await _advantageService.EditAsync(request);
            return RedirectToAction(nameof(Index));
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> Delete(int id)
        {
            await _advantageService.DeleteAsync(id);
           return Ok();
        }

        [HttpGet]
        [Authorize(Roles = "SuperAdmin")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> Create(AdvantageCreateVM request)
        {
            if (!ModelState.IsValid)
                return View();


            await _advantageService.CreateAsync(request);
            return RedirectToAction(nameof(Index));
        }
    }
}
