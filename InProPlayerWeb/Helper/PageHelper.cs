﻿namespace InProPlayerWeb.Helper
{
    public class PageHelper<T> : List<T>
    {
        public int page = 1;
        public int pageSize = 10;
        public List<T> pageList = new List<T>();

        public PageHelper()
        {
        }
        
        public int TotalPage()
        {
            return (int)Math.Ceiling((double)pageList.Count / pageSize);
        }

        public List<T> PageList()
        {
            return pageList
                   .Skip((page - 1) * pageSize)
                   .Take(pageSize)
                   .ToList();
        }

        public static IEnumerable<int> GetDisplayedPages(int currentPage, int totalPages, int displayCount = 10)
        {
            int halfDisplayCount = displayCount / 2;
            int startPage = currentPage - halfDisplayCount;
            int endPage = currentPage + halfDisplayCount;

            if (startPage <= 0)
            {
                endPage += Math.Abs(startPage) + 1;
                startPage = 1;
            }

            if (endPage > totalPages)
            {
                startPage -= endPage - totalPages;
                endPage = totalPages;
            }

            if (startPage <= 0)
            {
                startPage = 1;
            }

            return Enumerable.Range(startPage, endPage - startPage + 1);
        }
    }
}
