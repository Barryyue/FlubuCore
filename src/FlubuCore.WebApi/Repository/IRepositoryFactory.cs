﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlubuCore.WebApi.Repository
{
    public interface IRepositoryFactory
    {
        ISerilogRepository CreateSerilogRepository();
    }
}
