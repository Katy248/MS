using System.Diagnostics;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MS.Mvc.Models;
using MS.Mvc.ViewModels;

namespace MS.Mvc.Controllers;
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly SignInManager<MSUser> signInManager;
    private readonly UserManager<MSUser> userManager;

    public HomeController(ILogger<HomeController> logger, SignInManager<MSUser> signInManager, UserManager<MSUser> userManager)
    {
        _logger = logger;
        this.signInManager = signInManager;
        this.userManager = userManager;
    }

    public async Task<IActionResult> Index()
    {
        if(signInManager.IsSignedIn(User)) 
        {
            var user = await userManager.GetUserAsync(User);
            if (user.IsEmployee)
            {
                return RedirectToAction("Employee");
            }
            return RedirectToAction("Client");
        }
        return View();
    }
    public IActionResult Employee()
    {
        return View();
    }
    public IActionResult Client()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
