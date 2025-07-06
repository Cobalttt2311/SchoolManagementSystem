using SchoolManagementSystem.Configurations;
using SchoolManagementSystem.Modules.Students.Repositories;
using SchoolManagementSystem.Modules.Students.Services;
using SchoolManagementSystem.Modules.Teachers.Repositories;
using SchoolManagementSystem.Modules.Teachers.Services;
using SchoolManagementSystem.Modules.Classes.Repositories;
using SchoolManagementSystem.Modules.Classes.Services;
using SchoolManagementSystem.Modules.Enrollments.Services;
using SchoolManagementSystem.Modules.Enrollments.Repositories;
using SchoolManagementSystem.Middlewares;
using SchoolManagementSystem.Data;
using Microsoft.AspNetCore.Identity; 
using SchoolManagementSystem.Modules.Users.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer; // <-- Tambahkan ini
using Microsoft.IdentityModel.Tokens; // <-- Tambahkan ini
using System.Text; // <-- Tambahkan ini

// Muat variabel dari file .env di awal aplikasi
DotNetEnv.Env.Load();

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration; // Ambil konfigurasi untuk JWT

// --- Konfigurasi Services ---

// Daftarkan DbContext
builder.Services.AddDatabase(configuration);

// Daftarkan service untuk ASP.NET Core Identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

// =================================================================
// == TAMBAHKAN BLOK INI UNTUK KONFIGURASI JWT AUTHENTICATION ==
// =================================================================
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidAudience = configuration["Jwt:Audience"],
        ValidIssuer = configuration["Jwt:Issuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!))
    };
});
// =================================================================

// =================================================================
// == TAMBAHKAN BLOK INI UNTUK MENGATASI REDIRECT ==
// =================================================================
builder.Services.ConfigureApplicationCookie(options =>
{
    options.Events.OnRedirectToLogin = context =>
    {
        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
        return Task.CompletedTask;
    };
    options.Events.OnRedirectToAccessDenied = context =>
    {
        context.Response.StatusCode = StatusCodes.Status403Forbidden;
        return Task.CompletedTask;
    };
});
// =================================================================

// Mendaftarkan services Controller
builder.Services.AddControllers();
builder.Services.AddHttpContextAccessor();

// Tambahkan services untuk API Explorer dan Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Daftarkan semua Repository dan Service Anda
builder.Services.AddScoped<IStudentRepository, StudentRepository>();
builder.Services.AddScoped<IStudentService, StudentService>();
builder.Services.AddScoped<ITeacherRepository, TeacherRepository>();
builder.Services.AddScoped<ITeacherService, TeacherService>();
builder.Services.AddScoped<IClassRepository, ClassRepository>();
builder.Services.AddScoped<IClassService, ClassService>();
builder.Services.AddScoped<IEnrollmentRepository, EnrollmentRepository>(); 
builder.Services.AddScoped<IEnrollmentService, EnrollmentService>();


// --- Bangun Aplikasi ---
var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    await DbSeeder.SeedRolesAndAdminAsync(scope.ServiceProvider);
}

// --- Konfigurasi HTTP Request Pipeline ---

app.UseMiddleware<LoggingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionMiddleware>();

// Pastikan urutan ini benar: Authentication -> Authorization
app.UseAuthentication();
app.UseAuthorization();

// app.UseHttpsRedirection();

app.MapControllers();

app.Run();