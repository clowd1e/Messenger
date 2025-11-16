using Messenger.Application.Abstractions.Messaging;

namespace Messenger.ArchitectureTests.Application
{
    public sealed class CommandsAndQueriesTests : TestsBase
    {
        [Fact]
        public void CommandsAndQueries_Should_BeSealed()
        {
            // Arrange

            // Act

            var result = Types.InAssembly(ApplicationAssembly)
                .That()
                .ImplementInterface(typeof(ICommand))
                .Or()
                .ImplementInterface(typeof(ICommand<>))
                .Or()
                .ImplementInterface(typeof(IQuery<>))
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
                    "The following commands or queries should be sealed: {0}",
                    string.Join(", ", invalidTypes));
            }
        }

        [Fact]
        public void CommandsAndQueries_Should_BePublic()
        {
            // Arrange

            // Act
            var result = Types.InAssembly(ApplicationAssembly)
                .That()
                .ImplementInterface(typeof(ICommand))
                .Or()
                .ImplementInterface(typeof(ICommand<>))
                .Or()
                .ImplementInterface(typeof(IQuery<>))
                .Should()
                .BePublic()
                .GetResult();

            // Assert
            if (!result.IsSuccessful)
            {
                var invalidTypes = result.FailingTypes
                    .Select(t => t.FullName)
                    .ToList();

                invalidTypes.Should().BeEmpty(
                    "The following commands or queries should be public: {0}",
                    string.Join(", ", invalidTypes));
            }
        }

        [Fact]
        public void Commands_Should_HaveNameEndingWithCommand()
        {
            // Arrange

            // Act
            var result = Types.InAssembly(ApplicationAssembly)
                .That()
                .ImplementInterface(typeof(ICommand))
                .Or()
                .ImplementInterface(typeof(ICommand<>))
                .Should()
                .HaveNameEndingWith("Command")
                .GetResult();

            // Assert
            if (!result.IsSuccessful)
            {
                var invalidTypes = result.FailingTypes
                    .Select(t => t.FullName)
                    .ToList();

                invalidTypes.Should().BeEmpty(
                    "The following commands should end with 'Command': {0}",
                    string.Join(", ", invalidTypes));
            }
        }

        [Fact]
        public void Queries_Should_HaveNameEndingWithQuery()
        {
            // Arrange

            // Act
            var result = Types.InAssembly(ApplicationAssembly)
                .That()
                .ImplementInterface(typeof(IQuery<>))
                .Should()
                .HaveNameEndingWith("Query")
                .GetResult();

            // Assert
            if (!result.IsSuccessful)
            {
                var invalidTypes = result.FailingTypes
                    .Select(t => t.FullName)
                    .ToList();

                invalidTypes.Should().BeEmpty(
                    "The following queries should end with 'Query': {0}",
                    string.Join(", ", invalidTypes));
            }
        }

        [Fact]
        public void CommandsAndQueries_Should_BeRecords()
        {
            // Arrange

            // Act
            var types = Types.InAssembly(ApplicationAssembly)
                .That()
                .ImplementInterface(typeof(ICommand))
                .Or()
                .ImplementInterface(typeof(ICommand<>))
                .Or()
                .ImplementInterface(typeof(IQuery<>))
                .GetTypes();

            // Assert
            foreach (var type in types)
            {
                type.GetMethods().Any(m => m.Name == "<Clone>$").Should().BeTrue();
            }
        }
    }
}
