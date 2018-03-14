using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.IO;
using TodoApi.Models;

namespace TodoApi
{
    /// <summary>
    /// <remarks>
    /// Swashbuckle.AspNetCore.Swagger:将SwaggerDocument对象公开为JSON终结点的Swagger对象模型和中间件
    /// </remarks>
    /// <remarks>
    /// Swashbuckle.AspNetCore.SwaggerGen:Swagger生成器,从你的路由、控制器和模型直接生成SwaggerDocument对象.它通常与Swagger终结点中间件结合,以自动公开Swagger JSON.
    /// </remarks>
    /// <remarks>
    /// Swashbuckle.AspNetCore.SwaggerUI:Swagger UI工具的嵌入式版本,它解释Swagger JSON以构建描述Web API功能的可自定义的丰富体验.它包括针对公共方法的内置测试工具
    /// </remark>
    /// </summary>
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<TodoContext>(opt => opt.UseInMemoryDatabase("TodoList"));
            services.AddMvc();

            //将Swagger生成器添加到此处服务集合中
            services.AddSwaggerGen(c =>
            {
                //传递给SwaggerGen方法的配置操作可用于添加信息,如作者、许可证和说明
                c.SwaggerDoc("v1", new Info
                {
                    Version = "v1",
                    Title = "ToDo API",
                    Description="A simple example ASP.NET Core Web API",
                    TermsOfService="None",
                    Contact = new Contact { Name="Shayne Boyer",Email="",Url="https://twitter.com/spboyer" },
                    License = new License { Name="Use under LICX",Url="https://example.com/license"}
                });

                //配置Swagger以使用生成的XML文件
                //Set the comments path for the Swagger JSON and UI.
                var basePath = AppContext.BaseDirectory;
                var xmlPath = Path.Combine(basePath, "TodoApi.xml");
                c.IncludeXmlComments(xmlPath);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseMvc();

            app.UseStaticFiles();
            
            //在此处启用中间件为生成的JSON文档和SwaggerUI提供服务
            //Enable middleware to serve generated Swagger as a JSON endpoint
            app.UseSwagger();

            //Enable middleware to servce swagger-ui(HTML,JS,CSS,etc.),specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c => 
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });
        }
    }
}
