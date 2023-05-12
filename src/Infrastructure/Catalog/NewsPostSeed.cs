using FSH.WebApi.Application.Common.Interfaces;
using FSH.WebApi.Domain.Catalog.Fund;
using FSH.WebApi.Infrastructure.Persistence.Context;
using FSH.WebApi.Infrastructure.Persistence.Initialization;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSH.WebApi.Infrastructure.Catalog;
public class NewsPostSeed : ICustomSeeder
{
    
    private readonly ApplicationDbContext _db;

    private readonly ILogger<NewsPostSeed> _logger;
    public NewsPostSeed(ApplicationDbContext context, ILogger<NewsPostSeed> logger)
    {
        _db=context;
        _logger = logger;
    }
    public async Task InitializeAsync(CancellationToken cancellationToken)
    {
        if (!_db.NewsPosts.Any())
        {
            List<NewsPost> newsPosts = new List<NewsPost>();
            for (int i = 1; i < 12; i++)
            {
                newsPosts.Add(new NewsPost()
                {
                    Title = $"This is news post title {i}",
                    Body = $"<h2>This is <strong>test</strong> <i>rich edit text  &nbsp;</i><strong> {i}</strong> </h2><ol><li><i>Test</i></li><li><i>test</i></li></ol>",
                   
                    Author = "Robot"
                });
            }
            await _db.NewsPosts.AddRangeAsync(newsPosts);
            await _db.SaveChangesAsync(cancellationToken);
            _logger.LogInformation("Seeded news posts.");
        }
    }
}
