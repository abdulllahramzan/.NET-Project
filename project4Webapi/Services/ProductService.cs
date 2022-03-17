using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using project4Webapi.Data;
using project4Webapi.Dtos;
using project4Webapi.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace project4Webapi.Services
{
    public class ProductService : IProductService
    {
        

        private readonly IMapper _mapper;
        private readonly DataContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ProductService(IMapper mapper, DataContext context, IHttpContextAccessor httpContextAccessor)
        {
            _mapper = mapper;
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        private int GetUserId() => int.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
        private string GetUserRole() => _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Role);
        public async Task<List<GetProductDto>> AddProduct(AddProductDto newProduct)
        {
            Product product = _mapper.Map<Product>(newProduct);
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            var response = await _context.Products
                 .Where(c => c.User.Id == GetUserId())
                .Select(c => _mapper.Map<GetProductDto>(c)).ToListAsync();
            return response;
        }

        public async Task<List<GetProductDto>> DeleteProduct(int id)
        {

            Product product = await _context.Products.FirstOrDefaultAsync(c => c.ProdId == id && c.User.Id == GetUserId());
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            var response = _context.Products
                 .Where(c => c.User.Id == GetUserId())
                .Select(c => _mapper.Map<GetProductDto>(c)).ToList();
            return response;
        }

        public async Task<List<GetProductDto>> Get()
        {
            var dbProducts = 
                GetUserRole().Equals("Admin") ?
                await _context.Products.ToListAsync() :
                await _context.Products.Where(c => c.User.Id == GetUserId()).ToListAsync();
            var response = dbProducts.Select(c => _mapper.Map<GetProductDto>(c)).ToList();
            return response;
        }
        public async Task<GetProductDto> GetById(int id)
        {
            var dbProducts = await _context.Products.FirstOrDefaultAsync(c => c.ProdId == id && c.User.Id == GetUserId());
            var response = _mapper.Map<GetProductDto>(dbProducts);
            return response;
        }
        public async Task<GetProductDto> UpdateProduct(int id, UpdateProductDto updatedProduct)
        {
            updatedProduct.ProdId = id;
            Product product = await _context.Products
                .Include(c => c.User)
                .FirstOrDefaultAsync(c => c.ProdId == updatedProduct.ProdId);
              
            if (product.User.Id == GetUserId())
            {
                product.ProdName = updatedProduct.ProdName;
                product.ProdPrice = updatedProduct.ProdPrice;
                await _context.SaveChangesAsync();
                var response = _mapper.Map<GetProductDto>(product);
                return response;
            }
            else
            {
                return null;
            }


        }
       
    }
}
