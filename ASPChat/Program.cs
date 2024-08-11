using ASPChat.BuisnessLayer.Auth.Implementations;
using ASPChat.BuisnessLayer.Auth.Interfaces;
using ASPChat.BuisnessLayer.Chat.Implementation;
using ASPChat.BuisnessLayer.Chat.Interfaces;
using ASPChat.DAL.DalHelpers;
using ASPChat.DAL.Implementations;
using ASPChat.DAL.Interfaces;
using System.Data;
using System.Data.Common;

namespace ASPChat
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddControllersWithViews();
            builder.Services.AddControllers();
            builder.Services.AddSession();
            builder.Services.AddSignalR();
            builder.Services.AddTransient<IAuthenticationDAL, AuthenticationDAL>();
            builder.Services.AddSingleton<IDbConnectionFactory, DbConnectionFactory>();
            builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            builder.Services.AddSingleton<IEncrypt, Encrypt>();
            builder.Services.AddTransient<IAuthentication, Authentication>();
            builder.Services.AddTransient<IChatManager, ChatManager>();
            builder.Services.AddTransient<IChatDAL, ChatDAL>();
            

            var app = builder.Build();
            app.UseSession();
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthorization();
            app.MapHub<ChatHub.ChatHub>("/chat/signal");

            app.MapControllerRoute("default", "{controller=Chat}/{action=Index}/{id?}");
            app.Use(async (context, next) =>
            {
                if(context.Request.Path == "/")
                {
                    context.Response.Redirect("/Chat");
                }
                else
                {
                    await next();
                }
            });
            app.Run();
        }
    }
}
