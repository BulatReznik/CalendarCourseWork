using CalendarCourseWork.Security;
using Microsoft.OpenApi.Models;
using NLog;
using NLog.Config;
using NLog.Targets;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using CalendarCourseWork.BusinessLogic;
using Microsoft.EntityFrameworkCore;
using CalendarCourseWork.Logic;
using CalendarCourseWork.BusinessLogic.Storages;
using CalendarCourseWork.BusinessLogic.Models;
using Hangfire;
using Hangfire.SqlServer;
using System.Configuration;

namespace CalendarCourseWork
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHangfire(x => x.UseSqlServerStorage(Configuration.GetConnectionString("CalendarCourseWorkContext")));

            services.AddHangfireServer();

            services.AddTransient<JWTUser>();
            services.AddTransient<UsersManager>();
            services.AddTransient<UsersDataAccess>();

            services.AddTransient<CategoriesManager>();
            services.AddTransient<CategoriesDataAccess>();

            services.AddTransient<EventsDataAccess>();
            services.AddTransient<EventsManager>();

            services.AddTransient<JWTUser>();

            services.AddDbContext<CalendarCourseWorkContext>(options =>
             options.UseSqlServer(Configuration.GetConnectionString("CalendarCourseWorkContext") ??
                                throw new InvalidOperationException("Connection string 'CalendarCourseWorkContext' not found.")));

            services.AddControllers().AddNewtonsoftJson();

            services.AddDistributedMemoryCache();

            services.AddControllersWithViews();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    // укзывает, будет ли валидироваться издатель при валидации токена
                    ValidateIssuer = true,
                    // строка, представляющая издателя
                    ValidIssuer = AuthOptions.ISSUER,

                    // будет ли валидироваться потребитель токена
                    ValidateAudience = true,
                    // установка потребителя токена
                    ValidAudience = AuthOptions.AUDIENCE,
                    // будет ли валидироваться время существования
                    ValidateLifetime = true,

                    // установка ключа безопасности
                    IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(),
                    // валидация ключа безопасности
                    ValidateIssuerSigningKey = true,
                };
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "MOIORestAPI",
                    Version = "v1"
                });
                // Создание и добавление описания схемы безопасности "Bearer"
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "",                    // Описание схемы безопасности (пустая строка)
                    Name = "Authorization",              // Имя параметра заголовка, который будет использоваться для передачи токена
                    In = ParameterLocation.Header,       // Местоположение параметра - в заголовке запроса
                    Type = SecuritySchemeType.ApiKey,    // Тип схемы безопасности - API-ключ (API Key)
                    Scheme = "Bearer"                   // Название схемы безопасности - "Bearer" (часто используется для токенов)
                });

                // Создание и добавление требования безопасности
                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "oauth2",          // Название схемы безопасности - "oauth2"
                            Name = "Bearer",           // Имя параметра заголовка, который будет использоваться для передачи токена
                            In = ParameterLocation.Header,  // Местоположение параметра - в заголовке запроса
                        },
                        new List<string>()   // Перечисление разрешений (в данном случае пустой список строк)
                    }
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the
        //HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            var nLogConfig = new LoggingConfiguration();
            var logConsole = new ConsoleTarget();
            nLogConfig.AddRule(NLog.LogLevel.Info, NLog.LogLevel.Error, logConsole);
            LogManager.Configuration = nLogConfig;

            app.UseHangfireDashboard();
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "MOIORestApi v1"));

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
