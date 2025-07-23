#include "AttackArea.h"

AttackArea::AttackArea(float r, sf::Color color, sf::RenderWindow& window, std::vector<AttackArea*>& circles):circle(r), r(r){
	sf::Vector2u windowSize = window.getSize();
	circle.setFillColor(color);
	circle.setOrigin(r, r);
	setPosition(getCenterPos(window));
	orgColor = color;
	maxRadius = std::sqrt(windowSize.x * windowSize.x + windowSize.y * windowSize.y) * 0.4f;
	circles.push_back(this);
}

void AttackArea::setColor(sf::Color newColor){
	circle.setFillColor(newColor);
}

void AttackArea::showSprite(sf::RenderWindow& window, SPManager& spm){
	window.draw(circle);
}

void AttackArea::gainScore(){
	std::cout << "スコアを得る" << std::endl;
}

void AttackArea::resetCheckContact(){
	isContact = false;
}

//原点を中心に変更しているのでこれでいい
sf::Vector2f AttackArea::getCenterPos(sf::RenderWindow& window){
	sf::Vector2u windowSize = window.getSize();
    float x = windowSize.x / 2.0f;
    float y = windowSize.y / 2.0f;
	return sf::Vector2f(x,y);
}

void AttackArea::setPosition(sf::Vector2f pos){
	circle.setPosition(pos);
}

sf::Vector2f AttackArea::getPosition(){
	return circle.getPosition();
}

//最も近い矩形辺の座標を取得
float AttackArea::calcDistance(Enemy& e){
	sf::Vector2f circlePos = this->getPosition();
	sf::FloatRect enemyBounds = e.getCharacterSprite().getGlobalBounds();
	float closest_X = std::clamp(circlePos.x, enemyBounds.left, enemyBounds.left + enemyBounds.width);
	float closest_Y = std::clamp(circlePos.y, enemyBounds.top, enemyBounds.top + enemyBounds.height);
	float dx = closest_X - circlePos.x;
	float dy = closest_Y- circlePos.y;
	return dx*dx + dy*dy;
}