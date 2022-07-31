using Middleware1;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

// app.MapGet("/", () => "Hello World!");

app.Map("/test", async (pipeBuilder) =>
{
    pipeBuilder.UseMiddleware<CheckMIddleware>();

    pipeBuilder.Use(async (context, next) =>
    {
        context.Response.ContentType = "text/html";
        await context.Response.WriteAsync("1 Start<br/>");
        await next.Invoke();
        await context.Response.WriteAsync("1 End<br/>");
    });

    pipeBuilder.Use(async (context, next) => 
    {
        await context.Response.WriteAsync("2 Start<br/>");
        await next.Invoke();
        await context.Response.WriteAsync("2 End<br/>");
    });

    pipeBuilder.UseMiddleware<Test1MiddleWare>();
    pipeBuilder.Run(async context =>
    {
        await context.Response.WriteAsync("Run<br/>");
        dynamic? obj = context.Items["BodyJson"];
        if(obj!=null)
        {
            await context.Response.WriteAsync($"{obj.ToString()}<br/>");
        }

    });

    pipeBuilder.Run(async ctx => // �̫�@�Ӥ�����S��next
    {
        await ctx.Response.WriteAsync("hello middleware<br/>");
    });

    //pipeBuilder.Use(async (context, next) => // �ĥ|�Ӥ�����A���|����A�]���w�g��Run�F
    //{
    //    await context.Response.WriteAsync("3 Start<br/>");
    //    await next.Invoke();
    //    await context.Response.WriteAsync("3 End<br/>");
    //});
});

app.Run();
