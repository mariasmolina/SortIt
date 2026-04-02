using SortIt.Resources.Localization;

namespace SortIt.Services
{
    public class WasteCategoryMapper
    {
        // Määritleb konteinerite ressursivõtmed ja nendega seotud märksõnad
        private static readonly Dictionary<string, string[]> ContainerKeywords = new()
        {
            { nameof(AppResources.PlasticMetalContainer), new[]
                {
                    "plastic", "bottle", "pet", "container", "packaging",
                    "cup", "wrapper", "bag", "sachet", "foil pouch",
                    "detergent bottle", "shampoo bottle", "water bottle",
                    "metal", "can", "tin", "aluminum", "steel", "foil",
                    "drink", "beverage", "soda", "juice",
                    "yogurt", "milk carton", "food packaging",
                    "snack", "chips", "candy wrapper", "plastic bag",
                    "spray bottle", "cosmetic bottle", "lotion bottle",
                    "plastic lid", "metal lid", "capsule", "tube"
                }
            },
            { nameof(AppResources.GlassPackagingContainer), new[]
                {
                    "glass", "glass bottle", "wine bottle",
                    "jar", "glass jar", "beer bottle",
                    "drink bottle", "glass container",
                    "perfume bottle", "cosmetic jar",
                    "glass cup", "broken glass"
                }
            },
            { nameof(AppResources.PaperCardboardContainer), new[]
                {
                    "paper", "cardboard", "carton", "box",
                    "newspaper", "magazine", "book",
                    "paperboard", "notebook", "document",
                    "office paper", "paper bag", "mail",
                    "envelope", "pizza box",
                    "egg carton", "paper packaging",
                    "flyer", "poster", "receipt"
                }
            },
            { nameof(AppResources.BioWasteContainer), new[]
                {
                    "food", "fruit", "banana", "apple",
                    "vegetable", "organic", "bread",
                    "meal", "leftovers", "peel",
                    "coffee", "tea", "egg",
                    "meat", "fish", "salad",
                    "food waste", "kitchen waste",
                    "bones", "rice", "pasta",
                    "cheese", "cake", "cookie"
                }
            },
            { nameof(AppResources.DepositPackagingContainer), new[]
                {
                    "deposit bottle", "return bottle",
                    "deposit", "pant bottle",
                    "reusable bottle", "refund bottle",
                    "coca cola bottle", "beer can",
                    "plastic bottle", "drink can"
                }
            },
            { nameof(AppResources.HazardousWasteContainer), new[]
                {
                    "battery", "bulb", "electronics",
                    "paint", "chemical", "medicine",
                    "toxic", "hazardous",
                    "phone battery", "light bulb",
                    "charger", "power adapter",
                    "pills", "tablets", "blister",
                    "syringe", "thermometer",
                    "drug", "vitamin", "antibiotic",
                    "ointment", "cream tube"
                }
            },
            { nameof(AppResources.ElectronicsContainer), new[]
                {
                    "phone", "mobile phone", "smartphone",
                    "computer", "laptop", "keyboard",
                    "mouse", "monitor", "screen",
                    "tv", "television", "printer",
                    "electronics", "device",
                    "charger", "cable", "headphones",
                    "tablet", "camera", "remote control",
                    "game console"
                }
            },
            { nameof(AppResources.ClothesShoesContainer), new[]
                {
                    "clothes", "clothing", "shirt",
                    "t-shirt", "pants", "jeans",
                    "jacket", "coat", "sweater",
                    "dress", "fabric", "textile",
                    "shoes", "sneakers", "boots",
                    "footwear", "sock", "hat",
                    "gloves", "scarf", "bag"
                }
            },
            { nameof(AppResources.MixedWasteContainer), new[]
                {
                    "trash", "waste", "garbage",
                    "dirty", "broken", "old",
                    "used", "unknown",
                    "mixed waste", "junk",
                    "diaper", "toothbrush",
                    "ceramics", "porcelain",
                    "mirror", "vacuum bag"
                }
            },
            { nameof(AppResources.NotWaste), new[]
                {
                    "person", "man", "woman", "child",
                    "face", "hand", "cat", "dog", "animal",
                    "car", "bicycle", "motorcycle",
                    "tree","house", "building", "room"
                }
            }
        };

        // Kaardistab tuvastatud sildi konteineri ressursivõtme järgi
        public static string MapLabelToContainerKey(string label)
        {
            if (string.IsNullOrWhiteSpace(label))
                return nameof(AppResources.MixedWasteContainer);

            string lowerLabel = label.ToLower();

            foreach (var container in ContainerKeywords)
            {
                foreach (var keyword in container.Value)
                {
                    if (lowerLabel.Contains(keyword))
                        return container.Key;
                }
            }

            return nameof(AppResources.MixedWasteContainer);
        }
    }
}
