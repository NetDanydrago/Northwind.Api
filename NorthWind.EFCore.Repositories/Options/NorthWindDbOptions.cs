namespace NorthWind.EFCore.Repositories.Options;
public  class NorthWindDbOptions
{
    public const string SectionKey = nameof(NorthWindDbOptions);

    public string ConnectionString { get; set; }
}
