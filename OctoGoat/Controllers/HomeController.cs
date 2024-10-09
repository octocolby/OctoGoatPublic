using System.Diagnostics;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OctoGoat.Data;
using OctoGoat.Models;

namespace OctoGoat.Controllers;

[Authorize]
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly DatabaseContext _DbContext = new DatabaseContext();

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    [AllowAnonymous]
    public IActionResult Index()
    {
        var userProfiles = _DbContext.UserProfiles;
        var tweeters = _DbContext.TweeterModels;
        var checkedTweeters = new List<TweeterModel>();
        foreach (var tweeter in tweeters)
        {
            var checkmark = _DbContext.CheckmarkModels.FirstOrDefault(check => check.Name == tweeter.Name);
            tweeter.Checkmark = checkmark is {Approved: true};
            var profile = userProfiles.FirstOrDefault(prof => prof.UserName == tweeter.Name);
            if (profile != null)
            {
                tweeter.ProfilePicture = profile.ProfilePicture;
            }
            checkedTweeters.Add(tweeter);
        }

        checkedTweeters.Reverse();
        return View(checkedTweeters);
    }
    
    public IActionResult Test()
    {
        var userProfiles = _DbContext.UserProfiles;
        var profile = userProfiles.FirstOrDefault(prof => prof.UserName == User.Identity.Name);
        return View(profile);
    }

    public IActionResult ApplyForCheckmark(CheckmarkModel checkmarkModel)
    {
        if (checkmarkModel.Name != null)
        {
            checkmarkModel.Name = User.Identity.Name;
            var existingCheckmark = _DbContext.CheckmarkModels.FirstOrDefault(check => check.Name == checkmarkModel.Name);
            if (existingCheckmark == null)
            {
                _DbContext.CheckmarkModels.Add(checkmarkModel);
            }
            else
            {
                existingCheckmark.Approved = checkmarkModel.Approved;
                _DbContext.Update(checkmarkModel);
            }
            _DbContext.SaveChanges();
            return View(checkmarkModel);
        }
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
    }
}
