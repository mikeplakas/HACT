using HACT.Data;
using HACT.Hubs;
using HACT.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// ?? Database (MSSQL με EF Core + Identity)
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// ?? Identity (χρήστες + ρόλοι)
builder.Services.AddDefaultIdentity<IdentityUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = false; // πιο απλό login
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
})
    .AddRoles<IdentityRole>() // ? Για Admin role
    .AddEntityFrameworkStores<AppDbContext>();

// ?? Authorization Policies
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy =>
        policy.RequireRole("Admin"));
});

// ?? Razor Pages
builder.Services.AddRazorPages(options =>
{
    // Όλα τα pages κάτω από /Admin χρειάζονται Admin role
    options.Conventions.AuthorizeFolder("/Admin", "AdminOnly")
           .AllowAnonymousToPage("/Admin/Login")
           .AllowAnonymousToPage("/Admin/Logout");
});
builder.Services.AddSingleton(sp =>
{
    var config = builder.Configuration.GetSection("EmailSettings");
    return new HACT.Services.EmailSender(
        config["SmtpHost"],
        int.Parse(config["SmtpPort"]),
        config["SmtpUser"],
        config["SmtpPass"],
        config["FromEmail"]
    );
});

// ?? Δικά σου services (Contact, News κλπ)
builder.Services.AddScoped<IContactMessageRepository, SqlContactMessageRepository>();
builder.Services.AddScoped<IContactService, ContactService>();
builder.Services.AddScoped<INewRepository, SqlNewRepository>();
builder.Services.AddSignalR(); // ? SignalR

// ?? Cookie Events για custom redirects
builder.Services.ConfigureApplicationCookie(options =>
{
    options.Events.OnRedirectToLogin = context =>
    {
        // ? Μην πειράξεις το Admin/Login, άφησέ το να φορτώσει κανονικά
        if (context.Request.Path.StartsWithSegments("/Admin/Login"))
        {
            context.Response.Redirect("/Admin/Login");
        }
        else
        {
            context.Response.Redirect("/Index");
        }

        return Task.CompletedTask;
    };

    options.Events.OnRedirectToAccessDenied = context =>
    {
        context.Response.Redirect("/Index");
        return Task.CompletedTask;
    };
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

//app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication(); // ? Identity authentication
app.UseAuthorization();

app.MapRazorPages();
app.MapHub<NotificationHub>("/notificationHub");

app.Run();
