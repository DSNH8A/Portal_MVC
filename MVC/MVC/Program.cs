using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MVC.Data;
using MVC.Interface;
using MVC.Repository;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Reflection;
using System.Xml;
using Microsoft.AspNetCore.Authentication.Facebook;
using MVC.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Dependency;
using System.Security.Claims;
using MVC.Controllers;
using MVC.Services;

var builder = WebApplication.CreateBuilder(args);

/*-----XML DOCUMENTATION-----*/
var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
/*---------------------------*/


/*-----GOOGLE AUTHENTICATION-----*/
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
}).AddCookie().AddGoogle(GoogleDefaults.AuthenticationScheme, options =>
{
    options.ClientId = builder.Configuration.GetSection("GoogleKeys:ClientId").Value;
    options.ClientSecret = builder.Configuration.GetSection("GoogleKeys:ClientSecret").Value;
});
/*--------------------------------*/

/*-----FACEBOOK AUTHENTICATION-----*/
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = FacebookDefaults.AuthenticationScheme;
}).AddFacebook(FacebookDefaults.AuthenticationScheme, options =>
{
    options.AppId = builder.Configuration.GetSection("FacebookKeys:AppId").Value;
    options.AppSecret = builder.Configuration.GetSection("FacebookKeys:AppSecret").Value;
});
/*---------------------------------*/

/*-----GITHUB AUTHENTICATION-----*/
builder.Services.AddAuthentication()
    .AddOAuth("github", options =>
    {
        options.ClientId = builder.Configuration.GetSection("GitHubKeys:ClientId").Value;
        options.ClientSecret = builder.Configuration.GetSection("GitHubKeys:ClientSecret").Value;
        options.AuthorizationEndpoint = "https://github.com/login/oauth/authorize";
        options.TokenEndpoint = "https://github.com/login/oauth/access_token";
        options.UserInformationEndpoint = "https://api.github.com/user";
        //options.CallbackPath = "/oauth/github-cb";
        options.CallbackPath = "/home/create";
        options.SaveTokens = false;
    });
/*------------------------------*/

builder.Services.Configure<CloudinarySettings>(builder.Configuration.GetSection("CloudinarySettings"));
builder.Services.AddControllersWithViews();

/*-----DATABASE IMPLEMENTATION-----*/
builder.Services.AddDbContext<MVC_Context>(options =>

options.UseSqlServer(builder.Configuration.GetConnectionString("Default"))
);
/*---------------------------------*/

builder.Services.AddRouting(options =>
{
    options.LowercaseUrls = true;
    options.AppendTrailingSlash = true;
});

/*-----SWAGGER API SETUP-----*/
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
        {
        Title = "Employee API",
        Version = "v1",
        Description = "An API to perform Emloyee operations",
        TermsOfService = new Uri("https://example.com/terms"),
        Contact = new Microsoft.OpenApi.Models.OpenApiContact
            {
            Name = "John Walker",
            Email = "JohnWalker@gmail.com",
            Url = new Uri("https://twitter.com/jwalker"),
            },
        License = new Microsoft.OpenApi.Models.OpenApiLicense
            {
            Name = "Employee API LICX",
            Url = new Uri("https://example.com/license"),
            },
        });
    c.IncludeXmlComments(xmlPath);
});
/*------------------------*/

/*-----SESSION SETUP-----*/
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
/*-----------------------*/

builder.Services.AddControllers();
builder.Services.AddScoped<ICourseRepository, CourseRepository>();
builder.Services.AddScoped<IMaterialRepository, MaterialRepository>();
builder.Services.AddScoped<ISkillRepository, SkillRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IPhotoService, PhotoService>();


builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.SameSite = SameSiteMode.None;
    options.Cookie.SecurePolicy = CookieSecurePolicy.None;  
});

builder.Services.AddMemoryCache();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseSwagger();
app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1"));
app.UseHttpsRedirection();
//Enables to use static files in wwwroot like html.
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();
app.UseAuthentication();
//must be called before routes are mapped
app.UseSession();
app.MapControllers();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.Run();