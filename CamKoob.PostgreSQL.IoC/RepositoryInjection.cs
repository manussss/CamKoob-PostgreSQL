namespace CamKoob.PostgreSQL.IoC;

public static class RepositoryInjection
{
    public static void AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IOrderRepository, OrderRepository>();
    }
}