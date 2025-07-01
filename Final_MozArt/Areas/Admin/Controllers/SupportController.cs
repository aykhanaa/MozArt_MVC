using AutoMapper;
using Final_MozArt.Helpers.Extensions;
using Final_MozArt.Services;
using Final_MozArt.Services.Interfaces;
using Final_MozArt.ViewModels.Support;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Final_MozArt.Areas.Admin.Controllers
{
    public class SupportController : MainController
    {
        private readonly ISupportService  _supportService;
        private readonly IMapper _mapper;

        public SupportController(ISupportService supportService, IMapper mapper)
        {
            _supportService = supportService;
            _mapper = mapper;
        }

        [HttpGet]
        [Authorize(Roles = "Admin,SuperAdmin")]

        public async Task<IActionResult> Index()
        {
            var supports = await _supportService.GetAllAsync();
            return View(supports);
        }
        [HttpGet]
        [Authorize(Roles = "Admin,SuperAdmin")]

        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null)
                return RedirectToAction("Index", "Error");

            var support = await _supportService.GetByIdAsync(id.Value);
            if (support == null)
                return RedirectToAction("Index", "Error");

            return View(support);
        }

        [HttpGet]
        [Authorize(Roles = "Admin,SuperAdmin")]

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return RedirectToAction("Index", "Error");

            var support = await _supportService.GetEntityByIdAsync(id.Value);
            if (support == null)
                return NotFound();

            var model = new SupportEditVM
            {
                Id = support.Id,
                Title = support.Title,
                Description = support.Description,
              
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,SuperAdmin")]

        public async Task<IActionResult> Edit(int id, SupportEditVM request)
        {
            if (id != request.Id) return BadRequest();

            if (!ModelState.IsValid) return View(request);

            await _supportService.EditAsync(request);
            return RedirectToAction(nameof(Index));
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "SuperAdmin")]

        public async Task<IActionResult> Delete(int id)
        {
            await _supportService.DeleteAsync(id);
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

        public async Task<IActionResult> Create(SupportCreateVM request)
        {
            if (!ModelState.IsValid)
                return View();


            await _supportService.CreateAsync(request);
            return RedirectToAction(nameof(Index));
        }
    }
}
