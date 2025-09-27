using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlockChain.Tests.Functional
{
    public class CustomWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram> where TProgram : class
    {
        protected override IHost CreateHost(IHostBuilder builder)
        {
            builder.UseContentRoot(GetApiProjectPath());
            return base.CreateHost(builder);
        }

        private string GetApiProjectPath()
        {   
            var dir = Directory.GetCurrentDirectory();
            var path = Path.Combine(dir, "..", "..", "..", "..","..", "src", "MyBlockChain.API");
            return Path.GetFullPath(path);
        }
    }
}
