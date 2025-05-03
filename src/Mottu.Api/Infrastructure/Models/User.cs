namespace Mottu.Api.Infrastructure.Models;

public class User
{
    public User(Email email, string role)
    {
        Id = Guid.NewGuid().ToString();
        Email = email;
        Role = role;
    }

    public string Id { get; private set; }
    public Email Email { get; private set; }
    public string HashPassword { get; private set; }
    public bool EmailConfirmed { get; private set; }
    public string Role { get; set; }

    public void SetHashPassword(string hashPassword)
    {
        HashPassword = hashPassword;
    }

    public bool CheckPassword(string hashPassword)
    {
        return HashPassword == hashPassword;
    }
}

public class Email
{
    public Email(string address)
    {
        Address = address;
        Confirmed = false;
    }

    public string Address { get; private set; }
    public bool Confirmed { get; private set; }

    public void Confirm()
    {
        Confirmed = true;
    }
}