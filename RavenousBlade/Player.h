#pragma once
#include <SFML/Graphics.hpp>
#include <SFML/Audio.hpp>
#include <iostream>
#include "Character.h"
#include "Enemy.h"
#include "SPManager.h"
#include "TimerUtils.h"


class PlayerManager;
class GameContext;

class Player : public Character{
	public:
		Player(sf::RenderWindow& window, PlayerManager& pm, SPManager& spm){
			loadAllPlayerTexture();
			direction = Direction::down;
			setTexture(pm, spm);
			characterSprite.setOrigin(
					characterSprite.getGlobalBounds().width /2.0f,
					characterSprite.getGlobalBounds().height /2.0f
			);
			setScale(0.1f);
			setPosinWindowCenter(window);
			halfWidth = characterSprite.getGlobalBounds().width / 2.0f;
			halfHight = characterSprite.getGlobalBounds().height / 2.0f;
			if (!attackBuffer.loadFromFile("./Sound/slushSoundSE.wav")) {
				std::cerr << "Failed to load slushSoundSE.wav" << std::endl;
			} else {
				attackSound.setBuffer(attackBuffer);
			}
			if (!criticalAttackBuffer.loadFromFile("./Sound/CriticalSlushSoundSE.wav")) {
				std::cerr << "Failed to load CriticalSlushSoundSE.wav" << std::endl;
			} else {
				criticalAttackSound.setBuffer(criticalAttackBuffer);
			}
		};
		float getDistance();
		void setDistance(float);
		float calcDistance(Enemy&);
		bool isMinimumDistance(float);
		void resetDistance();
		bool loadAllPlayerTexture();
		void setPosinWindowCenter(sf::RenderWindow& window);
		void setTexture(PlayerManager&, SPManager&);
		void setTarget(Enemy*);
		Enemy* getNowTarget();
		void setNowTarget(Enemy*);
		void attack(std::vector<Enemy*>&, SPManager&);
		void rockOnOutLine(sf::RectangleShape& outline);
		void defeated(GameContext&, Enemy&);
		void blinkAnim(Direction, SPManager&);
		bool enemyIsHitted(Enemy&);
		void reset(GameContext&);
	private:
		float distance = std::numeric_limits<float>::max();
		float halfWidth;
		float halfHight;
		Enemy* nowTarget = nullptr;//プレイヤーの攻撃対象
		sf::Texture texture_front_sp;
		sf::Texture texture_left_sp;
		sf::Texture texture_right_sp;
		sf::Texture texture_back_sp;
		sf::Texture texture_front_attack_sp;
		sf::Texture texture_left_attack_sp;
		sf::Texture texture_right_attack_sp;
		sf::Texture texture_back_attack_sp;
		sf::Texture texture_front_eyeClose;
		sf::Texture texture_left_eyeClose;
		sf::Texture texture_right_eyeClose;
		sf::Texture texture_back_eyeClose;
		sf::SoundBuffer attackBuffer;
		sf::Sound attackSound;
		sf::SoundBuffer criticalAttackBuffer;
		sf::Sound criticalAttackSound;
};