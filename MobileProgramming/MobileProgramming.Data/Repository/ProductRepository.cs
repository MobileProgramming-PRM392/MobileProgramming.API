﻿using Microsoft.EntityFrameworkCore;
using MobileProgramming.Business.Models.DTO.Product;
using MobileProgramming.Data.Entities;
using MobileProgramming.Data.Generic;
using MobileProgramming.Data.Interfaces;
using MobileProgramming.Data.Models.Enum;
using MobileProgramming.Data.Models.Product;
using MobileProgramming.Data.Persistence;

namespace MobileProgramming.Data.Repository
{
    public class ProductRepository : RepositoryBase<Product>, IProductRepository
    {
        private readonly SaleProductDbContext _context;

        public ProductRepository(SaleProductDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<Product>> GetbyCategoryId(int id)
        {
            return await _context.Products.Where(p => p.CategoryId == id).ToListAsync();
        }

        public async Task<List<Product>> GetFilteredProductsAsync(ProductFilterDto filter, ProductSortDto sort)
        {
            var query = _context.Products.AsQueryable();

            if (!string.IsNullOrEmpty(filter.Brand))
            {
                query = query.Where(p => p.ProductBrand == filter.Brand);
            }
            if (filter.MinPrice.HasValue)
            {
                query = query.Where(p => p.Price >= filter.MinPrice.Value);
            }
            if (filter.MaxPrice.HasValue)
            {
                query = query.Where(p => p.Price <= filter.MaxPrice.Value);
            }
            if (filter.CategoryId.HasValue)
            {
                query = query.Where(p => p.CategoryId == filter.CategoryId.Value);
            }


            switch (sort.SortOption)
            {
                case ProductSortOption.PriceAsc:
                    query = query.OrderBy(p => p.Price);
                    break;
                case ProductSortOption.PriceDesc:
                    query = query.OrderByDescending(p => p.Price);
                    break;
                case ProductSortOption.Popularity:

                    query = GetProductsByPopularity(query);
                    break;
                
            }
            return query.ToList();
        }

        public async Task<Product?> GetProductDetail(int productId)
        {
            return await _context.Products.Include(p => p.ProductImages).Where(p => p.ProductId == productId).FirstOrDefaultAsync();
        }

        public async Task<List<Product>> GetProductsToDisplay()
        {
            return await _context.Products.Include(p => p.ProductImages).ToListAsync();
        }

        private IQueryable<Product> GetProductsByPopularity(IQueryable<Product> productsQuery)
        {
            return productsQuery
                .GroupJoin(
                    _context.CartItems
                        .Join(_context.Orders,
                            ci => ci.CartId,
                            o => o.CartId,
                            (ci, o) => new { ci, o }), // Joining CartItems and Orders
                    p => p.ProductId,
                    co => co.ci.ProductId,
                    (product, cartOrders) => new
                    {
                        product,
                        cartItemsCount = cartOrders.Count(co => co.o.OrderStatus == "Successfully") // Counting only if order is "Paid"
                    })
                .OrderByDescending(g => g.cartItemsCount)
                .Select(g => g.product);
        }


        public async Task<decimal> GetProductPriceAsync(int productId)
        {

            return await _context.Products
                .Where(p => p.ProductId == productId)
                .Select(p => p.Price).FirstOrDefaultAsync();

        }



    }
}
