using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OctoGoat.Data;
using OctoGoat.Models;

namespace OctoGoat.Controllers;

[Authorize]
public class AdminController : Controller
{
    private readonly ILogger<AdminController> _logger;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly DatabaseContext _DbContext = new DatabaseContext();

    public AdminController(ILogger<AdminController> logger, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
    {
        _logger = logger;
        _userManager = userManager;
        _signInManager = signInManager;
    }

    public IActionResult Index()
    {
        var userProfiles = _DbContext.UserProfiles;
        var profile = userProfiles.FirstOrDefault(prof => prof.UserName == User.Identity.Name);
        if (profile.IsAdmin == true)
        {
            return View();
        }
        return RedirectToAction("Index", "Home");
    }

    public async Task<IActionResult> PasswordReset(UserModel userModel)
    {
        if (userModel.UserName == null)
        {
            return View();
        }
        var userProfiles = _DbContext.UserProfiles;
        var profile = userProfiles.FirstOrDefault(prof => prof.UserName == User.Identity.Name);
        if (profile.IsAdmin == false)
        {
            return RedirectToAction("Index", "Home");
        }
        if (userModel.UserName != "jerry")
        {
            return View(userModel);
        }

        var user = await _userManager.FindByNameAsync(userModel.UserName);
        await _userManager.RemovePasswordAsync(user);
        await _userManager.AddPasswordAsync(user, userModel.Password);
        return View(userModel);
    }

    [AllowAnonymous]
    public async Task<IActionResult> UserList()
    {
        var profileList = new List<UserProfile>();
        var userProfiles = _DbContext.UserProfiles;
        foreach (var userProfile in userProfiles)
        {
            profileList.Add(userProfile);
        }
        return View(profileList);
    }
}
