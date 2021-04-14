using System.Collections.Generic;
using ArtUtils.Net.Core.Attributes;
using NUnit.Framework;

// ReSharper disable UnusedMember.Global

namespace ArtUtils.Net.Core.Unit.Tests
{
    public class ValidatorTests
    {
        #region Test classes
        [TableName("Well, this is the name")]
        internal class SampleClassWithValidTableName
        {
        }

        [TableName("")]
        internal class SampleClassWithoutValidTableName
        {
        }

        internal class SampleClassWithoutKeyField
        {
            public int Id { get; set; }
        }

        internal class SampleClassWithEmptyKeyField
        {
            public int Id2 { get; set; }

            [KeyField("")]
            public int Id { get; set; }
        }

        internal class SampleClassWithProperKeyField
        {
            public string SomeField { get; set; }
            [KeyField("And this is a key indicator")]
            public int Id { get; set; }

            public string SomeField2 { get; set; }
        }
        #endregion

        [Test]
        public void VerifyTableNameAttributeWhenTableNameIsNotPresent()
        {
            var sample = new List<string>();

            var result = Validator.VerifyTableName(sample);

            Assert.IsFalse(result.Valid, "TableName attribute is not present but false positive occurred");
            Assert.AreNotEqual(0, result.Errors.Count);
        }

        [Test]
        public void VerifyTableNameAttributeWhenTableNameIsPresentAndNotEmpty()
        {
            var sample = new SampleClassWithValidTableName();

            var result = Validator.VerifyTableName(sample);

            Assert.IsTrue(result.Valid, "TableName attribute is present but negative validation occurred");
            Assert.AreEqual(0, result.Errors.Count);
        }

        [Test]
        public void VerifyTableNameAttributeWhenTableNameIsPresentAndEmpty()
        {
            var sample = new SampleClassWithoutValidTableName();

            var result = Validator.VerifyTableName(sample);

            Assert.IsFalse(result.Valid, "Empty TableName attribute is present but false positive validation occurred");
            Assert.AreNotEqual(0, result.Errors.Count);
        }

        [Test]
        public void VerifyKeyFieldAttributeWhenKeyFieldIsNotPresent()
        {
            var sample = new SampleClassWithoutKeyField();

            var result = Validator.VerifyAttributeOnPropertiesPresent<KeyField>(sample, "TEST");

            Assert.IsFalse(result.Valid, "KeyField attribute is not present but false positive validation occurred");
            Assert.AreNotEqual(0, result.Errors.Count);
        }

        [Test]
        public void VerifyKeyFieldAttributeWhenKeyFieldIsPresentButEmpty()
        {
            var sample = new SampleClassWithEmptyKeyField();

            var result = Validator.VerifyAttributeOnPropertiesPresent<KeyField>(sample, "TEST");

            Assert.IsFalse(result.Valid, "KeyField attribute is not present but false positive validation occurred");
            Assert.AreNotEqual(0, result.Errors.Count);
        }

        [Test]
        public void VerifyKeyFieldAttributeWhenValidKeyFieldIsPresent()
        {
            var sample = new SampleClassWithProperKeyField();

            var result = Validator.VerifyAttributeOnPropertiesPresent<KeyField>(sample, "TEST");

            Assert.IsTrue(result.Valid, "KeyField attribute is not present but false positive validation occurred");
            Assert.AreEqual(0, result.Errors.Count);
        }
    }
}
