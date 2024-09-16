using Moq;
using Blazesoft.Controllers;
using Blazesoft.Models;
using Blazesoft.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Blazesoft.Tests
{
    public class SpinControllerTests
    {
        private readonly SpinController _controller;
        private readonly Mock<IPlayerService> _playerServiceMock;
        private readonly Mock<IConfiguration> _configurationMock;
        private readonly int _width = 5;
        private readonly int _height = 3;
        public SpinControllerTests()
        {
            _playerServiceMock = new Mock<IPlayerService>();
            _configurationMock = new Mock<IConfiguration>();

            _configurationMock.Setup(c => c["SlotMachineSettings:Width"]).Returns(_width.ToString());
            _configurationMock.Setup(c => c["SlotMachineSettings:Height"]).Returns(_height.ToString());

            _controller = new SpinController(_playerServiceMock.Object, _configurationMock.Object);
        }

        [Fact]
        public async Task Spin_PlayerDoesNotExist_ReturnsNotFound()
        {
            // Arrange
            var playerId = "playerId1";
            var betAmount = 100m;

            _playerServiceMock.Setup(service => service.GetPlayerAsync(playerId))
                              .ReturnsAsync((Player)null);

            var request = new SpinRequest { PlayerId = playerId, Bet = betAmount };

            // Act
            var result = await _controller.Spin(request);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result.Result);

        }

        [Fact]
        public async Task Spin_InsufficientBalance_ReturnsBadRequest()
        {
            // Arrange
            var playerId = "playerId1";
            var betAmount = 2000m; // Greater than the balance
            var player = new Player { Id = playerId, Balance = 1000m };

            _playerServiceMock.Setup(service => service.GetPlayerAsync(playerId))
                              .ReturnsAsync(player);

            var request = new SpinRequest { PlayerId = playerId, Bet = betAmount };

            // Act
            var result = await _controller.Spin(request);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result.Result);
        }
    }
}
