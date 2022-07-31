namespace Middleware1
{
    public class Test1MiddleWare
    {
        private readonly RequestDelegate next;

        public Test1MiddleWare(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task InvokeAsync(HttpContext context) // 名字不要打錯
        {
            context.Response.WriteAsync("Test1Middleware start<br/>");
            await next.Invoke(context);
            context.Response.WriteAsync("Test1Middleware end<br/>");
        }
    }
}
