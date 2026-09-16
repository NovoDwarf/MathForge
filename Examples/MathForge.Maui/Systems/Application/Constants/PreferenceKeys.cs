using System.Reflection;

namespace MathForge.Maui.Systems.Application.Constants;

public static class PreferenceKeys
{
	public static string[] All { get; } = GetAll();
	
	public const string Theme = "app_theme";
	public const string Locale = "app_locale";
	public const string RecentActivities = "recent_activities";
	
	private static string[] GetAll()
	{
		return typeof(PreferenceKeys).GetFields(BindingFlags.Public | BindingFlags.Static)
			.Where(f => f.FieldType == typeof(string))
			.Select(f => f.GetValue(null)?.ToString() ?? string.Empty)
			.ToArray();
	}
}