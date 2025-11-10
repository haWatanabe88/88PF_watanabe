// Player.cpp
#include "Player.h"
#include <iostream>

Player::Player()
        :moveSpeed(0.01f),
        :cleanSpeed(2.0f),
        :hp(100),
        :maxHp(100),
        :attackPower(10),
        :attackSpeed(2.0f),
        :revivalTime(5.0f);
{
    if (!texture.loadFromFile("../image/hermitcrab_spritesheet.png")) {
        std::cerr << "Failed to load player texture" << std::endl;
    }
    sprite.setTexture(texture);
    sprite.setTextureRect(sf::IntRect(0, 0, 235, 200));  // 仮のフレームサイズ
    sprite.setOrigin(235 / 2.0f, 200 / 2.0f);
    sprite.setScale(0.3f, 0.3f);
    sprite.setPosition(400, 300);
}

void Player::moveTowards(const sf::Vector2f& target, float deltaTime) {
    sf::Vector2f pos = sprite.getPosition();
    sf::Vector2f dir = target - pos;
    float length = std::sqrt(dir.x * dir.x + dir.y * dir.y);
    if (length > 0.1f) {
        dir /= length;
        sprite.move(dir * moveSpeed);
    }
    // 向きの設定
    sprite.setScale(dir.x < 0 ? -0.3f : 0.3f, 0.3f);
}

void Player::takeDamage(int damage) {
    hp -= damage;
    if (hp < 0) hp = 0;
}

void Player::heal() {
    hp = maxHp;
}

bool Player::isDead() const {
    return hp <= 0;
}

sf::Sprite& Player::getSprite() {
    return sprite;
}

int Player::getHp() const {
    return hp;
}

int Player::getMaxHp() const {
    return maxHp;
}

void Player::reset() {
    hp = maxHp;
    sprite.setPosition(400, 300);  // 初期位置に戻す（必要に応じて）
}
