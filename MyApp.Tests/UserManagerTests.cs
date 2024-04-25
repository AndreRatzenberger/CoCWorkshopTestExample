using FakeItEasy;
using Xunit;

public class UserManagerTests
{


    [Fact]
    public void AddUser_Ensure_That_BusinessPartner_Gets_Wholesome_Email()
    {
        // Arrange
        var fakeRepository = A.Fake<IUserRepository>();
        var fakeEmailService = A.Fake<IEmailService>();

        var userManager = new UserManagerService(fakeRepository, fakeEmailService);
        var user = new User { Email = "test@example.com", Name = "Test User", IsBusinessPartner = true};

        A.CallTo(() => fakeRepository.AddUser(user)).Returns(true);
        // Act
        bool result = userManager.AddUser(user);
        // Assert
        Assert.True(result);
        A.CallTo(() => fakeEmailService.SendEmail(user.Email, "A warm welcome!", 
        "Thank you for registering with us as a business partner."))
        .MustHaveHappenedOnceExactly();;
    }
    [Fact]
    public void AddUser_Ensure_That_NonValidUser_Throws_Exception()
    {
        // Arrange
        var fakeRepository = A.Fake<IUserRepository>();
        var fakeEmailService = A.Fake<IEmailService>();

        var userManager = new UserManagerService(fakeRepository, fakeEmailService);

        // Act
        // Assert
        Assert.Throws<ArgumentNullException>(() => userManager.AddUser(null));
    }

    [Fact]
    public void AddUser_UserIsAdded_SendWelcomeEmail()
    {
        // Arrange
        var fakeRepository = A.Fake<IUserRepository>();
        var fakeEmailService = A.Fake<IEmailService>();
        var userManager = new UserManagerService(fakeRepository, fakeEmailService);
        var user = new User { Email = "test@example.com", Name = "Test User" };

        A.CallTo(() => fakeRepository.AddUser(user)).Returns(true);

        // Act
        bool result = userManager.AddUser(user);

        // Assert
        Assert.True(result);

        A.CallTo(() => fakeEmailService.SendEmail(user.Email, "Welcome!", "Thank you for registering with us."))
        .MustHaveHappenedOnceExactly();
    }

    [Fact]
    public void AddUser_AddingUserFails_NoEmailSent()
    {
        // Arrange
        var fakeRepository = A.Fake<IUserRepository>();
        var fakeEmailService = A.Fake<IEmailService>();
        var userManager = new UserManagerService(fakeRepository, fakeEmailService);
        var user = new User { Email = "fail@example.com", Name = "Fail User" };

        A.CallTo(() => fakeRepository.AddUser(user)).Returns(false);

        // Act
        bool result = userManager.AddUser(user);

        // Assert
        Assert.False(result);
        A.CallTo(() => fakeEmailService.SendEmail(A<string>.Ignored, A<string>.Ignored, A<string>.Ignored)).MustNotHaveHappened();
    }
}