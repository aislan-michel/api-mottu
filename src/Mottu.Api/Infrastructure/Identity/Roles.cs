namespace Mottu.Api.Infrastructure.Identity;

public static class Roles
{
    public const string Admin = "admin";
    public const string Entregador = "entregador";

    public static string GetRoles() => string.Join(",", Admin, Entregador);
}