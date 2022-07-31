using Middleware1;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

// app.MapGet("/", () => "Hello World!");

app.Map("/test", async (pipeBuilder) =>
{
    // 第一個中間件，context代表當前請求上下文，next代表下一個中間件
    pipeBuilder.Use(async (context, next) =>
    {
        context.Response.ContentType = "text/html";
        await context.Response.WriteAsync("1 Start<br/>");
        await next.Invoke();
        await context.Response.WriteAsync("1 End<br/>");
    });

    pipeBuilder.Use(async (context, next) => // 第二個中間件
    {
        // 不用再設置content Type因為第一個中間件已經弄好了
        await context.Response.WriteAsync("2 Start<br/>");
        await next.Invoke();
        await context.Response.WriteAsync("2 End<br/>");
    });

    pipeBuilder.UseMiddleware<Test1MiddleWare>();

    pipeBuilder.Run(async ctx => // 最後一個中間件沒有next
    {
        await ctx.Response.WriteAsync("hello middleware<br/>");
    });

    //pipeBuilder.Use(async (context, next) => // 第四個中間件，不會執行，因為已經有Run了
    //{
    //    await context.Response.WriteAsync("3 Start<br/>");
    //    await next.Invoke();
    //    await context.Response.WriteAsync("3 End<br/>");
    //});
});

app.Run();
