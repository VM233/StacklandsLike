using System;
using StackLandsLike.Cards;
using VMFramework.GameLogicArchitecture;
using VMFramework.Property;

namespace StackLandsLike.GameCore
{
    [GamePrefabTypeAutoRegister(ID)]
    public sealed class CardNutritionProperty : PropertyConfig
    {
        public const string ID = "card_nutrition_property";

        public override Type targetType => typeof(IFoodCard);

        public override string GetValueString(object target)
        {
            var foodCard = (IFoodCard)target;
            return foodCard.nutrition.ToString();
        }
    }
}