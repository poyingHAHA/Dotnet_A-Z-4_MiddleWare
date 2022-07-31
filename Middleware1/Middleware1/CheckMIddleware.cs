using Dynamic.Json;

namespace Middleware1
{
    public class CheckMIddleware
    {
        private readonly RequestDelegate next;

        public CheckMIddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task InvokeAsync(HttpContext context) // 名字不要打錯
        {
            string password = context.Request.Query["password"];
            if(password == "123")
            {
                if(context.Request.HasJsonContentType())
                {
                    var stream = context.Request.BodyReader.AsStream();
                    // system.text.json目前不支持把Json反序列化為dynamic類型，要安裝dynamic.json
                    dynamic? obj = DJson.ParseAsync(stream);
                    context.Items["BodyJson"] = obj;
                }
                await next.Invoke(context);
            }
            else
            {
                context.Response.StatusCode = 401;
            }
        }
    }
}
