using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OrderTracker.Models;
using OrderTracker.Services;

namespace OrderTracker
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{

			services.Configure<MongoDBSettings>(
				Configuration.GetSection(nameof(MongoDBSettings)));

			services.AddSingleton<IMongoDBSettings>(sp =>
				sp.GetRequiredService<IOptions<MongoDBSettings>>().Value);

			//			services.AddSingleton<MongoDBOrdersRepository>();
			services.AddSingleton<IOrdersRepository> (sp =>
				sp.GetRequiredService<IOptions<InMemoryOrdersRepository>>().Value);

			services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);			
			
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IHostingEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			app.UseMvc();
		}
	}
}
