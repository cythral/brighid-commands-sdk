using System;

using Microsoft.Build.Locator;

using NUnit.Framework;

[SetUpFixture]
public class Setup
{
    [OneTimeSetUp]
    public void RegisterMSBuildAssemblies()
    {
        Console.WriteLine("test");
        MSBuildLocator.RegisterDefaults();
    }
}
