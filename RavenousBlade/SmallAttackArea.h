#pragma once
#include "AttackArea.h"
#include <iostream>
#include "SPManager.h"

class SmallAttackArea : public AttackArea{
	private:
		float org_r;
	public:
		SmallAttackArea(float, sf::Color, sf::RenderWindow&, std::vector<AttackArea*>&);
		void gainComboCounter();
		void gainSPgauge();
		void checkContact(Enemy& e) override;
		void upscaleAnim(sf::RenderWindow&);
		void downscaleAnim(sf::RenderWindow&, SPManager&);
		void showSprite(sf::RenderWindow&, SPManager&) override;
};