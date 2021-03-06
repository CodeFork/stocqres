﻿using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Autofac;
using Stocqres.Application.StockGroup.Services;
using Stocqres.Core.Commands;
using Stocqres.Core.Events;
using Stocqres.Infrastructure.ExternalServices;
using Stocqres.Infrastructure.ExternalServices.StockExchangeService;

namespace Stocqres.Application
{
    public static class ApplicationDependencyContainer
    {
        public static void Load(ContainerBuilder builder)
        {
            var assembly = Assembly.GetExecutingAssembly();

            builder
                .RegisterAssemblyTypes(assembly)
                .AsClosedTypesOf(typeof(ICommandHandler<>))
                .InstancePerLifetimeScope();

            builder
                .RegisterAssemblyTypes(assembly)
                .AsClosedTypesOf(typeof(IEventHandler<>))
                .InstancePerLifetimeScope();

            
            builder.RegisterType<StockExchangeService>().As<IStockExchangeService>();
            builder.RegisterType<StockGroupService>().As<IStockGroupService>();
        }
    }
}
