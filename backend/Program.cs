using QRCoder;
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

// Descomentar si deseas OpenAPI
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

// Para evitar problemas locales
// app.UseHttpsRedirection();

app.MapGet("/", () =>
{
    return Results.Ok(new
    {
        Aplicacion = "Demo OTP Jarvis",
        Version = "1.0",
        Estado = "Activo"
    });
});

app.MapGet("/api/otp/generate", () =>
{
    // Secret fijo para el piloto
    var secret = "JBSWY3DPEHPK3PXP";

    var otpUri =
        $"otpauth://totp/DemoOTP?secret={secret}&issuer=Jarvis";

    using var qrGenerator = new QRCodeGenerator();

    var qrData = qrGenerator.CreateQrCode(
        otpUri,
        QRCodeGenerator.ECCLevel.Q
    );

    var pngQrCode = new PngByteQRCode(qrData);

    byte[] qrBytes = pngQrCode.GetGraphic(20);

    var base64Qr =
        $"data:image/png;base64,{Convert.ToBase64String(qrBytes)}";

    return Results.Ok(
        new OtpQrResponse
        {
            Secret = secret,
            QrCode = base64Qr
        });
});

app.MapPost("/api/otp/validate", (OtpRequest request) =>
{
    var secret = request.Secret;

    if (string.IsNullOrWhiteSpace(secret))
    {
        return Results.Problem(
            title: "Error",
            detail: "Debe enviar el Secret."
        );
    }

    try
    {
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
    }
    catch (Exception ex)
    {
        return Results.Problem(
            title: "Error de validación",
            detail: ex.Message
        );
    }
});

app.MapGet("/version", () =>
{
    return Results.Ok(new
    {
        Version = "QR-OTP-20260605"
    });
});

app.Run();