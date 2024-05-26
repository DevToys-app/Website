namespace Website.Models
{
    internal record BlogPost
    {
        public required string Title { get; init; }

        public required string Description { get; init; }

        public required string Category { get; init; }

        public required DateTime Date { get; init; }

        public required string GitHubAuthorName { get; init; }

        public string HtmlFilePath { get; set; } = string.Empty;

        public string Url { get; set; } = string.Empty;
    }
}
