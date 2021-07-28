namespace Application.Core
{
    public class PagingParams
    {
        #region Pagination
        private const int MaxPageSize = 50;
        public int PageNumber { get; set; } = 1;
        private int _pageSize = 10;
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
        }
        #endregion

        #region Sorting
        private const string DefaultSortOrder = "asc";
        public string Field { get; set; }
        private string _order = "asc";
        public string Order
        {
            get => _order;
            set => _order = (value.ToLower() != "desc") || (value.ToLower() != "asc") ? DefaultSortOrder : value.ToLower();
        }
        #endregion
    }
}