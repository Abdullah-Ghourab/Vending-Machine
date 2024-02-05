using FlapKap.Core.Constants;
using FlapKap.Core.DTOs;
using FlapKap.Core.Interfaces;
using FlapKap.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FlapKap.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IGenericRepository<Product> _productRepository;
        private readonly UserManager<User> _userManager;
        private readonly ICurrencyService _currencyService;
        public ProductsController(IGenericRepository<Product> productRepository, UserManager<User> userManager, ICurrencyService currencyService)
        {
            _productRepository = productRepository;
            _userManager = userManager;
            _currencyService = currencyService;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<Product>> GetProducts()
        {
            var products = await _productRepository.GetAllAsync();
            return Ok(products);
        }
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<Product>> GetProductById(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product is null) return NotFound();
            return Ok(product);
        }
        [HttpPost]
        [Authorize(Roles = AppRoles.Seller)]
        public async Task<ActionResult<Product>> CreateProduct(Product product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            if (!_currencyService.CheckAmountValidation(product.Cost))
            {
                return BadRequest(new ResultDto() { Succeded = false, Message = "Cost should be one of these values 5,10,20,50 and 100 cents" });
            }
            var email = User.FindFirstValue(ClaimTypes.Email)!;
            var user = await _userManager.FindByEmailAsync(email);
            product.SellerId = user!.Id;
            _productRepository.Add(product);
            await _productRepository.Save();
            return Ok(product);
        }
        [HttpPut]
        [Authorize(Roles = AppRoles.Seller)]
        public async Task<ActionResult<Product>> UpdateProduct(Product product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            if (!_currencyService.CheckAmountValidation(product.Cost))
            {
                return BadRequest(new ResultDto() { Succeded = false, Message = "Cost should be one of these values 5,10,20,50 and 100 cents" });
            }
            var productFromDb = await _productRepository.GetByIdAsync(product.Id);
            var email = User.FindFirstValue(ClaimTypes.Email)!;
            var user = await _userManager.FindByEmailAsync(email);
            if (productFromDb.SellerId != user!.Id)
            {
                return Unauthorized(new ResultDto() { Succeded = false, Message = "product can only updated by only its seller" });
            }
            productFromDb.ProductName = product.ProductName;
            productFromDb.Cost = product.Cost;
            productFromDb.AmountAvailable = product.AmountAvailable;
            await _productRepository.Save();
            return Ok(product);
        }
        [HttpDelete("{id}")]
        [Authorize(Roles = AppRoles.Seller)]
        public async Task<ActionResult<bool>> DeleteProduct(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product is null) return BadRequest(false);
            var email = User.FindFirstValue(ClaimTypes.Email)!;
            var user = await _userManager.FindByEmailAsync(email);
            if (product.SellerId != user!.Id)
            {
                return Unauthorized(new ResultDto() { Succeded = false, Message = "product can only updated by only its seller" });
            }
            _productRepository.Delete(product);
            await _productRepository.Save();
            return Ok(true);
        }
        [HttpPost("Buy")]
        [Authorize(Roles = AppRoles.Buyer)]
        public async Task<ActionResult<BuyToReturnDto>> BuyProduct(BuyDto buyDto)
        {
            if (!ModelState.IsValid) return BadRequest();
            var product = await _productRepository.GetByIdAsync(buyDto.ProductId);
            if (product.AmountAvailable < buyDto.AmountOfProducts)
                return BadRequest(new ResultDto() { Succeded = false, Message = "Not enought products for your purchase" });
            int totalAmount = product!.Cost * buyDto.AmountOfProducts;
            var result = await _currencyService.Deduct(User, totalAmount);
            if (!result.Succeded)
            {
                return BadRequest(result);
            }
            product.AmountAvailable -= buyDto.AmountOfProducts;
            await _productRepository.Save();
            BuyToReturnDto buyToReturn = new BuyToReturnDto()
            {
                Total = totalAmount,
                Product = new Product()
                {
                    Id = product.Id,
                    ProductName = product.ProductName,
                    AmountAvailable = buyDto.AmountOfProducts,
                    SellerId = product.SellerId,
                    Cost = product.Cost
                }
            };
            return Ok(buyToReturn);
        }
    }
}
