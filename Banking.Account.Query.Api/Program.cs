using Banking.Account.Query.Application.Features.BankAccounts.Queries.FindAllAcounts;
using Banking.Account.Query.Infraestructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddInfraestructureServices(builder.Configuration);
builder.Services.AddMediatR(options => options.RegisterServicesFromAssembly(typeof(FindAllAccountsQuery).Assembly));

builder.Services.Configure<RouteOptions>(options =>
{
    options.LowercaseUrls = true;
});



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
