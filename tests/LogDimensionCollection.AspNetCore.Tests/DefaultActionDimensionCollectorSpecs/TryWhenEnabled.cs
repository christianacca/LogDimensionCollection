using System.Collections.Generic;
using CcAcca.LogDimensionCollection.AspNetCore;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Xunit;

namespace Specs.DefaultActionDimensionCollectorSpecs
{
    public class TryWhenEnabled
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
            var expected = new Dictionary<string, string>
            {
                ["InterestingKey"] = "123"
            };
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
            context.Items.Should().BeEmpty();
        }

        [Fact]
        public void Enabled_Action_Delegate_Throws()
        {
            var context = new DefaultHttpContext();
            var options = Fixture.DefaultOptions;
            options.Enabled = false;
            var existingDimensions = new Dictionary<string, string>
            {
                ["ExistingKey"] = "123"
            };
            context.Items[options.AggregatedDimensionsKey] = existingDimensions;
            var sut = Fixture.NewCollectorWith(options, context);

            // when
            var dimensions = new Dictionary<string, object>
            {
                ["InterestingKey"] = "123"
            };
            sut.TryWhenEnabled(x => x.CollectDimensions(dimensions));

            // then
            var expected = new Dictionary<string, string>(existingDimensions);
            context.Items[options.AggregatedDimensionsKey]
                .Should().BeSameAs(existingDimensions)
                .And.BeEquivalentTo(expected);
        }
    }
}
