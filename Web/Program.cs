using Microsoft.AspNetCore.HttpLogging;
using Web.Extensions;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddHttpLogging(options =>
{
    options.LoggingFields = HttpLoggingFields.RequestBody | HttpLoggingFields.RequestHeaders |
                            HttpLoggingFields.Duration | HttpLoggingFields.RequestPath |
                            HttpLoggingFields.ResponseBody | HttpLoggingFields.ResponseHeaders;
});
builder
    .AddBearerAuthorization()
    .AddOptions()
    .AddSwagger()
    .AddData()
    .AddApplicationServices()
    .AddBackgroundService();

var app = builder.Build();
app.UseHttpLogging();
app.UseAuthentication();
app.UseAuthorization();
app.UseSwagger();
app.UseSwaggerUI();
app.MapControllers();
app.Run();