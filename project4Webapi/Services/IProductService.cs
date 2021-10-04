using Microsoft.AspNetCore.Mvc;
using project4Webapi.Dtos;
using project4Webapi.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace project4Webapi
{
   public interface IProductService
    {
        public Task<List<GetProductDto>> Get();
        public Task<GetProductDto> GetById(int id);
        public Task<List<GetProductDto>> AddProduct(AddProductDto newProduct);
        public Task<GetProductDto> UpdateProduct(int id, UpdateProductDto updatedProduct);
        public Task<List<GetProductDto>> DeleteProduct(int id);
    }
}
