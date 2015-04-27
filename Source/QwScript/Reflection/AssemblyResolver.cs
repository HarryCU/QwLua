using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace QwLua.Reflection
{
    public static class AssemblyResolver
    {
        private static Assembly OnReflectionOnlyResolve(ResolveEventArgs args)
        {
            Assembly loadedAssembly = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(
                      asm => string.Equals(asm.FullName, args.Name, StringComparison.OrdinalIgnoreCase));
            if (loadedAssembly != null)
            {
                return loadedAssembly;
            }
            return null;
        }

        private static Assembly OnReflectionOnlyResolve(ResolveEventArgs args, DirectoryInfo directory)
        {
            Assembly loadedAssembly = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(
                      asm => string.Equals(asm.FullName, args.Name, StringComparison.OrdinalIgnoreCase));

            if (loadedAssembly != null)
            {
                return loadedAssembly;
            }

            var assemblyName = new AssemblyName(args.Name);
            string dependentAssemblyFilename = Path.Combine(directory.FullName, assemblyName.Name + ".dll");
            if (File.Exists(dependentAssemblyFilename))
            {
                return Assembly.LoadFrom(dependentAssemblyFilename);
            }
            return Assembly.LoadFrom(args.Name);
        }

        public static Assembly Load(byte[] buffer)
        {
            ResolveEventHandler resolveEventHandler = (s, e) => OnReflectionOnlyResolve(e);
            AppDomain.CurrentDomain.ReflectionOnlyAssemblyResolve += resolveEventHandler;
            var assemblyInfo = Assembly.Load(buffer);
            AppDomain.CurrentDomain.ReflectionOnlyAssemblyResolve -= resolveEventHandler;
            return assemblyInfo;
        }

        public static Assembly Load(byte[] buffer, string dependentDirectory)
        {
            var directory = new DirectoryInfo(dependentDirectory);
            ResolveEventHandler resolveEventHandler = (s, e) => OnReflectionOnlyResolve(e, directory);
            AppDomain.CurrentDomain.ReflectionOnlyAssemblyResolve += resolveEventHandler;
            var assembly = Assembly.Load(buffer);
            AppDomain.CurrentDomain.ReflectionOnlyAssemblyResolve -= resolveEventHandler;
            return assembly;
        }

        public static Assembly Load(string fileName)
        {
            var directoryInfo = new FileInfo(fileName).Directory;
            if (directoryInfo != null)
                return Load(fileName, directoryInfo.FullName);
            return null;
        }

        public static Assembly Load(string fileName, string dependentDirectory)
        {
            var directory = new DirectoryInfo(dependentDirectory);
            ResolveEventHandler resolveEventHandler = (s, e) => OnReflectionOnlyResolve(e, directory);
            AppDomain.CurrentDomain.ReflectionOnlyAssemblyResolve += resolveEventHandler;
            var assembly = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(a => String.Compare(a.Location, fileName, StringComparison.Ordinal) == 0) ??
                           Assembly.ReflectionOnlyLoadFrom(fileName);
            if (assembly != null)
                assembly = Assembly.LoadFrom(assembly.Location);
            AppDomain.CurrentDomain.ReflectionOnlyAssemblyResolve -= resolveEventHandler;
            return assembly;
        }

        public static Assembly LoadByMemory(string fileName)
        {
            using (var stream = File.Open(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                var buffer = new byte[stream.Length];
                stream.Read(buffer, 0, buffer.Length);
                return Load(buffer);
            }
        }

        public static Assembly LoadByMemory(string fileName, string dependentDirectory)
        {
            using (var stream = File.Open(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                var buffer = new byte[stream.Length];
                stream.Read(buffer, 0, buffer.Length);
                return Load(buffer, dependentDirectory);
            }
        }
    }
}
