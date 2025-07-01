using AutoMapper;
using Final_MozArt.Services.Interfaces;
using Final_MozArt.ViewModels.ContactIntro;
using Final_MozArt.ViewModels.Support;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Final_MozArt.Areas.Admin.Controllers
{
    public class ContactIntroController : MainController
    {
        private readonly IContactIntroService _contactIntroService;
        private readonly IMapper _mapper;

        public ContactIntroController(IContactIntroService contactIntroService, IMapper mapper)
        {
            _contactIntroService = contactIntroService;
            _mapper = mapper;
        }

        [HttpGet]
        [Authorize(Roles = "Admin,SuperAdmin")]
        public async Task<IActionResult> Index()
        {
            var contactintros = await _contactIntroService.GetAllAsync();
            return View(contactintros);
        }
        [HttpGet]
        [Authorize(Roles = "Admin,SuperAdmin")]

        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null)
                return RedirectToAction("Index", "Error");

            var contactintros = await _contactIntroService.GetByIdAsync(id.Value);
            if (contactintros == null)
                return RedirectToAction("Index", "Error");

            return View(contactintros);
        }

        [HttpGet]
        [Authorize(Roles = "Admin,SuperAdmin")]

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return RedirectToAction("Index", "Error");

            var contactintros = await _contactIntroService.GetEntityByIdAsync(id.Value);
            if (contactintros == null)
                return NotFound();

            var model = new ContactIntroEditVM
            {
                Id = contactintros.Id,
                Title = contactintros.Title,
                Description = contactintros.Description,

            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,SuperAdmin")]

        public async Task<IActionResult> Edit(int id, ContactIntroEditVM request)
        {
            if (id != request.Id) return BadRequest();

            if (!ModelState.IsValid) return View(request);

            await _contactIntroService.EditAsync(request);
            return RedirectToAction(nameof(Index));
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "SuperAdmin")]

        public async Task<IActionResult> Delete(int id)
        {
            await _contactIntroService.DeleteAsync(id);
           // return RedirectToAction(nameof(Index));
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

        public async Task<IActionResult> Create(ContactIntroCreateVM request)
        {
            if (!ModelState.IsValid)
                return View();


            await _contactIntroService.CreateAsync(request);
            return RedirectToAction(nameof(Index));
        }
    }
}
