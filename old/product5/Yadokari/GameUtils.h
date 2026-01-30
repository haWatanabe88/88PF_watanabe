// GameUtils.h
#pragma once
#include <SFML/Graphics.hpp>
#include <vector>
#include "Player.h"
#include "Enemy.h"
#include "GameState.h"  // enum GameState をここに分離しておくとスマート！
#include <cmath>

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
);


// --- ほぼ等しいか判定するユーティリティ関数 ---
// インライン関数として書くと安全！
inline bool isNearlyEqual(float a, float b, float precision = 0.001f) {
    return std::abs(a - b) <= precision;
}