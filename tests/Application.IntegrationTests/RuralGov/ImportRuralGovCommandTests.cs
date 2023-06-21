using FluentAssertions;
using FSH.WebApi.Application.Catalog.RuralGovs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.IntegrationTests.RuralGov;
using static Testing;
public class ImportRuralGovCommandTests: TestBase
{
    [Test]
    public void CreateRuralGov()
    {
        var cmd = new CreateRuralGovRequest()
        {
            Name = "TestRuralGov",
        };
        //var result = await SendAsync(cmd);
        FluentActions.Invoking(() =>
         SendAsync(cmd)).Should().ThrowAsync<FluentValidation.ValidationException>();
    }
}
