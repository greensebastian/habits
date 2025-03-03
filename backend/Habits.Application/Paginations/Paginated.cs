namespace Habits.Application.Paginations;

public record Paginated<T>(T[] Data, PaginationResponse Pagination);