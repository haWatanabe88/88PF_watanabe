#include "BigAttackArea.h"

BigAttackArea::BigAttackArea(float r, sf::Color color, sf::RenderWindow& window, std::vector<AttackArea*>& circles)
	:AttackArea(r, color, window, circles){};

void BigAttackArea::resetComboCounter(){
	std::cout << "コンボカウンターをリセット" << std::endl;
}


void BigAttackArea::checkContact(Enemy& e){
	if(!(e.areaType == Enemy::AreaType::Small))
	{
		if(this->calcDistance(e) <= this->getRadius() * this->getRadius())
		{
			e.areaType = Enemy::AreaType::Big;
		}
	}
}