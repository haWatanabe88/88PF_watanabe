// Player.h
#pragma once
#include <SFML/Graphics.hpp>

class Player {
public:
    Player();

    void moveTowards(const sf::Vector2f& target, float deltaTime);
    void takeDamage(int damage);
    void heal();
    void update(float deltaTime);
    void reset();

    sf::Sprite& getSprite();
    int getHp() const;
    int getMaxHp() const;
    bool isDead() const;

    // パラメータ
    float moveSpeed;
    float cleanSpeed;
    int hp;
    int maxHp;
    int attackPower;
    float attackSpeed;
    float revivalTime;
	// --- 経験値 ---
	float experience = 0.0f;
	float getexperience = 50.0f;
	float getexperiencedefeatenemy = 30.0f;
	int upgradeCount = 0;  // アップグレードされた回数を記録する
    // --- 衝突（掃除）遅延処理 ---
    bool collisionPending = false;
    sf::Clock collisionClock;
    // 復活処理管理用
    bool isReviving = false;
    sf::Clock revivalClock;

private:
    sf::Sprite sprite;
    sf::Texture texture;
	sf::Texture hermitCrabTexture;
	const int frameWidth = 235;//64_64
	const int frameHeight = 200;
	const int numFrames = 4;
	int currentFrame = 0;
	float animationSpeed = 0.2f;  // アニメーションスピード（秒）
	sf::Clock animationClock;
	sf::Sprite girlSprite;
	bool imageLoaded = false;
};

