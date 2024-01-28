using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Policiespp.Controllers
{
	[Authorize]
    public class ClaimsController : Controller
    {
		UserManager<IdentityUser> UserManager { get; }
        SignInManager<IdentityUser> SignInManager { get; }

        public ClaimsController(
			UserManager<IdentityUser> userManager, 
			SignInManager<IdentityUser> signInManager)
		{
			UserManager = userManager;
            SignInManager = signInManager;
        }

		public async Task<IActionResult> Add()
		{
			if (!User.HasClaim(c => c.Type == ClaimTypes.Role))
			{
                var user = await UserManager.GetUserAsync(User);
                var result = await UserManager.AddClaimAsync(
                            user, 
					        new Claim(ClaimTypes.Role, "Admin" )
                            );
				if (!result.Succeeded)
				{
					return RedirectToAction("Error", "Home");
				}
                else
                {
                    await SignInManager.RefreshSignInAsync(user);
                }
			}
            return RedirectToAction("Index","Home");
		}

		public async Task<IActionResult> Remove()
		{
			var Role = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role);
			if (Role != null)
			{
                var user = await UserManager.GetUserAsync(User);
                var result = await UserManager.RemoveClaimAsync(
                            user, Role
							);
				if (!result.Succeeded)
				{
					return RedirectToAction("Error", "Home");
				}
                else
                {
                    await SignInManager.RefreshSignInAsync(user);
                }
            }
            return RedirectToAction("Index", "Home");
		}
	}
}