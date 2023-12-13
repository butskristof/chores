using Chores.Application.Common.Exceptions;
using Chores.Application.IntegrationTests.Common;
using Chores.Domain.Models;

namespace Chores.Application.IntegrationTests.Exceptions;

[Collection(nameof(ApplicationFixture))]
public sealed class AuthenticationExceptionTests : ApplicationTestBase
{
    public AuthenticationExceptionTests(ApplicationFixture application) : base(application)
    {
    }

    [Fact]
    public async Task UserIdNull_ThrowsOnAudit()
    {
        Application.SetUserId(null);
        var action = () => Application.AddAsync(new Tag { Name = "never gonna happen" });
        await action
            .Should()
            .ThrowAsync<AuthenticationException>();
    }
}