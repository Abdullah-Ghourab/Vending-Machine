using FlapKap.Core.DTOs;
using System.Security.Claims;

namespace FlapKap.Core.Interfaces
{
    public interface ICurrencyService
    {
        Task<ResultDto> Deposit(ClaimsPrincipal User, int amount);
        Task<ResultDto> Deduct(ClaimsPrincipal User, int amount);
        Task<ResultDto> Reset(ClaimsPrincipal User);

        bool CheckAmountValidation(int amount);
    }
}
