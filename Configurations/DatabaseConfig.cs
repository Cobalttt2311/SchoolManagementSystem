// Lokasi: /Configurations/DatabaseConfig.cs
using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Data; // <-- TAMBAHKAN BARIS INI

namespace SchoolManagementSystem.Configurations
{
    public static class DatabaseConfig
    {
        public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            // Ambil connection string dari environment variable
            var connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING");

            // Daftarkan DbContext dengan Npgsql provider
            services.AddDbContext<AppDbContext>(options =>
                options.UseNpgsql(connectionString));

            return services;
        }
    }
}