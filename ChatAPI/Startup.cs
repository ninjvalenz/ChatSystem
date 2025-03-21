using ChatAPI.Service.Interface;
using Common.Handlers;
using Confluent.Kafka;
using MessageBroker.Interface;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ChatAPI
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
            services.AddDbContext<ChatDbContext>(options =>
         options.UseSqlServer(Configuration.GetConnectionString("ChatSystem")));

            var producerConfig = new ProducerConfig { BootstrapServers = "localhost:9092" };
            var producerBuilder = new ProducerBuilder<string, string>(producerConfig).Build();

            services.AddSingleton<IProducer<string, string>>(producerBuilder);
            services.AddSingleton<IMessagePublisher, KafkaMessagePublisher>();
            services.AddScoped<IChatService, ChatService>();

            services.AddScoped<GetActiveChatsHandler>();
            services.AddScoped<CloseChatSessionHandler>();
            services.AddScoped<CreateChatSessionHandler>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
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
