﻿using BlazorAppWebEcomm.Server.Models;
using BlazorAppWebEcomm.Server.Services.ProductServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlazorAppWebEcomm.Server.Controllers
{
    //[Route("api/[controller]/[action]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            this._productService = productService;
        }

        [HttpGet("api/Product/admin"),Authorize(Roles ="Admin")]
        public async Task<ActionResult<ServiceResponse<List<Models.Product>>>> GetAdminProducts()
        {
            ServiceResponse<List<Models.Product>>? products = await _productService.GetAdminProducts();
            return Ok(products);
        }

        [HttpGet]
        [Route("api/Product/GetProducts")]
        public async Task<ActionResult<ServiceResponse<List<Models.Product>>>> GetProducts()
        {
            ServiceResponse<List<Models.Product>>? products = await _productService.GetProductsAsync();
            return Ok(products);
        }

        [HttpGet]
        [Route("api/Product/GetProduct/{productId}")]
        public async Task<ActionResult<ServiceResponse<Models.Product>>> GetProduct(int productId)
        {
            ServiceResponse<Models.Product>? product = await _productService.GetProductAsync(productId);
            return Ok(product);
        }

        [HttpPost, Authorize(Roles = "Admin")]
        [Route("api/Product/createproduct")]
        public async Task<ActionResult<ServiceResponse<Models.Product>>> CreateProduct(Models.Product product)
        {
            ServiceResponse<Models.Product> response = await _productService.CreateProductAsync(product);
            return Ok(response);
        }

        [HttpPut,Authorize(Roles="Admin")]
        [Route("api/Product/updateproduct")]
        public async Task<ActionResult<ServiceResponse<Models.Product>>> UpdateProduct(Models.Product product)
        {
            ServiceResponse<Models.Product> response = await _productService.UpdateProductAsync(product);
            return Ok(product);
        }

        [HttpDelete, Authorize(Roles = "Admin")]
        [Route("api/Product/deleteproduct/{productId}")]
        public async Task<ActionResult<ServiceResponse<bool>>> DeleteProduct(int productId)
        {
            ServiceResponse<bool> product = await _productService.DeleteProductAsync(productId);
            return Ok(product);
        }

        [HttpGet]
        [Route("api/Product/GetProductsByCategory/{categoryUrl}")]
        public async Task<ActionResult<ServiceResponse<List<Models.Product>>>> GetProductsByCategory(string categoryUrl)
        {
            ServiceResponse<List<Models.Product>>? products = await _productService.GetProductByCategoryAsync(categoryUrl);
            return Ok(products);
        }

        [HttpGet]
        [Route("api/Product/SearchProducts/{searchText}/{page}")]
        public async Task<ActionResult<ServiceResponse<ProductSearchResult<Models.Product>>>> SearchProducts(string searchText, int page = 1)
        {
            ServiceResponse<ProductSearchResult<Models.Product>>? products = await _productService.SearchProducts(searchText, page);
            return Ok(products);
        }

        [HttpGet]
        [Route("api/Product/GetProductSearchSuggestion/{searchText}")]
        public async Task<ActionResult<ServiceResponse<List<string>>>> GetProductSearchSuggestion(string searchText)
        {
            ServiceResponse<List<string>>? strSearchSuggestion = await _productService.GetProductSearchSuggestion(searchText);
            return Ok(strSearchSuggestion);
        }


        [HttpGet]
        [Route("api/Product/GetFeaturedProductsAsync")]
        public async Task<ActionResult<ServiceResponse<List<Models.Product>>>> GetFeaturedProductsAsync()
        {
            ServiceResponse<List<Models.Product>>? products = await _productService.GetFeaturedProductsAsync();
            return Ok(products);
        }
    }
}
