using Microsoft.Extensions.Options;
using ProyectoEdificios.Models.Options;

namespace ProyectoEdificios.Services.Projects
{
    public sealed class ChangeControlWorkbookSource : IChangeControlWorkbookSource
    {
        private readonly IWebHostEnvironment _environment;
        private readonly ResourcesOptions _options;
        private readonly ILogger<ChangeControlWorkbookSource> _logger;

        public ChangeControlWorkbookSource(
            IWebHostEnvironment environment,
            IOptions<ResourcesOptions> options,
            ILogger<ChangeControlWorkbookSource> logger)
        {
            _environment = environment;
            _options = options.Value;
            _logger = logger;
        }

        public Task<Stream?> OpenReadAsync(CancellationToken cancellationToken = default)
        {
            return Task.FromResult(OpenLocalFile());
        }

        private Stream? OpenLocalFile()
        {
            var filePath = Path.IsPathRooted(_options.FolderName)
                ? Path.Combine(_options.FolderName, _options.ChangeControlFileName)
                : Path.Combine(_environment.ContentRootPath, _options.FolderName, _options.ChangeControlFileName);

            if (!File.Exists(filePath))
            {
                _logger.LogWarning("Change control file was not found at {FilePath}", filePath);
                return null;
            }

            return File.OpenRead(filePath);
        }
    }
}
