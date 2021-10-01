using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using project4Webapi.Dtos;
using project4Webapi.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace project4Webapi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

       [HttpGet]
        public async Task<ActionResult<List<GetProductDto>>> Get()
        {
            return Ok(await _productService.Get());
        }

        [HttpPost("Add")]
        public async Task<ActionResult<List<GetProductDto>>> AddProduct(AddProductDto newProduct)
        {
         
            return Ok(await _productService.AddProduct(newProduct));
        }

        [HttpPut("{id}")]
      
        public async Task<ActionResult<GetProductDto>> UpdateProduct(int id, UpdateProductDto updatedProduct)
        {
            return Ok(await _productService.UpdateProduct(id, updatedProduct));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<List<GetProductDto>>> DeleteProduct(int id)
        {
            return Ok(await _productService.DeleteProduct(id));
        }
    }
}
