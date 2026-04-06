using SortIt.Services;

namespace SortIt.Tests
{
    /* Unit-testid WasteCategoryMapper klassi jaoks
    Testitakse jäätmeliikide määramist erinevate sisendite korral */
    public class WasteCategoryMapperTests
    {
        // Kontrollib, et "banana peel" liigitatakse BioWasteContainer kategooriasse
        [Fact]
        public void MapLabelToContainerKey_ShouldReturnBioWaste_WhenLabelIsBananaPeel()
        {
            // Arrange
            string label = "banana peel";

            // Act
            string result = WasteCategoryMapper.MapLabelToContainerKey(label);

            // Assert
            Assert.Equal("BioWasteContainer", result);
        }

        // Kontrollib, et tundmatu ese liigitatakse MixedWasteContainer kategooriasse
        [Fact]
        public void MapLabelToContainerKey_ShouldReturnMixedWaste_WhenLabelIsUnknown()
        {
            // Arrange
            string label = "unknown item";

            // Act
            string result = WasteCategoryMapper.MapLabelToContainerKey(label);

            // Assert
            Assert.Equal("MixedWasteContainer", result);
        }

        // Kontrollib, et meetod ei sõltu tähtede suurusest (case insensitive)
        [Fact]
        public void MapLabelToContainerKey_ShouldIgnoreCase()
        {
            // Arrange
            string label = "BaNaNa PeEl";

            // Act
            string result = WasteCategoryMapper.MapLabelToContainerKey(label);

            // Assert
            Assert.Equal("BioWasteContainer", result);
        }

        // Kontrollib, et tühi string tagastab MixedWasteContainer
        [Fact]
        public void MapLabelToContainerKey_ShouldReturnMixedWaste_WhenLabelIsEmpty()
        {
            // Arrange
            string label = "";

            // Act
            string result = WasteCategoryMapper.MapLabelToContainerKey(label);

            // Assert
            Assert.Equal("MixedWasteContainer", result);
        }

        // Kontrollib, et null väärtus tagastab MixedWasteContainer
        [Fact]
        public void MapLabelToContainerKey_ShouldReturnMixedWaste_WhenLabelIsNull()
        {
            // Arrange
            string label = null;

            // Act
            string result = WasteCategoryMapper.MapLabelToContainerKey(label);

            // Assert
            Assert.Equal("MixedWasteContainer", result);
        }
    }
}