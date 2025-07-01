using AutoMapper;
using Final_MozArt.Helpers.Extensions;
using Final_MozArt.Services.Interfaces;
using Final_MozArt.ViewModels.Category;
using Final_MozArt.ViewModels.Instagram;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Final_MozArt.Areas.Admin.Controllers
{
    public class InstagramController : MainController
    {
        private readonly IInstagramService _instagramService;
        private readonly IMapper _mapper;

        public InstagramController(IInstagramService instagramService, IMapper mapper)
        {
            _instagramService = instagramService;
            _mapper = mapper;
        }

        [HttpGet]
        [Authorize(Roles = "Admin,SuperAdmin")]

        public async Task<IActionResult> Index()
        {
            var instagram = await _instagramService.GetAllAsync();
            return View(instagram);
        }
        [HttpGet]
        [Authorize(Roles = "Admin,SuperAdmin")]

        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null)
                return RedirectToAction("Index", "Error");

            var instagram = await _instagramService.GetByIdAsync(id.Value);
            if (instagram == null)
                return RedirectToAction("Index", "Error");

            return View(instagram);
        }

        [HttpGet]
        [Authorize(Roles = "Admin,SuperAdmin")]

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return RedirectToAction("Index", "Error");

            var instagram = await _instagramService.GetByIdAsync(id.Value);
            if (instagram == null)
                return NotFound();

            var model = new InstagramEditVM
            {
                Id = instagram.Id,
                Image = instagram.Image
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,SuperAdmin")]

        public async Task<IActionResult> Edit(int id, InstagramEditVM request)
        {
            if (id != request.Id) return BadRequest();

            if (!ModelState.IsValid) return View(request);

            if (request.Photo != null)
            {
                if (!request.Photo.CheckFileType("image/"))
                {
                    ModelState.AddModelError("Photo", "Only image files are allowed");
                    return View(request);
                }

                if (!request.Photo.CheckFileSize(2048))
                {
                    ModelState.AddModelError("Photo", "Max file size is 2MB");
                    return View(request);
                }
            }

            await _instagramService.EditAsync(request);
            return RedirectToAction(nameof(Index));
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "SuperAdmin")]

        public async Task<IActionResult> Delete(int id)
        {
            await _instagramService.DeleteAsync(id);
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

        public async Task<IActionResult> Create(InstagramCreateVM request)
        {
            if (!ModelState.IsValid)
                return View();

            if (!request.Photo.CheckFileType("image/"))
            {
                ModelState.AddModelError("Photo", "Only image files are allowed");
                return View();
            }

            if (!request.Photo.CheckFileSize(2048))
            {
                ModelState.AddModelError("Photo", "Max file size is 2MB");
                return View();
            }

            await _instagramService.CreateAsync(request);
            return RedirectToAction(nameof(Index));
        }
    }
}
