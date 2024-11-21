namespace ContosoPizza.Utilities.JWT
{
    public class JwtOption
    {
        public string SecretKey { get; set; } = string.Empty;
        public int ExpiresHours { get; set; }
    }
}