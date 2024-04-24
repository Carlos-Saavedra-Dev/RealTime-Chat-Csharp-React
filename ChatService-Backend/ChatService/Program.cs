using ChatService.DataService;
using ChatService.DbConnection;
using ChatService.Hubs;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSignalR();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(opt =>
{
    opt.AddPolicy("reactService", builder =>
    {
        builder.WithOrigins("http://localhost:3000")
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials();
    });
});


builder.Services.AddSingleton<SharedDb>();
builder.Services.AddSingleton<IMongoClient, MongoClient>(sp =>
{
    var settings = MongoClientSettings.FromConnectionString("mongodb://localhost:27017");
    return new MongoClient(settings);
});

builder.Services.AddSingleton<MongoDBService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseCors("reactService");
app.MapControllers();

app.MapHub<ChatHub>("/Chat");


app.Run();
