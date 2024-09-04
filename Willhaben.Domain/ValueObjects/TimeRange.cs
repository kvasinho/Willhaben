using Willhaben.Domain.Exceptions;

namespace Willhaben.Domain.Models;

public class TimeRange
{
    // Nullable backing fields
    private TimeOnly? _from = null;
    private TimeOnly? _to = null;

    // Public properties with default values
    public TimeOnly From
    {
        get => _from ?? TimeOnly.MinValue;  // Return MinValue if _from is null
        set
        {
            if (value == To) // Compare against the public To property
            {
                throw new EqualTimeException();
            }

            _from = value;
        }
    }

    public TimeOnly To
    {
        get => _to ?? TimeOnly.MaxValue;  // Return MaxValue if _to is null
        set
        {
            if (value == From) // Compare against the public From property
            {
                throw new EqualTimeException();
            }

            _to = value;
        }
    }
    public bool IsValidTimeFrom(TimeOnly time)
    {
        if ((_to.HasValue && _to.Value == time) || (!_to.HasValue && time == TimeOnly.MaxValue))
        {
            return false;
        }

        return true;
    }

    public bool IsValidTimeTo(TimeOnly time)
    {
        if ((_from.HasValue && _from.Value == time) || (!_from.HasValue && time == TimeOnly.MinValue))
        {
            return false;
        }

        return true;
    }

}