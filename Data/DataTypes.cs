namespace TimeTracker.Data;

using Nanosoft.Extensions.Common;

/// <summary>
/// The chart item.
/// </summary>
public class ChartItem
{
    /// <summary>
    /// Gets or sets the timestamp.
    /// </summary>
    public string Timestamp { get; set; }
    /// <summary>
    /// Gets the count.
    /// </summary>
    public uint Count { get; set; } = 1;
}

/// <summary>
/// The time entry.
/// </summary>
public class TimeEntryRaw(string[] data)
{
    /// <summary>
    /// Gets or sets the name.
    /// </summary>
    public string Name { get; set; } = data[0];
    /// <summary>
    /// Gets or sets the passcode.
    /// </summary>
    public string Passcode { get; set; } = data[1];
    /// <summary>
    /// Gets or sets the company.
    /// </summary>
    public string Company { get; set; } = data[2];
    /// <summary>
    /// Gets or sets the mobile.
    /// </summary>
    public string Mobile { get; set; } = data[3];
    /// <summary>
    /// Gets or sets the type.
    /// </summary>
    public string Type { get; set; } = data[4];
    /// <summary>
    /// Gets or sets the in date.
    /// </summary>
    public DateOnly InDate { get; set; } = DateOnly.Parse(data[5]);
    /// <summary>
    /// Gets or sets the in time.
    /// </summary>
    public TimeOnly InTime { get; set; } = TimeOnly.Parse(data[6]);
    /// <summary>
    /// Gets or sets the out date.
    /// </summary>
    public DateOnly OutDate { get; set; } = DateOnly.Parse(data[7]);
    /// <summary>
    /// Gets or sets the out time.
    /// </summary>
    public TimeOnly OutTime { get; set; } = TimeOnly.Parse(data[8]);
}

/// <summary>
/// The time entry.
/// </summary>
public class TimeEntry(string[] data) : TimeEntryRaw(data)
{
    /// <summary>
    /// Gets the in time stamp.
    /// </summary>
    public DateTime InTimeStamp => new(InDate, InTime);
    /// <summary>
    /// Gets the out time stamp.
    /// </summary>
    public DateTime OutTimeStamp => new(OutDate, OutTime);
}

/// <summary>
/// The security hour.
/// </summary>
public class SecurityLapse
{
    /// <summary>
    /// Gets or sets the date.
    /// </summary>
    public DateTime Date { get; set; }
    /// <summary>
    /// Gets or sets the from.
    /// </summary>
    public DateTime From{ get; set; }
    /// <summary>
    /// Gets or sets the to.
    /// </summary>
    public DateTime To { get; set; }
    /// <summary>
    /// Gets or sets the guard count.
    /// </summary>
    public int GuardCount { get; set; }
    /// <summary>
    /// Gets or sets the guard csv.
    /// </summary>
    public string GuardCsv { get; set; }
    /// <summary>
    /// Gets the lapse mins.
    /// </summary>
    public uint LapseMins => (uint)To.Subtract(From).TotalMinutes;
    /// <summary>
    /// Gets or sets the lapse.
    /// </summary>
    public string Lapse { get; set; }
}

/// <summary>
/// The security min.
/// </summary>
public class SecurityMin
{
    /// <summary>
    /// Gets the date.
    /// </summary>
    public DateTime Date => TimeStamp.Date;
    /// <summary>
    /// Gets the time.
    /// </summary>
    public TimeSpan Time => TimeStamp.TimeOfDay;
    /// <summary>
    /// Gets or sets the time stamp.
    /// </summary>
    public DateTime TimeStamp { get; set; }
    /// <summary>
    /// Gets or sets the count.
    /// </summary>
    public int Count { get; set; }
    /// <summary>
    /// Gets or sets the guards.
    /// </summary>
    public List<string> Guards { get; set; }
    /// <summary>
    /// Gets the guards csv.
    /// </summary>
    public string GuardsCsv => Guards?.OrderBy(h => h)?.ListToCsv();
}
