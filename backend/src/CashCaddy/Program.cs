

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
// Add the ExpenseContext to the services
builder.Services.AddDbContext<ExpenseDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register the repository
builder.Services.AddScoped<IExpenseRepository, ExpenseRepository>();

var app = builder.Build();

// Apply migrations at startup
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ExpenseDbContext>();
    dbContext.Database.Migrate();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

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


