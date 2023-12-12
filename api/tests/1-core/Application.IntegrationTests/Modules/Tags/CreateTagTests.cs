using Chores.Application.IntegrationTests.Common;
using Chores.Application.Modules.Tags;
using ErrorOr;

namespace Chores.Application.IntegrationTests.Modules.Tags;

// [Collection(nameof(TestFixture))]
// public sealed class CreateTagTests : TestBase
// {
//     public CreateTagTests(TestFixture fixture) : base(fixture)
//     {
//     }
//
//     [Fact]
//     public async Task InvalidRequest_ReturnsValidationError()
//     {
//         var request = new CreateTag.Request(string.Empty);
//         var result = await Fixture.SendAsync(request);
//         Assert.True(result.IsError);
//         var error = result.ErrorsOrEmptyList.SingleOrDefault();
//         Assert.Equal(ErrorType.Validation, error.Type);
//     }
//     
//     [Fact]
//     public async Task CreatesTag()
//     {
//         var request = new CreateTag.Request("hey");
//         var response = await Fixture.SendAsync(request);
//         Assert.False(response.IsError);
//         Assert.NotEqual(Guid.Empty, response.Value.Id);
//         Assert.Equal("hey", response.Value.Name);
//     }
//
//     public sealed class RequestWithValidationErrors
//     {
//         [Fact]
//         public void ReturnsValidationError()
//         {
//             
//         }
//     }
// }

public static class CreateTagTests
{
    [Collection(nameof(TestFixture))]
    public sealed class InvalidRequest : TestBase, IClassFixture<InvalidRequest.InnerFixture>
    {
        public InvalidRequest(TestFixture fixture, InnerFixture innerFixture) : base(fixture)
        {
            _fixture = innerFixture;
        }

        public class InnerFixture : IAsyncLifetime
        {
            public ErrorOr<CreateTag.Response> Result { get; }

            public async Task InitializeAsync()
            {
                var request = new CreateTag.Request(string.Empty);
                var result = await Fixture.SendAsync(request);
            }

            public Task DisposeAsync()
            {
                throw new NotImplementedException();
            }
        }

        private readonly InnerFixture _fixture;

        [Fact]
        public async Task ReturnsValidationResult()
        {
            Assert.True(result.IsError);
            var error = result.Errors.SingleOrDefault();
            Assert.Equal(ErrorType.Validation, error.Type);
        }
    }
}