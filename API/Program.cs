using API.Extensions.ProgramExtensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddIdentityServices();






var app = builder.Build();


app.UseGlobalExceptionHandler(); // Custom global exception handler




// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

await app.MigrateDatabase(); // Extension method to migrate database and seed data

app.Run();
