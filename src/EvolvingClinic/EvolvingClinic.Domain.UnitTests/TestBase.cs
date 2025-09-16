using EvolvingClinic.Domain.Utils;
using NUnit.Framework;

namespace EvolvingClinic.Domain.UnitTests;

public abstract class TestBase
{
    [SetUp]
    public void SetUp()
    {
        ApplicationClock.Reset();
    }
}