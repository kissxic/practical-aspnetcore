using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc;
using NSwag.AspNetCore;
using System.Reflection;
using NJsonSchema;
using NSwag.Annotations;

namespace PracticalAspNetCore
{
    public class Startup
    {
        public Startup(IHostingEnvironment env, ILoggerFactory logger, IConfiguration configuration)
        {
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddSwaggerDocument(settings =>
            {
                settings.Title = "Sample API";
            });
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory logger, IConfiguration configuration)
        {
            app.UseStaticFiles();

            app.UseSwagger();

            app.UseSwaggerUi3(settings =>
            {
                settings.TagsSorter = "alpha";
                settings.OperationsSorter = "alpha";
            });

            app.UseMvc();
        }
    }

    [Route("")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class HomeController : ControllerBase
    {
        [HttpGet("")]
        public ActionResult Index()
        {
            return new ContentResult
            {
                Content = "<html><body><b><a href=\"/swagger\">View API Documentation</a></b></body></html>",
                ContentType = "text/html"
            };
        }
    }

    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    [ApiExplorerSettings()]
    public class GreetingController : ControllerBase
    {
        public class Greeting
        {
            public string Message { get; set; }

            public string PersonName { get; set; }

            public string PersonAddressCity { get; set; }
        }

        /// <summary>
        /// This is an API to return a "Hello World" message (this text comes from the Action comment)
        /// </summary>
        /// <response code="200">The "Hello World" text</response>
        [HttpGet("")]
        [SwaggerTag("Basic")]
        public ActionResult<Greeting> Index()
        {
            return new Greeting
            {
                Message = "Hello World"
            };
        }

        [HttpPost("goodbye")]
        [SwaggerTag("Basic")]
        public ActionResult<Greeting> Goodbye(string name)
        {
            return new Greeting
            {
                Message = "Goodbye",
                PersonName = name
            };
        }

        [HttpPut("")]
        [SwaggerTag("Intermediate")]
        public ActionResult<Greeting> Relay(Greeting greet)
        {
            return greet;
        }

        [HttpDelete("greetings/{name}")]
        [SwaggerTag("Intermediate")]
        public ActionResult Remove(string name)
        {
            return Ok($"{name} removed");
        }

        [HttpPatch("")]
        [SwaggerTag("Advanced")]
        public ActionResult<Greeting> Update(string city)
        {
            return new Greeting
            {
                Message = "Hello World",
                PersonAddressCity = city
            };
        }

        [HttpGet("hide/this")]
        [SwaggerIgnore]
        public ActionResult HideThis()
        {
            return Ok(new { gretting = "hello" });
        }

        [HttpGet("hide/this2")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public ActionResult HideThisToo()
        {
            return Ok(new { gretting = "hello" });
        }

        [HttpGet("hide/fail")]
        public ActionResult NotHidden()
        {
            return Ok(new { gretting = "hello" });
        }
    }

    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                    webBuilder.UseStartup<Startup>()
                );
    }
}