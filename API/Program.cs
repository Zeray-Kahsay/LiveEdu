using API.Extensions.ProgramExtensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddIdentityServices();
builder.Services.AddAuthorization();






var app = builder.Build();






// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(opt =>
   {
       opt.AllowAnyHeader().AllowAnyMethod().AllowCredentials().WithOrigins([
           "http://localhost:3000",
            "https://localhost:3000"
       ]);
   });
app.UseGlobalExceptionHandler(); // Custom global exception handler
app.UseCors();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

await app.MigrateDatabase(); // Extension method to migrate database and seed data

app.Run();
