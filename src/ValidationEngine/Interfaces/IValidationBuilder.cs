using ValidationEngine.Models;

namespace ValidationEngine.Interfaces;

public interface IValidationBuilder
{
    ValidationEngineResult Validate<T>(T request) where T : ISetting;
}