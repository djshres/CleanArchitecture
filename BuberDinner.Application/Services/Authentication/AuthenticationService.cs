using BuberDinner.Application.Common.Errors;
using BuberDinner.Application.Common.Interfaces.Authentication;
using BuberDinner.Application.Persistence;
using BuberDinner.Domain.Common.Errors;
using BuberDinner.Domain.Entities;
using ErrorOr;

namespace BuberDinner.Application.Services.Authentication;

public class AuthenticationService : IAuthenticationService
{
    private readonly IJwtTokengenerator _jwtTokengenerator;
    private readonly IUserRepository _userRepository;

    public AuthenticationService(IJwtTokengenerator jwtTokengenerator, IUserRepository userRepository)
    {
        _jwtTokengenerator = jwtTokengenerator;
        _userRepository = userRepository;
    }

    public ErrorOr<AuthenticationResult> Register(string firstName, string lastName, string email, string password)
    {
        if (_userRepository.GetUserByEmail(email) is not null)
        {
            return Errors.User.DuplicateEmail;
        }

        var user = new User
        {
            FirstName = firstName,
            LastName = lastName,
            Email = email,
            Password = password
        };
        _userRepository.Add(user);

        var token = _jwtTokengenerator.GenerateToken(user);

        return new AuthenticationResult(
           user,
           token);
    }

    public ErrorOr<AuthenticationResult> Login(string email, string password)
    {
        if (_userRepository.GetUserByEmail(email) is not User user)
        {
            return Errors.Authentication.InvalidCredentials;
        }

        if (user.Password != password)
        {
            return Errors.Authentication.InvalidCredentials;
        }

        var token = _jwtTokengenerator.GenerateToken(user);

        return new AuthenticationResult(
            user,
            token);
    }
}