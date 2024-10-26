using EmailNotificationService.API;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.Configure<MailOptions>(
    builder.Configuration.GetSection(MailOptions.SECTION_NAME));
builder.Services.AddScoped<MailSender>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseExceptionHandler(exceptionHandler =>
{
    exceptionHandler.Run(async httpContext =>
    {
        httpContext.RequestServices.GetRequiredService(typeof(ExceptionHandler));
    });
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapPost("send", async (MailData mailData, MailSender mailSender) =>
    await mailSender.Send(mailData) == true ? Results.Ok() : Results.Problem());

app.UseHttpsRedirection();

app.Run();
