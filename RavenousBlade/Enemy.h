#pragma once
#include "Character.h"
#include "ScoreManager.h"
#include "ComboManager.h"
#include "SPGauge.h"
#include "SPManager.h"

class EnemyManager;

class Enemy : public Character{
	public:
		Enemy(Direction dir){
			direction = dir;
			characterSprite.setOrigin(
					characterSprite.getGlobalBounds().width /2.0f,
					characterSprite.getGlobalBounds().height /2.0f
			);
		};
		enum class AreaType{
			None,
			Big,
			Small,
		};
		AreaType areaType = AreaType::None;
		virtual ~Enemy() = default;
		virtual void setEnemyTexture(Enemy&, EnemyManager&) = 0;
		double getMoveSpeed();
		int getScore();//スコア
		float getFadeSpeed(){return fadeSpeed;};
		void setMoveSpeed(double);
		void setScore(int);//コンボ段階による変化
		void setEnemyPos(double, sf::RenderWindow&);
		void moveEnemy(double);
		bool fadeEnemySprite(float, SPManager&);
		void defeated(SPManager&);
	protected:
		float fadeSpeed = 0.02f;//敵撃破時に透過する処理（か、0.03ぐらい）
		double moveSpeed;
		int score;
		sf::Clock spawnClock;
		sf::Clock fadeClock;
		sf::Vector2f startPos;
};
