using AutoMapper;
using Blogs.CMS.Configurations;
using Blogs.DataAccess.EntityFramework;
using Google.Apis.Auth.AspNetCore3;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.HttpOverrides;
using System.Text.Json.Serialization;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

AppSettings appSettings = builder.Configuration.ConfigureAndGet<AppSettings>(builder.Services);

builder.Services.AddPooledDbContextFactory<BlogsDbContext>(option =>
{
    string? connection = builder.Configuration.GetConnectionString("DefaultConnection");
    option.UseNpgsql(connection, x => x.EnableRetryOnFailure());
});

builder.Services.AddTransient<BlogsDbContextFactory>();
builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders =
        ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
});

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
        options.JsonSerializerOptions.WriteIndented = true;
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });

//builder.Services.AddCors(options =>
//{
//    options.AddPolicy("APIPublicCors",
//        _ => _.AllowAnyOrigin()
//            .AllowAnyMethod()
//            .AllowAnyHeader()
//    );
//});

// Auto mapping
builder.Services.AddSingleton(provider => new MapperConfiguration(cfg =>
{
    cfg.AddProfile(new ModelMapping());
}).CreateMapper());

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddHttpClient();

builder.Services.AddAuthentication(o =>
{
    // This forces challenge results to be handled by Google OpenID Handler, so there's no
    // need to add an AccountController that emits challenges for Login.
    o.DefaultChallengeScheme = GoogleOpenIdConnectDefaults.AuthenticationScheme;
    // This forces forbid results to be handled by Google OpenID Handler, which checks if
    // extra scopes are required and does automatic incremental auth.
    o.DefaultForbidScheme = GoogleOpenIdConnectDefaults.AuthenticationScheme;
    // Default scheme that will handle everything else.
    // Once a user is authenticated, the OAuth2 token info is stored in cookies.
    o.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
})
    .AddCookie()
    .AddGoogleOpenIdConnect(options =>
    {
        options.ClientId = "772319508125-jmub31m1ef64js629mhom147radg4633.apps.googleusercontent.com";
        options.ClientSecret = "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";
    });

// DI settings
builder.Services.RegisterServices(appSettings);


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
//app.UseDeveloperExceptionPage();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

//app.UseCors("APIPublicCors");

app.UseAuthorization();
app.UseAuthentication();

//app.MapControllers();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Blog}/{action=Index}/{id?}");

app.Run();