using Microsoft.Extensions.FileProviders;
using Website.blog;
using Website.Components;

namespace Website
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddRazorComponents().AddInteractiveServerComponents();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            // Serve the static files in the "doc" folder.
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "doc")),
                RequestPath = "/doc"
            });

            app.UseRouting();
            app.UseAntiforgery();

            app.MapGet("/doc", () => Results.Redirect("/doc/articles/introduction.html"));

            app.MapRazorComponents<App>().AddInteractiveServerRenderMode();

            // Build the blog.
            BlogHelper.BlogPostsDirectory = Path.Combine(app.Environment.ContentRootPath, "blog", "_posts");
            BlogHelper.GeneratedHtmlDirectory = Path.Combine(app.Environment.ContentRootPath, "blog", "_generated");
            BlogHelper.GenerateHtml();

            app.Run();
        }
    }
}
