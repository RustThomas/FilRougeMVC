using Microsoft.AspNetCore.Authorization;
using PoliciesApp.Services.Requirements;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PoliciesApp.Services.Handlers
{
    public class RoleHandler : AuthorizationHandler<RoleRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
                                                       RoleRequirement requirement)
        {
            if (!context.User.HasClaim(c => c.Type == ClaimTypes.Role ))
            {
                return Task.CompletedTask;
            }

            var Role = 
                context.User.FindFirst(c => c.Type == ClaimTypes.Role).Value;



            if (Role == requirement.Role)
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
