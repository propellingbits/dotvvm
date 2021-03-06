﻿using System.Threading.Tasks;
using DotVVM.Framework.Hosting;
using DotVVM.Framework.Runtime.Filters;
using DotVVM.Framework.Binding.Properties;
using DotVVM.Framework.Binding;
using StackExchange.Profiling;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace DotVVM.Tracing.MiniProfiler.AspNetCore
{
    public class MiniProfilerActionFilter : ActionFilterAttribute
    {
        private readonly IOptions<MiniProfilerOptions> minipProfilerOptions;
        private readonly PathString resultsLink;

        public MiniProfilerActionFilter(IOptions<MiniProfilerOptions> minipProfilerOptions)
        {
            this.minipProfilerOptions = minipProfilerOptions;
            this.resultsLink = minipProfilerOptions.Value.RouteBasePath.Add(new PathString("/results-index"));
        }

        protected override Task OnPageLoadedAsync(IDotvvmRequestContext context)
        {
            // Naming for PostBack occurs in method OnCommandExecutingAsync
            // We don't want to override it with less information
            if (!context.IsPostBack)
            {
                AddMiniProfilerName(context, context.HttpContext.Request.Url.AbsoluteUri);
            }

            return base.OnPageLoadedAsync(context);
        }

        protected override Task OnCommandExecutingAsync(IDotvvmRequestContext context, ActionInfo actionInfo)
        {
            var commandCode = actionInfo.Binding
                ?.GetProperty<OriginalStringBindingProperty>(Framework.Binding.Expressions.ErrorHandlingMode.ReturnNull)?.Code;

            AddMiniProfilerName(context, context.HttpContext.Request.Url.AbsoluteUri,
                $"(PostBack {commandCode})");

            return base.OnCommandExecutingAsync(context, actionInfo);
        }

        private void AddMiniProfilerName(IDotvvmRequestContext context, params string[] nameParts)
        {
            var currentMiniProfiler = StackExchange.Profiling.MiniProfiler.Current;
            
            if (currentMiniProfiler != null)
            {
                currentMiniProfiler.AddCustomLink("results", resultsLink);
                currentMiniProfiler.Name = string.Join(" ", nameParts);
            }
        }
    }
}
