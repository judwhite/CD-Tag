using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CDTag.Common;
using CDTag.Common.Dispatcher;
using CDTag.ViewModel.Profile.NewProfile;
using NUnit.Framework;

namespace CDTag.ViewModel.Tests.Profile.NewProfile
{
    [TestFixture]
    public class NewProfileViewModelTests
    {
        [TestFixtureSetUp]
        public void Setup()
        {
            IoC.ClearAllRegistrations();

            IoC.RegisterInstance<IDispatcher>(new UnitTestDispatcher());
        }

        [Test]
        public void TestConstructor()
        {
            // Arrange/Act
            NewProfileViewModel newProfileViewModel = IoC.Resolve<NewProfileViewModel>();

            // Assert
            Assert.That(newProfileViewModel, Is.Not.Null);
            Assert.That(newProfileViewModel.EventAggregator, Is.Not.Null);
        }
    }
}
