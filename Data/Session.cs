namespace TimeTracker.Data;

/// <summary>
/// The session.
/// </summary>
public class Session()
{
    /// <summary>
    /// Gets or sets the security mins.
    /// </summary>
    public List<SecurityMin> SecurityMins { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether lapses processed.
    /// </summary>
    public bool LapsesProcessed => SecurityLapses.Count > 0;
    /// <summary>
    /// Gets a value indicating whether lapses not processed.
    /// </summary>
    public bool LapsesNotProcessed => !LapsesProcessed;
    /// <summary>
    /// Gets or sets the security lapses.
    /// </summary>
    public List<SecurityLapse> SecurityLapses { get; set; } = [];
    /// <summary>
    /// Gets or sets the minimum guards required.
    /// </summary>
    public int MinimumGuardsRequired { get; set; } = 2;
    /// <summary>
    /// Gets or sets the time entries.
    /// </summary>
    public List<TimeEntry> TimeEntries { get; set; }
    /// <summary>
    /// Gets or sets the check by hours.
    /// </summary>
    public int CheckByHours { get; set; } = 1;
    /// <summary>
    /// Gets or sets the guards.
    /// </summary>
    public List<string> Guards { get; set; }
}
