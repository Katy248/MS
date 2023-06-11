﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MS.Mvc.Interfaces.Managers;
using MS.Mvc.Models;
using MS.Mvc.ViewModels;

namespace MS.Mvc.Controllers;

public class UsersController : Controller
{
    private readonly UserManager<MSUser> _userManager;
    private readonly SignInManager<MSUser> _signInManager;
    private readonly IOrderManager _orderManager;

    public UsersController(UserManager<MSUser> userManager, SignInManager<MSUser> signInManager, IOrderManager orderManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _orderManager = orderManager;
    }
    public async Task<IActionResult> Register() => View();
    public async Task<IActionResult> Login() => View();
    [HttpPost]
    public async Task<IActionResult> Register([Bind] UserRegisterViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var user = new MSUser
        {
            Id = Guid.NewGuid().ToString(),
            UserName = model.Email,
            Email = model.Email,
        };
        await _userManager.CreateAsync(user, model.Password);

        return RedirectToAction(controllerName: "Home", actionName: "Index");
    }
    [HttpPost]
    public async Task<IActionResult> Login([Bind] UserLoginViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);
        var user = await _userManager.Users.FirstOrDefaultAsync(user => user.Email == model.Email);
        if (user is not null)
        {
            var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, false);
        }
        //var user = new MSUser { Email = model.Email, UserName = model.Email };

        return RedirectToAction(controllerName: "Home", actionName: "Index");
    }
    public async Task<IActionResult> OrderHistory ()
    {
        var orders = await _orderManager.GetUserOrders(User);
        return View(orders);
    }
}
