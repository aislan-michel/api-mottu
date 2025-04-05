using System.Linq.Expressions;

using Moq;

using Mottu.Api.Domain.Entities;
using Mottu.Api.Infrastructure.Notifications;
using Mottu.Api.Infrastructure.Repositories.GenericRepository;
using Mottu.Api.Models;
using Mottu.Api.UseCases.MotorcycleUseCases;

namespace Mottu.Api.Tests.UseCases
{
    public class MotorcycleUseCaseTests
    {
        private readonly Mock<INotificationService> _notificationServiceMock;
        private readonly Mock<IRepository<Motorcycle>> _repositoryMock;
        private readonly MotorcycleUseCase _motorcycleUseCase;

        public MotorcycleUseCaseTests()
        {
            _notificationServiceMock = new Mock<INotificationService>();
            _repositoryMock = new Mock<IRepository<Motorcycle>>();
            _motorcycleUseCase = new MotorcycleUseCase(_notificationServiceMock.Object, _repositoryMock.Object);
        }

        [Fact]
        public void Create_ShouldAddNotification_WhenYearIsInvalid()
        {
            var request = new PostMotorcycleRequest
            {
                Year = 0,
                Model = "Model 1",
                Plate = "ABC1234"
            };

            _notificationServiceMock.Setup(ns => ns.HaveNotifications()).Returns(true);

            // Act
            _motorcycleUseCase.Create(request);

            // Assert
            _notificationServiceMock.Verify(ns => ns.Add(It.Is<Notification>(n => n.Message == "Ano da moto menor ou igual a zero")), Times.Once);
            _repositoryMock.Verify(r => r.Create(It.IsAny<Motorcycle>()), Times.Never);
        }

        [Fact]
        public void Create_ShouldAddNotification_WhenModelIsNull()
        {
            var request = new PostMotorcycleRequest
            {
                Year = 2021,
                Model = null,
                Plate = "ABC1234"
            };

            _notificationServiceMock.Setup(ns => ns.HaveNotifications()).Returns(true);

            // Act
            _motorcycleUseCase.Create(request);

            // Assert
            _notificationServiceMock.Verify(ns => ns.Add(It.Is<Notification>(n => n.Message == "Modelo não pode ser nulo ou vazio")), Times.Once);
            _repositoryMock.Verify(r => r.Create(It.IsAny<Motorcycle>()), Times.Never);
        }

        [Fact]
        public void Create_ShouldAddNotification_WhenPlateIsNull()
        {
            var request = new PostMotorcycleRequest
            {
                Year = 2021,
                Model = "Model 1",
                Plate = null
            };

            _notificationServiceMock.Setup(ns => ns.HaveNotifications()).Returns(true);

            // Act
            _motorcycleUseCase.Create(request);

            // Assert
            _notificationServiceMock.Verify(ns => ns.Add(It.Is<Notification>(n => n.Message == "Placa não pode ser nulo ou vazio")), Times.Once);
            _repositoryMock.Verify(r => r.Create(It.IsAny<Motorcycle>()), Times.Never);
        }

        [Fact]
        public void Create_ShouldNotCreateMotorcycle_WhenPlateAlreadyExists()
        {
            var request = new PostMotorcycleRequest
            {
                Year = 2021,
                Model = "Model 1",
                Plate = "ABC1234"
            };

            _repositoryMock.Setup(r => r.Exists(It.IsAny<Predicate<Motorcycle>>())).Returns(true);

            _notificationServiceMock.Setup(ns => ns.HaveNotifications()).Returns(true);

            // Act
            _motorcycleUseCase.Create(request);

            // Assert
            _notificationServiceMock.Verify(ns => ns.Add(It.Is<Notification>(n => n.Message.Contains("Moto com a placa"))), Times.Once);
            _repositoryMock.Verify(r => r.Create(It.IsAny<Motorcycle>()), Times.Never);
        }

        [Fact]
        public void Create_ShouldCreateMotorcycle_WhenValidRequest()
        {
            var request = new PostMotorcycleRequest
            {
                Year = 2021,
                Model = "Model 1",
                Plate = "ABC1234"
            };

            _repositoryMock.Setup(r => r.Exists(It.IsAny<Predicate<Motorcycle>>())).Returns(false);

            _notificationServiceMock.Setup(ns => ns.HaveNotifications()).Returns(false);

            // Act
            _motorcycleUseCase.Create(request);

            // Assert
            _repositoryMock.Verify(r => r.Create(It.IsAny<Motorcycle>()), Times.Once);
        }

        [Fact]
        public void Update_ShouldAddNotification_WhenIdIsInvalid()
        {
            var request = new PatchMotorcycleRequest { Plate = "DEF5678" };

            _notificationServiceMock.Setup(ns => ns.HaveNotifications()).Returns(true);

            // Act
            _motorcycleUseCase.Update(-1, request);

            // Assert
            _notificationServiceMock.Verify(ns => ns.Add(It.Is<Notification>(n => n.Message == "Id não pode ser menor ou igual a zero")), Times.Once);
            _repositoryMock.Verify(r => r.GetFirst(It.IsAny<Func<Motorcycle, bool>>()), Times.Never);
            _repositoryMock.Verify(r => r.Update(It.IsAny<Motorcycle>()), Times.Never);
        }

        [Fact]
        public void Update_ShouldAddNotification_WhenPlateIsInvalid()
        {
            var request = new PatchMotorcycleRequest { Plate = null };

            _notificationServiceMock.Setup(ns => ns.HaveNotifications()).Returns(true);

            // Act
            _motorcycleUseCase.Update(1, request);

            // Assert
            _notificationServiceMock.Verify(ns => ns.Add(It.Is<Notification>(n => n.Message == "Placa não pode ser nulo ou vazio")), Times.Once);
            _repositoryMock.Verify(r => r.GetFirst(It.IsAny<Func<Motorcycle, bool>>()), Times.Never);
            _repositoryMock.Verify(r => r.Update(It.IsAny<Motorcycle>()), Times.Never);
        }

        [Fact]
        public void Update_ShouldAddNotification_WhenMotorcycleNotFound()
        {
            var request = new PatchMotorcycleRequest { Plate = "DEF5678" };

            _notificationServiceMock.Setup(ns => ns.HaveNotifications()).Returns(false);

            _repositoryMock.Setup(r => r.GetFirst(It.IsAny<Func<Motorcycle, bool>>())).Returns((Motorcycle)null);

            // Act
            _motorcycleUseCase.Update(1, request);

            // Assert
            _notificationServiceMock.Verify(ns => ns.Add(It.Is<Notification>(n => n.Message == "Moto não encontrada")), Times.Once);
            _repositoryMock.Verify(r => r.GetFirst(It.IsAny<Func<Motorcycle, bool>>()), Times.Once);
            _repositoryMock.Verify(r => r.Update(It.IsAny<Motorcycle>()), Times.Never);
        }

        [Fact]
        public void Update_ShouldUpdateMotorcycle_WhenValidRequest()
        {
            var request = new PatchMotorcycleRequest { Plate = "DEF5678" };
            var existingMotorcycle = new Motorcycle(1, 2021, "Model 1", "ABC1234");

            _notificationServiceMock.Setup(ns => ns.HaveNotifications()).Returns(false);

            _repositoryMock.Setup(r => r.GetFirst(It.IsAny<Func<Motorcycle, bool>>())).Returns(existingMotorcycle);

            // Act
            _motorcycleUseCase.Update(1, request);

            // Assert
            _notificationServiceMock.Verify(ns => ns.Add(It.IsAny<Notification>()), Times.Never);
            _notificationServiceMock.Verify(ns => ns.HaveNotifications(), Times.Once);
            _repositoryMock.Verify(r => r.Update(It.Is<Motorcycle>(m => m.Plate == "DEF5678")), Times.Once);
            _repositoryMock.Verify(r => r.GetFirst(It.IsAny<Func<Motorcycle, bool>>()), Times.Once);
        }

        [Fact]
        public void Delete_ShouldAddNotification_WhenIdIsInvalid()
        {
            // Act
            _motorcycleUseCase.Delete(-1);

            // Assert
            _notificationServiceMock.Verify(ns => ns.Add(It.Is<Notification>(n => n.Message == "Id não pode ser menor ou igual a zero")), Times.Once);
            _repositoryMock.Verify(r => r.GetFirst(It.IsAny<Func<Motorcycle, bool>>()), Times.Never);
            _repositoryMock.Verify(r => r.Delete(It.IsAny<Motorcycle>()), Times.Never);
        }

        [Fact]
        public void Delete_ShouldDeleteMotorcycle_WhenValidId()
        {
            var motorcycle = new Motorcycle(1, 2021, "Model 1", "ABC1234");

            _repositoryMock.Setup(r => r.GetFirst(It.IsAny<Func<Motorcycle, bool>>())).Returns(motorcycle);

            // Act
            _motorcycleUseCase.Delete(1);

            // Assert
            _repositoryMock.Verify(r => r.GetFirst(It.IsAny<Func<Motorcycle, bool>>()), Times.Once);
            _repositoryMock.Verify(r => r.Delete(It.Is<Motorcycle>(m => m.Id == 1)), Times.Once);
            _notificationServiceMock.Verify(ns => ns.Add(It.IsAny<Notification>()), Times.Never);
        }
    }
}
