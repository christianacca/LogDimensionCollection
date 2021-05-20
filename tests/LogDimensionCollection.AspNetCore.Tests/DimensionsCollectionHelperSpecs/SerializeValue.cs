using System;
using System.Collections.Generic;
using System.Text.Json;
using CcAcca.LogDimensionCollection.AspNetCore;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Xunit;
using SystemJsonSerializer = System.Text.Json.JsonSerializer;

namespace Specs.DimensionsCollectionHelperSpecs
{
    public class SerializeValue
    {
        [Fact]
        public void String_value()
        {
            DimensionsCollectionHelper.SerializeValue("value").Should().Be("value");
        }

        [Fact]
        public void Int32_value()
        {
            DimensionsCollectionHelper.SerializeValue(10).Should().Be("10");
        }

        [Fact]
        public void Nullable_Int32_value()
        {
            int? value = 10;
            DimensionsCollectionHelper.SerializeValue(value).Should().Be("10");
        }

        [Fact]
        public void Nullable_Int32_null_value()
        {
            int? value = null;
            // ReSharper disable once ExpressionIsAlwaysNull
            DimensionsCollectionHelper.SerializeValue(value).Should().BeNull();
        }

        [Fact]
        public void Int64_value()
        {
            const long value = 10;
            DimensionsCollectionHelper.SerializeValue(value).Should().Be("10");
        }

        [Fact]
        public void DateTime_default_kind_value()
        {
            var dtm = new DateTime(1999, 12, 31, 23, 59, 59);
            DimensionsCollectionHelper.SerializeValue(dtm).Should().Be("1999-12-31T23:59:59.0000000");
        }

        [Fact]
        public void DateTime_utc_value()
        {
            var dtm = new DateTime(1999, 12, 31, 23, 59, 59, DateTimeKind.Utc);
            DimensionsCollectionHelper.SerializeValue(dtm).Should().Be("1999-12-31T23:59:59.0000000Z");
        }


        [Fact]
        public void DateTimeOffset_value()
        {
            var dtm = new DateTimeOffset(new DateTime(1999, 12, 31, 23, 59, 59));
            DimensionsCollectionHelper.SerializeValue(dtm).Should().Be("1999-12-31T23:59:59.0000000+00:00");
        }


        [Fact]
        public void Dictionary_value()
        {
            var value = new Dictionary<string, int>
            {
                { "key1", 1 },
                { "key2", 2 }
            };
            const string expected = "{\"key1\":1,\"key2\":2}";
            DimensionsCollectionHelper.SerializeValue(value).Should().Be(expected);
        }

        [Fact]
        public void Custom_object_value()
        {
            var value = new CustomObject
            {
                Prop1 = "one",
                Prop2 = 2
            };
            const string expected = "{\"Prop1\":\"one\",\"Prop2\":2}";
            DimensionsCollectionHelper.SerializeValue(value).Should().Be(expected);
        }

        [Fact]
        public void Problem_Details_With_Extensions()
        {
            var value = new ProblemDetails
            {
                Extensions =
                {
                    { "Prop1", "one" },
                    { "Prop2", 2 }
                }
            };
            const string expected = "{\"Prop1\":\"one\",\"Prop2\":2}";
            DimensionsCollectionHelper.SerializeValue(value).Should().Be(expected);
        }

        [Fact]
        public void Custom_Problem_Details_With_Extensions()
        {
            var value = new CustomProblemDetails
            {
                Prop1 = "one",
                Prop2 = 2,
                Extensions =
                {
                    { "Prop3", "three" },
                    { "Prop4", 4 }
                }
            };
            const string expected = "{\"Prop1\":\"one\",\"Prop2\":2,\"Prop3\":\"three\",\"Prop4\":4}";
            DimensionsCollectionHelper.SerializeValue(value).Should().Be(expected);
        }

        [Fact]
        public void Custom_struct_value()
        {
            var value = new CustomStruct
            {
                Prop1 = "one",
                Prop2 = 2
            };
            const string expected = "{\"Prop1\":\"one\",\"Prop2\":2}";
            DimensionsCollectionHelper.SerializeValue(value).Should().Be(expected);
        }

        [Fact]
        public void JObject_value()
        {
            const string json = "{\"Prop1\":\"one\",\"Prop2\":2}";
            var jo = JsonConvert.DeserializeObject<JObject>(json);
            DimensionsCollectionHelper.SerializeValue(jo).Should().Be(json);
        }

        [Fact]
        public void JArray_value()
        {
            const string json = "[\"one\",\"two\"]";
            var ja = JsonConvert.DeserializeObject<JArray>(json);
            DimensionsCollectionHelper.SerializeValue(ja).Should().Be(json);
        }

        [Fact]
        public void JsonElement_Object_value()
        {
            const string json = "{\"Prop1\":\"one\",\"Prop2\":2}";
            var je = SystemJsonSerializer.Deserialize<JsonElement>(json);
            DimensionsCollectionHelper.SerializeValue(je).Should().Be(json);
        }

        [Fact]
        public void JsonElement_Array_value()
        {
            const string json = "[\"one\",\"two\"]";
            var je = SystemJsonSerializer.Deserialize<JsonElement>(json);
            DimensionsCollectionHelper.SerializeValue(je).Should().Be(json);
        }


        [Fact]
        public void On_json_serialization_exception_should_return_null()
        {
            var value = new CircularCustomObject
            {
                Prop1 = "one"
            };
            value.Self = value;

            DimensionsCollectionHelper.SerializeValue(value).Should().BeNull();
        }


        public class CustomObject
        {
            public string Prop1 { get; set; }
            public int Prop2 { get; set; }
        }

        public class CustomProblemDetails : ProblemDetails
        {
            public string Prop1 { get; set; }
            public int Prop2 { get; set; }
        }

        public class CircularCustomObject
        {
            public string Prop1 { get; set; }

            public CircularCustomObject Self { get; set; }
        }

        public struct CustomStruct
        {
            public string Prop1 { get; set; }
            public int Prop2 { get; set; }
        }
    }
}
