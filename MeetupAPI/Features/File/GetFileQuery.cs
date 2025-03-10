using System.IO;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.StaticFiles;

namespace Features.File;

public record GetFileQuery(string FileName) : IRequest<FileResult?>;

public record FileResult(byte[] FileContent, string ContentType, string FileName);

public class GetFileQueryHandler : IRequestHandler<GetFileQuery, FileResult?>
{
    public Task<FileResult?> Handle(GetFileQuery request, CancellationToken cancellationToken)
    {
        var rootFolder = Directory.GetCurrentDirectory();
        var fileFullPath = Path.Combine(rootFolder, "PrivateAssets", request.FileName);

        if (!System.IO.File.Exists(fileFullPath))
        {
            return Task.FromResult<FileResult?>(null);
        }

        var fileContent = System.IO.File.ReadAllBytes(fileFullPath);
        var fileProvider = new FileExtensionContentTypeProvider();

        fileProvider.TryGetContentType(fileFullPath, out var contentType);
        contentType ??= "application/octet-stream";

        var result = new FileResult(fileContent, contentType, request.FileName);
        return Task.FromResult<FileResult?>(result);
    }
}