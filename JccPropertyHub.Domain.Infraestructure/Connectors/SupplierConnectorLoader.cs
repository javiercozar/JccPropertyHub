using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using JccPropertyHub.Domain.Core.Interfaces;
using JccPropertyHub.Domain.Core.Services;

namespace JccPropertyHub.Domain.Infraestructure.Connectors {
    public class SupplierConnectorLoader : ISupplierConnectorLoader {
        public IEnumerable<ISupplierConnector> GetInstances(ConnectorsConfiguration configuration) {
            var path = !string.IsNullOrEmpty(configuration.PathConnectors) 
                ? AppDomain.CurrentDomain.BaseDirectory + $"/{configuration.PathConnectors}" 
                : AppDomain.CurrentDomain.BaseDirectory;

            var files = Directory.EnumerateFiles($"{path}", "*.dll");
            var assemblies = files.Select(p => Assembly.LoadFrom(p));

            var getTypes = (from assembly in assemblies
                    from t in assembly.GetTypes()
                    select t)
                .ToArray();

            var typeNames = getTypes
                .Where(x => typeof(ISupplierConnector).IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract)
                .ToList();

            var instances = typeNames.Select(p => Activator
                        .CreateInstance(p, GetParamsFromConfiguration(p, configuration))
                    as ISupplierConnector)
                .ToList();

            return instances;
        }

        private static object[] GetParamsFromConfiguration(Type s, ConnectorsConfiguration configuration) {
            var supplierConfiguration = configuration
                .Connectors
                .FirstOrDefault(supplier => s.Name.ToLower().Contains(supplier.ConnectorName.ToLower()));

            return new object[] {
                supplierConfiguration
            };
        }
    }
}