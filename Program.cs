using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers(); // <--- Add this line for MVC controller support

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast = Enumerable.Range(1, 4).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast")
.WithOpenApi();

app.MapControllers(); // <-- Add this line to map attribute-routed controllers

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}

// Add the Customer model
public class Customer
{
    public string CifNumber { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
}

// Add the CustomersController
[ApiController]
[Route("[controller]")]
public class CustomersController : ControllerBase
{
    private static List<Customer> customers = new List<Customer>
    {
        new Customer { CifNumber = "123456", Name = "John Doe", Email = "john.doe@example.com" },
        new Customer { CifNumber = "223344", Name = "Sana shaikh", Email = "Sana.Sana@example.com" },
        new Customer { CifNumber = "334455", Name = "Alice Johnson", Email = "alice.johnson@example.com" },
        new Customer { CifNumber = "112233", Name = "Mohammad Ahmed", Email = "Mohammad.Ahmed@example.com" },
        new Customer { CifNumber = "554667", Name = "Waseem Ali", Email = "Waseem.Ali1@example.com" }
    };

    [HttpGet("{cif}")]
    public ActionResult<Customer> GetCustomerByCif(string cif)
    {
        var customer = customers.FirstOrDefault(c => c.CifNumber == cif);
        if (customer == null)
        {
            return NotFound();
        }
        return Ok(customer);
    }
}
