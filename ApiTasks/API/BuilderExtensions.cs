﻿using Application.Interfaces.Repository;
using Application.Interfaces.UnitOfWork;
using Application.Mappings;
using Application.Services;
using Application.Services.Interfaces;
using Application.UseCases.UsersUseCase.Commands;
using Application.UseCases.UsersUseCase.Validators;
using FluentValidation;
using FluentValidation.AspNetCore;
using Infrastructure.Persistence;
using Infrastructure.Persistence.Repository;
using Infrastructure.Persistence.UnitOfWork;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text;

namespace API
{
    public static class BuilderExtensions
    {
        public static void AddServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
        }

        public static void AddSwaggerDocs(this WebApplicationBuilder builder)
        {
            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Tasks App",
                    Description = "Tasks application based on Trello and made with ASP.NET",
                    Contact = new OpenApiContact
                    {
                        Name = "Exemplo de página de contato",
                        Url = new Uri("https://example.com/contact") 
                    },
                    License = new OpenApiLicense
                    {
                        Name = "Example of license page",
                        Url = new Uri("https://example.com/license")
                    }
                });
                var xmlFileName = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFileName));
            });
        }

        public static void AddDatabase(this WebApplicationBuilder builder)
        {
            var configuration = builder.Configuration;
            builder.Services.AddDbContext<TasksDbContext>(options => options
                            .UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
        }

        public static void AddMediatrCommands(this WebApplicationBuilder builder)
        {
            builder.Services.AddMediatR(config => 
            config.RegisterServicesFromAssemblies(typeof(CreateUserCommand).Assembly));
        }

        public static void AddFluentValidationValidators(this WebApplicationBuilder builder)
        {
            builder.Services.AddValidatorsFromAssemblyContaining<CreateUserCommandValidator>();
            builder.Services.AddFluentValidationAutoValidation();
        }

        public static void AddMapper(this WebApplicationBuilder builder)
        {
            builder.Services.AddAutoMapper(typeof(ProfileMappings).Assembly);
        }

        public static void AddJwtAuth(this WebApplicationBuilder builder)
        {
            var configuration = builder.Configuration;

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidIssuer = configuration["JWT:Issuer"],
                    ValidAudience = configuration["JWT:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Key"]!))
                };
            })
            .AddCookie(options =>
            {
                options.Cookie.HttpOnly = true;
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                options.Cookie.SameSite = SameSiteMode.Strict;
                options.ExpireTimeSpan = TimeSpan.FromDays(int.Parse(configuration["JWT:CookiesExpirationTime"]!));
            });
        }

        public static void AddScopedServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddScoped<IAuthService, AuthService>();

            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
        }
    }
}
