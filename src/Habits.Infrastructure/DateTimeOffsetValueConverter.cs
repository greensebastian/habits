using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Habits.Infrastructure;

public class DateTimeOffsetValueConverter() : ValueConverter<DateTimeOffset, DateTimeOffset>(d => d.ToUniversalTime(),
    d => d.ToUniversalTime());