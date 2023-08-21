using AutoMapper;
using ECommerce.Data;
using ECommerce.Models;
using ECommerce.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace ECommerce.Areas.User.Controllers
{
    [Area("User")]
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
            var temp = context.Categories.ToList();
            return View(temp);
        }

        public IActionResult Tags()
        {
            var temp = context.Tags.ToList();
            return View(temp);
        }

        public IActionResult AddToBasket(int id)
        {
            try
            {
                var products = context.Products.ToList();

                var list = TempData["ProductsBasket"]?.ToString() is null ? new List<UserProductViewModel>() : JsonSerializer.Deserialize<List<UserProductViewModel>>(TempData["ProductsBasket"]?.ToString()!);

                var p = products.FirstOrDefault(x => x.Id == id);
                if (p is not null && list is not null)
                {
                    if (!list.Any(e => e.Id == p.Id))
                    {
                        list.Add(_mapper.Map<UserProductViewModel>(p));
                        TempData["ProductsBasket"] = JsonSerializer.Serialize(list);
                    }
                    return RedirectToAction("Index");

                }

                return View("Error", new ErrorViewModel());
            }
            catch (Exception)
            {
                return View("Error", new ErrorViewModel());

            }

        }

        public IActionResult ProductsBasket()
        {
            return View();
        }

        public IActionResult RemoveFromChart(int id)
        {
            try
            {
                var list = JsonSerializer.Deserialize<List<UserProductViewModel>>(TempData["ProductsBasket"]?.ToString()!);
                if (list is not null)
                {
                    var p = list.Find(p => p.Id == id);
                    if (p is not null)
                        list.Remove(p);

                }
                return RedirectToAction("ProductsBasket");
            }
            catch (Exception)
            {
                return RedirectToAction("ProductsBasket");
            }
        }
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home", new {area="default"});
        }

    }
}
