using Messenger.Application.Abstractions.Data;
using System.Reflection;

namespace Messenger.ArchitectureTests.Application
{
    public sealed class MappersTestsBase : TestsBase
    {
        [Fact]
        public void Mappers_Should_BeNotBePublic()
        {
            // Arrange

            // Act
            var result = Types.InAssembly(ApplicationAssembly)
                .That()
                .Inherit(typeof(Mapper<,>))
                .Should()
                .NotBePublic()
                .GetResult();

            // Assert
            if (!result.IsSuccessful)
            {
                var invalidTypes = result.FailingTypes
                    .Select(t => t.FullName)
                    .ToList();

                invalidTypes.Should().BeEmpty(
                    "The following mappers should not be public: {0}",
                    string.Join(", ", invalidTypes));
            }
        }

        [Fact]
        public void Mappers_Should_BeSealed()
        {
            // Arrange

            // Act
            var result = Types.InAssembly(ApplicationAssembly)
                .That()
                .Inherit(typeof(Mapper<,>))
                .Should()
                .BeSealed()
                .GetResult();

            // Assert
            if (!result.IsSuccessful)
            {
                var invalidTypes = result.FailingTypes
                    .Select(t => t.FullName)
                    .ToList();

                invalidTypes.Should().BeEmpty(
                    "The following mappers should be sealed: {0}",
                    string.Join(", ", invalidTypes));
            }
        }

        [Fact]
        public void Mappers_Should_HaveNameEndingWithMapper()
        {
            // Arrange

            // Act
            var result = Types.InAssembly(ApplicationAssembly)
                .That()
                .Inherit(typeof(Mapper<,>))
                .Should()
                .HaveNameEndingWith("Mapper")
                .GetResult();

            // Assert
            if (!result.IsSuccessful)
            {
                var invalidTypes = result.FailingTypes
                    .Select(t => t.FullName)
                    .ToList();

                invalidTypes.Should().BeEmpty(
                    "The following mappers should end with 'Mapper': {0}",
                    string.Join(", ", invalidTypes));
            }
        }

        [Fact]
        public void Mappers_Should_Not_HavePublicFielsAndProperties()
        {
            // Arrange

            // Act
            var types = Types.InAssembly(ApplicationAssembly)
                .That()
                .Inherit(typeof(Mapper<,>))
                .GetTypes();

            // Assert
            foreach (var type in types)
            {
                var publicFields = type.GetFields(
                    BindingFlags.Public | BindingFlags.Instance);

                publicFields.Should().BeEmpty();

                var publicProperties = type.GetProperties(
                    BindingFlags.Public | BindingFlags.Instance);

                publicProperties.Should().BeEmpty();
            }
        }

        [Fact]
        public void Mappers_Should_Not_HaveStaticMembers()
        {
            // Arrange

            // Act
            var types = Types.InAssembly(ApplicationAssembly)
                .That()
                .Inherit(typeof(Mapper<,>))
                .GetTypes();

            // Assert
            foreach (var type in types)
            {
                var staticFields = type.GetFields(
                    BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);

                staticFields.Should().BeEmpty();

                var staticProperties = type.GetProperties(
                    BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);

                staticProperties.Should().BeEmpty();
            }
        }
    }
}
