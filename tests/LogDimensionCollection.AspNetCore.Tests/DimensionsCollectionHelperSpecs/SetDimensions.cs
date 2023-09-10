using CcAcca.LogDimensionCollection.AspNetCore;
using FluentAssertions;
using Xunit;

namespace Specs.DimensionsCollectionHelperSpecs
{
    public class SetDimensions
    {
        [Fact]
        public void No_Dimensions_To_Collect()
        {
            // given
            var existing = new Dictionary<string, string?>();

            // when
            existing.SetDimensions(new Dictionary<string, object?>());

            // then
            existing.Should().BeEmpty();
        }

        [Fact]
        public void One_String_Dimension_To_Collect()
        {
            // given
            var existing = new Dictionary<string, string?>();

            // when
            var dimensions = new Dictionary<string, object?>
            {
                ["InterestingKey"] = "123"
            };
            existing.SetDimensions(dimensions);

            // then
            existing.Should().BeEquivalentTo(dimensions);
        }

        [Fact]
        public void One_Numeric_Dimension_To_Collect()
        {
            // given
            var existing = new Dictionary<string, string?>();

            // when
            var dimensions = new Dictionary<string, object?>
            {
                ["InterestingKey"] = 10
            };
            existing.SetDimensions(dimensions);

            // then
            var expected = new Dictionary<string, string?>
            {
                ["InterestingKey"] = "10"
            };
            existing.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void One_Anonymous_Object_Dimension_To_Collect()
        {
            // given
            var existing = new Dictionary<string, string?>();

            // when
            var anon = new { Prop1 = "Value1", Prop2 = 10 };
            var dimensions = new Dictionary<string, object?>
            {
                ["InterestingKey"] = anon
            };
            existing.SetDimensions(dimensions);

            // then
            var expected = new Dictionary<string, string?>
            {
                ["InterestingKey"] = DimensionsCollectionHelper.SerializeValue(anon)
            };
            existing.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void Has_Existing_Dimensions_One_Dimension_To_Collect()
        {
            // given
            var existing = new Dictionary<string, string?>
            {
                ["ExistingKey"] = "345"
            };

            // when
            var dimensions = new Dictionary<string, object?>
            {
                ["InterestingKey"] = "123"
            };
            existing.SetDimensions(dimensions);

            // then
            var expected = new Dictionary<string, string?>
            {
                ["ExistingKey"] = "345",
                ["InterestingKey"] = "123"
            };
            existing.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void Has_Existing_Dimensions_Dimension_Of_Object_To_Collect()
        {
            // given
            var existing = new Dictionary<string, string?>
            {
                ["ExistingKey"] = "345"
            };

            // when
            var dimensions = new Dictionary<string, object>
            {
                ["InterestingKey"] = "123"
            };
            var dimensionsCast = dimensions as IDictionary<string, object?>;
            existing.SetDimensions(dimensionsCast);


            // then
            var expected = new Dictionary<string, string?>
            {
                ["ExistingKey"] = "345",
                ["InterestingKey"] = "123"
            };
            existing.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void Prefix_Supplied_Dimension_Keys_Prefixed()
        {
            // given
            var existing = new Dictionary<string, string?>();

            // when
            var dimensions = new Dictionary<string, object?>
            {
                ["InterestingKey"] = "123"
            };
            existing.SetDimensions(dimensions, "SomePrefix.");

            // then
            var expected = new Dictionary<string, string?>
            {
                ["SomePrefix.InterestingKey"] = "123"
            };
            existing.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void Should_Pascal_Case_Prefix()
        {
            // given
            var existing = new Dictionary<string, string?>();

            // when
            var dimensions = new Dictionary<string, object?>
            {
                ["InterestingKey"] = "123"
            };
            existing.SetDimensions(dimensions, "c.");

            // then
            var expected = new Dictionary<string, string?>
            {
                ["C.InterestingKey"] = "123"
            };
            existing.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void Should_Ignore_Prefix_Already_Included_With_Dimension()
        {
            // given
            var existing = new Dictionary<string, string?>();

            // when
            var dimensions = new Dictionary<string, object?>
            {
                ["SomePrefix.InterestingKey"] = "123",
                ["UnprefixedKey"] = "456"
            };
            existing.SetDimensions(dimensions, "SomePrefix.");

            // then
            var expected = new Dictionary<string, string?>
            {
                ["SomePrefix.InterestingKey"] = "123",
                ["SomePrefix.UnprefixedKey"] = "456"
            };
            existing.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void Should_Ignore_Empty_Prefix()
        {
            // given
            var existing = new Dictionary<string, string?>();

            // when
            var dimensions = new Dictionary<string, object?>
            {
                ["InterestingKey"] = "123"
            };
            existing.SetDimensions(dimensions, "");

            // then
            existing.Should().BeEquivalentTo(dimensions);
        }

        [Fact]
        public void Should_Collect_Dimensions_Using_Pascal_Case_Keys()
        {
            // given
            var existing = new Dictionary<string, string?>();

            // when
            var dimensions = new Dictionary<string, object?>
            {
                ["camelCase"] = "123"
            };
            existing.SetDimensions(dimensions);

            // then
            var expected = new Dictionary<string, string?>
            {
                ["CamelCase"] = "123"
            };
            existing.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void Should_Not_Collect_Dimension_With_Empty_Key()
        {
            // given
            var existing = new Dictionary<string, string?>
            {
                ["ExistingKey"] = "345"
            };

            // when
            var dimensions = new Dictionary<string, object?>
            {
                [""] = "123"
            };
            existing.SetDimensions(dimensions);

            // then
            existing.Should().BeSameAs(existing);
        }

        [Fact]
        public void Should_Collect_Dimension_With_Empty_Or_Null_Value()
        {
            // given
            var existing = new Dictionary<string, string?>();

            // when
            var dimensions = new Dictionary<string, object?>
            {
                ["EmptyValue"] = "",
                ["NullValue"] = null
            };
            existing.SetDimensions(dimensions);

            // then
            existing.Should().BeEquivalentTo(dimensions);
        }

        [Fact]
        public void Should_Overwrite_Existing_Dimension_With_Same_Key()
        {
            // given
            var existing = new Dictionary<string, string?>
            {
                ["ExistingKey"] = "345"
            };

            // when
            var dimensions = new Dictionary<string, object?>
            {
                ["ExistingKey"] = "123"
            };
            existing.SetDimensions(dimensions);

            // then
            existing.Should().BeEquivalentTo(dimensions);
        }
    }
}
