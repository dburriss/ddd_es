using AutoMapper.SelfConfig;
using Lifecycle;
using PhilosophicalMonkey;
using System;

namespace Web.Infrastructure
{
    public class AutoMapperSetupTask : IRunAtStartup
    {
        public void Execute()
        {
            var seedTypes = new Type[] { typeof(Startup) };
            var assemblies = Reflect.OnTypes.GetAssemblies(seedTypes);
            var typesInAssemblies = Reflect.OnTypes.GetAllExportedTypes(assemblies);
            MappingLoader.LoadAllMappings(typesInAssemblies);
        }
    }
}
