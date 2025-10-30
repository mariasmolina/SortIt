using SortIt.Resources.Localization;

namespace SortIt.Services
{
    public class LevelService
    {
        // уровень по опыту
        public static int GetLevel(int xp) => Math.Max(1, (xp / 100) + 1);

        // сколько нужно XP до следующего уровня
        public static int NextLevelXp(int xp)
        {
            int level = GetLevel(xp);
            return level * 100;
        }

        // прогресс в процентах (для ProgressBar)
        public static double GetProgress(int xp)
        {
            int level = GetLevel(xp);
            int xpForThisLevel = (level - 1) * 100;   // сколько нужно для начала уровня
            int xpForNextLevel = level * 100;         // сколько нужно для следующего
            int xpInLevel = xp - xpForThisLevel;      // сколько уже набрано в этом уровне
            int xpNeeded = xpForNextLevel - xpForThisLevel; // сколько всего нужно

            double progress = (double)xpInLevel / xpNeeded;

            if (progress < 0) progress = 0;
            if (progress > 1) progress = 1;

            return progress;
        }

        // ранг по уровню
        public static string GetRank(int level) => level switch
        {
            <= 2 => AppResources.LabelRank_Seedling,
            <= 4 => AppResources.LabelRank_Recycler,
            <= 6 => AppResources.LabelRank_EcoScout,
            <= 8 => AppResources.LabelRank_GreenHero,
            _ => AppResources.LabelRank_PlanetGuardian
        };

        // изображение дерева по рангу
        public static string GetRankImage(int level)
        {
            if (level <= 2)
                return "plant_rank0_seedling.png"; 

            if (level <= 4)
                return "plant_rank1_sapling.png";

            if (level <= 6)
                return "plant_rank2_bush.png"; 

            if (level <= 8)
                return "plant_rank3_tree.png";

            return "plant_rank4_forest.png";  
        }
    }
}