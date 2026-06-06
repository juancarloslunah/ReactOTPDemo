using OtpNet;
using backend.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("ReactPolicy",
        policy =>
        {
            policy.AllowAnyOrigin()
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

builder.Services.AddOpenApi();

var app = builder.Build();

app.UseCors("ReactPolicy");

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapGet("/", () =>
{
    return Results.Ok(new
    {
        Aplicacion = "Demo OTP Jarvis",
        Version = "1.0"
    });
});

app.MapPost("/api/otp/validate", (OtpRequest request) =>
{
    var secret = "JBSWY3DPEHPK3PXP";

    var secretBytes = Base32Encoding.ToBytes(secret);

    var totp = new Totp(secretBytes);

    bool isValid = totp.VerifyTotp(
        request.Otp,
        out long timeStepMatched,
        VerificationWindow.RfcSpecifiedNetworkDelay
    );

    return Results.Ok(
        new OtpResponse
        {
            Success = isValid,
            Message = isValid
                ? "OTP válido"
                : "OTP inválido"
        });
});

app.Run();