using AzDoDashboard.Client.Interfaces;
using AzDoDashboard.Client.Models;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using System.Net.Http.Headers;
using System.Text;

namespace AzDoDashboard.Client
{
    public class AzureDevOpsClient : IAzureDevOpsClient
    {
        private static HttpClient _client = new HttpClient();

        private readonly string _personalAccessToken;
        private readonly ILogger<AzureDevOpsClient> _logger;
        private readonly ICache _cache;
        private string _hostUrl;

        public AzureDevOpsClient(string hostUrl, string personalAccessToken, ICache cache, ILogger<AzureDevOpsClient> logger)
        {
            _personalAccessToken = personalAccessToken;
            // _client = RestService.For<IAzureDevOpsClient>(hostUrl);
            _cache = cache;
            _hostUrl = hostUrl;
            _logger = logger;

            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.ASCII.GetBytes(string.Format("{0}:{1}", "", personalAccessToken))));

            ApprovalPolicy = new Dictionary<string, int>()
            {
                ["develop"] = 1,
                ["master"] = 2
            };
        }

        /// <summary>
        /// number of approvals needed per pull request by branch name
        /// </summary>
        public Dictionary<string, int> ApprovalPolicy { get; }

        public async Task<IEnumerable<ProjectDetails>> GetAllProjectsAsync()
        {
            using var response = await _client.GetAsync(_hostUrl + "/_apis/projects?api-version=6.0");
            response.EnsureSuccessStatusCode();

            var pageResult = await response.Content.ReadAsStringAsync();
            Projects projects = JsonSerializer.Deserialize<Projects>(pageResult);
            return projects?.projectDetails;
        }

        /// <summary>
        /// nuber of active Pull requests by Project Name
        /// </summary>
        public async Task<IEnumerable<AzDoPullRequest>> GetOpenPullRequestsByProjectAsync(string ProjectName)
        {
            string uri = String.Join("/", _hostUrl, ProjectName, "_apis/git/pullrequests?api-version=6.0");
            try
            {
                using var response = await _client.GetAsync(uri);
                response.EnsureSuccessStatusCode();

                var pageResult = await response.Content.ReadAsStringAsync();
                CustomPullRequest pullRequests = JsonSerializer.Deserialize<CustomPullRequest>(pageResult);
                return pullRequests?.pullRequests;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                _logger.LogError(ex.StackTrace, "GetOpenPullRequestsByProjectAsync");
                return null;
            }
        }

        public async Task<IEnumerable<PullRequest>> GetAllOpenPullRequestsAsync()
        {
            List<PullRequest> pullRequestList = new List<PullRequest>();
            try
            {
                IEnumerable<ProjectDetails> projectDetails = await GetAllProjectsAsync();
                foreach (var project in projectDetails)
                {
                    string uri = String.Join("/", _hostUrl, project.Name, "_apis/git/pullrequests?api-version=6.0");
                    using var pullRequestResponse = await _client.GetAsync(uri);
                    pullRequestResponse.EnsureSuccessStatusCode();
                    var pullRequestResult = await pullRequestResponse.Content.ReadAsStringAsync();
                    CustomPullRequest? customModel = pullRequestResult != null? JsonSerializer.Deserialize<CustomPullRequest>(pullRequestResult) : null;
                    if (customModel != null && customModel.count > 0)
                    {
                        foreach (var customPullRequest in customModel.pullRequests)
                        {
                            PullRequest pullRequest = new PullRequest(customPullRequest);
                            pullRequest.ApprovalsReceived = await GetPullRequestReviewerList(pullRequest, project.Name);
                            pullRequest.WorkItems = await GetPullRequestWorkItemsByPullRequestIdAsync(pullRequest, project.Name);
                            pullRequestList.Add(pullRequest);
                        }
                    }
                }
                return pullRequestList;
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                _logger.LogError(ex.StackTrace, "GetAllOpenPullRequestsAsync");
                return null;
            }

        }

        public Task<IEnumerable<WorkItem>> GetOpenWorkItemsByTagAsync(string tagName)
        {
            throw new NotImplementedException();
        }

        private async Task<List<string>> GetPullRequestReviewerList(PullRequest pullRequest, string projectName)
        {
            List<string> reviewerList = new List<string>();
            string uri = String.Join("/", _hostUrl, projectName, "_apis/git/repositories/" + pullRequest.pullRequestRepositoryId + "/pullrequests/" + pullRequest.Number + "/reviewers?api-version=6.0");
            using var pullRequestReviewerResponse = await _client.GetAsync(uri);
            pullRequestReviewerResponse.EnsureSuccessStatusCode();
            var pullRequestReviewerResult = await pullRequestReviewerResponse.Content.ReadAsStringAsync();
            PullRequestReviewersRoot PullRequestReviewers = JsonSerializer.Deserialize<PullRequestReviewersRoot>(pullRequestReviewerResult);
            if (PullRequestReviewers != null && PullRequestReviewers.count > 0)
            {
                foreach (var reviewer in PullRequestReviewers.PullRequests)
                {
                    if (reviewer.vote != 0 && reviewer.hasDeclined == false)
                        reviewerList.Add(reviewer.displayName);
                }
            }
            return reviewerList;
        }

        // method to return all work item information by PullRequest
        private async Task<List<WorkItem>> GetPullRequestWorkItemsByPullRequestIdAsync(PullRequest pullRequest, string projectName)
        {
            string uri = String.Join("/", _hostUrl, projectName, "_apis/git/repositories/" + pullRequest.pullRequestRepositoryId + "/pullrequests/" + pullRequest.Number + "/workitems?api-version=6.0");
            using var pullRequestWorkItemsResponse = await _client.GetAsync(uri);
            pullRequestWorkItemsResponse.EnsureSuccessStatusCode();
            var pullRequestWorkItemsResponseResult = await pullRequestWorkItemsResponse.Content.ReadAsStringAsync();
            PullRequestWorkItems pullRequestWorkItems = JsonSerializer.Deserialize<PullRequestWorkItems>(pullRequestWorkItemsResponseResult);
            var workItemIds = String.Join(",", pullRequestWorkItems.pullRequestWorkItem.Select(p => p.id));
            List<WorkItem> WorkItemList = await GetWorkItemsbyWorkItemIds(workItemIds, projectName);
            return WorkItemList;
            // return PullRequestReviewers.workItems;
        }

        // method to retrun WOrkitems information by comma sepearted string list of WorkItem Ids
        private async Task<List<WorkItem>> GetWorkItemsbyWorkItemIds(string workItemIds, string projectName)
        {
            List<WorkItem> workItemList = new List<WorkItem>();
            //https://dev.azure.com/MuhammadSaddam0495/Azure-Integration-Services/_apis/wit/workitems?ids=2&api-version=6.0
            string uri = String.Join("/", _hostUrl, projectName, "_apis/wit/workitems?ids=" + workItemIds + "&api-version=6.0");
            using var WorkItemsResponse = await _client.GetAsync(uri);
            WorkItemsResponse.EnsureSuccessStatusCode();
            var WorkItemsResponseResult = await WorkItemsResponse.Content.ReadAsStringAsync();
            CustomPullRequestWorkItems workItems = JsonSerializer.Deserialize<CustomPullRequestWorkItems>(WorkItemsResponseResult);
            if (workItems.count > 0)
            {
                foreach (var item in workItems.workItems)
                {
                    WorkItem workItem = new WorkItem(item);
                    workItemList.Add(workItem);
                }
                return workItemList;
            }
            return workItemList;
        }
    }


}