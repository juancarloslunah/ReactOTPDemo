namespace backend.Models;

public class OtpRequest
{
    public string Secret { get; set; } = "";

    public string Otp { get; set; } = "";
}