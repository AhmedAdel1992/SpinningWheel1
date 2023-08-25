using System.Reflection;

namespace Application;

public static class ApplicationProj
{
    public static Assembly Assembly => typeof(ApplicationProj).Assembly;
    public static string Namespace => typeof(ApplicationProj).Namespace!;
}