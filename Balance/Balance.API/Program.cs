
using Balance.Domain.Interfaces;
using Balance.Infra.Data;
using Balance.Infra.Messaging;
using Balance.Infra.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Balance.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // Add DbContext
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseMySql(connectionString, new MySqlServerVersion(new Version(5, 7, 44)),
                    mySqlOptions => mySqlOptions.EnableRetryOnFailure(
                        maxRetryCount: 5,
                        maxRetryDelay: TimeSpan.FromSeconds(10),
                        errorNumbersToAdd: null
                    )));

            builder.Services.AddScoped<IBalanceRepository, BalanceRepository>();


            // Add Kafka Consumer
            builder.Services.AddHostedService<KafkaConsumer>();

            var app = builder.Build();

            // Apply migrations automatically
            //using (var scope = app.Services.CreateScope())
            //{
            //    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            //    db.Database.Migrate();
            //    SeedData(db);
            //}


            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }

        private static void SeedData(ApplicationDbContext dbContext)
        {
            if (!dbContext.Balances.Any())
            {
                dbContext.Balances.Add(new Domain.Entities.Balance
                {
                    Id = Guid.NewGuid().ToString(),
                    AccountId = "sample-account-id",
                    Amount = 1000,
                    LastUpdated = DateTime.UtcNow
                });
                dbContext.SaveChanges();
            }
        }
    }
}
