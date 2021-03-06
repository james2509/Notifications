﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Notifications.Common.Event;
using Notifications.Common.Interfaces;
using Notifications.Common.TestAbstractions;
using Notifications.DataAccess;
using Notifications.DataAccess.Access;
using Notifications.Hub;
using Notifications.Services;
using Swashbuckle.AspNetCore.Swagger;
using Notifications.Common.Loggers;

namespace Notifications
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
            services.AddMvc();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "NotificationsAPI", Version = "v1" });
            });

            var connection = Configuration["SqlConnectionString"];
            services.AddDbContext<NotificationsDbContext>
                (options => options.UseSqlServer(connection));

            services.AddTransient<INotificationsAccess, NotificationsAccess>();
            services.AddTransient<INotificationsService, NotificationsService>();
            services.AddTransient<IClock, Clock>();
            services.AddScoped<INotificationsLogger, NotificationsConsoleLogger>();
            services.AddSingleton<INotificationNotifyEvent, NotificationNotifyEvent>();

            services.AddSignalR();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Notifications API V1");
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();

            app.UseSignalR(routes =>
            {
                routes.MapHub<NotificationHub>("/notificationhub");
            });
        }
    }
}
