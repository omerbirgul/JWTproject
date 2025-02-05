using AuthServer.Core.Dtos.ProductDtos;
using AuthServer.Core.Entities;
using AuthServer.Core.Services.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AuthServer.API.Controllers;

    [Authorize]
    public class ProductController : CustomBaseController
    {
        private readonly IGenericService<Product, GetProductDto> _productService ;

        public ProductController(IGenericService<Product, GetProductDto> productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<IActionResult> GetProducts()
        {
            var result = await _productService.GetAllAsync();
            return CreateCustomResult(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create(GetProductDto productDto)
        {
            var result = await _productService.AddAsync(productDto);
            return CreateCustomResult(result);
        }

        [HttpPut]
        public async Task<IActionResult> Update(GetProductDto productDto)
        {
            var result = await _productService.Update(productDto, productDto.Id);
            return CreateCustomResult(result);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _productService.Remove(id);
            return CreateCustomResult(result);
        }
    }

