using System.Text;
using MultiMart.Application.Common.Mailing;
using RazorEngineCore;

namespace MultiMart.Infrastructure.Mailing;

public class EmailTemplateService : IEmailTemplateService
{
    public string GenerateEmailTemplate<T>(string templateName, T mailTemplateModel)
    {
        string template = GetTemplate(templateName);

        IRazorEngine razorEngine = new RazorEngine();
        var modifiedTemplate = razorEngine.Compile(template);

        return modifiedTemplate.Run(mailTemplateModel);
    }

    public static string GetTemplate(string templateName)
    {
        string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
        string tmplFolder = Path.Combine(baseDirectory, "EmailTemplates");
        string filePath = Path.Combine(tmplFolder, $"{templateName}.cshtml");

        using var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
        using var sr = new StreamReader(fs, Encoding.Default);
        string mailText = sr.ReadToEnd();
        sr.Close();

        return mailText;
    }
}