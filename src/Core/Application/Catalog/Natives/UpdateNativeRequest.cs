using FSH.WebApi.Domain.Catalog.Fund;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSH.WebApi.Application.Catalog.Natives;
public class UpdateNativeRequest : IRequest<DefaultIdType>
{
    public DefaultIdType Id { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public string? MiddleName { get; set; }
    public DateTime? BirthDate { get; set; }
    public string? Village { get; set; }
    public string Description { get; set; }
    //Rating of person
    public int Rate { get; set; }
    public DefaultIdType RuralGovId { get; set; }
    public bool DeleteCurrentImage { get; set; } = false;
    public FileUploadRequest? Image { get; set; }

}
public class UpdateNativeRequestHandler : IRequestHandler<UpdateNativeRequest, DefaultIdType>
{
    private readonly IRepositoryWithEvents<Native> _repository;
    private readonly IStringLocalizer _l;
    private readonly IFileStorageService _file;
    public UpdateNativeRequestHandler(IRepositoryWithEvents<Native> repository, IStringLocalizer<UpdateNativeRequestHandler> l, IFileStorageService file)
    {
        _repository = repository;
        _l = l;
        _file = file;
    }

    public async Task<DefaultIdType> Handle(UpdateNativeRequest request, CancellationToken cancellationToken)
    {
        var native = await _repository.GetByIdAsync(request.Id, cancellationToken);
        _ = native ?? throw new NotFoundException(_l["Native {0} Not Found.", request.Id]);
        if (request.DeleteCurrentImage)
        {
            string? currentNativeImagePath = native.ImagePath;
            if (!string.IsNullOrEmpty(currentNativeImagePath))
            {
                string root = Directory.GetCurrentDirectory();
                _file.Remove(Path.Combine(root, currentNativeImagePath));
            }
            native.ClearImagePath();
        }
        string? nativeImagePath = request.Image is not null
            ? await _file.UploadAsync<Native>(request.Image, FileType.Image, cancellationToken)
            : null;
        var updateNative = native.Update(request.Name, request.Surname, request.MiddleName, request.BirthDate!.Value.ToUniversalTime(),request.Village,  request.Description, request.Rate, request.RuralGovId);
        await _repository.UpdateAsync(updateNative, cancellationToken);
        return request.Id;
    }
}
