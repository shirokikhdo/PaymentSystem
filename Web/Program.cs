using Web.Extensions;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder
    .AddBearerAuthorization()
    .AddOptions()
    .AddSwagger()
    .AddData()
    .AddApplicationServices();

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();
app.MapControllers();
app.Run();