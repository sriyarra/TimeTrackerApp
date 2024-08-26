namespace TimeTracker.Controllers;

using TimeTracker.Data;
using Microsoft.AspNetCore.Mvc;

/// <summary>
/// The upload controller.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="UploadController"/> class.
/// </remarks>
/// <param name="environment">The environment.</param>
/// <param name="sessin">The sessin.</param>
public partial class UploadController(IWebHostEnvironment environment, IServiceProvider svcProvider) : Controller
{
    private readonly IWebHostEnvironment environment = environment;
    private readonly IServiceProvider serviceProvider = svcProvider;

    // Single file upload
    /// <summary>
    /// Singles the.
    /// </summary>
    /// <param name="file">The file.</param>
    /// <returns>An IActionResult.</returns>
    [HttpPost("upload/single")]
    public IActionResult Single(IFormFile file)
    {
        try
        {
            ReadData(file);

            // Put your code here
            return StatusCode(200);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    // Multiple files upload
    /// <summary>
    /// Multiples the.
    /// </summary>
    /// <param name="files">The files.</param>
    /// <returns>An IActionResult.</returns>
    [HttpPost("upload/multiple")]
    public IActionResult Multiple(IFormFile[] files)
    {
        try
        {
            ReadData(files[0]);

            // Put your code here
            return StatusCode(200);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    // Multiple files upload with parameter
    /// <summary>
    /// Posts the.
    /// </summary>
    /// <param name="files">The files.</param>
    /// <param name="id">The id.</param>
    /// <returns>An IActionResult.</returns>
    [HttpPost("upload/{id}")]
    public IActionResult Post(IFormFile[] files, int id)
    {
        try
        {
            // Put your code here
            return StatusCode(200);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    // Image file upload (used by HtmlEditor components)
    /// <summary>
    /// Images the.
    /// </summary>
    /// <param name="file">The file.</param>
    /// <returns>An IActionResult.</returns>
    [HttpPost("upload/image")]
    public IActionResult Image(IFormFile file)
    {
        try
        {
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";

            using (var stream = new FileStream(Path.Combine(environment.WebRootPath, fileName), FileMode.Create))
            {
                // Save the file
                file.CopyTo(stream);

                // Return the URL of the file
                var url = Url.Content($"~/{fileName}");

                return Ok(new { Url = url });
            }
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    /// <summary>
    /// Reads the data.
    /// </summary>
    /// <param name="file">The file.</param>
    /// <returns>A list of TimeEntries.</returns>
    private void ReadData(IFormFile file)
    {
        List<TimeEntry> records = [];
        try
        {
            Stream stream = file.OpenReadStream();
            if (!stream.CanRead)
            {
                stream.Close();
            }

            StreamReader reader = new(stream);
            string header = reader.ReadLine();

            string line = reader.ReadLine();
            while (!string.IsNullOrWhiteSpace(line))
            {
                string[] data = line.Split(',');
                if (data.Length >= 8 && DateOnly.TryParse(data[5], out DateOnly dt) && DateOnly.TryParse(data[7], out dt))
                {
                    records.Add(new TimeEntry(data));

                }
                line = reader.ReadLine();
            }
            stream.Close();
        }
        catch (Exception ex)
        {
            //console.Log($"Client-side file read error: {ex.Message}");
        }

        Session session = serviceProvider.GetService<Session>();
        session.TimeEntries = records;
    }
}
