using System.Reflection;

namespace ValidationEngine;

public static class ValidationBuilderProj
{
    public static Assembly Assembly => typeof(ValidationBuilderProj).Assembly;
    public static string Namespace => typeof(ValidationBuilderProj).Namespace!;
}