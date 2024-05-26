# How to write an article

1. Create a new Markdown file in the `_posts` directory.
1. Write your article in markdown format.
1. If your article includes images, place them in `wwwroot/blog/your_article_folder` and reference them in your article like this:
   ```markdown
   ![Alt text](/blog/your_article_folder/image.png)
   ```
1. At the top of the file, include the following metadata:
   ```markdown
   ---
   title: Article title
   description: Description of the article
   category: The category name
   date: 12/31/2024
   gitHubAuthorName: github_user_name
   ---
   ```
1. Test your changes local by running the site locally.
1. Submit a pull request with your changes.