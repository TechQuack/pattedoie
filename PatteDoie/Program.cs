using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using PatteDoie;
using PatteDoie.Configuration;
using PatteDoie.Hubs;
using PatteDoie.Services;
using PatteDoie.Services.Platform;
using PatteDoie.Services.Scattergories;
using PatteDoie.Services.SpeedTyping;
using PatteDoie.Views;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
builder.Services.AddDbContextFactory<PatteDoieContext>(options =>
    options.UseSqlServer(string.Format(builder.Configuration.GetConnectionString("PatteDoieContext") ?? "", Environment.GetEnvironmentVariable("MSSQL_SA_PASSWORD"))));
builder.Services.AddHttpClient();
builder.Services.AddScoped<ISpeedTypingService, SpeedTypingService>();
builder.Services.AddScoped<IPlatformService, PlatformService>();
builder.Services.AddScoped<IScattegoriesService, ScattegoriesService>();
builder.Services.AddScoped<IClipboardService, ClipboardService>();
builder.Services.AddAutoMapper(typeof(MappingProfile).Assembly);
builder.Services.AddSignalR(o =>
{
    o.EnableDetailedErrors = true;
});

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

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAntiforgery();

app.UseAuthorization();

app.MapHub<SpeedTypingHub>("/hub/speedtyping");
app.MapHub<ScattergoriesHub>("/hub/scattergories");
app.MapHub<PlatformHub>("/hub/platform");

app.Run();
