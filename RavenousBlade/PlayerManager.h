#pragma once
#include <SFML/Graphics.hpp>
#include "Player.h"
#include <string>
#include <iostream>
#include "SPGauge.h"
#include "SPManager.h"
#include "BlackOut.h"

class GameContext;

class PlayerManager{
	private:
		sf::Clock keyCoolDownTime;
		sf::Clock tmp;
		float coolDownTime = 0.3f;//攻撃間隔を調整
		float attackSpriteTime = 0.25f;
		bool pressed_E = false;
	public:
		void InputDirection(Player&);
		void InputAction(Player& p, std::vector<Enemy*>& chars, SPManager& spm, BlackOut& blackOut);
		bool getPressed_E();
		void setPressed_E(bool rlt);
		void resetPressedE();
		void updateCoolTime(SPManager&);
		void handlePlayerInput(GameContext&);
};