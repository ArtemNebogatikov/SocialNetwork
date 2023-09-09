using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit;
using NUnit.Framework;
using SocialNetwork;
using SocialNetwork.BLL.Models;
using SocialNetwork.BLL.Services;

namespace SocialNetwork.Tests
{
    [TestFixture]
    public class UserServiceTests
    {
        [Test]
        public void RegisterFirstNameReturnNullException()
        {
            UserService service = new UserService();
            UserRegistrationData authData = new UserRegistrationData()
            {
                Email = "test@gmail.com",
                Password = "password",
                FirstName = null,
                LastName = "test"
            };
            Assert.Throws<ArgumentNullException>(() => service.Register(authData));
        }
        [Test]
        public void RegisterLastNameReturnNullException()
        {
            UserService service = new UserService();
            UserRegistrationData authData = new UserRegistrationData()
            {
                Email = "test@gmail.com",
                Password = "password",
                FirstName = "test",
                LastName = null
            };
            Assert.Throws<ArgumentNullException>(() => service.Register(authData));
        }
        [Test]
        public void RegisterPasswordReturnNullException()
        {
            UserService service = new UserService();
            UserRegistrationData authData = new UserRegistrationData()
            {
                Email = "test@gmail.com",
                Password = null,
                FirstName = "test",
                LastName = "test"
            };
            Assert.Throws<ArgumentNullException>(() => service.Register(authData));
        }
        [Test]
        public void RegisterPasswordLengthReturnNullException()
        {
            UserService service = new UserService();
            UserRegistrationData authData = new UserRegistrationData()
            {
                Email = "test@gmail.com",
                Password = "passw",
                FirstName = "test",
                LastName = "test"
            };
            Assert.Throws<ArgumentNullException>(() => service.Register(authData));
        }
        [Test]
        public void RegisterEmailIsNotValidReturnNullException()
        {
            UserService service = new UserService();
            UserRegistrationData authData = new UserRegistrationData()
            {
                Email = "test@",
                Password = "passw",
                FirstName = "test",
                LastName = "test"
            };
            Assert.Throws<ArgumentNullException>(() => service.Register(authData));
        }

    }
}
