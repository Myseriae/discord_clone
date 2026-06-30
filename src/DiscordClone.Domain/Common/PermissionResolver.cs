namespace DiscordClone.Domain.Common;

/// <summary>
/// Pure bit-math for the permission bitfield. Combines role permissions and tests for a
/// required permission. No dependencies — the data-loading (owner override, @everyone baseline)
/// lives in the Infrastructure permission service.
/// </summary>
public static class PermissionResolver
{
    /// <summary>OR-s a set of role permission bitfields into a single combined value.</summary>
    public static long Combine(IEnumerable<long> rolePermissions)
    {
        long combined = 0L;
        foreach (var permissions in rolePermissions)
        {
            combined |= permissions;
        }

        return combined;
    }

    /// <summary>True when <paramref name="permissions"/> contains every bit in <paramref name="required"/>.</summary>
    public static bool Has(long permissions, long required) => (permissions & required) == required;
}
