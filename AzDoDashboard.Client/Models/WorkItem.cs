using System.Text.Json.Serialization;

namespace AzDoDashboard.Client.Models
{
    public class WorkItem
    {
        public WorkItem(AzDoWorkItems azDoWorkItem)
        {
            Number = azDoWorkItem.id;
            Type = azDoWorkItem.fields.SystemWorkItemType;
            Title = azDoWorkItem.fields.SystemTitle;
            Area = azDoWorkItem.fields.SystemAreaPath;
            Url = azDoWorkItem.url;
            Tags = azDoWorkItem.fields.SystemTags.Split(";");
        }
        public WorkItem()
        {
        }
        public int Number { get; set; }
        public string Type { get; set; } 
        public string Title { get; set; } 
        public string Area { get; set; }
        public string[] Tags { get; set; }
        public string Url { get; set; }
        public string[] Areas => Area?.Split('\\') ?? new string[] { };
    }



    public class SystemCreatedBy
    {
        public string displayName { get; set; }
        public string url { get; set; }
        public Links _links { get; set; }
        public string id { get; set; }
        public string uniqueName { get; set; }
        public string imageUrl { get; set; }
        public string descriptor { get; set; }
    }

    public class SystemChangedBy
    {
        public string displayName { get; set; }
        public string url { get; set; }
        public Links _links { get; set; }
        public string id { get; set; }
        public string uniqueName { get; set; }
        public string imageUrl { get; set; }
        public string descriptor { get; set; }
    }

    public class Fields
    {
        [JsonPropertyName("System.AreaPath")]
        public string SystemAreaPath { get; set; }

        [JsonPropertyName("System.TeamProject")]
        public string SystemTeamProject { get; set; }

        [JsonPropertyName("System.IterationPath")]
        public string SystemIterationPath { get; set; }

        [JsonPropertyName("System.WorkItemType")]
        public string SystemWorkItemType { get; set; }

        [JsonPropertyName("System.State")]
        public string SystemState { get; set; }

        [JsonPropertyName("System.Reason")]
        public string SystemReason { get; set; }

        [JsonPropertyName("System.CreatedDate")]
        public DateTime SystemCreatedDate { get; set; }

        [JsonPropertyName("System.CreatedBy")]
        public SystemCreatedBy SystemCreatedBy { get; set; }

        [JsonPropertyName("System.ChangedDate")]
        public DateTime SystemChangedDate { get; set; }

        [JsonPropertyName("System.ChangedBy")]
        public SystemChangedBy SystemChangedBy { get; set; }

        [JsonPropertyName("System.CommentCount")]
        public int SystemCommentCount { get; set; }

        [JsonPropertyName("System.Title")]
        public string SystemTitle { get; set; }

        [JsonPropertyName("Microsoft.VSTS.Common.StateChangeDate")]
        public DateTime MicrosoftVSTSCommonStateChangeDate { get; set; }

        [JsonPropertyName("Microsoft.VSTS.Common.Priority")]
        public int MicrosoftVSTSCommonPriority { get; set; }
        [JsonPropertyName("System.Tags")]
        public string SystemTags { get; set; }
    }

    public class AzDoWorkItems
    {
        public int id { get; set; }
        public int rev { get; set; }
        public Fields fields { get; set; }
        public string url { get; set; }
    }

    public class CustomPullRequestWorkItems
    {
        public int count { get; set; }
        [JsonPropertyName("value")]
        public List<AzDoWorkItems> workItems { get; set; }
    }


    public class PullRequestWorkItem
    {
        public string id { get; set; }
        public string url { get; set; }
    }

    public class PullRequestWorkItems
    {
        public int count { get; set; }
        [JsonPropertyName("value")]
        public List<PullRequestWorkItem> pullRequestWorkItem { get; set; }
    }


}
