using FakeItEasy;
using FakeItEasy.ExtensionSyntax.Full;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace NS2.FeatureToggler.Test
{
    public class FeatureTest
    {
        //// Arrange Fakes for test -----------------------------------------------------------
        public class SomeFeature : IFeature { }
        public class SomeDynamicIsEnabledFeature : IFeature
        {
            public bool IsEnabled()
            {
                return true;
            }
        }
        public class SomeDynamicIsDisabledFeature : IFeature
        {
            public bool IsEnabled()
            {
                return false;
            }
        }
        //// --------------------------------------------------------------------------

        [Fact]
        public void When_Check_If_feature_is_enabled_in_config_expect_true()
        {
            ////Arrange
            var config = A.Fake<IConfigurationRoot>();
            config.CallsTo(x => x["SomeFeature"]).Returns("true");
            Feature.SetConfiguration(config);

            //Act
            var enabled = Feature<SomeFeature>.Is().Enabled;

            ////Test
            Assert.Equal(true, enabled);
        }

        [Fact]
        public void When_Check_If_feature_is_disabled_in_config_expect_true()
        {
            ////Arrange
            var config = A.Fake<IConfigurationRoot>();
            config.CallsTo(x => x["SomeFeature"]).Returns("false");
            Feature.SetConfiguration(config);

            //Act
            var disabled = Feature<SomeFeature>.Is().Disabled;

            ////Test
            Assert.Equal(true, disabled);
        }

        [Fact]
        public void When_Check_If_feature_is_enabled_with_no_config_settings_will_expect_false()
        {
            ////Arrange
            var config = A.Fake<IConfigurationRoot>();
            Feature.SetConfiguration(config);

            ////Act
            var enabled = Feature<SomeFeature>.Is().Enabled;

            ////Test
            Assert.Equal(false, enabled);
        }

        [Fact]
        public void When_Check_If_feature_is_disabled_with_no_config_settings_will_expect_disabled_as_true()
        {
            ////Arrange
            var config = A.Fake<IConfigurationRoot>();
            Feature.SetConfiguration(config);

            //Act
            var disabled = Feature<SomeFeature>.Is().Disabled;

            ////Test
            Assert.Equal(true, disabled);
        }

        [Fact]
        public void When_Check_If_feature_is_enabled_with_dynamic_IsEnabled_in_feature_Class_expect_true()
        {
            ////Act
            var enabled = Feature<SomeDynamicIsEnabledFeature>.Is().Enabled;

            ////Test
            Assert.Equal(true, enabled);
        }

        [Fact]
        public void When_Check_If_feature_is_disabled_with_dynamic_IsDisabled_in_feature_Class_expect_true()
        {
            ////Arrange & Act
            var disabled = Feature<SomeDynamicIsDisabledFeature>.Is().Disabled;

            ////Test
            Assert.Equal(true, disabled);
        }

        [Fact]
        public void When_Check_If_feature_is_enabled_with_no_dynamic_implemented_IsDisabled_in_feature_Class_expect_false()
        {
            ////Arrange & Act
            var enabled = Feature<SomeFeature>.Is().Enabled;

            ////Test
            Assert.Equal(false, enabled);
        }
    }
}
