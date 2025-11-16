using System.Reflection;

namespace Messenger.ArchitectureTests
{
    public sealed class ExceptionsTests : TestsBase
    {
        [Fact]
        public void Exceptions_Should_HaveProperNaming()
        {
            // Arrange
            IEnumerable<Assembly> assemblies = [
                DomainAssembly,
                ApplicationAssembly,
                InfrastructureAssembly,
                WebApiAssembly
            ];

            // Act
            var result = Types.InAssemblies(assemblies)
                .That()
                .Inherit(typeof(Exception))
                .Should()
                .HaveNameEndingWith("Exception")
                .GetResult();

            // Assert
            if (!result.IsSuccessful)
            {
                var invalidTypes = result.FailingTypes
                    .Select(t => t.FullName)
                    .ToList();

                invalidTypes.Should().BeEmpty(
                    "The following exception types should end with 'Exception': {0}",
                    string.Join(", ", invalidTypes));
            }
        }
    }
}
