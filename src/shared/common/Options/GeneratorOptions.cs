namespace Common.Options;

public class GeneratorOptions
{
    public string PrivateKeyFilePath { get; set; }
    public string CertificateFilePath { get; set; }
    public string InvoiceRootPath { get; set; }
    
    public string GetPrivateKeyFilePath()
    {
        return Path.Combine(AppDomain.CurrentDomain.BaseDirectory,"Files", PrivateKeyFilePath);
    }
    // Create GetCertificateFilePath() method
    public string GetCertificateFilePath()
    {
        return Path.Combine(AppDomain.CurrentDomain.BaseDirectory,"Files", CertificateFilePath);
    }
    public string GetInvoiceRootPath()
    {
        return Path.Combine(AppDomain.CurrentDomain.BaseDirectory,InvoiceRootPath,DateTime.Now.ToString("yyyy-MM-dd"));
    }
}