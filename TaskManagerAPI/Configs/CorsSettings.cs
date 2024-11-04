namespace TaskManagerAPI.Api.Configs;

public class CorsSettings
{
  public static string SectionName { get; } = "CorsSettings";
  public string DefaultPolicy { get; init; } = null!;
}