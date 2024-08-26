namespace TimeTracker.Components.Pages;

using TimeTracker.Data;
using Microsoft.AspNetCore.Components;
using Radzen;
using Radzen.Blazor;
using System;
using System.Collections.Generic;

/// <summary>
/// The index.
/// </summary>
public partial class TimeSheet
{
    IList<SecurityLapse> securityLapsesFiltered;
    IList<TimeEntry> timeEntriesFiltered;
    IList<SecurityMin> securityMinsFiltered;

    List<string> guardsFiltered;

    /// <summary>
    /// Gets or sets the protected session store.
    /// </summary>
    [Inject]
    protected Session Session { get; set; }

    protected List<int> hours = [1, 2, 3, 4];
    private readonly List<int> timeSteps = [10, 30, 60];
    RadzenUpload upload;
    int progress;
    bool showProgress;
    bool showComplete;
    string completionMessage;
    int customParameter = 1;
    bool cancelUpload = false;

    bool showGroupExpandColumn = true;
    bool? allGroupsExpanded = false;
    RadzenDataGrid<SecurityLapse> gridLapse;
    RadzenDataGrid<SecurityMin> gridGuards;
    RadzenDataGrid<TimeEntry> gridTimesheet;
    RadzenTabs tabGuard;

    IList<SecurityMin> guardChart;

    DateTime guardTime = DateTime.Now;

    DateTime? guardTimeFrom;
    DateTime? guardTimeTo;
    int guardTimeStep = 60;

    /// <summary>
    /// Ons the after render async.
    /// </summary>
    /// <param name="firstRender">If true, first render.</param>
    /// <returns>A Task.</returns>
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            //securityLapsesFiltered = Session.SecurityLapses;
            //StateHasChanged();
        }
    }

    /// <summary>
    /// Ons the render.
    /// </summary>
    /// <param name="args">The args.</param>
    void OnRender(DataGridRenderEventArgs<SecurityLapse> args)
    {
        if (args.FirstRender)
        {
            args.Grid.Groups.Add(new GroupDescriptor { Property = nameof(SecurityLapse.Date), Title = nameof(SecurityLapse.Date), SortOrder = SortOrder.Ascending });
            StateHasChanged();
        }
    }

    /// <summary>
    /// Completes the upload.
    /// </summary>
    /// <param name="args">The args.</param>
    void CompleteUpload(UploadCompleteEventArgs args)
    {
        if (!args.Cancelled)
            completionMessage = "Upload Complete!";
        else
            completionMessage = "Upload Cancelled!";

        showProgress = false;
        showComplete = true;
    }

    /// <summary>
    /// Tracks the progress.
    /// </summary>
    /// <param name="args">The args.</param>
    void TrackProgress(UploadProgressArgs args)
    {
        showProgress = true;
        showComplete = false;
        progress = args.Progress;

        // cancel upload
        args.Cancel = cancelUpload;

        // reset cancel flag
        cancelUpload = false;
    }

    /// <summary>
    /// Cancels the upload.
    /// </summary>
    void CancelUpload()
    {
        cancelUpload = true;
    }

    /// <summary>
    /// Ons the change.
    /// </summary>
    /// <param name="args">The args.</param>
    /// <param name="name">The name.</param>
    void OnChange(UploadChangeEventArgs args, string name)
    {
        foreach (FileInfo file in args.Files)
        {
            //console.Log($"File: {file.Name} / {file.Size} bytes");
        }
        //console.Log($"{name} changed");
    }

    /// <summary>
    /// Ons the progress.
    /// </summary>
    /// <param name="args">The args.</param>
    /// <param name="name">The name.</param>
    void OnProgress(UploadProgressArgs args, string name)
    {
        //console.Log($"{args.Progress}% '{name}' / {args.Loaded} of {args.Total} bytes.");

        if (args.Progress == 100)
        {
            foreach (var file in args.Files)
            {
                //console.Log($"Uploaded: {file.Name} / {file.Size} bytes");
            }
        }
    }

    /// <summary>
    /// Ons the complete.
    /// </summary>
    /// <param name="args">The args.</param>
    void OnComplete(UploadCompleteEventArgs args)
    {
        ProcessSecurityLapses();

        StateHasChanged();
        //console.Log($"Server response: {args.RawResponse}");
    }

    /// <summary>
    /// Ons the client change.
    /// </summary>
    /// <param name="args">The args.</param>
    void OnClientChange(UploadChangeEventArgs args)
    {
        //console.Log($"Client-side upload changed");

        foreach (FileInfo file in args.Files)
        {
            //console.Log($"File: {file.Name} / {file.Size} bytes");
        }
    }

    /// <summary>
    /// Processes the security hours.
    /// </summary>
    private void ProcessSecurityLapses()
    {
        List<SecurityMin> securityMins = [];
        List<SecurityLapse> lapses = [];
        DateTime first = Session.TimeEntries.OrderBy(t => t.InTimeStamp).FirstOrDefault().InTimeStamp;
        DateTime last = Session.TimeEntries.OrderByDescending(t => t.OutTimeStamp).FirstOrDefault().OutTimeStamp;

        SecurityLapse currLapse = null;

        for (DateTime ts = first; ts <= last; ts = ts.AddMinutes(1))
        {
            List<TimeEntry> entries = Session.TimeEntries.Where(e => e.InTimeStamp <= ts && e.OutTimeStamp >= ts)?.ToList();

            SecurityMin min = new()
            {
                TimeStamp = ts,
                Guards = entries?.Select(s => s.Name)?.Distinct()?.ToList()
            };
            min.Count = min.Guards.Count;

            if (min.Count < Session.MinimumGuardsRequired)
            {
                if (currLapse == null)
                {
                    currLapse = new() { Date = min.Date, From = ts, GuardCount = min.Count, GuardCsv = min.GuardsCsv };
                }
                else if (currLapse.GuardCount != min.Count)
                {
                    currLapse.To = ts;
                    currLapse.Lapse = ConvToEng(currLapse.LapseMins);
                    lapses.Add(currLapse);
                    currLapse = new() { Date = min.Date, From = ts, GuardCount = min.Count };
                }
            }
            else
            {
                if (currLapse != null)
                {
                    currLapse.To = ts;
                    currLapse.Lapse = ConvToEng(currLapse.LapseMins);
                    lapses.Add(currLapse);
                    currLapse = null;
                }
            }
            securityMins.Add(min);
        }

        lapses = lapses.Where(l => l.LapseMins > 1).ToList();
        Session.SecurityLapses = lapses;
        Session.SecurityMins = securityMins;
    }

    /// <summary>
    /// Toggles the groups.
    /// </summary>
    /// <param name="value">If true, value.</param>
    void ToggleGroups(bool? value)
    {
        allGroupsExpanded = value;
    }

    /// <summary>
    /// Ons the group row render.
    /// </summary>
    /// <param name="args">The args.</param>
    void OnGroupRowRender(GroupRowRenderEventArgs args)
    {
        //if (args.FirstRender && args.Group.Data.Key == "Vice President, Sales" || allGroupsExpanded != null)
        //{
        //    args.Expanded = allGroupsExpanded != null ? allGroupsExpanded : false;
        //}
    }

    /// <summary>
    /// Ons the group row expand.
    /// </summary>
    /// <param name="group">The group.</param>
    void OnGroupRowExpand(Group group)
    {
        //console.Log($"Group row with key: {group.Data.Key} expanded");
    }

    /// <summary>
    /// Ons the group row collapse.
    /// </summary>
    /// <param name="group">The group.</param>
    void OnGroupRowCollapse(Group group)
    {
        //console.Log($"Group row with key: {group.Data.Key} collapsed");
    }

    /// <summary>
    /// Ons the group.
    /// </summary>
    /// <param name="args">The args.</param>
    void OnGroup(DataGridColumnGroupEventArgs<SecurityLapse> args)
    {
        //console.Log($"DataGrid {(args.GroupDescriptor != null ? "grouped" : "ungrouped")} by {args.Column.GetGroupProperty()}");
    }

    /// <summary>
    /// Convs the to eng.
    /// </summary>
    /// <param name="minutes">The minutes.</param>
    /// <returns>A string.</returns>
    static string ConvToEng(double minutes)
    {
        if (minutes < 0)
        {
            return "Invalid number of minutes.";
        }

        int hours = (int)minutes / 60;
        int remainingMinutes = (int)minutes % 60;

        string result = "";

        if (hours > 0)
        {
            result += hours == 1 ? "1 hour" : $"{hours} hours";
        }

        if (remainingMinutes > 0)
        {
            if (result.Length > 0)
                result += " and ";

            result += remainingMinutes == 1 ? "1 minute" : $"{remainingMinutes} minutes";
        }

        return result.Length > 0 ? result : "0 minutes";
    }

    /// <summary>
    /// Ons the tab change.
    /// </summary>
    /// <param name="index">The index.</param>
    void OnTabChange(int index)
    {
        // console.Log($"Tab with index {index} was selected.");
    }

    /// <summary>
    /// Ons the chart page change.
    /// </summary>
    /// <param name="isNext">If true, is next.</param>
    void OnChartPageChange(bool? isNext = null)
    {
        int time = guardTimeStep * 20;
        guardTime = isNext.HasValue ? (isNext.Value ? guardTime.AddMinutes(time) : guardTime.AddMinutes(-time)) : guardTime;
        DateTime to = guardTime.AddMinutes(time);

        guardChart = Session.SecurityMins
            .Where(s => s.TimeStamp >= guardTime && s.TimeStamp <= to && s.TimeStamp.Minute % guardTimeStep == 0)?
            .OrderBy(t => t.TimeStamp)?.ToList();
    }

    /// <summary>
    /// Formats the timestamp.
    /// </summary>
    /// <param name="dateTime">The date time.</param>
    /// <returns>A string.</returns>
    private static string FormatTimestamp(object dateTime)
    {
        return ((DateTime)dateTime).ToString("dd");
    }

    /// <summary>
    /// Ons the date time range change.
    /// </summary>
    void OnDateTimeRangeChange()
    {
        if (guardTimeFrom.HasValue && guardTimeTo.HasValue)
        {
            IList<SecurityMin> mins = Session.SecurityMins
                .Where(s => s.TimeStamp >= guardTimeFrom && s.TimeStamp <= guardTimeTo && s.TimeStamp.Minute % guardTimeStep == 0)?.ToList();
            securityMinsFiltered = mins;
        }
        else
        {
            securityMinsFiltered = [];
        }

        StateHasChanged();
    }

    void OnClearCache()
    {
        Session.TimeEntries = [];
        Session.SecurityLapses = [];
        Session.SecurityMins = [];
        securityLapsesFiltered = [];
        securityMinsFiltered = [];
        guardChart = [];
        timeEntriesFiltered = [];

    }
}