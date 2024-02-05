using FlapKap.Core.DTOs;
using FlapKap.Core.Interfaces;
using FlapKap.Core.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FlapKap.Services
{
    public class CurrencyService : ICurrencyService
    {
        private readonly UserManager<User> _userManager;

        public CurrencyService(UserManager<User> userManager)
        {
            _userManager = userManager;
        }
        public bool CheckAmountValidation(int amount)
        {
            if (amount == 5 || amount == 10 || amount == 20 || amount == 50 || amount == 100)
                return true;
            return false;
        }

        public async Task<ResultDto> Deduct(ClaimsPrincipal User, int amount)
        {
            var email = User.FindFirstValue(ClaimTypes.Email)!;
            var user = await _userManager.FindByEmailAsync(email);
            ResultDto result = new ResultDto() { Succeded = true, Message = "" };
            if (user.Deposit < amount)
            {
                result.Succeded = false;
                result.Message = "user not having enough money for buying";
                return result;
            }
            user.Deposit -= amount;
            await _userManager.UpdateAsync(user);
            return result;
        }


        public async Task<ResultDto> Deposit(ClaimsPrincipal User, int amount)
        {
            var email = User.FindFirstValue(ClaimTypes.Email)!;
            var user = await _userManager.FindByEmailAsync(email);
            bool AmountChecked = CheckAmountValidation(amount);
            ResultDto result = new ResultDto() { Succeded = true, Message = "" };
            if (!AmountChecked)
            {
                result.Succeded = false;
                result.Message = "Allowed Deposit Amounts Are  5 ,10 ,20 ,50,100 ";
                return result;
            }
            user.Deposit += amount;
            await _userManager.UpdateAsync(user);
            return result;

        }

        public async Task<ResultDto> Reset(ClaimsPrincipal User)
        {
            var email = User.FindFirstValue(ClaimTypes.Email)!;
            var user = await _userManager.FindByEmailAsync(email);
            ResultDto result = new ResultDto() { Succeded = true, Message = "" };
            user.Deposit =0;
            await _userManager.UpdateAsync(user);
            return result;
        }
    }
}
