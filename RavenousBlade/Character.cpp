#include "Character.h"
#include <iostream>

Character::Direction Character::getDirection(){
	return direction;
}

void Character::setDirection(Direction newDir){
	direction = newDir;
}

void Character::showSprite(sf::RenderWindow& window){
	window.draw(characterSprite);
}

void Character::setScale(float value){
	characterSprite.scale(value, value);
}

bool Character::getIsDead(){
	return isDead;
}

void  Character::setIsDead(bool value){
	isDead = value;
}

sf::Vector2f Character::getCenterPos(sf::RenderWindow& window){
	sf::Vector2u windowSize = window.getSize();
    float x = (windowSize.x) / 2.0f;
    float y = (windowSize.y) / 2.0f;
	return sf::Vector2f(x,y);
}

sf::Vector2f Character::getPosition(){
	return characterSprite.getPosition();
}