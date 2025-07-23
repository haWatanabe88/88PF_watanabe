#include "SmallAttackArea.h"

SmallAttackArea::SmallAttackArea(float r, sf::Color color, sf::RenderWindow& window, std::vector<AttackArea*>& circles)
		:AttackArea(r, color, window, circles){
			org_r = r;
		};

void gainComboCounter(){
	std::cout << "コンボ獲得：小" << std::endl;
}

void gainSPgauge(){
	std::cout << "SPゲージ獲得：小" << std::endl;
}

void SmallAttackArea::checkContact(Enemy& e){
	if(this->calcDistance(e) <= this->getRadius() * this->getRadius())
	{
		e.areaType = Enemy::AreaType::Small;
	}
}

void SmallAttackArea::upscaleAnim(sf::RenderWindow& window){
	if(r < maxRadius){
		r+=0.1f;
		circle.setRadius(r);
		circle.setOrigin(r, r);
		circle.setPosition(getCenterPos(window));
		if(r > maxRadius){
			r = maxRadius;
		}
	}
}

void SmallAttackArea::downscaleAnim(sf::RenderWindow& window, SPManager& spm){
	if(org_r < r){
		r-=0.1f;
		circle.setRadius(r);
		circle.setOrigin(r, r);
		circle.setPosition(getCenterPos(window));
		if(org_r > r){
			r = org_r;
			spm.setCurrentState(SPManager::SPState::Idle);
		}
	}
}

void SmallAttackArea::showSprite(sf::RenderWindow& window, SPManager& spm){
	if(spm.getCurrentState() == SPManager::SPState::BlinkEnd){
		upscaleAnim(window);
	}
	if(spm.getCurrentState() == SPManager::SPState::Finishing){
		downscaleAnim(window, spm);
	}
	window.draw(circle);
}