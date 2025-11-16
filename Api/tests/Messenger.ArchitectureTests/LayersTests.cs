namespace Messenger.ArchitectureTests
{
    public sealed class LayersTests : TestsBase
    {
        [Fact]
        public void Domain_Should_Not_HaveDependencyOnApplicationLayer()
        {
            // Arrange

            // Act
            var result = Types.InAssembly(DomainAssembly)
                .Should()
                .NotHaveDependencyOn(ApplicationAssembly.FullName)
                .GetResult();

            // Assert
            result.IsSuccessful.Should().BeTrue();
        }

        [Fact]
        public void Domain_Should_Not_HaveDependencyOnInfrastructureLayer()
        {
            // Arrange

            // Act
            var result = Types.InAssembly(DomainAssembly)
                .Should()
                .NotHaveDependencyOn(InfrastructureAssembly.FullName)
                .GetResult();

            // Assert
            result.IsSuccessful.Should().BeTrue();
        }

        [Fact]
        public void Domain_Should_Not_HaveDependencyOnPresentationLayer()
        {
            // Arrange

            // Act
            var result = Types.InAssembly(DomainAssembly)
                .Should()
                .NotHaveDependencyOn(WebApiAssembly.FullName)
                .GetResult();

            // Assert
            result.IsSuccessful.Should().BeTrue();
        }

        [Fact]
        public void Application_Should_Not_HaveDependencyOnInfrastructureLayer()
        {
            // Arrange

            // Act
            var result = Types.InAssembly(ApplicationAssembly)
                .Should()
                .NotHaveDependencyOn(InfrastructureAssembly.FullName)
                .GetResult();

            // Assert
            result.IsSuccessful.Should().BeTrue();
        }

        [Fact]
        public void Application_Should_Not_HaveDependencyOnPresentationLayer()
        {
            // Arrange

            // Act
            var result = Types.InAssembly(ApplicationAssembly)
                .Should()
                .NotHaveDependencyOn(WebApiAssembly.FullName)
                .GetResult();

            // Assert
            result.IsSuccessful.Should().BeTrue();
        }

        [Fact]
        public void Infrastructure_Should_Not_HaveDependencyOnPresentationLayer()
        {
            // Arrange

            // Act
            var result = Types.InAssembly(InfrastructureAssembly)
                .Should()
                .NotHaveDependencyOn(WebApiAssembly.FullName)
                .GetResult();

            // Assert
            result.IsSuccessful.Should().BeTrue();
        }
    }
}
