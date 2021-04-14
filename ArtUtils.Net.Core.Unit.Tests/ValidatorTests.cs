using System.Collections.Generic;
using ArtUtils.Net.Core.Attributes;
using NUnit.Framework;

namespace ArtUtils.Net.Core.Unit.Tests
{
    public class ValidatorTests
    {
        [TableName("Well, this is the name")]
        internal class SampleClassWithValidTableName
        {

        }

        [TableName("")]
        internal class SampleClassWithoutValidTableName
        {

        }

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
    }
}
