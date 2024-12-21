namespace Acme.Application.Interfaces;

public interface IValidator
{
    string GenerateValidationCode();
    string HashPassword(string input);
}