using System.Reflection;

namespace Habits.Core;

public class NotFoundException(MemberInfo requestedType, string query) : Exception($"Could not find {requestedType.Name} by {query}");