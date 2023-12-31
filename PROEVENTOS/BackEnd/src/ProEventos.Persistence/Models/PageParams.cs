﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProEventos.Persistence.Models
{
    public class PageParams
    {
        public const int MaxPageSize = 50;
        public int PageNumber { get; set; } = 1;

        public int pageSize = 10;
        public int PageSize {
            get { return pageSize; }
            //para não retornar valor maior que 50
            set { pageSize = (value > MaxPageSize) ? MaxPageSize : value; } 
        }
        public string Term { get; set; } = "";
    }
}
