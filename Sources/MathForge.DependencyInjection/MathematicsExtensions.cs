using System.Reflection;
using MathForge.Distributions.Univariate.Discrete.Finite;
using Microsoft.Extensions.DependencyInjection;

namespace MathForge.DependencyInjection;

public static class MathematicsExtensions
{
	extension(IServiceCollection services)
	{
		public IServiceCollection AddDistributions()
		{
			return services.AddImplementationsOf<Distribution>(
				typeof(BernoulliDistribution).Assembly);
		}
		
		public IServiceCollection AddImplementationsOf<TBase>(Assembly assembly)
		{
			var baseType = typeof(TBase);

			var implTypes = assembly
				.GetTypes()
				.Where(t => t is { IsAbstract: false, IsInterface: false } && IServiceCollection.IsAssignableToGeneric(t, baseType));

			foreach (var type in implTypes)
				services.AddTransient(type);

			return services;
		}

		private static bool IsAssignableToGeneric(Type type, Type baseType)
		{
			if (!baseType.IsGenericTypeDefinition)
				return baseType.IsAssignableFrom(type);

			return type.GetInterfaces()
				       .Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == baseType) 
			       || (type.BaseType?.IsGenericType == true && type.BaseType.GetGenericTypeDefinition() == baseType);
		}
	}
}