namespace GarageBuddy.Common.Attributes
{
    using System;

    /// <summary>
    /// Attribute used to indicate that a property should be sanitized to prevent malicious input.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class SanitizeAttribute : Attribute
    {
    }
}
