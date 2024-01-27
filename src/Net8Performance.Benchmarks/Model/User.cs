namespace NetPerformance.Benchmarks.Model;

internal record User(
    Guid UserId,
    string FirstName,
    string LastName,
    string FullName,
    string Username,
    string Email
);
