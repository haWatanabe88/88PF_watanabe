#include "Enemy.h"
#include "EnemyManager.h"
#include <iostream>

double Enemy::getMoveSpeed(){
	return moveSpeed;
}

void Enemy::setMoveSpeed(double newMoveSpeed){
	moveSpeed = newMoveSpeed;
}

int Enemy::getScore(){
	return score;
}

void Enemy::setScore(int newScore){
	score = newScore;
}

void Enemy::setEnemyPos(double moveSpeed, sf::RenderWindow& window){
	characterSprite.setOrigin(
			characterSprite.getGlobalBounds().width /2.0f,
			characterSprite.getGlobalBounds().height /2.0f
	);
	this->setScale(0.1);
	sf::Vector2f centerPos = getCenterPos(window);
	if(direction == Direction::up){
		characterSprite.setPosition(centerPos.x, 0);
	}
	else if(direction == Direction::left){
		characterSprite.setPosition(0, centerPos.y);
	}
	else if(direction == Direction::down){
		characterSprite.setPosition(centerPos.x, centerPos.y * 2);
	}
	else if(direction == Direction::right){
		characterSprite.setPosition(centerPos.x * 2, centerPos.y);
	}
	startPos = characterSprite.getPosition();
	spawnClock.restart();
}

void Enemy::moveEnemy(double moveSpeed){
    float elapsed = spawnClock.getElapsedTime().asSeconds();
	sf::Vector2f offset;
	if(!this->isDead){
		if(direction == Direction::up){
			offset = sf::Vector2f(0.0f, moveSpeed * elapsed);
		}
		else if(direction == Direction::left){
			offset = sf::Vector2f(moveSpeed * elapsed, 0.0f);
		}
		else if(direction == Direction::down){
			offset = sf::Vector2f(0.0f, -moveSpeed * elapsed);
		}
		else if(direction == Direction::right){
			offset = sf::Vector2f(-moveSpeed * elapsed, 0.0f);
		}
		characterSprite.setPosition(startPos + offset);
	}
}

void Enemy::defeated(SPManager& spm){
	isDead = true;
	if(this->areaType == Enemy::AreaType::Small){
		if(spm.getCurrentState() == SPManager::SPState::Idle){
			SPGauge::addGaugeProgress();
		}
		ComboManager::addComboCounter();
	}else if(this->areaType == Enemy::AreaType::Big){
		ComboManager::resetComboCount();
	}
	ScoreManager::addScore(this->score * ComboManager::getComboMultiplier());
}

bool Enemy::fadeEnemySprite(float fadeSpeed, SPManager& spm){
	float deltaTime = fadeClock.restart().asSeconds();
	sf::Color spriteColor = this->characterSprite.getColor();
	if(!this->isDead)
		return false;
	if(spm.getCurrentState() != SPManager::SPState::Idle)
		return false;
	if(this->characterSprite.getColor().a > 0){
		int newAlpha = static_cast<int>(spriteColor.a - fadeSpeed * deltaTime);
		if(newAlpha < 0){
			newAlpha = 0;
		}
		spriteColor.a = static_cast<sf::Uint8>(newAlpha);
		this->characterSprite.setColor(spriteColor);
	}
	return (spriteColor.a == 0);
}