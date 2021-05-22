using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using WebAPI.Kafka;
using WebAPI.Models;
using WebAPI.MongoRepository;

namespace WebAPI
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

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "WebAPI", Version = "v1" });
            });

            services.Configure<MongoConnectionSetting>(
                    Configuration.GetSection(nameof(MongoConnectionSetting)));

            services.AddSingleton<IMongoConnectionSetting>(sp =>
                        sp.GetRequiredService<IOptions<MongoConnectionSetting>>().Value);

            services.Configure<KafkaConfig>(
                    Configuration.GetSection(nameof(KafkaConfig)));

            services.AddSingleton<IKafkaConfig>(sp =>
                        sp.GetRequiredService<IOptions<KafkaConfig>>().Value);

            services.AddTransient<IRepository, Repository>();
            services.AddHostedService<Consumer>();
            services.AddTransient<IProducer, Producer>();
            services.AddTransient<IKafkaAdmin, KafkaAdmin>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebAPI v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
