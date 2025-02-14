namespace Messenger.Application.Helpers
{
    internal static class PaginationCalculator
    {
        public static bool IsLastPage(
            int page,
            int pageSize,
            int totalItems)
        {
            return page * pageSize >= totalItems;
        }
    }
}
