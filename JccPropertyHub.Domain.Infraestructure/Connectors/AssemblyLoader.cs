using System.Reflection;
using System.Runtime.Loader;

namespace JccPropertyHub.Domain.Infraestructure.Connectors {
    public class AssemblyLoader : AssemblyLoadContext {
        protected override Assembly Load(AssemblyName assemblyName) {
            return LoadFromAssemblyName(assemblyName);
        }
    }
}