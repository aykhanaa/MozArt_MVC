using AutoMapper;
using Final_MozArt.Helpers.Extensions;
using Final_MozArt.Services;
using Final_MozArt.Services.Interfaces;
using Final_MozArt.ViewModels.Brand;
using Final_MozArt.ViewModels.Category;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Final_MozArt.Areas.Admin.Controllers
{
    public class BrandController  : MainController
    {
        private readonly IBrandService _brandService;
        private readonly IMapper _mapper;

    public BrandController(IBrandService  brandService, IMapper mapper)
    {
        _brandService = brandService;
        _mapper = mapper; 
    }


    [HttpGet]
     [Authorize(Roles = "Admin,SuperAdmin")]
    public async Task<IActionResult> Index()
    {
        var brands = await _brandService.GetAllAsync();
        return View(brands);
    }

    [HttpGet]
    [Authorize(Roles = "Admin,SuperAdmin")]
    public async Task<IActionResult> Detail(int? id)
    {
        if (id == null)
            return RedirectToAction("Index", "Error");

        var brand = await _brandService.GetByIdAsync(id.Value);
        if (brand == null)
            return RedirectToAction("Index", "Error");

        return View(brand);
    }

    [HttpGet]
     [Authorize(Roles = "Admin,SuperAdmin")]
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
            return RedirectToAction("Index", "Error");

        var brand = await _brandService.GetByIdAsync(id.Value);
        if (brand == null)
            return NotFound();

        var model = new BrandEditVM
        {
            Id = brand.Id,
            Name = brand.Name,
            Image = brand.Image
        };

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin,SuperAdmin")]
    public async Task<IActionResult> Edit(int id, BrandEditVM request)
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

            try
            {
                await _brandService.EditAsync(request);
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
        await _brandService.DeleteAsync(id);
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
    public async Task<IActionResult> Create(BrandCreateVM request)
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

            try
            {
                await _brandService.CreateAsync(request);
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
