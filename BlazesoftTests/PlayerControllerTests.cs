using Moq;
using Xunit;
using Blazesoft.Controllers;
using Blazesoft.Models;
using Blazesoft.Services;
using Microsoft.AspNetCore.Mvc;

namespace Blazesoft.Tests
{
    public class PlayerControllerTests
    {
        private readonly PlayerController _controller;
        private readonly Mock<IPlayerService> _playerServiceMock;

        public PlayerControllerTests()
        {
            _playerServiceMock = new Mock<IPlayerService>();
            _controller = new PlayerController(_playerServiceMock.Object);
        }

        [Fact]
        public async Task UpdateBalance_PlayerExists_ReturnsOkResult()
        {
            // Arrange
            var playerId = "playerId1";
            var amountToAdd = 500;
            var updatedBalance = 1500;

            var player = new Player { Id = playerId, Balance = 1000 };

            _playerServiceMock.Setup(service => service.GetPlayerAsync(playerId))
                              .ReturnsAsync(player);

            _playerServiceMock.Setup(service => service.UpdatePlayerAsync(It.IsAny<Player>()))
                              .Returns(Task.CompletedTask);

            var request = new UpdateBalanceRequest { PlayerId = playerId, Amount = amountToAdd };

            var result = await _controller.UpdateBalance(request);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = okResult.Value as dynamic;

            var balanceProperty = response.GetType().GetProperty("Balance");
            Assert.NotNull(balanceProperty);
            var balance = (decimal)balanceProperty.GetValue(response);
            Assert.Equal(updatedBalance, balance);
        }

        [Fact]
        public async Task UpdateBalance_PlayerDoesNotExist_ReturnsNotFound()
        {
            // Arrange
            var playerId = "playerId1";
            var amountToAdd = 500;

            _playerServiceMock.Setup(service => service.GetPlayerAsync(playerId))
                              .ReturnsAsync((Player)null);

            var request = new UpdateBalanceRequest { PlayerId = playerId, Amount = amountToAdd };

            // Act
            var result = await _controller.UpdateBalance(request);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }
    }
}
