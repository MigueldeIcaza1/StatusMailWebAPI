using Autofac;  
using Autofac.Integration.WebApi;
using AutoStatus.WebAPI.Interfaces;
using AutoStatus.WebAPI.Services;
using System;  
using System.Collections.Generic;  
using System.Linq;  
using System.Reflection;  
using System.Web;  
using System.Web.Http;  
  
namespace AutoStatus.WebAPI.App_Start
{
    public class AutofacWebapiConfig
    {

        public static IContainer Container;

        public static void Initialize(HttpConfiguration config)
        {
            Initialize(config, RegisterServices(new ContainerBuilder()));
        }


        public static void Initialize(HttpConfiguration config, IContainer container)
        {
            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);
        }

        private static IContainer RegisterServices(ContainerBuilder builder)
        {
            //Register your Web API controllers.  
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            // var builder = new ContainerBuilder();
            builder.RegisterType<TFSService>().As<ITaskManagementService>();
            builder.RegisterType<EmailSender>().As<IEmailSender>();
            builder.RegisterType<StatusSender>().As<IStatusSender>();

            // var container = builder.Build();

            // var statusSender = container.Resolve<IStatusSender>();

            //Set the dependency resolver to be Autofac.  
            Container = builder.Build();

            return Container;
        }

    }

    public class Bootstrapper
    {
        public static void Run()
        {
            //Configure AutoFac  
            AutofacWebapiConfig.Initialize(GlobalConfiguration.Configuration);
        }

    }
}