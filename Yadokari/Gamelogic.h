// Gamelogic.h
#pragma once
#include <SFML/Graphics.hpp>

	// --- タイマー ---
    sf::Text timerText;
    timerText.setFont(font);
    timerText.setCharacterSize(40);
    timerText.setFillColor(sf::Color::Red);
    timerText.setPosition(250, 20);
    sf::Clock gameClock;
    sf::Time activeTime = sf::Time::Zero;
    const sf::Time timeLimit = sf::seconds(300);//////////////////ゲーム時間

	float enemySpawnInterval = 5.0f;
    const size_t enemyMaxCount = 3;
    sf::Clock enemySpawnClock;
    bool enemyCollisionPending = false;
    sf::Clock enemyCollisionClock;
    int collidedEnemyIndex = -1;
    float enemyCollisionPauseTime = 0.5f;
    sf::Clock playerAttackClock;
    sf::Clock enemyAttackClock;