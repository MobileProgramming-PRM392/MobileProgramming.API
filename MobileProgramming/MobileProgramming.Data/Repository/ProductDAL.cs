﻿using Microsoft.Extensions.Logging;
using MobileProgramming.Data.Entities;
using MobileProgramming.Data.Generic;
using MobileProgramming.Data.Interfaces;
using MobileProgramming.Data.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileProgramming.Data.Repository
{
    public class ProductDAL : RepositoryBase<Product>, IProductRepository
    {
        private readonly ApplicationDbContext _context;

        public ProductDAL(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
