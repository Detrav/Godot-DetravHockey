namespace DetravHockey.Server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddSignalR(options =>
            {

            });



            var app = builder.Build();

            // Configure the HTTP request pipeline.
            app.UseWebSockets();
            //app.UseRouting();

            app.UseMiddleware<DetravHockeyMiddleware>();

            app.Run();

        }
    }
}
