using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ThiHuong.Framework.Helpers;
using ThiHuong.Framework.Models;
using ThiHuong.Service;
using Microsoft.AspNetCore.Cors.Infrastructure;
using ThiHuong.Logic.BaseRepository;
using ThiHuong.Framework.ViewModels;
using ThiHuong.Logic.QuestionService;
using ThiHuong.Logic.ExamService;
using ThiHuong.Framework.ViewModels.EntityViewModel;
using ThiHuong.Logic;
using Swashbuckle.AspNetCore.Swagger;

namespace ThiHuong.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddDbContext<ThiHuongDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("ThiHuongDb"));
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "ThiHuong API", Version = "v1" });
            });

            SetupAuthentication(services);
            SetupAuthorization(services);
            SetupAutoMapper();
            SetupDependencyInjection(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }
            app.UseCors(options =>
            {
                options.AllowAnyMethod();
                options.AllowAnyOrigin();
                options.AllowAnyHeader();
            });

            app.UseStaticFiles();

            app.UseHttpsRedirection();
            app.UseMvc();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Thi Huong API");
            });
        }

        private void SetupAutoMapper()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<UserRegisterdViewModel, Account>().ForMember(u => u.Role, options => options.Ignore());

                cfg.CreateMap<Question, QuestionViewModel>();
                cfg.CreateMap<QuestionViewModel, Question>();

                cfg.CreateMap<Exam, ExamViewModel>();
                cfg.CreateMap<ExamViewModel, Exam>();

                cfg.CreateMap<SubmitAnswerViewModel, ResultDetail>();
                cfg.CreateMap<ResultDetail, SubmitAnswerViewModel>();

            });

        }

        private void SetupDependencyInjection(IServiceCollection services)
        {
            services.AddHttpContextAccessor();

            //services.AddSingleton<AppSettings>(appSettings);
            services.AddSingleton<IConfiguration>(Configuration);

            services.AddScoped<ExtensionSettings>();
            services.AddScoped<JwtSecurityTokenProvider>();

            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IQuestionService, QuestionService>();
            services.AddScoped<IExamService, ExamService>();
            services.AddScoped<IResultDetailService, ResultDetailService>();
            services.AddScoped<UnitOfWork>();

        }

        private void SetupAuthentication(IServiceCollection services)
        {
            var appSettingsSection = Configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingsSection);

            var appSettings = appSettingsSection.Get<AppSettings>();
            var key = Encoding.ASCII.GetBytes(appSettings.SecretKey);
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });
        }

        private void SetupAuthorization(IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
                options.AddPolicy("ADMIN", policy =>
                {
                    policy.RequireAuthenticatedUser();
                    policy.AuthenticationSchemes = new List<string> { JwtBearerDefaults.AuthenticationScheme };
                    policy.RequireClaim(ClaimTypes.Roles, "ADMIN");
                });

                options.AddPolicy("USER", policy =>
                {
                    policy.RequireAuthenticatedUser();
                    policy.AuthenticationSchemes = new List<string> { JwtBearerDefaults.AuthenticationScheme };
                    policy.RequireClaim(ClaimTypes.Roles, "USER");
                });
            });

        }
    }
}
