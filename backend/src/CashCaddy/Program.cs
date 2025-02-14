

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new DateOnlyJsonConverter());
    });
// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
// Add the ExpenseContext to the services
builder.Services.AddDbContext<ExpenseDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register the repository
builder.Services.AddScoped<IExpenseRepository, ExpenseRepository>();

// Add CORS services
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.WithOrigins("http://localhost:5173")
               .AllowAnyHeader()
               .AllowAnyMethod();
    });
});

var app = builder.Build();

// Use CORS
app.UseCors();

// Apply migrations at startup
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ExpenseDbContext>();
    dbContext.Database.Migrate();
    SeedDatabase(dbContext);
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.MapControllers();

// Map minimal APIs for CRUD operations on expenses
app.MapGet("/expenses", async (IExpenseRepository repository) =>
{
    return Results.Ok(await repository.GetAllExpensesAsync());
});

app.MapGet("/expenses/{id}", async (Guid id, IExpenseRepository repository) =>
{
    var expense = await repository.GetExpenseByIdAsync(id);
    return expense is not null ? Results.Ok(expense) : Results.NotFound();
});

app.MapPost("/expenses", async (Expense expense, IExpenseRepository repository) =>
{
    await repository.AddExpenseAsync(expense);
    return Results.Created($"/expenses/{expense.Id}", expense);
});

app.MapPut("/expenses/{id}", async (Guid id, Expense updatedExpense, IExpenseRepository repository) =>
{
    var expense = await repository.GetExpenseByIdAsync(id);
    if (expense is null)
    {
        return Results.NotFound();
    }

    expense.Date = updatedExpense.Date;
    expense.Amount = updatedExpense.Amount;
    expense.Description = updatedExpense.Description;
    expense.Category = updatedExpense.Category;

    await repository.UpdateExpenseAsync(expense);
    return Results.NoContent();
});

app.MapDelete("/expenses/{id}", async (Guid id, IExpenseRepository repository) =>
{
    var expense = await repository.GetExpenseByIdAsync(id);
    if (expense is null)
    {
        return Results.NotFound();
    }

    await repository.DeleteExpenseAsync(id);
    return Results.NoContent();
});

app.Run();

void SeedDatabase(ExpenseDbContext context)
{
    if (!context.Expenses.Any())
    {
        context.Expenses.AddRange(
            new CashCaddy.API.Models.Expense
            {
                Date = DateTime.UtcNow.AddDays(-1),
                Amount = 50.00m,
                Description = "Groceries",
                Category = "Food"
            },
            new CashCaddy.API.Models.Expense
            {
                Date = DateTime.UtcNow.AddDays(-2),
                Amount = 20.00m,
                Description = "Taxi",
                Category = "Transport"
            },
            new CashCaddy.API.Models.Expense
            {
                Date = DateTime.UtcNow.AddDays(-3),
                Amount = 100.00m,
                Description = "Electricity Bill",
                Category = "Utilities"
            }
        );
        context.SaveChanges();
    }
}


