namespace AzDoDashboard.Client.Interfaces
{
    internal interface IAzureDevOpsClient
    {
        /// <summary>
        /// [Get All Open pullrequests by project name
        /// </summary>        
        Task<IEnumerable<Models.AzDoPullRequest>> GetOpenPullRequestsByProjectAsync(string projectName);
        /// <summary>
        /// [Get...
        /// </summary>        
        Task<IEnumerable<Models.WorkItem>> GetOpenWorkItemsByTagAsync(string tagName);
        /// <summary>
        /// Get list of all the projects with their details in an organization
        /// </summary> 
        Task<IEnumerable<Models.ProjectDetails>> GetAllProjectsAsync();
        /// <summary>
        /// Get list of all the pull requests in an organization
        /// </summary> 
        Task<IEnumerable<Models.PullRequest>> GetAllOpenPullRequestsAsync();
    }
}
