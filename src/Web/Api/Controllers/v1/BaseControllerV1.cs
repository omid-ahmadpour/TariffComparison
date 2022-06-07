﻿namespace TariffComparison.Api.Controllers.v1
{
    using ApiFramework.Attributes;
    using AutoMapper;
    using MediatR;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using System;

    [ValidateModelState]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class BaseControllerV1 : ControllerBase
    {
        protected IServiceProvider Resolver
        {
            get
            {
                return HttpContext.RequestServices;
            }
        }

        protected T GetService<T>()
        {
            return Resolver.GetService<T>();
        }

        protected IMapper Mapper => GetService<IMapper>();

        protected IMediator Mediator => GetService<IMediator>();

        protected ILogger Logger => GetService<ILogger>();
    }
}