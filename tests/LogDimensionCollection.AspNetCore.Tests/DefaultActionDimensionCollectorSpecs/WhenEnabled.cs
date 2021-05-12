using System.Collections.Generic;
using CcAcca.LogDimensionCollection.AspNetCore;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Xunit;

namespace Specs.DefaultActionDimensionCollectorSpecs
{
    public class WhenEnabled
    {
        [Fact]
        public void Enabled_Should_Run_Action_Delegate()
        {
            var context = new DefaultHttpContext();
            var options = Fixture.DefaultOptions;
            var sut = Fixture.NewCollectorWith(options, context);

            // when
            var dimensions = new Dictionary<string, object>
            {
                ["InterestingKey"] = "123"
            };
            sut.WhenEnabled(x => x.CollectDimensions(dimensions));

            // then
            var expected = new Dictionary<string, object>(dimensions);
            context.Items[options.AggregatedDimensionsKey].Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void Disabled_Should_Not_Run_Action_Delegate()
        {
            var context = new DefaultHttpContext();
            var options = Fixture.DefaultOptions;
            options.Enabled = false;
            var sut = Fixture.NewCollectorWith(options, context);

            // when
            var dimensions = new Dictionary<string, object>
            {
                ["InterestingKey"] = "123"
            };
            sut.WhenEnabled(x => x.CollectDimensions(dimensions));

            // then
            var expected = new Dictionary<string, object>(dimensions);
            context.Items.Should().BeEmpty();
        }
    }
}
