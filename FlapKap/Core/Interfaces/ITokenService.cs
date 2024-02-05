using FlapKap.Core.Models;

namespace FlapKap.Core.Interfaces
{
    public interface ITokenService
    {
        string CreateToken(User user, string role);
    }
}
