namespace LeadsApi.Services;

public interface IEmailFake
{
    Task SendAsync(string to, string subject, string body);
}
