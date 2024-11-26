using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using PatteDoie;
using PatteDoie.Configuration;
using PatteDoie.Hubs;
using PatteDoie.Services.Platform;
using PatteDoie.Services.SpeedTyping;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<PatteDoieContext>(options =>
    options.UseSqlServer(string.Format(builder.Configuration.GetConnectionString("PatteDoieContext") ?? "", Environment.GetEnvironmentVariable("MSSQL_SA_PASSWORD"))));
builder.Services.AddHttpClient();
builder.Services.AddScoped<ISpeedTypingService, SpeedTypingService>();
builder.Services.AddScoped<IPlatformService, PlatformService>();
builder.Services.AddAutoMapper(typeof(MappingProfile).Assembly);
builder.Services.AddServerSideBlazor();

builder.Services.AddSignalR();

builder.Services.AddResponseCompression(opts =>
{
    opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
        ["application/octet-stream"]);
});

var app = builder.Build();

app.UseResponseCompression();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.MapBlazorHub();
app.MapFallbackToController("Blazor", "Home");

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapHub<SpeedTypingHub>("/hub/speedtyping");

app.Run();
