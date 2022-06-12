using FoobarFactoryDemo.Application.Services;
using FoobarFactoryDemo.Infrastructure.Settings;
using FoobarFactoryDemo.Models.Domain;
using Microsoft.Extensions.Options;

namespace FoobarFactoryDemo.Tests.UnitTests.Application.Services;

[Trait("Category", "UnitTests")]
public class WorkerCountShutdownServiceTests
{
    private const int DefaultShutdownOnWorkerCount = 10;

    private static readonly IOptions<FactorySettings> DefaultFactorySettings = Options.Create(new FactorySettings
    {
        ShutdownOnWorkerCount = DefaultShutdownOnWorkerCount
    });

    private readonly Mock<IProductionLine<RobotState>> productionLineMock;

    private readonly IAutoShutdownService serviceUnderTest;

    public WorkerCountShutdownServiceTests()
    {
        this.productionLineMock = new Mock<IProductionLine<RobotState>>();

        this.serviceUnderTest = new WorkerCountShutdownService<RobotState>(
            this.productionLineMock.Object,
            DefaultFactorySettings);
    }

    [Fact]
    public void Construction_WithNullProductionLine_ShouldThrow()
    {
        var construction = () => new WorkerCountShutdownService<RobotState>(
            productionLine: null!,
            DefaultFactorySettings);

        construction
            .Should().Throw<ArgumentNullException>()
            .And.ParamName.Should().Be("productionLine");
    }

    [Fact]
    public void Construction_WithNullFactorySettingsOption_ShouldThrow()
    {
        var construction = () => new WorkerCountShutdownService<RobotState>(
            Mock.Of<IProductionLine<RobotState>>(),
            factorySettings: null!);

        construction
            .Should().Throw<ArgumentNullException>()
            .And.ParamName.Should().Be("factorySettings");
    }

    [Fact]
    public void Construction_WithNullFactorySettings_ShouldThrow()
    {
        var construction = () => new WorkerCountShutdownService<RobotState>(
            Mock.Of<IProductionLine<RobotState>>(),
            factorySettings: Options.Create<FactorySettings>(null!));

        construction
            .Should().Throw<ArgumentNullException>()
            .And.ParamName.Should().Be("factorySettings");
    }

    [Fact]
    public void ShouldShutdown_WithValueBelowMax_ShouldReturnFalse()
    {
        for (var i = 0; i < DefaultShutdownOnWorkerCount; i++)
        {
            this.productionLineMock
                .Setup(m => m.WorkerCount)
                .Returns(i);

            this.serviceUnderTest.ShouldShutdown.Should().BeFalse();
        }
    }

    [Theory]
    [InlineData(DefaultShutdownOnWorkerCount)]
    [InlineData(DefaultShutdownOnWorkerCount + 1)]
    [InlineData(DefaultShutdownOnWorkerCount + 10)]
    [InlineData(int.MaxValue)]
    public void ShouldShutdown_WithValueAboveOrEqualMax_ShouldReturnTrue(int workerCount)
    {
        this.productionLineMock
            .Setup(m => m.WorkerCount)
            .Returns(workerCount);

        this.serviceUnderTest.ShouldShutdown.Should().BeTrue();
    }
}