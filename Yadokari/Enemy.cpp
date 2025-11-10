#include "Enemy.h"
#include <iostream>
#include <cmath>

Enemy::Enemy() {
    if (!texture.loadFromFile("../image/enemy1.png")) {
        std::cerr << "Failed to load enemy texture" << std::endl;
    }
    if (!enemySheetTexture.loadFromFile("../image/enemy1.png")) {
        std::cerr << "Failed to load enemy1.png" << std::endl;
        return -1;
    }
    sprite.setTexture(texture);
    sprite.setTextureRect(sf::IntRect(30, 380, 260, 250)); // 仮の範囲
    sprite.setOrigin(260 / 2.0f, 250 / 2.0f);
    sprite.setScale(0.3f, 0.3f);

    // 初期ステータス
    moveSpeed = 0.01f;
    hp = 50;
    attackPower = 10;
    attackSpeed = 1.0f;
}

void Enemy::setPosition(float x, float y) {
    sprite.setPosition(x, y);
}

sf::Vector2f Enemy::getPosition() const {
    return sprite.getPosition();
}

sf::Sprite& Enemy::getSprite() {
    return sprite;
}

void Enemy::moveTowards(const sf::Vector2f& target, float deltaTime) {
    sf::Vector2f pos = sprite.getPosition();
    sf::Vector2f dir = target - pos;
    float len = std::sqrt(dir.x * dir.x + dir.y * dir.y);
    if (len > 0.1f) {
        dir /= len;
        sprite.move(dir * moveSpeed);
    }
    sprite.setScale(dir.x < 0 ? -0.3f : 0.3f, 0.3f);
}

void Enemy::takeDamage(int damage) {
    hp -= damage;
    if (hp < 0) hp = 0;
}

bool Enemy::isDead() const {
    return hp <= 0;
}
