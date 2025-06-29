using BL;
using BL.Api;
using BL.Models;
using Dal;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Unicode;

public class Startup
{
    public IConfiguration Configuration { get; }

    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        string connectionString = Configuration.GetConnectionString("DefaultConnection");
        services.AddDalServices(connectionString);
        services.AddBlServices();

        services.AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
            });

        var jwtSection = Configuration.GetSection("Jwt");
        var key = Encoding.UTF8.GetBytes(jwtSection["Key"]);
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSection["Issuer"],
                    ValidAudience = jwtSection["Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                };
            });

        services.AddAuthorization(options =>
        {
            options.AddPolicy("AdminOnly", policy =>
                policy.RequireClaim(ClaimTypes.Role, "Admin"));
        });

        services.Configure<OpenAiSettings>(Configuration.GetSection("OpenAiSettings"));
        services.AddHttpClient();
        services.AddEndpointsApiExplorer();

        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "AI Driven Learning API", Version = "v1" });

            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "הזן 'Bearer' ולאחר מכן רווח ואת אסימון ה-JWT.\nלדוגמה: 'Bearer 12345abcdef'"
            });

            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    new string[] {}
                }
            });
        });

        // ======================================================================
        //                              התיקון כאן
        // ======================================================================
        // הוספנו פוליסה מאוד פתוחה רק לצורך בדיקה, כדי לראות אם זה פותר את הבעיה
        // אם זה עובד, נדע שהבעיה היא באופן שבו השרת מזהה את הכתובת הספציפית
        // ======================================================================
        services.AddCors(options =>
        {
            options.AddPolicy("AllowAllOriginsForTesting",
                builder => builder.AllowAnyOrigin() // שינוי מרכזי: מאפשר גישה מכל מקור
                                  .AllowAnyMethod()
                                  .AllowAnyHeader());
        });
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "AI Driven Learning API v1"));
        }

        //app.UseHttpsRedirection();
        app.UseRouting();

        // ======================================================================
        //                              התיקון כאן
        // ======================================================================
        // השתמשנו בשם הפוליסה החדשה שיצרנו לצורך הבדיקה
        // ======================================================================
        app.UseCors("AllowAllOriginsForTesting");

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}