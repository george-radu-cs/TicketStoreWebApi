using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using TicketStore.Entities;
using TicketStore.Managers;
using TicketStore.Repositories;

namespace TicketStore
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        private IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "TicketStore", Version = "v1" });
            });

            // connection to database
            var connectionString = Configuration.GetSection("Database").GetSection("ConnectionString").Get<String>();
            services.AddDbContext<TicketStoreContext>(options => options.UseSqlServer(connectionString));

            services.AddIdentity<User, Role>().AddEntityFrameworkStores<TicketStoreContext>();

            services
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer("AuthScheme", options =>
                {
                    options.SaveToken = true;
                    var jwtSecretKey = Configuration.GetSection("Jwt").GetSection("SecretKey").Get<string>();
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        ValidateLifetime = true,
                        RequireExpirationTime = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecretKey)),
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ClockSkew = TimeSpan.Zero
                    };
                });

            // types of user authorization
            services.AddAuthorization(opt =>
            {
                opt.AddPolicy("Buyer",
                    policy => policy.RequireRole("Buyer").RequireAuthenticatedUser()
                        .AddAuthenticationSchemes("AuthSchema").Build());
                opt.AddPolicy("Organizer",
                    policy => policy.RequireRole("Organizer").RequireAuthenticatedUser()
                        .AddAuthenticationSchemes("AuthSchema").Build());
                opt.AddPolicy("Admin",
                    policy => policy.RequireRole("Admin").RequireAuthenticatedUser()
                        .AddAuthenticationSchemes("AuthSchema").Build());
            });
            
            // configure JSON serialization
            services.AddControllersWithViews()
                .AddNewtonsoftJson(options =>
                    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

            services.AddTransient<IAuthenticationManager, AuthenticationManager>();
            services.AddTransient<ITokenManager, TokenManager>();

            services.AddTransient<IEventRepository, EventRepository>();
            services.AddTransient<IEventManager, EventManager>();

            services.AddTransient<ITicketRepository, TicketRepository>();
            services.AddTransient<ITicketManager, TicketManager>();

            services.AddTransient<IReviewRepository, ReviewRepository>();
            services.AddTransient<IReviewManager, ReviewManager>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "TicketStore v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}