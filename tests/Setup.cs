using Microsoft.Build.Locator;

using NUnit.Framework;

[SetUpFixture]
public class Setup
{
    [OneTimeSetUp]
    public void RegisterMSBuildAssemblies()
    {
        MSBuildLocator.RegisterDefaults();
    }
}
