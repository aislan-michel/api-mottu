namespace Mottu.Api.Application.Interfaces;

public interface ILoggedUserService
{
    string? UserId { get; }
    string? DeliveryManId { get; }
}