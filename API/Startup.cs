using API.Extensions;
using API.Helpers;
using API.Middleware;
using AutoMapper;
using Infrastructure.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace API
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


      services.AddAutoMapper(typeof(MappingProfiles));
      services.AddControllers();
      // services.AddDbContext<StoreContext>(x => x.UseSqlite(Configuration.GetConnectionString("Default")));
      services.AddDbContext<StoreContext>(x => x.UseSqlServer(Configuration.GetConnectionString("RemoteDefault")));

      services.AddApplicationServices();
      services.AddSwaggerDocumentation();

    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
      // if (env.IsDevelopment())
      // {
      //   app.UseDeveloperExceptionPage();
      // }

      app.UseMiddleware<ExceptionMiddleware>();

      app.UseStatusCodePagesWithReExecute("/errors/{0}");

      app.UseHttpsRedirection();

      app.UseRouting();

      app.UseStaticFiles();

      app.UseAuthorization();


      app.UseSwaggerDocumentation();

      app.UseEndpoints(endpoints =>
      {
        endpoints.MapControllers();
      });
    }
  }
}
