using Moq;
using AutoMapper;
using ToolsTrackPro.Application.Features.Tools.Handlers;
using ToolsTrackPro.Infrastructure.Interfaces;
using ToolsTrackPro.Application.Features.Tools.Commands;
using ToolsTrackPro.Domain.Entities;
using FluentAssertions;
using Xunit;
using System.Threading;
using System.Threading.Tasks;

namespace ToolsTrackPro.UnitTests.Handlers
{
    public class AddToolCommandHandlerTests
    {
        private readonly Mock<IToolRepository> _mockRepo;
        private readonly Mock<IMapper> _mockMapper;
        private readonly AddToolCommandHandler _handler;

        public AddToolCommandHandlerTests()
        {
            _mockRepo = new Mock<IToolRepository>();
            _mockMapper = new Mock<IMapper>();

            _handler = new AddToolCommandHandler(_mockRepo.Object, _mockMapper.Object);
        }

        [Fact]
        public async Task Handle_ValidCommand_AddsPatientAndReturnsId()
        {
            // Arrange
            var command = new AddToolCommand(new Application.DTOs.ToolDto
            {
                Id = 1,
                Name = "Test Tool",
                Description = "Test Description",
                StatusId = 1,
                CreatedAt = DateTime.Now
            });

            var toolEntity = new Tool
            {
                Id = 1,
                Name = command.Tool.Name,
                Description = command.Tool.Description,
                StatusId = command.Tool.StatusId,
                CreatedAt = command.Tool.CreatedAt,
            };

            _mockMapper.Setup(m => m.Map<Tool>(command)).Returns(toolEntity);
            _mockRepo.Setup(repo => repo.AddAsync(It.IsAny<Tool>())).ReturnsAsync(true);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().Be(true);
            _mockRepo.Verify(repo => repo.AddAsync(It.IsAny<Tool>()), Times.Once);
        }
    }
}
