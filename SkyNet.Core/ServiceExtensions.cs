﻿using Microsoft.Extensions.DependencyInjection;
using SkyNet.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkyNet.Core
{
    public static class ServiceExtensions
    {
        public static void AddCoreServices(this IServiceCollection service)
        {
            service.AddTransient<UserService>();
        }
    }
}