using System.Reflection;

namespace Messenger.ArchitectureTests
{
    public abstract class TestsBase
    {
        protected static readonly Assembly DomainAssembly = typeof(Domain.AssemblyMarker).Assembly;

        protected static readonly Assembly ApplicationAssembly = typeof(Messenger.Application.AssemblyMarker).Assembly;

        protected static readonly Assembly InfrastructureAssembly = typeof(Infrastructure.AssemblyMarker).Assembly;

        protected static readonly Assembly WebApiAssembly = typeof(WebAPI.AssemblyMarker).Assembly;
    }
}
