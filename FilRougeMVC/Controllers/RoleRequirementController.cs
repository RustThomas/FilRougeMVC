using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PoliciesApp.Controllers
{
    [Authorize(Policy = "Admin")]
    public class RoleRequirementController : Controller
    {
		public IActionResult Access() => View();

	}
}