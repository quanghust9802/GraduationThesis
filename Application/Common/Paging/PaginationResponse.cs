namespace Application.Common.Paging
{
    public class PaginationResponse<TEntity>
    {
        public PaginationResponse(IEnumerable<TEntity> data, int totalCount)
        {
            Data = data;
            TotalCount = totalCount;
        }

        public IEnumerable<TEntity> Data { get; }
        public int TotalCount { get; }
    }
}
