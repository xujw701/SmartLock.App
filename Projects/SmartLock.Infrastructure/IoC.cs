using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SmartLock.Infrastructure
{
    /// <summary>
    /// Inversion of control container.
    /// </summary>
    public static class IoC
    {
        private static readonly Dictionary<Type, Type> DependencyMap = new Dictionary<Type, Type>();
        private static readonly Dictionary<Type, Func<object>> DependencyMethodMap = new Dictionary<Type, Func<object>>();
        private static readonly Dictionary<Type, object> Singletons = new Dictionary<Type, object>();

        public static T Resolve<T>()
        {
            return (T)Resolve(typeof(T));
        }

        public static void Register<TFrom, TTo>() where TTo : TFrom
        {
            DependencyMap.Add(typeof(TFrom), typeof(TTo));
        }

        public static void Register<TFrom>(Func<object> create)
        {
            DependencyMethodMap.Add(typeof(TFrom), create);
        }

        public static void Deregister<TFrom>()
        {
            if (DependencyMap.ContainsKey(typeof(TFrom)))
            {
                DependencyMap.Remove(typeof(TFrom));
            }
            else if (DependencyMethodMap.ContainsKey(typeof(TFrom)))
            {
                DependencyMethodMap.Remove(typeof(TFrom));
            }
            else if (Singletons.ContainsKey(typeof(TFrom)))
            {
                Singletons.Remove(typeof(TFrom));
            }
            else
            {
                throw new Exception(typeof(TFrom) + " was not registered");
            }
        }

        public static void RegisterSingleton<TFrom, TTo>() where TTo : TFrom
        {
            DependencyMap.Add(typeof(TFrom), typeof(TTo));
            Singletons.Add(typeof(TTo), null);
        }

        public static object Resolve(Type type)
        {
            if (DependencyMethodMap.ContainsKey(type))
            {
                return DependencyMethodMap[type]();
            }

            var resolvedType = LookUpDependency(type);
            var constructor = resolvedType.GetTypeInfo().DeclaredConstructors.First();
            var parameters = constructor.GetParameters();
            
            if (!parameters.Any())
            {
                // Constructor has no parameters - create;
                if (Singletons.ContainsKey(resolvedType))
                {
                    return Singletons[resolvedType] ??
                           (Singletons[resolvedType] = Activator.CreateInstance(resolvedType));
                }

                return Activator.CreateInstance(resolvedType);

            }

            // Constructor has parameters - resolve parameters and create;
            if (Singletons.ContainsKey(resolvedType))
            {
                return Singletons[resolvedType] ??
                       (Singletons[resolvedType] = constructor.Invoke(ResolveParameters(parameters).ToArray()));
            }

            return constructor.Invoke(ResolveParameters(parameters).ToArray());
        }

        private static Type LookUpDependency(Type type)
        {
            if (!DependencyMap.ContainsKey(type))
            {
                throw new Exception("IoC not set up for " + type);
            }

            return DependencyMap[type];
        }

        private static IEnumerable<object> ResolveParameters(IEnumerable<ParameterInfo> parameters)
        {
            return parameters
                .Select(p => Resolve(p.ParameterType))
                .ToList();
        }
    }
}
