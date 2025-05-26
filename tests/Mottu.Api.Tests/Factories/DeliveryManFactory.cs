using Mottu.Api.Domain.Entities;

namespace Mottu.Api.Tests.Factories
{
    public static class DeliveryManFactory
    {
        public static DeliveryMan DeliveryManWithoutDriverLicenseImage()
        {
            return new DeliveryMan(
                "Fulano da Silva", "09556767000176",
                new DateOnly(1999, 12, 28),
                new DriverLicense("12227462477", "A", null),
                Guid.NewGuid().ToString()
            );
        }
    }
}