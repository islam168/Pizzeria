using ContosoPizza.Models;

namespace ContosoPizza.Utilities.JWT
{
    public interface IJWTProvider
    {
        string GenerateToken(Customer customer);
    }
}
