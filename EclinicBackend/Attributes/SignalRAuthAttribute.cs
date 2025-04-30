using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace EclinicBackend.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class SignalRAuthAttribute : AuthorizeAttribute
    {
        public SignalRAuthAttribute()
        {
            Policy = "MedicalStaff"; // Set the policy name
        }
    }
}
