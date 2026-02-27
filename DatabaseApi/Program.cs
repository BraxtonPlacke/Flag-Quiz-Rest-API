var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddControllers();

// Add CORS so React can talk to this API
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        policy =>
        {
            policy.WithOrigins("http://localhost:5173")
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

var app = builder.Build();

// Middleware
app.UseHttpsRedirection();

app.UseCors("AllowFrontend");

app.UseAuthorization();

app.MapControllers();

app.Run();
