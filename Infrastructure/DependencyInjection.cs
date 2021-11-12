using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Infrastructure.EmailNotifications;
using Infrastructure.Notifications;
using Microsoft.AspNetCore.Builder;
using WebSocketManager;
using System;
using Infrastructure.Chat;

namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection ConfigureInfrastructure(this IServiceCollection services,
                    IConfiguration configuration)
        {
            services.AddSingleton<EmailSender>();
            services.AddSingleton<INotificator<bool>, EmailNotificator>();


            services.AddTransient<ChatHandler>();
            services.AddWebSocketManager();
            services.AddSingleton<ChatManager>();

            return services;
        }

        public static void ConfigureChat(this IApplicationBuilder app, IServiceProvider provider)
        {
            app.UseWebSockets();
            app.MapWebSocketManager("/chat", provider.GetService<ChatHandler>()); // Dependency Injection
        }
    }
}