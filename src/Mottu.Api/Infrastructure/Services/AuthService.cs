using Microsoft.AspNetCore.Identity;

using Mottu.Api.Application.Models;
using Mottu.Api.Domain.Interfaces;
using Mottu.Api.Infrastructure.Interfaces;
using Mottu.Api.Infrastructure.Models;

namespace Mottu.Api.Infrastructure.Services;

public class AuthService(IPasswordHasher<User> passwordHasher, IRepository<User> userRepository, ITokenService tokenService) : IAuthService
{
    private readonly IPasswordHasher<User> _passwordHasher = passwordHasher;
    private readonly IRepository<User> _userRepository = userRepository;
    private readonly ITokenService _tokenService = tokenService;

    public Result<string> Register(RegisterUserRequest request)
    {
        var user = new User(new Email(request.Email), request.Role);

        var hashPassword = _passwordHasher.HashPassword(user, request.Password);

        user.SetHashPassword(hashPassword);

        _userRepository.Create(user);

        return Result<string>.Ok("Usuário registrado com sucesso");
    }

    public Result<string> Login(LoginUserRequest request)
    {
        var user = _userRepository.GetFirst(x => x.Email.Address == request.Email);

        if(user == null)
        {
            return Result<string>.Fail("Usuário não encontrado");
        }

        var hashPassword = _passwordHasher.HashPassword(user, request.Password);

        System.Console.WriteLine($@"
            senha do user: {user.HashPassword}
            senha digitada pós hash: {hashPassword}
        ");

        if(!user.CheckPassword(hashPassword))
        {
            return Result<string>.Fail("Senha incorreta");
        }

        var token = _tokenService.GenerateToken(user.Role);

        return Result<string>.Ok(token);
    }
}