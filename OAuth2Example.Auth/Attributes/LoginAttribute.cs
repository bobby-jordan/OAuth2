using System;

namespace OAuth2Example.Auth.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class LoginAttribute : Attribute
    {
    }
}
