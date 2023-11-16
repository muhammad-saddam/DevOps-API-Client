
using System.Text.Json.Serialization;

namespace AzDoDashboard.Client.Models
{
    public class PullRequest
    {
        public PullRequest() { }
        public PullRequest(AzDoPullRequest pullRequest)
        {
            Number = pullRequest.pullRequestId;
            Title = pullRequest.title;
            RepositoryName = pullRequest.repository.RepositoryName;
            TargetBranch = pullRequest.TargetBranch;
            CreatedBy = pullRequest.createdBy.displayName;
            DateCreated = pullRequest.creationDate;
            IsDraft = pullRequest.isDraft;
            Url = pullRequest.url;
            pullRequestRepositoryId = pullRequest.repository.id;
        }

        [JsonPropertyName("pullRequestId")]
        public int Number { get; set; }
        [JsonPropertyName("title")]
        public string Title { get; set; }
        [JsonPropertyName("name")]
        public string RepositoryName { get; set; }
        [JsonPropertyName("targetRefName")]
        public string TargetBranch { get; set; } 
        public string CreatedBy { get; set; }
        public DateTime DateCreated { get; set; }
        public bool IsDraft { get; set; } 
        public string pullRequestRepositoryId { get; set; }
        public IEnumerable<WorkItem> WorkItems { get; set; }
        public string Url { get; set; } 
        public IEnumerable<string> ApprovalsReceived { get; set; }
    }


    public class Project
    {
        public string id { get; set; }
        public string name { get; set; }
        public string state { get; set; }
        public string visibility { get; set; }
        public DateTime lastUpdateTime { get; set; }
    }

    public class Repository
    {
        public string id { get; set; }
        [JsonPropertyName("name")]
        public string RepositoryName { get; set; }
        public string url { get; set; }
        public Project project { get; set; }
    }

    public class Avatar
    {
        public string href { get; set; }
    }

    public class Links
    {
        public Avatar avatar { get; set; }
    }

    public class CreatedBy
    {
        public string displayName { get; set; }
        public string url { get; set; }
        public Links _links { get; set; }
        public string id { get; set; }
        public string uniqueName { get; set; }
        public string imageUrl { get; set; }
        public string descriptor { get; set; }
    }

    public class LastMergeSourceCommit
    {
        public string commitId { get; set; }
        public string url { get; set; }
    }

    public class LastMergeTargetCommit
    {
        public string commitId { get; set; }
        public string url { get; set; }
    }

    public class LastMergeCommit
    {
        public string commitId { get; set; }
        public string url { get; set; }
    }

    public class AzDoPullRequest
    {
        public Repository repository { get; set; }
        public int pullRequestId { get; set; }
        public int codeReviewId { get; set; }
        public string status { get; set; }
        public CreatedBy createdBy { get; set; }
        public DateTime creationDate { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public string sourceRefName { get; set; }
        [JsonPropertyName("targetRefName")]
        public string TargetBranch { get; set; }
        public string mergeStatus { get; set; }
        public bool isDraft { get; set; }
        public string mergeId { get; set; }
        public LastMergeSourceCommit lastMergeSourceCommit { get; set; }
        public LastMergeTargetCommit lastMergeTargetCommit { get; set; }
        public LastMergeCommit lastMergeCommit { get; set; }
        public List<object> reviewers { get; set; }
        public string url { get; set; }
        public bool supportsIterations { get; set; }
    }

    public class CustomPullRequest
    {
        [JsonPropertyName("value")]
        public IEnumerable<AzDoPullRequest> pullRequests { get; set; }
        public int count { get; set; }
    }


}
