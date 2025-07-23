#pragma once
#include "AttackArea.h"
#include <iostream>

class BigAttackArea : public AttackArea{
	public:
		BigAttackArea(float, sf::Color, sf::RenderWindow&, std::vector<AttackArea*>&);
		void resetComboCounter();

		void checkContact(Enemy& e) override;
};