namespace TimeTracker.Components.Layout;

using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Components;

/// <summary>
/// The nav menu.
/// </summary>
public partial class NavMenu
{
    private string currentUrl;

    /// <summary>
    /// Gets or sets the navigation manager.
    /// </summary>
    [Inject]
    protected NavigationManager NavigationManager { get; set; }

    /// <summary>
    /// Ons the initialized.
    /// </summary>
    protected override void OnInitialized()
    {
        currentUrl = NavigationManager.ToBaseRelativePath(NavigationManager.Uri);
        NavigationManager.LocationChanged += OnLocationChanged;
    }

    /// <summary>
    /// Ons the location changed.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The e.</param>
    private void OnLocationChanged(object? sender, LocationChangedEventArgs e)
    {
        currentUrl = NavigationManager.ToBaseRelativePath(e.Location);
        StateHasChanged();
    }

    /// <summary>
    /// Disposes the.
    /// </summary>
    public void Dispose()
    {
        NavigationManager.LocationChanged -= OnLocationChanged;
    }
}
