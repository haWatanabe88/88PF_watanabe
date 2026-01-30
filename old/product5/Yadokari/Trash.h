// Trash.h

#pragma once
#include <SFML/Graphics.hpp>


int collidedTrushIndex = -1;

    // --- ゴミ（trush）関連 ---
    std::vector<sf::Sprite> trushSprites;
    sf::Texture trushTexture;
    if (!trushTexture.loadFromFile("../image/trush.png")) {
        std::cerr << "Failed to load trush.jpg" << std::endl;
        return -1;
    }
    float trushSpawnInterval = 3.0f;///変更
    size_t maxTrushCount = 100;//変更
    sf::Clock trushSpawnClock;
    sf::Clock spawnClock;
    sf::Time spawnTimeAcc = sf::Time::Zero;
    sf::Sprite ts;
    ts.setTexture(trushTexture);
    sf::FloatRect tsBounds = ts.getLocalBounds();
    ts.setOrigin(ts.getLocalBounds().width / 2.0f, ts.getLocalBounds().height / 2.0f);
    ts.setScale(0.07f, 0.07f);
    float randX = static_cast<float>(std::rand() % 700 + 50);
    float randY = static_cast<float>(std::rand() % 500 + 50);
    ts.setPosition(randX, randY);
    trushSprites.push_back(ts);