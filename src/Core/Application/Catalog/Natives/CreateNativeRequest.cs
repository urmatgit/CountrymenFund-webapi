using FSH.WebApi.Domain.Catalog.Fund;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSH.WebApi.Application.Catalog.Natives;
public class CreateNativeRequest : IRequest<DefaultIdType>
{
    public string Name { get; set; }
    public string Surname { get; set; }
    public string? MiddleName { get; set; }
    public DateTime? BirthDate { get; set; }
    
    public string Description { get; set; }
    //Rating of person
    public byte Rate { get; set; }
    public DefaultIdType RuralGovId { get; set; }
    public FileUploadRequest? Image { get; set; }
}
public class CreateNativeRequestHangler : IRequestHandler<CreateNativeRequest, DefaultIdType>
{
    readonly IRepositoryWithEvents<Native> _repository;
    private readonly IFileStorageService _file;
    public CreateNativeRequestHangler(IRepositoryWithEvents<Native> repository, IFileStorageService file)
    {
        _repository = repository;
        _file = file;
    }

    public async Task<DefaultIdType> Handle(CreateNativeRequest request, CancellationToken cancellationToken)
    {
        string nativeImage =await _file.UploadAsync<Native>(request.Image,FileType.Image,cancellationToken);
        var native = new Native(request.Name, request.Surname, request.MiddleName, request.BirthDate, request.Description, request.RuralGovId);
        await _repository.AddAsync(native);
        return native.Id; 
             
    }
}