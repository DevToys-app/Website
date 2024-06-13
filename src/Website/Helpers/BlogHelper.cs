using Markdig;
using System.Diagnostics;
using Website.Models;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Website.Helpers
{
    internal static class BlogHelper
    {
        internal static string BlogPostsDirectory { get; set; } = string.Empty;

        internal static string GeneratedHtmlDirectory { get; set; } = string.Empty;

        internal static IReadOnlyList<BlogPost> BlogPosts { get; set; } = Array.Empty<BlogPost>();

        internal static void GenerateHtml()
        {
            Debug.Assert(!string.IsNullOrEmpty(BlogPostsDirectory));

            if (!Directory.Exists(GeneratedHtmlDirectory))
            {
                Directory.CreateDirectory(GeneratedHtmlDirectory);
            }

            const string yamlDelimiter = "---";
            var posts = new List<BlogPost>();
            IDeserializer deserializer
               = new DeserializerBuilder()
                   .WithNamingConvention(CamelCaseNamingConvention.Instance)
                   .Build();

            // For each markdown file in the blog posts directory, parse the YAML front matter and convert the markdown to HTML.
            foreach (string markdownFilePath in Directory.EnumerateFiles(BlogPostsDirectory, "*.md"))
            {
                string markdown = File.ReadAllText(markdownFilePath);
                string[] parts = markdown.Split(new[] { yamlDelimiter }, StringSplitOptions.RemoveEmptyEntries);

                string html = Markdown.ToHtml(string.Join(Environment.NewLine, parts.Skip(1)));
                if (!string.IsNullOrEmpty(html))
                {
                    string filePath = Path.Combine(GeneratedHtmlDirectory, Path.GetFileName(Path.ChangeExtension(markdownFilePath, ".html")));

                    // Deserialize the YAML front matter.
                    BlogPost blogPost = deserializer.Deserialize<BlogPost>(parts[0]);
                    blogPost.HtmlFilePath = filePath;
                    blogPost.Url = Path.GetFileNameWithoutExtension(markdownFilePath);

                    // Write the HTML to a file.
                    File.WriteAllText(filePath, html);

                    posts.Add(blogPost);
                }
            }

            // Sort the blog posts by date.
            BlogPosts = posts.OrderByDescending(post => post.Date).ToList();
        }

        internal static BlogPost? GetBlogPost(string url)
        {
            return BlogPosts.FirstOrDefault(post => post.Url == url);
        }
    }
}
