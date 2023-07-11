using Microsoft.EntityFrameworkCore;
using Npgsql;
using TiktokLikeASP.Context;

var builder = WebApplication.CreateBuilder(args);



// Add services to the container.
builder.Services.AddControllersWithViews();

var conStrBuilder = new NpgsqlConnectionStringBuilder(
    builder.Configuration.GetConnectionString("WebApiDatabase")
);
conStrBuilder.Password = builder.Configuration["DB_PASSWORD"];
conStrBuilder.Username = builder.Configuration["DB_USER"];
conStrBuilder.Database = builder.Configuration["DB_NAME"];

builder.Services.AddDbContext<ApplicationDbContext>(
    options => options.UseNpgsql(
        conStrBuilder.ConnectionString
    )
);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
