using System.Text;
namespace LeadsApi.Services;

public class FileEmailFake : IEmailFake
{
    public Task SendAsync(string to, string subject, string body)
    {
        var outbox = Path.Combine(AppContext.BaseDirectory, "outbox");
        Directory.CreateDirectory(outbox);

        var safeTo = to.Replace("@","_").Replace(".","_");
        var file = Path.Combine(outbox, $"{safeTo}_{DateTime.UtcNow:yyyyMMdd_HHmmss}.txt");

        var content = $"TO: {to}{Environment.NewLine}SUBJECT: {subject}{Environment.NewLine}{Environment.NewLine}{body}";
        return File.WriteAllTextAsync(file, content, Encoding.UTF8);
    }
}
