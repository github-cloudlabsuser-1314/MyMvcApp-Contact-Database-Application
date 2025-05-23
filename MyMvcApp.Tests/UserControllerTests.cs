using Xunit;
using MyMvcApp.Controllers;
using MyMvcApp.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace MyMvcApp.Tests
{
    public class UserControllerTests
    {
        public UserControllerTests()
        {
            // Clear the static userlist before each test
            UserController.userlist.Clear();
        }

        [Fact]
        public void Index_ReturnsViewResult_WithUserList()
        {
            // Arrange
            UserController.userlist.Add(new User { Id = 1, Name = "Test", Email = "test@email.com" });
            var controller = new UserController();

            // Act
            var result = controller.Index() as ViewResult;

            // Assert
            Assert.NotNull(result);
            var model = Assert.IsAssignableFrom<List<User>>(result.Model);
            Assert.Single(model);
        }

        [Fact]
        public void Details_ReturnsViewResult_WhenUserExists()
        {
            // Arrange
            UserController.userlist.Add(new User { Id = 1, Name = "Test", Email = "test@email.com" });
            var controller = new UserController();

            // Act
            var result = controller.Details(1) as ViewResult;

            // Assert
            Assert.NotNull(result);
            var model = Assert.IsType<User>(result.Model);
            Assert.Equal(1, model.Id);
        }

        [Fact]
        public void Details_ReturnsNotFound_WhenUserDoesNotExist()
        {
            // Arrange
            var controller = new UserController();

            // Act
            var result = controller.Details(99);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void Create_Post_AddsUserAndRedirects_WhenModelStateIsValid()
        {
            // Arrange
            var controller = new UserController();
            var user = new User { Name = "Test", Email = "test@email.com" };

            // Act
            var result = controller.Create(user) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
            Assert.Single(UserController.userlist);
        }

        [Fact]
        public void Edit_Post_UpdatesUserAndRedirects_WhenModelStateIsValid()
        {
            // Arrange
            UserController.userlist.Add(new User { Id = 1, Name = "Old", Email = "old@email.com" });
            var controller = new UserController();
            var updatedUser = new User { Id = 1, Name = "New", Email = "new@email.com" };

            // Act
            var result = controller.Edit(1, updatedUser) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
            Assert.Equal("New", UserController.userlist[0].Name);
        }

        [Fact]
        public void Delete_Post_RemovesUserAndRedirects_WhenUserExists()
        {
            // Arrange
            UserController.userlist.Add(new User { Id = 1, Name = "Test", Email = "test@email.com" });
            var controller = new UserController();

            // Act
            var result = controller.Delete(1, null) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
            Assert.Empty(UserController.userlist);
        }
    }
}
