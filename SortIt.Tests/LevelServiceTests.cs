using SortIt.Services;

namespace SortIt.Tests
{
    /* Unit-testid LevelService klassi jaoks
    Testitakse taseme arvutamist, progressi ja järgmise taseme XP väärtust */
    public class LevelServiceTests
    {
        // Kontrollib, et kui XP = 0, siis tase on 1
        [Fact]
        public void GetLevel_ShouldReturn1_WhenXpIs0()
        {
            // Arrange
            int xp = 0;

            // Act
            int result = LevelService.GetLevel(xp);

            // Assert
            Assert.Equal(1, result);
        }

        // Kontrollib, et kui XP = 100, siis tase on 2
        [Fact]
        public void GetLevel_ShouldReturn2_WhenXpIs100()
        {
            // Arrange
            int xp = 100;

            // Act
            int result = LevelService.GetLevel(xp);

            // Assert
            Assert.Equal(2, result);
        }

        // Kontrollib järgmise taseme XP väärtust kui XP = 100
        [Fact]
        public void NextLevelXp_ShouldReturn200_WhenXpIs100()
        {
            // Arrange
            int xp = 100;

            // Act
            int result = LevelService.NextLevelXp(xp);

            // Assert
            Assert.Equal(200, result);
        }

        // Kontrollib, et progress on 0.5 kui XP = 50
        [Fact]
        public void GetProgress_ShouldReturnHalf_WhenXpIs50()
        {
            // Arrange
            int xp = 50;

            // Act
            double result = LevelService.GetProgress(xp);

            // Assert
            Assert.Equal(0.5, result, 1);
        }

        // Kontrollib, et taseme 1 jaoks tagastatakse õige pilt
        [Fact]
        public void GetRankImage_ShouldReturnSeedling_WhenLevelIs1()
        {
            // Arrange
            int level = 1;

            // Act
            string result = LevelService.GetRankImage(level);

            // Assert
            Assert.Equal("plant_rank0_seedling.svg", result);
        }

        // Kontrollib, et negatiivse XP korral jääb tase 1
        [Fact]
        public void GetLevel_ShouldReturn1_WhenXpIsNegative()
        {
            // Arrange
            int xp = -50;

            // Act
            int result = LevelService.GetLevel(xp);

            // Assert
            Assert.Equal(1, result);
        }

        // Kontrollib progressi väärtust kui XP on peaaegu järgmise tasemeni (XP = 99)
        [Fact]
        public void GetProgress_ShouldReturnAlmostOne_WhenXpIs99()
        {
            // Arrange
            int xp = 99;

            // Act
            double result = LevelService.GetProgress(xp);

            // Assert
            Assert.Equal(0.99, result, 2);
        }

        // Kontrollib, et XP = 200 korral on tase 3
        [Fact]
        public void GetLevel_ShouldReturn3_WhenXpIs200()
        {
            // Arrange
            int xp = 200;

            // Act
            int result = LevelService.GetLevel(xp);

            // Assert
            Assert.Equal(3, result);
        }
    }
}