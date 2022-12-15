using Microsoft.EntityFrameworkCore;
using Serilog;
using UserBlogApp.Data;

namespace UserBlogApp
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(builder.Configuration)
                .CreateLogger();

            try
            {
                await Setup(builder);
            }
            catch (Exception exception)
            {
                Log.Fatal(exception, "Application terminated unexpectedly!");
            }
            finally
            {
                await Log.CloseAndFlushAsync();
            }
        }

        private static async Task Setup(WebApplicationBuilder builder)
        {
            builder.Host.UseSerilog();

            if (builder.Environment.IsDevelopment())
            {
                builder.Services.AddDbContext<AppDbContext>(opts => opts.UseInMemoryDatabase("InMem"));
            }
            else
            {
                string sqlServerConnection = builder.Configuration.GetConnectionString("SqlServer");
                builder.Services.AddDbContext<AppDbContext>(opts => opts.UseSqlServer(sqlServerConnection));
            }

            builder.Services.AddScoped<IUsersRepository, UsersRepository>();
            builder.Services.AddScoped<IPostsRepository, PostsRepository>();

            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            app.UseSerilogRequestLogging();

            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            await AppDbContextSeed.Seed(app, app.Environment);

            app.Run();
        }
    }
}