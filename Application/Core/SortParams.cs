namespace Application.Core
{
    public class SortParams
    {
        private const string DefaultSortOrder = "asc";
        public string Field { get; set; }
        private string _order = "asc";
        public string Order
        {
            get => _order;
            set => _order = (value.ToLower() == "desc") || (value.ToLower() == "asc") ? DefaultSortOrder : value.ToLower();
        }
    }
}