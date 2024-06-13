using System;

[AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
sealed class SkipAttribute : Attribute
{
}

[AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
sealed class SkipInFormAttribute : Attribute
{
}
