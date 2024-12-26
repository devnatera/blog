using BlogApp.Server.Infra.Repository;

namespace BlogApp.Server.Config
{
    public static class RepositoryConfig
    {
        public static void ConfigureRepositories(this IServiceCollection services)
        {
            services.AddScoped<BlogRepository>();
        }
    }
}
