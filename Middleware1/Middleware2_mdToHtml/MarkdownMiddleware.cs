using MarkdownSharp;
using System.Text;

namespace Middleware2_mdToHtml
{
    public class MarkdownMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IWebHostEnvironment _hostEnv;

        public MarkdownMiddleware(RequestDelegate next, IWebHostEnvironment hostEnv)
        {
            _next = next;
            _hostEnv = hostEnv;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            string path = context.Request.Path.ToString();
            if(!path.EndsWith(".md", true, null)) // 只處理.md請求
            {
                await _next(context);
                return;
            }

            // 讀取wwwroot底下的文件
            var file = _hostEnv.WebRootFileProvider.GetFileInfo(path);
            if(!file.Exists)
            {
                // 處裡不了，換下一個middleware
                await _next.Invoke(context); 
                return;
            }

            using var stream = file.CreateReadStream();
            Ude.CharsetDetector cdet = new Ude.CharsetDetector();
            cdet.Feed(stream);
            cdet.DataEnd();
            string charset = cdet.Charset??"UTF-8";

            stream.Position = 0; // 因為已經被cdet讀過，所以我們先復原
            using StreamReader reader = new StreamReader(stream, Encoding.GetEncoding(charset));
            string mdText = await reader.ReadToEndAsync();

            Markdown md = new Markdown();
            string html = md.Transform(mdText);
            context.Response.ContentType = "text/html;charset=UTF-8";
            await context.Response.WriteAsync(html);
        }

    }
}
