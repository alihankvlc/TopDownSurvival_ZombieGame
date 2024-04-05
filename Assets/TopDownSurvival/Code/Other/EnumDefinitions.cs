using System.ComponentModel;
using UnityEngine;

public enum PoolOperation
{
    GetObject,
    ReturnObject
}

public enum WaveState
{
    Spawning,

    [Description("Waiting for the next wave")] WaitingForNextWave,

    [Description("Wave completed")] Completed
}

public enum EffectType
{
    Score
}

public enum QuestType
{
    // Düşmanları öldürme görevi
    KillEnemies,

    // Eşyaları toplama görevi
    CollectItems,

    // Belirli bir alanı keşfetme görevi
    ExploreArea,

    // Belirli bir nesneyi teslim etme görevi
    DeliverItem,

    // Kayıp NPC'yi kurtarma görevi
    RescueNPC,

    // NPC'yi bir noktadan bir noktaya eşlik etme görevi
    EscortNPC,

    // Bulmaca çözme görevi
    SolvePuzzle,

    // Belirli bir hedefe ulaşma görevi
    ReachDestination,

    // Belirli bir konumu savunma görevi
    DefendLocation,

    // Bir eşya yapma görevi
    CraftItem,

    // Kaynakları toplama görevi
    GatherResources,

    // NPC ile etkileşim kurma görevi
    InteractWithNPC,

    // Gizli bir şey keşfetme görevi
    DiscoverSecret,

    // Bir zorluğu tamamlama görevi
    CompleteChallenge
}

public enum DropType
{
    Health,
    Ammo,
    Gold,
}

public enum ZombieType
{
    Infected,
    Boss
}

public enum RewardType
{
    Exp,
    Gold,
}

public enum StackType
{
    Increase,
    Decrease
}

public enum ItemType
{
    Weapon,
    Consumable,
    Food,
    Drink,
    Resources
}

public enum LootState
{
    NotLootable,
    WaitingForLoot,
    Looting,
    Looted
}

public enum LootRichness
{
    Poor = 1, // Yetersiz
    Normal = 3, // Normal
    Rich = 5, // Zengin
    VeryRich = 8, // Çok Zengin
}

public enum LootType
{
    Weapon,
    Ammo,
    Book,
    Consumable,
    Resources,
    Random,
    Health
}

public enum SlotType
{
    Inventory,
    Loot
}

public enum StatType
{
    None,
    Health,
    Thirst,
    Hunger,
    Radiation,
    Experience
}