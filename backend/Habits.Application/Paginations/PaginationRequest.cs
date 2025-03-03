namespace Habits.Application.Paginations;

public record PaginationRequest(int Offset = 0, int Limit = 10);