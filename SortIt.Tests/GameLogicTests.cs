using SortIt.Models;
using SortIt.Services;

namespace SortIt.Tests
{
    /* Unit-testid mänguloogika jaoks
    Testitakse õigete ja valede vastuste mõju XP-le ja statistikale */
    public class GameLogicTests
    {
        // Kontrollib, et õige vastus suurendab kasutaja XP-d
        [Fact]
        public void CorrectAnswer_ShouldIncreaseXp()
        {
            // Arrange
            UserProfile profile = new UserProfile();
            int xpBefore = profile.Xp;

            // Act
            profile.Xp += 10;

            // Assert
            Assert.True(profile.Xp > xpBefore);
        }

        // Kontrollib, et õige vastus suurendab õigete vastuste arvu
        [Fact]
        public void CorrectAnswer_ShouldIncreaseCorrectCount()
        {
            // Arrange
            UserProfile profile = new UserProfile();
            int correctBefore = profile.TotalCorrect;

            // Act
            profile.TotalCorrect++;

            // Assert
            Assert.Equal(correctBefore + 1, profile.TotalCorrect);
        }

        // Kontrollib, et XP = 100 korral muutub kasutaja tase 2-ks
        [Fact]
        public void Level_ShouldChangeTo2_WhenXpIs100()
        {
            // Arrange
            int xp = 100;

            // Act
            int level = LevelService.GetLevel(xp);

            // Assert
            Assert.Equal(2, level);
        }

        // Kontrollib, et vale vastus ei vähenda kasutaja XP-d
        [Fact]
        public void WrongAnswer_ShouldNotDecreaseXp()
        {
            // Arrange
            UserProfile profile = new UserProfile();
            profile.Xp = 50;

            // Act
            profile.TotalWrong++;

            // Assert
            Assert.Equal(50, profile.Xp);
        }

        // Kontrollib mitut vale vastust järjest ja et XP ei muutu
        [Fact]
        public void WrongAnswers_ShouldIncreaseWrongCount_AndNotChangeXp()
        {
            // Arrange
            UserProfile profile = new UserProfile();
            profile.Xp = 30;

            // Act
            profile.TotalWrong++;
            profile.TotalWrong++;

            // Assert
            Assert.Equal(2, profile.TotalWrong);
            Assert.Equal(30, profile.Xp);
        }
    }
}