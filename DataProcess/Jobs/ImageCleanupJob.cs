using DataAccess.DbContext;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Quartz;

namespace DataAccess.Jobs;

public class ImageCleanupJob : IJob
{
    private readonly AppDbContext _context;
    private readonly IWebHostEnvironment _environment;
    private readonly ILogger<ImageCleanupJob> _logger;

    public ImageCleanupJob(
        AppDbContext context,
        IWebHostEnvironment environment,
        ILogger<ImageCleanupJob> logger)
    {
        _context = context;
        _environment = environment;
        _logger = logger;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        try
        {
            var picturePath = Path.Combine(_environment.WebRootPath, "profile_pictures");
            bool isPathExists = Directory.Exists(picturePath);
            if (!isPathExists)
            {
                return;
            }

            var filesInDirectory = Directory.GetFiles(picturePath)
                .Select(Path.GetFileName)
                .Where(f => !string.IsNullOrEmpty(f))
                .ToList();

            // Get all profile picture filenames from database
            var dbPictures = await _context.Images
                .Where(i => i.ImagePath != null)
                .Select(i => i.ImagePath)
                .ToListAsync();

            // Find files that exist in directory but not in database
            var filesToDelete = filesInDirectory
                .Where(f => !dbPictures.Contains(f))
                .ToList();

            foreach (var file in filesToDelete)
            {
                var fullPath = Path.Combine(picturePath, file);
                if (File.Exists(fullPath))
                {
                    File.Delete(fullPath);
                    _logger.LogInformation($"Deleted orphaned profile picture: {file}");
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while cleaning up profile pictures");
            throw;
        }
    }
}