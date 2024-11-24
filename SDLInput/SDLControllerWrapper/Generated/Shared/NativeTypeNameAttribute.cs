namespace SDLControllerWrapper.Generated.Shared
{
    using System;
    using System.Diagnostics;

    /// <summary>Defines the type of a member as it was used in the native signature.</summary>
    [AttributeUsage(AttributeTargets.Struct | AttributeTargets.Enum | AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter | AttributeTargets.ReturnValue, AllowMultiple = false, Inherited = true)]
    [Conditional("DEBUG")]
    internal sealed partial class NativeTypeNameAttribute : Attribute
    {

        /// <summary>Initializes a new instance of the <see cref="NativeTypeNameAttribute" /> class.</summary>
        /// <param name="name">The name of the type that was used in the native signature.</param>
        public NativeTypeNameAttribute(string name)
        {
            this.Name = name;
        }

        /// <summary>Gets the name of the type that was used in the native signature.</summary>
        public string Name { get; }
    }
}
