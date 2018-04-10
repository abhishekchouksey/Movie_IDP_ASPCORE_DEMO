using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;

namespace MVCMovie.Controllers
{

    [Route("account")]
    public class AccountController : Controller
    {
        

        //[HttpGet("signout")]
        //public async Task SignOut()
        //{
        //    await HttpContext.Authentication.ChallengeAsync
        //        (OpenIdConnectDefaults.AuthenticationScheme,
        //                        new Microsoft.AspNetCore.Http.Authentication.AuthenticationProperties { RedirectUri = "/" });
        //}
    }
}