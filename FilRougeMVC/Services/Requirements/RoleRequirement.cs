namespace PoliciesApp.Services.Requirements
{
    using Microsoft.AspNetCore.Authorization;

    public class RoleRequirement : IAuthorizationRequirement
    {
        public string Role { get; private set; }

        public RoleRequirement(string role)
        {
            Role = role;
        }
    }
}
