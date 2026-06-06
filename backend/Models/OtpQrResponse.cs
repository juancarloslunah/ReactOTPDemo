namespace backend.Models
{
    public class OtpQrResponse
    {
        public string Secret { get; set; } = string.Empty;

        public string QrCode { get; set; } = string.Empty;
    }
}