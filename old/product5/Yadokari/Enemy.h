// Enemy.h
#pragma once
#include <iostream>
#include <SFML/Graphics.hpp>

class Enemy{
	public:
		Enemy();

		void setPosition(float x, float y);
		sf::Vector2f getPosition() const;
		void moveTowards(const sf::Vector2f& target, float deltaTime);
		sf::Sprite& getSprite();
		void takeDamage(int damage);
		bool isDead() const;

		//パラメータ
		float moveSpeed;
		int hp;
		int attackPower;
		float attackSpeed;  // エネミーの攻撃間隔

	private:
		sf::Sprite sprite;
		sf::Texture texture;
		// --- enemySprite 用アニメーション設定 ---
		sf::Texture enemySheetTexture;
		const int enemyFrameCount = 4;
		const int enemyFrameWidth = 260;
		const int enemyFrameHeight = 250;
		int currentEnemyFrame = 0;
		sf::Clock enemyAnimationClock;
		float enemyAnimationSpeed = 0.2f; // 秒
};




