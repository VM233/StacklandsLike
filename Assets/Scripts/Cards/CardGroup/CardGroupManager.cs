using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Sirenix.OdinInspector;
using StackLandsLike.GameCore;
using UnityEngine;
using VMFramework.Containers;
using VMFramework.Core;
using VMFramework.Core.Pool;
using VMFramework.GameLogicArchitecture;
using VMFramework.OdinExtensions;
using VMFramework.Procedure;

namespace StackLandsLike.Cards
{
    [ManagerCreationProvider(nameof(GameManagerType.Card))]
    public sealed class CardGroupManager : ManagerBehaviour<CardGroupManager>
    {
        private static readonly StackComponentPool<CardGroup> cache = new(() =>
        {
            var cardGroupObject = Instantiate(GameSetting.cardGeneralSetting.cardGroupPrefab);
            var cardGroup = cardGroupObject.GetComponent<CardGroup>();
            cardGroup.Init();
            return cardGroup;
        }, onGetCallback: cardGroup =>
        {
            cardGroup.SetActive(true);
            cardGroup.transform.SetParent(null);
        },onReturnCallback: cardGroup =>
        {
            cardGroup.SetActive(false);
            cardGroup.transform.SetParent(instance.cardGroupCacheContainer);
        });
        
        private static readonly HashSet<CardGroup> activeCardGroups = new();

        public static event Action<CardGroup> OnCardGroupCreated;
        public static event Action<CardGroup> OnCardGroupDestroyed;

        [SerializeField]
        private Transform cardGroupCacheContainer;

        /// <summary>
        /// Creates a new card group with a single card.
        /// </summary>
        /// <returns></returns>
        [Button]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static CardGroup CreateCardGroup([GamePrefabID(typeof(ICardConfig))] string cardID,
            Vector2 position = default)
        {
            var card = IGameItem.Create<ICard>(cardID);

            return CreateCardGroup(card);
        }

        /// <summary>
        /// Creates a new card group with a single card.
        /// </summary>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static CardGroup CreateCardGroup(ICard card, Vector2 position = default)
        {
            card.AssertIsNotNull(nameof(card));
            
            var cardGroup = cache.Get();

            if (cardGroup.cardContainer.TryAddItem(card) == false)
            {
                throw new InvalidOperationException("Failed to add card to card group.");
            }
            
            activeCardGroups.Add(cardGroup);

            cardGroup.SetPosition(position);
            
            OnCardGroupCreated?.Invoke(cardGroup);
            
            return cardGroup;
        }
        
        /// <summary>
        /// Destroys a card group and returns it to the cache.
        /// Cards in the card group are also removed.
        /// </summary>
        /// <param name="cardGroup"></param>
        [Button]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void DestroyCardGroup(CardGroup cardGroup)
        {
            if (activeCardGroups.Contains(cardGroup) == false)
            {
                return;
            }
            
            activeCardGroups.Remove(cardGroup);
            
            OnCardGroupDestroyed?.Invoke(cardGroup);
            
            foreach (var card in cardGroup.cards.ToList())
            {
                if (cardGroup.cardContainer.TryRemoveItem(card) == false)
                {
                    throw new InvalidOperationException(
                        $"Failed to remove {card} from card group :{cardGroup.name}.");
                }
                
                IGameItem.Destroy(card);
            }
            
            cache.Return(cardGroup);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IReadOnlyCollection<CardGroup> GetActiveCardGroups()
        {
            return activeCardGroups;
        }
    }
}