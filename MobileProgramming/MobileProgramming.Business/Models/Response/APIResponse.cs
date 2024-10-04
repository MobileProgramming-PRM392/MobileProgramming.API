﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MobileProgramming.Business.Models.Response
{
    public class APIResponse
    {
        public HttpStatusCode StatusResponse { get; set; }
        public string? Message { get; set; } = string.Empty;
        public object? Data { get; set; }
    }
}
