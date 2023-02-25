using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SusGalerie.Data;
using tusdotnet;
using tusdotnet.Helpers;
using tusdotnet.Interfaces;
using tusdotnet.Models;
using tusdotnet.Models.Configuration;
using tusdotnet.Stores;
using SusGalerie.Services;
using SusGalerie.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<User>(options =>
{
    options.SignIn.RequireConfirmedAccount = true;
    options.SignIn.RequireConfirmedPhoneNumber = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireDigit = false;
    options.Password.RequireNonAlphanumeric = false;
}).AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddTransient<FileStorageManager>();

builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();   

app.UseAuthentication();
app.UseAuthorization();

app.MapTus("/files", async httpContext => new()
{
    // This method is called on each request so different configurations can be returned per user, domain, path etc.
    // Return null to disable tusdotnet for the current request.

    // Where to store data?
    Store = new TusDiskStore(Path.Combine(app.Environment.ContentRootPath, "Uploads"), deletePartialFilesOnConcat: true),
    Events = new()
    {
        // What to do when file is completely uploaded?
        OnFileCompleteAsync = async eventContext =>
        {
            tusdotnet.Interfaces.ITusFile file = await eventContext.GetFileAsync();
            // Dictionary<string, tusdotnet.Models.Metadata> metadata = await file.GetMetadataAsync(eventContext.CancellationToken);
            // using Stream content = await file.GetContentAsync(eventContext.CancellationToken);
            var fsm = httpContext.RequestServices.GetService<FileStorageManager>();

            await fsm.StoreTus(file, eventContext.CancellationToken);
        }
    }
});

app.MapRazorPages();

app.Run();
