public interface IUserRepository
{
    bool AddUser(User user);
}

public interface IEmailService
{
    void SendEmail(string to, string subject, string body);
}

public class User
{
    public string Email { get; set; }
    public string Name { get; set; }
    public bool IsBusinessPartner { get; set; }
}

public class UserManagerService
{
    private readonly IUserRepository _userRepository;
    private readonly IEmailService _emailService;

    public UserManagerService(IUserRepository userRepository, IEmailService emailService)
    {
        _userRepository = userRepository;
        _emailService = emailService;
    }

    public bool AddUser(User user)
    {
        if (user == null)
            throw new ArgumentNullException(nameof(user));
        
        var result = _userRepository.AddUser(user);
        if (result)
        {
            if(user.IsBusinessPartner)
                _emailService.SendEmail(user.Email, "A warm welcome!", "Thank you for registering with us as a business partner.");
            else
                _emailService.SendEmail(user.Email, "Welcome!", "Thank you for registering with us.");
        }
        return result;
    }
}
