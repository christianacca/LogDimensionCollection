using CcAcca.LogDimensionCollection.AspNetCore;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Xunit;

namespace Specs.DefaultActionDimensionCollectorSpecs
{
    public class CollectDimensions
    {
        [Fact]
        public void Dimension_Supplied()
        {
            // given
            var context = new DefaultHttpContext();
            var options = Fixture.DefaultOptions;
            var sut = Fixture.NewCollectorWith(options, context);

            // when
            var anon = new { Prop1 = "Value1", Prop2 = 10 };
            var dimensions = new Dictionary<string, object?>
            {
                ["InterestingKey"] = anon
            };
            sut.CollectActionDimensions(dimensions);

            // then
            var expected = new Dictionary<string, string?>
            {
                [$"{options.ActionDimensionPrefix}InterestingKey"] = options.SerializeValue(anon)
            };
            context.Items[options.AggregatedDimensionsKey].Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void Null_Dimensions_Supplied()
        {
            // given
            var context = new DefaultHttpContext();
            var options = Fixture.DefaultOptions;
            var sut = Fixture.NewCollectorWith(options, context);

            // when
            sut.CollectActionDimensions(null!);

            // then
            context.Items.Should().NotContainKey(options.AggregatedDimensionsKey);
        }

        [Fact]
        public void Dimension_Supplied_With_Empty_Key()
        {
            // given
            var context = new DefaultHttpContext();
            var options = Fixture.DefaultOptions;
            var sut = Fixture.NewCollectorWith(options, context);

            // when
            var dimensions = new Dictionary<string, object?>
            {
                [""] = 123
            };
            sut.CollectActionDimensions(dimensions);

            // then
            context.Items.Should().NotContainKey(options.AggregatedDimensionsKey);
        }

        [Fact]
        public void Existing_Dimensions_Dimension_Supplied()
        {
            // given
            var context = new DefaultHttpContext();
            var options = Fixture.DefaultOptions;
            var existingDimensions = new Dictionary<string, string>
            {
                ["ExistingKey"] = "123"
            };
            context.Items[options.AggregatedDimensionsKey] = existingDimensions;
            var sut = Fixture.NewCollectorWith(options, context);

            // when
            var dimensions = new Dictionary<string, object?>
            {
                ["InterestingKey"] = "345"
            };
            sut.CollectActionArgDimensions(dimensions);

            // then
            var expected = new Dictionary<string, string>
            {
                ["ExistingKey"] = "123",
                [$"{options.ActionArgDimensionPrefix}InterestingKey"] = "345"
            };
            context.Items[options.AggregatedDimensionsKey]
                .Should().BeSameAs(existingDimensions)
                .And.BeEquivalentTo(expected);
        }

        [Fact]
        public void Custom_Options_ActionDimensionPrefix_Dimension_Supplied()
        {
            // given
            var context = new DefaultHttpContext();
            var customOptions = new MvcDimensionCollectionOptions
            {
                ActionDimensionPrefix = "Act:"
            };
            Fixture.ConfigureOptions(customOptions);
            var sut = Fixture.NewCollectorWith(customOptions, context);

            // when
            var dimensions = new Dictionary<string, object?>
            {
                ["InterestingKey"] = "345"
            };
            sut.CollectActionArgDimensions(dimensions);

            // then
            var expected = new Dictionary<string, string>
            {
                ["Act:Input.InterestingKey"] = "345"
            };
            var actual = context.Items[customOptions.AggregatedDimensionsKey];
            actual.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void Custom_Options_Empty_ActionDimensionPrefix_Dimension_Supplied()
        {
            // given
            var context = new DefaultHttpContext();
            var customOptions = new MvcDimensionCollectionOptions
            {
                ActionDimensionPrefix = ""
            };
            Fixture.ConfigureOptions(customOptions);
            var sut = Fixture.NewCollectorWith(customOptions, context);

            // when
            var dimensions = new Dictionary<string, object?>
            {
                ["InterestingKey"] = "345"
            };
            sut.CollectActionArgDimensions(dimensions);

            // then
            var expected = new Dictionary<string, string>
            {
                ["Input.InterestingKey"] = "345"
            };
            var actual = context.Items[customOptions.AggregatedDimensionsKey];
            actual.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void Custom_Options_Empty_ActionArgDimensionPrefix_Dimension_Supplied()
        {
            // given
            var context = new DefaultHttpContext();
            var customOptions = new MvcDimensionCollectionOptions
            {
                ActionArgDimensionPrefix = ""
            };
            Fixture.ConfigureOptions(customOptions);
            var sut = Fixture.NewCollectorWith(customOptions, context);

            // when
            var dimensions = new Dictionary<string, object?>
            {
                ["InterestingKey"] = "345"
            };
            sut.CollectActionArgDimensions(dimensions);

            // then
            var expected = new Dictionary<string, object?>(dimensions);
            var actual = context.Items[customOptions.AggregatedDimensionsKey];
            actual.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void Existing_Dimensions_Null_Dimensions_Supplied()
        {
            // given
            var context = new DefaultHttpContext();
            var options = Fixture.DefaultOptions;
            var existingDimensions = new Dictionary<string, string>
            {
                ["ExistingKey"] = "123"
            };
            context.Items[options.AggregatedDimensionsKey] = existingDimensions;
            var sut = Fixture.NewCollectorWith(options, context);

            // when
            sut.CollectActionArgDimensions(null!);

            // then
            var expected = new Dictionary<string, string>(existingDimensions);
            context.Items[options.AggregatedDimensionsKey]
                .Should().BeSameAs(existingDimensions)
                .And.BeEquivalentTo(expected);
        }
    }
}
