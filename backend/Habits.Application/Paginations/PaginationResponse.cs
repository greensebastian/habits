namespace Habits.Application.Paginations;

public record PaginationResponse(int Offset, int Limit, int Total);