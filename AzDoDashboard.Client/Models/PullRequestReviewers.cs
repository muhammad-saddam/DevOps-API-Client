using System.Text.Json.Serialization;

namespace AzDoDashboard.Client.Models
{
    public class PullRequestReviewers
    {
        public string reviewerUrl { get; set; }
        public int vote { get; set; }
        public bool hasDeclined { get; set; }
        public bool isRequired { get; set; }
        public bool isFlagged { get; set; }
        public string displayName { get; set; }
        public string url { get; set; }
        public Links _links { get; set; }
        public string id { get; set; }
        public string uniqueName { get; set; }
        public string imageUrl { get; set; }

    }


    public class PullRequestReviewersRoot
    {
        public int count { get; set; }
        [JsonPropertyName("value")]
        public List<PullRequestReviewers> PullRequests { get; set; }
    }


}
