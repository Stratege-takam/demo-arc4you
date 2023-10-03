using Arc4u.Configuration;
using AutoFixture;
using AutoFixture.AutoMoq;
using EG.DemoPCBE99925.Core.Business;
using FluentAssertions;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;

namespace EG.DemoPCBE99925.Core.UnitTest;

public class EnvironmentBLTests
{
    public EnvironmentBLTests()
    {
        _fixture = new Fixture();
        _fixture.Customize(new AutoMoqCustomization());
    }

    private readonly Fixture _fixture;

    [Fact]
    public async Task GetEnvironmentShouldBe()
    {
        // arrange
        var config = _fixture.Create<ApplicationConfig>();

        var mockOptions = _fixture.Freeze<Mock<IOptions<ApplicationConfig>>>();
        mockOptions.Setup(p => p.Value).Returns(config);

        var sut = _fixture.Create<EnvironmentInfoBL>();

        // act
        var envInfo = await sut.GetEnvironmentInfoAsync().ConfigureAwait(false);

        // assert
        envInfo.Name.Should().Be(config.Environment.Name);
    }
}
