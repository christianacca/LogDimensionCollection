using CcAcca.LogDimensionCollection.AspNetCore;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Moq;

namespace Specs.DefaultActionDimensionCollectorSpecs
{
    public static class Fixture
    {
        public static MvcDimensionCollectionOptions DefaultOptions
        {
            get
            {
                var options = new MvcDimensionCollectionOptions();
                ConfigureOptions(options);
                return options;
            }
        }

        public static void ConfigureOptions(MvcDimensionCollectionOptions options)
        {
            var setup = new MvcDimensionCollectionOptionsSetup();
            setup.PostConfigure("", options);
        }

        public static IOptionsMonitor<MvcDimensionCollectionOptions> OptionsOf(MvcDimensionCollectionOptions options)
        {
            var mock = new Mock<IOptionsMonitor<MvcDimensionCollectionOptions>>();
            mock.Setup(o => o.CurrentValue).Returns(options);
            return mock.Object;
        }

        public static IHttpContextAccessor ContextAccessorFor(HttpContext context)
        {
            var mock = new Mock<IHttpContextAccessor>();
            mock.Setup(o => o.HttpContext).Returns(context);
            return mock.Object;
        }

        public static DefaultActionDimensionCollector NewCollectorWith(MvcDimensionCollectionOptions options,
            DefaultHttpContext httpContext)
        {
            return new DefaultActionDimensionCollector(OptionsOf(options), ContextAccessorFor(httpContext));
        }
    }
}
