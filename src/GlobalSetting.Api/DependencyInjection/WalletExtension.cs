namespace Wallet.Api.DependencyInjection;

public static class WalletExtension
{
    public static IServiceCollection AddWalletDependency(this IServiceCollection services,
        IConfiguration configuration)
    {
        return services;
    }
}