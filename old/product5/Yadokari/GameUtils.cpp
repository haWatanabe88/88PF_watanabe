// GameUtils.cpp
#include "GameUtils.h"
#include <cstdlib>

void resetGameState(
    GameState& currentState,
    sf::Time& activeTime,
    sf::Clock& gameClock,
    sf::Clock& spawnClock,
    sf::Clock& enemySpawnClock,
    float& experience,
    int& upgradeCount,
    float& getexperience,
    float& pollution,
    PlayerStats& player,
    bool& isReviving,
    std::vector<Enemy>& enemies,
    std::vector<sf::Sprite>& trushSprites,
    int& collidedEnemyIndex,
    bool& enemyCollisionPending,
    bool& collisionPending,
    bool& imageLoaded,
    sf::Texture& trushTexture
) {
    currentState = GameState::Title;
    activeTime = sf::Time::Zero;
    gameClock.restart();
    spawnClock.restart();
    enemySpawnClock.restart();
    experience = 0.0f;
    upgradeCount = 0;
    getexperience = 50.0f;
    pollution = 0.0f;
    player.moveSpeed = 0.01f;
    player.cleanSpeed = 2.0f;
    player.hp = 100;
    player.maxHp = 100;
    player.attackPower = 10;
    player.attackSpeed = 2.0f;
    player.revivalTime = 5.0f;
    isReviving = false;
    enemies.clear();
    trushSprites.clear();
    collidedEnemyIndex = -1;
    enemyCollisionPending = false;
    collisionPending = false;
    imageLoaded = false;

    // 初期ゴミを1個だけ出しておく
    sf::Sprite ts;
    ts.setTexture(trushTexture);
    ts.setOrigin(ts.getLocalBounds().width / 2.0f, ts.getLocalBounds().height / 2.0f);
    ts.setScale(0.3f, 0.3f);
    ts.setPosition(static_cast<float>(std::rand() % 700 + 50), static_cast<float>(std::rand() % 500 + 50));
    trushSprites.push_back(ts);
}

// --- ほぼ等しいか判定するユーティリティ関数 ---
auto isNearlyEqual = [](float a, float b, float precision = 0.001f) {
    return std::abs(a - b) <= precision;
};