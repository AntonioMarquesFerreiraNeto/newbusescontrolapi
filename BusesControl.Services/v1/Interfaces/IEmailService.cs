namespace BusesControl.Services.v1.Interfaces;

public interface IEmailService
{
    bool SendEmailStepCode(string email, string name, string code);
    bool SendEmailForWelcomeUserRegistration(string email, string name);
}
