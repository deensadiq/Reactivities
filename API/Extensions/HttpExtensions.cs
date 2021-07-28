using System.Text.Json;
using Microsoft.AspNetCore.Http;

namespace API.Extensions
{
    public static class HttpExtensions
    {
        public static void AddPaginationHeader(this HttpResponse response, int currentPage, int itemsPerPage, int totalItems, int totalPages)
        {
            var paginationHeader = new
            {
                currentPage,
                itemsPerPage,
                totalItems,
                totalPages
            };

            response.Headers.Add("Pagination", JsonSerializer.Serialize(paginationHeader));
            response.Headers.Add("Access-Control-Expose-Headers", "Pagination");
        }

        public static void AddSortHeader(this HttpResponse response, string field, string order)
        {
            var sortHeader = new
            {
                field,
                order
            };

            response.Headers.Add("Sort", JsonSerializer.Serialize(sortHeader));
            response.Headers.Add("Access-Control-Expose-Headers", "Sort");
        }
    }
}