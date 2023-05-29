using FasenmyerConference.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Npgsql.EntityFrameworkCore.PostgreSQL.Query.ExpressionTranslators.Internal;

namespace FasenmyerConference.Controllers
{
    [Authorize(Roles = "Admin")]
    public class LoginController : Controller
    {
        public IActionResult Login()
        {
            return View("LoginSuccess");
        }

        //Everything under this is no longer in use, has been replaced with AzureAD
        /*
        public List<UserModel> PutValue()
        {
            var users = new List<UserModel>
            {
                new UserModel{id=1, username="AdminTest", password="testpro123"},
                new UserModel{id=2, username="PSUAdmin", password="psutest123"}
            
            };

            return users;
        }

        public IActionResult Verify(UserModel usr)
        {
            var u = PutValue();
            var un = u.Where(u => u.username.Equals(usr.username));
            var up = un.Where(p => p.password.Equals(usr.password));

            if (up.Count() == 1)
            {
                ViewBag.message = "Login Success";
                return View("LoginSuccess");
            }
            else
            {
                ViewBag.message = "Login Failed";
                return View("Login");
            }
        }*/
    }
}
