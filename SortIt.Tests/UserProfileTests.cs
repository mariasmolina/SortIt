using SortIt.Models;

namespace SortIt.Tests
{
    /* Unit-testid UserProfile mudeli jaoks
    Testitakse vaikeväärtusi ja omaduste muutmist */
    public class UserProfileTests
    {
        // Kontrollib kasutajaprofiili vaikeväärtusi uue profiili loomisel
        [Fact]
        public void UserProfile_ShouldHaveDefaultValues()
        {
            // Arrange
            UserProfile profile = new UserProfile();

            // Assert
            Assert.Equal("Eco Hero", profile.Name);
            Assert.Equal("avatar_leaf.svg", profile.Avatar);
            Assert.Equal(0, profile.Xp);
            Assert.Equal(0, profile.TotalCorrect);
            Assert.Equal(0, profile.TotalWrong);
        }

        // Kontrollib, et kasutaja XP väärtust saab muuta
        [Fact]
        public void UserProfile_ShouldUpdateXp()
        {
            // Arrange
            UserProfile profile = new UserProfile();

            // Act
            profile.Xp = 150;

            // Assert
            Assert.Equal(150, profile.Xp);
        }
    }
}