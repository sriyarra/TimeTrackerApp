namespace TimeTracker.Components.Layout;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.JSInterop;
using Radzen;

/// <summary>
/// The main layout.
/// </summary>
public partial class MainLayout
{
    /// <summary>
    /// Gets or sets the j s runtime.
    /// </summary>
    [Inject]
    protected IJSRuntime JSRuntime { get; set; }

    //[Inject]
    //protected NavigationManager NavigationManager { get; set; }

    /// <summary>
    /// Gets or sets the dialog service.
    /// </summary>
    [Inject]
    protected DialogService DialogService { get; set; }

    /// <summary>
    /// Gets or sets the tooltip service.
    /// </summary>
    [Inject]
    protected TooltipService TooltipService { get; set; }

    /// <summary>
    /// Gets or sets the context menu service.
    /// </summary>
    [Inject]
    protected ContextMenuService ContextMenuService { get; set; }

    /// <summary>
    /// Gets or sets the notification service.
    /// </summary>
    [Inject]
    protected NotificationService NotificationService { get; set; }

    private bool sidebarExpanded = true;

    /// <summary>
    /// Gets or sets the context.
    /// </summary>
    [CascadingParameter]
    protected HttpContext Context { get; set; } = default!;


    /// <summary>
    /// Sidebars the toggle click.
    /// </summary>
    void SidebarToggleClick()
    {
        sidebarExpanded = !sidebarExpanded;
    }

    private string? currentUrl;

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
    private void OnLocationChanged(object sender, LocationChangedEventArgs e)
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
