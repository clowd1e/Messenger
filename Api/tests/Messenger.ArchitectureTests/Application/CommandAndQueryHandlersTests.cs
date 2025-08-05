using Messenger.Application.Abstractions.Messaging;
using System.Reflection;

namespace Messenger.ArchitectureTests.Application
{
    public sealed class CommandAndQueryHandlersTests : TestsBase
    {
        [Fact]
        public void CommandAndQueryHandlers_Should_BeSealed()
        {
            // Arrange

            // Act
            var result = Types.InAssembly(ApplicationAssembly)
                .That()
                .ImplementInterface(typeof(ICommandHandler<>))
                .Or()
                .ImplementInterface(typeof(ICommandHandler<,>))
                .Or()
                .ImplementInterface(typeof(IQueryHandler<,>))
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
                    "The following command or query handlers should be sealed: {0}",
                    string.Join(", ", invalidTypes));
            }
        }

        [Fact]
        public void CommandAndQueryHandlers_Should_BeNotPublic()
        {
            // Arrange

            // Act
            var result = Types.InAssembly(ApplicationAssembly)
                .That()
                .ImplementInterface(typeof(ICommandHandler<>))
                .Or()
                .ImplementInterface(typeof(ICommandHandler<,>))
                .Or()
                .ImplementInterface(typeof(IQueryHandler<,>))
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
                    "The following command or query handlers should not be public: {0}",
                    string.Join(", ", invalidTypes));
            }
        }

        [Fact]
        public void CommandHandlers_Should_HaveNameEndingWithCommandHandler()
        {
            // Arrange

            // Act
            var result = Types.InAssembly(ApplicationAssembly)
                .That()
                .ImplementInterface(typeof(ICommandHandler<>))
                .Or()
                .ImplementInterface(typeof(ICommandHandler<,>))
                .Should()
                .HaveNameEndingWith("CommandHandler")
                .GetResult();

            // Assert
            if (result.IsSuccessful)
            {
                var invalidTypes = result.FailingTypes
                    .Select(t => t.FullName)
                    .ToList();

                invalidTypes.Should().BeEmpty(
                    "The following command handlers should end with 'CommandHandler': {0}",
                    string.Join(", ", invalidTypes));
            }
        }

        [Fact]
        public void QueryHandlers_Should_HaveNameEndingWithQueryHandler()
        {
            // Arrange

            // Act
            var result = Types.InAssembly(ApplicationAssembly)
                .That()
                .ImplementInterface(typeof(IQueryHandler<,>))
                .Should()
                .HaveNameEndingWith("QueryHandler")
                .GetResult();

            // Assert
            if (result.IsSuccessful)
            {
                var invalidTypes = result.FailingTypes
                    .Select(t => t.FullName)
                    .ToList();

                invalidTypes.Should().BeEmpty(
                    "The following query handlers should end with 'QueryHandler': {0}",
                    string.Join(", ", invalidTypes));
            }
        }

        [Fact]
        public void CommandAndQueryHandlers_Should_Not_HavePublicFieldsAndProperties()
        {
            // Arrange

            // Act
            var types = Types.InAssembly(ApplicationAssembly)
                .That()
                .ImplementInterface(typeof(ICommandHandler<>))
                .Or()
                .ImplementInterface(typeof(ICommandHandler<,>))
                .Or()
                .ImplementInterface(typeof(IQueryHandler<,>))
                .GetTypes();

            // Assert
            foreach (var type in types)
            {
                var publicFields = type.GetFields(
                    BindingFlags.Public | BindingFlags.Instance);

                publicFields.Should().BeEmpty();

                var publicProperties = type.GetProperties();

                publicProperties.Should().BeEmpty();
            }
        }

        [Fact]
        public void CommandAndQueryHandlers_Should_Not_HaveStaticMembers()
        {
            // Arrange

            // Act
            var types = Types.InAssembly(ApplicationAssembly)
                .That()
                .ImplementInterface(typeof(ICommandHandler<>))
                .Or()
                .ImplementInterface(typeof(ICommandHandler<,>))
                .Or()
                .ImplementInterface(typeof(IQueryHandler<,>))
                .GetTypes();

            // Assert
            foreach (var type in types)
            {
                var staticFields = type.GetFields(
                    BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static);

                staticFields.Should().BeEmpty();

                var staticProperties = type.GetProperties(
                    BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static);

                staticProperties.Should().BeEmpty();
            }
        }
    }
}
