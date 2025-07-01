using AutoMapper;
using Final_MozArt.Services;
using Final_MozArt.Services.Interfaces;
using Final_MozArt.ViewModels.Tag;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Final_MozArt.Areas.Admin.Controllers
{
    public class TagController : MainController
    {
        private readonly ITagService _tagService;
        private readonly IMapper _mapper;

        public TagController(ITagService tagService, IMapper mapper)
        {
            _tagService = tagService;
            _mapper = mapper;
        }

        [HttpGet]
        [Authorize(Roles = "Admin,SuperAdmin")]

        public async Task<IActionResult> Index()
        {
            var tags = await _tagService.GetAllAsync();
            return View(tags);
        }

        [HttpGet]
        [Authorize(Roles = "Admin,SuperAdmin")]

        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null)
                return RedirectToAction("Index", "Error");

            var tag = await _tagService.GetByIdAsync(id.Value);
            if (tag == null)
                return RedirectToAction("Index", "Error");

            return View(tag);
        }

        [HttpGet]
        [Authorize(Roles = "Admin,SuperAdmin")]

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return RedirectToAction("Index", "Error");

            var tag = await _tagService.GetEntityByIdAsync(id.Value);
            if (tag == null)
                return NotFound();

            var model = new TagEditVM
            {
                Id = tag.Id,
                Name = tag.Name

            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,SuperAdmin")]
        public async Task<IActionResult> Edit(int id, TagEditVM request)
        {
            if (id != request.Id) return BadRequest();

            if (!ModelState.IsValid) return View(request);


            try
            {
                await _tagService.EditAsync(request);
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError("Name", ex.Message);
                return View(request);
            }
            return RedirectToAction(nameof(Index));
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "SuperAdmin")]

        public async Task<IActionResult> Delete(int id)
        {
            await _tagService.DeleteAsync(id);
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

        public async Task<IActionResult> Create(TagCreateVM request)
        {
            if (!ModelState.IsValid)
                return View();


            try
            {
                await _tagService.CreateAsync(request);
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError("Name", ex.Message);
                return View();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
