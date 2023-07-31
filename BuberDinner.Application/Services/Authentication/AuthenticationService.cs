using BuberDinner.Application.Common.Interfaces.Authentication;

namespace BuberDinner.Application.Services.Authentication;

public class AuthenticationService : IAuthenticationService
{
    private readonly IJwtTokengenerator _jwtTokengenerator;

	public AuthenticationService(IJwtTokengenerator jwtTokengenerator)
	{
		_jwtTokengenerator = jwtTokengenerator;
	}

	public AuthenticationResult Register(string firstName, string lastName, string email, string password)
    {
        //Create Jwt token
        Guid userId = Guid.NewGuid();
        var token = _jwtTokengenerator.GenerateToken(userId, firstName, lastName);

        return new AuthenticationResult(
            Guid.NewGuid(),
            firstName, 
            lastName,
            email,
            token);
    }

    public AuthenticationResult Login(string email, string password)
    {
        return new AuthenticationResult(
        Guid.NewGuid(),
            "firstName", 
            "lastName",
            email,
            "token");
    }
}