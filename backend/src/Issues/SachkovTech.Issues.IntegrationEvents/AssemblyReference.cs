using System.Reflection;

namespace SachkovTech.Issues.IntegrationEvents;

public static class AssemblyReference
{
    public static Assembly Assembly => typeof(AssemblyReference).Assembly;
}