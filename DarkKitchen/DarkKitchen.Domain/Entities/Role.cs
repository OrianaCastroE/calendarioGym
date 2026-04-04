namespace DarkKitchen.Domain.Entities;

/// <summary>
/// Represents the role of a user in the system.
/// </summary>
public enum Role
{
    /// <summary>
    /// Administrator role with full system access.
    /// </summary>
    Admin,

    /// <summary>
    /// Client role with limited access.
    /// </summary>
    Client,

    /// <summary>
    /// Dispatcher role for managing dispatch operations.
    /// </summary>
    Dispatcher,
}
