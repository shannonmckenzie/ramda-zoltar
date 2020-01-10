using System;

namespace RamdaZoltar.Api.Models.GitHub
{
    public class PullRequest
    {
        public int Id { get; set; }
        public int Number { get; set; }
        public string State { get; set; }
        public bool? Locked { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public string ActiveLockReason { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? ClosedAt { get; set; }
        public DateTime? MergedAt { get; set; }
    }
}