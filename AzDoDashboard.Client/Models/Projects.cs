using System.Text.Json.Serialization;

namespace AzDoDashboard.Client.Models
{
   public class Projects
    {
            public int count { get; set; }
        [JsonPropertyName("value")]
        public List<ProjectDetails> projectDetails { get; set; }
    }
    public class ProjectDetails
    {
        [JsonPropertyName("id")]
        public string? Id { get; set; }
        [JsonPropertyName("name")]
        public string? Name { get; set; }
        [JsonPropertyName("url")]
        public string? URL { get; set; }
        [JsonPropertyName("state")]
        public string? State { get; set; }
        [JsonPropertyName("revision")]
        public int Revision { get; set; }
        [JsonPropertyName("visibility")]
        public string? Visibility { get; set; }
        [JsonPropertyName("lastUpdateTime")]
        public DateTime LastUpdateTime { get; set; }
        [JsonPropertyName("description")]
        public string? Description { get; set; }
    }
}
