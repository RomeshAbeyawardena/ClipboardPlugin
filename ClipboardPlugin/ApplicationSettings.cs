using Microsoft.Extensions.Configuration;

namespace ClipboardPlugin;

public record ApplicationSettings
{
	public ApplicationSettings(IConfiguration configuration)
	{
        Mappings = new Dictionary<string, IEnumerable<string>>();
        configuration.Bind(this);
    }

    public Dictionary<string, string> SwitchingProfile { get
        {
            var grouping = Mappings.GroupBy(s => s.Value);//.Select(s => KeyValuePair.Create(s.Key, s.ToArray()));
            var switchingProfiles = new Dictionary<string, string>();
            foreach (var group in grouping)
            {
                foreach(var profile in group.ToDictionary(m => m.Key, p => p.Value)) 
                    foreach(var item in profile.Value)
                        switchingProfiles.Add(item, profile.Key);
            }

            return switchingProfiles;
        } 
    }

    public Dictionary<string, IEnumerable<string>> Mappings { get; set; }
}
