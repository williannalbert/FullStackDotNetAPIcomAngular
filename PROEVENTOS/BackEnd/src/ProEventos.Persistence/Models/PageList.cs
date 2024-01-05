using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProEventos.Persistence.Models
{
    public class PageList<T> : List<T>
    {
        public int CurrentPage { get; set; }
        public int TotalPage { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public PageList()
        {
            
        }

        //herança de T ao iniciar e no constutor, e significa que é uma lista gennérica
        public PageList(List<T> items, int count, int pageNumber, int pageSize)
        {
            TotalCount = count;
            PageSize = pageSize;
            CurrentPage = pageNumber;
            TotalPage = (int)Math.Ceiling(count / (double)pageSize);
            //usado devido a herança de List
            AddRange(items);
        }

        public static async Task<PageList<T>> CreateAsync(
            IQueryable<T> source, int pageNumber, int pageSize
        )
        {
            var count = await source.CountAsync();
            var items = await source.Skip((pageNumber - 1)*pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PageList<T>(items, count, pageNumber, pageSize);
        }

    }
}
