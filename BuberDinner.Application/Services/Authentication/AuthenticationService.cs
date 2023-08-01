using BuberDinner.Application.Common.Interfaces.Authentication;
using BuberDinner.Application.Persistence;
using BuberDinner.Domain.Entities;

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

    public AuthenticationResult Register(string firstName, string lastName, string email, string password)
    {
        if (_userRepository.GetUserByEmail(email) is not null)
        {
            throw new Exception("User with given email already exists.");
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

    public AuthenticationResult Login(string email, string password)
    {
        if (_userRepository.GetUserByEmail(email) is not User user)
        {
            throw new Exception("User with given email does not exist.");
        }

        if (user.Password != password)
        {
            throw new Exception("Invalid Password");
        }

        var token = _jwtTokengenerator.GenerateToken(user);

        return new AuthenticationResult(
            user,
            token);
    }
}