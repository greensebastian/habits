namespace Habits.Application.Paginations;

public record PaginationRequest(int Offset = 0, int Limit = 10)
{
    private PaginationResponse PaginationResponse(int totalCount) => new(Offset, Limit, totalCount);
    public Paginated<TEntry> Response<TEntry>(TEntry[] data, int totalCount) => new(data, PaginationResponse(totalCount));
};