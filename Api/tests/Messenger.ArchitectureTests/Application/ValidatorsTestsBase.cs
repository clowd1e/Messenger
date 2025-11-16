using FluentValidation;

namespace Messenger.ArchitectureTests.Application
{
    public sealed class ValidatorsTestsBase : TestsBase
    {
        [Fact]
        public void Validators_Should_BeSealed()
        {
            // Arrange

            // Act
            var result = Types.InAssembly(ApplicationAssembly)
                .That()
                .Inherit(typeof(AbstractValidator<>))
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
                    "The following validators should be sealed: {0}",
                    string.Join(", ", invalidTypes));
            }
        }

        [Fact]
        public void Validators_Should_BeNotPublic()
        {
            // Arrange

            // Act
            var result = Types.InAssembly(ApplicationAssembly)
                .That()
                .Inherit(typeof(AbstractValidator<>))
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
                    "The following validators should not be public: {0}",
                    string.Join(", ", invalidTypes));
            }
        }

        [Fact]
        public void Validators_Should_HaveNameEndingWithValidator()
        {
            // Arrange

            // Act
            var result = Types.InAssembly(ApplicationAssembly)
                .That()
                .Inherit(typeof(AbstractValidator<>))
                .Should()
                .HaveNameEndingWith("Validator")
                .GetResult();

            // Assert
            if (!result.IsSuccessful)
            {
                var invalidTypes = result.FailingTypes
                    .Select(t => t.FullName)
                    .ToList();

                invalidTypes.Should().BeEmpty(
                    "The following validators should end with 'Validator': {0}",
                    string.Join(", ", invalidTypes));
            }
        }
    }
}
