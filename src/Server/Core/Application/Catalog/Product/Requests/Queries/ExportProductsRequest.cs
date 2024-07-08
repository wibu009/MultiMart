using MultiMart.Application.Catalog.Product.Specifications;
using MultiMart.Application.Common.Exporters;
using MultiMart.Application.Common.Models;
using MultiMart.Application.Common.Persistence;

namespace MultiMart.Application.Catalog.Product.Requests.Queries;

public class ExportProductsRequest : BaseFilter, IRequest<Stream>
{
    public Guid? BrandId { get; set; }
    public decimal? MinimumRate { get; set; }
    public decimal? MaximumRate { get; set; }
}

public class ExportProductsRequestHandler : IRequestHandler<ExportProductsRequest, Stream>
{
    private readonly IReadRepository<Domain.Catalog.Product> _repository;
    private readonly IExcelWriter _excelWriter;

    public ExportProductsRequestHandler(IReadRepository<Domain.Catalog.Product> repository, IExcelWriter excelWriter)
    {
        _repository = repository;
        _excelWriter = excelWriter;
    }

    public async Task<Stream> Handle(ExportProductsRequest request, CancellationToken cancellationToken)
    {
        var spec = new ExportProductsWithBrandsSpecification(request);

        var list = await _repository.ListAsync(spec, cancellationToken);

        return _excelWriter.WriteToStream(list);
    }
}