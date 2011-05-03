using System.Collections.Generic;
using FillMe.Extensions;
using NUnit.Framework;

namespace FillMe.Tests.Extensions
{
    [TestFixture]
    public class TypeExtensionTests
    {
        [Test]
        public void ShouldConfirmTypeIsEnumerable()
        {
            Assert.That(typeof (List<string>).IsEnumerable(), Is.True);
            Assert.That(typeof (int).IsEnumerable(), Is.False);
            Assert.That(typeof (string).IsEnumerable(), Is.False);
        }
    }
}
