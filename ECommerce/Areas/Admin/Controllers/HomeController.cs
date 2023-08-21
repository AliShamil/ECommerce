using AutoMapper;
using ECommerce.Data;
using ECommerce.Models.ViewModels;
using ECommerce.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace ECommerce.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class HomeController : Controller
    {
        private readonly AppDbContext context;
        private static IMapper _mapper;
        private readonly SignInManager<AppUser> signInManager;

        public HomeController(AppDbContext context, IMapper mapper, SignInManager<AppUser> signInManager)
        {
            this.context = context;
            _mapper = mapper;
            this.signInManager = signInManager;
        }

        public IActionResult Index()
        {
            var temp = context.Products.ToList();
            return View(temp);
        }

        public IActionResult Categories()
        {
            return View(context.Categories.ToList());
        }

        public IActionResult Tags()
        {
            return View(context.Tags.ToList());
        }

        public IActionResult AddProduct()
        {
            ViewBag.Categories = new SelectList(context.Categories, "Id", "Name");
            ViewBag.Tags = new MultiSelectList(context.Tags, "Id", "Name");
            return View();
        }

        public IActionResult AddCategory()
        {
            return View();
        }

        public IActionResult AddTag()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddProduct(AddProductViewModel model)
        {
            try
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors);

                if (ModelState.IsValid)
                {
                    Product product = _mapper.Map<Product>(model);

                    context.Add(product);
                    await context.SaveChangesAsync();


                    foreach (var tag in model.TagIds)
                    {
                        context.Add(new ProductTag { TagId = tag, ProductId = product.Id });
                    }

                    await context.SaveChangesAsync();

                    return RedirectToAction("Index");
                }
                return View("Error", new ErrorViewModel());
            }
            catch (Exception)
            {
                return View("Error", new ErrorViewModel());
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddCategory(AddCategoryViewModel category)
        {
            if (ModelState.IsValid)
            {
                var c = _mapper.Map<Category>(category);
                context.Categories.Add(c);
                await context.SaveChangesAsync();
                return RedirectToAction("Categories");
            }

            return View("Error", new ErrorViewModel());

        }

        [HttpPost]
        public async Task<IActionResult> AddTag(AddTagViewModel tag)
        {

            if (ModelState.IsValid)
            {
                var t = _mapper.Map<Tag>(tag);
                context.Tags.Add(t);
                await context.SaveChangesAsync();
                return RedirectToAction("Tags");
            }
            return RedirectToAction("Error", new ErrorViewModel());
        }

        public async Task<IActionResult> DeleteProduct(int id)
        {
            try
            {
                var p = context.Products.FirstOrDefault(x => x.Id == id);
                if (p is not null)
                {
                    context.Remove(p);
                    await context.SaveChangesAsync();
                }

                return RedirectToAction("Index");
            }
            catch (Exception)
            {

                return View("Error", new ErrorViewModel());
            }
        }

        public async Task<IActionResult> DeleteCategory(int id)
        {
            try
            {
                var c = context.Categories.FirstOrDefault(x => x.Id == id);
                if (c is not null)
                {
                    context.Remove(c);
                    await context.SaveChangesAsync();
                }

                return RedirectToAction("Categories");
            }
            catch (Exception)
            {

                return View("Error", new ErrorViewModel());
            }

        }

        public async Task<IActionResult> DeleteTag(int id)
        {
            try
            {
                var t = context.Tags.FirstOrDefault(x => x.Id == id);
                if (t is not null)
                {
                    context.Remove(t);
                    await context.SaveChangesAsync();
                }

                return RedirectToAction("Tags");
            }
            catch (Exception)
            {

                return View("Error", new ErrorViewModel());
            }
        }

        public IActionResult EditProduct(int id)
        {
            try
            {
                var p = context.Products.FirstOrDefault(p => p.Id == id);
                if (p is not null)
                {
                    AddProductViewModel model = p;
                    ViewBag.Categories = new SelectList(context.Categories, "Id", "Name");
                    ViewBag.Tags = new MultiSelectList(context.Tags, "Id", "Name");
                    ViewBag.Id = id;
                    return View(model);
                }
                return View("Error", new ErrorViewModel());

            }
            catch (Exception ex)
            {
                return View(ex.Message, new ErrorViewModel());
            }
        }

        public IActionResult EditTag(int id)
        {
            try
            {
                var t = context.Tags.FirstOrDefault(t => t.Id == id);
                if (t is not null)
                {
                    AddTagViewModel model = t;
                    ViewBag.Id = id;
                    return View(model);
                }
                return View("Error", new ErrorViewModel());

            }
            catch (Exception)
            {
                return View("Error", new ErrorViewModel());
            }
        }

        public IActionResult EditCategory(int id)
        {
            try
            {
                var c = context.Categories.FirstOrDefault(c => c.Id == id);
                if (c is not null)
                {
                    AddCategoryViewModel model = c;
                    ViewBag.Id = id;
                    return View(model);
                }
                return View("Error", new ErrorViewModel());

            }
            catch (Exception)
            {
                return View("Error", new ErrorViewModel());
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateProduct(AddProductViewModel addProductViewModel, int id)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var p = context.Products.FirstOrDefault(p => p.Id == id);
                    if (p is not null)
                    {
                        _mapper.Map(addProductViewModel, p);
                        context.Update(p);
                        await context.SaveChangesAsync();

                        foreach (var pt in p.ProductTags)
                            context.Remove(pt);
                        await context.SaveChangesAsync();

                        foreach (var tag in addProductViewModel.TagIds)
                            context.Add(new ProductTag { TagId = tag, ProductId = p.Id });
                        await context.SaveChangesAsync();

                        return RedirectToAction("Index");
                    }
                    return View("Error", new ErrorViewModel());
                }
                return View("Error", new ErrorViewModel());
            }
            catch (Exception)
            {
                return View("Error", new ErrorViewModel());

            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateTag(AddTagViewModel addTagViewModel, int id)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var t = context.Tags.FirstOrDefault(t => t.Id == id);
                    if (t is not null)
                    {
                        _mapper.Map(addTagViewModel, t);
                        context.Update(t);
                        await context.SaveChangesAsync();
                        return RedirectToAction("Tags");
                    }
                    return View("Error", new ErrorViewModel());
                }
                return View("Error", new ErrorViewModel());

            }
            catch (Exception)
            {
                return View("Error", new ErrorViewModel());
            }

        }

        [HttpPost]
        public async Task<IActionResult> UpdateCategory(AddCategoryViewModel addCategoryViewModel, int id)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var c = context.Categories.FirstOrDefault(c => c.Id == id);
                    if (c is not null)
                    {
                        _mapper.Map(addCategoryViewModel, c);
                        context.Update(c);
                        await context.SaveChangesAsync();
                        return RedirectToAction("Categories");
                    }
                    return View("Error", new ErrorViewModel());
                }
                return View("Error", new ErrorViewModel());

            }
            catch (Exception)
            {
                return View("Error", new ErrorViewModel());
            }

        }
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home", new { area = "default" });
        }
    }
}

