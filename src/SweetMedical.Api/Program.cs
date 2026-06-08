using SweetMedical.Api.Errors;
using SweetMedical.Api.Mappings;
using SweetMedical.Application;
using SweetMedical.Infrastructure;
using SweetMedical.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

{
    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
    builder.Services.AddInfrastructure(builder.Configuration).AddApplication();
    builder.Services.AddAutoMapper(cfg =>
    {
        cfg.AddProfile<DoctorMappingProfile>();
        cfg.AddProfile<AppointmentMappingProfile>();
    });
    builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
    builder.Services.AddProblemDetails();
    var allowedOrigins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>() ?? [];
    builder.Services.AddCors(options =>
    {
        options.AddDefaultPolicy(policy =>
            policy.WithOrigins(allowedOrigins)
                  .AllowAnyHeader()
                  .AllowAnyMethod());
    });
}

var app = builder.Build();

{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Sweet Medical");
        options.RoutePrefix = "api/docs";
    });
}

{
    await DbSeeder.SeedAsync(app.Services);
    app.UseHttpsRedirection();
    app.UseCors();
    app.UseExceptionHandler();
    app.MapControllers();
    app.Run();
}
